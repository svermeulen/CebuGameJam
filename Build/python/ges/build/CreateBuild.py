
import sys
import os
import re

import argparse

from ges.log.LogStreamFile import LogStreamFile
from ges.log.LogStreamConsole import LogStreamConsole

from ges.util.ZipHelper import ZipHelper

from ges.util.ScriptRunner import ScriptRunner

from ges.util.ProcessRunner import ProcessRunner
from ges.util.SystemHelper import SystemHelper
from ges.util.VarManager import VarManager
from ges.config.Config import Config
from ges.log.Logger import Logger
from ges.util.VisualStudioHelper import VisualStudioHelper
from ges.util.UnityHelper import UnityHelper, Platforms

from ges.util.Assert import *

import ges.ioc.Container as Container
from ges.ioc.Inject import Inject

ScriptDir = os.path.dirname(os.path.realpath(__file__))
RootDir = os.path.realpath(os.path.join(ScriptDir, '../../../..'))

class Runner:
    _scriptRunner = Inject('ScriptRunner')
    _unityHelper = Inject('UnityHelper')
    _log = Inject('Logger')
    _sys = Inject('SystemHelper')
    _varManager = Inject('VarManager')
    _zipHelper = Inject('ZipHelper')

    def __init__(self):
        self._platform = Platforms.Windows

    def run(self, args):
        self._args = args
        success = self._scriptRunner.runWrapper(self._runInternal)

        if not success:
            sys.exit(1)

    def _runBuilds(self):

        if self._args.clearOutput:
            self._log.heading("Clearing output directory")
            self._sys.clearDirectoryContents('[OutputRootDir]')

        self._clearTempDirectory()
        self._copyToTempDirectory()

        if self._args.buildType == 'all' or self._args.buildType == 'webgl':
            self._log.heading("Building WebGl")
            self._platform = Platforms.WebGl
            self._createBuild()
            self._sys.copyFile('[WebGlTemplate]', '[OutputRootDir]/WebGl/Web.config')
            self._zipHelper.createZipFile('[OutputRootDir]/WebGl', '[OutputRootDir]/WebGlZip/GreatEggscapeWebGl.zip')

        if self._args.buildType == 'all' or self._args.buildType == 'pc':
            self._log.heading("Building windows")
            self._platform = Platforms.Windows
            self._createBuild()

    def _clearTempDirectory(self):
        self._sys.deleteDirectoryIfExists('[TempDir]')
        self._sys.createDirectory('[TempDir]')
        self._sys.clearDirectoryContents('[TempDir]')

    def _copyToTempDirectory(self):
        self._log.info('Copying to temporary directory')
        try:
            self._sys.copyDirectory('[UnityProjectPath]', '[UnityProjectPathTempDir]')
        except:
            pass

    def _runInternal(self):

        if self._args.runBuilds:
            self._runBuilds()

        if self._args.openUnity:
            self._openUnity()

    def _createBuild(self):
        self._log.info("Creating build")
        self._runEditorFunction('BuildRelease')
        #self._runEditorFunction('BuildDebug')

    def _openUnity(self):
        self._unityHelper.openUnity('[UnityProjectPath]', self._platform)

    def _runEditorFunction(self, functionName):
        self._log.info("Calling Builder." + functionName)
        self._unityHelper.runEditorFunction('[UnityProjectPathTempDir]', 'Builder.' + functionName, self._platform)

def installBindings():

    config = {
        'PathVars': {
            'ScriptDir': ScriptDir,
            'RootDir': RootDir,
            'TempDir': '[RootDir]/Temp',
            'WebGlTemplate': '[ScriptDir]/web_config_template.xml',
            'OutputRootDir': '[RootDir]/Builds',
            'UnityExePath': 'D:/Utils/Unity/Installs/2018.2.2f1/Editor/Unity.exe',
            'LogPath': '[RootDir]/Log.txt',
            'UnityProjectPath': '[RootDir]/UnityProject',
            'UnityProjectPathTempDir': '[TempDir]/UnityProject',
            'MsBuildExePath': 'C:/Windows/Microsoft.NET/Framework/v4.0.30319/msbuild.exe'
        },
        'Compilation': {
            'UseDevenv': False
        },
    }
    Container.bind('Config').toSingle(Config, [config])

    Container.bind('LogStream').toSingle(LogStreamFile)
    Container.bind('LogStream').toSingle(LogStreamConsole, True, False)

    Container.bind('ScriptRunner').toSingle(ScriptRunner)
    Container.bind('VarManager').toSingle(VarManager)
    Container.bind('SystemHelper').toSingle(SystemHelper)
    Container.bind('Logger').toSingle(Logger)
    Container.bind('ProcessRunner').toSingle(ProcessRunner)
    Container.bind('ZipHelper').toSingle(ZipHelper)
    Container.bind('VisualStudioHelper').toSingle(VisualStudioHelper)
    Container.bind('UnityHelper').toSingle(UnityHelper)

if __name__ == '__main__':

    if (sys.version_info < (3, 0)):
        print('Wrong version of python!  Install python 3 and try again')
        sys.exit(2)

    parser = argparse.ArgumentParser(description='Create Sample')
    parser.add_argument('-ou', '--openUnity', action='store_true', help='')
    parser.add_argument('-c', '--clearOutput', action='store_true', help='')
    parser.add_argument('-rb', '--runBuilds', action='store_true', help='')
    parser.add_argument('-t', '--buildType', type=str, default='webgl', choices=['all', 'webgl', 'pc'], help='')

    args = parser.parse_args(sys.argv[1:])

    installBindings()

    Runner().run(args)


