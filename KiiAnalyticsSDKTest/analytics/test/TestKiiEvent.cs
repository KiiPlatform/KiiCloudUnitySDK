using System;
using NUnit.Framework;

using JsonOrg;
using KiiCorp.Cloud.Storage;

namespace KiiCorp.Cloud.Analytics
{
    [TestFixture()]
    public class TestKiiEvent
    {
        private const string KEY_TRIGGERED_AT = "_triggeredAt";
        private const string KEY_UPLOADED_AT = "_uploadedAt";
        [SetUp()]
        public void SetUp() 
        {
            KiiAnalytics.Instance = null;
            KiiAnalytics.Initialize("appId", "appKey", KiiAnalytics.Site.US, "dev001");            
        }
        
        private void AssertAndRemoveTimes(JsonObject json)
        {
            Assert.IsTrue(json.Has(KEY_TRIGGERED_AT));
            Assert.IsTrue(json.Has(KEY_UPLOADED_AT));
            
            json.Remove(KEY_TRIGGERED_AT);
            json.Remove(KEY_UPLOADED_AT);
        }
        
        #region Setter
        [Test()]
        public void Test_0000_intValue ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev["page"] = 1;
            
            JsonObject json = ev.ConvertToJsonObject("dev001");
            AssertAndRemoveTimes(json);
            
            Assert.AreEqual("{\"page\":1,\"_deviceID\":\"dev001\",\"_type\":\"testType\"}", json.ToString());
        }

        [Test()]
        public void Test_0001_longValue ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev["time"] = (long)12345678901234;
            
            JsonObject json = ev.ConvertToJsonObject("dev001");            
            AssertAndRemoveTimes(json);
            
            Assert.AreEqual("{\"time\":12345678901234,\"_deviceID\":\"dev001\",\"_type\":\"testType\"}", json.ToString());
        }    

        [Test()]
        public void Test_0002_floatValue ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev["score"] = (float)1.34;
            
            JsonObject json = ev.ConvertToJsonObject("dev001");            
            AssertAndRemoveTimes(json);
            
            Assert.AreEqual("{\"score\":1.34,\"_deviceID\":\"dev001\",\"_type\":\"testType\"}", json.ToString());
        }        

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0003_doubleValue ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev["score"] = (double)1.3456;
        }

        [Test()]
        public void Test_0004_stringValue ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev["label"] = "KiiButton";
            
            JsonObject json = ev.ConvertToJsonObject("dev001");            
            AssertAndRemoveTimes(json);
            
            Assert.AreEqual("{\"label\":\"KiiButton\",\"_deviceID\":\"dev001\",\"_type\":\"testType\"}", json.ToString());
        }

        [Test()]
        public void Test_0005_intArrayValue ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev["selected"] = new int[] {2, 5, 8};
            
            JsonObject json = ev.ConvertToJsonObject("dev001");            
            AssertAndRemoveTimes(json);
            
            Assert.AreEqual("{\"selected\":[2,5,8],\"_deviceID\":\"dev001\",\"_type\":\"testType\"}", json.ToString());
        }
  
        [Test()]
        public void Test_0006_longArrayValue ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev["selected"] = new long[] {1234567890123, 987654321098765};
            
            JsonObject json = ev.ConvertToJsonObject("dev001");            
            AssertAndRemoveTimes(json);
            
            Assert.AreEqual("{\"selected\":[1234567890123,987654321098765],\"_deviceID\":\"dev001\",\"_type\":\"testType\"}", json.ToString());
        }
        
        [Test()]
        public void Test_0007_floatArrayValue ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev["selected"] = new float[] {(float)1.23, (float)3.57};
            
            JsonObject json = ev.ConvertToJsonObject("dev001");            
            AssertAndRemoveTimes(json);
            
            Assert.AreEqual("{\"selected\":[1.23,3.57],\"_deviceID\":\"dev001\",\"_type\":\"testType\"}", json.ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0008_doubleArrayValue ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev["selected"] = new double[] {(double)1.23456789, (double)9.8765432};
        }

        [Test()]
        public void Test_0009_stringArrayValue ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev["selected"] = new string[] {"item1", "item3", "item6"};
            
            JsonObject json = ev.ConvertToJsonObject("dev001");
            AssertAndRemoveTimes(json);
            
            Assert.AreEqual("{\"selected\":[\"item1\",\"item3\",\"item6\"],\"_deviceID\":\"dev001\",\"_type\":\"testType\"}", json.ToString());
        }  
        #endregion
        
        #region Key
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0020_key_null ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev[null] = 1;
        }

        [Test()]
        public void Test_0021_key_len1 ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev["a"] = 1;

            JsonObject json = ev.ConvertToJsonObject("dev001");
            AssertAndRemoveTimes(json);
            
            Assert.AreEqual("{\"a\":1,\"_deviceID\":\"dev001\",\"_type\":\"testType\"}", json.ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0022_key_underscore ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev["_"] = 1;
        }
        
        #endregion
        
        #region Value
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void Test_0030_value_null ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev["key"] = null;
        }
        
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0031_value_empty_string ()
        {
            KiiEvent ev = KiiAnalytics.NewEvent("testType");
            ev["key"] = "";
        }        
        #endregion
        
        
    }
}

