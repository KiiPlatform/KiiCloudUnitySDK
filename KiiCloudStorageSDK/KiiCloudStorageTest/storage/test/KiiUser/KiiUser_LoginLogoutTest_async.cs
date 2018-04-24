using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiUser_LoginLogoutTest_async
    {
        // Use blocking mock as it is login test.
        private MockHttpClient mockClient;

        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);

            MockHttpClientFactory mockFactory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = mockFactory;

            mockClient = mockFactory.Client;
        }

        [TearDown()]
        public void TearDown()
        {
            KiiUser.LogOut();
        }

        private void setStandardResponse()
        {
            mockClient.AddResponse(200, "{" +
                "\"id\" : \"abcd\"," +
                "\"access_token\" : \"cdef\"," +
                "\"expires_in\" : 9223372036854775}");
        }

         #region KiiUser.LogIn(string, string)
        [Test(), KiiUTInfo(
            action = "When We call LogIn() with username/password and Server returns KiiUser,",
            expected = "We can get ID that server sends"
            )]
        public void Test_0000_LogIn_username_OK()
        {
            this.setStandardResponse();

            Exception exception = null;
            KiiUser target = null;
            KiiUser.LogIn("kii1234", "pass1234", (KiiUser user, Exception e)=> {
                target = user;
                exception = e;
            });
            Assert.AreEqual("abcd", target.ID);
            Assert.IsNull(exception);
        }

        [Test(), KiiUTInfo(
            action = "When We call LogIn() with null username,",
            expected = "ArgumentException must be passed in the callback"
            )]
        public void Test_0001_LogIn_username_null_identifier()
        {
            this.setStandardResponse();
            KiiUser target = null;
            Exception exception = null;
            KiiUser.LogIn(null, "pass1234", (KiiUser user, Exception e) => {
                target = user;
                exception = e;
            });
            Assert.IsNull(target);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }

        [Test(), KiiUTInfo(
            action = "When We call LogIn() with null password,",
            expected = "ArgumentException must be passed in the callback"
            )]
        public void Test_0002_LogIn_username_null_password()
        {
            this.setStandardResponse();
            KiiUser target = null;
            Exception exception = null;
            KiiUser.LogIn("kii1234", null, (KiiUser user, Exception e) => {
                target = user;
                exception = e;
            });
            Assert.IsNull(target);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }

        [Test(), KiiUTInfo(
            action = "When We call LogIn() 2 times and Server returns KiiUser for both call,",
            expected = "We can get UserID that server sends"
            )]
        public void Test_0003_LogIn_2times()
        {
            this.setStandardResponse();
            KiiUser target = null;
            Exception exp = null;
            KiiUser.LogIn("kii1234", "pass1234", (KiiUser user, Exception e)=> {
                target = user;
                exp = e;
            });
            Assert.AreEqual("abcd", target.ID);
            Assert.IsNull(exp);

            // set response
            this.mockClient.AddResponse(200, "{" +
                               "\"id\" : \"efgh\"," +
                               "\"access_token\" : \"cdef\"," +
                               "\"expires_in\" : 9223372036854775}");
            KiiUser.LogIn("kii1234", "pass1234", (KiiUser user, Exception e)=> {
                target = user;
                exp = e;
            });
            Assert.AreEqual("efgh", target.ID);
            Assert.IsNull(exp);
        }

        [Test(), KiiUTInfo(
            action = "When We call LogIn() and Server returns HTTP 400 for refresh API call,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0004_LogIn_refresh_fail()
        {
            this.setStandardResponse();
            this.mockClient.AddResponse(new CloudException(400, "{}"));

            KiiUser target = null;
            Exception exp = null;
            KiiUser.LogIn("kii1234", "pass1234", (KiiUser user, Exception e)=> {
                target = user;
                exp = e;
            });
            Assert.IsNotNull(target);
            Assert.IsNotNull(exp);
            Assert.IsInstanceOfType(typeof(CloudException), exp);
        }
        #endregion

        #region KiiUser.LogOut()
        [Test(), KiiUTInfo(
            action = "When We call LogOut(), ",
            expected = "KiiUser.CurrentUser must be null"
            )]
        public void Test_0100_LogOut_OK()
        {

            this.setStandardResponse();

            KiiUser target = null;
            Exception exp = null;
            KiiUser.LogIn("kii1234", "pass1234", (KiiUser user, Exception e)=> {
                target = user;
                exp = e;
            });
            Assert.IsNotNull(target);
            Assert.AreEqual("abcd", target.ID);
            Assert.IsNull(exp);

            // check User
            KiiUser currentUser = KiiUser.CurrentUser;
            Assert.IsNotNull(currentUser);
            Assert.AreEqual("abcd", currentUser.ID);

            // LogOut
            KiiUser.LogOut();

            // check User
            currentUser = KiiUser.CurrentUser;
            Assert.IsNull(currentUser);
        }
        #endregion

        #region KiiUser.LogInWithLocalPhone(string, string, string)
        [Test(), KiiUTInfo(
            action = "When We call LogInWithLocalPhone(), and Server returns KiiUser,",
            expected = "We can get UserID that Server sends"
            )]
        public void Test_0200_LogInWithLocalPhone_OK()
        {
            this.setStandardResponse();
            // LogIn
            KiiUser target = null;
            Exception exp = null;
            KiiUser.LogInWithLocalPhone("09011112222", "pass1111", "JP", (KiiUser user, Exception e) => {
                target = user;
                exp = e;
            });
            Assert.AreEqual("abcd", target.ID);
            Assert.IsNull(exp);
        }

        [Test(), KiiUTInfo(
            action = "When We call LogInWithLocalPhone() with null phone number",
            expected = "ArgumentException must be passed in the callback"
            )]
        public void Test_0201_LogInWithLocalPhone_phone_null()
        {
            this.setStandardResponse();
            KiiUser target = null;
            Exception exp = null;
            KiiUser.LogInWithLocalPhone(null, "pass1111", "JP", (KiiUser user, Exception e) => {
                target = user;
                exp = e;
            });
            Assert.IsNull(target);
            Assert.IsInstanceOfType(typeof(ArgumentException), exp);
        }

        [Test(), KiiUTInfo(
            action = "When We call LogInWithLocalPhone() with null password,",
            expected = "ArgumentException must be passed in the callback"
            )]
        public void Test_0202_LogInWithLocalPhone_pass_null()
        {
            this.setStandardResponse();

            KiiUser target = null;
            Exception exp = null;
            KiiUser.LogInWithLocalPhone("09011112222", null, "JP", (KiiUser user, Exception e) => {
                target = user;
                exp = e;
            });
            Assert.IsNull(target);
            Assert.IsInstanceOfType(typeof(ArgumentException), exp);
        }

        [Test(), KiiUTInfo(
            action = "When We call LogInWithLocalPhone() with null country,",
            expected = "ArgumentException must be passed in the callback"
            )]
        public void Test_0203_LogInWithLocalPhone_country_null()
        {
            this.setStandardResponse();
            KiiUser target = null;
            Exception exp = null;
            KiiUser.LogInWithLocalPhone("09011112222", "pass1111", null, (KiiUser user, Exception e)=> {
                target = user;
                exp = e;
            });
            Assert.IsNull(target);
            Assert.IsInstanceOfType(typeof(ArgumentException), exp);
        }
        #endregion

        #region KiiUser.LoginWithToken(string)
        [Test(), KiiUTInfo(
            action = "When We call LoginWithToken() and Server returns KiiUser,",
            expected = "We can get UserID that server sends"
            )]
        public void Test_0300_LoginWithToken_OK()
        {
            // set response
            this.mockClient.AddResponse(200, "{\"userID\":\"cdef\"}");

            KiiUser target = null;
            Exception exp = null;
            // LogIn
            KiiUser.LoginWithToken("tokenABCD", (KiiUser user, Exception e) => {
                target = user;
                exp = e;
            });
            Assert.AreEqual("cdef", target.ID);
            Assert.IsNull(exp);
            // check AccessTokenDictionary
            Dictionary<string, object> dict = target.GetAccessTokenDictionary();
            string token = (string)dict["access_token"];
            Assert.AreEqual("tokenABCD", token);
            DateTime expiresAt = (DateTime)dict["expires_at"];
            Assert.AreEqual(DateTime.MaxValue, expiresAt);
        }

        [Test(), KiiUTInfo(
            action = "When We call LoginWithToken() with null,",
            expected = "ArgumentException must be passed in the callback"
            )]
        public void Test_0301_LoginWithToken_null()
        {
            this.setStandardResponse();

            KiiUser target = null;
            Exception exp = null;
            KiiUser.LoginWithToken(null, (KiiUser user, Exception e) => {
                target = user;
                exp = e;
            });
            Assert.IsNull(target);
            Assert.IsInstanceOfType(typeof(ArgumentException), exp);
        }

        [Test(), KiiUTInfo(
            action = "When We call LoginWithToken() and Server returns HTTP 400,",
            expected = "CloudException must be passed in the callback"
            )]
        public void Test_0302_LoginWithToken_failed()
        {
            this.mockClient.AddResponse(new CloudException(400, "{}"));
            
            KiiUser target = null;
            Exception exp = null;
            KiiUser.LoginWithToken("tokenABCD", (KiiUser user, Exception e) => {
                target = user;
                exp = e;
            });
            Assert.IsNull (target);
            Assert.IsInstanceOfType(typeof(CloudException), exp);
        }

        #endregion
        
        #region VerifyPhone(string)
        [Test(), KiiUTInfo(
            action = "When We call VerifyPhone() and Server returns HTTP 204(OK),",
            expected = "PhoneVerified must be true"
            )]
        public void Test_0400_VerifyPhone_OK()
        {
            this.setStandardResponse();

            // LogIn
            KiiUser target = null;
            Exception exp = null;
            KiiUser.LogIn("kii1234", "pass1234", (KiiUser user, Exception e) => {
                target = user;
                exp = e;
            });
            Assert.AreEqual("abcd", target.ID);
            Assert.IsNull(exp);
            Assert.IsNotNull(KiiUser.CurrentUser);
            Assert.AreEqual(false, target.PhoneVerified);

            this.mockClient.AddResponse(204, "");
            // verification
            target.VerifyPhone("1234", (KiiUser user, Exception e) => {
                target = user;
                exp =e;
            });
            Assert.IsNull(exp);
            Assert.IsTrue(target.PhoneVerified);
        }

        [Test(), KiiUTInfo(
            action = "When We call VerifyPhone() with null,",
            expected = "ArgumentException must be passed in the callback"
            )]
        public void Test_0401_VerifyPhone_null()
        {
            // set response
            this.setStandardResponse();
            
            // LogIn
            KiiUser target = null;
            Exception exp = null;
            KiiUser.LogIn("kii1234", "pass1234", (KiiUser user, Exception e) => {
                target = user;
                exp = e;
            });
            Assert.AreEqual("abcd", target.ID);
            Assert.IsNull(exp);
            Assert.IsNotNull(KiiUser.CurrentUser);
            Assert.AreEqual(false, target.PhoneVerified);
            
            // set response
            this.mockClient.AddResponse(204, "");
            
            // verification
            target.VerifyPhone(null, (KiiUser user, Exception e) => {
                target = user;
                exp =e;
            });
            Assert.IsNull(target);
            Assert.IsInstanceOfType(typeof(ArgumentException), exp);
        }

        [Test(), KiiUTInfo(
            action = "When We call VerifyPhone() and Server returns HTTP 400,",
            expected = "CloudException must be passed in the callback"
            )]
        public void Test_0402_VerifyPhone_server_error()
        {
            this.setStandardResponse();
            
            // LogIn
            // LogIn
            KiiUser target = null;
            Exception exp = null;
            KiiUser.LogIn("kii1234", "pass1234", (KiiUser user, Exception e) => {
                target = user;
                exp = e;
            });
            Assert.AreEqual("abcd", target.ID);
            Assert.IsNull(exp);
            Assert.IsNotNull(KiiUser.CurrentUser);
            Assert.AreEqual(false, target.PhoneVerified);
            
            // set response
            this.mockClient.AddResponse(new CloudException(400, "{}"));
            
            // verification
            target.VerifyPhone("1234", (KiiUser user, Exception e) => {
                target = user;
                exp =e;
            });
            Assert.IsNull(target);
            Assert.IsInstanceOfType(typeof(CloudException), exp);
        }
        #endregion
        
        #region KiiUser.ResetPassword(string)
        
        [Test(), KiiUTInfo(
            action = "When We call ResetPassword() with email and Server returns HTTP 204(OK),",
            expected = "No Exception must not passed"
            )]
        public void Test_0500_ResetPassword_email()
        {
            // set response
            this.mockClient.AddResponse(204, "");

            Exception exp = null;
            // reset password
            KiiUser.ResetPassword("test@kii.com", (Exception e) => {
                exp = e;
            });
            Assert.IsNull(exp);
        }
 
        [Test(), KiiUTInfo(
            action = "When We call ResetPassword() with phone,",
            expected = "ArgumentException must be passed in callback"
            )]
        public void Test_0501_ResetPassword_phone()
        {
            this.mockClient.AddResponse(204, "");
            // reset password
            Exception exp = null;
            KiiUser.ResetPassword("+819011112222", (Exception e) => {
                exp = e;
            });
            Assert.IsInstanceOfType(typeof(ArgumentException), exp);
        }

        [Test(), KiiUTInfo(
            action = "When We call ResetPassword() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0502_ResetPassword_null()
        {
            this.mockClient.AddResponse(204, "");

            Exception exp = null;
            // reset password
            KiiUser.ResetPassword(null, (Exception e) => {
                exp = e;
            });
            Assert.IsInstanceOfType(typeof(ArgumentException), exp);
        }

        [Test(), KiiUTInfo(
            action = "When We call ResetPassword() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0503_ResetPassword_server_error()
        {
            this.mockClient.AddResponse(new CloudException(400, "{}"));
            Exception exp = null;
            // reset password
            KiiUser.ResetPassword("test@kii.com", (Exception e) => {
                exp =e;
            });
            Assert.IsInstanceOfType(typeof(CloudException), exp);
        }
        #endregion

    }
}

