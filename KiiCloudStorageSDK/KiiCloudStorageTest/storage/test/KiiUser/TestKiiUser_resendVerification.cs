using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiUser_resendVerification
    {
        private MockHttpClient client;

        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

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
            KiiUser.LogIn("kii1234", "pass1234");
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
            KiiUser.RequestResendEmailVerification();
        }

        [Test(),ExpectedException(typeof(InvalidOperationException)),
         KiiUTInfo(
            action = "Request resend verification without Login",
            expected = "InvalidOperationException thrown."
            )]
        public void Test_0001_resendEmailVerification_logout ()
        {
            this.client.AddResponse(204, "");
            KiiUser.RequestResendEmailVerification();
        }

        [Test(),ExpectedException(typeof(CloudException)),
         KiiUTInfo(
            action = "Request resend verification and server response 404",
            expected = "CloudException exception thrown."
            )]
        public void Test_0002_resendEmailVerification_notFound ()
        {
            this.LogIn();
            this.client.AddResponse(new CloudException(404, "{}"));
            KiiUser.RequestResendEmailVerification();
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
            KiiUser.RequestResendPhoneVerificationCode();
        }

        [Test(),ExpectedException(typeof(InvalidOperationException)),
         KiiUTInfo(
            action = "Request resend verification without Login",
            expected = "InvalidOperationException thrown."
            )]
        public void Test_1001_resendPhoneVerification_logout ()
        {
            this.client.AddResponse(204, "");
            KiiUser.RequestResendPhoneVerificationCode();
        }

        [Test(),ExpectedException(typeof(CloudException)),
         KiiUTInfo(
            action = "Request resend verification and server response 404",
            expected = "CloudException exception thrown."
            )]
        public void Test_1002_resendPhoneVerification_notFound ()
        {
            this.LogIn();
            this.client.AddResponse(new CloudException(404, "{}"));
            KiiUser.RequestResendPhoneVerificationCode();
        }
        #endregion
    }
}

