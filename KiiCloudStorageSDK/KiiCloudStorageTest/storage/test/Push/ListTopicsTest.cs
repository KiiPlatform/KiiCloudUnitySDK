using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class ListTopicsTest
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
                "\"access_token\" : \"cdef\"," +
                "\"expires_in\" : 9223372036854775}");
            KiiUser.LogIn(userId, "pass1234");
            client.Clear();
        }

        #region Sync
        [Test()]
        public void Test_AppScopeSync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{"Topic1", "Topic2"}, null);
            KiiListResult<KiiTopic> topics = Kii.ListTopics();

            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(2, topics.Result.Count);
            Assert.AreEqual("Topic1", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("topics", "Topic1"), topics.Result[0].Uri.ToString());
            Assert.AreEqual("Topic2", topics.Result[1].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("topics", "Topic2"), topics.Result[1].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);
        }
        [Test()]
        public void Test_AppScopeWithEmptyResultSync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{}, null);
            KiiListResult<KiiTopic> topics = Kii.ListTopics();

            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(0, topics.Result.Count);

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);
        }
        [Test()]
        public void Test_AppScopeWithPaginationSync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{"Topic1", "Topic2"}, "ab=12/１２+");
            AddMockResponse(200, new string[]{"Topic3"}, null);
            KiiListResult<KiiTopic> topics = Kii.ListTopics();

            Assert.IsTrue(topics.HasNext);
            Assert.AreEqual("ab=12/１２+", topics.PaginationKey);
            Assert.AreEqual(2, topics.Result.Count);
            Assert.AreEqual("Topic1", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("topics", "Topic1"), topics.Result[0].Uri.ToString());
            Assert.AreEqual("Topic2", topics.Result[1].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("topics", "Topic2"), topics.Result[1].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);

            topics = Kii.ListTopics(topics.PaginationKey);
            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(1, topics.Result.Count);
            Assert.AreEqual("Topic3", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("topics", "Topic3"), topics.Result[0].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [1]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics") + "?paginationKey=" + Uri.EscapeUriString("ab=12/１２+"), client.RequestUrl [1]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [1]["Authorization"]);
        }
        [Test()]
        public void Test_GroupScopeSync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{"Topic1", "Topic2"}, null);
            string groupID = "test_group";
            KiiListResult<KiiTopic> topics = KiiGroup.GroupWithID(groupID).ListTopics();

            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(2, topics.Result.Count);
            Assert.AreEqual("Topic1", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("groups", groupID, "topics", "Topic1"), topics.Result[0].Uri.ToString());
            Assert.AreEqual("Topic2", topics.Result[1].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("groups", groupID, "topics", "Topic2"), topics.Result[1].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);
        }
        [Test()]
        public void Test_GroupScopeWithEmptyResultSync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{}, null);
            string groupID = "test_group";
            KiiListResult<KiiTopic> topics = KiiGroup.GroupWithID(groupID).ListTopics();

            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(0, topics.Result.Count);

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);
        }
        [Test()]
        public void Test_GroupScopeWithPaginationSync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{"Topic1", "Topic2"}, "ab=12/１２+");
            AddMockResponse(200, new string[]{"Topic3"}, null);
            string groupID = "test_group";
            KiiListResult<KiiTopic> topics = KiiGroup.GroupWithID(groupID).ListTopics();

            Assert.IsTrue(topics.HasNext);
            Assert.AreEqual("ab=12/１２+", topics.PaginationKey);
            Assert.AreEqual(2, topics.Result.Count);
            Assert.AreEqual("Topic1", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("groups", groupID, "topics", "Topic1"), topics.Result[0].Uri.ToString());
            Assert.AreEqual("Topic2", topics.Result[1].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("groups", groupID, "topics", "Topic2"), topics.Result[1].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);

            topics = KiiGroup.GroupWithID(groupID).ListTopics(topics.PaginationKey);
            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(1, topics.Result.Count);
            Assert.AreEqual("Topic3", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("groups", groupID, "topics", "Topic3"), topics.Result[0].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [1]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics") + "?paginationKey=" + Uri.EscapeUriString("ab=12/１２+"), client.RequestUrl [1]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [1]["Authorization"]);
        }
        [Test()]
        public void Test_UserScopeSync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{"Topic1", "Topic2"}, null);
            string userID = "test_user";
            KiiListResult<KiiTopic> topics = KiiUser.UserWithID(userID).ListTopics();

            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(2, topics.Result.Count);
            Assert.AreEqual("Topic1", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("users", userID, "topics", "Topic1"), topics.Result[0].Uri.ToString());
            Assert.AreEqual("Topic2", topics.Result[1].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("users", userID, "topics", "Topic2"), topics.Result[1].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);
        }
        [Test()]
        public void Test_UserScopeWithEmptyResultSync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{}, null);
            string userID = "test_user";
            KiiListResult<KiiTopic> topics = KiiUser.UserWithID(userID).ListTopics();

            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(0, topics.Result.Count);

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);
        }
        [Test()]
        public void Test_UserScopeWithPaginationSync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{"Topic1", "Topic2"}, "ab=12/１２+");
            AddMockResponse(200, new string[]{"Topic3"}, null);
            string userID = "test_user";
            KiiListResult<KiiTopic> topics = KiiUser.UserWithID(userID).ListTopics();

            Assert.IsTrue(topics.HasNext);
            Assert.AreEqual("ab=12/１２+", topics.PaginationKey);
            Assert.AreEqual(2, topics.Result.Count);
            Assert.AreEqual("Topic1", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("users", userID, "topics", "Topic1"), topics.Result[0].Uri.ToString());
            Assert.AreEqual("Topic2", topics.Result[1].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("users", userID, "topics", "Topic2"), topics.Result[1].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);

            topics = KiiUser.UserWithID(userID).ListTopics(topics.PaginationKey);
            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(1, topics.Result.Count);
            Assert.AreEqual("Topic3", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("users", userID, "topics", "Topic3"), topics.Result[0].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [1]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics") + "?paginationKey=" + Uri.EscapeUriString("ab=12/１２+"), client.RequestUrl [1]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [1]["Authorization"]);
        }
        [Test(), ExpectedException(typeof(InvalidOperationException))]
        public void Test_AnonymousSync()
        {
            KiiUser.LogOut();
            AddMockResponse(200, new string[]{}, null);
            Kii.ListTopics();
        }
        [Test(), ExpectedException(typeof(BadRequestException))]
        public void Test_Status400Sync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new BadRequestException("", null, "", BadRequestException.Reason.__UNKNOWN__));
            Kii.ListTopics();
        }
        [Test(), ExpectedException(typeof(UnauthorizedException))]
        public void Test_Status401Sync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new UnauthorizedException("", null, ""));
            Kii.ListTopics();
        }
        [Test(), ExpectedException(typeof(ForbiddenException))]
        public void Test_Status403Sync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new ForbiddenException("", null, ""));
            Kii.ListTopics();
        }
        [Test(), ExpectedException(typeof(InvalidOperationException))]
        public void Test_GroupScopeNoIDSync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{"Topic1", "Topic2"}, null);
            Kii.Group("group").ListTopics();
        }
        [Test(), ExpectedException(typeof(InvalidOperationException))]
        public void Test_UserScopeNoIDSync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{"Topic1", "Topic2"}, null);
            KiiUser.BuilderWithName("user").Build().ListTopics();
        }

        #endregion

        #region ASync
        [Test()]
        public void Test_AppScopeASync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{"Topic1", "Topic2"}, null);
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;

            Kii.ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(2, topics.Result.Count);
            Assert.AreEqual("Topic1", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("topics", "Topic1"), topics.Result[0].Uri.ToString());
            Assert.AreEqual("Topic2", topics.Result[1].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("topics", "Topic2"), topics.Result[1].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);
        }
        [Test()]
        public void Test_AppScopeWithEmptyResultASync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{}, null);
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;

            Kii.ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(0, topics.Result.Count);

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);
        }
        [Test()]
        public void Test_AppScopeWithPaginationASync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{"Topic1", "Topic2"}, "ab=12/１２+");
            AddMockResponse(200, new string[]{"Topic3"}, null);
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;

            Kii.ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.IsTrue(topics.HasNext);
            Assert.AreEqual("ab=12/１２+", topics.PaginationKey);
            Assert.AreEqual(2, topics.Result.Count);
            Assert.AreEqual("Topic1", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("topics", "Topic1"), topics.Result[0].Uri.ToString());
            Assert.AreEqual("Topic2", topics.Result[1].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("topics", "Topic2"), topics.Result[1].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);

            string paginationKey = topics.PaginationKey;
            cd = new CountDownLatch (1);
            topics = null;
            exception = null;

            Kii.ListTopics(paginationKey, (KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(1, topics.Result.Count);
            Assert.AreEqual("Topic3", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("topics", "Topic3"), topics.Result[0].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [1]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics") + "?paginationKey=" + Uri.EscapeUriString("ab=12/１２+"), client.RequestUrl [1]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [1]["Authorization"]);
        }
        [Test()]
        public void Test_GroupScopeASync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{"Topic1", "Topic2"}, null);
            string groupID = "test_group";
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;

            KiiGroup.GroupWithID(groupID).ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(2, topics.Result.Count);
            Assert.AreEqual("Topic1", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("groups", groupID, "topics", "Topic1"), topics.Result[0].Uri.ToString());
            Assert.AreEqual("Topic2", topics.Result[1].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("groups", groupID, "topics", "Topic2"), topics.Result[1].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);
        }
        [Test()]
        public void Test_GroupScopeWithEmptyResultASync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{}, null);
            string groupID = "test_group";
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;

            KiiGroup.GroupWithID(groupID).ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(0, topics.Result.Count);

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);
        }
        [Test()]
        public void Test_GroupScopeWithPaginationASync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{"Topic1", "Topic2"}, "ab=12/１２+");
            AddMockResponse(200, new string[]{"Topic3"}, null);
            string groupID = "test_group";
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;

            KiiGroup.GroupWithID(groupID).ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.IsTrue(topics.HasNext);
            Assert.AreEqual("ab=12/１２+", topics.PaginationKey);
            Assert.AreEqual(2, topics.Result.Count);
            Assert.AreEqual("Topic1", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("groups", groupID, "topics", "Topic1"), topics.Result[0].Uri.ToString());
            Assert.AreEqual("Topic2", topics.Result[1].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("groups", groupID, "topics", "Topic2"), topics.Result[1].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);

            string paginationKey = topics.PaginationKey;
            cd = new CountDownLatch (1);
            topics = null;
            exception = null;

            KiiGroup.GroupWithID(groupID).ListTopics(paginationKey, (KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(1, topics.Result.Count);
            Assert.AreEqual("Topic3", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("groups", groupID, "topics", "Topic3"), topics.Result[0].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [1]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", groupID, "topics") + "?paginationKey=" + Uri.EscapeUriString("ab=12/１２+"), client.RequestUrl [1]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [1]["Authorization"]);
        }
        [Test()]
        public void Test_UserScopeASync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{"Topic1", "Topic2"}, null);
            string userID = "test_user";
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;

            KiiUser.UserWithID(userID).ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(2, topics.Result.Count);
            Assert.AreEqual("Topic1", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("users", userID, "topics", "Topic1"), topics.Result[0].Uri.ToString());
            Assert.AreEqual("Topic2", topics.Result[1].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("users", userID, "topics", "Topic2"), topics.Result[1].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);
        }
        [Test()]
        public void Test_UserScopeWithEmptyResultASync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{}, null);
            string userID = "test_user";
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;

            KiiUser.UserWithID(userID).ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(0, topics.Result.Count);

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);
        }
        [Test()]
        public void Test_UserScopeWithPaginationASync()
        {
            LogIn("test-user-00001");
            AddMockResponse(200, new string[]{"Topic1", "Topic2"}, "ab=12/１２+");
            AddMockResponse(200, new string[]{"Topic3"}, null);
            string userID = "test_user";
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;

            KiiUser.UserWithID(userID).ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.IsTrue(topics.HasNext);
            Assert.AreEqual("ab=12/１２+", topics.PaginationKey);
            Assert.AreEqual(2, topics.Result.Count);
            Assert.AreEqual("Topic1", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("users", userID, "topics", "Topic1"), topics.Result[0].Uri.ToString());
            Assert.AreEqual("Topic2", topics.Result[1].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("users", userID, "topics", "Topic2"), topics.Result[1].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics"), client.RequestUrl [0]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [0]["Authorization"]);

            String paginationKey = topics.PaginationKey;
            cd = new CountDownLatch (1);
            topics = null;
            exception = null;

            KiiUser.UserWithID(userID).ListTopics(paginationKey, (KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(1, topics.Result.Count);
            Assert.AreEqual("Topic3", topics.Result[0].Name);
            Assert.AreEqual("kiicloud://" + Utils.Path("users", userID, "topics", "Topic3"), topics.Result[0].Uri.ToString());

            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [1]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", userID, "topics") + "?paginationKey=" + Uri.EscapeUriString("ab=12/１２+"), client.RequestUrl [1]);
            Assert.AreEqual("Bearer " + KiiUser.AccessToken, client.RequestHeader [1]["Authorization"]);
        }
        [Test()]
        public void Test_AnonymousASync()
        {
            KiiUser.LogOut();
            AddMockResponse(200, new string[]{}, null);
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;
            Kii.ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(topics);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(InvalidOperationException), exception);
        }
        [Test()]
        public void Test_Status400ASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new BadRequestException("", null, "", BadRequestException.Reason.__UNKNOWN__));
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;
            Kii.ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(topics);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(BadRequestException), exception);
            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics"), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_Status401ASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new UnauthorizedException("", null, ""));
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;
            Kii.ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(topics);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(UnauthorizedException), exception);
            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics"), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_Status403ASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new ForbiddenException("", null, ""));
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;
            Kii.ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(topics);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ForbiddenException), exception);
            Assert.AreEqual (KiiHttpMethod.GET, client.RequestMethod [0]);
            Assert.AreEqual (Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "topics"), client.RequestUrl [0]);
        }
        [Test()]
        public void Test_GroupScopeNoIDASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new UnauthorizedException("", null, ""));
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;
            Kii.Group("group").ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(topics);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(InvalidOperationException), exception);
        }
        [Test()]
        public void Test_UserScopeNoIDASync()
        {
            LogIn("test-user-00001");
            client.AddResponse (new UnauthorizedException("", null, ""));
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;
            KiiUser.BuilderWithName("user").Build().ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(topics);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(InvalidOperationException), exception);
        }

        #endregion

        private void AddMockResponse(int httpStatus, string[] topicIDs, string paginationKey)
        {
            JsonObject response = new JsonObject();
            if (!string.IsNullOrEmpty(paginationKey))
            {
                response.Put("paginationKey", paginationKey);
            }
            if (topicIDs == null)
            {
                topicIDs = new string[0];
            }
            JsonArray topics = new JsonArray();
            foreach (String topicID in topicIDs)
            {
                JsonObject topic = new JsonObject();
                topic.Put("topicID", topicID);
                topics.Put(topic);
            }
            response.Put("topics", topics);
            client.AddResponse(httpStatus, response.ToString());
        }
    }
}

