using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiPushSubscription
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
        private void LogIn()
        {
            // set Response
            client.AddResponse(200, "{" +
                               "\"id\" : \"user1234\"," +
                               "\"access_token\" : \"cdef\"," +
                               "\"expires_in\" : 9223372036854775}");
            KiiUser.LogIn("kii1234", "pass1234");
        }

        #region Subscribe
        [Test(), KiiUTInfo(
            action = "When we call Subscribe(KiiTopic)",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0001_Subscribe_Topic()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            KiiUser.CurrentUser.PushSubscription.Subscribe (topic);
            Assert.AreEqual (KiiHttpMethod.PUT, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic/push/subscriptions/users/user1234", client.RequestUrl [0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call Subscribe(KiiBucket)",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0002_Subscribe_Bucket()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiBucket bucket = KiiUser.CurrentUser.Bucket ("my_bucket");
            KiiUser.CurrentUser.PushSubscription.Subscribe (bucket);
            Assert.AreEqual (KiiHttpMethod.PUT, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/buckets/my_bucket/filters/all/push/subscriptions/users/user1234", client.RequestUrl [0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call Subscribe(KiiTopic) by async",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0003_Subscribe_Topic_Async()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            KiiUser.CurrentUser.PushSubscription.Subscribe (topic, (KiiSubscribable target, Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.PUT, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic/push/subscriptions/users/user1234", client.RequestUrl [0]);
            });
        }
        [Test(), KiiUTInfo(
            action = "When we call Subscribe(KiiBucket) by async",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0004_Subscribe_Bucket_Async()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiBucket bucket = KiiUser.CurrentUser.Bucket ("my_bucket");
            KiiUser.CurrentUser.PushSubscription.Subscribe (bucket, (KiiSubscribable target, Exception e) => {
                Assert.AreEqual (KiiHttpMethod.PUT, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/buckets/my_bucket/filters/all/push/subscriptions/users/user1234", client.RequestUrl [0]);
            });
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call Subscribe(subscribable) with null",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_0005_Subscribe_Null()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiUser.CurrentUser.PushSubscription.Subscribe (null);
        }
        [Test(), KiiUTInfo(
            action = "When we call Subscribe(subscribable) by async with null",
            expected = "ArgumentNullException must be passed to callback."
            )]
        public void Test_0006_Subscribe_Null_Async()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiUser.CurrentUser.PushSubscription.Subscribe (null, (KiiSubscribable target, Exception e) => {
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(ArgumentNullException), e);
            });
        }
        #endregion
        
        #region Unsubscribe
        [Test(), KiiUTInfo(
            action = "When we call Unsubscribe(KiiTopic)",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0007_Unsubscribe_Topic()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            KiiUser.CurrentUser.PushSubscription.Unsubscribe (topic);
            Assert.AreEqual (KiiHttpMethod.DELETE, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic/push/subscriptions/users/user1234", client.RequestUrl [0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call Unsubscribe(KiiBucket)",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0008_Unsubscribe_Bucket()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiBucket bucket = KiiUser.CurrentUser.Bucket ("my_bucket");
            KiiUser.CurrentUser.PushSubscription.Unsubscribe (bucket);
            Assert.AreEqual (KiiHttpMethod.DELETE, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/buckets/my_bucket/filters/all/push/subscriptions/users/user1234", client.RequestUrl [0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call Unsubscribe(KiiTopic) by async",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0009_Unsubscribe_Topic_Async()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            KiiUser.CurrentUser.PushSubscription.Unsubscribe (topic, (KiiSubscribable target, Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.DELETE, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic/push/subscriptions/users/user1234", client.RequestUrl [0]);
            });
        }
        [Test(), KiiUTInfo(
            action = "When we call Unsubscribe(KiiBucket) by async",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0010_Unsubscribe_Bucket_Async()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiBucket bucket = KiiUser.CurrentUser.Bucket ("my_bucket");
            KiiUser.CurrentUser.PushSubscription.Unsubscribe (bucket, (KiiSubscribable target, Exception e) => {
                Assert.AreEqual (KiiHttpMethod.DELETE, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/buckets/my_bucket/filters/all/push/subscriptions/users/user1234", client.RequestUrl [0]);
            });
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call Unsubscribe(subscribable) with null",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_0011_Unsubscribe_Null()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiUser.CurrentUser.PushSubscription.Unsubscribe (null);
        }
        [Test(), KiiUTInfo(
            action = "When we call Unsubscribe(subscribable) by async with null",
            expected = "ArgumentNullException must be passed to callback."
            )]
        public void Test_0012_Unsubscribe_Null_Async()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiUser.CurrentUser.PushSubscription.Unsubscribe (null, (KiiSubscribable target, Exception e) => {
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(ArgumentNullException), e);
            });
        }
        #endregion

        #region IsSubscribed
        [Test(), KiiUTInfo(
            action = "When we call IsSubscribed(KiiTopic)",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0013_IsSubscribed_Topic_true()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (204, null);
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            bool isSubscribed = KiiUser.CurrentUser.PushSubscription.IsSubscribed (topic);
            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic/push/subscriptions/users/user1234", client.RequestUrl [0]);
            Assert.IsTrue (isSubscribed);
        }
        [Test(), KiiUTInfo(
            action = "When we call IsSubscribed(KiiTopic)",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0014_IsSubscribed_Topic_false()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (404, null);
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            bool isSubscribed = KiiUser.CurrentUser.PushSubscription.IsSubscribed (topic);
            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic/push/subscriptions/users/user1234", client.RequestUrl [0]);
            Assert.IsFalse (isSubscribed);
        }
        [Test(), KiiUTInfo(
            action = "When we call IsSubscribed(KiiBucket)",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0015_IsSubscribed_Bucket_true()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (204, null);
            KiiBucket bucket = KiiUser.CurrentUser.Bucket ("my_bucket");
            bool isSubscribed = KiiUser.CurrentUser.PushSubscription.IsSubscribed (bucket);
            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/buckets/my_bucket/filters/all/push/subscriptions/users/user1234", client.RequestUrl [0]);
            Assert.IsTrue (isSubscribed);
        }
        [Test(), KiiUTInfo(
            action = "When we call IsSubscribed(KiiBucket)",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0016_IsSubscribed_Bucket_false()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (404, null);
            KiiBucket bucket = KiiUser.CurrentUser.Bucket ("my_bucket");
            bool isSubscribed = KiiUser.CurrentUser.PushSubscription.IsSubscribed (bucket);
            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/buckets/my_bucket/filters/all/push/subscriptions/users/user1234", client.RequestUrl [0]);
            Assert.IsFalse (isSubscribed);
        }
        [Test(), KiiUTInfo(
            action = "When we call IsSubscribed(KiiTopic) by async",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0017_IsSubscribed_Topic_Async_true()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (204, null);
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            KiiUser.CurrentUser.PushSubscription.IsSubscribed (topic, (KiiSubscribable target, bool isSubscribed, Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic/push/subscriptions/users/user1234", client.RequestUrl [0]);
                Assert.IsTrue (isSubscribed);
            });
        }
        [Test(), KiiUTInfo(
            action = "When we call IsSubscribed(KiiTopic) by async",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0018_IsSubscribed_Topic_Async_false()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (404, null);
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            KiiUser.CurrentUser.PushSubscription.IsSubscribed (topic, (KiiSubscribable target, bool isSubscribed, Exception e) => {
                Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic/push/subscriptions/users/user1234", client.RequestUrl [0]);
                Assert.IsFalse (isSubscribed);
            });
        }
        [Test(), KiiUTInfo(
            action = "When we call IsSubscribed(KiiBucket) by async",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0019_IsSubscribed_Bucket_Async_true()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (204, null);
            KiiBucket bucket = KiiUser.CurrentUser.Bucket ("my_bucket");
            KiiUser.CurrentUser.PushSubscription.IsSubscribed (bucket, (KiiSubscribable target, bool isSubscribed, Exception e) => {
                Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/buckets/my_bucket/filters/all/push/subscriptions/users/user1234", client.RequestUrl [0]);
                Assert.IsTrue (isSubscribed);
            });
        }
        [Test(), KiiUTInfo(
            action = "When we call IsSubscribed(KiiBucket) by async",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0020_IsSubscribed_Bucket_Async_false()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (404, null);
            KiiBucket bucket = KiiUser.CurrentUser.Bucket ("my_bucket");
            KiiUser.CurrentUser.PushSubscription.IsSubscribed (bucket, (KiiSubscribable target, bool isSubscribed, Exception e) => {
                Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/buckets/my_bucket/filters/all/push/subscriptions/users/user1234", client.RequestUrl [0]);
                Assert.IsFalse (isSubscribed);
            });
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call IsSubscribed(subscribable) with null",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_0021_IsSubscribed_Null()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiUser.CurrentUser.PushSubscription.IsSubscribed (null);
        }
        [Test(), KiiUTInfo(
            action = "When we call IsSubscribed(subscribable) by async with null",
            expected = "ArgumentNullException must be passed to callback."
            )]
        public void Test_0022_IsSubscribed_Null_Async()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiUser.CurrentUser.PushSubscription.IsSubscribed (null, (KiiSubscribable target, bool isSubscribed, Exception e) => {
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(ArgumentNullException), e);
            });
        }
        #endregion

        private void ClearClientRequest()
        {
            client.RequestUrl.Clear ();
            client.RequestHeader.Clear ();
            client.RequestBody.Clear ();
            client.RequestMethod.Clear ();
        }
    }
}

