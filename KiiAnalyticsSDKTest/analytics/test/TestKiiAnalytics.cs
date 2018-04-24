using System;
using System.Collections.Generic;
using NUnit.Framework;

using JsonOrg;
using KiiCorp.Cloud.Storage;

namespace KiiCorp.Cloud.Analytics
{
    [TestFixture()]
    public class TestKiiAnalytics
    {
        private const string CHAR_10 = "abcdefghij";
        private const string CHAR_50 = CHAR_10 + CHAR_10 + CHAR_10 + CHAR_10 + CHAR_10;
        private const string CHAR_100 = CHAR_50 + CHAR_50;
        
        private int ToCount(IEnumerator<string> it)
        {
            int count = 0;
            while (it.MoveNext())
            {
                ++count;
            }
            return count;
        }
        
        private KiiEvent[] CreateEvents()
        {
            KiiEvent ev1 = KiiAnalytics.NewEvent("type1");
            ev1["page"] = 1;
            ev1["label"] = "OK";

            KiiEvent ev2 = KiiAnalytics.NewEvent("type1");
            ev2["page"] = 2;
            ev2["label"] = "Cancel";

            KiiEvent ev3 = KiiAnalytics.NewEvent("type1");
            ev3["page"] = 3;
            ev3["label"] = "Next";
            
            return new KiiEvent[] { ev1, ev2, ev3};
        }

        [SetUp()]
        public void SetUp()
        {
            KiiAnalytics.Instance = null;
        }
        
        
        #region KiiAnalytics.Initialize(string, string, Site, string)
        [Test()]
        public void Test_0000_Initialize_US ()
        {
            KiiAnalytics.Initialize("appId", "appKey", KiiAnalytics.Site.US, "dev001");
            KiiEvent ev = KiiAnalytics.NewEvent("type");
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(204, "");
            
            KiiAnalytics.Upload(ev);
            
            Assert.AreEqual("https://api.kii.com/api/apps/appId/events", client.RequestUrl[0]);
        }
              
