using System;
using NUnit.Framework;
using JsonOrg;
using KiiCorp.Cloud.Storage.Connector;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiUser_SocialProviderTest
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
        }
        private void LogIn(string userId)
        {
            // set Response
            client.AddResponse(200, "{" +
                "\"id\" : \"" + userId + "\"," +
                "\"access_token\" : \"cdef\"," +
                "\"expires_in\" : 9223372036854775}");
            KiiUser.LogIn(userId, "pass1234");
            client.Clear();
        }
        private void SetRefreshResponse(params Provider[] providers)
        {
            JsonObject response = new JsonObject();
            response.Put("userID", "0398e67a-818d-47ee-83fb-3519a6197b81");
            response.Put("internalUserID", 1111111);
            response.Put("loginName", "test000");
            if (providers != null)
            {
                JsonObject thirdPartyAccounts = new JsonObject();
                foreach (Provider provider in providers)
                {
                    string linkedSocialNetworkName = provider.GetLinkedProviderSocialNetworkName();
                    JsonObject inner = new JsonObject();
                    inner.Put("id",  linkedSocialNetworkName + "-12345");
                    inner.Put("type", linkedSocialNetworkName);
                    inner.Put("createdAt", "1397555714567");
                    thirdPartyAccounts.Put(linkedSocialNetworkName, inner);
                }
                response.Put("_thirdPartyAccounts", thirdPartyAccounts);
            }
            client.AddResponse(200, response.ToString());
        }

        #region Sync Tests
        [Test()]
        public void NotLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            SetRefreshResponse(null);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(0, Kii.CurrentUser.LinkedSocialAccounts.Count);
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
        }
        [Test()]
        public void BoxLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.BOX;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
            SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
            Assert.AreEqual(p, info.Provider);
            Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
            Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
        }
        [Test()]
        public void DropBoxLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.DROPBOX;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
            SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
            Assert.AreEqual(p, info.Provider);
            Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
            Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
        }
        [Test()]
        public void FacebookLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.FACEBOOK;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
            SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
            Assert.AreEqual(p, info.Provider);
            Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
            Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
        }

        [Test()]
        public void GoogleLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.GOOGLE;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
            SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[Provider.GOOGLEPLUS];
            Assert.AreEqual(Provider.GOOGLEPLUS, info.Provider);
            Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
            Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLEPLUS));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
        }
        [Test()]
        public void GoogleplusLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.GOOGLEPLUS;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
            SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[Provider.GOOGLEPLUS];
            Assert.AreEqual(Provider.GOOGLEPLUS, info.Provider);
            Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
            Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLEPLUS));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
        }
        [Test()]
        public void LinkedInLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.LINKEDIN;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
            SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
            Assert.AreEqual(p, info.Provider);
            Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
            Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
        }
        [Test()]
        public void LiveLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.LIVE;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
            SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
            Assert.AreEqual(p, info.Provider);
            Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
            Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
        }
        [Test()]
        public void RenRenLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.RENREN;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
            SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
            Assert.AreEqual(p, info.Provider);
            Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
            Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
        }
        [Test()]
        public void SinaLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.SINA;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
            SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
            Assert.AreEqual(p, info.Provider);
            Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
            Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
        }
        [Test()]
        public void TwitterLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.TWITTER;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
            SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
            Assert.AreEqual(p, info.Provider);
            Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
            Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
        }
        [Test()]
        public void YahooLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.YAHOO;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
            SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
            Assert.AreEqual(p, info.Provider);
            Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
            Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
        }
        [Test()]
        public void QQLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.QQ;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
            SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
            Assert.AreEqual(p, info.Provider);
            Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
            Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.QQ));
        }
        [Test()]
        public void AllLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            SetRefreshResponse(Provider.BOX, Provider.DROPBOX, Provider.FACEBOOK, Provider.GOOGLEPLUS, Provider.LINKEDIN,
                Provider.LIVE, Provider.RENREN, Provider.SINA, Provider.TWITTER, Provider.YAHOO, Provider.QQ, Provider.KII);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(12, Kii.CurrentUser.LinkedSocialAccounts.Count);
            foreach (Provider p in (Provider[]) Enum.GetValues(typeof(Provider)))
            {
                Assert.AreEqual(12, Kii.CurrentUser.LinkedSocialAccounts.Count);
                SocialAccountInfo info = null;
                if(p == Provider.GOOGLE) {
                    try {
                        info = Kii.CurrentUser.LinkedSocialAccounts[p];
                        Assert.Fail("should throw exception");
                    } catch(KeyNotFoundException e){
                        // pass
                    }
                } else {
                    info = Kii.CurrentUser.LinkedSocialAccounts[p];
                    Assert.AreEqual(p, info.Provider);
                    Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
                    Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
                }
            }
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLEPLUS));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.QQ));
        }
        [Test()]
        public void WithoutRefreshTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            SetRefreshResponse(Provider.BOX, Provider.DROPBOX, Provider.FACEBOOK, Provider.GOOGLE, Provider.LINKEDIN,
                Provider.LIVE, Provider.RENREN, Provider.SINA, Provider.TWITTER, Provider.YAHOO, Provider.QQ);
            Assert.AreEqual(0, Kii.CurrentUser.LinkedSocialAccounts.Count);
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
        }
        #endregion

        #region ASync Tests
        [Test()]
        public void NotLinkedTestAsync()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            SetRefreshResponse(null);
            Kii.CurrentUser.Refresh((KiiUser user, Exception e)=>{
                Assert.AreEqual(0, Kii.CurrentUser.LinkedSocialAccounts.Count);
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
            });
        }
        [Test()]
        public void BoxLinkedTestAsync()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.BOX;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh((KiiUser user, Exception e)=>{
                Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
                SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
                Assert.AreEqual(p, info.Provider);
                Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
                Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
            });
        }
        [Test()]
        public void DropBoxLinkedTestAsync()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.DROPBOX;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh((KiiUser user, Exception e)=>{
                Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
                SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
                Assert.AreEqual(p, info.Provider);
                Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
                Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
            });
        }
        [Test()]
        public void FacebookLinkedTestAsync()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.FACEBOOK;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh((KiiUser user, Exception e)=>{
                Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
                SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
                Assert.AreEqual(p, info.Provider);
                Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
                Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
            });
        }
        [Test()]
        public void GoogleLinkedTestAsync()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.GOOGLE;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh((KiiUser user, Exception e)=>{
                Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
                SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[Provider.GOOGLEPLUS];
                Assert.AreEqual(Provider.GOOGLEPLUS, info.Provider);
                Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
                Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLEPLUS));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
            });
        }
        [Test()]
        public void GoogleplusLinkedTestAsync()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.GOOGLEPLUS;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh((KiiUser user, Exception e)=>{
                Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
                SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[Provider.GOOGLEPLUS];
                Assert.AreEqual(Provider.GOOGLEPLUS, info.Provider);
                Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
                Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLEPLUS));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
            });
        }
        [Test()]
        public void LinkedInLinkedTestAsync()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.LINKEDIN;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh((KiiUser user, Exception e)=>{
                Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
                SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
                Assert.AreEqual(p, info.Provider);
                Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
                Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
            });
        }
        [Test()]
        public void LiveLinkedTestAsync()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.LIVE;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh((KiiUser user, Exception e)=>{
                Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
                SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
                Assert.AreEqual(p, info.Provider);
                Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
                Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
            });
        }
        [Test()]
        public void RenRenLinkedTestAsync()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.RENREN;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh((KiiUser user, Exception e)=>{
                Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
                SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
                Assert.AreEqual(p, info.Provider);
                Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
                Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
            });
        }
        [Test()]
        public void SinaLinkedTestAsync()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.SINA;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh((KiiUser user, Exception e)=>{
                Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
                SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
                Assert.AreEqual(p, info.Provider);
                Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
                Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
            });
        }
        [Test()]
        public void TwitterLinkedTestAsync()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.TWITTER;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh((KiiUser user, Exception e)=>{
                Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
                SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
                Assert.AreEqual(p, info.Provider);
                Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
                Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
            });
        }
        [Test()]
        public void YahooLinkedTestAsync()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.YAHOO;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh((KiiUser user, Exception e)=>{
                Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
                SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
                Assert.AreEqual(p, info.Provider);
                Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
                Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
            });
        }
        [Test()]
        public void QQLinkedTestAsync()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.QQ;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh((KiiUser user, Exception e)=>{
                Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
                SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
                Assert.AreEqual(p, info.Provider);
                Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
                Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
                Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.QQ));
            });
        }
        [Test()]
        public void AllLinkedTestAsync()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            SetRefreshResponse(Provider.BOX, Provider.DROPBOX, Provider.FACEBOOK, Provider.GOOGLEPLUS, Provider.LINKEDIN,
                Provider.LIVE, Provider.RENREN, Provider.SINA, Provider.TWITTER, Provider.YAHOO, Provider.QQ, Provider.KII);
            Kii.CurrentUser.Refresh((KiiUser user, Exception e)=>{
                Assert.AreEqual(12, Kii.CurrentUser.LinkedSocialAccounts.Count);
                foreach (Provider p in (Provider[]) Enum.GetValues(typeof(Provider)))
                {
                    SocialAccountInfo info = null;
                    if(p == Provider.GOOGLE) {
                        try {
                            info = Kii.CurrentUser.LinkedSocialAccounts[p];
                            Assert.Fail("should throw exception");
                        } catch(KeyNotFoundException ex){
                            // pass
                        }
                    } else {
                        info = Kii.CurrentUser.LinkedSocialAccounts[p];
                        Assert.AreEqual(p, info.Provider);
                        Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
                        Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
                    }
                }
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
                Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.QQ));
            });
        }

        [Test()]
        public void GooglePlusLinkedTest()
        {
            LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            Provider p = Provider.GOOGLEPLUS;
            SetRefreshResponse(p);
            Kii.CurrentUser.Refresh();
            Assert.AreEqual(1, Kii.CurrentUser.LinkedSocialAccounts.Count);
            SocialAccountInfo info = Kii.CurrentUser.LinkedSocialAccounts[p];
            Assert.AreEqual(p, info.Provider);
            Assert.AreEqual(p.GetLinkedProviderSocialNetworkName() + "-12345", info.SocialAccountId);
            Assert.AreEqual(Utils.UnixTimeToDateTime(1397555714567L), info.CreatedAt);
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.BOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.DROPBOX));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.FACEBOOK));
 
            // both google and google plus true
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLE));
            Assert.IsTrue(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.GOOGLEPLUS));

            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LINKEDIN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.LIVE));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.RENREN));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.SINA));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.TWITTER));
            Assert.IsFalse(Kii.CurrentUser.IsLinkedWithSocialProvider(Provider.YAHOO));
        }
        #endregion
    }
}

