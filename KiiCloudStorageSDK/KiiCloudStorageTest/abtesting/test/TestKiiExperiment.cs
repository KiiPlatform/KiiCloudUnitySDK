using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;
using JsonOrg;

namespace KiiCorp.Cloud.ABTesting
{
    [TestFixture()]
    public class TestKiiExperiment
    {
        private MockHttpClient client;

        #region Test Data
        private static string id = "ID-0001";
        private static string description = "Blocking GetByID Test";
        private static int version = 5;
        private static KiiExperimentStatus status = KiiExperimentStatus.RUNNING;
        
        private static string variationNameA = "UI-Pattern-A";
        private static int percentageA = 30;
        private static string variableSetA = "{\"message\":\"Hello Kii\"}";
        private static string eventNameA = "LoginA";
        
        private static string variationNameB = "UI-Pattern-B";
        private static int percentageB = 70;
        private static string variableSetB = "{\"message\":\"Hello KIIIIII\"}";
        private static string eventNameB = "LoginB";
        #endregion

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
        private String CreateKiiExperimentAsJsonString(string id, string description, int version, KiiExperimentStatus status,
                                                       string variationNameA, string eventNameA, int percentageA, string variableSetA,
                                                       string variationNameB, string eventNameB, int percentageB, string variableSetB,
                                                       string chosenVariationName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("\"_id\"" + ":" + "\"" + id + "\"" + ",");
            sb.AppendLine("\"description\"" + ":" + "\"" + description + "\"" + ",");
            sb.AppendLine("\"version\"" + ":" + version + ",");
            sb.AppendLine("\"status\"" + ":" + ((int)status) + ",");
            
            sb.AppendLine("\"conversionEvents\"" + ":");
            sb.Append("[");
            sb.Append("{\"name\":\"" + eventNameA + "\"}");
            sb.Append(",");
            sb.Append("{\"name\":\"" + eventNameB + "\"}");
            sb.Append("],");

            sb.AppendLine("\"variations\"" + ":");
            sb.Append("[");
            sb.Append("{\"name\":\"" + variationNameA + "\", \"percentage\":" + percentageA + ", \"variableSet\":" + variableSetA + "}");
            sb.Append(",");
            sb.Append("{\"name\":\"" + variationNameB + "\", \"percentage\":" + percentageB + ", \"variableSet\":" + variableSetB + "}");
            sb.Append("]");
            if (!Utils.IsEmpty (chosenVariationName))
            {
                sb.Append(",");
                sb.AppendLine("\"chosenVariation\"" + ":" + "\"" + chosenVariationName + "\"");
            }
            sb.AppendLine("}");
            return sb.ToString ();
        }

