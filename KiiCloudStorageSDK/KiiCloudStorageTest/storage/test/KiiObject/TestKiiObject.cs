using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiObject
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


        [Test(), KiiUTInfo(
            action = "When we call Save() and Server returns objectID/createdAt,",
            expected = "We can get ObjectID/CreatedTime/Modifiedtime that server sends"
            )]
        public void Test_0000_Save ()
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

            obj.Save();

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call Save() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0001_Save_server_error ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new CloudException(400, "{}"));

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            obj.Save();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call Save() and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0002_Save_broken_json ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "{}");

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            obj.Save();
        }

        [Test(), KiiUTInfo(
            action = "When we call Save() and Server returns objectID/createdAt,",
            expected = "We can get ObjectID/CreatedTime/Modifiedtime that server sends"
            )]
        public void Test_0003_Save_2times ()
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

            obj.Save();

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // clear stub
            client.RequestBody.Clear();
            // set Response
            client.AddResponse(200, "{\"_created\": 1234,\"_modified\": 3456}");

            obj["score"] = 80;

            obj.Save();

            Assert.AreEqual("{\"score\":80}", client.RequestBody[0]);

        }

        [Test(), KiiUTInfo(
            action = "When we call Save() for updating and Server returns _created/_modified,",
            expected = "We can get createdTime/modifiedTime that server sends"
            )]
        public void Test_0010_Save_Update ()
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

            obj.Save();

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{\"_created\": 1234,\"_modified\": 3456}");

            obj.Save();
            Assert.AreEqual(1234, obj.CreatedTime);
            Assert.AreEqual(3456, obj.ModifedTime);

        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call Save() for updating and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0011_Save_Update_server_error ()
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

            obj.Save();

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(new CloudException(400, "{}"));

            obj.Save();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call Save() for updating and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0012_Save_Update_broken_json ()
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

            obj.Save();

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{}");

            obj.Save();
        }

        [Test(), KiiUTInfo(
            action = "When we call Save() with false(no overwrite) and Server returns _created/_modified,",
            expected = "We can get createdTime/modifiedTime that server sends"
            )]
        public void Test_0020_Save_Update_No_Overwrite ()
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

            obj.Save();

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{\"_created\": 1234,\"_modified\": 3456}");

            obj.Save(false);
            Assert.AreEqual(1234, obj.CreatedTime);
            Assert.AreEqual(3456, obj.ModifedTime);

        }

        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call Save() with false(no overwrite) and no etag,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_0021_Save_Update_No_Overwrite_No_Etag ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                "\"objectID\" : \"d8dc9f29-0fb9-48be-a80c-ec60fddedb54\"," +
                "\"createdAt\" : 1337039114613," +
                "\"dataType\" : \"application/vnd.sandobx.mydata+json\"" +
                "}",
                null);

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            obj.Save();

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{\"_created\": 1234,\"_modified\": 3456}");

            obj.Save(false);
        }

        [Test(), KiiUTInfo(
            action = "When we call SaveAllFields() with true(overwrite) and Server sends ObjectId/createdAt,",
            expected = "We can get ObjectID/createdTime/modifiedTime that server sends"
            )]
        public void Test_0100_SaveAllField ()
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

            obj.SaveAllFields(true);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

        }

        [Test(), KiiUTInfo(
            action = "When we call SaveAllFields() with false(no overwrite) and Server sends ObjectId/createdAt,",
            expected = "We can get ObjectID/createdTime/modifiedTime that server sends"
            )]
        public void Test_0101_SaveAllField_false ()
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

            obj.SaveAllFields(false);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

        }

        [Test(), KiiUTInfo(
            action = "When we call SaveAllFields() with true(overwrite) for updating and Server sends _modifiedAt,",
            expected = "We can get modifiedTime that server sends and createdTime must not be updated"
            )]
        public void Test_0110_SaveAllFields_Update ()
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

            obj.Save();

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{\"modifiedAt\": 3456}");

            obj.SaveAllFields(true);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(3456, obj.ModifedTime);

        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call SaveAllFields() with true(overwrite) for updating and Server sends HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0111_SaveAllFields_Update_server_error ()
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

            obj.Save();

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(new CloudException(400, "{}"));

            obj.SaveAllFields(true);
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call SaveAllFields() with true(overwrite) for updating and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0112_SaveAllFields_Update_broken_json ()
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

            obj.Save();

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{}");

            obj.SaveAllFields(true);
        }

        [Test(), KiiUTInfo(
            action = "When we call SaveAllFields() with false(no overwrite) for updating and Server returns _modifiedAt,",
            expected = "We can get modifiedTime that server sends and createdTime must not be updated"
            )]
        public void Test_0120_Save_Update_No_Overwrite ()
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

            obj.Save();

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{\"modifiedAt\": 3456}");

            obj.SaveAllFields(false);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(3456, obj.ModifedTime);

        }

        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call SaveAllFields() with false(no overwrite) and no etag for updating,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_0121_Save_Update_No_Overwrite_No_Etag ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                "\"objectID\" : \"d8dc9f29-0fb9-48be-a80c-ec60fddedb54\"," +
                "\"createdAt\" : 1337039114613," +
                "\"dataType\" : \"application/vnd.sandobx.mydata+json\"" +
                "}",
                null);

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            obj.Save();

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{\"_created\": 1234,\"_modified\": 3456}");

            obj.SaveAllFields(false);
        }

        #region Delete()

        [Test(), KiiUTInfo(
            action = "When we call Delete() and Server returns HTTP 204(OK),",
            expected = "We can get createdTime = -1/ modifiedTime = -1 / Uri must be null"
            )]
        public void Test_0200_Delete ()
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

            obj.Save();

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);
            // set body content-type
            SDKTestHack.SetField(obj, "mBodyContentType", "text/plain");

            // set Response
            client.AddResponse(204, "");

            obj.Delete();
            Assert.AreEqual(-1, obj.CreatedTime);
            Assert.AreEqual(-1, obj.ModifedTime);
            Assert.AreEqual(null, obj.Uri);
            Assert.IsNull (obj.BodyContentType);
        }

        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call Delete() to KiiObject that doesn't have ID,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_0201_Delete_No_ID ()
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

            obj.Delete();
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call Delete() and Server returns HTTP 404(Not found),",
            expected = "CloudException must be thrown"
            )]
        public void Test_0202_Delete_server_error ()
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

            obj.Save();

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // set Response
            client.AddResponse(new CloudException(404, "{}"));

            obj.Delete();
        }
        #endregion

        #region Refresh()

        [Test(), KiiUTInfo(
            action = "When we call Refresh() and Server returns KiiObject,",
            expected = "We can get ObjectID/createdTime/modifiedTime that server sends"
            )]
        public void Test_0300_Refresh()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardRefreshResponse(client);

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            obj.Refresh();

            Assert.AreEqual("abcd", obj.ID);
            Assert.AreEqual(2345, obj.CreatedTime);
            Assert.AreEqual(6789, obj.ModifedTime);
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call Refresh() and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0301_Refresh_server_error()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new CloudException(400, "{}"));

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            obj.Refresh();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call Refresh() and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0302_Refresh_broken_json()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "{}");

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            obj.Refresh();
        }

        #endregion

        #region GeoPoint

        #endregion
    }
}

