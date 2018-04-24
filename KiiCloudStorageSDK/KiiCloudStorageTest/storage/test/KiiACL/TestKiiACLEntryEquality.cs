using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiACLEntryEquality
    {
        private MockHttpClient client;

        public TestKiiACLEntryEquality()
        {
        }

        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            Kii.AsyncHttpClientFactory = factory;
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

        [Test()]
        public void KiiBucketAclTest()
        {
            KiiBucketAcl acl1 = new KiiBucketAcl(Kii.Bucket("AA"), BucketAction.CREATE_OBJECTS_IN_BUCKET);
            KiiBucketAcl acl2 = new KiiBucketAcl(Kii.Bucket("AA"), BucketAction.CREATE_OBJECTS_IN_BUCKET);

            KiiUser userA  = KiiUser.CreateByUri(new Uri("kiicloud://users/userA"));
            KiiUser userA_ = KiiUser.CreateByUri(new Uri("kiicloud://users/userA"));
            KiiUser userB  = KiiUser.CreateByUri(new Uri("kiicloud://users/userB"));
            KiiUser userC  = KiiUser.CreateByUri(new Uri("kiicloud://users/C"));

            KiiGroup groupA  = KiiGroup.CreateByUri(new Uri("kiicloud://groups/groupA"));
            KiiGroup groupA_ = KiiGroup.CreateByUri(new Uri("kiicloud://groups/groupA"));
            KiiGroup groupB  = KiiGroup.CreateByUri(new Uri("kiicloud://groups/groupB"));
            KiiGroup groupC  = KiiGroup.CreateByUri(new Uri("kiicloud://groups/C"));

            KiiACLEntry<KiiBucket, BucketAction> aclEntry1 = null;
            KiiACLEntry<KiiBucket, BucketAction> aclEntry2 = null;

            // same user
            aclEntry1 = new KiiACLEntry<KiiBucket, BucketAction>(acl1, userA);
            aclEntry2 = new KiiACLEntry<KiiBucket, BucketAction>(acl2, userA_);
            Assert.IsTrue(aclEntry1.Equals(aclEntry2));
            Assert.IsTrue(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // same user
            aclEntry1 = new KiiACLEntry<KiiBucket, BucketAction>(acl1, KiiAnonymousUser.Get());
            aclEntry2 = new KiiACLEntry<KiiBucket, BucketAction>(acl2, KiiAnonymousUser.Get());
            Assert.IsTrue(aclEntry1.Equals(aclEntry2));
            Assert.IsTrue(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // different user
            aclEntry1 = new KiiACLEntry<KiiBucket, BucketAction>(acl1, userA);
            aclEntry2 = new KiiACLEntry<KiiBucket, BucketAction>(acl2, userB);
            Assert.IsFalse(aclEntry1.Equals(aclEntry2));
            Assert.IsFalse(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // different user
            aclEntry1 = new KiiACLEntry<KiiBucket, BucketAction>(acl1, KiiAnonymousUser.Get());
            aclEntry2 = new KiiACLEntry<KiiBucket, BucketAction>(acl2, KiiAnyAuthenticatedUser.Get());
            Assert.IsFalse(aclEntry1.Equals(aclEntry2));
            Assert.IsFalse(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // same group
            aclEntry1 = new KiiACLEntry<KiiBucket, BucketAction>(acl1, groupA);
            aclEntry2 = new KiiACLEntry<KiiBucket, BucketAction>(acl2, groupA_);
            Assert.IsTrue(aclEntry1.Equals(aclEntry2));
            Assert.IsTrue(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // different group
            aclEntry1 = new KiiACLEntry<KiiBucket, BucketAction>(acl1, groupA);
            aclEntry2 = new KiiACLEntry<KiiBucket, BucketAction>(acl2, groupB);
            Assert.IsFalse(aclEntry1.Equals(aclEntry2));
            Assert.IsFalse(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // different subject kind
            aclEntry1 = new KiiACLEntry<KiiBucket, BucketAction>(acl1, userC);
            aclEntry2 = new KiiACLEntry<KiiBucket, BucketAction>(acl2, groupC);
            Assert.IsFalse(aclEntry1.Equals(aclEntry2));
            Assert.IsFalse(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
        }
        [Test()]
        public void KiiObjectAclTest()
        {
            KiiObject objectA  = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/a"));
            KiiObjectAcl acl1 = new KiiObjectAcl(objectA, ObjectAction.READ_EXISTING_OBJECT);
            KiiObjectAcl acl2 = new KiiObjectAcl(objectA, ObjectAction.READ_EXISTING_OBJECT);

            KiiUser userA  = KiiUser.CreateByUri(new Uri("kiicloud://users/userA"));
            KiiUser userA_ = KiiUser.CreateByUri(new Uri("kiicloud://users/userA"));
            KiiUser userB  = KiiUser.CreateByUri(new Uri("kiicloud://users/userB"));
            KiiUser userC  = KiiUser.CreateByUri(new Uri("kiicloud://users/C"));
            
            KiiGroup groupA  = KiiGroup.CreateByUri(new Uri("kiicloud://groups/groupA"));
            KiiGroup groupA_ = KiiGroup.CreateByUri(new Uri("kiicloud://groups/groupA"));
            KiiGroup groupB  = KiiGroup.CreateByUri(new Uri("kiicloud://groups/groupB"));
            KiiGroup groupC  = KiiGroup.CreateByUri(new Uri("kiicloud://groups/C"));

            KiiACLEntry<KiiObject, ObjectAction> aclEntry1 = null;
            KiiACLEntry<KiiObject, ObjectAction> aclEntry2 = null;

            // same user
            aclEntry1 = new KiiACLEntry<KiiObject, ObjectAction>(acl1, userA);
            aclEntry2 = new KiiACLEntry<KiiObject, ObjectAction>(acl2, userA_);
            Assert.IsTrue(aclEntry1.Equals(aclEntry2));
            Assert.IsTrue(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // same user
            aclEntry1 = new KiiACLEntry<KiiObject, ObjectAction>(acl1, KiiAnonymousUser.Get());
            aclEntry2 = new KiiACLEntry<KiiObject, ObjectAction>(acl2, KiiAnonymousUser.Get());
            Assert.IsTrue(aclEntry1.Equals(aclEntry2));
            Assert.IsTrue(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // different user
            aclEntry1 = new KiiACLEntry<KiiObject, ObjectAction>(acl1, userA);
            aclEntry2 = new KiiACLEntry<KiiObject, ObjectAction>(acl2, userB);
            Assert.IsFalse(aclEntry1.Equals(aclEntry2));
            Assert.IsFalse(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // different user
            aclEntry1 = new KiiACLEntry<KiiObject, ObjectAction>(acl1, KiiAnonymousUser.Get());
            aclEntry2 = new KiiACLEntry<KiiObject, ObjectAction>(acl2, KiiAnyAuthenticatedUser.Get());
            Assert.IsFalse(aclEntry1.Equals(aclEntry2));
            Assert.IsFalse(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // same group
            aclEntry1 = new KiiACLEntry<KiiObject, ObjectAction>(acl1, groupA);
            aclEntry2 = new KiiACLEntry<KiiObject, ObjectAction>(acl2, groupA_);
            Assert.IsTrue(aclEntry1.Equals(aclEntry2));
            Assert.IsTrue(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // different group
            aclEntry1 = new KiiACLEntry<KiiObject, ObjectAction>(acl1, groupA);
            aclEntry2 = new KiiACLEntry<KiiObject, ObjectAction>(acl2, groupB);
            Assert.IsFalse(aclEntry1.Equals(aclEntry2));
            Assert.IsFalse(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // different subject kind
            aclEntry1 = new KiiACLEntry<KiiObject, ObjectAction>(acl1, userC);
            aclEntry2 = new KiiACLEntry<KiiObject, ObjectAction>(acl2, groupC);
            Assert.IsFalse(aclEntry1.Equals(aclEntry2));
            Assert.IsFalse(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
        }
        [Test()]
        public void KiiTopicAclTest()
        {
            KiiTopicACL acl1 = new KiiTopicACL(Kii.Topic("A"));
            KiiTopicACL acl2 = new KiiTopicACL(Kii.Topic("A"));

            KiiUser userA  = KiiUser.CreateByUri(new Uri("kiicloud://users/userA"));
            KiiUser userA_ = KiiUser.CreateByUri(new Uri("kiicloud://users/userA"));
            KiiUser userB  = KiiUser.CreateByUri(new Uri("kiicloud://users/userB"));
            KiiUser userC  = KiiUser.CreateByUri(new Uri("kiicloud://users/C"));
            
            KiiGroup groupA  = KiiGroup.CreateByUri(new Uri("kiicloud://groups/groupA"));
            KiiGroup groupA_ = KiiGroup.CreateByUri(new Uri("kiicloud://groups/groupA"));
            KiiGroup groupB  = KiiGroup.CreateByUri(new Uri("kiicloud://groups/groupB"));
            KiiGroup groupC  = KiiGroup.CreateByUri(new Uri("kiicloud://groups/C"));

            KiiACLEntry<KiiTopic, TopicAction> aclEntry1 = null;
            KiiACLEntry<KiiTopic, TopicAction> aclEntry2 = null;
            
            // same user
            aclEntry1 = new KiiACLEntry<KiiTopic, TopicAction>(acl1, userA);
            aclEntry2 = new KiiACLEntry<KiiTopic, TopicAction>(acl2, userA_);
            Assert.IsTrue(aclEntry1.Equals(aclEntry2));
            Assert.IsTrue(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // same user
            aclEntry1 = new KiiACLEntry<KiiTopic, TopicAction>(acl1, KiiAnonymousUser.Get());
            aclEntry2 = new KiiACLEntry<KiiTopic, TopicAction>(acl2, KiiAnonymousUser.Get());
            Assert.IsTrue(aclEntry1.Equals(aclEntry2));
            Assert.IsTrue(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // different user
            aclEntry1 = new KiiACLEntry<KiiTopic, TopicAction>(acl1, userA);
            aclEntry2 = new KiiACLEntry<KiiTopic, TopicAction>(acl2, userB);
            Assert.IsFalse(aclEntry1.Equals(aclEntry2));
            Assert.IsFalse(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // different user
            aclEntry1 = new KiiACLEntry<KiiTopic, TopicAction>(acl1, KiiAnonymousUser.Get());
            aclEntry2 = new KiiACLEntry<KiiTopic, TopicAction>(acl2, KiiAnyAuthenticatedUser.Get());
            Assert.IsFalse(aclEntry1.Equals(aclEntry2));
            Assert.IsFalse(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // same group
            aclEntry1 = new KiiACLEntry<KiiTopic, TopicAction>(acl1, groupA);
            aclEntry2 = new KiiACLEntry<KiiTopic, TopicAction>(acl2, groupA_);
            Assert.IsTrue(aclEntry1.Equals(aclEntry2));
            Assert.IsTrue(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // different group
            aclEntry1 = new KiiACLEntry<KiiTopic, TopicAction>(acl1, groupA);
            aclEntry2 = new KiiACLEntry<KiiTopic, TopicAction>(acl2, groupB);
            Assert.IsFalse(aclEntry1.Equals(aclEntry2));
            Assert.IsFalse(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
            // different subject kind
            aclEntry1 = new KiiACLEntry<KiiTopic, TopicAction>(acl1, userC);
            aclEntry2 = new KiiACLEntry<KiiTopic, TopicAction>(acl2, groupC);
            Assert.IsFalse(aclEntry1.Equals(aclEntry2));
            Assert.IsFalse(aclEntry1.GetHashCode() == aclEntry2.GetHashCode());
            Assert.IsFalse(aclEntry1 == aclEntry2);
        }
    }
}

