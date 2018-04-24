using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiTopicACL
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

        private const string RESPONSE_BODY = @"
        {
            ""SUBSCRIBE_TO_TOPIC"":[
                {""userID"":""ANONYMOUS_USER""},
                {""userID"":""ANY_AUTHENTICATED_USER""},
                {""userID"":""UUUU-1111-2222-3333-4444""},
                {""groupID"":""GGGG-1111-2222-3333-4444""}
            ],
            ""SEND_MESSAGE_TO_TOPIC"":[
                {""userID"":""UUUU-5555-6666-7777-8888""},
                {""groupID"":""GGGG-5555-6666-7777-8888""}
            ]
        }";
        
        #region Sync
        [Test()]
        public void Test_AppScopeSync()
        {
            LogIn("test-user-00001");
            client.AddResponse (200, RESPONSE_BODY);
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            IList<KiiACLEntry<KiiTopic, TopicAction>> entries = topic.ListAclEntries();

            Assert.AreEqual(6, entries.Count);
            Assert.AreEqual(TopicAction.SEND_MESSAGE_TO_TOPIC, entries [0].Action);
            Assert.AreEqual("UUUU-5555-6666-7777-8888", ((KiiUser)entries [0].Subject).ID);
            Assert.AreEqual(TopicAction.SEND_MESSAGE_TO_TOPIC, entries [1].Action);
            Assert.AreEqual("GGGG-5555-6666-7777-8888", ((KiiGroup)entries[1].Subject).ID);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [2].Action);
            Assert.AreEqual(KiiAnonymousUser.Get(), entries [2].Subject);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [3].Action);
            Assert.AreEqual(KiiAnyAuthenticatedUser.Get(), entries [3].Subject);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [4].Action);
            Assert.AreEqual("UUUU-1111-2222-3333-4444", ((KiiUser)entries [4].Subject).ID);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [5].Action);
            Assert.AreEqual("GGGG-1111-2222-3333-4444", ((KiiGroup)entries[5].Subject).ID);

            Assert.AreEqual(KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics", topicName, "acl"), client.RequestUrl [0]);
        }
        [Test(), ExpectedException(typeof(NotFoundException))]
        public void Test_AppScope404Sync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new NotFoundException("", null, "", NotFoundException.Reason.ACL_NOT_FOUND));
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            topic.ListAclEntries();
        }
        [Test(), ExpectedException(typeof(InvalidOperationException))]
        public void Test_NotLoggedInSync()
        {
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            topic.ListAclEntries();
        }
        [Test()]
        public void Test_GroupScopeSync()
        {
            LogIn("test-user-00001");
            client.AddResponse (200, RESPONSE_BODY);
            string groupID = "test_group";
            string topicName = "test_topic";
            KiiTopic topic = KiiGroup.GroupWithID(groupID).Topic(topicName);
            IList<KiiACLEntry<KiiTopic, TopicAction>> entries = topic.ListAclEntries();

            Assert.AreEqual(6, entries.Count);
            Assert.AreEqual(TopicAction.SEND_MESSAGE_TO_TOPIC, entries [0].Action);
            Assert.AreEqual("UUUU-5555-6666-7777-8888", ((KiiUser)entries [0].Subject).ID);
            Assert.AreEqual(TopicAction.SEND_MESSAGE_TO_TOPIC, entries [1].Action);
            Assert.AreEqual("GGGG-5555-6666-7777-8888", ((KiiGroup)entries[1].Subject).ID);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [2].Action);
            Assert.AreEqual(KiiAnonymousUser.Get(), entries [2].Subject);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [3].Action);
            Assert.AreEqual(KiiAnyAuthenticatedUser.Get(), entries [3].Subject);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [4].Action);
            Assert.AreEqual("UUUU-1111-2222-3333-4444", ((KiiUser)entries [4].Subject).ID);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [5].Action);
            Assert.AreEqual("GGGG-1111-2222-3333-4444", ((KiiGroup)entries[5].Subject).ID);

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics", topicName, "acl"), client.RequestUrl [0]);
        }
        [Test(), ExpectedException(typeof(NotFoundException))]
        public void Test_GroupScope404Sync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new NotFoundException("", null, "", NotFoundException.Reason.ACL_NOT_FOUND));
            string groupID = "test_group";
            string topicName = "test_topic";
            KiiTopic topic = KiiGroup.GroupWithID(groupID).Topic(topicName);
            topic.ListAclEntries();
        }
        [Test()]
        public void Test_UserScopeSync()
        {
            LogIn("test-user-00001");
            client.AddResponse (200, RESPONSE_BODY);
            string userID = "test_user";
            string topicName = "test_topic";
            KiiTopic topic = KiiUser.UserWithID(userID).Topic(topicName);
            IList<KiiACLEntry<KiiTopic, TopicAction>> entries = topic.ListAclEntries();

            Assert.AreEqual(6, entries.Count);
            Assert.AreEqual(TopicAction.SEND_MESSAGE_TO_TOPIC, entries [0].Action);
            Assert.AreEqual("UUUU-5555-6666-7777-8888", ((KiiUser)entries [0].Subject).ID);
            Assert.AreEqual(TopicAction.SEND_MESSAGE_TO_TOPIC, entries [1].Action);
            Assert.AreEqual("GGGG-5555-6666-7777-8888", ((KiiGroup)entries[1].Subject).ID);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [2].Action);
            Assert.AreEqual(KiiAnonymousUser.Get(), entries [2].Subject);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [3].Action);
            Assert.AreEqual(KiiAnyAuthenticatedUser.Get(), entries [3].Subject);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [4].Action);
            Assert.AreEqual("UUUU-1111-2222-3333-4444", ((KiiUser)entries [4].Subject).ID);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [5].Action);
            Assert.AreEqual("GGGG-1111-2222-3333-4444", ((KiiGroup)entries[5].Subject).ID);

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics", topicName, "acl"), client.RequestUrl [0]);
        }
        [Test(), ExpectedException(typeof(NotFoundException))]
        public void Test_UserScope404Sync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new NotFoundException("", null, "", NotFoundException.Reason.ACL_NOT_FOUND));
            string userID = "test_user";
            string topicName = "test_topic";
            KiiTopic topic = KiiUser.UserWithID(userID).Topic(topicName);
            topic.ListAclEntries();
        }
        #endregion

        #region ASync
        [Test()]
        public void Test_AppScopeASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (200, RESPONSE_BODY);
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            IList<KiiACLEntry<KiiTopic, TopicAction>> entries = null;
            Exception exception = null;
            topic.ListAclEntries((IList<KiiACLEntry<KiiTopic, TopicAction>> result, Exception e)=>{
                entries = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.AreEqual(6, entries.Count);
            Assert.AreEqual(TopicAction.SEND_MESSAGE_TO_TOPIC, entries [0].Action);
            Assert.AreEqual("UUUU-5555-6666-7777-8888", ((KiiUser)entries [0].Subject).ID);
            Assert.AreEqual(TopicAction.SEND_MESSAGE_TO_TOPIC, entries [1].Action);
            Assert.AreEqual("GGGG-5555-6666-7777-8888", ((KiiGroup)entries[1].Subject).ID);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [2].Action);
            Assert.AreEqual(KiiAnonymousUser.Get(), entries [2].Subject);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [3].Action);
            Assert.AreEqual(KiiAnyAuthenticatedUser.Get(), entries [3].Subject);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [4].Action);
            Assert.AreEqual("UUUU-1111-2222-3333-4444", ((KiiUser)entries [4].Subject).ID);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [5].Action);
            Assert.AreEqual("GGGG-1111-2222-3333-4444", ((KiiGroup)entries[5].Subject).ID);

            Assert.AreEqual(KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics", topicName, "acl"), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_AppScope404ASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new NotFoundException("", null, "", NotFoundException.Reason.ACL_NOT_FOUND));
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            IList<KiiACLEntry<KiiTopic, TopicAction>> entries = null;
            Exception exception = null;
            topic.ListAclEntries((IList<KiiACLEntry<KiiTopic, TopicAction>> result, Exception e)=>{
                entries = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(entries);
            Assert.IsInstanceOfType(typeof(NotFoundException), exception);
            Assert.AreEqual(KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual(Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics", topicName, "acl"), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_NotLoggedInASync()
        {
            string topicName = "test_topic";
            KiiTopic topic = Kii.Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            IList<KiiACLEntry<KiiTopic, TopicAction>> entries = null;
            Exception exception = null;
            topic.ListAclEntries((IList<KiiACLEntry<KiiTopic, TopicAction>> result, Exception e)=>{
                entries = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(entries);
            Assert.IsInstanceOfType(typeof(InvalidOperationException), exception);
        }
        [Test()]
        public void Test_GroupScopeASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (200, RESPONSE_BODY);
            string groupID = "test_group";
            string topicName = "test_topic";
            KiiTopic topic = KiiGroup.GroupWithID(groupID).Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            IList<KiiACLEntry<KiiTopic, TopicAction>> entries = null;
            Exception exception = null;
            topic.ListAclEntries((IList<KiiACLEntry<KiiTopic, TopicAction>> result, Exception e)=>{
                entries = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.AreEqual(6, entries.Count);
            Assert.AreEqual(TopicAction.SEND_MESSAGE_TO_TOPIC, entries [0].Action);
            Assert.AreEqual("UUUU-5555-6666-7777-8888", ((KiiUser)entries [0].Subject).ID);
            Assert.AreEqual(TopicAction.SEND_MESSAGE_TO_TOPIC, entries [1].Action);
            Assert.AreEqual("GGGG-5555-6666-7777-8888", ((KiiGroup)entries[1].Subject).ID);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [2].Action);
            Assert.AreEqual(KiiAnonymousUser.Get(), entries [2].Subject);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [3].Action);
            Assert.AreEqual(KiiAnyAuthenticatedUser.Get(), entries [3].Subject);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [4].Action);
            Assert.AreEqual("UUUU-1111-2222-3333-4444", ((KiiUser)entries [4].Subject).ID);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [5].Action);
            Assert.AreEqual("GGGG-1111-2222-3333-4444", ((KiiGroup)entries[5].Subject).ID);

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics", topicName, "acl"), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_GroupScope404ASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new NotFoundException("", null, "", NotFoundException.Reason.ACL_NOT_FOUND));
            string groupID = "test_group";
            string topicName = "test_topic";
            KiiTopic topic = KiiGroup.GroupWithID(groupID).Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            IList<KiiACLEntry<KiiTopic, TopicAction>> entries = null;
            Exception exception = null;
            topic.ListAclEntries((IList<KiiACLEntry<KiiTopic, TopicAction>> result, Exception e)=>{
                entries = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(entries);
            Assert.IsInstanceOfType(typeof(NotFoundException), exception);
            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics", topicName, "acl"), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_UserScopeASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (200, RESPONSE_BODY);
            string userID = "test_user";
            string topicName = "test_topic";
            KiiTopic topic = KiiUser.UserWithID(userID).Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            IList<KiiACLEntry<KiiTopic, TopicAction>> entries = null;
            Exception exception = null;
            topic.ListAclEntries((IList<KiiACLEntry<KiiTopic, TopicAction>> result, Exception e)=>{
                entries = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.AreEqual(6, entries.Count);
            Assert.AreEqual(TopicAction.SEND_MESSAGE_TO_TOPIC, entries [0].Action);
            Assert.AreEqual("UUUU-5555-6666-7777-8888", ((KiiUser)entries [0].Subject).ID);
            Assert.AreEqual(TopicAction.SEND_MESSAGE_TO_TOPIC, entries [1].Action);
            Assert.AreEqual("GGGG-5555-6666-7777-8888", ((KiiGroup)entries[1].Subject).ID);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [2].Action);
            Assert.AreEqual(KiiAnonymousUser.Get(), entries [2].Subject);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [3].Action);
            Assert.AreEqual(KiiAnyAuthenticatedUser.Get(), entries [3].Subject);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [4].Action);
            Assert.AreEqual("UUUU-1111-2222-3333-4444", ((KiiUser)entries [4].Subject).ID);
            Assert.AreEqual(TopicAction.SUBSCRIBE_TO_TOPIC, entries [5].Action);
            Assert.AreEqual("GGGG-1111-2222-3333-4444", ((KiiGroup)entries[5].Subject).ID);

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics", topicName, "acl"), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_UserScope404ASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new NotFoundException("", null, "", NotFoundException.Reason.ACL_NOT_FOUND));
            string userID = "test_user";
            string topicName = "test_topic";
            KiiTopic topic = KiiUser.UserWithID(userID).Topic(topicName);
            CountDownLatch cd = new CountDownLatch (1);
            IList<KiiACLEntry<KiiTopic, TopicAction>> entries = null;
            Exception exception = null;
            topic.ListAclEntries((IList<KiiACLEntry<KiiTopic, TopicAction>> result, Exception e)=>{
                entries = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(entries);
            Assert.IsInstanceOfType(typeof(NotFoundException), exception);
            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics", topicName, "acl"), client.RequestUrl [0]);
        }
        #endregion

    }
}

