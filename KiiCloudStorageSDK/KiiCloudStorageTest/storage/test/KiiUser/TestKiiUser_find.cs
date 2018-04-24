using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiUser_find
    {
        private MockHttpClient client;

        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
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

        private void SetStandardFindResponse(MockHttpClient client)
        {
            client.AddResponse(200, "{" +
                "\"userID\" : \"a7784f94-3134-4a78-82af-75df87f23873\"," +
                "\"loginName\" : \"Test003\"," +
                "\"displayName\" : \"Person Test003\"}");
        }

        private void SetStandardMemberResponse(MockHttpClient client)
        {
            client.AddResponse(200, "{" +
                "\"groups\" : [{" +
                "\"groupID\" : \"2d7c1743-0a90-4b91-8d82-c7800951ae4c\"," +
                "\"name\" : \"testing group 1\"," +
                "\"owner\" : \"0f418324-6c94-4db1-a683-dd7d802f1e92\"}," +
                "{" +
                "\"groupID\" : \"9367d8bc-0ca0-4495-8cca-c1637715b917\"," +
                "\"name\" : \"testing group 2\"," +
                "\"owner\" : \"0f418324-6c94-4db1-a683-dd7d802f1e92\"}]}");
        }

        private void SetStandardOwnerResponse(MockHttpClient client)
        {
            client.AddResponse(200, 
                @"
                {
                  ""groups"" : [ {
                    ""groupID"" : ""3hj8w3592z8ex1lm9le4gp2u9"",
                    ""name"" : ""testing group1"",
                    ""owner"" : ""dfa848a00022-040a-4e11-1305-0d46f973"",
                    ""createdAt"" : 1412913781542,
                    ""modifiedAt"" : 1412913781542
                  }, {
                    ""groupID"" : ""esv505f6e9h6rjt8h4fxyrqu9"",
                    ""name"" : ""testing group2"",
                    ""owner"" : ""dfa848a00022-040a-4e11-1305-0d46f973"",
                    ""createdAt"" : 1412913542835,
                    ""modifiedAt"" : 1412913542835
                  } ]
                }
                 "
            );
        }

        #region KiiUser.FindUserByUserName(string)
        [Test(), KiiUTInfo(
            action = "When we call FindUserByUserName() and Server returns KiiUser,",
            expected = "We can get UserID/Username/Displayname from it"
            )]
        public void Test_0000_findByUsername ()
        {
            this.LogIn();

            // set Response
            this.SetStandardFindResponse(client);

            KiiUser user = KiiUser.FindUserByUserName("user1234");

            Assert.AreEqual("a7784f94-3134-4a78-82af-75df87f23873", user.ID);
            Assert.AreEqual("Test003", user.Username);
            Assert.AreEqual("Person Test003", user.Displayname);
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call FindUserByUserName() and Server returns HTTP 404,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0001_findByUsername_server_error ()
        {
            this.LogIn();

            // set Response
            client.AddResponse(new CloudException(404, "{}"));

            KiiUser.FindUserByUserName("user1234");
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call FindUserByUserName() and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0002_findByUsername_broken_json ()
        {
            this.LogIn();

            // set Response
            client.AddResponse(200, "broken");

            KiiUser.FindUserByUserName("user1234");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call FindUserByUserName() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0003_findByUsername_null ()
        {
            this.LogIn();

            // set Response
            this.SetStandardFindResponse(client);

            KiiUser.FindUserByUserName(null);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call FindUserByUserName() with invalid username,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0004_findByUsername_invalid ()
        {
            this.LogIn();

            // set Response
            this.SetStandardFindResponse(client);

            KiiUser.FindUserByUserName("ab");
        }
        #endregion

        #region KiiUser.FindUserByEmail(string)
        [Test(), KiiUTInfo(
            action = "When we call FindUserByEmail() and Server returns KiiUser,",
            expected = "We can get UserID/Username/Displayname from it"
            )]
        public void Test_0100_findByEmail ()
        {
            this.LogIn();

            // set Response
            this.SetStandardFindResponse(client);

            KiiUser user = KiiUser.FindUserByEmail("test@test.com");

            Assert.AreEqual("a7784f94-3134-4a78-82af-75df87f23873", user.ID);
            Assert.AreEqual("Test003", user.Username);
            Assert.AreEqual("Person Test003", user.Displayname);
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call FindUserByEmail() and Server returns HTTP 404,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0101_findByEmail_server_error ()
        {
            this.LogIn();

            // set Response
            client.AddResponse(new CloudException(404, "{}"));

            KiiUser.FindUserByEmail("test@test.com");
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call FindUserByEmail() and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0102_findByEmail_broken_json ()
        {
            this.LogIn();

            // set Response
            client.AddResponse(200, "broken");

            KiiUser.FindUserByEmail("test@test.com");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call FindUserByEmail() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0103_findByEmail_null ()
        {
            this.LogIn();

            // set Response
            this.SetStandardFindResponse(client);

            KiiUser.FindUserByEmail(null);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call FindUserByEmail() with invalid email,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0104_findByEmail_invalid ()
        {
            this.LogIn();

            // set Response
            this.SetStandardFindResponse(client);

            KiiUser.FindUserByEmail("test");
        }
        #endregion

        #region KiiUser.FindUserByPhone(string)
        [Test(), KiiUTInfo(
            action = "When we call FindUserByPhone() and Server returns KiiUser,",
            expected = "We can get UserID/Username/Displayname from it"
            )]
        public void Test_0200_findByPhone ()
        {
            this.LogIn();

            // set Response
            this.SetStandardFindResponse(client);

            KiiUser user = KiiUser.FindUserByPhone("+818011112222");

            Assert.AreEqual("a7784f94-3134-4a78-82af-75df87f23873", user.ID);
            Assert.AreEqual("Test003", user.Username);
            Assert.AreEqual("Person Test003", user.Displayname);
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call FindUserByPhone() and Server returns HTTP 404,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0201_findByPhone_server_error ()
        {
            this.LogIn();

            // set Response
            client.AddResponse(new CloudException(404, "{}"));

            KiiUser.FindUserByPhone("+818011112222");
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call FindUserByPhone() and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0202_findByPhone_broken_json ()
        {
            this.LogIn();

            // set Response
            client.AddResponse(200, "broken");

            KiiUser.FindUserByPhone("+818011112222");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call FindUserByPhone() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0203_findByPhone_null ()
        {
            this.LogIn();

            // set Response
            this.SetStandardFindResponse(client);

            KiiUser.FindUserByPhone(null);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call FindUserByPhone() with invalid phone number,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0204_findByPhone_invalid ()
        {
            this.LogIn();

            // set Response
            this.SetStandardFindResponse(client);

            KiiUser.FindUserByPhone("invalid");
        }
        #endregion


        #region KiiUser.MemberOfGroups()
        [Test(), KiiUTInfo(
            action = "When we call MemberOfGroups() and Server returns 2 KiiGroups,",
            expected = "We can get GroupURI/GroupName/GroupOwner from them"
            )]
        public void Test_1000_MemberOfGrups()
        {
            this.LogIn();

            // set Response
            this.SetStandardMemberResponse(client);

            IList<KiiGroup> groups = KiiUser.CurrentUser.MemberOfGroups();

            Assert.AreEqual(2, groups.Count);
            Assert.AreEqual("kiicloud://groups/2d7c1743-0a90-4b91-8d82-c7800951ae4c", groups[0].Uri.ToString());
            Assert.AreEqual("testing group 1", groups[0].Name);
            Assert.AreEqual("kiicloud://users/0f418324-6c94-4db1-a683-dd7d802f1e92", groups[0].Owner.Uri.ToString());

            Assert.AreEqual("kiicloud://groups/9367d8bc-0ca0-4495-8cca-c1637715b917", groups[1].Uri.ToString());
            Assert.AreEqual("testing group 2", groups[1].Name);
            Assert.AreEqual("kiicloud://users/0f418324-6c94-4db1-a683-dd7d802f1e92", groups[1].Owner.Uri.ToString());

        }

        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call MemberOfGroups() of no-login KiiUser object,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_1001_MemberOfGrups_noId()
        {
            this.LogIn();

            // set Response
            this.SetStandardMemberResponse(client);

            KiiUser.BuilderWithName("newUser").Build().MemberOfGroups();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call MemberOfGroups() and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_1002_MemberOfGrups_no_group()
        {
            this.LogIn();

            // set Response
            client.AddResponse(200, "{}");

            KiiUser.CurrentUser.MemberOfGroups();
        }

        [Test(), KiiUTInfo(
            action = "When we call MemberOfGroups() and Server returns empty groups,",
            expected = "We can get a group list with length=0"
            )]
        public void Test_1003_MemberOfGrups_empty_group()
        {
            this.LogIn();

            // set Response
            client.AddResponse(200, "{\"groups\":[]}");

            IList<KiiGroup> groups =  KiiUser.CurrentUser.MemberOfGroups();
            Assert.AreEqual(0, groups.Count);
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call MemberOfGroups() and Server returns groups without GroupID,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_1004_MemberOfGrups_no_groupID()
        {
            this.LogIn();

            // set Response
            client.AddResponse(200, "{" +
                "\"groups\" : [{" +
                "\"name\" : \"testing group 1\"," +
                "\"owner\" : \"0f418324-6c94-4db1-a683-dd7d802f1e92\"}]}");
            KiiUser.CurrentUser.MemberOfGroups();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call MemberOfGroups() and Server returns groups without GroupName,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_1005_MemberOfGrups_no_name()
        {
            this.LogIn();

            // set Response
            client.AddResponse(200, "{" +
                "\"groups\" : [{" +
                "\"groupID\" : \"2d7c1743-0a90-4b91-8d82-c7800951ae4c\"," +
                "\"owner\" : \"0f418324-6c94-4db1-a683-dd7d802f1e92\"}]}");
            KiiUser.CurrentUser.MemberOfGroups();
        }

        [Test(), KiiUTInfo(
            action = "When we call MemberOfGroups() and Server returns groups without owner,",
            expected = "returning Group instance."
            )]
        public void Test_1006_MemberOfGrups_no_owner()
        {
            this.LogIn();

            // set Response
            client.AddResponse(200, "{" +
                "\"groups\" : [{" +
                "\"groupID\" : \"2d7c1743-0a90-4b91-8d82-c7800951ae4c\"," +
                "\"name\" : \"testing group 1\"}]}");
            IList<KiiGroup> groups = KiiUser.CurrentUser.MemberOfGroups();
            Assert.AreEqual(1, groups.Count);
            Assert.AreEqual("kiicloud://groups/2d7c1743-0a90-4b91-8d82-c7800951ae4c", groups[0].Uri.ToString());
            Assert.AreEqual("testing group 1", groups[0].Name);
            Assert.IsNull(groups[0].Owner);
        }
        #endregion

        #region KiiUser.OwnerOfGroups()
        [Test(), KiiUTInfo(
            action = "When we call OwnerOfGroups() and Server returns 2 KiiGroups,",
            expected = "We can get GroupURI/GroupName/GroupOwner from them"
        )]
        public void Test_2000_OwnerOfGrups()
        {
            this.LogIn();

            // set Response
            this.SetStandardOwnerResponse(client);

            IList<KiiGroup> groups = KiiUser.CurrentUser.OwnerOfGroups();

            Assert.AreEqual(2, groups.Count);
            Assert.AreEqual("kiicloud://groups/3hj8w3592z8ex1lm9le4gp2u9", groups[0].Uri.ToString());
            Assert.AreEqual("testing group1", groups[0].Name);
            Assert.AreEqual("kiicloud://users/dfa848a00022-040a-4e11-1305-0d46f973", groups[0].Owner.Uri.ToString());

            Assert.AreEqual("kiicloud://groups/esv505f6e9h6rjt8h4fxyrqu9", groups[1].Uri.ToString());
            Assert.AreEqual("testing group2", groups[1].Name);
            Assert.AreEqual("kiicloud://users/dfa848a00022-040a-4e11-1305-0d46f973", groups[1].Owner.Uri.ToString());

        }

        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call OwnerOfGroups() of no-login KiiUser object,",
            expected = "InvalidOperationException must be thrown"
        )]
        public void Test_2001_OwnerOfGrups_noId()
        {
            this.LogIn();

            // set Response
            this.SetStandardOwnerResponse(client);

            KiiUser.BuilderWithName("newUser").Build().OwnerOfGroups();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call OwnerOfGroups() and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
        )]
        public void Test_2002_OwnerOfGrups_no_group()
        {
            this.LogIn();

            // set Response
            client.AddResponse(200, "{}");

            KiiUser.CurrentUser.OwnerOfGroups();
        }

        [Test(), KiiUTInfo(
            action = "When we call OwnerOfGroups() and Server returns empty groups,",
            expected = "We can get a group list with length=0"
        )]
        public void Test_2003_OwnerOfGrups_empty_group()
        {
            this.LogIn();

            // set Response
            client.AddResponse(200, "{\"groups\":[]}");

            IList<KiiGroup> groups =  KiiUser.CurrentUser.OwnerOfGroups();
            Assert.AreEqual(0, groups.Count);
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call OwnerOfGroups() and Server returns groups without GroupID,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
        )]
        public void Test_2004_OwnerOfGrups_no_groupID()
        {
            this.LogIn();

            // set Response
            client.AddResponse(200, "{" +
                "\"groups\" : [{" +
                "\"name\" : \"testing group 1\"," +
                "\"owner\" : \"0f418324-6c94-4db1-a683-dd7d802f1e92\"}]}");
            KiiUser.CurrentUser.OwnerOfGroups();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call OwnerOfGroups() and Server returns groups without GroupName,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
        )]
        public void Test_2005_OwnerOfGrups_no_name()
        {
            this.LogIn();

            // set Response
            client.AddResponse(200, "{" +
                "\"groups\" : [{" +
                "\"groupID\" : \"2d7c1743-0a90-4b91-8d82-c7800951ae4c\"," +
                "\"owner\" : \"0f418324-6c94-4db1-a683-dd7d802f1e92\"}]}");
            KiiUser.CurrentUser.OwnerOfGroups();
        }

        [Test(), KiiUTInfo(
            action = "When we call OwnerOfGroups() and Server returns groups without owner,",
            expected = "returning Group instance."
        )]
        public void Test_2006_OwnerOfGrups_no_owner()
        {
            this.LogIn();

            // set Response
            client.AddResponse(200, "{" +
                "\"groups\" : [{" +
                "\"groupID\" : \"2d7c1743-0a90-4b91-8d82-c7800951ae4c\"," +
                "\"name\" : \"testing group 1\"}]}");
            IList<KiiGroup> groups = KiiUser.CurrentUser.OwnerOfGroups();
            Assert.AreEqual(1, groups.Count);
            Assert.AreEqual("kiicloud://groups/2d7c1743-0a90-4b91-8d82-c7800951ae4c", groups[0].Uri.ToString());
            Assert.AreEqual("testing group 1", groups[0].Name);
            Assert.IsNull(groups[0].Owner);
        }
        #endregion

    }
}

