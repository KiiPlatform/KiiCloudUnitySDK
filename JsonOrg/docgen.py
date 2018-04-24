#!/usr/bin/env python
# -*- coding: utf-8 -*-

import re

import sys
import subprocess

def extract_libraries(target, regex):

    # If python > 2.6, we can use context statement(with)
    _file = open(target)
    matchers = [re.search(regex, line) for line in _file]
    libraries = [m.group(1).replace('\\', '/') for m in matchers if m]
    _file.close()
    return libraries

def main():

    if len(sys.argv) != 2:
        print 'error'
        sys.exit(1)

    target = sys.argv[1]
    regex = r'<Compile Include="(.*\.cs)" />'
    cmd = 'mcs /target:library /doc:doc.xml '

    libraries = extract_libraries(target, regex)
    print cmd + ' '.join(libraries)

if __name__ == '__main__':
    main()
