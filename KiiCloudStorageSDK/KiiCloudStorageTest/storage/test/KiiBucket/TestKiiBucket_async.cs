using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiBucket_async
    {
        private MockHttpClient client;

        [SetUp()]
        public void SetUp()
        {
            Kii.Instance = null;

            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;

            client = factory.Client;
        }

        [TearDown()]
        public void TearDown()
        {
            Kii.Instance = null;
        }

        private void SetDefaultQueryResult()
        {
            client.AddResponse(200, "{" +
                "\"queryDescription\" : \"WHERE ( 1 = 1 )\"," +
                "\"results\":[" +
                "{\"_id\":\"497fd6ff-9178-42ec-b6ec-14bce7b5c7c9\",\"name\":\"John Smith\",\"age\":18," +
                "\"_created\":1334505527480,\"_modified\":1334505527480,\"_owner\":\"789399f7-7552-47a8-a524-b9119056edd9\",\"_version\":1}" +
                "]}");
        }

        private void SetDefaultDeleteResult()
        {
            client.AddResponse(204, "");
        }

        #region KiiBucket.Query(KiiQuery, KiiQueryCallback)

        [Test(), KiiUTInfo(
            action = "When we call Query(callback) with null and Server returns 1 KiiObject,",
            expected = "The count of result must be 1"
            )]
        public void Test_0100_query_null ()
        {
            // set response
            this.SetDefaultQueryResult();

            KiiBucket bucket = Kii.Bucket("test");
            bool done = false;
            KiiQueryResult<KiiObject> result = null;
            Exception exception = null;
            bucket.Query(null, (KiiQueryResult<KiiObject> list, Exception e) =>
            {
                done = true;
                result = list;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(result);
            Assert.IsNull(exception);

            Assert.AreEqual(1, result.Count);
        }

        [Test(), KiiUTInfo(
            action = "When we call Query(callback) with all QueryObject and Server returns 1 KiiObject,",
            expected = "The count of result must be 1 and HasNext must be false"
            )]
        public void Test_0101_query_all ()
        {
            // set response
            this.SetDefaultQueryResult();

            KiiBucket bucket = Kii.Bucket("test");
            KiiQuery query = new KiiQuery();
            bool done = false;
            KiiQueryResult<KiiObject> result = null;
            Exception exception = null;
            bucket.Query(query, (KiiQueryResult<KiiObject> list, Exception e) =>
            {
                done = true;
                result = list;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(result);
            Assert.IsNull(exception);

            Assert.AreEqual(1, result.Count);
            Assert.IsFalse(result.HasNext);

        }

        [Test(), KiiUTInfo(
            action = "When we call Query(callback) with all QueryObject and Server returns nextPaginationKey,",
            expected = "HasNext must be true"
            )]
        public void Test_0102_query_next ()
        {
            // set response
            client.AddResponse(200, "{" +
                "\"queryDescription\" : \"WHERE ( 1 = 1 )\"," +
                "\"results\":[" +
                "{\"_id\":\"497fd6ff-9178-42ec-b6ec-14bce7b5c7c9\",\"name\":\"Nick\",\"age\":18," +
                "\"_created\":1334505527480,\"_modified\":1334505527480,\"_owner\":\"789399f7-7552-47a8-a524-b9119056edd9\",\"_version\":1}" +
                "]," +
                "\"nextPaginationKey\":\"abcd\"" +
                "}");

            KiiBucket bucket = Kii.Bucket("test");
            KiiQuery query = new KiiQuery();

            bool done = false;
            int calledCount = 0;
            KiiQueryCallback<KiiObject> callback = null;
            callback = (KiiQueryResult<KiiObject> result, Exception e) =>
            {
                ++calledCount;
                if (calledCount == 1)
                {
                    // first result
                    Assert.AreEqual(1, result.Count);
                    Assert.IsTrue(result.HasNext);
                    Assert.AreEqual("Nick", result[0]["name"]);

                    // set response
                    this.SetDefaultQueryResult();
                    result.GetNextQueryResult(callback);
                }
                else
                {
                    Assert.AreEqual(1, result.Count);
                    Assert.IsFalse(result.HasNext);
                    Assert.AreEqual("John Smith", result[0]["name"]);

                    done = true;
                }

            };
            bucket.Query(query, callback);

            Assert.IsTrue(done);

        }

        [Test(), KiiUTInfo(
            action = "When we call Query(callback) and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0103_query_broken_json ()
        {
            // set response
            client.AddResponse(200, "{}");

            KiiBucket bucket = Kii.Bucket("test");
            bool done = false;
            KiiQueryResult<KiiObject> result = null;
            Exception exception = null;
            bucket.Query(null, (KiiQueryResult<KiiObject> list, Exception e) =>
            {
                done = true;
                result = list;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNull(result);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is IllegalKiiBaseObjectFormatException);
        }

        [Test(), KiiUTInfo(
            action = "When we call Query(callback) and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0104_query_server_error ()
        {
            // set response
            client.AddResponse(new CloudException(400, "{}"));

            KiiBucket bucket = Kii.Bucket("test");
            bool done = false;
            KiiQueryResult<KiiObject> result = null;
            Exception exception = null;
            bucket.Query(null, (KiiQueryResult<KiiObject> list, Exception e) =>
            {
                done = true;
                result = list;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNull(result);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is CloudException);
        }

        #endregion

        #region KiiBucket.Delete()

        [Test(), KiiUTInfo(
            action = "When we call Delete(callback) and Server returns HTTP 204(OK),",
            expected = "No Exception must be thrown"
            )]
        public void Test_0200_delete ()
        {
            // set response
            this.SetDefaultDeleteResult();

            KiiBucket bucket = Kii.Bucket("test");
            bool done = false;
            KiiBucket result = null;
            Exception exception = null;
            bucket.Delete((KiiBucket deletedBucket, Exception e) =>
            {
                done = true;
                result = deletedBucket;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(result);
            Assert.IsNull(exception);
        }

        [Test(), KiiUTInfo(
            action = "When we call Delete(callback) and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0201_delete_server_exception ()
        {
            // set response
            client.AddResponse(new CloudException(400, "{}"));

            KiiBucket bucket = Kii.Bucket("test");
            bool done = false;
            KiiBucket result = null;
            Exception exception = null;
            bucket.Delete((KiiBucket deletedBucket, Exception e) =>
            {
                done = true;
                result = deletedBucket;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(result);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is CloudException);
        }

        #endregion
    }
}

