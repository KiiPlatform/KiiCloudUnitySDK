using System;
using System.Collections.Generic;

using JsonOrg;
using KiiCorp.Cloud.Storage;
using NUnit.Framework;

namespace KiiCorp.Cloud.Analytics
{
    [TestFixture()]
    public class TestKiiAnalytics_result
    {
        private MockHttpClient client;
        [SetUp()]
        public void SetUp()
        {
            KiiAnalytics.Instance = null;
            KiiAnalytics.Initialize("appId", "appKey", KiiAnalytics.Site.US, "dev001");
            KiiAnalytics.HttpClientFactory = new MockHttpClientFactory();
            client = ((MockHttpClientFactory)KiiAnalytics.HttpClientFactory).Client;
        }
        
        [Test()]
        public void Test_0000_result ()
        {
            // set Response
            client.AddResponse(200, "{" +
                "\"snapshots\" : [ {" +
                "\"name\" : \"Male\"," +
                "\"data\" : [ 222.294835, 184.859009, 453.422571, 185.310889, 434.687488, 315.944617, 218.675664, 332.096156, 474.059216, 427.635396, 607.377385, 182.989111, 267.282833, 237.294397, 585.562209, 127.352572, 456.195061, 529.733865, 455.323995, 304.316517, 275.685962, 291.335683, 354.212864, 106.744223, 483.895313, 457.807643, 370.337862, 353.872758, 454.131286, 424.851845, 484.060775, 408.070000, 281.822881, 479.177081, 460.197312, 523.185927, 490.907883, 514.245721, 216.094391, 328.348445, 435.999564, 380.014523, 400.103032, 405.576053, 187.472656, 444.090072, 431.877222, 544.836822, 217.047206, 297.560631, 314.844519, 474.073309, 364.085242, 552.185623, 546.595184, 288.153741, 557.183213, 96.317067, 265.017995, 385.089756, 300.245837, 386.901327, 322.140900, 277.306892, 466.558671, 410.920488, 609.337438, 304.479284, 497.136333, 310.189396, 281.707101, 234.958195, 264.602315, 284.586213, 251.030762, 371.324380, 263.875690, 329.653625, 388.521379, 361.096650, 469.496049, 196.725086, 452.307670, 307.137883, 318.312414, 248.647910, 276.700819, 189.217764, 546.775207 ]," +
                "\"pointStart\" : 1338044400000," +
                "\"pointInterval\" : 86400000}, " +
                "{" +
                "\"name\" : \"Female\"," +
                "\"data\" : [ 346.305236, 501.444413, 617.671229, 537.357017, 355.478067, 404.642204, 364.288033, 565.898442, 270.366472, 111.157234, 304.692874, 377.427459, 297.359837, 446.025727, 194.513834, 227.794766, 496.519506, 382.536019, 322.876513, 523.961579, 305.243624, 335.280346, 394.433233, 204.628791, 331.097080, 345.934077, 317.861409, 309.969498, 500.848069, 282.480485, 409.545528, 422.166471, 397.133416, 368.552844, 392.834897, 396.818330, 315.194726, 187.574257, 381.826013, 545.207723, 339.240000, 84.996984, 486.240724, 328.812197, 369.365575, 171.141350, 427.793929, 290.233798, 443.794539, 243.286860, 311.815673, 341.051886, 340.875463, 260.142600, 381.126592, 276.706563, 211.045746, 530.105022, 228.239281, 337.375485, 301.008514, 309.429648, 277.987930, 467.552527, 310.626487, 308.933878, 210.379692, 184.297412, 289.776182, 594.744936, 463.423854, 326.469224, 289.920369, 291.238148, 408.031207, 244.472051, 308.886730, 522.389382, 461.119893, 291.509892, 354.703656, 422.712123, 279.998337, 360.828966, 251.726391, 253.988308, 176.768056, 253.481142, 325.216381 ]," +
                "\"pointStart\" : 1338044400000," +
                "\"pointInterval\" : 86400000}" +
                "]}");
            
            GroupedResult result = KiiAnalytics.GetResult("a", null);
            
            Assert.IsNotNull(result.SnapShots);
            IList<GroupedSnapShot> snapshots = result.SnapShots;
            Assert.AreEqual(2, snapshots.Count);
            
            GroupedSnapShot snapShot1 = snapshots[0];
            Assert.AreEqual("Male", snapShot1.Name);
            Assert.AreEqual(1338044400000, snapShot1.PointStart);
            Assert.AreEqual(86400000, snapShot1.PointInterval);
            JsonArray data = snapShot1.Data;
            Assert.AreEqual(89, data.Length());

            GroupedSnapShot snapShot2 = snapshots[1];
            Assert.AreEqual("Female", snapShot2.Name);
            Assert.AreEqual(1338044400000, snapShot2.PointStart);
            Assert.AreEqual(86400000, snapShot2.PointInterval);
            data = snapShot2.Data;
            Assert.AreEqual(89, data.Length());
        
        }
        
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_0010_result_ruleId_null ()
        {
            KiiAnalytics.GetResult(null, null);
        }

        [Test(), ExpectedException(typeof(CloudException))]
        public void Test_0011_result_server_error ()
        {
            // set response
            client.AddResponse(new CloudException(400, ""));
            KiiAnalytics.GetResult("id1", null);
        }
        
        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException))]
        public void Test_0012_result_broken_json ()
        {
            // set response
            client.AddResponse(200, "broken");
            KiiAnalytics.GetResult("id1", null);
        }
    }
}

