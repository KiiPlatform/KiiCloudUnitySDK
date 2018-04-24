# Parse input from json file (default:largetests.js) and run the tests. 
#
# To run all the tests input json is:
# => {"assembly":"your test assembly path from project root", "count":1}
#
# To run a single tests json is:
# => {"assembly":"your test assembly path from project root", "name":"KiiCorp.Cloud.Storage.test_name", "count":1}  

import subprocess
import json
import time as time_
import argparse
import os
import sys

parser = argparse.ArgumentParser()
parser.add_argument('--file', nargs='?', default='largetests.js', help='Test case definition file')
args = parser.parse_args()

tests = json.load(open(args.file))

monoOptions = os.environ.get('MONO_OPTIONS', "")
workingDir = os.getcwd()
print("Working directory: " + workingDir)

if not os.path.isdir("test-results"):
    os.mkdir("test-results")

for t in tests:
    assembly = t["assembly"]
    name = ""
    runArg = ""
    if "name" in t:
        name = t["name"]
        runArg = "-run"
    count = t["count"]
    for i in range(count):
        processArg = ["mono", "--debug", monoOptions, "NUnit-2.6.3/bin/nunit-console.exe", assembly, runArg, name]
        processArg = filter(bool, processArg)
        print("Command: " + " ".join(processArg))
        subprocess.call(processArg, cwd=(workingDir))
        report_file = "test-results/TestResult-" + str(int(round(time_.time() * 1000))) + ".xml"
        print("Report file: " + report_file)
        subprocess.call(["mv", "TestResult.xml", report_file], cwd=(workingDir))

