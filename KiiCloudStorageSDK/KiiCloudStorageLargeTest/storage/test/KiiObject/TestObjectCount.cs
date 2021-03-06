// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    // Test spec : https://docs.google.com/a/kii.com/spreadsheet/ccc?key=0AsiWA7MkWrQldHFnX3I2bXBzQlhMSlRuTG9JdUQ5X0E&usp=drive_web#gid=4
    [TestFixture()]
    public class TestObjectCount : LargeTestBase
    {

        [SetUp()]
        public override void SetUp ()
        {
            base.SetUp ();
            string uname = "kiiqueryTest-" + CurrentTimeMillis ();
            AppUtil.CreateNewUser (uname, "123456");
        }

        [TearDown()]
        public override void TearDown ()
        {
            AppUtil.DeleteUser (KiiUser.CurrentUser);
            base.TearDown ();
        }

        private long CurrentTimeMillis ()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        [Test()]
        public void Test_1_1_CountAllWhenObjectNotExists ()
        {
            string bucketName = "bucket" + CurrentTimeMillis ();
            KiiBucket bucket = Kii.Bucket (bucketName);
            int count = bucket.Count ();
            Assert.AreEqual (0, count);
        }

        [Test()]
        public void Test_1_3_CountWhenObjectExistsInBucket ()
        {
            string bucketName = "bucket" + CurrentTimeMillis ();
            KiiBucket bucket = Kii.Bucket (bucketName);
            for (int i = 0; i < 10; i++) {
                KiiObject obj = bucket.NewKiiObject ();
                obj ["intField"] = i;
                obj.Save ();
            }

            int count = bucket.Count ();
            Assert.AreEqual (10, count);
        }

        [Test()]
        public void Test_2_5_CountWithQuery ()
        {
            string bucketName = "bucket" + CurrentTimeMillis ();
            KiiBucket bucket = Kii.Bucket (bucketName);
            for (int i = 0; i < 15; i++) {
                KiiObject obj = bucket.NewKiiObject ();
                obj ["intField"] = i;
                obj.Save ();
            }

            KiiClause clause = KiiClause.GreaterThanOrEqual ("intField", 10);
            int count = bucket.Count (new KiiQuery (clause));
            Assert.AreEqual (5, count);
        }

        [Test()]
        public void Test_2_6_CountWithUnsupportedQuery ()
        {
            string bucketName = "bucket" + CurrentTimeMillis ();
            KiiBucket bucket = Kii.Bucket (bucketName);
            for (int i = 0; i < 2; i++) {
                KiiObject obj = bucket.NewKiiObject ();
                obj ["intField"] = i;
                obj.Save ();
            }
            

            KiiQuery query = new KiiQuery (null);
            query.Limit = 1;

            KiiQueryResult<KiiObject> result = bucket.Query (query);
            Assert.AreEqual (1, result.Count);
            query = result.NextKiiQuery;
            CloudException exp = null;
            try {
                bucket.Count (query);
                Assert.Fail ("Exception not thrown");
            } catch (CloudException e) {
                exp = e;
            }
            Assert.IsNotNull (exp);
            Assert.AreEqual (400, exp.Status);
            JsonObject body = new JsonObject(exp.Body);
            Assert.AreEqual ("QUERY_NOT_SUPPORTED", body.GetString("errorCode"));
        }
    }
}