        [Test()]
        public void Test_0000_Initialize_CN ()
        {
            KiiAnalytics.Initialize("appId", "appKey", KiiAnalytics.Site.CN, "dev001");
            KiiEvent ev = KiiAnalytics.NewEvent("type");
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(204, "");
            
            KiiAnalytics.Upload(ev);
            
            Assert.AreEqual("https://api-cn2.kii.com/api/apps/appId/events", client.RequestUrl[0]);
        }
        
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0001_Initialize_appId_null ()
        {
            KiiAnalytics.Initialize(null, "appKey", KiiAnalytics.Site.US, "dev001");
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0002_Initialize_appKey_null ()
        {
            KiiAnalytics.Initialize("appID", null, KiiAnalytics.Site.US, "dev001");
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0003_Initialize_deviceId_null ()
        {
            KiiAnalytics.Initialize("appID", "appKey", KiiAnalytics.Site.US, null);
        }

        #endregion

        #region KiiAnalytics.Initialize(string, string, string, string)
        [Test()]
        public void Test_0100_Initialize ()
        {
            KiiAnalytics.Initialize("appID002", "appKey", "https://api-sg.kii.com/api", "dev002");
            KiiEvent ev = KiiAnalytics.NewEvent("type");
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(204, "");
            
            KiiAnalytics.Upload(ev);
            
            Assert.AreEqual("https://api-sg.kii.com/api/apps/appID002/events", client.RequestUrl[0]);
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0101_Initialize_appId_null ()
        {
            KiiAnalytics.Initialize(null, "appKey", "https://api-sg.kii.com/api", "dev002");
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0102_Initialize_appKey_null ()
        {
            KiiAnalytics.Initialize("appID002", null, "https://api-sg.kii.com/api", "dev002");
        }

        [Test()]
        public void Test_0103_Initialize_serverUrl_null ()
        {
            try
            {
                KiiAnalytics.Initialize("appID002", "appKey", null, "dev002");
                Assert.Fail("ArgumentException must be thrown");
            }
            catch (ArgumentException e)
            {
                Assert.IsNull(Kii.Instance);
            }
        }
        [Test()]
        public void Test_0103_Initialize_serverUrl_invalid ()
        {
            try
            {
                KiiAnalytics.Initialize("appID002", "appKey", "hogehoge", "dev002");
                Assert.Fail("ArgumentException must be thrown");
            }
            catch (ArgumentException e)
            {
                Assert.IsNull(Kii.Instance);
            }
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0104_Initialize_deviceId_null ()
        {
            KiiAnalytics.Initialize("appID", "appKey", "https://api-sg.kii.com/api", null);
        }

        #endregion    
        
        #region KiiAnalytics.NewEvent(string);
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0200_NewEvent_null ()
        {
            KiiAnalytics.Initialize("appID003", "appKey", KiiAnalytics.Site.JP, "dev003");
            KiiAnalytics.NewEvent(null);
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0201_NewEvent_space ()
        {
            KiiAnalytics.Initialize("appID003", "appKey", KiiAnalytics.Site.JP, "dev003");
            KiiAnalytics.NewEvent(" type");
        }

        [Test()]
        public void Test_0202_NewEvent_len128 ()
        {
            KiiAnalytics.Initialize("appID003", "appKey", KiiAnalytics.Site.JP, "dev003");
            KiiAnalytics.NewEvent(CHAR_100 + CHAR_10 + CHAR_10 + "abcdefgh");
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0203_NewEvent_len129 ()
        {
            KiiAnalytics.Initialize("appID003", "appKey", KiiAnalytics.Site.JP, "dev003");
            KiiAnalytics.NewEvent(CHAR_100 + CHAR_10 + CHAR_10 + "abcdefghi");
        }
        
        #endregion
        
        #region KiiAnalytics.Upload(KiiEvent...)
        
        [Test()]
        public void Test_0300_Upload ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent ev = KiiAnalytics.NewEvent("type1");
            ev["page"] = 1;
            ev["label"] = "OK";
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(204, "");

            KiiAnalytics.Upload(ev);

            Assert.AreEqual("https://api-jp.kii.com/api/apps/appID004/events", client.RequestUrl[0]);
            Assert.AreEqual("application/vnd.kii.EventRecordList+json", client.RequestContentType[0]);
            JsonArray array = new JsonArray(client.RequestBody[0]);
            JsonObject json = array.GetJsonObject(0);
            int count = ToCount(json.Keys());
            Assert.AreEqual(6, count);
            Assert.AreEqual(1, json.GetInt("page"));
            Assert.AreEqual("OK", json.GetString("label"));
            Assert.AreEqual("dev004", json.GetString("_deviceID"));
            Assert.AreEqual("type1", json.GetString("_type"));
            Assert.IsTrue(json.Has("_triggeredAt"));
            Assert.IsTrue(json.Has("_uploadedAt"));
        }

        [Test()]
        public void Test_0301_Upload_2 ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent ev1 = KiiAnalytics.NewEvent("type1");
            ev1["page"] = 1;
            ev1["label"] = "OK";
            
            KiiEvent ev2 = KiiAnalytics.NewEvent("type1");
            ev2["page"] = 2;
            ev2["label"] = "Cancel";

            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(204, "");

            KiiAnalytics.Upload(ev1, ev2);
            
            Assert.AreEqual("https://api-jp.kii.com/api/apps/appID004/events", client.RequestUrl[0]);
            Assert.AreEqual("application/vnd.kii.EventRecordList+json", client.RequestContentType[0]);
            JsonArray array = new JsonArray(client.RequestBody[0]);
            Assert.AreEqual(2, array.Length());
            JsonObject json = array.GetJsonObject(1);
            
            int count = ToCount(json.Keys());
            Assert.AreEqual(6, count);
            Assert.AreEqual(2, json.GetInt("page"));
            Assert.AreEqual("Cancel", json.GetString("label"));
            Assert.AreEqual("dev004", json.GetString("_deviceID"));
            Assert.AreEqual("type1", json.GetString("_type"));
            Assert.IsTrue(json.Has("_triggeredAt"));
            Assert.IsTrue(json.Has("_uploadedAt"));
        }

        [Test()]
        public void Test_0303_Upload_float_max ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent ev = KiiAnalytics.NewEvent("type1");
            ev["page"] = float.MaxValue;
            ev["label"] = "OK";
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(204, "");

            KiiAnalytics.Upload(ev);
            
            Assert.AreEqual("https://api-jp.kii.com/api/apps/appID004/events", client.RequestUrl[0]);
            Assert.AreEqual("application/vnd.kii.EventRecordList+json", client.RequestContentType[0]);
            JsonArray array = new JsonArray(client.RequestBody[0]);
            JsonObject json = array.GetJsonObject(0);
            int count = ToCount(json.Keys());
            Assert.AreEqual(6, count);
            Assert.AreEqual(float.MaxValue, json.GetDouble("page"));
            Assert.AreEqual("OK", json.GetString("label"));
            Assert.AreEqual("dev004", json.GetString("_deviceID"));
            Assert.AreEqual("type1", json.GetString("_type"));
            Assert.IsTrue(json.Has("_triggeredAt"));
            Assert.IsTrue(json.Has("_uploadedAt"));
        }

        [Test()]
        public void Test_0304_Upload_float_min ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent ev = KiiAnalytics.NewEvent("type1");
            ev["page"] = float.MinValue;
            ev["label"] = "OK";
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(204, "");

            KiiAnalytics.Upload(ev);
            
            Assert.AreEqual("https://api-jp.kii.com/api/apps/appID004/events", client.RequestUrl[0]);
            Assert.AreEqual("application/vnd.kii.EventRecordList+json", client.RequestContentType[0]);
            JsonArray array = new JsonArray(client.RequestBody[0]);
            JsonObject json = array.GetJsonObject(0);
            int count = ToCount(json.Keys());
            Assert.AreEqual(6, count);
            Assert.AreEqual(float.MinValue, json.GetDouble("page"));
            Assert.AreEqual("OK", json.GetString("label"));
            Assert.AreEqual("dev004", json.GetString("_deviceID"));
            Assert.AreEqual("type1", json.GetString("_type"));
            Assert.IsTrue(json.Has("_triggeredAt"));
            Assert.IsTrue(json.Has("_uploadedAt"));
        }        
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0310_Upload_empty ()
        {
            KiiAnalytics.Initialize("appID005", "appKey", KiiAnalytics.Site.JP, "dev005");
            KiiAnalytics.Upload();
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0311_Upload_null ()
        {
            KiiAnalytics.Initialize("appID005", "appKey", KiiAnalytics.Site.JP, "dev005");
			KiiEvent ev = null;
            KiiAnalytics.Upload(ev);
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0311_Upload_null_2 ()
        {
            KiiAnalytics.Initialize("appID005", "appKey", KiiAnalytics.Site.JP, "dev005");

            KiiEvent ev1 = KiiAnalytics.NewEvent("type1");
            ev1["page"] = 1;
            ev1["label"] = "OK";

            KiiAnalytics.Upload(ev1, null);
        }

        [Test()]
        public void Test_0313_Upload_50 ()
        {
            KiiAnalytics.Initialize("appID005", "appKey", KiiAnalytics.Site.JP, "dev005");
   
            KiiEvent[] eventList = new KiiEvent[50];
            for (int i = 0 ; i < eventList.Length ; ++i)
            {
                KiiEvent ev = KiiAnalytics.NewEvent("type1");
                ev["page"] = i;
                eventList[i] = ev;
            }
   
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(204, "");
            
            KiiAnalytics.Upload(eventList);
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0314_Upload_51 ()
        {
            KiiAnalytics.Initialize("appID005", "appKey", KiiAnalytics.Site.JP, "dev005");
   
            KiiEvent[] eventList = new KiiEvent[51];
            for (int i = 0 ; i < eventList.Length ; ++i)
            {
                KiiEvent ev = KiiAnalytics.NewEvent("type1");
                ev["page"] = i;
                eventList[i] = ev;
            }

            KiiAnalytics.Upload(eventList);
        }
        
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0320_Upload_sent ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent ev = KiiAnalytics.NewEvent("type1");
            ev["page"] = 1;
            ev["label"] = "OK";
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(204, "");

            KiiAnalytics.Upload(ev);
            // set sent event
            KiiAnalytics.Upload(ev);
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0321_Upload_sent_2_1 ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent ev1 = KiiAnalytics.NewEvent("type1");
            ev1["page"] = 1;
            ev1["label"] = "OK";
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(204, "");

            KiiAnalytics.Upload(ev1);

            KiiEvent ev2 = KiiAnalytics.NewEvent("type1");
            ev2["page"] = 1;
            ev2["label"] = "OK";

            // set sent event
            KiiAnalytics.Upload(ev1, ev2);
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0322_Upload_sent_2_2 ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent ev1 = KiiAnalytics.NewEvent("type1");
            ev1["page"] = 1;
            ev1["label"] = "OK";
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(204, "");

            KiiAnalytics.Upload(ev1);

            KiiEvent ev2 = KiiAnalytics.NewEvent("type1");
            ev2["page"] = 1;
            ev2["label"] = "OK";

            // set sent event
            KiiAnalytics.Upload(ev2, ev1);
        }
        
        [Test()]
        public void Test_0330_Upload_Partial_first ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent[] events = CreateEvents();
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(200, 
                "{" +
                "\"errorCode\" : \"PARTIAL_SUCCESS\"," +
                "\"message\" : \"There are some errors in event data\"," +
                "\"invalidEvents\" : [ {" +
                "\"index\" : 0," +
                "\"errorDetails\" : [ {" +
                "\"fieldName\" : \"_uploadedAt\"," +
                "\"errorCode\" : \"UPLOADED_AT_MISSING\"," +
                "\"message\" : \"You must provide event upload time\"}]" +
                "}]" +
                "}");
   
            try 
            {
                KiiAnalytics.Upload(events);
                Assert.Fail("Exception must be thrown");
            }
            catch (EventUploadException e)
            {
                IList<KiiEvent> errorList = e.ErrorEvents;
                Assert.IsFalse(events[0].Sent);
                Assert.IsTrue(events[1].Sent);
                Assert.IsTrue(events[2].Sent);                
                Assert.AreEqual(1, errorList.Count);
                KiiEvent error1 = errorList[0];
                Assert.AreEqual(1, error1["page"]);
                Assert.AreEqual("OK", error1["label"]);
            }
        }        

        [Test()]
        public void Test_0331_Upload_Partial_second ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent[] events = CreateEvents();
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(200, 
                "{" +
                "\"errorCode\" : \"PARTIAL_SUCCESS\"," +
                "\"message\" : \"There are some errors in event data\"," +
                "\"invalidEvents\" : [ {" +
                "\"index\" : 1," +
                "\"errorDetails\" : [ {" +
                "\"fieldName\" : \"_uploadedAt\"," +
                "\"errorCode\" : \"UPLOADED_AT_MISSING\"," +
                "\"message\" : \"You must provide event upload time\"}]" +
                "}]" +
                "}");
   
            try 
            {
                KiiAnalytics.Upload(events);
                Assert.Fail("Exception must be thrown");
            }
            catch (EventUploadException e)
            {
                IList<KiiEvent> errorList = e.ErrorEvents;
                Assert.IsTrue(events[0].Sent);
                Assert.IsFalse(events[1].Sent);
                Assert.IsTrue(events[2].Sent);                
                Assert.AreEqual(1, errorList.Count);
                KiiEvent error1 = errorList[0];
                Assert.AreEqual(2, error1["page"]);
                Assert.AreEqual("Cancel", error1["label"]);
            }
        }               

        [Test()]
        public void Test_0332_Upload_Partial_last ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent[] events = CreateEvents();
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(200, 
                "{" +
                "\"errorCode\" : \"PARTIAL_SUCCESS\"," +
                "\"message\" : \"There are some errors in event data\"," +
                "\"invalidEvents\" : [ {" +
                "\"index\" : 2," +
                "\"errorDetails\" : [ {" +
                "\"fieldName\" : \"_uploadedAt\"," +
                "\"errorCode\" : \"UPLOADED_AT_MISSING\"," +
                "\"message\" : \"You must provide event upload time\"}]" +
                "}]" +
                "}");
   
            try 
            {
                KiiAnalytics.Upload(events);
                Assert.Fail("Exception must be thrown");
            }
            catch (EventUploadException e)
            {
                IList<KiiEvent> errorList = e.ErrorEvents;
                Assert.IsTrue(events[0].Sent);
                Assert.IsTrue(events[1].Sent);
                Assert.IsFalse(events[2].Sent);                
                Assert.AreEqual(1, errorList.Count);
                KiiEvent error1 = errorList[0];
                Assert.AreEqual(3, error1["page"]);
                Assert.AreEqual("Next", error1["label"]);
            }
        }                      

        [Test()]
        public void Test_0333_Upload_Partial_all ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent[] events = CreateEvents();
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(200, 
                "{" +
                "\"errorCode\" : \"PARTIAL_SUCCESS\"," +
                "\"message\" : \"There are some errors in event data\"," +
                "\"invalidEvents\" : [" +
                "{" +
                "\"index\" : 0," +
                "\"errorDetails\" : [ {" +
                "\"fieldName\" : \"_uploadedAt\"," +
                "\"errorCode\" : \"UPLOADED_AT_MISSING\"," +
                "\"message\" : \"You must provide event upload time\"}]" +
                "}," +
                "{" +
                "\"index\" : 1," +
                "\"errorDetails\" : [ {" +
                "\"fieldName\" : \"_uploadedAt\"," +
                "\"errorCode\" : \"UPLOADED_AT_MISSING\"," +
                "\"message\" : \"You must provide event upload time\"}]" +
                "}," +
                "{" +
                "\"index\" : 2," +
                "\"errorDetails\" : [ {" +
                "\"fieldName\" : \"_uploadedAt\"," +
                "\"errorCode\" : \"UPLOADED_AT_MISSING\"," +
                "\"message\" : \"You must provide event upload time\"}]" +
                "}" +
                "]" +
                "}");
   
            try 
            {
                KiiAnalytics.Upload(events);
                Assert.Fail("Exception must be thrown");
            }
            catch (EventUploadException e)
            {
                IList<KiiEvent> errorList = e.ErrorEvents;
                Assert.IsFalse(events[0].Sent);
                Assert.IsFalse(events[1].Sent);
                Assert.IsFalse(events[2].Sent);                
                Assert.AreEqual(3, errorList.Count);
                
                KiiEvent error1 = errorList[0];
                Assert.AreEqual(1, error1["page"]);
                Assert.AreEqual("OK", error1["label"]);
                
                KiiEvent error2 = errorList[1];
                Assert.AreEqual(2, error2["page"]);
                Assert.AreEqual("Cancel", error2["label"]);

                KiiEvent error3 = errorList[2];
                Assert.AreEqual(3, error3["page"]);
                Assert.AreEqual("Next", error3["label"]);
                
            }
        }   
        
        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException))]
        public void Test_0334_Upload_Partial_broken_json_1 ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent[] events = CreateEvents();
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(200, "broken");

            KiiAnalytics.Upload(events);
        }
        
        [Test()]
        public void Test_0335_Upload_Partial_broken_json_2 ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent[] events = CreateEvents();
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(200, 
                "{" +
                "\"errorCode\" : \"PARTIAL_SUCCESS\"," +
                "\"message\" : \"There are some errors in event data\"," +
                "\"invalidEvents\" : []}");
   
            try
            {
                KiiAnalytics.Upload(events);
                Assert.Fail("Exception must be thrown.");
            }
            catch (EventUploadException e)
            {
                Assert.AreEqual(0, e.ErrorEvents.Count);
            }
        }
        
        [Test()]
        public void Test_0336_Upload_Partial_broken_json_3 ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent[] events = CreateEvents();
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(200, 
                "{" +
                "\"errorCode\" : \"PARTIAL_SUCCESS\"," +
                "\"message\" : \"There are some errors in event data\"," +
                "\"invalidEvents\" : [ \"not Json\"]}");
            
            try
            {
                KiiAnalytics.Upload(events);
                Assert.Fail("Exception must be thrown");                
            }
            catch (EventUploadException e)
            {
                Assert.AreEqual(0, e.ErrorEvents.Count);
            }
                
        }
        
        [Test()]
        public void Test_0337_Upload_Partial_broken_json_4 ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent[] events = CreateEvents();
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(200, 
                "{" +
                "\"errorCode\" : \"PARTIAL_SUCCESS\"," +
                "\"message\" : \"There are some errors in event data\"," +
                "\"invalidEvents\" : [ {" +
                "\"index\" : \"not number\"," +
                "\"errorDetails\" : [ {" +
                "\"fieldName\" : \"_uploadedAt\"," +
                "\"errorCode\" : \"UPLOADED_AT_MISSING\"," +
                "\"message\" : \"You must provide event upload time\"}]" +
                "}]" +
                "}");
   
            try
            {
                KiiAnalytics.Upload(events);
                Assert.Fail("Exception must be thrown");                
            }
            catch (EventUploadException e)
            {
                Assert.AreEqual(0, e.ErrorEvents.Count);
            }
        }        

        [Test()]
        public void Test_0340_Upload_CloudException ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent[] events = CreateEvents();
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(new CloudException(400, "{}"));
   
            try
            {
                KiiAnalytics.Upload(events);
                Assert.Fail("Exception must be thrown");
            }
            catch (EventUploadException e)
            {
                Assert.AreEqual(3, e.ErrorEvents.Count);
            }
        }         
        
        [Test()]
        public void Test_0341_Upload_CloudException_broken_json ()
        {
            KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
            KiiEvent[] events = CreateEvents();
            
            // set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
            client.AddResponse(new CloudException(400, "broken"));
   
            try
            {
                KiiAnalytics.Upload(events);
                Assert.Fail("Exception must be thrown");
            }
            catch (EventUploadException e)
            {
                Assert.AreEqual(3, e.ErrorEvents.Count);
            }
        }                 
		[Test()]
		public void Test_0350_Upload_NullEvent ()
		{
			KiiAnalytics.Initialize("appID004", "appKey", KiiAnalytics.Site.JP, "dev004");
			KiiEvent ev = new KiiEvent.NullKiiEvent ();

			// set mock
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            MockHttpClient client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
			client.AddResponse(204, "");
			
			KiiAnalytics.Upload(ev);
			
			Assert.AreEqual("https://api-jp.kii.com/api/apps/appID004/events", client.RequestUrl[0]);
			Assert.AreEqual("application/vnd.kii.EventRecordList+json", client.RequestContentType[0]);
			JsonArray array = new JsonArray(client.RequestBody[0]);
			Assert.AreEqual(0, array.Length());
		}
		#endregion
    }
}

