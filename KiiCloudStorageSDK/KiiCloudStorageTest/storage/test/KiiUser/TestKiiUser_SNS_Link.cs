using System;
using NUnit.Framework;
using JsonOrg;
using KiiCorp.Cloud.Storage.Connector;
using System.Collections;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiUser_SNS_Link
    {
        private MockHttpClient client;

        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            Kii.AsyncHttpClientFactory = factory;
            client = (MockHttpClient)factory.Client;
            if (KiiUser.CurrentUser != null)
            {
                KiiUser.LogOut();
            }
        }
        [TearDown()]
        public void TearDown()
        {
            if (KiiUser.CurrentUser != null)
            {
                KiiUser.LogOut();
            }
        }
        private void LogIn(string userId)
        {
            // set Response
            client.AddResponse(200, "{" +
                "\"id\" : \"" + userId + "\"," +
                "\"access_token\" : \"token-aaaa-bbbb-cccc\"," +
                "\"expires_in\" : 9223372036854775}");
            KiiUser.LogIn(userId, "pass1234");
            client.Clear();
        }

        #region Sync
        [Test()]
        public void Test_FacebookSync()
        {
            LogIn("user-0000-1111-2222");
            AddMockLinkAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "facebook-9999-8888-7777");
            KiiUser user = KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.FACEBOOK);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual("facebook-9999-8888-7777", user.GetSocialAccessTokenDictionary() ["oauth_token"]);
            Assert.AreEqual(false, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.LinkFacebookRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "facebook", "link"), client.RequestUrl[0]);
            Assert.AreEqual(1, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("facebook-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
        }
        [Test()]
        public void Test_GoogleSync()
        {
            LogIn("user-0000-1111-2222");
            AddMockLinkAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "google-9999-8888-7777");
            KiiUser user = KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.GOOGLE);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual("google-9999-8888-7777", user.GetSocialAccessTokenDictionary() ["oauth_token"]);
            Assert.AreEqual(false, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.LinkGoogleRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "google", "link"), client.RequestUrl[0]);
            Assert.AreEqual(1, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("google-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
        }
        [Test()]
        public void Test_RenRenSync()
        {
            LogIn("user-0000-1111-2222");
            AddMockLinkAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "renren-9999-8888-7777");
            KiiUser user = KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.RENREN);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual("renren-9999-8888-7777", user.GetSocialAccessTokenDictionary() ["oauth_token"]);
            Assert.AreEqual(false, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.LinkRenRenRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "renren", "link"), client.RequestUrl[0]);
            Assert.AreEqual(1, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("renren-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
        }
        [Test()]
        public void Test_TwitterSync()
        {
            LogIn("user-0000-1111-2222");
            AddMockLinkAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "twitter-9999-8888-7777");
            accessCredential.Add("accessTokenSecret", "twitter-secret-6666-5555");
            KiiUser user = KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.TWITTER);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual("twitter-9999-8888-7777", user.GetSocialAccessTokenDictionary() ["oauth_token"]);
            Assert.AreEqual(false, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);
            Assert.AreEqual("twitter-secret-6666-5555", user.GetSocialAccessTokenDictionary() ["oauth_token_secret"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.LinkTwitterRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "twitter", "link"), client.RequestUrl[0]);
            Assert.AreEqual(2, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("twitter-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
            Assert.AreEqual("twitter-secret-6666-5555", new JsonObject(client.RequestBody[0]).Get("accessTokenSecret"));
        }
        [Test()]
        public void Test_QQSync()
        {
            LogIn("user-0000-1111-2222");
            AddMockLinkAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "qq-9999-8888-7777");
            accessCredential.Add("openID", "qq-openid-6666-5555");
            KiiUser user = KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.QQ);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual("qq-9999-8888-7777", user.GetSocialAccessTokenDictionary() ["oauth_token"]);
            Assert.AreEqual(false, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);
            Assert.AreEqual("qq-openid-6666-5555", user.GetSocialAccessTokenDictionary() ["openID"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.LinkQQRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "qq", "link"), client.RequestUrl[0]);
            Assert.AreEqual(2, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("qq-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
            Assert.AreEqual("qq-openid-6666-5555", new JsonObject(client.RequestBody[0]).Get("openID"));
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_FacebookWithoutAccessTokenSync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.FACEBOOK);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_GoogleWithoutAccessTokenSync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.GOOGLE);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_RenRenWithoutAccessTokenSync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.RENREN);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_TwitterWithoutAccesstokenSync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessTokenSecret", "twitter-secret-6666-5555");
            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.TWITTER);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_TwitterWithoutAccessTokenSecretSync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "twitter-9999-8888-7777");
            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.TWITTER);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_QQWithoutAccessTokenSync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("openID", "qq-openid-6666-5555");
            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.QQ);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_QQWithoutOpenIDSync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "qq-9999-8888-7777");
            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.QQ);
        }
        [Test(), ExpectedException(typeof(UnauthorizedException))]
        public void Test_401Sync()
        {
            // Not authorized to unlink the user.
            LogIn("user-0000-1111-2222");
            client.AddResponse(new UnauthorizedException("", null, ""));
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "facebook-9999-8888-7777");
            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.FACEBOOK);
        }
        [Test(), ExpectedException(typeof(NotFoundException))]
        public void Test_404Sync()
        {
            // The user to link is not found
            LogIn("user-0000-1111-2222");
            client.AddResponse(new NotFoundException("", null, "", NotFoundException.Reason.USER_NOT_FOUND));
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "facebook-9999-8888-7777");
            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.FACEBOOK);
        }
        [Test(), ExpectedException(typeof(ConflictException))]
        public void Test_409Sync()
        {
            // The user to link is already linked with a facebook account.
            LogIn("user-0000-1111-2222");
            client.AddResponse(new ConflictException("", null, "", ConflictException.Reason.USER_ALREADY_EXISTS));
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "facebook-9999-8888-7777");
            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.FACEBOOK);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_UnsupportedProviderSync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "dropbox-9999-8888-7777");
            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.DROPBOX);
        }
        #endregion

        #region ASync
        [Test()]
        public void Test_FacebookASync()
        {
            LogIn("user-0000-1111-2222");
            AddMockLinkAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "facebook-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.FACEBOOK, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual("facebook-9999-8888-7777", user.GetSocialAccessTokenDictionary() ["oauth_token"]);
            Assert.AreEqual(false, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.LinkFacebookRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "facebook", "link"), client.RequestUrl[0]);
            Assert.AreEqual(1, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("facebook-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
        }
        [Test()]
        public void Test_GoogleASync()
        {
            LogIn("user-0000-1111-2222");
            AddMockLinkAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "google-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.GOOGLE, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual("google-9999-8888-7777", user.GetSocialAccessTokenDictionary() ["oauth_token"]);
            Assert.AreEqual(false, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.LinkGoogleRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "google", "link"), client.RequestUrl[0]);
            Assert.AreEqual(1, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("google-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
        }
        [Test()]
        public void Test_RenRenASync()
        {
            LogIn("user-0000-1111-2222");
            AddMockLinkAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "renren-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.RENREN, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual("renren-9999-8888-7777", user.GetSocialAccessTokenDictionary() ["oauth_token"]);
            Assert.AreEqual(false, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.LinkRenRenRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "renren", "link"), client.RequestUrl[0]);
            Assert.AreEqual(1, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("renren-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
        }
        [Test()]
        public void Test_TwitterASync()
        {
            LogIn("user-0000-1111-2222");
            AddMockLinkAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "twitter-9999-8888-7777");
            accessCredential.Add("accessTokenSecret", "twitter-secret-6666-5555");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.TWITTER, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual("twitter-9999-8888-7777", user.GetSocialAccessTokenDictionary() ["oauth_token"]);
            Assert.AreEqual(false, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);
            Assert.AreEqual("twitter-secret-6666-5555", user.GetSocialAccessTokenDictionary() ["oauth_token_secret"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.LinkTwitterRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "twitter", "link"), client.RequestUrl[0]);
            Assert.AreEqual(2, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("twitter-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
            Assert.AreEqual("twitter-secret-6666-5555", new JsonObject(client.RequestBody[0]).Get("accessTokenSecret"));
        }
        [Test()]
        public void Test_QQASync()
        {
            LogIn("user-0000-1111-2222");
            AddMockLinkAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "qq-9999-8888-7777");
            accessCredential.Add("openID", "qq-openid-6666-5555");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.QQ, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual("qq-9999-8888-7777", user.GetSocialAccessTokenDictionary() ["oauth_token"]);
            Assert.AreEqual(false, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);
            Assert.AreEqual("qq-openid-6666-5555", user.GetSocialAccessTokenDictionary() ["openID"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.LinkQQRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "qq", "link"), client.RequestUrl[0]);
            Assert.AreEqual(2, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("qq-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
            Assert.AreEqual("qq-openid-6666-5555", new JsonObject(client.RequestBody[0]).Get("openID"));
        }
        [Test()]
        public void Test_FacebookWithoutAccessTokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.FACEBOOK, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_FacebookWithNullAccessTokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", null);

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.FACEBOOK, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_FacebookWithEmptyAccessTokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.FACEBOOK, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_GoogleWithoutAccessTokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.GOOGLE, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_GoogleWithNullAccessTokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", null);

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.GOOGLE, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_GoogleWithEmptyAccessTokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.GOOGLE, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_RenRenWithoutAccessTokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.RENREN, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_RenRenWithNullAccessTokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", null);

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.RENREN, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_RenRenWithEmptyAccessTokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.RENREN, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_TwitterWithoutAccesstokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessTokenSecret", "twitter-secret-6666-5555");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.TWITTER, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_TwitterWithNullAccesstokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", null);
            accessCredential.Add("accessTokenSecret", "twitter-secret-6666-5555");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.TWITTER, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_TwitterWithEmptyAccesstokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "");
            accessCredential.Add("accessTokenSecret", "twitter-secret-6666-5555");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.TWITTER, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_TwitterWithoutAccessTokenSecretASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "twitter-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.TWITTER, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_TwitterWithNullAccessTokenSecretASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "twitter-9999-8888-7777");
            accessCredential.Add("accessTokenSecret", null);

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.TWITTER, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_TwitterWithEmptyAccessTokenSecretASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "twitter-9999-8888-7777");
            accessCredential.Add("accessTokenSecret", "");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.TWITTER, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_QQWithoutAccessTokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("openID", "qq-openid-6666-5555");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.QQ, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_QQWithNullAccessTokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", null);
            accessCredential.Add("openID", "qq-openid-6666-5555");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.QQ, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_QQWithEmptyAccessTokenASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "");
            accessCredential.Add("openID", "qq-openid-6666-5555");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.QQ, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_QQWithoutOpenIDASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "qq-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.QQ, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_QQWithNullOpenIDASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "qq-9999-8888-7777");
            accessCredential.Add("openID", null);

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.QQ, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_QQWithEmptyOpenIDASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "qq-9999-8888-7777");
            accessCredential.Add("openID", "");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.QQ, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test()]
        public void Test_401ASync()
        {
            LogIn("user-0000-1111-2222");
            client.AddResponse(new UnauthorizedException("", null, ""));
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "facebook-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.RENREN, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(UnauthorizedException), exception);
        }
        [Test()]
        public void Test_404ASync()
        {
            LogIn("user-0000-1111-2222");
            client.AddResponse(new NotFoundException("", null, "", NotFoundException.Reason.USER_NOT_FOUND));
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "facebook-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.RENREN, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(NotFoundException), exception);
        }
        [Test()]
        public void Test_409ASync()
        {
            LogIn("user-0000-1111-2222");
            client.AddResponse(new ConflictException("", null, "", ConflictException.Reason.USER_ALREADY_EXISTS));
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "facebook-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.RENREN, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ConflictException), exception);
        }
        [Test()]
        public void Test_UnsupportedProviderASync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "dropbox-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.DROPBOX, (KiiUser u, Exception e)=>{
                user = u;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(user);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void Test_ASyncWithoutCallback()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            KiiUser.CurrentUser.LinkWithSocialNetwork(accessCredential, Provider.FACEBOOK, null);
        }
        #endregion

        private void AddMockLinkAndRefreshResponse(int httpStatus, string id, string accessToken, long expiresIn)
        {
            JsonObject response = new JsonObject();
            if (id != null)
            {
                response.Put("id", id);
            }
            if (accessToken != null)
            {
                response.Put("access_token", accessToken);
            }
            response.Put("expires_in", expiresIn);
            client.AddResponse(httpStatus, response.ToString());

            JsonObject refreshResponse = new JsonObject();

            refreshResponse.Put("userID", id);
            refreshResponse.Put("loginName", "username_" + id);
            refreshResponse.Put("displayName", "display_" + id);
            refreshResponse.Put("expires_in", expiresIn);

            client.AddResponse(200, refreshResponse.ToString());
        }
    }
}

