using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;
using JsonOrg;

// TestSpec : https://docs.google.com/a/kii.com/spreadsheets/d/1CcPDmQUnoiAff1QxykrHuFGq_8rMOijSaRD-KBJLXQw/edit#gid=0
namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestPushToAppMessageParse
    {
        
        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
        }

        class ExpectResult
        {
            string value;

            public ExpectResult(string value)
            {
                this.value = value;
            }

            public string Value
            {
                get
                {
                    return this.value;
                }
            }
        }

        public enum Scope
        {
            APP,
            APP_AND_GROUP,
            APP_AND_USER
        }

        class TestCase
        {
            internal string senderID;
            internal Scope? scope;
            internal string origin;
            internal string bucketType;
            internal string bucketID;
            internal string objectID;
            internal string groupID;
            internal string userID;
            internal ExpectResult result;

            public TestCase(string origin, string senderID, Scope? scope,
                     string bucketType, string bucketID, string objectID,
                     string groupID, string userID, ExpectResult result)
            {
                this.senderID = senderID;
                this.scope = scope;
                this.origin = origin;
                this.bucketType = bucketType;
                this.bucketID = bucketID;
                this.objectID = objectID;
                this.groupID = groupID;
                this.userID = userID;
                this.result = result;
            }

            public string GetPushMessage()
            {
                JsonObject json = new JsonObject();
                if (this.origin != null)
                    json.Put("origin", this.origin);
                if (this.senderID != null)
                    json.Put("sender", this.senderID);
                if (this.bucketID != null)
                    json.Put("bucketID", this.bucketID);
                if (this.bucketType != null)
                    json.Put("bucketType", this.bucketType);
                if (this.userID != null)
                    json.Put("objectScopeUserID", this.userID);
                if (this.groupID != null)
                    json.Put("objectScopeGroupID", this.groupID);
                if (this.scope.HasValue)
                    json.Put("objectScopeType", this.scope.ToString());
                if (this.objectID != null)
                    json.Put("objectID", this.objectID);
                return json.ToString();
            }
        }


        TestCase[] SENDER_TEST_CASES = {
            new TestCase("EVENT", "ab-12", null, null, null, null, null, null,
                         new ExpectResult("kiicloud://users/ab-12")) 
        };
        TestCase[] BUCKET_TEST_CASES = {
            new TestCase(null, null, null, "rw", "bkt", null, null, null,
                         new ExpectResult(null)),
            new TestCase(null, null, Scope.APP, "rw", "bkt", null, null, null,
                         new ExpectResult("kiicloud://buckets/bkt")),
            new TestCase(null, null, Scope.APP_AND_GROUP, "rw", "bkt", null,
                         "gid", null, new ExpectResult(
                "kiicloud://groups/gid/buckets/bkt")),
            new TestCase(null, null, Scope.APP_AND_GROUP, "rw", "bkt", null,
                         null, null, new ExpectResult(null)),
            new TestCase(null, null, Scope.APP_AND_USER, "rw", "bkt", null,
                         null, "uid", new ExpectResult(
                "kiicloud://users/uid/buckets/bkt")),
            new TestCase(null, null, Scope.APP_AND_USER, "rw", "bkt", null,
                         null, null, new ExpectResult(null)),
            new TestCase("EVENT", null, Scope.APP, null, null, null, null, null,
                         new ExpectResult(null))
        };
        TestCase[] OBJECT_TEST_CASES = {
            new TestCase(null, null, Scope.APP, "rw", "bkt", "objId", null,
                         null, new ExpectResult(
                "kiicloud://buckets/bkt/objects/objId")),
            new TestCase(null, null, Scope.APP, null, "bkt", null, null, null,
                         new ExpectResult(null)),
            new TestCase(null, null, Scope.APP, "rw", "bkt", null, "", null,
                         new ExpectResult(null)),
            new TestCase(null, null, Scope.APP_AND_GROUP, "rw", "bkt", "objId",
                         "gid", null, new ExpectResult(
                "kiicloud://groups/gid/buckets/bkt/objects/objId")),
            new TestCase(null, null, Scope.APP_AND_GROUP, "rw", "bkt", null,
                         null, null, new ExpectResult(null)),
            new TestCase(null, null, Scope.APP_AND_USER, "rw", "bkt", "objId",
                         null, "uid", new ExpectResult(
                "kiicloud://users/uid/buckets/bkt/objects/objId")),
            new TestCase(null, null, Scope.APP_AND_USER, "rw", "bkt", null,
                         null, "uid", new ExpectResult(null))
        };
        TestCase[] MSG_TYPE_TEST_CASES = {
            new TestCase("EVENT", null, null, "rw", "bkt", null, null, null, new ExpectResult("PUSH_TO_APP")),
            new TestCase(null, null, null, null, "bkt", null, null, null, new ExpectResult("PUSH_TO_APP")),
            new TestCase(null, null, null, "rw", null, null, null, null, new ExpectResult("PUSH_TO_APP")),
            new TestCase("EVENT", null, null, null, null, null, null, null, new ExpectResult("PUSH_TO_APP"))
        };

        [Test(), TestCaseNumber("1-1")]
        public void TestParseSender()
        {
            List<string> errors = new List<string>();
            for (int i = 0; i < SENDER_TEST_CASES.Length; i++)
            {
                TestCase tc = SENDER_TEST_CASES[i];
                PushToAppMessage message = (PushToAppMessage)ReceivedMessage.Parse(tc.GetPushMessage());
                ExpectResult result = tc.result;
                try
                {
                    if (result.Value == null)
                    {
                        Assert.IsNull(message.Sender);
                    }
                    else
                    {
                        Assert.IsNotNull(message.Sender);
                        Assert.AreEqual(result.Value, message.Sender.Uri.ToString());
                    }
                }
                catch (AssertionException e)
                {
                    errors.Add("TestCase: " + (i + 1) + " message:" + e.Message);
                }
            }
            if (errors.Count > 0)
            {
                StringBuilder sb = new StringBuilder("One or More testcase failed. \n");
                foreach (string error in errors)
                {
                    sb.Append(error);
                }
                Assert.Fail(sb.ToString());
            }
        }

        [Test(), TestCaseNumber("2-1 to 2-7")]
        public void TestParseKiiBucket()
        {
            List<string> errors = new List<string>();
            for (int i = 0; i < BUCKET_TEST_CASES.Length; i++)
            {
                TestCase tc = BUCKET_TEST_CASES[i];
                PushToAppMessage message = (PushToAppMessage)ReceivedMessage.Parse(tc.GetPushMessage());
                ExpectResult result = tc.result;
                try
                {
                    Assert.IsNotNull(message);
                    if (result.Value == null)
                    {
                        Assert.IsNull(message.KiiBucket);
                        Assert.IsFalse(message.ContainsKiiBucket());
                    }
                    else
                    {
                        Assert.IsNotNull(message.KiiBucket);
                        Assert.AreEqual(result.Value, message.KiiBucket.Uri.ToString());
                        Assert.IsTrue(message.ContainsKiiBucket());
                    }
                }
                catch (AssertionException e)
                {
                    errors.Add("TestCase: " + (i + 1) + " message:" + e.Message);
                }
            }
            if (errors.Count > 0)
            {
                StringBuilder sb = new StringBuilder("One or More testcase failed. \n");
                foreach (string error in errors)
                {
                    sb.Append(error);
                }
                Assert.Fail(sb.ToString());
            }
        }

        [Test(), TestCaseNumber("3-1 to 3-7")]
        public void TestParseKiiObject()
        {
            List<string> errors = new List<string>();
            for (int i = 0; i < OBJECT_TEST_CASES.Length; i++)
            {
                TestCase tc = OBJECT_TEST_CASES[i];
                PushToAppMessage message = (PushToAppMessage)ReceivedMessage.Parse(tc.GetPushMessage());
                ExpectResult result = tc.result;
                Console.WriteLine("No: " + (i + 1) + " message: " + tc.GetPushMessage());
                try
                {
                    Assert.IsNotNull(message);
                    if (result.Value == null)
                    {
                        Assert.IsFalse(message.ContainsKiiObject());
                        Assert.IsNull(message.KiiObject);
                    }
                    else
                    {
                        Assert.IsNotNull(message.KiiObject);
                        Assert.AreEqual(result.Value, message.KiiObject.Uri.ToString());
                        Assert.IsTrue(message.ContainsKiiObject());
                    }
                }
                catch (AssertionException e)
                {
                    errors.Add("TestCase: " + (i + 1) + " message:" + e.Message);
                }
            }
            if (errors.Count > 0)
            {
                StringBuilder sb = new StringBuilder("One or More testcase failed. \n");
                foreach (string error in errors)
                {
                    sb.Append(error);
                }
                Assert.Fail(sb.ToString());
            }
        }

        [Test(), TestCaseNumber("4-1 to 4-4")]
        public void TestParsePushMessageType()
        {
            List<string> errors = new List<string>();
            for (int i = 0; i < MSG_TYPE_TEST_CASES.Length; i++)
            {
                TestCase tc = MSG_TYPE_TEST_CASES[i];
                PushToAppMessage message = (PushToAppMessage)ReceivedMessage.Parse(tc.GetPushMessage());
                ExpectResult result = tc.result;
                Console.WriteLine("No: " + (i + 1) + " message: " + tc.GetPushMessage());
                try
                {
                    Assert.IsNotNull(message);
                    if (result.Value == null)
                    {
                        Assert.IsNull(message.PushMessageType);
                    }
                    else
                    {
                        Assert.AreEqual(result.Value, message.PushMessageType.ToString());
                    }
                }
                catch (AssertionException e)
                {
                    errors.Add("TestCase: " + (i + 1) + " message:" + e.Message);
                }
            }
            if (errors.Count > 0)
            {
                StringBuilder sb = new StringBuilder("One or More testcase failed. \n");
                foreach (string error in errors)
                {
                    sb.Append(error);
                }
                Assert.Fail(sb.ToString());
            }
        }
    }
}

