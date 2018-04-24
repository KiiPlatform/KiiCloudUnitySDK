using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiUser_FBLogin
    {

        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
        }

        [TearDown()]
        public void TearDown() {
            if(KiiUser.CurrentUser != null){
                KiiUser.LogOut();
            }
        }

        private void setPseudoUserCreationResponse(MockHttpClient client)
        {
            client.AddResponse(201, "{" +
                               "\"userID\" : \"user11111\"," +
                               "\"internalUserID\" : 1111111111111," +
                               "\"_accessToken\" : \"dummyAccessToken1\"," +
                               "\"_hasPassword\" : false," +
                               "\"_disabled\" : false}");
        }

        private void setFBLoginResponse(MockHttpClient client)
        {
            client.AddResponse(200, "{" +
                "\"id\" : \"user22222\"," +
                "\"access_token\" : \"dummyAccessToken2\"," +
                "\"expires_in\" : 9223372036854775," +
                "\"token_type\" : \"bearer\"," +
                "\"new_user_created\" : true}");
        }

        private void setRefreshResponse(MockHttpClient client)
        {
            client.AddResponse(200,
                "{" +
                    "\"userID\" : \"user22222\"," +
                    "\"internalUserID\" : 2222222222222222," +
                    "\"loginName\" : \"u.a1629a40-4cb2-4f56-b007-59e652a40f8d\"," +
                    "\"displayName\" : \"User 2\"," +
                    "\"locale\" : \"ja_JP\"," +
                    "\"_hasPassword\" : true," +
                    "\"_disabled\" : false," +
                    "\"_thirdPartyAccounts\" : {" +
                            "\"facebook\" : {" +
                            "\"id\" : \"100000188475423\"," +
                            "\"type\" : \"facebook\"," +
                            "\"createdAt\" : 1418290423360" +
                        "}" +
                    "}" +
                "}");
        }

        [Test(), KiiUTInfo(
            action = "Login as pseudo user and execute login with facebook token.",
            expected = "Login as another user after facebook login is executed."
            )]
        public void Test_FBLogin_After_PseudoUserCreation()
        {
            // setup mock http client
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            MockHttpClient client = (MockHttpClient)factory.Client;

            // set response
            this.setPseudoUserCreationResponse(client);
            this.setFBLoginResponse(client);
            this.setRefreshResponse(client);

            KiiUser user = KiiUser.RegisterAsPseudoUser(null);
            Assert.AreEqual("user11111", user.ID);
            Assert.AreEqual("dummyAccessToken1", user.GetAccessTokenDictionary() ["access_token"]);

            KiiUser.LoginWithFacebookToken("fbtoken-dummy");
            System.Collections.Generic.IList<MockHttpHeaderList> l = client.RequestHeader;
            string auth = l[2]["Authorization"];
            Assert.AreEqual("Bearer " + "dummyAccessToken2", auth);

            user = KiiUser.CurrentUser;
            Assert.AreEqual("user22222", user.ID);
            Assert.AreEqual("dummyAccessToken2", user.GetAccessTokenDictionary() ["access_token"]);
            Assert.AreEqual("User 2", user.Displayname);

            // verify social access token dictionary.
            Dictionary<string, object> dict = user.GetSocialAccessTokenDictionary();
            Assert.AreEqual(true, dict["kii_new_user"]);
            Assert.AreEqual("100000188475423", dict["provider_user_id"]);
            Assert.AreEqual(KiiCorp.Cloud.Storage.Connector.Provider.FACEBOOK, dict["provider"]);
            Assert.AreEqual("fbtoken-dummy", dict["oauth_token"]);
        }

        [Test(), KiiUTInfo(
            action = "login with facebook token.",
            expected = "current loggedin user and social accesstoken dictionary is not null"
            )]
        public void Test_LoginWithFacebookToken()
        {
            // setup mock http client
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            MockHttpClient client = (MockHttpClient)factory.Client;

            // set response
            this.setFBLoginResponse(client);
            this.setRefreshResponse(client);

            // verify properties for return logged in user.
            KiiUser user = KiiUser.LoginWithFacebookToken("fbtoken-dummy");
            Assert.AreEqual("user22222", user.ID);
            Assert.AreEqual("dummyAccessToken2", user.GetAccessTokenDictionary() ["access_token"]);
            Assert.AreEqual("User 2", user.Displayname);
            
            // verify social access token dictionary.
            Dictionary<string, object> dict = user.GetSocialAccessTokenDictionary();
            Assert.AreEqual(true, dict["kii_new_user"]);
            Assert.AreEqual("100000188475423", dict["provider_user_id"]);
            Assert.AreEqual(KiiCorp.Cloud.Storage.Connector.Provider.FACEBOOK, dict["provider"]);
            Assert.AreEqual("fbtoken-dummy", dict["oauth_token"]);

            // verify properties for current user.
            user = KiiUser.CurrentUser;
            Assert.AreEqual("user22222", user.ID);
            Assert.AreEqual("dummyAccessToken2", user.GetAccessTokenDictionary() ["access_token"]);
            Assert.AreEqual("User 2", user.Displayname);
            
            // verify social access token dictionary.
            dict = user.GetSocialAccessTokenDictionary();
            Assert.AreEqual(true, dict["kii_new_user"]);
            Assert.AreEqual("100000188475423", dict["provider_user_id"]);
            Assert.AreEqual(KiiCorp.Cloud.Storage.Connector.Provider.FACEBOOK, dict["provider"]);
            Assert.AreEqual("fbtoken-dummy", dict["oauth_token"]);
        }

        [Test(), KiiUTInfo(
            action = "Login as pseudo user and execute login with facebook token.",
            expected = "Login as another user after facebook login is executed."
        )]
        public void Test_LoginWithFacebookToken_Async(){
            
            // mock http client 
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            MockHttpClient client = factory.Client;
            
            // set refresh response
            this.setFBLoginResponse(client);
            this.setRefreshResponse(client);
            
            // perform login.
            Assert.IsNull(KiiUser.CurrentUser);
            CountDownLatch cd = new CountDownLatch(1);
            KiiUser user = null;
            Exception exp = null;
            KiiUser.LoginWithFacebookToken("fbtoken-dummy", (KiiUser usr, Exception e ) => {
                user = usr;
                exp = e;
                cd.Signal();
            });
            
            if(!cd.Wait())
                Assert.Fail("Callback has not called.");

            Assert.IsNull(exp);
            Assert.IsNotNull(user);

            // verify properties for return logged in user.
            Assert.AreEqual("user22222", user.ID);
            Assert.AreEqual("dummyAccessToken2", user.GetAccessTokenDictionary() ["access_token"]);
            Assert.AreEqual("User 2", user.Displayname);
            
            // verify social access token dictionary.
            Dictionary<string, object> dict = user.GetSocialAccessTokenDictionary();
            Assert.AreEqual(true, dict["kii_new_user"]);
            Assert.AreEqual("100000188475423", dict["provider_user_id"]);
            Assert.AreEqual(KiiCorp.Cloud.Storage.Connector.Provider.FACEBOOK, dict["provider"]);
            Assert.AreEqual("fbtoken-dummy", dict["oauth_token"]);
            
            // verify properties for current user.
            user = KiiUser.CurrentUser;
            Assert.AreEqual("user22222", user.ID);
            Assert.AreEqual("dummyAccessToken2", user.GetAccessTokenDictionary() ["access_token"]);
            Assert.AreEqual("User 2", user.Displayname);
            
            // verify social access token dictionary.
            dict = user.GetSocialAccessTokenDictionary();
            Assert.AreEqual(true, dict["kii_new_user"]);
            Assert.AreEqual("100000188475423", dict["provider_user_id"]);
            Assert.AreEqual(KiiCorp.Cloud.Storage.Connector.Provider.FACEBOOK, dict["provider"]);
            Assert.AreEqual("fbtoken-dummy", dict["oauth_token"]);
        }
    }
}

