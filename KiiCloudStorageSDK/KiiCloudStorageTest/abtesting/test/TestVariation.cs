using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Analytics;

namespace KiiCorp.Cloud.ABTesting
{
    [TestFixture()]
    public class TestVariation
    {
        private MockHttpClient client;
        
        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
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
        private String CreateKiiExperimentAsJsonString(int experimentVersion, KiiExperimentStatus status, string variationNameA, string eventNameA, string variationNameB, string eventNameB)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("\"_id\"" + ":" + "\"ID-001\"" + ",");
            sb.AppendLine("\"description\"" + ":" + "\"Experiment for UT\"" + ",");
            sb.AppendLine("\"version\"" + ":" + experimentVersion + ",");
            sb.AppendLine("\"status\"" + ":" + ((int)status) + ",");
            
            sb.AppendLine("\"conversionEvents\"" + ":");
            sb.Append("[");
            sb.Append("{\"name\":\"" + eventNameA + "\"}");
            sb.Append(",");
            sb.Append("{\"name\":\"" + eventNameB + "\"}");
            sb.Append("],");
            
            sb.AppendLine("\"variations\"" + ":");
            sb.Append("[");
            sb.Append("{\"name\":\"" + variationNameA + "\", \"percentage\":50, \"variableSet\":{}}");
            sb.Append(",");
            sb.Append("{\"name\":\"" + variationNameB + "\", \"percentage\":50, \"variableSet\":{}}");
            sb.Append("]");
            sb.AppendLine("}");
            return sb.ToString ();
        }

