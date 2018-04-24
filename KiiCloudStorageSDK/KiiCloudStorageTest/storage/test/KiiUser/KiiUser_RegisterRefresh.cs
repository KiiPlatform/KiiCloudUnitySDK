using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiUser_RegisterRefresh
    {
        private void setStandardResponse(MockHttpClient client)
        {
            // register response
            client.AddResponse(201, "{" +
                "\"userID\" : \"userABCD\"," +
                "\"internalUserID\" : 148478248144076800," +
                "\"loginName\" : \"test000\"," +
                "\"displayName\" : \"person test000\"," +
                "\"emailAddress\" : \"test001@testkii.com\"," +
                "\"phoneNumber\" : \"+819098439211\"," +
                "\"country\" : \"JP\"}");

            // get access token respose
            client.AddResponse(200, "{" +
                "\"id\" : \"userABCD\"," +
                "\"expires_in\" : 148478248144076800," +
                "\"access_token\" : \"tokenAbcd\"}");
        }

        private void setStandardResponseForRefresh(MockHttpClient client)
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
            action = "When we call Register() with username and Server returns KiiUser,",
            expected = "We can get UserID that server sends"
            )]
        public void Test_0000_Register()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);
            this.setStandardResponseForRefresh(client);

            // register
            KiiUser user = KiiUser.BuilderWithName("kiitest")
                .WithEmail("test001@testkii.com")
                .WithPhone("+819098439211")
                .Build();
            user.Displayname = "person test000";
            user.Register("pass1234");

            // verify user properties
            Assert.AreEqual("userABCD", user.ID);
            Assert.AreEqual("test000", user.Username);
            Assert.AreEqual("person test000", user.Displayname);
            Assert.AreEqual("test001@testkii.com", user.Email);
            Assert.AreEqual("+819098439211", user.Phone);
            Assert.AreEqual("JP", user.Country);
            Assert.IsTrue(user.EmailVerified);
            Assert.IsTrue(user.PhoneVerified);
            Assert.IsNotNull(KiiUser.AccessToken);

            // verify current user
            Assert.IsNotNull(KiiUser.CurrentUser);
            Assert.AreEqual(user, KiiUser.CurrentUser);

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

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call Register() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0001_Register_null()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            KiiUser user = KiiUser.BuilderWithName("kiitest").Build();
            user.Register(null);
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call Register() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0002_Register_server_error()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new CloudException(400, "{}"));

            KiiUser user = KiiUser.BuilderWithName("kiitest").Build();
            user.Register("pass1234");
        }

        #endregion

        #region

        [Test(), KiiUTInfo(
            action = "When we call Refresh() and Server returns KiiUser,",
            expected = "We can get UserID/DisplayName/Country/Email/EmailVerified/Phone/PhoneVerified that server sends"
            )]
        public void Test_0100_Refresh()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponseForRefresh(client);

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/abcd"));
            user.Refresh();
            Assert.AreEqual("abcd", user.ID);
            Assert.AreEqual("person test000", user.Displayname);
            Assert.AreEqual("JP", user.Country);
            Assert.AreEqual("test001@testkii.com", user.Email);
            Assert.AreEqual(true, user.EmailVerified);
            Assert.AreEqual("+819098439211", user.Phone);
            Assert.AreEqual(true, user.PhoneVerified);
        }

        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call Refresh() to KiiUser that doesn't have UserID,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_0101_Refresh_null_id()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponseForRefresh(client);

            KiiUser user = KiiUser.BuilderWithName("kii1234").Build();
            user.Refresh();
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call Refresh() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0102_Refresh_server_error()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new CloudException(400, "{}"));

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/abcd"));
            user.Refresh();
        }

        #endregion

    }
}