        #region GetByID
        [Test(), KiiUTInfo(
            action = "When we call GetByID(string) and method returns KiiExperiment",
            expected = "We can get the KiiExperiment"
            )]
        public void Test_0001_GetByID()
        {
            this.LogIn();
            client.AddResponse(
                200,
                CreateKiiExperimentAsJsonString(id, description, version, status,
                                            variationNameA, eventNameA, percentageA, variableSetA,
                                            variationNameB, eventNameB, percentageB, variableSetB,
                                            variationNameA)
                );
            KiiExperiment experiment = KiiExperiment.GetByID(id);
            
            // verify Experiment
            Assert.AreEqual(id, experiment.ID);
            Assert.AreEqual(description, experiment.Description);
            Assert.AreEqual(version, experiment.Version);
            Assert.AreEqual(status, experiment.Status);
            Assert.AreEqual(experiment.Variations[0], experiment.ChosenVariation);
            // verify VariationA
            Assert.AreEqual(variationNameA, experiment.Variations[0].Name);
            Assert.AreEqual(percentageA, experiment.Variations[0].Percentage);
            Assert.AreEqual(new JsonObject(variableSetA).ToString(), experiment.Variations[0].VariableSet.ToString());
            Assert.AreEqual(eventNameA, experiment.ConversionEvents[0].Name);
            // verify VariationB
            Assert.AreEqual(variationNameB, experiment.Variations[1].Name);
            Assert.AreEqual(percentageB, experiment.Variations[1].Percentage);
            Assert.AreEqual(new JsonObject(variableSetB).ToString(), experiment.Variations[1].VariableSet.ToString());
            Assert.AreEqual(eventNameB, experiment.ConversionEvents[1].Name);
        }
        [Test(), KiiUTInfo(
            action = "When we call GetByID(string) and method throws ArgumentException",
            expected = "method throws ArgumentException if specified id is invalid"
            )]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_0002_GetByID_With_InvalidID()
        {
            this.LogIn();
            client.AddResponse(
                200,
                CreateKiiExperimentAsJsonString(id, description, version, status,
                                            variationNameA, eventNameA, percentageA, variableSetA,
                                            variationNameB, eventNameB, percentageB, variableSetB,
                                            variationNameA)
                );
            KiiExperiment.GetByID("+100*");
        }
        [Test(), KiiUTInfo(
            action = "When we call GetByID(string, KiiExperimentCallback) and method returns KiiExperiment",
            expected = "We can get the KiiExperiment by async"
            )]
        public void Test_0003_GetByID_ASync()
        {
            this.LogIn();
            client.AddResponse(
                200,
                CreateKiiExperimentAsJsonString(id, description, version, status,
                                            variationNameA, eventNameA, percentageA, variableSetA,
                                            variationNameB, eventNameB, percentageB, variableSetB,
                                            variationNameA)
                );
            KiiExperiment.GetByID(id, (KiiExperiment experiment, Exception e) => {
                // verify Experiment
                Assert.AreEqual(id, experiment.ID);
                Assert.AreEqual(description, experiment.Description);
                Assert.AreEqual(version, experiment.Version);
                Assert.AreEqual(status, experiment.Status);
                Assert.AreEqual(experiment.Variations[0], experiment.ChosenVariation);
                // verify VariationA
                Assert.AreEqual(variationNameA, experiment.Variations[0].Name);
                Assert.AreEqual(percentageA, experiment.Variations[0].Percentage);
                Assert.AreEqual(new JsonObject(variableSetA).ToString(), experiment.Variations[0].VariableSet.ToString());
                Assert.AreEqual(eventNameA, experiment.ConversionEvents[0].Name);
                // verify VariationB
                Assert.AreEqual(variationNameB, experiment.Variations[1].Name);
                Assert.AreEqual(percentageB, experiment.Variations[1].Percentage);
                Assert.AreEqual(new JsonObject(variableSetB).ToString(), experiment.Variations[1].VariableSet.ToString());
                Assert.AreEqual(eventNameB, experiment.ConversionEvents[1].Name);
            });
        }
        [Test(), KiiUTInfo(
            action = "When we call GetByID(string, KiiExperimentCallback) and method returns KiiExperiment",
            expected = "We can get the Exception by async if specified id is invalid"
            )]
        public void Test_0004_GetByID_ASync_With_InvalidID()
        {
            this.LogIn();
            client.AddResponse(
                200,
                CreateKiiExperimentAsJsonString(id, description, version, status,
                                            variationNameA, eventNameA, percentageA, variableSetA,
                                            variationNameB, eventNameB, percentageB, variableSetB,
                                            variationNameA)
                );
            KiiExperiment.GetByID("+100*", (KiiExperiment experiment, Exception e) => {
                if (e == null)
                {
                    Assert.Fail("ArgumentException was not thrown.");
                }
                if (e.GetType() != typeof(ArgumentException))
                {
                    Assert.Fail("unexpected exception. " + e.GetType());
                }
            });
        }
        #endregion

