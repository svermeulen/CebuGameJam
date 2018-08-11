
import unittest

import ges.ioc.Container as Container
from ges.ioc.Inject import Inject
import ges.ioc.IocAssertions as Assertions

from ges.config.ConfigLoaderHardCoded import ConfigLoaderHardCoded
from ges.config.Config import Config

from ges.util.Assert import *
from ges.util.VarManager import VarManager

class TestVarManager(unittest.TestCase):
    def setUp(self):
        Container.clear()

    def test1(self):
        config = {
            'PathVars': {
                'foo': 'yep [bar]',
                'bar': 'result2',
                'nest1': 'asdf [foo]',
            }
        }
        Container.bind('Config').toSingle(Config, [config])

        Container.bind('VarManager').toSingle(VarManager)

        pathMgr = Container.resolve('VarManager')

        assertThat(pathMgr.hasKey('foo'))
        assertThat(not pathMgr.hasKey('asdf'))
        assertThat(pathMgr.tryGet('bobsdf') == None)
        assertThat(pathMgr.expand('before [bar] after') == 'before result2 after')
        assertThat(pathMgr.expand('before [foo] after') == 'before yep result2 after')

        assertThat(not pathMgr.hasKey('qux'))
        pathMgr.add('qux', 'sadf')
        assertThat(pathMgr.hasKey('qux'))
        assertThat(pathMgr.expand('[qux]') == 'sadf')

        assertThat(pathMgr.expand('[nest1]') == 'asdf yep result2')

        print('Done')

if __name__ == '__main__':
    unittest.main()

