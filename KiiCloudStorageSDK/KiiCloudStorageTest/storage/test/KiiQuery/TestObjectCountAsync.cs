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
    // Test spec : https://docs.google.com/a/kii.com/spreadsheet/ccc?key=0AsiWA7MkWrQldHFnX3I2bXBzQlhMSlRuTG9JdUQ5X0E&usp=drive_web#gid=5
    [TestFixture()]
    public class TestObjectCountAsync
    {
        [SetUp()]
        public void SetUp(){
            Kii.Initialize("appId", "appKey", Kii.Site.US);
        }

        [Test()]
        public void Test_1_2_CountAllAsync(){
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"aggregations\" : { \"count_field\" : 10 } }");
            
            string bucketName = "TestBucket";
            KiiBucket bucket = Kii.Bucket(bucketName);

            KiiBucket callbackBucket = null;
            KiiQuery callbackQuery = null;
            int count = -1;
            Exception exp = null;
            CountDownLatch cd = new CountDownLatch(1);
            bucket.Count((KiiBucket b, KiiQuery q, int c, Exception e) => {
                callbackBucket = b;
                callbackQuery = q;
                count = c;
                exp = e;
                cd.Signal();
            });

            if(!cd.Wait(new TimeSpan(0, 0, 0, 3)))
                Assert.Fail("Callback not fired.");
            Assert.IsNotNull(callbackBucket);
            Assert.AreEqual(bucket.Name, callbackBucket.Name);
            Assert.IsNotNull(callbackQuery);
            KiiAssertion.AssertJson(new KiiQuery().toJson(), callbackQuery.toJson());
            Assert.IsNull(exp);
            Assert.AreEqual(10, count);
            
            // check request.
            Console.WriteLine(client.RequestBody[0]);
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","TestBucket", "query");
            Assert.AreEqual(url, client.RequestUrl[0]);
            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod[0]);
            MockHttpHeaderList headerList = client.RequestHeader[0];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            string queryStr = "{ " +
                "\"bucketQuery\" : {" +
                    "\"clause\" : {" +
                        "\"type\" : \"all\"" +
                    "}," +
                    "\"aggregations\" : [ {" +
                        "\"type\" : \"COUNT\"," +
                        "\"putAggregationInto\" : \"count_field\"" +
                    "}]" +
                "}" +
            "}";
            JsonObject expectedBodyJson = new JsonObject(queryStr);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[0]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call count(CountCallback) with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_1_3_CountAllAsyncWithNullCallback(){
            string bucketName = "TestBucket";
            KiiBucket bucket = Kii.Bucket(bucketName);
            CountCallback callback = null;
            bucket.Count(callback);
        }

        [Test()]
        public void Test_2_2_CountWithQueryAllAsync(){
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"aggregations\" : { \"count_field\" : 10 } }");
            
            string bucketName = "TestBucket";
            KiiBucket bucket = Kii.Bucket(bucketName);
            KiiQuery query = new KiiQuery(null);
            
            KiiBucket callbackBucket = null;
            KiiQuery callbackQuery = null;
            int count = -1;
            Exception exp = null;
            CountDownLatch cd = new CountDownLatch(1);
            bucket.Count(query,(KiiBucket b, KiiQuery q, int c, Exception e) => {
                callbackBucket = b;
                callbackQuery = q;
                count = c;
                exp = e;
                cd.Signal();
            });
            
            if(!cd.Wait(new TimeSpan(0, 0, 0, 3)))
                Assert.Fail("Callback not fired.");
            Assert.IsNotNull(callbackBucket);
            Assert.AreEqual(bucket.Name, callbackBucket.Name);
            Assert.IsNotNull(callbackQuery);
            KiiAssertion.AssertJson(query.toJson(), callbackQuery.toJson());
            Assert.IsNull(exp);
            Assert.AreEqual(10, count);
            
            // check request.
            Console.WriteLine(client.RequestBody[0]);
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","TestBucket", "query");
            Assert.AreEqual(url, client.RequestUrl[0]);
            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod[0]);
            MockHttpHeaderList headerList = client.RequestHeader[0];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            string queryStr = "{ " +
                "\"bucketQuery\" : {" +
                    "\"clause\" : {" +
                    "\"type\" : \"all\"" +
                    "}," +
                    "\"aggregations\" : [ {" +
                    "\"type\" : \"COUNT\"," +
                    "\"putAggregationInto\" : \"count_field\"" +
                    "}]" +
                    "}" +
                    "}";
            JsonObject expectedBodyJson = new JsonObject(queryStr);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[0]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call count(KiiQuery, CountCallback) with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_2_3_CountAllAsyncWithNullCallback(){
            string bucketName = "TestBucket";
            KiiBucket bucket = Kii.Bucket(bucketName);
            KiiQuery query = new KiiQuery(null);
            CountCallback callback = null;
            bucket.Count(query, callback);
        }
    }
}

