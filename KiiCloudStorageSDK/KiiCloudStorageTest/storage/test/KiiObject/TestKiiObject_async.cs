using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiObject_async
    {

        private MockHttpClient client;
        
        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            
            // This test is Unit test so we use blocking mock client
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            client = factory.Client;
            
        }

        private void SetStandardSaveResponse()
        {
            client.AddResponse(201, "{" +
                "\"objectID\" : \"d8dc9f29-0fb9-48be-a80c-ec60fddedb54\"," +
                "\"createdAt\" : 1337039114613," +
                "\"dataType\" : \"application/vnd.sandobx.mydata+json\"" +
                "}",
                "1");
        }

        private void SetStandardRefreshResponse()
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

        #region KiiObject.Save(KiiObjectCallback)

        [Test(), KiiUTInfo(
            action = "When we call Save(callback) and Server returns objectID/createdAt,",
            expected = "We can get ObjectID/CreatedTime/Modifiedtime that server sends"
            )]
        public void Test_0000_Save ()
        {

            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });

            Assert.IsTrue(done);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", outObj.ID);
            Assert.AreEqual(1337039114613, outObj.CreatedTime);
            Assert.AreEqual(1337039114613, outObj.ModifedTime);

        }

        [Test(), KiiUTInfo(
            action = "When we call Save(callback) and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0001_Save_server_error ()
        {
            // set response
            client.AddResponse(new CloudException(400, "{}"));

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is CloudException);
            Assert.AreEqual(400, (exception as CloudException).Status);
                                  
        }

        [Test(), KiiUTInfo(
            action = "When we call Save(callback) and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0002_Save_broken_json ()
        {
            // set response
            client.AddResponse(200, "{}");

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is IllegalKiiBaseObjectFormatException);

        }

        [Test(), KiiUTInfo(
            action = "When we call Save(callback) and Server returns objectID/createdAt,",
            expected = "We can get ObjectID/CreatedTime/Modifiedtime that server sends"
            )]
        public void Test_0003_Save_2times ()
        {
            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });

            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", outObj.ID);
            Assert.AreEqual(1337039114613, outObj.CreatedTime);
            Assert.AreEqual(1337039114613, outObj.ModifedTime);

            // clear stub
            client.RequestBody.Clear();
            // set Response
            client.AddResponse(200, "{\"_created\": 1234,\"_modified\": 3456}");

            obj["score"] = 80;

            done = false;
            outObj = null;
            exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("{\"score\":80}", client.RequestBody[0]);

        }

        [Test(), KiiUTInfo(
            action = "When we call Save(callback) for updating and Server returns _created/_modified,",
            expected = "We can get createdTime/modifiedTime that server sends"
            )]
        public void Test_0010_Save_Update ()
        {
            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", outObj.ID);
            Assert.AreEqual(1337039114613, outObj.CreatedTime);
            Assert.AreEqual(1337039114613, outObj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{\"_created\": 1234,\"_modified\": 3456}");

            done = false;
            outObj = null;
            exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
                     {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual(1234, obj.CreatedTime);
            Assert.AreEqual(3456, obj.ModifedTime);

        }

        [Test(), KiiUTInfo(
            action = "When we call Save(callback) for updating and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0011_Save_Update_server_error ()
        {
            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", outObj.ID);
            Assert.AreEqual(1337039114613, outObj.CreatedTime);
            Assert.AreEqual(1337039114613, outObj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(new CloudException(400, "{}"));

            done = false;
            outObj = null;
            exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is CloudException);

        }

        [Test(), KiiUTInfo(
            action = "When we call Save(callback) for updating and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0012_Save_Update_broken_json ()
        {
            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", outObj.ID);
            Assert.AreEqual(1337039114613, outObj.CreatedTime);
            Assert.AreEqual(1337039114613, outObj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{}");

            done = false;
            outObj = null;
            exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is IllegalKiiBaseObjectFormatException);
        }

        [Test(), KiiUTInfo(
            action = "When we call Save(callback) with false(no overwrite) and Server returns _created/_modified,",
            expected = "We can get createdTime/modifiedTime that server sends"
            )]
        public void Test_0020_Save_Update_No_Overwrite ()
        {
            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", outObj.ID);
            Assert.AreEqual(1337039114613, outObj.CreatedTime);
            Assert.AreEqual(1337039114613, outObj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{\"_created\": 1234,\"_modified\": 3456}");

            done = false;
            outObj = null;
            exception = null;
            obj.Save(false, (KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual(1234, outObj.CreatedTime);
            Assert.AreEqual(3456, outObj.ModifedTime);

        }

        [Test(), KiiUTInfo(
            action = "When we call Save() with false(no overwrite) and no etag,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_0021_Save_Update_No_Overwrite_No_Etag ()
        {
            // set response
            client.AddResponse(201, "{" +
                "\"objectID\" : \"d8dc9f29-0fb9-48be-a80c-ec60fddedb54\"," +
                "\"createdAt\" : 1337039114613," +
                "\"dataType\" : \"application/vnd.sandobx.mydata+json\"" +
                "}",
                null);

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", outObj.ID);
            Assert.AreEqual(1337039114613, outObj.CreatedTime);
            Assert.AreEqual(1337039114613, outObj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{\"_created\": 1234,\"_modified\": 3456}");

            done = false;
            outObj = null;
            exception = null;
            obj.Save(false, (KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is InvalidOperationException);
        }

        #endregion

        [Test(), KiiUTInfo(
            action = "When we call SaveAllFields() with true(overwrite) and Server sends ObjectId/createdAt,",
            expected = "We can get ObjectID/createdTime/modifiedTime that server sends"
            )]
        public void Test_0100_SaveAllField ()
        {
            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.SaveAllFields(true, (KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", outObj.ID);
            Assert.AreEqual(1337039114613, outObj.CreatedTime);
            Assert.AreEqual(1337039114613, outObj.ModifedTime);

        }

        [Test(), KiiUTInfo(
            action = "When we call SaveAllFields(callback) with false(no overwrite) and Server sends ObjectId/createdAt,",
            expected = "We can get ObjectID/createdTime/modifiedTime that server sends"
            )]
        public void Test_0101_SaveAllField_false ()
        {
            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.SaveAllFields(false, (KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", outObj.ID);
            Assert.AreEqual(1337039114613, outObj.CreatedTime);
            Assert.AreEqual(1337039114613, outObj.ModifedTime);

        }

        [Test(), KiiUTInfo(
            action = "When we call SaveAllFields(callback) with true(overwrite) for updating and Server sends _modifiedAt,",
            expected = "We can get modifiedTime that server sends and createdTime must not be updated"
            )]
        public void Test_0110_SaveAllFields_Update ()
        {
            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", outObj.ID);
            Assert.AreEqual(1337039114613, outObj.CreatedTime);
            Assert.AreEqual(1337039114613, outObj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{\"modifiedAt\": 3456}");

            done = false;
            outObj = null;
            exception = null;
            obj.SaveAllFields(true, (KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual(1337039114613, outObj.CreatedTime);
            Assert.AreEqual(3456, outObj.ModifedTime);

        }

        [Test(), KiiUTInfo(
            action = "When we call SaveAllFields(callback) with true(overwrite) for updating and Server sends HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0111_SaveAllFields_Update_server_error ()
        {
            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", outObj.ID);
            Assert.AreEqual(1337039114613, outObj.CreatedTime);
            Assert.AreEqual(1337039114613, outObj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(new CloudException(400, "{}"));

            done = false;
            outObj = null;
            exception = null;
            obj.SaveAllFields(true, (KiiObject createdObj, Exception e) =>
                              {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is CloudException);
            
        }

        [Test(), KiiUTInfo(
            action = "When we call SaveAllFields() with true(overwrite) for updating and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0112_SaveAllFields_Update_broken_json ()
        {
            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{}");

            done = false;
            outObj = null;
            exception = null;
            obj.SaveAllFields(true, (KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is IllegalKiiBaseObjectFormatException);
        }

        [Test(), KiiUTInfo(
            action = "When we call SaveAllFields(callback) with false(no overwrite) for updating and Server returns _modifiedAt,",
            expected = "We can get modifiedTime that server sends and createdTime must not be updated"
            )]
        public void Test_0120_Save_Update_No_Overwrite ()
        {
            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{\"modifiedAt\": 3456}");

            done = false;
            outObj = null;
            exception = null;
            obj.SaveAllFields(false, (KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual(1337039114613, outObj.CreatedTime);
            Assert.AreEqual(3456, outObj.ModifedTime);

        }

        [Test(), KiiUTInfo(
            action = "When we call SaveAllFields(callback) with false(no overwrite) and no etag for updating,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_0121_Save_Update_No_Overwrite_No_Etag ()
        {
            // set response
            client.AddResponse(201, "{" +
                "\"objectID\" : \"d8dc9f29-0fb9-48be-a80c-ec60fddedb54\"," +
                "\"createdAt\" : 1337039114613," +
                "\"dataType\" : \"application/vnd.sandobx.mydata+json\"" +
                "}",
                null);

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // update
            obj["age"] = 19;

            // set Response
            client.AddResponse(200, "{\"_created\": 1234,\"_modified\": 3456}");

            done = false;
            outObj = null;
            exception = null;
            obj.SaveAllFields(false, (KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is InvalidOperationException);
        }

        #region Delete()

        [Test(), KiiUTInfo(
            action = "When we call Delete() and Server returns HTTP 204(OK),",
            expected = "We can get createdTime = -1/ modifiedTime = -1 / Uri must be null"
            )]
        public void Test_0200_Delete ()
        {
            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // set Response
            client.AddResponse(204, "");

            done = false;
            outObj = null;
            exception = null;
            obj.Delete((KiiObject deletedObj, Exception e) =>
            {
                done = true;
                outObj = deletedObj;
                exception = e;
            });
            Console.WriteLine(exception);
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual(-1, outObj.CreatedTime);
            Assert.AreEqual(-1, outObj.ModifedTime);
            Assert.AreEqual(null, outObj.Uri);
        }

        [Test(), KiiUTInfo(
            action = "When we call Delete(callback) to KiiObject that doesn't have ID,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_0201_Delete_No_ID ()
        {
            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Delete((KiiObject deletedObj, Exception e) =>
            {
                done = true;
                outObj = deletedObj;
                exception = e;
            });

            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is InvalidOperationException);
        }

        [Test(), KiiUTInfo(
            action = "When we call Delete(callback) and Server returns HTTP 404(Not found),",
            expected = "CloudException must be thrown"
            )]
        public void Test_0202_Delete_server_error ()
        {
            // set response
            this.SetStandardSaveResponse();

            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Save((KiiObject createdObj, Exception e) =>
            {
                done = true;
                outObj = createdObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // set Response
            client.AddResponse(new CloudException(404, "{}"));

            done = false;
            outObj = null;
            exception = null;
            obj.Delete((KiiObject deletedObj, Exception e) =>
            {
                done = true;
                outObj = deletedObj;
                exception = e;
            });
            
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is CloudException);
        }
        #endregion

        #region Refresh()

        [Test(), KiiUTInfo(
            action = "When we call Refresh(callback) and Server returns KiiObject,",
            expected = "We can get ObjectID/createdTime/modifiedTime that server sends"
            )]
        public void Test_0300_Refresh()
        {
            // set response
            this.SetStandardRefreshResponse();

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Refresh((KiiObject refreshedObj, Exception e) =>
            {
                done = true;
                outObj = refreshedObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNull(exception);

            Assert.AreEqual("abcd", outObj.ID);
            Assert.AreEqual(2345, outObj.CreatedTime);
            Assert.AreEqual(6789, outObj.ModifedTime);
        }

        [Test(), KiiUTInfo(
            action = "When we call Refresh(callback) and Server returns HTTP 400,",
            expected = "CloudException must be thrown"
            )]
        public void Test_0301_Refresh_server_error()
        {
            // set response
            client.AddResponse(new CloudException(400, "{}"));

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Refresh((KiiObject refreshedObj, Exception e) =>
            {
                done = true;
                outObj = refreshedObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is CloudException);
        }

        [Test(), KiiUTInfo(
            action = "When we call Refresh(callback) and Server returns broken Json,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0302_Refresh_broken_json()
        {
            // set response
            client.AddResponse(200, "{}");

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/abcd"));
            bool done = false;
            KiiObject outObj = null;
            Exception exception = null;
            obj.Refresh((KiiObject refreshedObj, Exception e) =>
            {
                done = true;
                outObj = refreshedObj;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outObj);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is IllegalKiiBaseObjectFormatException);
        }

        #endregion

        #region GeoPoint

        #endregion
    }
}

