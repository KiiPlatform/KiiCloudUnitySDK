using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiUser_LoginLogoutTest
    {
        private void setStandardResponse(MockHttpClient client)
        {
            client.AddResponse(200, "{" +
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
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            KiiUser user = KiiUser.LogIn("kii1234", "pass1234");
            Assert.AreEqual("abcd", user.ID);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When We call LogIn() with null username/password,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0001_LogIn_username_null_identifier()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            KiiUser.LogIn(null, "pass1234");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When We call LogIn() with username/null password,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0002_LogIn_username_null_password()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            KiiUser.LogIn("kii1234", null);
        }

        [Test(), KiiUTInfo(
            action = "When We call LogIn() 2 times and Server returns KiiUser for both call,",
            expected = "We can get UserID that server sends"
            )]
        public void Test_0003_LogIn_2times()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            KiiUser user = KiiUser.LogIn("kii1234", "pass1234");
            Assert.AreEqual("abcd", user.ID);

            // set response
            client.AddResponse(200, "{" +
                "\"id\" : \"efgh\"," +
                "\"access_token\" : \"cdef\"," +
                "\"expires_in\" : 9223372036854775}");

            user = KiiUser.LogIn("kii2345", "pass2345");
            Assert.AreEqual("efgh", user.ID);
        }

        [Test(), KiiUTInfo(
            action = "When We call LogIn() and Server returns HTTP 400 for refresh API call,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0004_LogIn_refresh_fail()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);
            client.AddResponse(new CloudException(400, "{}"));

            try
            {
                KiiUser.LogIn("kii1234", "pass1234");
                Assert.Fail("");
            }
            catch (CloudException)
            {
                // OK
            }
            catch
            {
                Assert.Fail("Exception must be thrown");
            }
            KiiUser user = KiiUser.CurrentUser;
            Assert.IsNotNull(user);

        }

        #endregion

        #region KiiUser.LogOut()
        [Test(), KiiUTInfo(
            action = "When We call LogOut(), ",
            expected = "KiiUser.CurrentUser must be null"
            )]
        public void Test_0100_LogOut_OK()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            // LogIn
            KiiUser user = KiiUser.LogIn("kii1234", "pass1234");
            Assert.AreEqual("abcd", user.ID);

            // check User
            user = KiiUser.CurrentUser;
            Assert.IsNotNull(user);

            // LogOut
            KiiUser.LogOut();

            // check User
            user = KiiUser.CurrentUser;
            Assert.IsNull(user);
        }

        #endregion

        #region KiiUser.LogInWithLocalPhone(string, string, string)

        [Test(), KiiUTInfo(
            action = "When We call LogInWithLocalPhone(), and Server returns KiiUser,",
            expected = "We can get UserID that Server sends"
            )]
        public void Test_0200_LogInWithLocalPhone_OK()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            // LogIn
            KiiUser user = KiiUser.LogInWithLocalPhone("09011112222", "pass1111", "JP");
            Assert.AreEqual("abcd", user.ID);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When We call LogInWithLocalPhone() with null phone number / password,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0201_LogInWithLocalPhone_phone_null()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            // LogIn
            KiiUser.LogInWithLocalPhone(null, "pass1111", "JP");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When We call LogInWithLocalPhone() with phone number / null password,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0202_LogInWithLocalPhone_pass_null()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            // LogIn
            KiiUser.LogInWithLocalPhone("09011112222", null, "JP");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When We call LogInWithLocalPhone() with phone number / password / null country,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0203_LogInWithLocalPhone_country_null()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            // LogIn
            KiiUser.LogInWithLocalPhone("09011112222", "pass1111", null);
        }
        #endregion

        #region KiiUser.LoginWithToken(string)

        [Test(), KiiUTInfo(
            action = "When We call LoginWithToken() and Server returns KiiUser,",
            expected = "We can get UserID that server sends"
            )]
        public void Test_0300_LoginWithToken_OK()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "{\"userID\":\"cdef\"}");

            // LogIn
            KiiUser user = KiiUser.LoginWithToken("tokenABCD");
            Assert.AreEqual("cdef", user.ID);

            // check AccessTokenDictionary
            Dictionary<string, object> dict = user.GetAccessTokenDictionary();
            string token = (string)dict["access_token"];
            Assert.AreEqual("tokenABCD", token);
            DateTime expiresAt = (DateTime)dict["expires_at"];
            Assert.AreEqual(DateTime.MaxValue, expiresAt);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When We call LoginWithToken() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0301_LoginWithToken_null()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            // LogIn
            KiiUser.LoginWithToken(null);
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When We call LoginWithToken() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0302_LoginWithToken_failed()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new CloudException(400, "{}"));

            // LogIn
            KiiUser.LoginWithToken("tokenABCD");
        }

        #endregion

        #region VerifyPhone(string)

        [Test(), KiiUTInfo(
            action = "When We call VerifyPhone() and Server returns HTTP 204(OK),",
            expected = "PhoneVerified must be true"
            )]
        public void Test_0400_VerifyPhone_OK()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            // LogIn
            KiiUser user = KiiUser.LogIn("kii1234", "pass1234");
            Assert.IsNotNull(KiiUser.CurrentUser);
            Assert.AreEqual(false, user.PhoneVerified);

            // set response
            client.AddResponse(204, "");

            // verification
            user.VerifyPhone("1234");
            Assert.AreEqual(true, user.PhoneVerified);
        }

        [Test(),ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When We call VerifyPhone() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0401_VerifyPhone_null()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            // LogIn
            KiiUser user = KiiUser.LogIn("kii1234", "pass1234");
            Assert.IsNotNull(KiiUser.CurrentUser);
            Assert.AreEqual(false, user.PhoneVerified);

            // set response
            client.AddResponse(204, "");

            // verification
            user.VerifyPhone(null);
        }

        [Test(),ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When We call VerifyPhone() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0402_VerifyPhone_server_error()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            // LogIn
            KiiUser user = KiiUser.LogIn("kii1234", "pass1234");
            Assert.IsNotNull(KiiUser.CurrentUser);
            Assert.AreEqual(false, user.PhoneVerified);

            // set response
            client.AddResponse(new CloudException(400, "{}"));

            // verification
            user.VerifyPhone("123456");
        }

        #endregion

        #region KiiUser.ResetPassword(string)

        [Test(), KiiUTInfo(
            action = "When We call ResetPassword() with email and Server returns HTTP 204(OK),",
            expected = "No Exception must be thrown"
            )]
        public void Test_0500_ResetPassword_email()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(204, "");

            // reset password
            KiiUser.ResetPassword("test@kii.com");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When We call ResetPassword() with phone,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0501_ResetPassword_phone()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(204, "");

            // reset password
            KiiUser.ResetPassword("+819011112222");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When We call ResetPassword() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0502_ResetPassword_null()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(204, "");

            // reset password
            KiiUser.ResetPassword(null);
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When We call ResetPassword() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0503_ResetPassword_server_error()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new CloudException(400, "{}"));

            // reset password
            KiiUser.ResetPassword("test@kii.com");
        }
        #endregion
    }
}

