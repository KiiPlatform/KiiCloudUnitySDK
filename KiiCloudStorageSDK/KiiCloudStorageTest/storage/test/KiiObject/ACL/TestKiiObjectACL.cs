using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiObjectACL
    {
        private MockHttpClient client;

        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            client = factory.Client;
        }

        private void SetStandardResponse(MockHttpClient client)
        {
            client.AddResponse(204, "");
        }

        private void SetStandardGetResponse(MockHttpClient client)
        {
            client.AddResponse(200, "{" +
                "\"READ_EXISTING_OBJECT\":[" +
                "{\"userID\":\"user1234\"}" +
                "]," +
                "\"WRITE_EXISTING_OBJECT\":[" +
                "{\"userID\":\"user5678\"}" +
                "]" +
                "}"
                );
        }

        #region ACL.Save()

        [Test(), KiiUTInfo(
            action = "When we call Save() with 1 put entry,",
            expected = "1 API must be called"
            )]
        public void Test_0000_PutAclEntry ()
        {

            // set response
            this.SetStandardResponse(client);

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            KiiObjectAcl acl = obj.Acl(ObjectAction.READ_EXISTING_OBJECT);

            // user
            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/id1234"));
            acl.Subject(user).Save(ACLOperation.GRANT);

            Assert.AreEqual(1, client.RequestUrl.Count);
            Assert.AreEqual("https://api.kii.com/api/apps/appId/buckets/test/objects/abcd/acl/READ_EXISTING_OBJECT/UserID:id1234", client.RequestUrl[0]);
        }

        [Test(), KiiUTInfo(
            action = "When we call Save() with 2 put entries,",
            expected = "2 API must be called"
            )]
        public void Test_0001_PutAclEntry_2user ()
        {
            // set response
            this.SetStandardResponse(client);

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            KiiObjectAcl acl = obj.Acl(ObjectAction.READ_EXISTING_OBJECT);

            // user
            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/id1234"));
            acl.Subject(user).Save(ACLOperation.GRANT);

            user = KiiUser.CreateByUri(new Uri("kiicloud://users/id5678"));
            obj.Acl(ObjectAction.WRITE_EXISTING_OBJECT).Subject(user).Save(ACLOperation.GRANT);


            Assert.AreEqual(2, client.RequestUrl.Count);
            Assert.AreEqual("https://api.kii.com/api/apps/appId/buckets/test/objects/abcd/acl/READ_EXISTING_OBJECT/UserID:id1234", client.RequestUrl[0]);
            Assert.AreEqual("https://api.kii.com/api/apps/appId/buckets/test/objects/abcd/acl/WRITE_EXISTING_OBJECT/UserID:id5678", client.RequestUrl[1]);
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call Save() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0002_PutAclEntry_server_error ()
        {

            // set response
            client.AddResponse(new CloudException(400, "{}"));

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            KiiObjectAcl acl = obj.Acl(ObjectAction.READ_EXISTING_OBJECT);

            // user
            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/id1234"));
            acl.Subject(user).Save(ACLOperation.GRANT);

        }

        #endregion

        #region ListAclEntries()

        [Test(), KiiUTInfo(
            action = "When we call ListAclEntries() and Server returns 2 KiiUser entries,",
            expected = "We can get 2 entries whose subject must be KiiUser"
            )]
        public void Test_0200_ListAclEntries ()
        {

            // set response
            this.SetStandardGetResponse(client);

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));

            // list
            IList<KiiACLEntry<KiiObject, ObjectAction>> entries = obj.ListAclEntries();
            Assert.AreEqual(2, entries.Count);

            // entry 1
            KiiACLEntry<KiiObject, ObjectAction> entry = entries[0];
            Assert.AreEqual(ObjectAction.READ_EXISTING_OBJECT, entry.Action);
            Assert.IsTrue(entry.Subject is KiiUser);
            Assert.AreEqual("kiicloud://users/user1234", ((KiiUser)entry.Subject).Uri.ToString());

            // entry 2
            entry = entries[1];
            Assert.AreEqual(ObjectAction.WRITE_EXISTING_OBJECT, entry.Action);
            Assert.IsTrue(entry.Subject is KiiUser);
            Assert.AreEqual("kiicloud://users/user5678", ((KiiUser)entry.Subject).Uri.ToString());

        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call ListAclEntries() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0201_ListAclEntries_server_error ()
        {
            // set response
            client.AddResponse(new CloudException(400, "{}"));

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));

            // list
            obj.ListAclEntries();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call ListAclEntries() and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0202_ListAclEntries_broken_json ()
        {
            // set response
            client.AddResponse(200, "broken");

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));

            // list
            obj.ListAclEntries();
        }

        [Test(), KiiUTInfo(
            action = "When we call ListAclEntries() and Server returns unknown action,",
            expected = "We can get only entries whose actions are known"
            )]
        public void Test_0203_ListAclEntries_unknown_action ()
        {
            // set response
            client.AddResponse(200, "{" +
                "\"READ_EXISTING_OBJECT\":[" +
                "{\"userID\":\"user1234\"}" +
                "]," +
                "\"CREATE_OBJECTS_IN_BUCKET\":[" +
                "{\"userID\":\"user5678\"}" +
                "]," +
                "\"WRITE_EXISTING_OBJECT\":[" +
                "{\"groupID\":\"group5678\"}" +
                "]" +
                "}"
                );

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));

            // list
            IList<KiiACLEntry<KiiObject, ObjectAction>> entries = obj.ListAclEntries();
            Assert.AreEqual(2, entries.Count);

            // entry 1
            KiiACLEntry<KiiObject, ObjectAction> entry = entries[0];
            Assert.AreEqual(ObjectAction.READ_EXISTING_OBJECT, entry.Action);
            Assert.IsTrue(entry.Subject is KiiUser);
            Assert.AreEqual("kiicloud://users/user1234", ((KiiUser)entry.Subject).Uri.ToString());

            // entry 2
            entry = entries[1];
            Assert.AreEqual(ObjectAction.WRITE_EXISTING_OBJECT, entry.Action);
            Assert.IsTrue(entry.Subject is KiiGroup);
            Assert.AreEqual("kiicloud://groups/group5678", ((KiiGroup)entry.Subject).Uri.ToString());

        }

        [Test(), KiiUTInfo(
            action = "When we call ListAclEntries() and Server returns entry whose id field is unknown,",
            expected = "We can get only entries whose id fields are known"
            )]
        public void Test_0204_ListAclEntries_unknown_id ()
        {
            // set response
            client.AddResponse(200, "{" +
                "\"READ_EXISTING_OBJECT\":[" +
                "{\"userID\":\"user1234\"}" +
                "]," +
                "\"WRITE_EXISTING_OBJECT\":[" +
                "{\"objectID\":\"object5678\"}" +
                "]" +
                "}"
                );

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));

            // list
            IList<KiiACLEntry<KiiObject, ObjectAction>> entries = obj.ListAclEntries();
            Assert.AreEqual(1, entries.Count);

            // entry 1
            KiiACLEntry<KiiObject, ObjectAction> entry = entries[0];
            Assert.AreEqual(ObjectAction.READ_EXISTING_OBJECT, entry.Action);
            Assert.IsTrue(entry.Subject is KiiUser);
            Assert.AreEqual("kiicloud://users/user1234", ((KiiUser)entry.Subject).Uri.ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call ListAclEntries() and Server returns only READ entry,",
            expected = "We can get only READ entry(not crashed)"
            )]
        public void Test_0205_ListAclEntries_READ_ONLY ()
        {
            // set response
            client.AddResponse(200, "{" +
                "\"READ_EXISTING_OBJECT\":[" +
                "{\"userID\":\"user1234\"}" +
                "]" +
                "}"
                );

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));

            // list
            IList<KiiACLEntry<KiiObject, ObjectAction>> entries = obj.ListAclEntries();
            Assert.AreEqual(1, entries.Count);

            // entry 1
            KiiACLEntry<KiiObject, ObjectAction> entry = entries[0];
            Assert.AreEqual(ObjectAction.READ_EXISTING_OBJECT, entry.Action);
            Assert.IsTrue(entry.Subject is KiiUser);
            Assert.AreEqual("kiicloud://users/user1234", ((KiiUser)entry.Subject).Uri.ToString());
        }
        #endregion

    }
}

