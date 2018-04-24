using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiBucketACL_async
    {
        private MockHttpClient client;

        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);

            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            Kii.AsyncHttpClientFactory = factory;

            client = factory.Client;
            if (KiiUser.CurrentUser != null)
            {
                KiiUser.LogOut();
            }
        }

        [TearDown()]
        public void TearDown()
        {
            Kii.Instance = null;
        }

        private void SetStandardGetResponse(MockHttpClient client)
        {
            client.AddResponse(200, "{" +
                               "\"QUERY_OBJECTS_IN_BUCKET\":[" +
                               "{\"userID\":\"user1234\"}" +
                               "]," +
                               "\"CREATE_OBJECTS_IN_BUCKET\":[" +
                               "{\"userID\":\"user5678\"}" +
                               "]" +
                               "}"
                               );
        }

        private void SetDefaultQueryResult(MockHttpClient client)
        {
            client.AddResponse(200, "{" +
                "\"queryDescription\" : \"WHERE ( 1 = 1 )\"," +
                "\"results\":[" +
                "{\"_id\":\"497fd6ff-9178-42ec-b6ec-14bce7b5c7c9\",\"name\":\"John Smith\",\"age\":18," +
                "\"_created\":1334505527480,\"_modified\":1334505527480,\"_owner\":\"789399f7-7552-47a8-a524-b9119056edd9\",\"_version\":1}" +
                "]}");
        }

        private void SetDefaultDeleteResult(MockHttpClient client)
        {
            client.AddResponse(204, "");
        }

        private void LogIn(string userId)
        {
            client.Clear();
            // set Response
            client.AddResponse(200, "{" +
                "\"id\" : \"" + userId + "\"," +
                "\"access_token\" : \"cdef\"," +
                "\"expires_in\" : 9223372036854775}");
            KiiUser.LogIn(userId, "pass1234");
            client.Clear();
        }

        #region KiiBucket.ListAclEntries(KiiACLListCallback)
        
        [Test(), KiiUTInfo(
            action = "When we call ListAclEntries(callback) and Server returns 2 KiiUser entries,",
            expected = "We can get 2 entries whose subject must be KiiUser"
            )]
        public void Test_1000_ListAclEntries ()
        {
            LogIn("test-user-00001");
            // set response
            this.SetStandardGetResponse(client);
            
            KiiBucket bucket = Kii.Bucket("test");
            IList<KiiACLEntry<KiiBucket, BucketAction>> list = null;
            Exception exception = null;
            bucket.ListAclEntries((IList<KiiACLEntry<KiiBucket, BucketAction>> entries, Exception e) =>
            {
                list = entries;
                exception = e;
            });
            // Assertion
            Assert.IsNotNull(list);
            Assert.IsNull(exception);
            Assert.AreEqual(2, list.Count);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call ListAclEntries(callback) and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_1001_ListAclEntries_server_error ()
        {
            LogIn("test-user-00001");
            // set response
            client.AddResponse(new CloudException(400, "{}"));
            
            KiiBucket bucket = Kii.Bucket("test");
            
            // list
            IList<KiiACLEntry<KiiBucket, BucketAction>> list = null;
            Exception exception = null;
            bucket.ListAclEntries((IList<KiiACLEntry<KiiBucket, BucketAction>> entries, Exception e) =>
            {
                list = entries;
                exception = e;
            });
            // Assertion
            Assert.IsNull(list);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is CloudException);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call ListAclEntries(callback) and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_1002_ListAclEntries_broken_json ()
        {
            LogIn("test-user-00001");
            // set response
            client.AddResponse(200, "broken");
            
            KiiBucket bucket = Kii.Bucket("test");
            
            // list
            IList<KiiACLEntry<KiiBucket, BucketAction>> list = null;
            Exception exception = null;
            bucket.ListAclEntries((IList<KiiACLEntry<KiiBucket, BucketAction>> entries, Exception e) =>
            {
                list = entries;
                exception = e;
            });
            // Assertion
            Assert.IsNull(list);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is IllegalKiiBaseObjectFormatException);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call ListAclEntries(callback) and Server returns unknown action,",
            expected = "We can get only entries whose actions are known"
            )]
        public void Test_1003_ListAclEntries_unknown_action ()
        {
            LogIn("test-user-00001");
            // set response
            client.AddResponse(200, "{" +
                                    "\"QUERY_OBJECTS_IN_BUCKET\":[" +
                                    "{\"userID\":\"user1234\"}" +
                                    "]," +
                                    "\"READ_EXISTING_OBJECT\":[" +
                                    "{\"userID\":\"user5678\"}" +
                                    "]," +
                                    "\"DROP_BUCKET_WITH_ALL_CONTENT\":[" +
                                    "{\"groupID\":\"group5678\"}" +
                                    "]" +
                                    "}"
                                    );
            
            KiiBucket bucket = Kii.Bucket("test");
            
            // list
            IList<KiiACLEntry<KiiBucket, BucketAction>> entries = null;
            Exception exception = null;
            bucket.ListAclEntries((IList<KiiACLEntry<KiiBucket, BucketAction>> list, Exception e) =>
            {
                entries = list;
                exception = e;
            });
            // Assertion
            Assert.IsNotNull(entries);
            Assert.IsNull(exception);

            Assert.AreEqual(2, entries.Count);
            
            // entry 1
            KiiACLEntry<KiiBucket, BucketAction> entry = entries[0];
            Assert.AreEqual(BucketAction.DROP_BUCKET_WITH_ALL_CONTENT, entry.Action);
            Assert.IsTrue(entry.Subject is KiiGroup);
            Assert.AreEqual("kiicloud://groups/group5678", ((KiiGroup)entry.Subject).Uri.ToString());
            
            // entry 2
            entry = entries[1];
            Assert.AreEqual(BucketAction.QUERY_OBJECTS_IN_BUCKET, entry.Action);
            Assert.IsTrue(entry.Subject is KiiUser);
            Assert.AreEqual("kiicloud://users/user1234", ((KiiUser)entry.Subject).Uri.ToString());
        }
        
        [Test(), KiiUTInfo(
            action = "When we call ListAclEntries() and Server returns entry whose id field is unknown,",
            expected = "We can get only entries whose id fields are known"
            )]
        public void Test_1004_ListAclEntries_unknown_id ()
        {
            LogIn("test-user-00001");
            // set response
            client.AddResponse(200, "{" +
                                    "\"QUERY_OBJECTS_IN_BUCKET\":[" +
                                    "{\"userID\":\"user1234\"}" +
                                    "]," +
                                    "\"WRITE_EXISTING_OBJECT\":[" +
                                    "{\"objectID\":\"object5678\"}" +
                                    "]" +
                                    "}"
                                    );
            
            KiiBucket bucket = Kii.Bucket("test");
            
            // list
            IList<KiiACLEntry<KiiBucket, BucketAction>> entries = null;
            Exception exception = null;
            bucket.ListAclEntries((IList<KiiACLEntry<KiiBucket, BucketAction>> list, Exception e) =>
            {
                entries = list;
                exception = e;
            });
            // Assertion
            Assert.IsNotNull(entries);
            Assert.IsNull(exception);

            Assert.AreEqual(1, entries.Count);
            
            // entry 1
            KiiACLEntry<KiiBucket, BucketAction> entry = entries[0];
            Assert.AreEqual(BucketAction.QUERY_OBJECTS_IN_BUCKET, entry.Action);
            Assert.IsTrue(entry.Subject is KiiUser);
            Assert.AreEqual("kiicloud://users/user1234", ((KiiUser)entry.Subject).Uri.ToString());
        }
        
        [Test(), KiiUTInfo(
            action = "When we call ListAclEntries(callback) and Server returns only QUERY entry,",
            expected = "We can get only QUERY entry(not crashed)"
            )]
        public void Test_1005_ListAclEntries_QUERY_ONLY ()
        {
            LogIn("test-user-00001");
            // set response
            client.AddResponse(200, "{" +
                                    "\"QUERY_OBJECTS_IN_BUCKET\":[" +
                                    "{\"userID\":\"user1234\"}" +
                                    "]" +
                                    "}"
                                    );
            
            KiiBucket bucket = Kii.Bucket("test");
            
            // list
            IList<KiiACLEntry<KiiBucket, BucketAction>> entries = null;
            Exception exception = null;
            bucket.ListAclEntries((IList<KiiACLEntry<KiiBucket, BucketAction>> list, Exception e) =>
            {
                entries = list;
                exception = e;
            });
            // Assertion
            Assert.IsNotNull(entries);
            Assert.IsNull(exception);

            Assert.AreEqual(1, entries.Count);
            
            // entry 1
            KiiACLEntry<KiiBucket, BucketAction> entry = entries[0];
            Assert.AreEqual(BucketAction.QUERY_OBJECTS_IN_BUCKET, entry.Action);
            Assert.IsTrue(entry.Subject is KiiUser);
            Assert.AreEqual("kiicloud://users/user1234", ((KiiUser)entry.Subject).Uri.ToString());
        }
        #endregion
    }
}

