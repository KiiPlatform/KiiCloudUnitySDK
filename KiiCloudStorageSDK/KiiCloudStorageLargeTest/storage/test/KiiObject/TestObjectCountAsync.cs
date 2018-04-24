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
    public class TestObjectCountAsync : LargeTestBase
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
            
            string bucketName = "TestBucket" + CurrentTimeMillis ();
            KiiBucket bucket = Kii.Bucket (bucketName);
            
            KiiBucket callbackBucket = null;
            KiiQuery callbackQuery = null;
            int count = -1;
            Exception exp = null;
            CountDownLatch cd = new CountDownLatch (1);
            bucket.Count ((KiiBucket b, KiiQuery q, int c, Exception e) => {
                callbackBucket = b;
                callbackQuery = q;
                count = c;
                exp = e;
                cd.Signal ();
            });
 
            if (!cd.Wait (new TimeSpan (0, 0, 0, 3)))
                Assert.Fail ("Callback not fired.");
            Assert.IsNotNull (callbackBucket);
            Assert.AreEqual (bucket.Uri, callbackBucket.Uri);
            Assert.IsNotNull (callbackQuery);
            Assert.AreEqual (new KiiQuery (null).ToString (), callbackQuery.ToString ());
            Assert.IsNull (exp);
            Assert.AreEqual (0, count);
        }

        [Test()]
        public void Test_1_2_CountAllWhenObjectExistsInBucket ()
        {
            
            string bucketName = "TestBucket" + CurrentTimeMillis ();
            KiiBucket bucket = Kii.Bucket (bucketName);
            for (int i = 0; i < 10; i++) {
                KiiObject obj = bucket.NewKiiObject ();
                obj ["intField"] = i;
                obj.Save ();
            }
            
            KiiBucket callbackBucket = null;
            KiiQuery callbackQuery = null;
            int count = -1;
            Exception exp = null;
            CountDownLatch cd = new CountDownLatch (1);
            bucket.Count ((KiiBucket b, KiiQuery q, int c, Exception e) => {
                callbackBucket = b;
                callbackQuery = q;
                count = c;
                exp = e;
                cd.Signal ();
            });
            
            if (!cd.Wait (new TimeSpan (0, 0, 0, 3)))
                Assert.Fail ("Callback not fired.");
            Assert.IsNotNull (callbackBucket);
            Assert.AreEqual (bucket.Uri, callbackBucket.Uri);
            Assert.IsNotNull (callbackQuery);
            Assert.AreEqual (new KiiQuery (null).ToString (), callbackQuery.ToString ());
            Assert.IsNull (exp);
            Assert.AreEqual (10, count);
        }

        [Test()]
        public void Test_2_1_CountWithQueryWhenObjectNotExists ()
        {
            
            string bucketName = "TestBucket" + CurrentTimeMillis ();
            KiiBucket bucket = Kii.Bucket (bucketName);
            
            KiiBucket callbackBucket = null;
            KiiQuery callbackQuery = null;
            int count = -1;
            Exception exp = null;
            CountDownLatch cd = new CountDownLatch (1);
            KiiQuery query = new KiiQuery (null);
            bucket.Count (query, (KiiBucket b, KiiQuery q, int c, Exception e) => {
                callbackBucket = b;
                callbackQuery = q;
                count = c;
                exp = e;
                cd.Signal ();
            });
            
            if (!cd.Wait (new TimeSpan (0, 0, 0, 3)))
                Assert.Fail ("Callback not fired.");
            Assert.IsNotNull (callbackBucket);
            Assert.AreEqual (bucket.Uri, callbackBucket.Uri);
            Assert.IsNotNull (callbackQuery);
            Assert.AreEqual (query.ToString (), callbackQuery.ToString ());
            Assert.IsNull (exp);
            Assert.AreEqual (0, count);
        }

        [Test()]
        public void Test_2_2_CountWithQueryWhenObjectExistsInBucket ()
        {
            
            string bucketName = "TestBucket" + CurrentTimeMillis ();
            KiiBucket bucket = Kii.Bucket (bucketName);
            for (int i = 0; i < 10; i++) {
                KiiObject obj = bucket.NewKiiObject ();
                obj ["key"] = "value";
                obj.Save ();
            }
            
            KiiBucket callbackBucket = null;
            KiiQuery callbackQuery = null;
            int count = -1;
            Exception exp = null;
            CountDownLatch cd = new CountDownLatch (1);

            KiiClause clause = KiiClause.Equals ("key", "value");
            KiiQuery query = new KiiQuery (clause);
            bucket.Count (query, (KiiBucket b, KiiQuery q, int c, Exception e) => {
                callbackBucket = b;
                callbackQuery = q;
                count = c;
                exp = e;
                cd.Signal ();
            });
            
            if (!cd.Wait (new TimeSpan (0, 0, 0, 3)))
                Assert.Fail ("Callback not fired.");
            Assert.IsNotNull (callbackBucket);
            Assert.AreEqual (bucket.Uri, callbackBucket.Uri);
            Assert.IsNotNull (callbackQuery);
            Assert.AreEqual (query.ToString (), callbackQuery.ToString ());
            Assert.IsNull (exp);
            Assert.AreEqual (10, count);
        }
    }
}
