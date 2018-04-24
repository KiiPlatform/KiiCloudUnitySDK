using System;
using NUnit.Framework;
using System.Collections;
using System.Text;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiUser_PasswordResetTest
    {
        [SetUp()]
        public void SetUp ()
        {
            Kii.Initialize ("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory ();
            Kii.HttpClientFactory = factory;

            MockHttpClient client = factory.Client;
            client.AddResponse (200, null);
        }

        internal class TestCase
        {
            string identifier;
            bool expectedResult;

            internal TestCase (string identifier, bool expectedResult)
            {
                this.identifier = identifier;
                this.expectedResult = expectedResult;
            }

            internal bool ExpectedResult {
                get {
                    return expectedResult;
                }
            }

            internal string Identifier {
                get {
                    return identifier;
                }
            }

        }
        internal TestCase[] TESTCASES = {
                            new TestCase (null, false), 
                            new TestCase ("", false),
                            new TestCase ("+818034068125", false),
                            new TestCase ("testUser", false),
                            new TestCase ("'%#=!('%~^¥", false),
                            new TestCase ("moshiur.rahman@kii.com", true)
                        };

        #region ResetPassword(string)
        [Test(), KiiUTInfo(
                            action = "When we reset password  by calling resetPassword(identifier) with TESTCASES params",
                            expected = "We get ArgumentEx"
                            )]
        public void Test_ResetPassword ()
        {
            ArrayList errors = new ArrayList ();
            for (int i = 0; i < TESTCASES.Length; i++) {
                TestCase tc = TESTCASES [i];
                if (tc.ExpectedResult) {
                    try {

                        KiiUser.ResetPassword (tc.Identifier);
                    } catch (Exception e) {
                        errors.Add ("TestCase : " + i + " identifier : "
                            + tc.Identifier + " message :" + e.Message);
                    } 

                } else {
                    try {
                        KiiUser.ResetPassword (tc.Identifier);
                        // error if exception is not thrown.
                        errors.Add ("TestCase : " + i + " identifier : "
                            + tc.Identifier);
                    } catch (ArgumentException) {
                        // passed.
                    } catch (Exception e) {
                        errors.Add ("TestCase : " + i + " identifier : "
                            + tc.Identifier + " message :" + e.Message);
                    }
                }
            }
            if (errors.Count > 0) {
                StringBuilder builder = new StringBuilder (
                                    "One or more case failed. Messages : \n");
                foreach (String msg in errors) {
                    builder.Append (msg + "\n");
                }
                Assert.Fail (builder.ToString ());
            }

        }
        #endregion
    }
}

