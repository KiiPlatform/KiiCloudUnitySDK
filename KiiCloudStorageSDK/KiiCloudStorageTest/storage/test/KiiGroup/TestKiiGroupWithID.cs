using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{

    [TestFixture()]
    public class TestKiiGroupWithID
    {


        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize(AppConst.APP_ID, AppConst.APP_KEY, AppConst.APP_SITE);
            KiiUser user = KiiUser.UserWithID("user1234");
            SDKTestHack.SetField(Kii.Instance, "mLoginUser", user);
            KiiCloudEngine.UpdateAccessToken("token1234");
        }

        [Test()]
        public void Test_1_1_GroupWithID_ExistsInCloud_Refresh_ChangeName()
        {
            // mock refresh response.
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            MockHttpClient client  = (MockHttpClient)factory.Client;
            client.AddResponse(200, "{" +
                "\"groupID\" : \"dummyId\"," +
                "\"name\" : \"MyGroup\"," +
                "\"owner\" : \"user1234\"" +
                "}");

            // mock changename response
            client.AddResponse(204, null);

            // create group
            string groupId = "dummyId";
            KiiGroup group = KiiGroup.GroupWithID(groupId);
            Assert.IsNull(group.Name);

            // refresh
            group.Refresh();
            Assert.AreEqual("MyGroup", group.Name);

            // change name
            string newGroupName = "MyGroupUpdate";
            group.ChangeName(newGroupName);
            Assert.AreEqual(newGroupName, group.Name);

            // verify changename request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","groups","dummyId", "name");
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("Bearer token1234", client.RequestHeader[0]["Authorization"]);
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
        }

        [Test()]
        public void Test_1_2_GroupWithID_ExistsInCloud_ListMembers()
        {
            // mock list members response.
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            MockHttpClient client  = (MockHttpClient)factory.Client;
            client.AddResponse(200, "{" +
                "\"members\":[{" +
                    "\"userID\" : \"dummyUser\"" +
                "}]" +
            "}");

            // create group
            string groupId = "dummyId";
            KiiGroup group = KiiGroup.GroupWithID(groupId);
            
            // list members
            IList<KiiUser> members = group.ListMembers();
            Assert.AreEqual(1, members.Count);
            
            //check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","groups","dummyId", "members");
            Assert.AreEqual(url, client.RequestUrl[0]);
            Assert.AreEqual(KiiHttpMethod.GET, client.RequestMethod[0]);
            MockHttpHeaderList headerList = client.RequestHeader[0];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("Bearer token1234", client.RequestHeader[0]["Authorization"]);
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
        }

        [Test()]
        public void Test_1_3_GroupWithID_ExistsInCloud_Refresh_AddMembers()
        {
            // mock refresh response
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            MockHttpClient client  = (MockHttpClient)factory.Client;
            client.AddResponse(200, "{" +
                               "\"groupID\" : \"dummyId\"," +
                               "\"name\" : \"MyGroup\"," +
                               "\"owner\" : \"user1234\"" +
                               "}");

            // mock response for adding user
            client.AddResponse(201, "{}");

            // create group
            string groupId = "dummyId";
            KiiGroup group = KiiGroup.GroupWithID(groupId);
            Assert.IsNull(group.Name);
            
            // refresh
            group.Refresh();
            Assert.AreEqual("MyGroup", group.Name);
            
            // add member
            string memberID = TextUtils.generateUUID();
            KiiUser member = KiiUser.UserWithID(memberID);
            group.AddUser(member);

            HashSet<KiiUser> addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            HashSet<KiiUser> removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);

            // Save group
            group.Save();
            
            // Verify add member request
            string requestUrl = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","groups","dummyId", "members", memberID);
            Assert.AreEqual(requestUrl, client.RequestUrl[1]);
            Assert.AreEqual("PUT", client.RequestMethod[1].ToString());
            Assert.AreEqual(AppConst.APP_ID, client.RequestHeader[1]["X-Kii-AppID"]);
            Assert.AreEqual(AppConst.APP_KEY, client.RequestHeader[1]["X-Kii-AppKey"]);
            Assert.AreEqual("Bearer token1234", client.RequestHeader[1]["Authorization"]);
            Assert.AreEqual(null, client.RequestBody[1]);
            
            // Verify local userList get empty.
            Assert.AreEqual(groupId, group.ID);
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(0, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);
        }

        [Test()]
        public void Test_1_4_GroupWithID_ExistsInCloud_Refresh_Delete()
        {
            // mock refresh response.
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            MockHttpClient client  = (MockHttpClient)factory.Client;
            client.AddResponse(200, "{" +
                               "\"groupID\" : \"dummyId\"," +
                               "\"name\" : \"MyGroup\"," +
                               "\"owner\" : \"user1234\"" +
                               "}");

            // mock delete response
            client.AddResponse(204, null);

            // create group.
            string groupId = "dummyId";
            KiiGroup group = KiiGroup.GroupWithID(groupId);
            Assert.IsNull(group.Name);
            
            // refresh
            group.Refresh();
            Assert.AreEqual("MyGroup", group.Name);
            
            // delete
            group.Delete();
            
            //check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","groups","dummyId");
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.DELETE, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("Bearer token1234", client.RequestHeader[0]["Authorization"]);
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
        }

        public void Test_1_5_GroupWithID_ExistsInCloud_Refresh()
        {
            // mock refresh response
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            MockHttpClient client  = (MockHttpClient)factory.Client;
            client.AddResponse(200, "{" +
                               "\"groupID\" : \"dummyId\"," +
                               "\"name\" : \"MyGroup\"," +
                               "\"owner\" : \"user1234\"" +
                               "}");
            // create group.
            string groupId = "dummyId";
            KiiGroup group = KiiGroup.GroupWithID(groupId);
            Assert.IsNull(group.Name);

            // refresh.
            group.Refresh();
            Assert.AreEqual("MyGroup", group.Name);

            //check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","groups","dummyId");
            Assert.AreEqual(url, client.RequestUrl[0]);
            Assert.AreEqual(KiiHttpMethod.GET, client.RequestMethod[0]);
            MockHttpHeaderList headerList = client.RequestHeader[0];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("Bearer token1234", client.RequestHeader[0]["Authorization"]);
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
        }

        [Test()]
        public void Test_1_7_GroupWithID_ExistsInCloud_NotRefresh_AddMembers()
        {
            // mock response for adding user
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            MockHttpClient client  = (MockHttpClient)factory.Client;
            client.AddResponse(201, "{}");

            // create group
            string groupId = "dummyId";
            KiiGroup group = KiiGroup.GroupWithID(groupId);
            
            // add member
            string memberID = TextUtils.generateUUID();
            KiiUser member = KiiUser.UserWithID(memberID);
            group.AddUser(member);
            HashSet<KiiUser> addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            HashSet<KiiUser> removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);

            // Save group
            group.Save();
            
            // Verify request
            string requestUrl = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","groups","dummyId", "members", memberID);
            Assert.AreEqual(requestUrl, client.RequestUrl[0]);
            Assert.AreEqual("PUT", client.RequestMethod[0].ToString());
            IList<MockHttpHeaderList> headers = client.RequestHeader;
            Assert.AreEqual(AppConst.APP_ID, client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual(AppConst.APP_KEY, client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("Bearer token1234", client.RequestHeader[0]["Authorization"]);
            Assert.AreEqual(null, client.RequestBody[0]);
            
            // Verify local list get empty.
            Assert.AreEqual(groupId, group.ID);
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(0, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);
        }

        [Test()]
        public void Test_1_8_GroupWithID_ExistsInCloud_NotRefresh_Delete()
        {
            // mock delete response
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            MockHttpClient client  = (MockHttpClient)factory.Client;
            client.AddResponse(204, null);

            // create group.
            string groupId = "dummyId";
            KiiGroup group = KiiGroup.GroupWithID(groupId);
            Assert.IsNull(group.Name);

            // delete
            group.Delete();
            
            //check delete request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","groups","dummyId");
            Assert.AreEqual(url, client.RequestUrl[0]);
            Assert.AreEqual(KiiHttpMethod.DELETE, client.RequestMethod[0]);
            MockHttpHeaderList headerList = client.RequestHeader[0];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("Bearer token1234", client.RequestHeader[0]["Authorization"]);
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
        }

        [Test()]
        public void Test_1_6_GroupWithID_ExistsInCloud_NotRefresh_ChangeName()
        {
            // mock change name response.
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            MockHttpClient client  = (MockHttpClient)factory.Client;
            client.AddResponse(204, null);

            // create group
            string groupId = "dummyId";
            KiiGroup group = KiiGroup.GroupWithID(groupId);
            Assert.IsNull(group.Name);

            // change name.
            string newGroupName = "MyGroupUpdate";
            group.ChangeName(newGroupName);
            Assert.AreEqual(newGroupName, group.Name);
            
            //check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","groups","dummyId", "name");
            Assert.AreEqual(url, client.RequestUrl[0]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[0]);
            MockHttpHeaderList headerList = client.RequestHeader[0];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
        }

        public void Test_1_11_GroupWithID_NotExistsInCloud_Refresh()
        {
            // mock response
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            MockHttpClient client  = (MockHttpClient)factory.Client;
            client.AddResponse(new CloudException(404, "GROUP_NOT_FOUND"));

            // create group.
            string groupId = "dummyId";
            KiiGroup group = KiiGroup.GroupWithID(groupId);
            Assert.IsNull(group.Name);

            // refresh.
            CloudException exp = null;
            try {
                group.Refresh();
                Assert.Fail("Exception not thrown");
            } catch (CloudException e) {
                exp = e;
            }
            Assert.IsNotNull(exp);
            Assert.AreEqual(404, exp.Status);

            //check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","groups","dummyId");
            Assert.AreEqual(url, client.RequestUrl[0]);
            Assert.AreEqual(KiiHttpMethod.GET, client.RequestMethod[0]);
            MockHttpHeaderList headerList = client.RequestHeader[0];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("Bearer token1234", client.RequestHeader[0]["Authorization"]);
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
        }
        
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_2_1_GroupWithNullID()
        {
            KiiGroup.GroupWithID(null);
        }
        
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_2_2_UserWithEmptyID()
        {
            KiiGroup.GroupWithID(null);
        }
    }
}