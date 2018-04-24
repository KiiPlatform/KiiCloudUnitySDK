using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiGroup_request_async
    {
        private MockHttpClient client;

        [SetUp()]
        public void SetUp ()
        {
            Kii.Initialize ("appId", "appKey", Kii.Site.US);

            // This test is Unit test so we use blocking mock client
            MockHttpClientFactory factory = new MockHttpClientFactory ();
            Kii.AsyncHttpClientFactory = factory;

            client = factory.Client;
        }

        [TearDown()]
        public void TearDown ()
        {
            KiiUser.LogOut ();
        }

        private void LogIn ()
        {
            // set Response
            client.AddResponse (200, "{" +
                "\"id\" : \"user1234\"," +
                "\"access_token\" : \"cdef\"," +
                "\"expires_in\" : 9223372036854775}");
            KiiUser target = null;
            Exception exception = null;
            KiiUser.LogIn ("kii1234", "pass1234", (KiiUser user, Exception e) => {
                target = user;
                exception = e;
            });
            Assert.AreEqual ("user1234", target.ID);
            Assert.IsNull (exception);
        }

        private void SetStandardSaveResponse ()
        {
            client.AddResponse (200, "{" +
                "\"groupID\" : \"0c4375c7-16f5-4ce6-9cd3-ec24bc0519e9\"" +
                "}");
        }

        private void SetStandardListResponse ()
        {
            client.AddResponse (201, "{" +
                "\"members\" : [" +
                "{\"userID\" : \"e3ef892c-66c8-488e-a928-6e142587b3d7\"}," +
                "{\"userID\" : \"408cee91-84dd-4737-a33c-2941987b2dc5\"}," +
                "{\"userID\" : \"78688ca5-fc71-462b-8e8d-2e9e6120d082\"}," +
                "{\"userID\" : \"a10d16a0-c84c-44bf-9695-43f6268702d4\"} ]}");
        }

        private void SetStandardRefreshResponse ()
        {
            client.AddResponse (200, "{" +
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
            this.LogIn ();

            KiiGroup group = Kii.Group ("testGroup");

            KiiUser user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user1234"));
            group.AddUser (user);

            // set Response
            this.SetStandardSaveResponse ();

            bool done = false;
            KiiGroup savedGroup = null;
            Exception savedException = null;
            group.Save ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                savedGroup = retGroup;
                savedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNull (savedException);
            Assert.AreEqual ("0c4375c7-16f5-4ce6-9cd3-ec24bc0519e9", savedGroup.ID);
        }

        [Test(), KiiUTInfo(
                action = "When we call Save() with 1 user added and Server returns HTTP 400,",
                expected = "GroupOperationException must be passed in the callback"
                )]
        public void Test_0001_Save_server_error ()
        {
            this.LogIn ();

            KiiGroup group = Kii.Group ("testGroup");

            KiiUser user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user1234"));
            group.AddUser (user);

            // set Response
            client.AddResponse (new CloudException (400, "{ \"errorCode\" : \"INVALID_INPUT_DATA\", \"message\" : \"There are validation errors\", \"suppressed\" : [ ]}"));

            bool done = false;
            KiiGroup savedGroup = null;
            Exception savedException = null;
            group.Save ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                savedGroup = retGroup;
                savedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (savedException);
            Assert.IsTrue (savedException is GroupOperationException);
            Assert.IsNotNull (savedGroup);
            Assert.AreEqual ("testGroup", savedGroup.Name);
        }

        [Test(), KiiUTInfo(
                action = "When we call Save() with 1 user added and Server returns broken Json,",
                expected = "IllegalKiiBaseObjectFormatException must be passed in the callback"
                )]
        public void Test_0002_Save_broken_json ()
        {
            this.LogIn ();

            KiiGroup group = Kii.Group ("testGroup");

            KiiUser user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user1234"));
            group.AddUser (user);

            // set Response
            client.AddResponse (200, "{}");

            bool done = false;
            KiiGroup savedGroup = null;
            Exception savedException = null;
            group.Save ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                savedGroup = retGroup;
                savedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (savedException);
            Assert.IsTrue (savedException is IllegalKiiBaseObjectFormatException);
            Assert.IsNotNull (savedGroup);
            Assert.AreEqual ("testGroup", savedGroup.Name);
        }

        [Test(), KiiUTInfo(
                action = "When we call Save() with 1 user added for updating group,",
                expected = "1 API is called by SDK"
                )]
        public void Test_0010_Save_update_add ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user1234"));
            group.AddUser (user);

            // set Response
            client.RequestUrl.Clear ();
            this.SetStandardSaveResponse ();

            bool done = false;
            KiiGroup savedGroup = null;
            Exception savedException = null;
            group.Save ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                savedGroup = retGroup;
                savedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNull (savedException);
            Assert.IsNotNull (savedGroup);

            Assert.AreEqual (1, client.RequestUrl.Count);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/groups/group1234/members/user1234", client.RequestUrl [0]);
        }

        [Test(), KiiUTInfo(
                action = "When we call Save() with 2 users added for updating group,",
                expected = "2 API is called by SDK"
                )]
        public void Test_0011_Save_update_add_2 ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user1234"));
            group.AddUser (user);

            user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user5678"));
            group.AddUser (user);

            // set Response
            client.RequestUrl.Clear ();
            this.SetStandardSaveResponse ();

            bool done = false;
            KiiGroup savedGroup = null;
            Exception savedException = null;
            group.Save ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                savedGroup = retGroup;
                savedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNull (savedException);
            Assert.IsNotNull (savedGroup);

            Assert.AreEqual (2, client.RequestUrl.Count);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/groups/group1234/members/user1234", client.RequestUrl [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/groups/group1234/members/user5678", client.RequestUrl [1]);
        }

        [Test(), KiiUTInfo(
                action = "When we call Save() with 1 user deleted for updating group,",
                expected = "1 API is called by SDK"
                )]
        public void Test_0012_Save_update_delete ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user1234"));
            group.RemoveUser (user);


            // set Response
            client.RequestUrl.Clear ();
            this.SetStandardSaveResponse ();

            bool done = false;
            KiiGroup savedGroup = null;
            Exception savedException = null;
            group.Save ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                savedGroup = retGroup;
                savedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNull (savedException);
            Assert.IsNotNull (savedGroup);

            Assert.AreEqual (1, client.RequestUrl.Count);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/groups/group1234/members/user1234", client.RequestUrl [0]);
        }

        [Test(), KiiUTInfo(
                action = "When we call Save() with 2 users deleted for updating group,",
                expected = "2 API is called by SDK"
                )]
        public void Test_0013_Save_update_delete_2 ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user1234"));
            group.RemoveUser (user);

            user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user5678"));
            group.RemoveUser (user);

            // set Response
            client.RequestUrl.Clear ();
            this.SetStandardSaveResponse ();

            bool done = false;
            KiiGroup savedGroup = null;
            Exception savedException = null;
            group.Save ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                savedGroup = retGroup;
                savedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNull (savedException);
            Assert.IsNotNull (savedGroup);

            Assert.AreEqual (2, client.RequestUrl.Count);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/groups/group1234/members/user1234", client.RequestUrl [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/groups/group1234/members/user5678", client.RequestUrl [1]);
        }

        [Test(), KiiUTInfo(
                action = "When we call Save() with 2 user added for updating group and Server returns HTTP 400 for first call,",
                expected = "We can get 2 AddFailedUsers from Exception"
                )]
        public void Test_0020_Save_update_add_server_error ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user1234"));
            group.AddUser (user);

            user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user5678"));
            group.AddUser (user);

            // set Response
            client.RequestUrl.Clear ();
            client.AddResponse (new CloudException (400, "{ \"errorCode\" : \"INVALID_INPUT_DATA\", \"message\" : \"There are validation errors\", \"suppressed\" : [ ]}"));

            bool done = false;
            KiiGroup savedGroup = null;
            Exception savedException = null;
            group.Save ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                savedGroup = retGroup;
                savedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (savedException);
            Assert.IsTrue (savedException is GroupOperationException);
            Assert.IsNotNull (savedGroup);
            Assert.AreEqual ("group1234", savedGroup.ID);

            GroupOperationException groupExp = (GroupOperationException)savedException;
            Assert.AreEqual (2, groupExp.AddFailedUsers.Count);
            Assert.AreEqual (0, groupExp.RemoveFailedUsers.Count);
        }

        [Test(), KiiUTInfo(
                action = "When we call Save() with 2 user added for updating group and Server returns HTTP 400 for second call,",
                expected = "We can get 1 AddFailedUsers from Exception"
                )]
        public void Test_0021_Save_update_add_server_error_partial ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user1234"));
            group.AddUser (user);

            user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user5678"));
            group.AddUser (user);

            // set Response
            client.RequestUrl.Clear ();
            this.SetStandardSaveResponse ();
            client.AddResponse (new CloudException (400, "{ \"errorCode\" : \"INVALID_INPUT_DATA\", \"message\" : \"There are validation errors\", \"suppressed\" : [ ]}"));

            bool done = false;
            KiiGroup savedGroup = null;
            Exception savedException = null;
            group.Save ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                savedGroup = retGroup;
                savedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (savedException);
            Assert.IsTrue (savedException is GroupOperationException);
            Assert.IsNotNull (savedGroup);
            Assert.AreEqual ("group1234", savedGroup.ID);

            GroupOperationException groupExp = (GroupOperationException)savedException;
            Assert.AreEqual (1, groupExp.AddFailedUsers.Count);
            Assert.AreEqual (0, groupExp.RemoveFailedUsers.Count);
        }

        [Test(), KiiUTInfo(
                action = "When we call Save() with 1 user added / 1 user removed for updating group and Server returns HTTP 400 for first call,",
                expected = "We can get 1 AddFailedUsers / 1 RemoveFailedUsers from Exception"
                )]
        public void Test_0022_Save_update_add_remove_server_error ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user1234"));
            group.RemoveUser (user);

            user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user5678"));
            group.AddUser (user);

            // set Response
            client.RequestUrl.Clear ();
            client.AddResponse (new CloudException (400, "{ \"errorCode\" : \"INVALID_INPUT_DATA\", \"message\" : \"There are validation errors\", \"suppressed\" : [ ]}"));

            bool done = false;
            KiiGroup savedGroup = null;
            Exception savedException = null;
            group.Save ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                savedGroup = retGroup;
                savedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (savedException);
            Assert.IsTrue (savedException is GroupOperationException);
            Assert.IsNotNull (savedGroup);
            Assert.AreEqual ("group1234", savedGroup.ID);

            GroupOperationException groupExp = (GroupOperationException)savedException;
            Assert.AreEqual (1, groupExp.AddFailedUsers.Count);
            Assert.AreEqual (1, groupExp.RemoveFailedUsers.Count);
        }

        [Test(), KiiUTInfo(
                action = "When we call Save() with 1 user added / 1 user removed for updating group and Server returns HTTP 400 for second call,",
                expected = "We can get 0 AddFailedUsers / 1 RemoveFailedUsers from Exception"
                )]
        public void Test_0023_Save_update_add_remove_server_error_partial ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            KiiUser user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user1234"));
            group.RemoveUser (user);

            user = KiiUser.CreateByUri (new Uri ("kiicloud://users/user5678"));
            group.AddUser (user);

            // set Response
            client.RequestUrl.Clear ();
            this.SetStandardSaveResponse ();
            client.AddResponse (new CloudException (400, "{ \"errorCode\" : \"INVALID_INPUT_DATA\", \"message\" : \"There are validation errors\", \"suppressed\" : [ ]}"));

            bool done = false;
            KiiGroup savedGroup = null;
            Exception savedException = null;
            group.Save ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                savedGroup = retGroup;
                savedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (savedException);
            Assert.IsTrue (savedException is GroupOperationException);
            Assert.IsNotNull (savedGroup);
            Assert.AreEqual ("group1234", savedGroup.ID);

            GroupOperationException groupExp = (GroupOperationException)savedException;
            Assert.AreEqual (0, groupExp.AddFailedUsers.Count);
            Assert.AreEqual (1, groupExp.RemoveFailedUsers.Count);
        }

