using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestRegisterGroupWithID
    {
        private MockHttpClient client;
        private string AppID = "aTpEpSiTd";
        private string AppKey = "aTpEpSkTey";

        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize(AppID, AppKey, Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            Kii.AsyncHttpClientFactory = factory;
            client = (MockHttpClient)factory.Client;
            LogIn(Guid.NewGuid().ToString());
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

        private string GetGroupID()
        {
            return "my-group";
        }

        #region Sync
        [Test(), KiiUTInfo(
            action = "When we call KiiGroup.RegisterGroupWithID()",
            expected = "HTTP request should be sent to server as expected.")]
        public void RegisterGroupWithIDSyncTest()
        {
            string groupID = GetGroupID();
            string groupName = "group-" + DateTime.Now.Ticks.ToString(); 
            List<KiiUser> members = new List<KiiUser>();
            members.Add(KiiUser.CreateByUri(new Uri("kiicloud://users/user-member-0001")));
            members.Add(KiiUser.CreateByUri(new Uri("kiicloud://users/user-member-0002")));

            client.AddResponse(200, "{\"groupID\":\"" + groupID + "\"}");

            KiiGroup group = KiiGroup.RegisterGroupWithID(groupID, groupName, members);

            Assert.AreEqual(groupID, group.ID);
            Assert.AreEqual(groupName, group.Name);
            Assert.AreEqual(KiiUser.CurrentUser.ID, group.Owner.ID);

            JsonObject expectedRequestBody = new JsonObject();
            JsonArray expectedMembers = new JsonArray();
            expectedMembers.Put("user-member-0001");
            expectedMembers.Put("user-member-0002");
            expectedRequestBody.Put("owner", KiiUser.CurrentUser.ID);
            expectedRequestBody.Put("name", groupName);
            expectedRequestBody.Put("members", expectedMembers);

            Assert.AreEqual (KiiHttpMethod.PUT, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/" + AppID + "/groups/" + groupID, client.RequestUrl [0]);
            KiiAssertion.AssertJson(expectedRequestBody, new JsonObject(client.RequestBody [0]));
            Assert.AreEqual("application/vnd.kii.GroupCreationRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call KiiGroup.RegisterGroupWithID() without members",
            expected = "HTTP request should be sent to server as expected.")]
        public void RegisterGroupWithIDWithoutMembersSyncTest()
        {
            string groupID = GetGroupID();
            string groupName = "group-" + DateTime.Now.Ticks.ToString(); 

            client.AddResponse(200, "{\"groupID\":\"" + groupID + "\"}");

            KiiGroup group = KiiGroup.RegisterGroupWithID(groupID, groupName, null);

            Assert.AreEqual(groupID, group.ID);
            Assert.AreEqual(groupName, group.Name);
            Assert.AreEqual(KiiUser.CurrentUser.ID, group.Owner.ID);

            JsonObject expectedRequestBody = new JsonObject();
            JsonArray expectedMembers = new JsonArray();
            expectedMembers.Put("user-member-0001");
            expectedMembers.Put("user-member-0002");
            expectedRequestBody.Put("owner", KiiUser.CurrentUser.ID);
            expectedRequestBody.Put("name", groupName);

            Assert.AreEqual (KiiHttpMethod.PUT, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/" + AppID + "/groups/" + groupID, client.RequestUrl [0]);
            KiiAssertion.AssertJson(expectedRequestBody, new JsonObject(client.RequestBody [0]));
            Assert.AreEqual("application/vnd.kii.GroupCreationRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call KiiGroup.RegisterGroupWithID() without groupID",
            expected = "ArgumentException should be thrown.")]
        public void RegisterGroupWithIDWithoutGroupIDSyncTest()
        {
            string groupName = "group-" + DateTime.Now.Ticks.ToString(); 
            KiiGroup.RegisterGroupWithID(null, groupName, null);
        }
        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call KiiGroup.RegisterGroupWithID() without group name",
            expected = "ArgumentException should be thrown.")]
        public void RegisterGroupWithIDWithoutGroupNameSyncTest()
        {
            string groupID = GetGroupID();
            KiiGroup.RegisterGroupWithID(groupID, null, null);
        }
        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call KiiGroup.RegisterGroupWithID() with invalid ID",
            expected = "ArgumentException should be thrown.")]
        public void RegisterGroupWithIDWithInvalidIDSyncTest()
        {
            string groupID = "aaa-bbb-@";
            string groupName = "group-" + DateTime.Now.Ticks.ToString(); 
            KiiGroup.RegisterGroupWithID(groupID, groupName, null);
        }
        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call KiiGroup.RegisterGroupWithID() when user not logged in",
            expected = "InvalidOperationException should be thrown.")]
        public void RegisterGroupWithIDWhenNotLoggedinSyncTest()
        {
            KiiUser.LogOut();
            string groupID = GetGroupID();
            string groupName = "group-" + DateTime.Now.Ticks.ToString(); 
            KiiGroup.RegisterGroupWithID(groupID, groupName, null);
        }
        [Test(), ExpectedException(typeof(GroupOperationException)), KiiUTInfo(
            action = "When we call KiiGroup.RegisterGroupWithID() with group id that already exists",
            expected = "GroupOperationException  should be thrown.")]
        public void RegisterGroupWithIDConfrictSyncTest()
        {
            string groupID = GetGroupID();
            string groupName = "group-" + DateTime.Now.Ticks.ToString(); 

            client.AddResponse(new ConflictException("", null, "", ConflictException.Reason.GROUP_ALREADY_EXISTS));

            KiiGroup.RegisterGroupWithID(groupID, groupName, null);
        }
        #endregion

        #region ASync
        [Test(), KiiUTInfo(
            action = "When we call KiiGroup.RegisterGroupWithID()",
            expected = "HTTP request should be sent to server as expected.")]
        public void RegisterGroupWithIDASyncTest()
        {
            string groupID = GetGroupID();
            string groupName = "group-" + DateTime.Now.Ticks.ToString(); 
            List<KiiUser> members = new List<KiiUser>();
            members.Add(KiiUser.CreateByUri(new Uri("kiicloud://users/user-member-0001")));
            members.Add(KiiUser.CreateByUri(new Uri("kiicloud://users/user-member-0002")));

            client.AddResponse(200, "{\"groupID\":\"" + groupID + "\"}");

            CountDownLatch cd = new CountDownLatch(1);
            KiiGroup group = null;
            Exception exception = null;

            KiiGroup.RegisterGroupWithID(groupID, groupName, members, (KiiGroup result, Exception e)=>{
                group = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);
          
            Assert.AreEqual(groupID, group.ID);
            Assert.AreEqual(groupName, group.Name);
            Assert.AreEqual(KiiUser.CurrentUser.ID, group.Owner.ID);

            JsonObject expectedRequestBody = new JsonObject();
            JsonArray expectedMembers = new JsonArray();
            expectedMembers.Put("user-member-0001");
            expectedMembers.Put("user-member-0002");
            expectedRequestBody.Put("owner", KiiUser.CurrentUser.ID);
            expectedRequestBody.Put("name", groupName);
            expectedRequestBody.Put("members", expectedMembers);

            Assert.AreEqual (KiiHttpMethod.PUT, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/" + AppID + "/groups/" + groupID, client.RequestUrl [0]);
            KiiAssertion.AssertJson(expectedRequestBody, new JsonObject(client.RequestBody [0]));
            Assert.AreEqual("application/vnd.kii.GroupCreationRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call KiiGroup.RegisterGroupWithID() without members",
            expected = "HTTP request should be sent to server as expected.")]
        public void RegisterGroupWithIDWithoutMembersASyncTest()
        {
            string groupID = GetGroupID();
            string groupName = "group-" + DateTime.Now.Ticks.ToString(); 

            client.AddResponse(200, "{\"groupID\":\"" + groupID + "\"}");

            CountDownLatch cd = new CountDownLatch(1);
            KiiGroup group = null;
            Exception exception = null;

            KiiGroup.RegisterGroupWithID(groupID, groupName, null, (KiiGroup result, Exception e)=>{
                group = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);

            Assert.AreEqual(groupID, group.ID);
            Assert.AreEqual(groupName, group.Name);
            Assert.AreEqual(KiiUser.CurrentUser.ID, group.Owner.ID);

            JsonObject expectedRequestBody = new JsonObject();
            JsonArray expectedMembers = new JsonArray();
            expectedMembers.Put("user-member-0001");
            expectedMembers.Put("user-member-0002");
            expectedRequestBody.Put("owner", KiiUser.CurrentUser.ID);
            expectedRequestBody.Put("name", groupName);

            Assert.AreEqual (KiiHttpMethod.PUT, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/" + AppID + "/groups/" + groupID, client.RequestUrl [0]);
            KiiAssertion.AssertJson(expectedRequestBody, new JsonObject(client.RequestBody [0]));
            Assert.AreEqual("application/vnd.kii.GroupCreationRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call KiiGroup.RegisterGroupWithID() without groupID",
            expected = "ArgumentException should be passed via callback.")]
        public void RegisterGroupWithIDWithoutGroupIDASyncTest()
        {
            string groupName = "group-" + DateTime.Now.Ticks.ToString(); 
            CountDownLatch cd = new CountDownLatch(1);
            KiiGroup group = null;
            Exception exception = null;

            KiiGroup.RegisterGroupWithID(null, groupName, null, (KiiGroup result, Exception e)=>{
                group = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(group);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test(), KiiUTInfo(
            action = "When we call KiiGroup.RegisterGroupWithID() without group name",
            expected = "ArgumentException should be passed via callback.")]
        public void RegisterGroupWithIDWithoutGroupNameASyncTest()
        {
            string groupID = GetGroupID();
            CountDownLatch cd = new CountDownLatch(1);
            KiiGroup group = null;
            Exception exception = null;

            KiiGroup.RegisterGroupWithID(groupID, null, null, (KiiGroup result, Exception e)=>{
                group = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(group);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test(), KiiUTInfo(
            action = "When we call KiiGroup.RegisterGroupWithID() with invalid ID",
            expected = "ArgumentException should be passed via callback.")]
        public void RegisterGroupWithIDWithInvalidIDASyncTest()
        {
            string groupID = "aaa-bbb-$";
            string groupName = "group-" + DateTime.Now.Ticks.ToString(); 
            CountDownLatch cd = new CountDownLatch(1);
            KiiGroup group = null;
            Exception exception = null;

            KiiGroup.RegisterGroupWithID(groupID, groupName, null, (KiiGroup result, Exception e)=>{
                group = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(group);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(ArgumentException), exception);
        }
        [Test(), KiiUTInfo(
            action = "When we call KiiGroup.RegisterGroupWithID() when user not logged in",
            expected = "InvalidOperationException should be passed via callback.")]
        public void RegisterGroupWithIDWhenNotLoggedinASyncTest()
        {
            KiiUser.LogOut();
            string groupID = GetGroupID();
            string groupName = "group-" + DateTime.Now.Ticks.ToString(); 
            CountDownLatch cd = new CountDownLatch(1);
            KiiGroup group = null;
            Exception exception = null;

            KiiGroup.RegisterGroupWithID(groupID, groupName, null, (KiiGroup result, Exception e)=>{
                group = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(group);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(InvalidOperationException), exception);
        }
        [Test(), KiiUTInfo(
            action = "When we call KiiGroup.RegisterGroupWithID() with group id that already exists",
            expected = "GroupOperationException should be passed via callback.")]
        public void RegisterGroupWithIDConfrictASyncTest()
        {
            string groupID = GetGroupID();
            string groupName = "group-" + DateTime.Now.Ticks.ToString(); 

            client.AddResponse(new ConflictException("", null, "", ConflictException.Reason.GROUP_ALREADY_EXISTS));

            CountDownLatch cd = new CountDownLatch(1);
            KiiGroup group = null;
            Exception exception = null;

            KiiGroup.RegisterGroupWithID(groupID, groupName, null, (KiiGroup result, Exception e)=>{
                group = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(group);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(GroupOperationException), exception);
        }

        #endregion

        [Test()]
        public void ValidateGroupIDTest()
        {
            Dictionary<string, bool> testPatterns = new Dictionary<string, bool>();
            testPatterns.Add("a", true);
            testPatterns.Add("A", false);
            testPatterns.Add("ab-012_zw.987", true);
            testPatterns.Add("123456789-123456789-123456789-", true);
            testPatterns.Add("123456789-123456789-123456789-1", false);
            testPatterns.Add("", false);
            testPatterns.Add("abc@", false);
            testPatterns.Add("123*", false);
            foreach (KeyValuePair<string, bool> testPattern in testPatterns)
            {
                Assert.AreEqual(testPattern.Value, Utils.ValidateGroupID(testPattern.Key));
            }
        }
    }
}

