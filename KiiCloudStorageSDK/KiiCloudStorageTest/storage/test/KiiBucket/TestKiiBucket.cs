using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiBucket
    {
        [SetUp()]
        public void SetUp()
        {
            Kii.Instance = null;
        }

        [TearDown()]
        public void TearDown()
        {
            Kii.Instance = null;
        }

        private void SetDefaultQueryResult(MockHttpClient client)
        {
            client.AddResponse(200, "{" +
                "\"queryDescription\" : \"WHERE ( 1 = 1 )\"," +
                "\"results\":[" +
                "{\"_id\":\"497fd6ff-9178-42ec-b6ec-14bce7b5c7c9\",\"name\":\"John Smith\",\"age\":18," +
                "\"_created\":1334505527480,\"_modified\":1334505527480,\"_owner\":\"789399f7-7552-47a8-a524-b9119056edd9\",\"_version\":1}" +
                "]}");
        }

        private void SetDefaultDeleteResult(MockHttpClient client)
        {
            client.AddResponse(204, "");
        }

        #region KiiBucket.Name
        [Test(), KiiUTInfo(
            action = "When we create KiiBucket and access Name property,",
            expected = "It must be backet name"
        )]
        public void Test_Name ()
        {
            KiiBucket appScope = Kii.Bucket("app_bucket");
            Assert.AreEqual("app_bucket", appScope.Name);
        }
        #endregion

        #region KiiBucket.NewKiiObject()
        [Test(), KiiUTInfo(
            action = "When we create KiiBucket and call NewKiiObject(),",
            expected = "It must not be null"
            )]
        public void Test_0000_NewObject ()
        {
            KiiBucket bucket = Kii.Bucket("test");
            KiiObject obj = bucket.NewKiiObject();
            Assert.IsNotNull(obj);
        }

        #endregion

        #region KiiBucket.Query()

        [Test(), KiiUTInfo(
            action = "When we call Query() with null and Server returns 1 KiiObject,",
            expected = "The count of result must be 1"
            )]
        public void Test_0100_query_null ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = (MockHttpClient)factory.Client;
            this.SetDefaultQueryResult(client);

            KiiBucket bucket = Kii.Bucket("test");
            KiiQueryResult<KiiObject> result = bucket.Query(null);
            Assert.AreEqual(1, result.Count);
        }

        [Test(), KiiUTInfo(
            action = "When we call Query() with all QueryObject and Server returns 1 KiiObject,",
            expected = "The count of result must be 1 and HasNext must be false"
            )]
        public void Test_0101_query_all ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = (MockHttpClient)factory.Client;
            this.SetDefaultQueryResult(client);

            KiiBucket bucket = Kii.Bucket("test");
            KiiQuery query = new KiiQuery();
            KiiQueryResult<KiiObject> result = bucket.Query(query);
            Assert.AreEqual(1, result.Count);
            Assert.IsFalse(result.HasNext);

        }

        [Test(), KiiUTInfo(
            action = "When we call Query() with all QueryObject and Server returns nextPaginationKey,",
            expected = "HasNext must be true"
            )]
        public void Test_0102_query_next ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = (MockHttpClient)factory.Client;
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
            KiiQueryResult<KiiObject> result = bucket.Query(query);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.HasNext);
            Assert.AreEqual("Nick", result[0]["name"]);

            // set response
            this.SetDefaultQueryResult(client);

            // next result
            result = result.GetNextQueryResult();
            Assert.AreEqual(1, result.Count);
            Assert.IsFalse(result.HasNext);
            Assert.AreEqual("John Smith", result[0]["name"]);
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call Query() and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0103_query_broken_json ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = (MockHttpClient)factory.Client;
            client.AddResponse(200, "{}");

            KiiBucket bucket = Kii.Bucket("test");
            bucket.Query(null);
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call Query() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0104_query_server_error ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = (MockHttpClient)factory.Client;
            client.AddResponse(new CloudException(400, "{}"));

            KiiBucket bucket = Kii.Bucket("test");
            bucket.Query(null);
        }

        #endregion

        #region KiiBucket.Delete()

        [Test(), KiiUTInfo(
            action = "When we call Delete() and Server returns HTTP 204(OK),",
            expected = "No Exception must be thrown"
            )]
        public void Test_0200_delete ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = (MockHttpClient)factory.Client;
            this.SetDefaultDeleteResult(client);

            KiiBucket bucket = Kii.Bucket("test");
            bucket.Delete();
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call Delete() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0201_delete_server_exception ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = (MockHttpClient)factory.Client;
            client.AddResponse(new CloudException(400, "{}"));

            KiiBucket bucket = Kii.Bucket("test");
            bucket.Delete();
        }

        #endregion

        #region KiiBucket.Uri

        [Test(), KiiUTInfo(
            action = "When we create AppScope KiiBucket,",
            expected = "Uri must be kiicloud://buckets/name1234"
            )]
        public void Test_0300_Uri_AppScope()
        {
            KiiBucket bucket = Kii.Bucket("name1234");
            Assert.AreEqual("kiicloud://buckets/name1234", bucket.Uri.ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we create UserScope KiiBucket,",
            expected = "Uri must be kiicloud://users/user1234/buckets/name1234"
            )]
        public void Test_0301_Uri_UserScope()
        {
            Kii.Initialize("abbId", "appKey", Kii.Site.JP);

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234"));
            KiiBucket bucket = user.Bucket("name1234");
            Assert.AreEqual("kiicloud://users/user1234/buckets/name1234", bucket.Uri.ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we create UserScope KiiBucket whose Uri ends with /,",
            expected = "Uri must be kiicloud://users/user1234/buckets/name1234"
            )]
        public void Test_0302_Uri_UserScope_endWith_slash()
        {
            Kii.Initialize("abbId", "appKey", Kii.Site.JP);

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/user1234/"));
            KiiBucket bucket = user.Bucket("name1234");
            Assert.AreEqual("kiicloud://users/user1234/buckets/name1234", bucket.Uri.ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we create GroupScope KiiBucket,",
            expected = "Uri must be kiicloud://groups/group1234/buckets/name1234"
            )]
        public void Test_0303_Uri_GroupScope()
        {
            Kii.Initialize("abbId", "appKey", Kii.Site.JP);

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234"));
            KiiBucket bucket = group.Bucket("name1234");
            Assert.AreEqual("kiicloud://groups/group1234/buckets/name1234", bucket.Uri.ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we create GroupScope KiiBucket whose Uri ends with /,",
            expected = "Uri must be kiicloud://groups/group1234/buckets/name1234"
            )]
        public void Test_0304_Uri_GroupScope_endWith_shash()
        {
            Kii.Initialize("abbId", "appKey", Kii.Site.JP);

            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/group1234/"));
            KiiBucket bucket = group.Bucket("name1234");
            Assert.AreEqual("kiicloud://groups/group1234/buckets/name1234", bucket.Uri.ToString());
        }
        #endregion
    }
}

