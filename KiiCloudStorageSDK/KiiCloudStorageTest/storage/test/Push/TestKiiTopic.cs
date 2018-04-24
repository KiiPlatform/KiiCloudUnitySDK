using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiTopic
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

        #region Delete
        [Test(), KiiUTInfo(
            action = "When we call Delete()",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0001_Delete()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            topic.Delete ();
            Assert.AreEqual (KiiHttpMethod.DELETE, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic", client.RequestUrl [0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call Delete() by async",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0002_Delete_Async()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            topic.Delete ((KiiTopic target, Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.DELETE, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic", client.RequestUrl [0]);
            });
        }
        [Test(),
         ExpectedException(typeof(InvalidOperationException)),
         KiiUTInfo(
            action = "When we call Delete() after logout",
            expected = "InvalidOperationException must be thrown."
            )]
        public void Test_Delete_By_Anonymous()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            Kii.LogOut ();
            topic.Delete ();
        }
        #endregion
        
        #region Save
        [Test(), KiiUTInfo(
            action = "When we call Save()",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0003_Save()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            topic.Save ();
            Assert.AreEqual (KiiHttpMethod.PUT, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic", client.RequestUrl [0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call Save() by async",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0004_Save_Async()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            topic.Save ((KiiTopic target, Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.PUT, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic", client.RequestUrl [0]);
            });
        }
        [Test(), KiiUTInfo(
            action = "When we call Save() after logout.",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_Save_By_Anonymous()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse(new CloudException(401, null));
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            Kii.LogOut ();
            try {
                topic.Save ();
                Assert.Fail("CloudException has not thrown");
            } catch (CloudException e) {
                // pass
            }
            Assert.AreEqual (KiiHttpMethod.PUT, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic", client.RequestUrl [0]);
        }

        #endregion

        #region SendMessage
        [Test(), KiiUTInfo(
            action = "When we call SendMessage()",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0005_SendMessage()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);

            KiiPushMessageData data = new KiiPushMessageData();
            data.Put ("payload", "abc");
            KiiPushMessage message = KiiPushMessage.BuildWith(data).Build();

            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            topic.SendMessage (message);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("{\"data\":{\"payload\":\"abc\"},\"gcm\":{\"enabled\":true},\"apns\":{\"enabled\":true},\"mqtt\":{\"enabled\":true}}", client.RequestBody [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic/push/messages", client.RequestUrl [0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call SendMessage() by async",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0006_SendMessage_Async()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            
            KiiPushMessageData data = new KiiPushMessageData();
            data.Put ("payload", "abc");
            KiiPushMessage message = KiiPushMessage.BuildWith(data).Build();
            
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            topic.SendMessage (message, (KiiPushMessage msg, Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
                Assert.AreEqual ("{\"data\":{\"payload\":\"abc\"},\"gcm\":{\"enabled\":true},\"apns\":{\"enabled\":true},\"mqtt\":{\"enabled\":true}}", client.RequestBody [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic/push/messages", client.RequestUrl [0]);
            });
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call SendMessage() with null parameter",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_0007_SendMessage_Null()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            topic.SendMessage (null);
        }
        [Test(), KiiUTInfo(
            action = "When we call SendMessage() by async with null parameter",
            expected = "ArgumentNullException must be passed to callback."
            )]
        public void Test_0008_SendMessage_Null_Async()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse (200, null);
            
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            topic.SendMessage (null, (KiiPushMessage msg, Exception e)=>{
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(ArgumentNullException), e);
            });
        }
        [Test(), KiiUTInfo(
            action = "When we call SendMessage() after logout",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_SendMessage_Anonymous()
        {
            this.LogIn ();
            ClearClientRequest ();
            client.AddResponse(new CloudException(401, null));
            
            KiiPushMessageData data = new KiiPushMessageData();
            data.Put ("payload", "abc");
            KiiPushMessage message = KiiPushMessage.BuildWith(data).Build();
            
            KiiTopic topic = KiiUser.CurrentUser.Topic ("my_topic");
            Kii.LogOut ();
            try {
                topic.SendMessage (message);
                Assert.Fail("CloudException has not thrown");
            } catch (CloudException e) {
                // pass
            }
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("{\"data\":{\"payload\":\"abc\"},\"gcm\":{\"enabled\":true},\"apns\":{\"enabled\":true},\"mqtt\":{\"enabled\":true}}", client.RequestBody [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/user1234/topics/my_topic/push/messages", client.RequestUrl [0]);
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

