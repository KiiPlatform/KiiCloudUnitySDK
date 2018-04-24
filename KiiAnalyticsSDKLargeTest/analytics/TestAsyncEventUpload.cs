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
using System.Collections.Generic;
using JsonOrg;
using System.Threading;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;
namespace KiiCorp.Cloud.Analytics
{
    // Test spec : https://docs.google.com/a/kii.com/spreadsheet/ccc?key=0AlSyizQDsStPdE5mVXRLVWNURS1yUmo3SWpYMFVlOVE&usp=drive_web#gid=2
    [TestFixture()]
    public class TestAsyncEventUpload
    {
        [SetUp()]
        public void SetUp()
        {
            KiiAnalytics.Instance = null;
            BaseApp app = new AnalyticsApp();
            KiiAnalytics.Initialize (app.AppId, app.AppKey, app.BaseUrl, "dev004");
        }
        [Test()]
        public void Test_1_1_AsyncUpload ()
        {
            // create events
            KiiEvent ev1 = KiiAnalytics.NewEvent ("type1");
            ev1 ["page"] = 1;
            ev1 ["label"] = "OK";

            // async upload
            Exception exp = null;
            CountDownLatch cd = new CountDownLatch(1);
            KiiAnalytics.Upload((Exception e)=> {
                Interlocked.Exchange(ref exp, e);
                cd.Signal();
            }, new KiiEvent[]{ev1});
            
            if(!cd.Wait(new TimeSpan(0, 0, 0, 3)))
                Assert.Fail("Callback not fired.");
            Assert.Null(exp);
        }

        [Test()]
        public void Test_1_2_AsyncUpload_PartialSuccess ()
        {
            // create events
            KiiEvent ev1 = KiiAnalytics.NewEvent ("type1");
            ev1 ["page"] = 1;
            ev1 ["label"] = "OK";

            KiiEvent ev2 = KiiAnalytics.NewEvent ("type2");
            ev1 ["page"] = 1;
            ev1 ["labels"] = new string[]{};
 
            // async upload
            Exception exp = null;
            CountDownLatch cd = new CountDownLatch(1);
            KiiAnalytics.Upload((Exception e)=> {
                Interlocked.Exchange(ref exp, e);
                cd.Signal();
            }, new KiiEvent[]{ev1, ev2});

            if(!cd.Wait(new TimeSpan(0, 0, 0, 3)))
                Assert.Fail("Callback not fired.");
            Assert.NotNull(exp);
            Assert.That (exp, Is.InstanceOf<EventUploadException> ());
            EventUploadException ue = (EventUploadException)exp;
            Assert.AreEqual(200, ue.Status);

            // check response body
            JsonObject json = new JsonObject(ue.Body);
            Assert.AreEqual("PARTIAL_SUCCESS", json.GetString("errorCode"));
            JsonArray invalidEvents = json.GetJsonArray("invalidEvents");
            int length = invalidEvents.Length();
            Assert.AreEqual(1, length);
            JsonObject invalidEvent = invalidEvents.GetJsonObject(0);
            Assert.AreEqual(0, invalidEvent.GetInt("index"));
        }
    }
}

