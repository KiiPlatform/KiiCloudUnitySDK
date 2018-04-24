using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiPushMessageData
    {
        #region KiiPushMessageData
        [Test(), KiiUTInfo(
            action = "When we call Put(key, value) and ToJsonObject()",
            expected = "We can get a JsonObject which contains put fields."
            )]
        public void Test_0001_KiiPushMessageData()
        {
            KiiPushMessageData data = new KiiPushMessageData ();
            data.Put ("string", "abc")
                .Put ("int", 10)
                .Put ("long", 1000L)
                .Put ("double", 10.05)
                .Put ("bool", false);

            JsonObject json = data.ToJsonObject ();
            Assert.AreEqual (json.Get("string"), "abc");
            Assert.AreEqual (json.Get("int"), 10);
            Assert.AreEqual (json.Get("long"), 1000L);
            Assert.AreEqual (json.Get("double"), 10.05);
            Assert.AreEqual (json.Get("bool"), false);
        }
        [Test(),
         ExpectedException(typeof(ArgumentException)),
         KiiUTInfo(
            action = "When we call Put(key, value) with empty key",
            expected = "ArgumentException must be thrown."
            )]
        public void Test_0002_KiiPushMessageData_InvalidKey()
        {
            KiiPushMessageData data = new KiiPushMessageData ();
            data.Put ("", "abc");
        }
        #endregion
        
        #region GCMData
        [Test(),
         ExpectedException(typeof(ArgumentException)),
         KiiUTInfo(
            action = "When we call Put(key, value) with 'from' key",
            expected = "ArgumentException must be thrown."
            )]
        public void Test_0003_GCMData_InvalidKey_from()
        {
            GCMData data = new GCMData ();
            data.Put ("from", "abc");
        }
        [Test(),
         ExpectedException(typeof(ArgumentException)),
         KiiUTInfo(
            action = "When we call Put(key, value) with 'registration_ids' key",
            expected = "ArgumentException must be thrown."
            )]
        public void Test_0004_GCMData_InvalidKey_registration_ids()
        {
            GCMData data = new GCMData ();
            data.Put ("registration_ids", "abc");
        }
        [Test(),
         ExpectedException(typeof(ArgumentException)),
         KiiUTInfo(
            action = "When we call Put(key, value) with 'collapse_key' key",
            expected = "ArgumentException must be thrown."
            )]
        public void Test_0005_GCMData_InvalidKey_collapse_key()
        {
            GCMData data = new GCMData ();
            data.Put ("collapse_key", "abc");
        }
        [Test(),
         ExpectedException(typeof(ArgumentException)),
         KiiUTInfo(
            action = "When we call Put(key, value) with 'data' key",
            expected = "ArgumentException must be thrown."
            )]
        public void Test_0006_GCMData_InvalidKey_data()
        {
            GCMData data = new GCMData ();
            data.Put ("data", "abc");
        }
        [Test(),
         ExpectedException(typeof(ArgumentException)),
         KiiUTInfo(
            action = "When we call Put(key, value) with 'delay_while_idle' key",
            expected = "ArgumentException must be thrown."
            )]
        public void Test_0007_GCMData_InvalidKey_delay_while_idle()
        {
            GCMData data = new GCMData ();
            data.Put ("delay_while_idle", "abc");
        }
        [Test(),
         ExpectedException(typeof(ArgumentException)),
         KiiUTInfo(
            action = "When we call Put(key, value) with 'time_to_live' key",
            expected = "ArgumentException must be thrown."
            )]
        public void Test_0008_GCMData_InvalidKey_time_to_live()
        {
            GCMData data = new GCMData ();
            data.Put ("time_to_live", "abc");
        }
        [Test(),
         ExpectedException(typeof(ArgumentException)),
         KiiUTInfo(
            action = "When we call Put(key, value) with 'restricted_package_name' key",
            expected = "ArgumentException must be thrown."
            )]
        public void Test_0009_GCMData_InvalidKey_restricted_package_name()
        {
            GCMData data = new GCMData ();
            data.Put ("restricted_package_name", "abc");
        }
        [Test(),
         ExpectedException(typeof(ArgumentException)),
         KiiUTInfo(
            action = "When we call Put(key, value) with 'dry_run' key",
            expected = "ArgumentException must be thrown."
            )]
        public void Test_0010_GCMData_InvalidKey_dry_run()
        {
            GCMData data = new GCMData ();
            data.Put ("dry_run", "abc");
        }
        [Test(),
         ExpectedException(typeof(ArgumentException)),
         KiiUTInfo(
            action = "When we call Put(key, value) with 'google_xxx' key",
            expected = "ArgumentException must be thrown."
            )]
        public void Test_0011_GCMData_InvalidKey_start_with_google()
        {
            GCMData data = new GCMData ();
            data.Put ("google_xxx", "abc");
        }
        [Test(),
         KiiUTInfo(
            action = "When we call Put(key, value) with 'xxx_google' key and ToJsonObject()",
            expected = "We can get a JsonObject which contains put fields."
            )]
        public void Test_0012_GCMData_ValidKey_end_with_google()
        {
            GCMData data = new GCMData ();
            data.Put ("xxx_google", "abc");
            JsonObject json = data.ToJsonObject ();
            Assert.AreEqual (json.Get("xxx_google"), "abc");
        }
        #endregion

    }
}