        #region GetVariationByName
        [Test(), KiiUTInfo(
            action = "When we call GetVariationByName(string) and method returns Variation",
            expected = "We can get the Variation by name"
            )]
        public void Test_0005_GetVariationByName()
        {
            this.LogIn();
            client.AddResponse(
                200,
                CreateKiiExperimentAsJsonString(id, description, version, status,
                                            variationNameA, eventNameA, percentageA, variableSetA,
                                            variationNameB, eventNameB, percentageB, variableSetB,
                                            variationNameA)
                );
            KiiExperiment experiment = KiiExperiment.GetByID(id);
            Variation variation = experiment.GetVariationByName(variationNameB);
            Assert.AreEqual(experiment.Variations[1], variation);

            variation = experiment.GetVariationByName("hoge");
            Assert.IsNull(variation);
        }
        #endregion

        #region GetConversionEventByName
        [Test(), KiiUTInfo(
            action = "When we call GetConversionEventByName(string) and method returns ConversionEvent",
            expected = "We can get the ConversionEvent by name"
            )]
        public void Test_0006_GetConversionEventByName()
        {
            this.LogIn();
            client.AddResponse(
                200,
                CreateKiiExperimentAsJsonString(id, description, version, status,
                                            variationNameA, eventNameA, percentageA, variableSetA,
                                            variationNameB, eventNameB, percentageB, variableSetB,
                                            variationNameA)
                );
            KiiExperiment experiment = KiiExperiment.GetByID(id);
            ConversionEvent conversionEvent = experiment.GetConversionEventByName(eventNameB);
            Assert.AreEqual(experiment.ConversionEvents [1], conversionEvent);

            conversionEvent = experiment.GetConversionEventByName("hoge");
            Assert.IsNull(conversionEvent);
        }
        #endregion

        #region GetAppliedVariation
        [Test(), KiiUTInfo(
            action = "When we call GetAppliedVariation(Variation) and method returns Variation",
            expected = "We can get the Variation"
            )]
        public void Test_0007_GetAppliedVariation()
        {
            this.LogIn();
            client.AddResponse(
                200,
                CreateKiiExperimentAsJsonString(id, description, version, KiiExperimentStatus.RUNNING,
                                            variationNameA, eventNameA, 0, variableSetA,
                                            variationNameB, eventNameB, 100, variableSetB,
                                            variationNameA)
                );
            KiiExperiment experiment = KiiExperiment.GetByID(id);
            Variation variation = experiment.GetAppliedVariation(null);
            Assert.AreEqual(experiment.Variations[1], variation);
        }
        [Test(), KiiUTInfo(
            action = "When we call GetAppliedVariation(Variation) and method returns Variation",
            expected = "We can get the null"
            )]
        public void Test_0008_GetAppliedVariation_Fallback()
        {
            this.LogIn();
            client.AddResponse(
                200,
                CreateKiiExperimentAsJsonString(id, description, version, KiiExperimentStatus.DRAFT,
                                            variationNameA, eventNameA, 0, variableSetA,
                                            variationNameB, eventNameB, 100, variableSetB,
                                            variationNameA)
                );
            KiiExperiment experiment = KiiExperiment.GetByID(id);
            Variation variation = experiment.GetAppliedVariation(null);
            Assert.IsNull(variation);
        }
        [Test(), KiiUTInfo(
            action = "When we call GetAppliedVariation(Variation, VariationSampler) and method returns Variation",
            expected = "We can get the VariationA regardless of percentage."
            )]
        public void Test_0009_GetAppliedVariation_With_CustomSampler()
        {
            this.LogIn();
            client.AddResponse(
                200,
                CreateKiiExperimentAsJsonString(id, description, version, KiiExperimentStatus.RUNNING,
                                            variationNameA, eventNameA, 0, variableSetA,
                                            variationNameB, eventNameB, 100, variableSetB,
                                            variationNameA)
                );
            KiiExperiment experiment = KiiExperiment.GetByID(id);
            Variation variation = experiment.GetAppliedVariation(null, new CustomVariationSampler());
            // get VariationsA even its percentage is 0.
            Assert.AreEqual(experiment.Variations[0], variation);
        }
        private class CustomVariationSampler : VariationSampler
        {
            public Variation ChooseVariation(KiiExperiment experiment, Variation fallback)
            {
                return experiment.Variations[0];
            }
        }
        #endregion

