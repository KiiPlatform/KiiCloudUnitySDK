using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TopicExistsTest
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

        #region Sync
        [Test()]
        public void Test_AppScopeTrueSync()
        {
            LogIn("test-user-00001");
            client.AddResponse (204, null);
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            Assert.IsTrue(topic.Exists());
            Assert.AreEqual (KiiHttpMethod.HEAD, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics", topicName), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_AppScopeFalseSync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new NotFoundException("", null, "", NotFoundException.Reason.TOPIC_NOT_FOUND));
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            Assert.IsFalse(topic.Exists());
            Assert.AreEqual (KiiHttpMethod.HEAD, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics", topicName), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_GroupScopeTrueSync()
        {
            LogIn("test-user-00001");
            client.AddResponse (204, null);
            string groupID = "test_group";
            string topicName = "test_topic";
            KiiTopic topic = KiiGroup.GroupWithID(groupID).Topic(topicName);
            Assert.IsTrue(topic.Exists());
            Assert.AreEqual (KiiHttpMethod.HEAD, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics", topicName), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_GroupScopeFalseSync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new NotFoundException("", null, "", NotFoundException.Reason.TOPIC_NOT_FOUND));
            string groupID = "test_group";
            string topicName = "test_topic";
            KiiTopic topic = KiiGroup.GroupWithID(groupID).Topic(topicName);
            Assert.IsFalse(topic.Exists());
            Assert.AreEqual (KiiHttpMethod.HEAD, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics", topicName), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_UserScopeTrueSync()
        {
            LogIn("test-user-00001");
            client.AddResponse (204, null);
            string userID = "test_user";
            string topicName = "test_topic";
            KiiTopic topic = KiiUser.UserWithID(userID).Topic(topicName);
            Assert.IsTrue(topic.Exists());
            Assert.AreEqual (KiiHttpMethod.HEAD, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics", topicName), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_UserScopeFalseSync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new NotFoundException("", null, "", NotFoundException.Reason.TOPIC_NOT_FOUND));
            string userID = "test_user";
            string topicName = "test_topic";
            KiiTopic topic = KiiUser.UserWithID(userID).Topic(topicName);
            Assert.IsFalse(topic.Exists());
            Assert.AreEqual (KiiHttpMethod.HEAD, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics", topicName), client.RequestUrl [0]);
        }
        [Test(), ExpectedException(typeof(InvalidOperationException))]
        public void Test_AnonymousSync()
        {
            KiiUser.LogOut();
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            topic.Exists();
        }
        [Test(), ExpectedException(typeof(UnauthorizedException))]
        public void Test_Status401Sync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new UnauthorizedException("", null, ""));
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            topic.Exists();
        }
        [Test(), ExpectedException(typeof(ForbiddenException))]
        public void Test_Status403Sync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new ForbiddenException("", null, ""));
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            topic.Exists();
        }
        #endregion

        #region ASync
        [Test()]
        public void Test_AppScopeTrueASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (204, null);
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            bool? existence = null;
            Exception exception = null;
            topic.Exists((bool? b, Exception e) => {
                existence = b;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsTrue(existence.Value);
            Assert.IsNull(exception);
            Assert.AreEqual (KiiHttpMethod.HEAD, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics", topicName), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_AppScopeFalseASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new NotFoundException("", null, "", NotFoundException.Reason.TOPIC_NOT_FOUND));
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            bool? existence = null;
            Exception exception = null;
            topic.Exists((bool? b, Exception e) => {
                existence = b;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsFalse(existence.Value);
            Assert.IsNull(exception);
            Assert.AreEqual (KiiHttpMethod.HEAD, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics", topicName), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_GroupScopeTrueASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (204, null);
            string groupID = "test_group";
            string topicName = "test_topic";
            KiiTopic topic = KiiGroup.GroupWithID(groupID).Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            bool? existence = null;
            Exception exception = null;
            topic.Exists((bool? b, Exception e) => {
                existence = b;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsTrue(existence.Value);
            Assert.IsNull(exception);
            Assert.AreEqual (KiiHttpMethod.HEAD, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics", topicName), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_GroupScopeFalseASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new NotFoundException("", null, "", NotFoundException.Reason.TOPIC_NOT_FOUND));
            string groupID = "test_group";
            string topicName = "test_topic";
            KiiTopic topic = KiiGroup.GroupWithID(groupID).Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            bool? existence = null;
            Exception exception = null;
            topic.Exists((bool? b, Exception e) => {
                existence = b;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsFalse(existence.Value);
            Assert.IsNull(exception);
            Assert.AreEqual (KiiHttpMethod.HEAD, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics", topicName), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_UserScopeTrueASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (204, null);
            string userID = "test_user";
            string topicName = "test_topic";
            KiiTopic topic = KiiUser.UserWithID(userID).Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            bool? existence = null;
            Exception exception = null;
            topic.Exists((bool? b, Exception e) => {
                existence = b;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsTrue(existence.Value);
            Assert.IsNull(exception);
            Assert.AreEqual (KiiHttpMethod.HEAD, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics", topicName), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_UserScopeFalseASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new NotFoundException("", null, "", NotFoundException.Reason.TOPIC_NOT_FOUND));
            string userID = "test_user";
            string topicName = "test_topic";
            KiiTopic topic = KiiUser.UserWithID(userID).Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            bool? existence = null;
            Exception exception = null;
            topic.Exists((bool? b, Exception e) => {
                existence = b;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsFalse(existence.Value);
            Assert.IsNull(exception);
            Assert.AreEqual (KiiHttpMethod.HEAD, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics", topicName), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_AnonymousASync()
        {
            KiiUser.LogOut();
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            bool? existence = null;
            Exception exception = null;
            topic.Exists((bool? b, Exception e) => {
                existence = b;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsFalse(existence.HasValue);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(InvalidOperationException), exception);
        }
        [Test()]
        public void Test_Status401ASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new UnauthorizedException("", null, ""));
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            bool? existence = null;
            Exception exception = null;
            topic.Exists((bool? b, Exception e) => {
                existence = b;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsFalse(existence.HasValue);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(UnauthorizedException), exception);
            Assert.AreEqual (KiiHttpMethod.HEAD, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics", topicName), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_Status403ASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new ForbiddenException("", null, ""));
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            bool? existence = null;
            Exception exception = null;
            topic.Exists((bool? b, Exception e) => {
                existence = b;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsFalse(existence.HasValue);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ForbiddenException), exception);
            Assert.AreEqual (KiiHttpMethod.HEAD, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics", topicName), client.RequestUrl [0]);
        }
        #endregion

    }
}

