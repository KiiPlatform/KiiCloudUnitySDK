using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiACLEquality
    {
        private MockHttpClient client;

        public TestKiiACLEquality()
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

            KiiBucket appBukcetA  = Kii.Bucket("AA");
            KiiBucket appBukcetA_ = Kii.Bucket("AA");
            KiiBucket appBukcetB  = Kii.Bucket("BB");
            KiiBucket usrBucketA  = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234")).Bucket("AA");

            KiiBucketAcl acl1 = null;
            KiiBucketAcl acl2 = null;

            // same app bucket
            acl1 = new KiiBucketAcl(appBukcetA);
            acl2 = new KiiBucketAcl(appBukcetA_);
            Assert.IsTrue(acl1.Equals(acl2));
            Assert.IsTrue(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
            // same app bucket and same action
            acl1 = new KiiBucketAcl(appBukcetA, BucketAction.QUERY_OBJECTS_IN_BUCKET);
            acl2 = new KiiBucketAcl(appBukcetA_, BucketAction.QUERY_OBJECTS_IN_BUCKET);
            Assert.IsTrue(acl1.Equals(acl2));
            Assert.IsTrue(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
            // different action
            acl1 = new KiiBucketAcl(appBukcetA, BucketAction.DROP_BUCKET_WITH_ALL_CONTENT);
            acl2 = new KiiBucketAcl(appBukcetA_, BucketAction.QUERY_OBJECTS_IN_BUCKET);
            Assert.IsFalse(acl1.Equals(acl2));
            Assert.IsFalse(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
            // different bucket name
            acl1 = new KiiBucketAcl(appBukcetA, BucketAction.QUERY_OBJECTS_IN_BUCKET);
            acl2 = new KiiBucketAcl(appBukcetB, BucketAction.QUERY_OBJECTS_IN_BUCKET);
            Assert.IsFalse(acl1.Equals(acl2));
            Assert.IsFalse(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
            // different scope of bucket
            acl1 = new KiiBucketAcl(appBukcetA, BucketAction.QUERY_OBJECTS_IN_BUCKET);
            acl2 = new KiiBucketAcl(usrBucketA, BucketAction.QUERY_OBJECTS_IN_BUCKET);
            Assert.IsFalse(acl1.Equals(acl2));
            Assert.IsFalse(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
        }
        [Test()]
        public void KiiObjectAclTest()
        {
            KiiObject objectA  = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/a"));
            KiiObject objectA_ = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/a"));
            KiiObject objectB  = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/b"));

            KiiObjectAcl acl1 = null;
            KiiObjectAcl acl2 = null;

            // same object
            acl1 = new KiiObjectAcl(objectA);
            acl2 = new KiiObjectAcl(objectA_);
            Assert.IsTrue(acl1.Equals(acl2));
            Assert.IsTrue(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
            // same object and action
            acl1 = new KiiObjectAcl(objectA, ObjectAction.READ_EXISTING_OBJECT);
            acl2 = new KiiObjectAcl(objectA_, ObjectAction.READ_EXISTING_OBJECT);
            Assert.IsTrue(acl1.Equals(acl2));
            Assert.IsTrue(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
            // different object
            acl1 = new KiiObjectAcl(objectA);
            acl2 = new KiiObjectAcl(objectB);
            Assert.IsFalse(acl1.Equals(acl2));
            Assert.IsFalse(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
            // different action
            acl1 = new KiiObjectAcl(objectA, ObjectAction.WRITE_EXISTING_OBJECT);
            acl2 = new KiiObjectAcl(objectA, ObjectAction.READ_EXISTING_OBJECT);
            Assert.IsFalse(acl1.Equals(acl2));
            Assert.IsFalse(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
        }
        [Test()]
        public void KiiTopicAclTest()
        {
            this.LogIn();

            KiiTopic appTopicA = Kii.Topic("A");
            KiiTopic appTopicA_ = Kii.Topic("A");
            KiiTopic appTopicB = Kii.Topic("B");
            KiiTopic usrTopicA = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234")).Topic("A");

            KiiTopicACL acl1 = null;
            KiiTopicACL acl2 = null;

            // same object
            acl1 = new KiiTopicACL(appTopicA);
            acl2 = new KiiTopicACL(appTopicA_);
            Assert.IsTrue(acl1.Equals(acl2));
            Assert.IsTrue(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
            // same object and action
            acl1 = new KiiTopicACL(appTopicA, TopicAction.SEND_MESSAGE_TO_TOPIC);
            acl2 = new KiiTopicACL(appTopicA_, TopicAction.SEND_MESSAGE_TO_TOPIC);
            Assert.IsTrue(acl1.Equals(acl2));
            Assert.IsTrue(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
            // different action
            acl1 = new KiiTopicACL(appTopicA, TopicAction.SEND_MESSAGE_TO_TOPIC);
            acl2 = new KiiTopicACL(appTopicA_, TopicAction.SUBSCRIBE_TO_TOPIC);
            Assert.IsFalse(acl1.Equals(acl2));
            Assert.IsFalse(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
            // different topic name
            acl1 = new KiiTopicACL(appTopicA, TopicAction.SEND_MESSAGE_TO_TOPIC);
            acl2 = new KiiTopicACL(appTopicB, TopicAction.SEND_MESSAGE_TO_TOPIC);
            Assert.IsFalse(acl1.Equals(acl2));
            Assert.IsFalse(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
            // different scope of bucket
            acl1 = new KiiTopicACL(appTopicA, TopicAction.SEND_MESSAGE_TO_TOPIC);
            acl2 = new KiiTopicACL(usrTopicA, TopicAction.SEND_MESSAGE_TO_TOPIC);
            Assert.IsFalse(acl1.Equals(acl2));
            Assert.IsFalse(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
        }
        [Test()]
        public void KiiNotSavedObjectAclTest()
        {
            KiiObject objectA  = Kii.Bucket("app_bucket").NewKiiObject();

            KiiObjectAcl acl1 = null;
            KiiObjectAcl acl2 = null;
            acl1 = new KiiObjectAcl(objectA);
            acl2 = new KiiObjectAcl(objectA);
            Assert.IsFalse(acl1.Equals(acl2));
            Assert.IsTrue(acl1.GetHashCode() == acl2.GetHashCode());
            Assert.IsFalse(acl1 == acl2);
        }
    }
}

