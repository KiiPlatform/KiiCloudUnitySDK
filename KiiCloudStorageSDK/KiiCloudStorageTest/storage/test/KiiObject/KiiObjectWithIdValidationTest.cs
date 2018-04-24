// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Text;
using NUnit.Framework;
using JsonOrg;
using System.Collections;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{


    [TestFixture(), TestSpec(url="https://docs.google.com/a/kii.com/spreadsheet/ccc?key=0AsJL8lP7ZQXGdFFnak9XeWlwSG55MjdxTktFUm5JZWc&usp=drive_web#gid=1")]
    public class KiiObjectWithIdValidationTest
    {
        static string OBJECT_ID_LEN_100 = "abcde1234-abcde5678-abcde9012-abcde1234-"
            + "abcde1234-abcde1234-abcde1234-abcde1234-abcde1234-abcde12345";
        internal class TestCase {
            string objectID;
            bool expPass;
            
            public TestCase(string objectID, bool expectPass) {
                this.objectID = objectID;
                this.expPass = expectPass;
            }
            internal string objID
            {
                get
                {
                    return objectID;
                }
            }
            internal bool expectPass
            {
                get
                {
                    return expPass;
                }
            }
        }

        TestCase[] TEST_CASES = new TestCase[] {
            new TestCase("", false), 
            new TestCase(null, false),
            new TestCase("a", false), 
            new TestCase("1-d_t.5", true),
            new TestCase(OBJECT_ID_LEN_100, true),
            new TestCase(OBJECT_ID_LEN_100 + "a", false),
            new TestCase("abcd 1234", false), // contains whitespace
            new TestCase("abcd\\1234", false), // contains backslash
            new TestCase("!\"*+^~|)('&%$#", false),
            new TestCase("\u3042", false)
           
        };

        [Test(), TestCaseNumber("3-1-1 to 3-1-10")]
        public void Test_Validate_ObjectID() {
            List<string> errors = new List<string>();
            for(int i = 0; i < TEST_CASES.Length; i++) {
                TestCase tc = TEST_CASES[i];
                string inputId = tc.objID;
                bool expectPass = tc.expectPass;

                if(expectPass) {
                    // if invalid, fail.
                    bool isValid = Utils.ValidateObjectID(inputId);
                    if(!isValid)
                        errors.Add("TestCase: "+i+", input: "+inputId+" failed");
                } else {
                    // if true, fail.
                    bool isValid = Utils.ValidateObjectID(inputId);
                    if(isValid)
                        errors.Add("TestCase: "+i+", input: "+inputId+" failed");
                }
            }
            if(errors.Count > 0){
                StringBuilder sb = new StringBuilder("One or More testcase failed. \n");
                foreach(string error in errors){
                    sb.Append(error);
                }
                Assert.Fail(sb.ToString());
            }
        }

        [Test(), TestCaseNumber("1-1-1 to 1-1-10")]
        public void Test_Validate_KiiBucket_NewObjectWithID() {
            List<string> errors = new List<string>();
            for(int i = 0; i < TEST_CASES.Length; i++) {
                TestCase tc = TEST_CASES[i];
                string inputId = tc.objID;
                bool expectPass = tc.expectPass;
                
                if(expectPass) {
                    // if exceptio thrown, fail.
                    try {
                        Kii.Bucket("MyBucket").NewKiiObject(inputId);
                    } catch(Exception e){
                        errors.Add("TestCase: "+i+", input: "+inputId+" failed, error: "+e.Message);
                    }
                } else {
                    // if ArgumentException not thrown, fail.
                    try {
                        Kii.Bucket("MyBucket").NewKiiObject(inputId);
                        errors.Add("TestCase: "+i+", input: "+inputId+" " +
                            "failed, error: exception has not thrown");
                    } catch(ArgumentException e){
                        // pass
                    } catch(Exception e){
                        errors.Add("TestCase: "+i+", input: "+inputId+" failed, error: "+e.Message);
                    }
                }
            }
            if(errors.Count > 0){
                StringBuilder sb = new StringBuilder("One or More testcase failed. \n");
                foreach(string error in errors){
                    sb.Append(error);
                }
                Assert.Fail(sb.ToString());
            }
        }
    }
}