#endregion

#region KiiGroup.istMembers()
        [Test(), KiiUTInfo(
                action = "When we call ListMembers() and Server returns 4 members,",
                expected = "We can get 4 members"
                )]
        public void Test_0100_ListMembers ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // set Response
            this.SetStandardListResponse ();

            bool done = false;
            IList<KiiUser> listMembers = null;
            Exception listException = null;
            group.ListMembers ((IList<KiiUser> retUserList, Exception retExp) =>
            {
                done = true;
                listMembers = retUserList;
                listException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNull (listException);
            Assert.IsNotNull (listMembers);

            Assert.AreEqual (4, listMembers.Count);
        }

        [Test(),  KiiUTInfo(
                action = "When we call ListMembers() to KiiGroup that doesn't have ID,",
                expected = "InvalidOperationException must be passed in the callback"
                )]
        public void Test_0101_ListMembers_NoID ()
        {
            this.LogIn ();

            KiiGroup group = Kii.Group ("newGroup");

            // set Response
            this.SetStandardListResponse ();

            bool done = false;
            IList<KiiUser> listMembers = null;
            Exception listException = null;
            group.ListMembers ((IList<KiiUser> retUserList, Exception retExp) =>
            {
                done = true;
                listMembers = retUserList;
                listException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (listException);
            Assert.IsTrue (listException is InvalidOperationException);
            Assert.IsNull (listMembers);
        }

        [Test(), KiiUTInfo(
                action = "When we call ListMembers() and Server returns no member field,",
                expected = "We can get the list that has no entry"
                )]
        public void Test_0102_ListMembers_no_member ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // Set Response of empty response to throw exception from ListMembers request
            client.AddResponse (200, "{}");

            bool done = false;
            IList<KiiUser> listMembers = null;
            Exception listException = null;
            group.ListMembers ((IList<KiiUser> retUserList, Exception retExp) =>
            {
                done = true;
                listMembers = retUserList;
                listException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (listException);
            Assert.IsTrue (listException is IllegalKiiBaseObjectFormatException);
            Assert.IsNull (listMembers);
        }

        [Test(), KiiUTInfo(
                action = "When we call ListMembers() and Server returns empty JsonArray,",
                expected = "We can get the list that has no entry"
                )]
        public void Test_0103_ListMembers_empty_member ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // Set Response of no entry to throw exception from ListMembers request
            client.AddResponse (200, "{\"members\":[]}");

            bool done = false;
            IList<KiiUser> listMembers = null;
            Exception listException = null;
            group.ListMembers ((IList<KiiUser> retUserList, Exception retExp) =>
            {
                done = true;
                listMembers = retUserList;
                listException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNull (listException);
            Assert.IsNotNull (listMembers);

            Assert.AreEqual (0, listMembers.Count);
        }

        [Test(), KiiUTInfo(
                action = "When we call ListMembers() and Server returns member array that has broken entry,",
                expected = "IllegalKiiBaseObjectFormatException must be passed in the callback"
                )]
        public void Test_0104_ListMembers_no_userId ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // Set Response of no "userID" field to throw exception from ListMembers request
            client.AddResponse (200, "{\"members\":[" +
                "{\"ID\":\"1234\"}" +
                "]}");

            bool done = false;
            IList<KiiUser> listMembers = null;
            Exception listException = null;
            group.ListMembers ((IList<KiiUser> retUserList, Exception retExp) =>
            {
                done = true;
                listMembers = retUserList;
                listException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (listException);
            Assert.IsTrue (listException is IllegalKiiBaseObjectFormatException);
            Assert.IsNull (listMembers);
        }

        [Test(), KiiUTInfo(
                action = "When we call ListMembers() and Server returns member array that has broken entry(empty userID),",
                expected = "IllegalKiiBaseObjectFormatException must be passed in the callback"
                )]
        public void Test_0105_ListMembers_empty_userId ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // Set Response of empty user ID to throw exception from ListMembers request
            client.AddResponse (200, "{\"members\":[" +
                "{\"userID\":\"\"}" +
                "]}");

            bool done = false;
            IList<KiiUser> listMembers = null;
            Exception listException = null;
            group.ListMembers ((IList<KiiUser> retUserList, Exception retExp) =>
            {
                done = true;
                listMembers = retUserList;
                listException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (listException);
            Assert.IsTrue (listException is IllegalKiiBaseObjectFormatException);
            Assert.IsNull (listMembers);
        }
#endregion

#region KiiGroup.Refresh()

        [Test(), KiiUTInfo(
                action = "When we call Refresh() and Server returns KiiGroup,",
                expected = "We can get groupName and owner URI"
                )]
        public void Test_0200_Refresh ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // set Response
            this.SetStandardRefreshResponse ();

            bool done = false;
            KiiGroup refreshedGroup = null;
            Exception refreshedException = null;
            group.Refresh ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                refreshedGroup = retGroup;
                refreshedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNull (refreshedException);
            Assert.IsNotNull (refreshedGroup);

            Assert.AreEqual ("testing group", group.Name);
            Assert.AreEqual ("kiicloud://users/e3137ebe-2874-4d02-b7ef-6780bf8ecc1d", group.Owner.Uri.ToString ());
        }

        [Test(), KiiUTInfo(
                action = "When we call Refresh() to KiiGroup that doesn't have ID,",
                expected = "InvalidOperationException must be passed in the callback"
                )]
        public void Test_0201_Refresh_no_ID ()
        {
            this.LogIn ();

            KiiGroup group = Kii.Group ("newGroup");

            // set Response
            this.SetStandardRefreshResponse ();

            bool done = false;
            KiiGroup refreshedGroup = null;
            Exception refreshedException = null;
            group.Refresh ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                refreshedGroup = retGroup;
                refreshedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (refreshedException);
            Assert.IsTrue (refreshedException is InvalidOperationException);
            Assert.IsNotNull (refreshedGroup);
            Assert.AreEqual ("newGroup", refreshedGroup.Name);
        }

        [Test(), KiiUTInfo(
                action = "When we call Refresh() and Server returns broken Json,",
                expected = "IllegalKiiBaseObjectFormatException must be passed in the callback"
                )]
        public void Test_0202_Refresh_broken_json ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse (200, "broken");

            bool done = false;
            KiiGroup refreshedGroup = null;
            Exception refreshedException = null;
            group.Refresh ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                refreshedGroup = retGroup;
                refreshedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (refreshedException);
            Assert.IsTrue (refreshedException is IllegalKiiBaseObjectFormatException);
            Assert.IsNotNull (refreshedGroup);
            Assert.AreEqual ("group1234", refreshedGroup.ID);
        }

        [Test(), KiiUTInfo(
                action = "When we call Refresh() and Server returns Json that doesn't have groupID,",
                expected = "IllegalKiiBaseObjectFormatException must be passed in the callback"
                )]
        public void Test_0203_Refresh_broken_no_groupId ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse (200, "{" +
                "\"name\" : \"testing group\"," +
                "\"owner\" : \"e3137ebe-2874-4d02-b7ef-6780bf8ecc1d\"}");

            bool done = false;
            KiiGroup refreshedGroup = null;
            Exception refreshedException = null;
            group.Refresh ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                refreshedGroup = retGroup;
                refreshedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (refreshedException);
            Assert.IsTrue (refreshedException is IllegalKiiBaseObjectFormatException);
            Assert.IsNotNull (refreshedGroup);
            Assert.AreEqual ("group1234", refreshedGroup.ID);
        }

        [Test(), KiiUTInfo(
                action = "When we call Refresh() and Server returns Json that doesn't have name,",
                expected = "IllegalKiiBaseObjectFormatException must be passed in the callback"
                )]
        public void Test_0204_Refresh_broken_no_name ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse (200, "{" +
                "\"groupID\" : \"90def9aa-565e-4037-bde8-3a8704c7d806\"," +
                "\"owner\" : \"e3137ebe-2874-4d02-b7ef-6780bf8ecc1d\"}");

            bool done = false;
            KiiGroup refreshedGroup = null;
            Exception refreshedException = null;
            group.Refresh ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                refreshedGroup = retGroup;
                refreshedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (refreshedException);
            Assert.IsTrue (refreshedException is IllegalKiiBaseObjectFormatException);
            Assert.IsNotNull (refreshedGroup);
            // We won't rollback the id change after the exception happens.
            Assert.AreEqual ("90def9aa-565e-4037-bde8-3a8704c7d806", refreshedGroup.ID);
        }

        [Test(), KiiUTInfo(
                action = "When we call Refresh() and Server returns Json that doesn't have owner,",
                expected = "We can get groupName and ID, but owner is null"
                )]
        public void Test_0205_Refresh_broken_no_owner ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse (200, "{" +
                "\"groupID\" : \"90def9aa-565e-4037-bde8-3a8704c7d806\"," +
                "\"name\" : \"testing group\"}");

            bool done = false;
            KiiGroup refreshedGroup = null;
            Exception refreshedException = null;
            group.Refresh ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                refreshedGroup = retGroup;
                refreshedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNull (refreshedException);
            Assert.IsNotNull (refreshedGroup);

            Assert.AreEqual ("testing group", refreshedGroup.Name);
            Assert.AreEqual ("90def9aa-565e-4037-bde8-3a8704c7d806", refreshedGroup.ID);
            Assert.IsNull (refreshedGroup.Owner);
        }

        [Test(), KiiUTInfo(
                action = "When we call Refresh() and Server returns HTTP 400,",
                expected = "CloudException must be passed in the callback"
                )]
        public void Test_0206_Refresh_server_error ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse (new CloudException (400, "{ \"errorCode\" : \"INVALID_INPUT_DATA\", \"message\" : \"There are validation errors\", \"suppressed\" : [ ]}"));

            bool done = false;
            KiiGroup refreshedGroup = null;
            Exception refreshedException = null;
            group.Refresh ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                refreshedGroup = retGroup;
                refreshedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (refreshedException);
            Assert.IsTrue (refreshedException is CloudException);
            Assert.IsNotNull (refreshedGroup);
            Assert.AreEqual ("group1234", refreshedGroup.ID);
        }
#endregion

#region KiiGroup.Delete()

        [Test(), KiiUTInfo(
                action = "When we call Delete() and Server returns HTTP 204(OK),",
                expected = "Uri must be null after Deletion"
                )]
        public void Test_0300_Delete ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse (204, "");

            bool done = false;
            KiiGroup deletedGroup = null;
            Exception deletedException = null;
            group.Delete ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                deletedGroup = retGroup;
                deletedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNull (deletedException);
            Assert.IsNotNull (deletedGroup);

            Assert.IsNull (group.Uri);
        }

        [Test(), KiiUTInfo(
                action = "When we call Delete() to KiiGroup that doesn't have ID,",
                expected = "InvalidOperationException must be passed in the callback"
                )]
        public void Test_0301_Delete_no_ID ()
        {
            this.LogIn ();

            KiiGroup group = Kii.Group ("newGroup");

            // set Response
            client.AddResponse (204, "");

            bool done = false;
            KiiGroup deletedGroup = null;
            Exception deletedException = null;
            group.Delete ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                deletedGroup = retGroup;
                deletedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (deletedException);
            Assert.IsTrue (deletedException is InvalidOperationException);
            Assert.IsNotNull (deletedGroup);
            Assert.AreEqual ("newGroup", deletedGroup.Name);
        }

        [Test(), KiiUTInfo(
                action = "When we call Delete() and Server returns HTTP 400,",
                expected = "CloudException must be passed in the callback"
                )]
        public void Test_0302_Delete_server_error ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse (new CloudException (400, "{ \"errorCode\" : \"INVALID_INPUT_DATA\", \"message\" : \"There are validation errors\", \"suppressed\" : [ ]}"));

            bool done = false;
            KiiGroup deletedGroup = null;
            Exception deletedException = null;
            group.Delete ((KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                deletedGroup = retGroup;
                deletedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (deletedException);
            Assert.IsTrue (deletedException is CloudException);
            Assert.IsNotNull (deletedGroup);
            Assert.AreEqual ("group1234", deletedGroup.ID);
        }

#endregion

#region KiiGroup.ChangeName(string)

        [Test(), KiiUTInfo(
                action = "When we call ChangeName() and Server returns HTTP 204(OK),",
                expected = "We can get updated name by group.Name"
                )]
        public void Test_0400_ChangeName ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse (204, "");

            bool done = false;
            KiiGroup changedGroup = null;
            Exception changedException = null;
            group.ChangeName ("newName", (KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                changedGroup = retGroup;
                changedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNull (changedException);
            Assert.IsNotNull (changedGroup);

            Assert.AreEqual ("newName", group.Name);
        }

        [Test(), KiiUTInfo(
                action = "When we call ChangeName() with null,",
                expected = "ArgumentException must be passed in the callback"
                )]
        public void Test_0401_ChangeName_null ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));

            // set Response
            client.AddResponse (204, "");

            bool done = false;
            KiiGroup changedGroup = null;
            Exception changedException = null;
            group.ChangeName (null, (KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                changedGroup = retGroup;
                changedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (changedException);
            Assert.IsTrue (changedException is ArgumentException);
            Assert.IsNotNull (changedGroup);
            Assert.AreEqual ("group1234", changedGroup.ID);
        }

        [Test(), KiiUTInfo(
                action = "When we call ChangeName() to KiiGroup that doesn't have ID,",
                expected = "InvalidOperationException must be passed in the callback"
                )]
        public void Test_0402_ChangeName_no_id ()
        {
            this.LogIn ();

            KiiGroup group = Kii.Group ("groupName");

            // set Response
            client.AddResponse (204, "");

            bool done = false;
            KiiGroup changedGroup = null;
            Exception changedException = null;
            group.ChangeName ("newGroupName", (KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                changedGroup = retGroup;
                changedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (changedException);
            Assert.IsTrue (changedException is InvalidOperationException);
            Assert.IsNotNull (changedGroup);
            Assert.AreEqual ("groupName", changedGroup.Name);
        }

        [Test(), KiiUTInfo(
                action = "When we call ChangeName() and Server returns HTTP 400,",
                expected = "CloudException must be passed in the callback"
                )]
        public void Test_0403_ChangeName_server_error ()
        {
            this.LogIn ();

            KiiGroup group = KiiGroup.CreateByUri (new Uri ("kiicloud://groups/group1234"));
            group.Name = "testGroup";

            // set Response
            client.AddResponse (new CloudException (400, "{ \"errorCode\" : \"INVALID_INPUT_DATA\", \"message\" : \"There are validation errors\", \"suppressed\" : [ ]}"));

            bool done = false;
            KiiGroup changedGroup = null;
            Exception changedException = null;
            group.ChangeName ("newGroupName", (KiiGroup retGroup, Exception retExp) =>
            {
                done = true;
                changedGroup = retGroup;
                changedException = retExp;
            });

            Assert.IsTrue (done);
            Assert.IsNotNull (changedException);
            Assert.IsTrue (changedException is CloudException);
            Assert.IsNotNull (changedGroup);
            Assert.AreEqual ("group1234", changedGroup.ID);
            Assert.AreEqual ("testGroup", changedGroup.Name);
        }
#endregion
    }
}

