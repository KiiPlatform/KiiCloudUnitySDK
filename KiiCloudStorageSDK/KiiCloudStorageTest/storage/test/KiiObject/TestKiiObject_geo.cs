using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiObject_geo
    {
         private void SetStandardSaveResponse(MockHttpClient client)
        {
            client.AddResponse(201, "{" +
                "\"objectID\" : \"d8dc9f29-0fb9-48be-a80c-ec60fddedb54\"," +
                "\"createdAt\" : 1337039114613," +
                "\"dataType\" : \"application/vnd.sandobx.mydata+json\"" +
                "}",
                "1");
        }

        private void SetStandardRefreshResponse(MockHttpClient client)
        {
            client.AddResponse(200, "{" +
                "\"_id\" : \"d8dc9f29-0fb9-48be-a80c-ec60fddedb54\"," +
                "\"_created\" : 2345," +
                "\"_modified\" : 6789," +
                "\"name\" : \"Kii\"," +
                "\"score\" : 100" +
                "}",
                "1");
        }

        private void SetGeoRefreshResponse(MockHttpClient client)
        {
            client.AddResponse(200, "{" +
                "\"_id\" : \"d8dc9f29-0fb9-48be-a80c-ec60fddedb54\"," +
                "\"_created\" : 2345," +
                "\"_modified\" : 6789," +
                "\"name\" : \"Kii\"," +
                "\"location\" : {\"lat\" : 72.00,\"lon\" : 100.00, \"_type\" : \"point\"}" +
                "}",
                "1");
        }


         [Test(), KiiUTInfo(
            action = "When we call Refresh() with geo data and Server returns KiiObject,",
            expected = "We can get ObjectID/createdTime/modifiedTime/geoPoint that server sends"
            )]
        public void Test_0001_GetGeoPoint_valid_param()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetGeoRefreshResponse(client);

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            obj.Refresh();
            KiiGeoPoint expectedResult = new KiiGeoPoint(72.00,100.00);
            Assert.AreEqual("abcd", obj.ID);
            Assert.AreEqual(2345, obj.CreatedTime);
            Assert.AreEqual(6789, obj.ModifedTime);
            Assert.AreEqual(expectedResult,obj.GetGeoPoint("location"));
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GetGeoPoint() with null key",
            expected = "Throw Argument exception"
            )]
        public void Test_0002_GetGeoPoint_null_key()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetGeoRefreshResponse(client);

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            obj.Refresh();

            Assert.AreEqual("abcd", obj.ID);
            Assert.AreEqual(2345, obj.CreatedTime);
            Assert.AreEqual(6789, obj.ModifedTime);
            obj.GetGeoPoint(null);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GetGeoPoint() with empty key",
            expected = "Throw IllegalKiiBaseObjectFormat exception"
            )]
        public void Test_0003_GetGeoPoint_empty_key()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetGeoRefreshResponse(client);

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            obj.Refresh();

            Assert.AreEqual("abcd", obj.ID);
            Assert.AreEqual(2345, obj.CreatedTime);
            Assert.AreEqual(6789, obj.ModifedTime);
            obj.GetGeoPoint("");
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call GetGeoPoint() with incorrect key",
            expected = "Throw IllegalKiiBaseObjectFormat exception"
            )]
         public void Test_0004_GetGeoPoint_notfound_key()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetGeoRefreshResponse(client);

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            obj.Refresh();

            Assert.AreEqual("abcd", obj.ID);
            Assert.AreEqual(2345, obj.CreatedTime);
            Assert.AreEqual(6789, obj.ModifedTime);
            obj.GetGeoPoint("notfound");
        }

        [Test(), KiiUTInfo(
            action = "When we call Refresh() with geo data and Server returns KiiObject,",
            expected = "We can get ObjectID/createdTime/modifiedTime/geoPoint that server sends"
            )]
        public void Test_0005_GetGeoPoint_fallback_valid_param()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetGeoRefreshResponse(client);

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            obj.Refresh();
            KiiGeoPoint expectedResult = new KiiGeoPoint(72.00,100.00);
            KiiGeoPoint fallBack = new KiiGeoPoint(50.00,100.00);
            Assert.AreEqual("abcd", obj.ID);
            Assert.AreEqual(2345, obj.CreatedTime);
            Assert.AreEqual(6789, obj.ModifedTime);
            Assert.AreEqual(expectedResult,obj.GetGeoPoint("location",fallBack));
        }

        [Test(), KiiUTInfo(
            action = "When we call GetGeoPoint with null key,",
            expected = "We can get fallback"
            )]
        public void Test_0006_GetGeoPoint_fallback_null_key()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetGeoRefreshResponse(client);

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            obj.Refresh();

            KiiGeoPoint fallBack = new KiiGeoPoint(50.00,100.00);
            Assert.AreEqual("abcd", obj.ID);
            Assert.AreEqual(2345, obj.CreatedTime);
            Assert.AreEqual(6789, obj.ModifedTime);
            obj.GetGeoPoint(null,fallBack);
        }

         [Test(), KiiUTInfo(
            action = "When we call GetGeoPoint with empty key,",
            expected = "We can get fallback"
            )]
        public void Test_0007_GetGeoPoint_fallback_empty_key()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetGeoRefreshResponse(client);

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            obj.Refresh();

            KiiGeoPoint fallBack = new KiiGeoPoint(50.00,100.00);
            Assert.AreEqual("abcd", obj.ID);
            Assert.AreEqual(2345, obj.CreatedTime);
            Assert.AreEqual(6789, obj.ModifedTime);
            obj.GetGeoPoint("",fallBack);
        }

        [Test(), KiiUTInfo(
            action = "When we call GetGeoPoint with not found key,",
            expected = "We can get fallback"
            )]
         public void Test_0008_GetGeoPoint_fallback_notfound_key()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetGeoRefreshResponse(client);

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            obj.Refresh();

            KiiGeoPoint fallBack = new KiiGeoPoint(50.00,100.00);
            Assert.AreEqual("abcd", obj.ID);
            Assert.AreEqual(2345, obj.CreatedTime);
            Assert.AreEqual(6789, obj.ModifedTime);
            Assert.AreEqual(fallBack,obj.GetGeoPoint("notfound",fallBack));
        }

        [Test(), KiiUTInfo(
            action = "When we call setGeoPoint of a KiiObject,",
            expected = "We can get ObjectID/createdTime/modifiedTime that server sends"
            )]
        public void Test_0010_SetGeoPointValidParams()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardSaveResponse(client);

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;
            obj.SetGeoPoint("location",new KiiGeoPoint(72.00,100.00));

            obj.SaveAllFields(true);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

        }


       [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call SetGeoPoint() with null key",
            expected = "Throw Argument exception"
            )]
        public void Test_0011_SetGeoPoint_nullKey()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardSaveResponse(client);

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;
            obj.SetGeoPoint(null,new KiiGeoPoint(72.00,100.00));

        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call SetGeoPoint() with empty key",
            expected = "Throw Argument exception"
            )]
        public void Test_0012_SetGeoPoint_emptyKey()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardSaveResponse(client);

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;
            obj.SetGeoPoint("",new KiiGeoPoint(72.00,100.00));

        }

    }
}

