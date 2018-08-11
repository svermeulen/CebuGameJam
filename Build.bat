@echo off

set PYTHONPATH=%~dp0/Build/python;%PYTHONPATH%
python -m ges.build.CreateBuild %*