        #region GetByID
        [Test(), KiiUTInfo(
            action = "When we call GetByID() and received json does not have status field",
            expected = "We can get the KiiExperiment whose status is KiiExperimentStatus.PAUSED"
            )]
        public void Test_0010_GetByID_No_Status()
        {
            this.LogIn();
            client.AddResponse(
                200,
                "{" +
                "    \"_id\" : \"ID-001\"," +
                "    \"description\" : \"Experiment for UT\"," +
                "    \"version\" : 2, " +
                "    \"conversionEvents\" : [ " +
                "        { \"name\" : \"Event-A\" }, " +
                "        { \"name\" : \"Event-B\" } " +
                "    ], " +
                "    \"variations\" : [ " +
                "        { " +
                "            \"name\" : \"A\", " +
                "            \"percentage\" : 50, " +
                "            \"variableSet\" : {\"message\" : \"set A\"} " +
                "        }, " +
                "        { " +
                "            \"name\" : \"B\", " +
                "            \"percentage\" : 50, " +
                "            \"variableSet\" : {\"message\" : \"set B\"} " +
                "        } " +
                "    ]" +
                "}");
            KiiExperiment experiment = KiiExperiment.GetByID(id);

            // verify Experiment
            Assert.AreEqual("ID-001", experiment.ID);
            Assert.AreEqual("Experiment for UT", experiment.Description);
            Assert.AreEqual(2, experiment.Version);
            Assert.AreEqual(KiiExperimentStatus.PAUSED, experiment.Status);

            // verify VariationA
            Variation variationA = experiment.GetVariationByName("A");
            Assert.IsNotNull(variationA);
            Assert.AreEqual("A", variationA.Name);
            Assert.AreEqual(50, variationA.Percentage);
            Assert.AreEqual(
                new JsonObject("{\"message\" : \"set A\"}").ToString(),
                variationA.VariableSet.ToString());
            ConversionEvent eventA = experiment.GetConversionEventByName("Event-A");
            Assert.AreEqual("Event-A", eventA.Name);
            // verify VariationB
            Variation variationB = experiment.GetVariationByName("B");
            Assert.AreEqual("B", variationB.Name);
            Assert.AreEqual(50, variationB.Percentage);
            Assert.AreEqual(
                new JsonObject("{\"message\" : \"set B\"}").ToString(),
                variationB.VariableSet.ToString());
            ConversionEvent eventB = experiment.GetConversionEventByName("Event-B");
            Assert.AreEqual("Event-B", eventB.Name);
        }
        #endregion

