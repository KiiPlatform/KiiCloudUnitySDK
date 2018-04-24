using System;
using NUnit.Framework;
using JsonOrg;
using KiiCorp.Cloud.Storage.Connector;
using System.Collections;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiUser_SNS_UnLink
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
            Dictionary<string, object> socialAccessTokenDictionary = new Dictionary<string, object>();
            socialAccessTokenDictionary.Add("accessToken", "dummy");
            KiiUser.CurrentUser.SetSocialAccessTokenDictionary(socialAccessTokenDictionary);

            AddMockUnLinkAndRefreshResponse(200, "user-0000-1111-2222");
            KiiUser user = KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.FACEBOOK);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual(0, user.GetSocialAccessTokenDictionary().Count);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.IsFalse(client.RequestHeader[0].values.ContainsKey("content-type"));
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "facebook", "unlink"), client.RequestUrl[0]);
            Assert.IsNull(client.RequestBody[0]);
        }
        [Test()]
        public void Test_GoogleSync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, object> socialAccessTokenDictionary = new Dictionary<string, object>();
            socialAccessTokenDictionary.Add("accessToken", "dummy");
            KiiUser.CurrentUser.SetSocialAccessTokenDictionary(socialAccessTokenDictionary);

            AddMockUnLinkAndRefreshResponse(200, "user-0000-1111-2222");
            KiiUser user = KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.GOOGLE);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual(0, user.GetSocialAccessTokenDictionary().Count);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.IsFalse(client.RequestHeader[0].values.ContainsKey("content-type"));
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "google", "unlink"), client.RequestUrl[0]);
            Assert.IsNull(client.RequestBody[0]);
        }
        [Test()]
        public void Test_RenRenSync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, object> socialAccessTokenDictionary = new Dictionary<string, object>();
            socialAccessTokenDictionary.Add("accessToken", "dummy");
            KiiUser.CurrentUser.SetSocialAccessTokenDictionary(socialAccessTokenDictionary);

            AddMockUnLinkAndRefreshResponse(200, "user-0000-1111-2222");
            KiiUser user = KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.RENREN);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual(0, user.GetSocialAccessTokenDictionary().Count);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.IsFalse(client.RequestHeader[0].values.ContainsKey("content-type"));
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "renren", "unlink"), client.RequestUrl[0]);
            Assert.IsNull(client.RequestBody[0]);
        }
        [Test()]
        public void Test_TwitterSync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, object> socialAccessTokenDictionary = new Dictionary<string, object>();
            socialAccessTokenDictionary.Add("accessToken", "dummy");
            KiiUser.CurrentUser.SetSocialAccessTokenDictionary(socialAccessTokenDictionary);

            AddMockUnLinkAndRefreshResponse(200, "user-0000-1111-2222");
            KiiUser user = KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.TWITTER);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual(0, user.GetSocialAccessTokenDictionary().Count);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.IsFalse(client.RequestHeader[0].values.ContainsKey("content-type"));
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "twitter", "unlink"), client.RequestUrl[0]);
            Assert.IsNull(client.RequestBody[0]);
        }
        [Test()]
        public void Test_QQSync()
        {
            LogIn("user-0000-1111-2222");
            Dictionary<string, object> socialAccessTokenDictionary = new Dictionary<string, object>();
            socialAccessTokenDictionary.Add("accessToken", "dummy");
            KiiUser.CurrentUser.SetSocialAccessTokenDictionary(socialAccessTokenDictionary);

            AddMockUnLinkAndRefreshResponse(200, "user-0000-1111-2222");
            KiiUser user = KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.QQ);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.ID, "user-0000-1111-2222");
            Assert.AreEqual(user.Username, "username_user-0000-1111-2222");
            Assert.AreEqual(user.Displayname, "display_user-0000-1111-2222");
            Assert.AreEqual(KiiUser.CurrentUser.ID, "user-0000-1111-2222");
            Assert.AreEqual(KiiUser.AccessToken, "token-aaaa-bbbb-cccc");
            Assert.AreEqual(0, user.GetSocialAccessTokenDictionary().Count);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.IsFalse(client.RequestHeader[0].values.ContainsKey("content-type"));
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "qq", "unlink"), client.RequestUrl[0]);
            Assert.IsNull(client.RequestBody[0]);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_UnsupportedProviderSync()
        {
            LogIn("user-0000-1111-2222");
            KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.DROPBOX);
        }
        [Test(), ExpectedException(typeof(UnauthorizedException))]
        public void Test_401Sync()
        {
            // Not authorized to unlink the user.
            LogIn("user-0000-1111-2222");
            client.AddResponse(new UnauthorizedException("", null, ""));
            KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.FACEBOOK);
        }
        [Test(), ExpectedException(typeof(NotFoundException))]
        public void Test_404Sync()
        {
            // The user to unlink is not found
            LogIn("user-0000-1111-2222");
            client.AddResponse(new NotFoundException("", null, "", NotFoundException.Reason.USER_NOT_FOUND));
            KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.FACEBOOK);
        }
        [Test(), ExpectedException(typeof(ConflictException))]
        public void Test_409Sync()
        {
            // The user to unlink is not linked with the SNS.
            LogIn("user-0000-1111-2222");
            client.AddResponse(new ConflictException("", null, "", ConflictException.Reason.USER_NOT_LINKED));
            KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.FACEBOOK);
        }
        #endregion

        #region ASync
        [Test()]
        public void Test_FacebookASync()
        {
            LogIn("user-0000-1111-2222");
            AddMockUnLinkAndRefreshResponse(200, "user-0000-1111-2222");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            Dictionary<string, object> socialAccessTokenDictionary = new Dictionary<string, object>();
            socialAccessTokenDictionary.Add("accessToken", "dummy");
            KiiUser.CurrentUser.SetSocialAccessTokenDictionary(socialAccessTokenDictionary);

            KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.FACEBOOK, (KiiUser u, Exception e)=>{
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
            Assert.AreEqual(0, user.GetSocialAccessTokenDictionary().Count);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.IsFalse(client.RequestHeader[0].values.ContainsKey("content-type"));
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "facebook", "unlink"), client.RequestUrl[0]);
            Assert.IsNull(client.RequestBody[0]);
        }
        [Test()]
        public void Test_GoogleASync()
        {
            LogIn("user-0000-1111-2222");
            AddMockUnLinkAndRefreshResponse(200, "user-0000-1111-2222");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            Dictionary<string, object> socialAccessTokenDictionary = new Dictionary<string, object>();
            socialAccessTokenDictionary.Add("accessToken", "dummy");
            KiiUser.CurrentUser.SetSocialAccessTokenDictionary(socialAccessTokenDictionary);

            KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.GOOGLE, (KiiUser u, Exception e)=>{
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
            Assert.AreEqual(0, user.GetSocialAccessTokenDictionary().Count);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.IsFalse(client.RequestHeader[0].values.ContainsKey("content-type"));
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "google", "unlink"), client.RequestUrl[0]);
            Assert.IsNull(client.RequestBody[0]);
        }
        [Test()]
        public void Test_RenRenASync()
        {
            LogIn("user-0000-1111-2222");
            AddMockUnLinkAndRefreshResponse(200, "user-0000-1111-2222");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            Dictionary<string, object> socialAccessTokenDictionary = new Dictionary<string, object>();
            socialAccessTokenDictionary.Add("accessToken", "dummy");
            KiiUser.CurrentUser.SetSocialAccessTokenDictionary(socialAccessTokenDictionary);

            KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.RENREN, (KiiUser u, Exception e)=>{
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
            Assert.AreEqual(0, user.GetSocialAccessTokenDictionary().Count);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.IsFalse(client.RequestHeader[0].values.ContainsKey("content-type"));
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "renren", "unlink"), client.RequestUrl[0]);
            Assert.IsNull(client.RequestBody[0]);
        }
        [Test()]
        public void Test_TwitterASync()
        {
            LogIn("user-0000-1111-2222");
            AddMockUnLinkAndRefreshResponse(200, "user-0000-1111-2222");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            Dictionary<string, object> socialAccessTokenDictionary = new Dictionary<string, object>();
            socialAccessTokenDictionary.Add("accessToken", "dummy");
            KiiUser.CurrentUser.SetSocialAccessTokenDictionary(socialAccessTokenDictionary);

            KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.TWITTER, (KiiUser u, Exception e)=>{
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
            Assert.AreEqual(0, user.GetSocialAccessTokenDictionary().Count);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.IsFalse(client.RequestHeader[0].values.ContainsKey("content-type"));
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "twitter", "unlink"), client.RequestUrl[0]);
            Assert.IsNull(client.RequestBody[0]);
        }
        [Test()]
        public void Test_QQASync()
        {
            LogIn("user-0000-1111-2222");
            AddMockUnLinkAndRefreshResponse(200, "user-0000-1111-2222");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            Dictionary<string, object> socialAccessTokenDictionary = new Dictionary<string, object>();
            socialAccessTokenDictionary.Add("accessToken", "dummy");
            KiiUser.CurrentUser.SetSocialAccessTokenDictionary(socialAccessTokenDictionary);

            KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.QQ, (KiiUser u, Exception e)=>{
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
            Assert.AreEqual(0, user.GetSocialAccessTokenDictionary().Count);

            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual("appId", client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual("appKey", client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.IsFalse(client.RequestHeader[0].values.ContainsKey("content-type"));
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", "qq", "unlink"), client.RequestUrl[0]);
            Assert.IsNull(client.RequestBody[0]);
        }
        [Test()]
        public void Test_401ASync()
        {
            // Not authorized to unlink the user.
            LogIn("user-0000-1111-2222");
            client.AddResponse(new UnauthorizedException("", null, ""));

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.FACEBOOK, (KiiUser u, Exception e)=>{
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
            // The user to unlink is not found
            LogIn("user-0000-1111-2222");
            client.AddResponse(new NotFoundException("", null, "", NotFoundException.Reason.USER_NOT_FOUND));

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.FACEBOOK, (KiiUser u, Exception e)=>{
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
            // The user to unlink is not linked with the SNS.
            LogIn("user-0000-1111-2222");
            client.AddResponse(new ConflictException("", null, "", ConflictException.Reason.USER_NOT_LINKED));

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.FACEBOOK, (KiiUser u, Exception e)=>{
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
            CountDownLatch cd = new CountDownLatch (1);
            KiiUser user = null;
            Exception exception = null;

            KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.DROPBOX, (KiiUser u, Exception e)=>{
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
            KiiUser.CurrentUser.UnLinkWithSocialNetwork(Provider.FACEBOOK, null);
        }
        #endregion

        private void AddMockUnLinkAndRefreshResponse(int httpStatus, string id)
        {
            client.AddResponse(204, "");

            JsonObject refreshResponse = new JsonObject();
            refreshResponse.Put("userID", id);
            refreshResponse.Put("loginName", "username_" + id);
            refreshResponse.Put("displayName", "display_" + id);

            client.AddResponse(200, refreshResponse.ToString());
        }

    }
}

