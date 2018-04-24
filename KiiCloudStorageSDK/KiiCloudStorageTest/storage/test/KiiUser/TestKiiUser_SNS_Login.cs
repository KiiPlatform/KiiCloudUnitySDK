using System;
using NUnit.Framework;
using JsonOrg;
using KiiCorp.Cloud.Storage.Connector;
using System.Collections;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiUser_SNS_Login
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

        #region Sync
        [Test()]
        public void Test_FacebookSync()
        {
            AddMockLoginAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600, true);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "facebook-9999-8888-7777");
            KiiUser user = KiiUser.LoginWithSocialNetwork(accessCredential, Provider.FACEBOOK);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual("facebook-9999-8888-7777", user.GetSocialAccessTokenDictionary() ["oauth_token"]);
            Assert.AreEqual(true, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.AuthTokenFacebookRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "integration", "facebook"), client.RequestUrl[0]);
            Assert.AreEqual(1, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("facebook-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
        }
        [Test()]
        public void Test_GoogleSync()
        {
            AddMockLoginAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600, false);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "google-9999-8888-7777");
            KiiUser user = KiiUser.LoginWithSocialNetwork(accessCredential, Provider.GOOGLE);
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
            Assert.AreEqual("application/vnd.kii.AuthTokenGoogleRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "integration", "google"), client.RequestUrl[0]);
            Assert.AreEqual(1, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("google-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
        }
        [Test()]
        public void Test_RenRenkSync()
        {
            AddMockLoginAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600, true);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "renren-9999-8888-7777");
            KiiUser user = KiiUser.LoginWithSocialNetwork(accessCredential, Provider.RENREN);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual("renren-9999-8888-7777", user.GetSocialAccessTokenDictionary() ["oauth_token"]);
            Assert.AreEqual(true, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.AuthTokenRenRenRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "integration", "renren"), client.RequestUrl[0]);
            Assert.AreEqual(1, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("renren-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
        }
        [Test()]
        public void Test_TwitterSync()
        {
            AddMockLoginAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600, false);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "twitter-9999-8888-7777");
            accessCredential.Add("accessTokenSecret", "twitter-secret-6666-5555");
            KiiUser user = KiiUser.LoginWithSocialNetwork(accessCredential, Provider.TWITTER);
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
            Assert.AreEqual("application/vnd.kii.AuthTokenTwitterRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "integration", "twitter"), client.RequestUrl[0]);
            Assert.AreEqual(2, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("twitter-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
            Assert.AreEqual("twitter-secret-6666-5555", new JsonObject(client.RequestBody[0]).Get("accessTokenSecret"));
        }
        [Test()]
        public void Test_QQSync()
        {
            AddMockLoginAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600, true);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "qq-9999-8888-7777");
            accessCredential.Add("openID", "qq-openid-6666-5555");
            KiiUser user = KiiUser.LoginWithSocialNetwork(accessCredential, Provider.QQ);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual("qq-9999-8888-7777", user.GetSocialAccessTokenDictionary() ["oauth_token"]);
            Assert.AreEqual(true, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);
            Assert.AreEqual("qq-openid-6666-5555", user.GetSocialAccessTokenDictionary() ["openID"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.AuthTokenQQRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "integration", "qq"), client.RequestUrl[0]);
            Assert.AreEqual(2, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("qq-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
            Assert.AreEqual("qq-openid-6666-5555", new JsonObject(client.RequestBody[0]).Get("openID"));
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_FacebookWithoutAccessTokenSync()
        {
            AddMockLoginAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600, true);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.FACEBOOK);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_GoogleWithoutAccessTokenSync()
        {
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.GOOGLE);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_RenRenWithoutAccessTokenSync()
        {
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.RENREN);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_QQWithoutAccessTokenSync()
        {
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("openID", "qq-openid-6666-5555");
            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.QQ);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_QQWithoutOpenIDSync()
        {
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "qq-9999-8888-7777");
            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.QQ);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_TwitterWithoutAccessTokenSync()
        {
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessTokenSecret", "twitter-secret-6666-5555");
            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.TWITTER);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_TwitterWithoutAccessTokenSecretSync()
        {
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "twitter-9999-8888-7777");
            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.TWITTER);
        }
        [Test(), ExpectedException(typeof(BadRequestException))]
        public void Test_400Sync()
        {
            client.AddResponse(new BadRequestException("", null, "", BadRequestException.Reason.INVALID_INPUT_DATA));
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "facebook-9999-8888-7777");
            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.FACEBOOK);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_UnsupportedProviderSync()
        {
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "dropbox-9999-8888-7777");
            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.DROPBOX);
        }
        #endregion

        #region Async
        [Test()]
        public void Test_FacebookASync()
        {
            AddMockLoginAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600, false);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "facebook-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.FACEBOOK, (KiiUser u, Exception e)=>{
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
            Assert.AreEqual("application/vnd.kii.AuthTokenFacebookRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "integration", "facebook"), client.RequestUrl[0]);
            Assert.AreEqual(1, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("facebook-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
        }
        [Test()]
        public void Test_GoogleASync()
        {
            AddMockLoginAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600, true);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "google-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.GOOGLE, (KiiUser u, Exception e)=>{
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
            Assert.AreEqual(true, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.AuthTokenGoogleRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "integration", "google"), client.RequestUrl[0]);
            Assert.AreEqual(1, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("google-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
        }
        [Test()]
        public void Test_RenRenkASync()
        {
            AddMockLoginAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600, false);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "renren-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.RENREN, (KiiUser u, Exception e)=>{
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
            Assert.AreEqual("application/vnd.kii.AuthTokenRenRenRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "integration", "renren"), client.RequestUrl[0]);
            Assert.AreEqual(1, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("renren-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
        }
        [Test()]
        public void Test_TwitterASync()
        {
            AddMockLoginAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600, true);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "twitter-9999-8888-7777");
            accessCredential.Add("accessTokenSecret", "twitter-secret-6666-5555");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.TWITTER, (KiiUser u, Exception e)=>{
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
            Assert.AreEqual(true, user.GetSocialAccessTokenDictionary() ["kii_new_user"]);
            Assert.AreEqual("twitter-secret-6666-5555", user.GetSocialAccessTokenDictionary() ["oauth_token_secret"]);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("application/vnd.kii.AuthTokenTwitterRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "integration", "twitter"), client.RequestUrl[0]);
            Assert.AreEqual(2, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("twitter-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
            Assert.AreEqual("twitter-secret-6666-5555", new JsonObject(client.RequestBody[0]).Get("accessTokenSecret"));
        }
        [Test()]
        public void Test_QQASync()
        {
            AddMockLoginAndRefreshResponse(200, "user-0000-1111-2222", "token-aaaa-bbbb-cccc", 3600, false);
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "qq-9999-8888-7777");
            accessCredential.Add("openID", "qq-openid-6666-5555");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.QQ, (KiiUser u, Exception e)=>{
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
            Assert.AreEqual("application/vnd.kii.AuthTokenQQRequest+json", client.RequestHeader[0]["content-type"]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "integration", "qq"), client.RequestUrl[0]);
            Assert.AreEqual(2, new JsonObject(client.RequestBody[0]).Length());
            Assert.AreEqual("qq-9999-8888-7777", new JsonObject(client.RequestBody[0]).Get("accessToken"));
            Assert.AreEqual("qq-openid-6666-5555", new JsonObject(client.RequestBody[0]).Get("openID"));
        }
        [Test()]
        public void Test_FacebookWithoutAccessTokenASync()
        {
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.FACEBOOK, (KiiUser u, Exception e)=>{
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
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.GOOGLE, (KiiUser u, Exception e)=>{
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
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.RENREN, (KiiUser u, Exception e)=>{
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
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("openID", "qq-openid-6666-5555");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.QQ, (KiiUser u, Exception e)=>{
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
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "qq-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.QQ, (KiiUser u, Exception e)=>{
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
        public void Test_TwitterWithoutAccessTokenASync()
        {
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessTokenSecret", "twitter-secret-6666-5555");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.TWITTER, (KiiUser u, Exception e)=>{
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
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "twitter-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.TWITTER, (KiiUser u, Exception e)=>{
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
        public void Test_400ASync()
        {
            client.AddResponse(new BadRequestException("", null, "", BadRequestException.Reason.INVALID_INPUT_DATA));
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "facebook-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.FACEBOOK, (KiiUser u, Exception e)=>{
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
            Assert.IsInstanceOfType(typeof(BadRequestException), exception);
        }
        [Test()]
        public void Test_UnsupportedProviderASync()
        {
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "dropbox-9999-8888-7777");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.DROPBOX, (KiiUser u, Exception e)=>{
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
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", "facebook-9999-8888-7777");
            KiiUser.LoginWithSocialNetwork(accessCredential, Provider.FACEBOOK, null);
        }
        #endregion

        private void AddMockLoginAndRefreshResponse(int httpStatus, string id, string accessToken, long expiresIn, bool kiiNewUser)
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
            response.Put("new_user_created", kiiNewUser);
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