        [Test(), KiiUTInfo(
            action = "When we call EventForConversion(ConversionEvent) and method returns KiiEvent",
            expected = "We can get the KiiEvent"
            )]
        public void Test_0000_EventForConversion()
        {
            this.LogIn();
            int experimentVersion = 10;
            client.AddResponse(200, CreateKiiExperimentAsJsonString(experimentVersion, KiiExperimentStatus.RUNNING, "UI-A", "EV-A", "UI-B", "EV-B"));
            KiiExperiment experiment = KiiExperiment.GetByID("000001");
            Variation variationA = experiment.Variations[0];
            Variation variationB = experiment.Variations[1];
            ConversionEvent eventA = experiment.ConversionEvents[0];
            ConversionEvent eventB = experiment.ConversionEvents[1];

            KiiEvent kiiEventA = variationA.EventForConversion(eventA);
            Assert.AreEqual(variationA.Name, kiiEventA["variationName"]);
            Assert.AreEqual(eventA.Name, kiiEventA["conversionEvent"]);
            Assert.AreEqual(experimentVersion, kiiEventA["version"]);

            KiiEvent kiiEventB = variationB.EventForConversion(eventB);
            Assert.AreEqual(variationB.Name, kiiEventB["variationName"]);
            Assert.AreEqual(eventB.Name, kiiEventB["conversionEvent"]);
            Assert.AreEqual(experimentVersion, kiiEventB["version"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call EventForConversion(int) and method returns KiiEvent",
            expected = "We can get the KiiEvent"
            )]
        public void Test_0001_EventForConversion()
        {
            this.LogIn();
            int experimentVersion = 10;
            client.AddResponse(200, CreateKiiExperimentAsJsonString(experimentVersion, KiiExperimentStatus.RUNNING, "UI-A", "EV-A", "UI-B", "EV-B"));
            KiiExperiment experiment = KiiExperiment.GetByID("000001");
            Variation variationA = experiment.Variations[0];
            Variation variationB = experiment.Variations[1];
            ConversionEvent eventA = experiment.ConversionEvents[0];
            ConversionEvent eventB = experiment.ConversionEvents[1];
            
            KiiEvent kiiEventA = variationA.EventForConversion(0);
            Assert.AreEqual(variationA.Name, kiiEventA["variationName"]);
            Assert.AreEqual(eventA.Name, kiiEventA["conversionEvent"]);
            Assert.AreEqual(experimentVersion, kiiEventA["version"]);
            
            KiiEvent kiiEventB = variationB.EventForConversion(1);
            Assert.AreEqual(variationB.Name, kiiEventB["variationName"]);
            Assert.AreEqual(eventB.Name, kiiEventB["conversionEvent"]);
            Assert.AreEqual(experimentVersion, kiiEventB["version"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call EventForConversion(ConversionEvent) and method returns KiiEvent.NullKiiEvent when experiment status is draft",
            expected = "We can get the NullKiiEvent"
            )]
        public void Test_0003_EventForConversionWhenStatusIsDraft()
        {
            this.LogIn();
            int experimentVersion = 10;
            client.AddResponse(200, CreateKiiExperimentAsJsonString(experimentVersion, KiiExperimentStatus.DRAFT, "UI-A", "EV-A", "UI-B", "EV-B"));
            KiiExperiment experiment = KiiExperiment.GetByID("000001");
            Variation variationA = experiment.Variations[0];
            Variation variationB = experiment.Variations[1];
            ConversionEvent eventA = experiment.ConversionEvents[0];
            ConversionEvent eventB = experiment.ConversionEvents[1];
            
            KiiEvent kiiEventA = variationA.EventForConversion(eventA);
            Assert.IsInstanceOfType (typeof(KiiEvent.NullKiiEvent), kiiEventA);

            KiiEvent kiiEventB = variationB.EventForConversion(eventB);
            Assert.IsInstanceOfType (typeof(KiiEvent.NullKiiEvent), kiiEventB);
        }
        [Test(), KiiUTInfo(
            action = "When we call EventForConversion(ConversionEvent) and method returns KiiEvent.NullKiiEvent when experiment status is paused",
            expected = "We can get the NullKiiEvent"
            )]
        public void Test_0004_EventForConversionWhenStatusIsPaused()
        {
            this.LogIn();
            int experimentVersion = 10;
            client.AddResponse(200, CreateKiiExperimentAsJsonString(experimentVersion, KiiExperimentStatus.PAUSED, "UI-A", "EV-A", "UI-B", "EV-B"));
            KiiExperiment experiment = KiiExperiment.GetByID("000001");
            Variation variationA = experiment.Variations[0];
            Variation variationB = experiment.Variations[1];
            ConversionEvent eventA = experiment.ConversionEvents[0];
            ConversionEvent eventB = experiment.ConversionEvents[1];
            
            KiiEvent kiiEventA = variationA.EventForConversion(eventA);
            Assert.IsInstanceOfType (typeof(KiiEvent.NullKiiEvent), kiiEventA);
            
            KiiEvent kiiEventB = variationB.EventForConversion(eventB);
            Assert.IsInstanceOfType (typeof(KiiEvent.NullKiiEvent), kiiEventB);
        }
        [Test(), KiiUTInfo(
            action = "When we call EventForConversion(ConversionEvent) and method returns KiiEvent.NullKiiEvent when experiment status is terminated",
            expected = "We can get the NullKiiEvent"
            )]
        public void Test_0005_EventForConversionWhenStatusIsTerminated()
        {
            this.LogIn();
            int experimentVersion = 10;
            client.AddResponse(200, CreateKiiExperimentAsJsonString(experimentVersion, KiiExperimentStatus.TERMINATED, "UI-A", "EV-A", "UI-B", "EV-B"));
            KiiExperiment experiment = KiiExperiment.GetByID("000001");
            Variation variationA = experiment.Variations[0];
            Variation variationB = experiment.Variations[1];
            ConversionEvent eventA = experiment.ConversionEvents[0];
            ConversionEvent eventB = experiment.ConversionEvents[1];
            
            KiiEvent kiiEventA = variationA.EventForConversion(eventA);
            Assert.IsInstanceOfType (typeof(KiiEvent.NullKiiEvent), kiiEventA);
            
            KiiEvent kiiEventB = variationB.EventForConversion(eventB);
            Assert.IsInstanceOfType (typeof(KiiEvent.NullKiiEvent), kiiEventB);
        }

    }
}

