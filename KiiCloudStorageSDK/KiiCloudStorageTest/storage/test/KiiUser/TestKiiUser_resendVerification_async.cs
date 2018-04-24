using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiUser_resendVerification_async
    {
        private MockHttpClient client;

        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;

            client = (MockHttpClient)factory.Client;
        }

        [TearDown()]
        public void TearDown()
        {
            KiiUser.LogOut();
        }

        private void LogIn()
        {
            // set Response
            client.AddResponse(200, "{" +
                               "\"id\" : \"user1234\"," +
                               "\"access_token\" : \"cdef\"," +
                               "\"expires_in\" : 9223372036854775}");
            KiiUser loginUser = null;
            Exception exp = null;
            KiiUser.LogIn("kii1234", "pass1234", (KiiUser u, Exception e) => {
                loginUser  = u;
                exp = e;
            });
            Assert.IsNotNull(loginUser);
            Assert.AreEqual("user1234", loginUser.ID);
            Assert.IsNull (exp);
        }

        #region RequestResendEmailVerification
        [Test(), KiiUTInfo(
            action = "Request resend verification and server response OK.",
            expected = "No exception thrown."
            )]
        public void Test_0000_resendEmailVerification ()
        {
            this.LogIn();

            this.client.AddResponse(204, "");
            Exception exp = null;
            KiiUser.RequestResendEmailVerification((Exception e) => {
                exp = e;
            });
            Assert.IsNull(exp);
        }

        [Test(),
         KiiUTInfo(
            action = "Request resend verification without Login",
            expected = "InvalidOperationException thrown."
            )]
        public void Test_0001_resendEmailVerification_logout ()
        {
            this.client.AddResponse(204, "");
            Exception exp = null;
            KiiUser.RequestResendEmailVerification((Exception e) => {
                exp = e;
            });
            Assert.IsInstanceOfType(typeof(InvalidOperationException), exp);
        }

        [Test(),
         KiiUTInfo(
            action = "Request resend verification and server response 404",
            expected = "CloudException exception thrown."
            )]
        public void Test_0002_resendEmailVerification_notFound ()
        {
            this.LogIn();
            this.client.AddResponse(new CloudException(404, "{}"));
            this.client.AddResponse(204, "");
            KiiUser.RequestResendEmailVerification((Exception e) => {
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(CloudException), e);
            });
        }
        #endregion

        #region RequestResendPhoneVerificationCode
        [Test(), KiiUTInfo(
            action = "Request resend verification and server response OK.",
            expected = "No exception thrown."
            )]
        public void Test_1000_resendPhoneVerification ()
        {
            this.LogIn();
            
            this.client.AddResponse(204, "");
            Exception exp = null;
            KiiUser.RequestResendPhoneVerificationCode((Exception e) => {
                exp = e;
            });
            Assert.IsNull (exp);
        }

        [Test(),
         KiiUTInfo(
            action = "Request resend verification without Login",
            expected = "InvalidOperationException thrown."
            )]
        public void Test_1001_resendPhoneVerification_logout ()
        {
            this.client.AddResponse(204, "");
            KiiUser.RequestResendPhoneVerificationCode((Exception e) => {
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(InvalidOperationException), e);
            });
        }

        [Test(),
         KiiUTInfo(
            action = "Request resend verification and server response 404",
            expected = "CloudException exception thrown."
            )]
        public void Test_1002_resendPhoneVerification_notFound ()
        {
            this.LogIn();
            this.client.AddResponse(new CloudException(404, "{}"));
            this.client.AddResponse(204, "");
            Exception exp = null;
            KiiUser.RequestResendPhoneVerificationCode((Exception e) => {
                exp = e;
            });
            Assert.IsInstanceOfType(typeof(CloudException), exp);
        }
        #endregion
    }
}

