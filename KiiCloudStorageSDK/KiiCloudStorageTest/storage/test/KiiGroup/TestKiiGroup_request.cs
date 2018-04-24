using System;
using System.Collections.Generic;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiGroup_request
    {
        private MockHttpClient client;

        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize(AppConst.APP_ID, AppConst.APP_KEY, AppConst.APP_SITE);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

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

        private void SetStandardSaveResponse(MockHttpClient client)
        {
            client.AddResponse(200, "{" +
                "\"groupID\" : \"0c4375c7-16f5-4ce6-9cd3-ec24bc0519e9\"" +
                "}");
        }

        private void SetStandardListResponse(MockHttpClient client)
        {
            client.AddResponse(201, "{" +
                "\"members\" : [" +
                "{\"userID\" : \"e3ef892c-66c8-488e-a928-6e142587b3d7\"}," +
                "{\"userID\" : \"408cee91-84dd-4737-a33c-2941987b2dc5\"}," +
                "{\"userID\" : \"78688ca5-fc71-462b-8e8d-2e9e6120d082\"}," +
                "{\"userID\" : \"a10d16a0-c84c-44bf-9695-43f6268702d4\"} ]}");
        }

        private void SetStandardRefreshResponse(MockHttpClient client)
        {
            client.AddResponse(200, "{" +
                "\"groupID\" : \"90def9aa-565e-4037-bde8-3a8704c7d806\"," +
                "\"name\" : \"testing group\"," +
                "\"owner\" : \"e3137ebe-2874-4d02-b7ef-6780bf8ecc1d\"}");
        }

        #region KiiGroup.Save()
        [Test(), KiiUTInfo(
            action = "When we call Save() with 1 user added and Server returns GroupID,",
            expected = "We can get GroupID that server sends"
            )]
        public void Test_0000_Save ()
        {
            this.LogIn();

            KiiGroup group = Kii.Group("testGroup");

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234"));
            group.AddUser(user);

            // set Response
            this.SetStandardSaveResponse(client);

            group.Save();

            Assert.AreEqual("0c4375c7-16f5-4ce6-9cd3-ec24bc0519e9", group.ID);
        }

        [Test(), ExpectedException(typeof(GroupOperationException)), KiiUTInfo(
            action = "When we call Save() with 1 user added and Server returns HTTP 400,",
            expected = "GroupOperationException must be thrown"
            )]
        public void Test_0001_Save_server_error ()
        {
            this.LogIn();

            KiiGroup group = Kii.Group("testGroup");

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234"));
            group.AddUser(user);

            // set Response
            client.AddResponse(new CloudException(400, "{}"));

            group.Save();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call Save() with 1 user added and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0002_Save_broken_json ()
        {
            this.LogIn();

            KiiGroup group = Kii.Group("testGroup");

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234"));
            group.AddUser(user);

            // set Response
            client.AddResponse(200, "{}");

            group.Save();
        }

        [Test(), KiiUTInfo(
            action = "When we call Save() with 1 user added for updating group,",
            expected = "1 API is called by SDK"
            )]
        public void Test_0010_Save_update_add ()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234"));
            group.AddUser(user);

            // set Response
            client.RequestUrl.Clear();
            this.SetStandardSaveResponse(client);

            group.Save();

            Assert.AreEqual(1, client.RequestUrl.Count);
            Assert.AreEqual("https://api.kii.com/api/apps/appId/groups/group1234/members/user1234", client.RequestUrl[0]);
        }

        [Test(), KiiUTInfo(
            action = "When we call Save() with 2 users added for updating group,",
            expected = "2 API is called by SDK"
            )]
        public void Test_0011_Save_update_add_2 ()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234"));
            group.AddUser(user);

            user = KiiUser.CreateByUri(new Uri("kiicloud://users/user5678"));
            group.AddUser(user);


            // set Response
            client.RequestUrl.Clear();
            this.SetStandardSaveResponse(client);

            group.Save();

            Assert.AreEqual(2, client.RequestUrl.Count);
            Assert.AreEqual("https://api.kii.com/api/apps/appId/groups/group1234/members/user1234", client.RequestUrl[0]);
            Assert.AreEqual("https://api.kii.com/api/apps/appId/groups/group1234/members/user5678", client.RequestUrl[1]);
        }

        [Test(), KiiUTInfo(
            action = "When we call Save() with 1 user deleted for updating group,",
            expected = "1 API is called by SDK"
            )]
        public void Test_0012_Save_update_delete ()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234"));
            group.RemoveUser(user);


            // set Response
            client.RequestUrl.Clear();
            this.SetStandardSaveResponse(client);

            group.Save();

            Assert.AreEqual(1, client.RequestUrl.Count);
            Assert.AreEqual("https://api.kii.com/api/apps/appId/groups/group1234/members/user1234", client.RequestUrl[0]);
        }

        [Test(), KiiUTInfo(
            action = "When we call Save() with 2 users deleted for updating group,",
            expected = "2 API is called by SDK"
            )]
        public void Test_0013_Save_update_delete_2 ()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234"));
            group.RemoveUser(user);

            user = KiiUser.CreateByUri(new Uri("kiicloud://users/user5678"));
            group.RemoveUser(user);

            // set Response
            client.RequestUrl.Clear();
            this.SetStandardSaveResponse(client);

            group.Save();

            Assert.AreEqual(2, client.RequestUrl.Count);
            Assert.AreEqual("https://api.kii.com/api/apps/appId/groups/group1234/members/user1234", client.RequestUrl[0]);
            Assert.AreEqual("https://api.kii.com/api/apps/appId/groups/group1234/members/user5678", client.RequestUrl[1]);
        }

        [Test(), KiiUTInfo(
            action = "When we call Save() with 2 user added for updating group and Server returns HTTP 400 for first call,",
            expected = "We can get 2 AddFailedUsers from Exception"
            )]
        public void Test_0020_Save_update_add_server_error ()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234"));
            group.AddUser(user);

            user = KiiUser.CreateByUri(new Uri("kiicloud://users/user5678"));
            group.AddUser(user);

            // set Response
            client.RequestUrl.Clear();
            client.AddResponse(new CloudException(400, "{}"));

            try
            {
                group.Save();
            }
            catch (GroupOperationException e)
            {
                Assert.AreEqual(2, e.AddFailedUsers.Count);
                Assert.AreEqual(0, e.RemoveFailedUsers.Count);
            }
        }

        [Test(), KiiUTInfo(
            action = "When we call Save() with 2 user added for updating group and Server returns HTTP 400 for second call,",
            expected = "We can get 1 AddFailedUsers from Exception"
            )]
        public void Test_0021_Save_update_add_server_error_partial ()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234"));
            group.AddUser(user);

            user = KiiUser.CreateByUri(new Uri("kiicloud://users/user5678"));
            group.AddUser(user);

            // set Response
            client.RequestUrl.Clear();
            this.SetStandardSaveResponse(client);
            client.AddResponse(new CloudException(400, "{}"));

            try
            {
                group.Save();
            }
            catch (GroupOperationException e)
            {
                Assert.AreEqual(1, e.AddFailedUsers.Count);
                Assert.AreEqual(0, e.RemoveFailedUsers.Count);
            }
        }

        [Test(), KiiUTInfo(
            action = "When we call Save() with 1 user added / 1 user removed for updating group and Server returns HTTP 400 for first call,",
            expected = "We can get 1 AddFailedUsers / 1 RemoveFailedUsers from Exception"
            )]
        public void Test_0022_Save_update_add_remove_server_error ()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234"));
            group.RemoveUser(user);

            user = KiiUser.CreateByUri(new Uri("kiicloud://users/user5678"));
            group.AddUser(user);

            // set Response
            client.RequestUrl.Clear();
            client.AddResponse(new CloudException(400, "{}"));

            try
            {
                group.Save();
            }
            catch (GroupOperationException e)
            {
                Assert.AreEqual(1, e.AddFailedUsers.Count);
                Assert.AreEqual(1, e.RemoveFailedUsers.Count);
            }
        }

        [Test(), KiiUTInfo(
            action = "When we call Save() with 1 user added / 1 user removed for updating group and Server returns HTTP 400 for second call,",
            expected = "We can get 0 AddFailedUsers / 1 RemoveFailedUsers from Exception"
            )]
        public void Test_0023_Save_update_add_remove_server_error_partial ()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234"));
            group.RemoveUser(user);

            user = KiiUser.CreateByUri(new Uri("kiicloud://users/user5678"));
            group.AddUser(user);

            // set Response
            client.RequestUrl.Clear();
            this.SetStandardSaveResponse(client);
            client.AddResponse(new CloudException(400, "{}"));

            try
            {
                group.Save();
            }
            catch (GroupOperationException e)
            {
                Assert.AreEqual(0, e.AddFailedUsers.Count);
                Assert.AreEqual(1, e.RemoveFailedUsers.Count);
            }
        }

        #endregion

        #region KiiGroup.istMembers()
        [Test(), KiiUTInfo(
            action = "When we call ListMembers() and Server returns 4 members,",
            expected = "We can get 4 members"
            )]
        public void Test_0100_ListMembers()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            this.SetStandardListResponse(client);
            IList<KiiUser> members = group.ListMembers();

            Assert.AreEqual(4, members.Count);
        }

        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call ListMembers() to KiiGroup that doesn't have ID,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_0101_ListMembers_NoID()
        {
            this.LogIn();

            KiiGroup group = Kii.Group("newGroup");

            // set Response
            this.SetStandardListResponse(client);
            group.ListMembers();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call ListMembers() and Server returns no member field,",
            expected = "We can get the list that has no entry"
            )]
        public void Test_0102_ListMembers_no_member()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse(200, "{}");
            IList<KiiUser> members = group.ListMembers();

            Assert.AreEqual(0, members.Count);
        }

        [Test(), KiiUTInfo(
            action = "When we call ListMembers() and Server returns empty JsonArray,",
            expected = "We can get the list that has no entry"
            )]
        public void Test_0103_ListMembers_empty_member()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse(200, "{\"members\":[]}");
            IList<KiiUser> members = group.ListMembers();

            Assert.AreEqual(0, members.Count);
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call ListMembers() and Server returns member array that has broken entry,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0104_ListMembers_no_userId()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse(200, "{\"members\":[" +
                "{\"ID\":\"1234\"}" +
                "]}");
            group.ListMembers();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call ListMembers() and Server returns member array that has broken entry(empty userID),",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0105_ListMembers_empty_userId()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse(200, "{\"members\":[" +
                "{\"userID\":\"\"}" +
                "]}");
            group.ListMembers();
        }

        #endregion

        #region KiiGroup.Refresh()

        [Test(), KiiUTInfo(
            action = "When we call Refresh() and Server returns KiiGroup,",
            expected = "We can get groupName and owner URI"
            )]
        public void Test_0200_Refresh()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            this.SetStandardRefreshResponse(client);
            group.Refresh();

            Assert.AreEqual("testing group", group.Name);
            Assert.AreEqual("kiicloud://users/e3137ebe-2874-4d02-b7ef-6780bf8ecc1d", group.Owner.Uri.ToString());

        }

        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call Refresh() to KiiGroup that doesn't have ID,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_0201_Refresh_no_ID()
        {
            this.LogIn();

            KiiGroup group = Kii.Group("newGroup");

            // set Response
            this.SetStandardRefreshResponse(client);
            group.Refresh();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call Refresh() and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0202_Refresh_broken_json()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse(200, "broken");
            group.Refresh();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call Refresh() and Server returns Json that doesn't have groupID,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0203_Refresh_broken_no_groupId()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse(200, "{" +
                "\"name\" : \"testing group\"," +
                "\"owner\" : \"e3137ebe-2874-4d02-b7ef-6780bf8ecc1d\"}");

            group.Refresh();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call Refresh() and Server returns Json that doesn't have name,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0204_Refresh_broken_no_name()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse(200, "{" +
                "\"groupID\" : \"90def9aa-565e-4037-bde8-3a8704c7d806\"," +
                "\"owner\" : \"e3137ebe-2874-4d02-b7ef-6780bf8ecc1d\"}");

            group.Refresh();
        }

        [Test(), KiiUTInfo(
            action = "When we call Refresh() and Server returns Json that doesn't have owner,",
            expected = "We can get groupName and ID, but owner is null"
            )]
        public void Test_0205_Refresh_broken_no_owner()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse(200, "{" +
                "\"groupID\" : \"90def9aa-565e-4037-bde8-3a8704c7d806\"," +
                "\"name\" : \"testing group\"}");
            group.Refresh();

            Assert.AreEqual("testing group", group.Name);
            Assert.AreEqual("90def9aa-565e-4037-bde8-3a8704c7d806", group.ID);
            Assert.IsNull(group.Owner);
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call Refresh() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0206_Refresh_server_error()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse(new CloudException(400, "{}"));
            group.Refresh();
        }


        #endregion

        #region KiiGroup.Delete()

        [Test(), KiiUTInfo(
            action = "When we call Delete() and Server returns HTTP 204(OK),",
            expected = "Uri must be null after Deletion"
            )]
        public void Test_0300_Delete()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse(204, "");
            group.Delete();

            Assert.IsNull(group.Uri);
        }

        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call Delete() to KiiGroup that doesn't have ID,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_0301_Delete_no_ID()
        {
            this.LogIn();

            KiiGroup group = Kii.Group("newGroup");

            // set Response
            client.AddResponse(204, "");
            group.Delete();
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call Delete() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0302_Delete_server_error()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse(new CloudException(400, "{}"));
            group.Delete();
        }

        #endregion

        #region KiiGroup.ChangeName(string)

        [Test(), KiiUTInfo(
            action = "When we call ChangeName() and Server returns HTTP 204(OK),",
            expected = "We can get updated name by group.Name"
            )]
        public void Test_0400_ChangeName()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse(204, "");
            group.ChangeName("newName");

            Assert.AreEqual("newName", group.Name);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call ChangeName() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0401_ChangeName_null()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse(204, "");
            group.ChangeName(null);
        }

        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call ChangeName() to KiiGroup that doesn't have ID,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_0402_ChangeName_no_id()
        {
            this.LogIn();

            KiiGroup group = Kii.Group("groupName");

            // set Response
            client.AddResponse(204, "");
            group.ChangeName("newGroupName");
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call ChangeName() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0403_ChangeName_server_error()
        {
            this.LogIn();

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse(new CloudException(400, "{}"));
            group.ChangeName("newGroupName");
        }
        #endregion

        #region KiiGroup.add/removeUser : CMO-5597

        [Test()]
        public void Test_0501_CreateGroup_without_member()
        {
            // Create test user for group owner
            string userID = TextUtils.generateUUID();
            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/" + userID));
            SDKTestHack.SetField(Kii.Instance, "mLoginUser", user);
            string authToken = TextUtils.generateUUID();
            KiiCloudEngine.UpdateAccessToken(authToken);
            
            // Prepare group
            string groupName = TextUtils.randomAlphaNumeric(10);
            KiiGroup group = Kii.Group(groupName);

            // Check value
            HashSet<KiiUser> addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            HashSet<KiiUser> removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(0, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);

            // Prepare mock response for creating group
            string groupID = TextUtils.randomAlphaNumeric(26);
            JsonObject resEntity = new JsonObject(new Dictionary<string, object>() {
                { "groupID", groupID },
                { "notFoundUsers", new JsonArray() }
            });
            client.AddResponse(201, resEntity.ToString());
            
            // Save group
            group.Save();

            // Verify request
            Assert.AreEqual(1, client.RequestUrl.Count);
            Assert.AreEqual(1, client.RequestHeader.Count);
            string requestUrl = AppUtil.getUrlOfSite(AppConst.APP_SITE, AppConst.APP_ID, "groups");
            Assert.AreEqual(requestUrl, client.RequestUrl[0]);
            Assert.AreEqual("POST", client.RequestMethod[0].ToString());
            IList<MockHttpHeaderList> headers = client.RequestHeader;
            Assert.AreEqual(AppConst.APP_ID, client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual(AppConst.APP_KEY, client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("Bearer " + authToken, client.RequestHeader[0]["Authorization"]);

            JsonObject requestBody = new JsonObject(client.RequestBody[0]);
            Assert.AreEqual(3, requestBody.Length());
            Assert.AreEqual(groupName, requestBody.GetString("name"));
            Assert.AreEqual(userID, requestBody.GetString("owner"));
            Assert.AreEqual(0, requestBody.GetJsonArray("members").Length());

            // Verify
            Assert.AreEqual(groupID, group.ID);
            Assert.AreEqual(user.ID, group.Owner.ID);

            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(0, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);
        }

        [Test()]
        public void Test_0502_CreateGroup_with_member()
        {
            // Create test user for group owner
            string userID = TextUtils.generateUUID();
            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/" + userID));
            SDKTestHack.SetField(Kii.Instance, "mLoginUser", user);
            string authToken = TextUtils.generateUUID();
            KiiCloudEngine.UpdateAccessToken(authToken);

            // Prepare KiiUser for member
            string memberID = TextUtils.generateUUID();
            KiiUser member = KiiUser.CreateByUri(new Uri("kiicloud://users/" + memberID));

            // Prepare group
            string groupName = TextUtils.randomAlphaNumeric(10);
            KiiGroup group = Kii.Group(groupName, new List<KiiUser>(){ member });

            // Check value
            HashSet<KiiUser> addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            HashSet<KiiUser> removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);

            // Prepare mock response for creating group
            string groupID = TextUtils.randomAlphaNumeric(26);
            JsonObject resEntity = new JsonObject(new Dictionary<string, object>() {
                { "groupID", groupID },
                { "notFoundUsers", new JsonArray() }
            });
            client.AddResponse(201, resEntity.ToString());

            // Save group
            group.Save();

            // Verify request
            Assert.AreEqual(1, client.RequestUrl.Count);
            Assert.AreEqual(1, client.RequestHeader.Count);
            string requestUrl = AppUtil.getUrlOfSite(AppConst.APP_SITE, AppConst.APP_ID, "groups");
            Assert.AreEqual(requestUrl, client.RequestUrl[0]);
            Assert.AreEqual("POST", client.RequestMethod[0].ToString());
            IList<MockHttpHeaderList> headers = client.RequestHeader;
            Assert.AreEqual(AppConst.APP_ID, client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual(AppConst.APP_KEY, client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("Bearer " + authToken, client.RequestHeader[0]["Authorization"]);

            JsonObject requestBody = new JsonObject(client.RequestBody[0]);
            Assert.AreEqual(3, requestBody.Length());
            Assert.AreEqual(groupName, requestBody.GetString("name"));
            Assert.AreEqual(userID, requestBody.GetString("owner"));
            Assert.AreEqual(1, requestBody.GetJsonArray("members").Length());
            Assert.AreEqual(memberID, requestBody.GetJsonArray("members").GetString(0));

            // Verify
            Assert.AreEqual(groupID, group.ID);
            Assert.AreEqual(user.ID, group.Owner.ID);

            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(0, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);
        }

        [Test()]
        public void Test_0503_add_member()
        {
            // Create test user for group owner
            string userID = TextUtils.generateUUID();
            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/" + userID));
            SDKTestHack.SetField(Kii.Instance, "mLoginUser", user);
            string authToken = TextUtils.generateUUID();
            KiiCloudEngine.UpdateAccessToken(authToken);

            // Prepare group
            string groupName = TextUtils.randomAlphaNumeric(10);
            string groupID = TextUtils.randomAlphaNumeric(26);
            KiiGroup group = Kii.Group(groupName);
            SDKTestHack.SetField(group, "mId", groupID);
            SDKTestHack.SetField(group, "mOwnerId", userID);

            // Prepare KiiUser for member
            string memberID = TextUtils.generateUUID();
            KiiUser member = KiiUser.CreateByUri(new Uri("kiicloud://users/" + memberID));

            group.AddUser(member);

            // Check value
            HashSet<KiiUser> addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            HashSet<KiiUser> removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);

            // Prepare mock response for adding user
            client.AddResponse(204, "");

            // Save group
            group.Save();

            // Verify request
            Assert.AreEqual(1, client.RequestUrl.Count);
            Assert.AreEqual(1, client.RequestHeader.Count);
            string requestUrl = AppUtil.getUrlOfSite(AppConst.APP_SITE, AppConst.APP_ID, "groups", groupID, "members", memberID);
            Assert.AreEqual(requestUrl, client.RequestUrl[0]);
            Assert.AreEqual("PUT", client.RequestMethod[0].ToString());
            IList<MockHttpHeaderList> headers = client.RequestHeader;
            Assert.AreEqual(AppConst.APP_ID, client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual(AppConst.APP_KEY, client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("Bearer " + authToken, client.RequestHeader[0]["Authorization"]);
            Assert.AreEqual(null, client.RequestBody[0]);

            // Verify
            Assert.AreEqual(groupID, group.ID);

            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(0, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);
        }

        [Test()]
        public void Test_0504_remove_member()
        {
            // Create test user for group owner
            string userID = TextUtils.generateUUID();
            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/" + userID));
            SDKTestHack.SetField(Kii.Instance, "mLoginUser", user);
            string authToken = TextUtils.generateUUID();
            KiiCloudEngine.UpdateAccessToken(authToken);

            // Prepare group
            string groupName = TextUtils.randomAlphaNumeric(10);
            string groupID = TextUtils.randomAlphaNumeric(26);
            KiiGroup group = Kii.Group(groupName);
            SDKTestHack.SetField(group, "mId", groupID);
            SDKTestHack.SetField(group, "mOwnerId", userID);

            // Prepare KiiUser for member
            string memberID = TextUtils.generateUUID();
            KiiUser member = KiiUser.CreateByUri(new Uri("kiicloud://users/" + memberID));

            group.RemoveUser(member);

            // Check value
            HashSet<KiiUser> addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            HashSet<KiiUser> removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(0, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);

            // Prepare mock response for adding user
            client.AddResponse(204, "");

            // Save group
            group.Save();

            // Verify request
            Assert.AreEqual(1, client.RequestUrl.Count);
            Assert.AreEqual(1, client.RequestHeader.Count);
            string requestUrl = AppUtil.getUrlOfSite(AppConst.APP_SITE, AppConst.APP_ID, "groups", groupID, "members", memberID);
            Assert.AreEqual(requestUrl, client.RequestUrl[0]);
            Assert.AreEqual("DELETE", client.RequestMethod[0].ToString());
            IList<MockHttpHeaderList> headers = client.RequestHeader;
            Assert.AreEqual(AppConst.APP_ID, client.RequestHeader[0]["X-Kii-AppID"]);
            Assert.AreEqual(AppConst.APP_KEY, client.RequestHeader[0]["X-Kii-AppKey"]);
            Assert.AreEqual("Bearer " + authToken, client.RequestHeader[0]["Authorization"]);
            Assert.AreEqual(null, client.RequestBody[0]);

            // Verify
            Assert.AreEqual(groupID, group.ID);

            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(0, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);
        }

        [Test()]
        public void Test_0505_AddAndRemoveUser_same_KiiUser_instance()
        {
            // Prepare KiiUser for member
            KiiUser memberUser1 = KiiUser.CreateByUri(new Uri("kiicloud://users/" + TextUtils.generateUUID()));
            KiiUser memberUser2 = KiiUser.CreateByUri(new Uri("kiicloud://users/" + TextUtils.generateUUID()));
            KiiUser memberUser3 = KiiUser.CreateByUri(new Uri("kiicloud://users/" + TextUtils.generateUUID()));

            // Prepare group
            string groupName = TextUtils.randomAlphaNumeric(10);
            string groupID = TextUtils.randomAlphaNumeric(26);
            KiiGroup group = Kii.Group(groupName);
            SDKTestHack.SetField(group, "mId", groupID);
            SDKTestHack.SetField(group, "mOwnerId", TextUtils.generateUUID());
            group.AddUser(memberUser1);
            group.RemoveUser(memberUser2);
            // Check value
            HashSet<KiiUser> addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            HashSet<KiiUser> removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);

            // Add user
            group.AddUser(memberUser3);
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(2, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);

            // Add user again
            group.AddUser(memberUser3);
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(2, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);

            // Remove user
            group.RemoveUser(memberUser3);
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(2, removeUsers.Count);

            // Remove user again
            group.RemoveUser(memberUser3);
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(2, removeUsers.Count);

            // Add user
            group.AddUser(memberUser3);
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(2, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);
        }

        [Test()]
        public void Test_0506_AddAndRemoveUser_different_KiiUser_instance()
        {
            // Prepare KiiUser for member
            KiiUser memberUser1 = KiiUser.CreateByUri(new Uri("kiicloud://users/" + TextUtils.generateUUID()));
            KiiUser memberUser2 = KiiUser.CreateByUri(new Uri("kiicloud://users/" + TextUtils.generateUUID()));
            KiiUser memberUser3 = KiiUser.CreateByUri(new Uri("kiicloud://users/" + TextUtils.generateUUID()));

            // Prepare group
            string groupName = TextUtils.randomAlphaNumeric(10);
            string groupID = TextUtils.randomAlphaNumeric(26);
            KiiGroup group = Kii.Group(groupName);
            SDKTestHack.SetField(group, "mId", groupID);
            SDKTestHack.SetField(group, "mOwnerId", TextUtils.generateUUID());
            group.AddUser(memberUser1);
            group.RemoveUser(memberUser2);
            // Check value
            HashSet<KiiUser> addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            HashSet<KiiUser> removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);

            // Add user
            group.AddUser(memberUser3);
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(2, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);

            // Add user again
            KiiUser memberUser3a = KiiUser.CreateByUri(memberUser3.Uri);
            group.AddUser(memberUser3a);
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(2, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);

            // Remove user
            group.RemoveUser(memberUser1);
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(2, removeUsers.Count);

            // Remove user again
            KiiUser memberUser1a = KiiUser.CreateByUri(memberUser1.Uri);
            group.RemoveUser(memberUser1a);
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(2, removeUsers.Count);

            // Add user
            KiiUser memberUser2a = KiiUser.CreateByUri(memberUser2.Uri);
            group.AddUser(memberUser2a);
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(2, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);
        }

        [Test()]
        public void Test_0507_AddAndRemoveUser_notsaved_KiiUser_instance()
        {
            // Prepare KiiUser for member
            KiiUser memberUser1 = KiiUser.CreateByUri(new Uri("kiicloud://users/" + TextUtils.generateUUID()));
            KiiUser memberUser2 = KiiUser.CreateByUri(new Uri("kiicloud://users/" + TextUtils.generateUUID()));
            KiiUser notSavedUser = KiiUser.BuilderWithName(TextUtils.randomAlphaNumeric(10)).Build();

            // Prepare group
            string groupName = TextUtils.randomAlphaNumeric(10);
            string groupID = TextUtils.randomAlphaNumeric(26);
            KiiGroup group = Kii.Group(groupName);
            SDKTestHack.SetField(group, "mId", groupID);
            SDKTestHack.SetField(group, "mOwnerId", TextUtils.generateUUID());
            group.AddUser(memberUser1);
            group.RemoveUser(memberUser2);
            // Check value
            HashSet<KiiUser> addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            HashSet<KiiUser> removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);

            // Add user
            try
            {
                group.AddUser(notSavedUser);
                Assert.Fail("Should throw ArgumentException");
            }
            catch (ArgumentException)
            {
                // Pass
            }
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);

            // Remove user
            try
            {
                group.RemoveUser(notSavedUser);
                Assert.Fail("Should throw ArgumentException");
            }
            catch (ArgumentException)
            {
                // Pass
            }
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);
        }

        [Test()]
        public void Test_0508_AddAndRemoveUser_null_KiiUser_instance()
        {
            // Prepare KiiUser for member
            KiiUser memberUser1 = KiiUser.CreateByUri(new Uri("kiicloud://users/" + TextUtils.generateUUID()));
            KiiUser memberUser2 = KiiUser.CreateByUri(new Uri("kiicloud://users/" + TextUtils.generateUUID()));
            KiiUser nullUser = null;

            // Prepare group
            string groupName = TextUtils.randomAlphaNumeric(10);
            string groupID = TextUtils.randomAlphaNumeric(26);
            KiiGroup group = Kii.Group(groupName);
            SDKTestHack.SetField(group, "mId", groupID);
            SDKTestHack.SetField(group, "mOwnerId", TextUtils.generateUUID());
            group.AddUser(memberUser1);
            group.RemoveUser(memberUser2);
            // Check value
            HashSet<KiiUser> addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            HashSet<KiiUser> removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);

            // Add user
            try
            {
                group.AddUser(nullUser);
                Assert.Fail("Should throw ArgumentException");
            }
            catch (ArgumentException)
            {
                // Pass
            }
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);

            // Remove user
            try
            {
                group.RemoveUser(nullUser);
                Assert.Fail("Should throw ArgumentException");
            }
            catch (ArgumentException)
            {
                // Pass
            }
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(1, removeUsers.Count);
        }

        [Test()]
        public void Test_0509_AddAndRemoveUser_notsaved_group()
        {
            // Prepare KiiUser for member
            KiiUser memberUser1 = KiiUser.CreateByUri(new Uri("kiicloud://users/" + TextUtils.generateUUID()));
            KiiUser memberUser2 = KiiUser.CreateByUri(new Uri("kiicloud://users/" + TextUtils.generateUUID()));

            // Prepare group
            string groupName = TextUtils.randomAlphaNumeric(10);
            string groupID = TextUtils.randomAlphaNumeric(26);
            KiiGroup group = Kii.Group(groupName);
            // Check value
            HashSet<KiiUser> addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            HashSet<KiiUser> removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(0, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);

            // Add user
            group.AddUser(memberUser1);
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);

            // Remove user
            group.RemoveUser(memberUser2);
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(1, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);

            // Remove user for user1
            group.RemoveUser(memberUser1);
            // Check value
            addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(0, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);
        }

        #endregion
    }
}

