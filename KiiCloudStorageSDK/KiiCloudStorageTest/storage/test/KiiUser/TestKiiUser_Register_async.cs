using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiUser_Register_async
    {
        private MockHttpClient client;
        
        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            client = factory.Client;
        }

        private void SetStandardRegisterResponse()
        {
            client.AddResponse(201, "{" +
                               "\"userID\" : \"userABCD\"," +
                               "\"internalUserID\" : 148478248144076800," +
                               "\"loginName\" : \"test000\"," +
                               "\"displayName\" : \"person test000\"," +
                               "\"emailAddress\" : \"test001@testkii.com\"," +
                               "\"phoneNumber\" : \"+819098439211\"," +
                               "\"country\" : \"JP\"}");
        }

        private void SetStandardLoginResponse()
        {
            client.AddResponse(200, "{" +
                               "\"id\" : \"userABCD\"," +
                               "\"expires_in\" : 148478248144076800," +
                               "\"access_token\" : \"tokenAbcd\"}");
        }

        private void setStandardResponse()
        {
            SetStandardRegisterResponse();
            SetStandardLoginResponse();
        }

        private void setStandardResponseForRefresh()
        {
            client.AddResponse(200, "{" +
                "\"userID\" : \"userABCD\"," +
                "\"internalUserID\" : 87442786592227328," +
                "\"loginName\" : \"test000\"," +
                "\"displayName\" : \"person test000\"," +
                "\"country\" : \"JP\"," +
                "\"emailAddress\" : \"test001@testkii.com\"," +
                "\"emailAddressVerified\" : true," +
                "\"phoneNumber\" : \"+819098439211\"," +
                "\"phoneNumberVerified\" : true}");
        }

        #region KiiUser.LogIn(string, string)

        [Test(), KiiUTInfo(
            action = "When we call Register(callback) with username and Server returns KiiUser,",
            expected = "We can get UserID that server sends"
            )]
        public void Test_0000_Register()
        {
            // set response
            this.setStandardResponse();
            this.setStandardResponseForRefresh();

            // register
            KiiUser user = KiiUser.BuilderWithName("kiitest")
                .WithEmail("test001@testkii.com")
                .WithPhone("+819098439211")
                .Build();
            bool done = false;
            KiiUser user2 = null;
            Exception exception = null;
            CountDownLatch cd = new CountDownLatch(1);
            user.Register("pass1234", (KiiUser created, Exception e) =>
            {
                done = true;
                user2 = created;
                exception = e;
                cd.Signal();
            });

            if(!cd.Wait(new TimeSpan(0, 0, 0, 3)))
                Assert.Fail("Callback not fired.");

            Assert.IsTrue(done);
            Assert.IsNotNull(user2);
            Assert.IsNull(exception);

            // verify user's
            Assert.AreEqual(user, user2);
            Assert.IsNotNull(KiiUser.CurrentUser);
            Assert.AreEqual(user, KiiUser.CurrentUser);

            // verify user properties.
            Assert.AreEqual("userABCD", user.ID);
            Assert.AreEqual("test000", user.Username);
            Assert.AreEqual("person test000", user.Displayname);
            Assert.AreEqual("test001@testkii.com", user.Email);
            Assert.AreEqual("+819098439211", user.Phone);
            Assert.AreEqual("JP", user.Country);
            Assert.IsTrue(user.EmailVerified);
            Assert.IsTrue(user.PhoneVerified);
            Assert.IsNotNull(KiiUser.AccessToken);

            // verify callbacked user properties
            Assert.AreEqual(user.ID, user2.ID);
            Assert.AreEqual(user.Username, user2.Username);
            Assert.AreEqual(user.Displayname, user2.Displayname);
            Assert.AreEqual(user.Email, user2.Email);
            Assert.AreEqual(user.Phone, user2.Phone);
            Assert.AreEqual(user.Country, user2.Country);
            Assert.IsTrue(user2.EmailVerified);
            Assert.IsTrue(user2.PhoneVerified);
            
            // verify current user properties
            Assert.AreEqual(user.ID, KiiUser.CurrentUser.ID);
            Assert.AreEqual(user.Username, KiiUser.CurrentUser.Username);
            Assert.AreEqual(user.Displayname, KiiUser.CurrentUser.Displayname);
            Assert.AreEqual(user.Email, KiiUser.CurrentUser.Email);
            Assert.AreEqual(user.Phone, KiiUser.CurrentUser.Phone);
            Assert.AreEqual(user.Country, KiiUser.CurrentUser.Country);
            Assert.IsTrue(KiiUser.CurrentUser.EmailVerified);
            Assert.IsTrue(KiiUser.CurrentUser.PhoneVerified);
        }

        [Test(), KiiUTInfo(
            action = "When we call Register(callback) with global phone number and Server returns KiiUser,",
            expected = "We can get UserID that server sends"
        )]
        public void Test_0001_Register_With_GlobalPhoneNumber()
        {
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 148478248144076800,
                 ""phoneNumber"": ""+819011112222"",
                 ""phoneNumberVerified"": true
            }
            ";
            client.AddResponse(200, response1);
            string response2 = @"
            {
                 ""id"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""expires_in"" : 148478248144076800,
                 ""access_token"" : ""abcdefghijklmeopqrstuvwxyz0123456789""
            }
            ";
            client.AddResponse(200, response2);
            client.AddResponse(200, response1);

            KiiUser user = KiiUser.BuilderWithPhone("+819011112222").Build();
            bool done = false;
            KiiUser user2 = null;
            Exception exception = null;
            user.Register("pass1234", (KiiUser created, Exception e) =>
            {
                done = true;
                user2 = created;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(user2);
            Assert.IsNull(exception);

            Assert.AreEqual("0398e67a-818d-47ee-83fb-3519a6197b81", user2.ID);
            Assert.AreEqual("+819011112222", user2.Phone);
        }

        [Test(), KiiUTInfo(
            action = "When we call Register(callback) with local phone number and Server returns KiiUser,",
            expected = "We can get UserID that server sends"
        )]
        public void Test_0002_Register_With_LocalPhoneNumber()
        {
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 148478248144076800,
                 ""phoneNumber"": ""09011112222"",
                 ""phoneNumberVerified"": true
            }
            ";
            client.AddResponse(200, response1);
            string response2 = @"
            {
                 ""id"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""expires_in"" : 148478248144076800,
                 ""access_token"" : ""abcdefghijklmeopqrstuvwxyz0123456789""
            }
            ";
            client.AddResponse(200, response2);
            client.AddResponse(200, response1);

            KiiUser user = KiiUser.BuilderWithPhone("09011112222").Build();
            user.Country = "JP";
            bool done = false;
            KiiUser user2 = null;
            Exception exception = null;
            user.Register("pass1234", (KiiUser created, Exception e) =>
            {
                done = true;
                user2 = created;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(user2);
            Assert.IsNull(exception);

            Assert.AreEqual("0398e67a-818d-47ee-83fb-3519a6197b81", user2.ID);
            Assert.AreEqual("09011112222", user2.Phone);
        }

        [Test(), KiiUTInfo(
            action = "When we call Register(callback) with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0010_Register_null()
        {
            // set response
            this.setStandardResponse();

            KiiUser user = KiiUser.BuilderWithName("kiitest").Build();
            bool done = false;
            KiiUser user2 = null;
            Exception exception = null;
            user.Register(null, (KiiUser created, Exception e) =>
            {
                done = true;
                user2 = created;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(user2);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is ArgumentException);
        }

        [Test(), KiiUTInfo(
            action = "When we call Register(callback) and Server returns HTTP 400,",
            expected = "CloudException must be given"
            )]
        public void Test_0011_Register_server_error_Register()
        {
            // set response
            client.AddResponse(new CloudException(400, "{}"));

            KiiUser user = KiiUser.BuilderWithName("kiitest").Build();
            bool done = false;
            KiiUser user2 = null;
            Exception exception = null;
            user.Register("pass1234", (KiiUser created, Exception e) =>
            {
                done = true;
                user2 = created;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(user2);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is CloudException);
        }

        [Test(), KiiUTInfo(
            action = "When we call Register(callback) and Server returns HTTP 400,",
            expected = "CloudException must be given"
            )]
        public void Test_0012_Register_server_error_Login()
        {
            // set response
            this.SetStandardRegisterResponse();
            client.AddResponse(new CloudException(400, "{}"));
            
            KiiUser user = KiiUser.BuilderWithName("kiitest").Build();
            bool done = false;
            KiiUser user2 = null;
            Exception exception = null;
            user.Register("pass1234", (KiiUser created, Exception e) =>
            {
                done = true;
                user2 = created;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(user2);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is CloudException);
        }

        [Test(), KiiUTInfo(
            action = "When we call Register(callback) and Server returns HTTP 400,",
            expected = "CloudException must be given"
            )]
        public void Test_0013_Register_server_error_Refresh()
        {
            // set response
            this.setStandardResponse();
            client.AddResponse(new CloudException(400, "{}"));
            
            KiiUser user = KiiUser.BuilderWithName("kiitest").Build();
            bool done = false;
            KiiUser user2 = null;
            Exception exception = null;
            user.Register("pass1234", (KiiUser created, Exception e) =>
            {
                done = true;
                user2 = created;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(user2);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is CloudException);
        }

        [Test(), KiiUTInfo(
            action = "When we call Register(callback) with local phone number without country code,",
            expected = "ArgumentException must be given"
        )]
        public void Test_0014_Register_LocalPhoneNumber_without_country()
        {
            // set response
            this.setStandardResponse();

            KiiUser user = KiiUser.BuilderWithPhone("09011112222").Build();
            bool done = false;
            KiiUser user2 = null;
            Exception exception = null;
            user.Register("pass1234", (KiiUser created, Exception e) =>
            {
                done = true;
                user2 = created;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(user2);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is ArgumentException);
        }

        [Test(), KiiUTInfo(
            action = "When we call Register(callback) with null and Server returns HTTP 400,",
            expected = "No exception must be given"
            )]
        public void Test_0020_Register_server_error_Register_callback_null()
        {
            // set response
            client.AddResponse(new CloudException(400, "{}"));
            
            KiiUser user = KiiUser.BuilderWithName("kiitest").Build();
            user.Register("pass1234", null);
        }
        #endregion
    }
}