        #region GetByID
        [Test(), KiiUTInfo(
            action = "When we call GetByID() and type ofstatus in json is string",
            expected = "We can get the KiiExperiment whose status is KiiExperimentStatus.PAUSED"
            )]
        public void Test_0011_GetByID_Status_Is_String()
        {
            this.LogIn();
            client.AddResponse(
                200,
                "{" +
                "    \"_id\" : \"ID-001\"," +
                "    \"description\" : \"Experiment for UT\"," +
                "    \"version\" : 2, " +
                "    \"status\" : \"invalid status\", " +
                "    \"conversionEvents\" : [ " +
                "        { \"name\" : \"Event-A\" }, " +
                "        { \"name\" : \"Event-B\" } " +
                "    ], " +
                "    \"variations\" : [ " +
                "        { " +
                "            \"name\" : \"A\", " +
                "            \"percentage\" : 50, " +
                "            \"variableSet\" : {\"message\" : \"set A\"} " +
                "        }, " +
                "        { " +
                "            \"name\" : \"B\", " +
                "            \"percentage\" : 50, " +
                "            \"variableSet\" : {\"message\" : \"set B\"} " +
                "        } " +
                "    ]" +
                "}");
            KiiExperiment experiment = KiiExperiment.GetByID(id);

            // verify Experiment
            Assert.AreEqual("ID-001", experiment.ID);
            Assert.AreEqual("Experiment for UT", experiment.Description);
            Assert.AreEqual(2, experiment.Version);
            Assert.AreEqual(KiiExperimentStatus.PAUSED, experiment.Status);

            // verify VariationA
            Variation variationA = experiment.GetVariationByName("A");
            Assert.AreEqual("A", variationA.Name);
            Assert.AreEqual(50, variationA.Percentage);
            Assert.AreEqual(
                new JsonObject("{\"message\" : \"set A\"}").ToString(),
                variationA.VariableSet.ToString());
            ConversionEvent eventA = experiment.GetConversionEventByName("Event-A");
            Assert.AreEqual("Event-A", eventA.Name);
            // verify VariationB
            Variation variationB = experiment.GetVariationByName("B");
            Assert.AreEqual("B", variationB.Name);
            Assert.AreEqual(50, variationB.Percentage);
            Assert.AreEqual(
                new JsonObject("{\"message\" : \"set B\"}").ToString(),
                variationB.VariableSet.ToString());
            ConversionEvent eventB = experiment.GetConversionEventByName("Event-B");
            Assert.AreEqual("Event-B", eventB.Name);
        }
        #endregion

        #region GetByID
        [Test(), KiiUTInfo(
            action = "When we call GetByID() and type ofstatus in json is string",
            expected = "We can get the KiiExperiment whose status is KiiExperimentStatus.PAUSED"
            )]
        public void Test_0012_GetByID_Status_Is_Out_Of_Range()
        {
            this.LogIn();
            client.AddResponse(
                200,
                "{" +
                "    \"_id\" : \"ID-001\"," +
                "    \"description\" : \"Experiment for UT\"," +
                "    \"version\" : 2, " +
                "    \"status\" : 1000, " +
                "    \"conversionEvents\" : [ " +
                "        { \"name\" : \"Event-A\" }, " +
                "        { \"name\" : \"Event-B\" } " +
                "    ], " +
                "    \"variations\" : [ " +
                "        { " +
                "            \"name\" : \"A\", " +
                "            \"percentage\" : 50, " +
                "            \"variableSet\" : {\"message\" : \"set A\"} " +
                "        }, " +
                "        { " +
                "            \"name\" : \"B\", " +
                "            \"percentage\" : 50, " +
                "            \"variableSet\" : {\"message\" : \"set B\"} " +
                "        } " +
                "    ]" +
                "}");
            KiiExperiment experiment = KiiExperiment.GetByID(id);

            // verify Experiment
            Assert.AreEqual("ID-001", experiment.ID);
            Assert.AreEqual("Experiment for UT", experiment.Description);
            Assert.AreEqual(2, experiment.Version);
            Assert.AreEqual(KiiExperimentStatus.PAUSED, experiment.Status);

            // verify VariationA
            Variation variationA = experiment.GetVariationByName("A");
            Assert.AreEqual("A", variationA.Name);
            Assert.AreEqual(50, variationA.Percentage);
            Assert.AreEqual(
                new JsonObject("{\"message\" : \"set A\"}").ToString(),
                variationA.VariableSet.ToString());
            ConversionEvent eventA = experiment.GetConversionEventByName("Event-A");
            Assert.AreEqual("Event-A", eventA.Name);
            // verify VariationB
            Variation variationB = experiment.GetVariationByName("B");
            Assert.AreEqual("B", variationB.Name);
            Assert.AreEqual(50, variationB.Percentage);
            Assert.AreEqual(
                new JsonObject("{\"message\" : \"set B\"}").ToString(),
                variationB.VariableSet.ToString());
            ConversionEvent eventB = experiment.GetConversionEventByName("Event-B");
            Assert.AreEqual("Event-B", eventB.Name);
        }
        #endregion

    }
}

