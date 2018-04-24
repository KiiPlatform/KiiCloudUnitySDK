using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;

namespace KiiCorp.Cloud.ABTesting
{
    [TestFixture()]
    public class TestVariationSamplerByKiiUser
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
            // VariationSamplerByKiiUser uses UserID as seed in order to generate random.
            System.Threading.Thread.Sleep(1);
            string userID = "user" + Environment.TickCount;
            client.AddResponse(200, "{" +
                               "\"id\" : \"" + userID + "\"," +
                               "\"access_token\" : \"cdef\"," +
                               "\"expires_in\" : 9223372036854775}");
            KiiUser.LogIn(userID, "pass1234");
        }
        private String CreateKiiExperimentAsJsonString(KiiExperimentStatus status, int percentageA, int percentageB, string chosenVariationName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("\"_id\"" + ":" + "\"ID-001\"" + ",");
            sb.AppendLine("\"description\"" + ":" + "\"Experiment for UT\"" + ",");
            sb.AppendLine("\"version\"" + ":" + "1" + ",");
            sb.AppendLine("\"status\"" + ":" + ((int)status) + ",");

            sb.AppendLine("\"conversionEvents\"" + ":");
            sb.Append("[");
            sb.Append("{\"name\":\"Event-A\"}");
            sb.Append(",");
            sb.Append("{\"name\":\"Event-B\"}");
            sb.Append("],");

            sb.AppendLine("\"variations\"" + ":");
            sb.Append("[");
            sb.Append("{\"name\":\"A\", \"percentage\":" + percentageA + ", \"variableSet\":{}}");
            sb.Append(",");
            sb.Append("{\"name\":\"B\", \"percentage\":" + percentageB + ", \"variableSet\":{}}");
            sb.Append("]");
            if (!Utils.IsEmpty (chosenVariationName))
            {
                sb.Append(",");
                sb.AppendLine("\"chosenVariation\"" + ":" + "\"" + chosenVariationName + "\"");
            }
            sb.AppendLine("}");
            return sb.ToString ();
        }

        [Test(), KiiUTInfo(
            action = "When we call OnCrank(KiiExperiment, Variation) and method returns Variation at random",
            expected = "We can get the Variation at random (A = 50%, B = 50%)"
            )]
        public void Test_0000_OnCrank_Percentage_50_50()
        {
            this.LogIn();
            VariationSampler sampler = new VariationSamplerByKiiUser();
            client.AddResponse(200, CreateKiiExperimentAsJsonString(KiiExperimentStatus.RUNNING, 50, 50, null));
            KiiExperiment experiment = KiiExperiment.GetByID("000001");
            Variation variationA = experiment.Variations[0]; // 50%
            Variation variationB = experiment.Variations[1]; // 50%
            int selectedCountA = 0;
            int selectedCountB = 0;
            for (int i = 0; i < 1000; i++)
            {
                this.LogIn();
                Variation variation = sampler.ChooseVariation(experiment, null);
                if (variation == variationA)
                {
                    selectedCountA++;
                }
                else if (variation == variationB)
                {
                    selectedCountB++;
                }
                else
                {
                    Assert.Fail("sampler returned unexpected variation");
                }
            }
            // This test will fail with a probability of 0.0017305361
            Assert.IsTrue(450 < selectedCountA, "selectedCountA=" + selectedCountA);
            Assert.IsTrue(550 > selectedCountA, "selectedCountA=" + selectedCountA);
            Assert.IsTrue(450 < selectedCountB, "selectedCountB=" + selectedCountB);
            Assert.IsTrue(550 > selectedCountB, "selectedCountB=" + selectedCountB);
        }
        [Test(), KiiUTInfo(
            action = "When we call OnCrank(KiiExperiment, Variation) and method returns Variation at random",
            expected = "We can get the Variation at random (A = 100%, B = 0%)"
            )]
        public void Test_0001_OnCrank_Percentage_100_0()
        {
            this.LogIn();
            VariationSampler sampler = new VariationSamplerByKiiUser();
            client.AddResponse(200, CreateKiiExperimentAsJsonString(KiiExperimentStatus.RUNNING, 100, 0, null));
            KiiExperiment experiment = KiiExperiment.GetByID("000001");
            Variation variationA = experiment.Variations[0]; // 100%
            Variation variationB = experiment.Variations[1]; // 0%
            bool selectedA = false;
            bool selectedB = false;
            for (int i = 0; i < 1000; i++)
            {
                this.LogIn();
                Variation variation = sampler.ChooseVariation(experiment, null);
                if (variation == variationA)
                {
                    selectedA = true;
                }
                else if (variation == variationB)
                {
                    selectedB = true;
                }
                else
                {
                    Assert.Fail("sampler returned unexpected variation");
                }
            }
            Assert.IsTrue(selectedA);
            Assert.IsFalse(selectedB);
        }
        [Test(), KiiUTInfo(
            action = "When we call OnCrank(KiiExperiment, Variation) and method returns Variation at random",
            expected = "We can get the Variation at random (A = 0%, B = 100%)"
            )]
        public void Test_0002_OnCrank_Percentage_0_100()
        {
            this.LogIn();
            VariationSampler sampler = new VariationSamplerByKiiUser();
            client.AddResponse(200, CreateKiiExperimentAsJsonString(KiiExperimentStatus.RUNNING, 0, 100, null));
            KiiExperiment experiment = KiiExperiment.GetByID("000001");
            Variation variationA = experiment.Variations[0]; // 0%
            Variation variationB = experiment.Variations[1]; // 100%
            bool selectedA = false;
            bool selectedB = false;
            for (int i = 0; i < 1000; i++)
            {
                this.LogIn();
                Variation variation = sampler.ChooseVariation(experiment, null);
                if (variation == variationA)
                {
                    selectedA = true;
                }
                else if (variation == variationB)
                {
                    selectedB = true;
                }
                else
                {
                    Assert.Fail("sampler returned unexpected variation");
                }
            }
            Assert.IsFalse(selectedA);
            Assert.IsTrue(selectedB);
        }
        [Test(), KiiUTInfo(
            action = "When we call OnCrank(KiiExperiment, Variation) and method returns Variation at random",
            expected = "We can get the Variation at random (A = 5%, B = 95%)"
            )]
        public void Test_0003_OnCrank_Percentage_5_95()
        {
            this.LogIn();
            VariationSampler sampler = new VariationSamplerByKiiUser();
            client.AddResponse(200, CreateKiiExperimentAsJsonString(KiiExperimentStatus.RUNNING, 5, 95, null));
            KiiExperiment experiment = KiiExperiment.GetByID("000001");
            Variation variationA = experiment.Variations[0]; // 5%
            Variation variationB = experiment.Variations[1]; // 95%
            int selectedCountA = 0;
            int selectedCountB = 0;
            for (int i = 0; i < 1000; i++)
            {
                this.LogIn();
                Variation variation = sampler.ChooseVariation(experiment, null);
                if (variation == variationA)
                {
                    selectedCountA++;
                }
                else if (variation == variationB)
                {
                    selectedCountB++;
                }
                else
                {
                    Assert.Fail("sampler returned unexpected variation");
                }
            }
            // This test will fail with a probability of 0.0349698573
            Assert.IsTrue(35 < selectedCountA, "selectedCountA=" + selectedCountA);
            Assert.IsTrue(65 > selectedCountA, "selectedCountA=" + selectedCountA);
        }
        [Test(), KiiUTInfo(
            action = "When we call OnCrank(KiiExperiment, Variation) and method returns Variation at random",
            expected = "We can get the Variation at random (A = 5%, B = 95%)"
            )]
        public void Test_0004_OnCrank_Percentage_95_5()
        {
            this.LogIn();
            VariationSampler sampler = new VariationSamplerByKiiUser();
            client.AddResponse(200, CreateKiiExperimentAsJsonString(KiiExperimentStatus.RUNNING, 95, 5, null));
            KiiExperiment experiment = KiiExperiment.GetByID("000001");
            Variation variationA = experiment.Variations[0]; // 95%
            Variation variationB = experiment.Variations[1]; // 5%
            int selectedCountA = 0;
            int selectedCountB = 0;
            for (int i = 0; i < 1000; i++)
            {
                this.LogIn();
                Variation variation = sampler.ChooseVariation(experiment, null);
                if (variation == variationA)
                {
                    selectedCountA++;
                }
                else if (variation == variationB)
                {
                    selectedCountB++;
                }
                else
                {
                    Assert.Fail("sampler returned unexpected variation");
                }
            }
            // This test will fail with a probability of 0.0349698573
            Assert.IsTrue(35 < selectedCountB, "selectedCountB=" + selectedCountB);
            Assert.IsTrue(65 > selectedCountB, "selectedCountB=" + selectedCountB);
        }
        [Test(), KiiUTInfo(
            action = "When we call OnCrank(KiiExperiment, Variation) and method returns Variation at random",
            expected = "We can get the fallback variation when status of Experiment is DRAFT"
            )]
        public void Test_0005_OnCrank_With_Status_DRAFT()
        {
            this.LogIn();
            VariationSampler sampler = new VariationSamplerByKiiUser();
            client.AddResponse(200, CreateKiiExperimentAsJsonString(KiiExperimentStatus.DRAFT, 50, 50, null));
            KiiExperiment experiment = KiiExperiment.GetByID("000001");
            Variation variationA = experiment.Variations[0]; // 50%
            Variation variationB = experiment.Variations[1]; // 50%

            Variation variation = sampler.ChooseVariation(experiment, null);
            Assert.IsNull(variation);
            variation = sampler.ChooseVariation(experiment, variationA);
            Assert.AreEqual(variationA, variation);
            variation = sampler.ChooseVariation(experiment, variationB);
            Assert.AreEqual(variationB, variation);
        }
        [Test(), KiiUTInfo(
            action = "When we call OnCrank(KiiExperiment, Variation) and method returns Variation at random",
            expected = "We can get the fallback variation when status of Experiment is PAUSED"
            )]
        public void Test_0006_OnCrank_With_Status_PAUSED()
        {
            this.LogIn();
            VariationSampler sampler = new VariationSamplerByKiiUser();
            client.AddResponse(200, CreateKiiExperimentAsJsonString(KiiExperimentStatus.PAUSED, 50, 50, null));
            KiiExperiment experiment = KiiExperiment.GetByID("000001");
            Variation variationA = experiment.Variations[0]; // 50%
            Variation variationB = experiment.Variations[1]; // 50%

            Variation variation = sampler.ChooseVariation(experiment, null);
            Assert.IsNull(variation);
            variation = sampler.ChooseVariation(experiment, variationA);
            Assert.AreEqual(variationA, variation);
            variation = sampler.ChooseVariation(experiment, variationB);
            Assert.AreEqual(variationB, variation);
        }
        [Test(), KiiUTInfo(
            action = "When we call OnCrank(KiiExperiment, Variation) and method returns Variation at random",
            expected = "We can get the fallback variation when status of Experiment is TERMINATED and ChosenVariation is null"
            )]
        public void Test_0007_OnCrank_With_Status_TERMINATED_Whitout_ChosenVariation()
        {
            this.LogIn();
            VariationSampler sampler = new VariationSamplerByKiiUser();
            client.AddResponse(200, CreateKiiExperimentAsJsonString(KiiExperimentStatus.TERMINATED, 50, 50, null));
            KiiExperiment experiment = KiiExperiment.GetByID("000001");
            Variation variationA = experiment.Variations[0]; // 50%
            Variation variationB = experiment.Variations[1]; // 50%
            
            Variation variation = sampler.ChooseVariation(experiment, null);
            Assert.IsNull(variation);
            variation = sampler.ChooseVariation(experiment, variationA);
            Assert.AreEqual(variationA, variation);
            variation = sampler.ChooseVariation(experiment, variationB);
            Assert.AreEqual(variationB, variation);
        }
        [Test(), KiiUTInfo(
            action = "When we call OnCrank(KiiExperiment, Variation) and method returns Variation at random",
            expected = "We can get the ChosenVariation when status of Experiment is TERMINATED and KiiExperiment has ChosenVariation"
            )]
        public void Test_0008_OnCrank_With_Status_TERMINATED_Whit_ChosenVariation()
        {
            this.LogIn();
            VariationSampler sampler = new VariationSamplerByKiiUser();
            client.AddResponse(200, CreateKiiExperimentAsJsonString(KiiExperimentStatus.TERMINATED, 50, 50, "B"));
            KiiExperiment experiment = KiiExperiment.GetByID("000001");
            Variation variationA = experiment.Variations[0]; // 50%
            Variation variationB = experiment.Variations[1]; // 50%
            
            Variation variation = sampler.ChooseVariation(experiment, null);
            Assert.AreEqual(variationB, variation);
            variation = sampler.ChooseVariation(experiment, variationA);
            Assert.AreEqual(variationB, variation);
            variation = sampler.ChooseVariation(experiment, variationB);
            Assert.AreEqual(variationB, variation);
        }
    }
}

