using System;
using NUnit.Framework;
using JsonOrg;

// test spec : https://docs.google.com/a/kii.com/spreadsheet/ccc?key=0AsJL8lP7ZQXGdFFnak9XeWlwSG55MjdxTktFUm5JZWc&usp=drive_web#gid=4
namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiObjectWithUri
    {

        [Test()]
        public void Test_2_1_CreateWithUri_NotInCloud_NoPatch_NotOverwrite_EtagNone ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/"+objId));
            obj["key"] = "value";
            obj.SaveAllFields(false);
            
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", objId);
            Assert.AreEqual(url, client.RequestUrl[0]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[0]);
            MockHttpHeaderList headerList = client.RequestHeader[0];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("*",headerList["If-None-Match"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));

            string reqBody = "{ \"key\" : \"value\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[0]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_2_4_CreateWithUri_NotInCloud_NoPatch_Overwrite_EtagNone ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/"+objId));
            obj["key"] = "value";
            obj.SaveAllFields(true);
            
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", objId);
            Assert.AreEqual(url, client.RequestUrl[0]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[0]);
            MockHttpHeaderList headerList = client.RequestHeader[0];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            
            string reqBody = "{ \"key\" : \"value\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[0]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test(),  ExpectedException(typeof(InvalidOperationException))]
        public void Test_2_7_CreateWithUri_NotInCloud_Patch_NotOverwrite_EtagNone ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/"+objId));
            obj["key"] = "value";
            obj.Save(false);
        }

        [Test()]
        public void Test_2_10_CreateWithUri_notInCloud_Patch_overwrite_etagNone ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // set response
            MockHttpClient client = factory.Client;
            string mockResponseBody =  "{" +
                    "\"errorCode\" : \"OBJECT_NOT_FOUND\"," +
                    "\"message\" : \"The object with specified id not found\" " +
                    "}";
            client.AddResponse(new CloudException(404, mockResponseBody));

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/"+objId));
            obj["key"] = "value";
            CloudException exp = null;
            try {
                obj.Save(true);
                Assert.Fail("Exception not thrown");
            } catch (CloudException e) {
                exp = e;
            }
            Assert.IsNotNull(exp);
            Assert.AreEqual(404, exp.Status);
            Assert.AreEqual(mockResponseBody, exp.Body);

            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", objId);
            Assert.AreEqual(url, client.RequestUrl[0]);
            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod[0]);
            MockHttpHeaderList headerList = client.RequestHeader[0];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            
            string reqBody = "{ \"key\" : \"value\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[0]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_2_13_CreateWithUri_InCloud_NoPatch_NotOverwrite_EtagNone ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // set response
            MockHttpClient client = factory.Client;
            string mockResponseBody =  "{" +
                "\"errorCode\" : \"OBJECT_ALREADY_EXISTS\"," +
                    "\"message\" : \"The object with specified id already exists\" " +
                    "}";
            client.AddResponse(new CloudException(404, mockResponseBody));
            
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/"+objId));
            obj["key"] = "value";
            CloudException exp = null;
            try {
                obj.SaveAllFields(false);
                Assert.Fail("Exception not thrown");
            } catch (CloudException e) {
                exp = e;
            }
            Assert.IsNotNull(exp);
            Assert.AreEqual(404, exp.Status);
            Assert.AreEqual(mockResponseBody, exp.Body);
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", objId);
            Assert.AreEqual(url, client.RequestUrl[0]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[0]);
            MockHttpHeaderList headerList = client.RequestHeader[0];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("*",headerList["If-None-Match"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            
            string reqBody = "{ \"key\" : \"value\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[0]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        
        [Test()]
        public void Test_2_14_CreateWithUri_InCloud_NoPatch_NotOverwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // save object to cloud
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/"+objId));
            obj["key"] = "value";
            obj.SaveAllFields(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            string etag = (string)SDKTestHack.GetField(obj,"mEtag");
            Assert.AreEqual("1", etag);

            // update object
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            obj["key1"] = "value1";
            obj.SaveAllFields(false);
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", objId);
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId", headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey", headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            Assert.AreEqual("1", headerList["If-Match"] );
            
            string reqBody = "{ \"key\" : \"value\", \"key1\" : \"value1\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_2_15_CreateWithUri_InCloud_NoPatch_NotOverwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // save object to cloud
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/"+objId));
            obj["key"] = "value";
            obj.SaveAllFields(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            string etag = (string)SDKTestHack.GetField(obj,"mEtag");
            Assert.AreEqual("1", etag);
            
            // server send error response (assume object already updated in server side)
            string mockResponseBody = "{\"errorCode\" : \"OBJECT_VERSION_IN_STALE\"}";
            client.AddResponse(new CloudException(409, mockResponseBody));
            obj["key1"] = "value1";
            CloudException exp = null;
            try {
                obj.SaveAllFields(false);
                Assert.Fail("Exception not thrown");
            } catch (CloudException e) {
                exp = e;
            }
            Assert.IsNotNull(exp);
            Assert.AreEqual(409, exp.Status);
            Assert.AreEqual(mockResponseBody, exp.Body);

            
            // request contains if-match
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", objId);
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId", headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey", headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            Assert.AreEqual("1", headerList["If-Match"] );
            
            string reqBody = "{ \"key\" : \"value\", \"key1\" : \"value1\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_2_16_CreateWithUri_InCloud_NoPatch_Overwrite_EtagNone ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // prepare response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");

            // save object to cloud
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/"+objId));
            obj["key"] = "value";
            obj.SaveAllFields(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            string etag = (string)SDKTestHack.GetField(obj,"mEtag");
            Assert.AreEqual("1", etag);

            // Set Etag to null
            SDKTestHack.GetField(obj,"mEtag");
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");

            // save successful as overwrite is true.
            obj.SaveAllFields(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", objId);
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId", headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey", headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            string reqBody = "{ \"key\" : \"value\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_2_17_CreateWithUri_InCloud_NoPatch_Overwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // prepare response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            // save object to cloud
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/"+objId));
            obj["key"] = "value";
            obj.SaveAllFields(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            string etag = (string)SDKTestHack.GetField(obj,"mEtag");
            Assert.AreEqual("1", etag);
            
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}");
            
            // save successful as overwrite is true.
            obj.SaveAllFields(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", objId);
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId", headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey", headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            string reqBody = "{ \"key\" : \"value\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test(), ExpectedException(typeof(InvalidOperationException))]
        public void Test_2_19_CreateWithUri_InCloud_NoPatch_NoOverwrite_EtagNone ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // prepare response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            // save object to cloud
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/"+objId));
            obj["key"] = "value";
            obj.SaveAllFields(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            string etag = (string)SDKTestHack.GetField(obj,"mEtag");
            Assert.AreEqual("1", etag);

            // set etag to null
            SDKTestHack.SetField(obj, "mEtag", null);
            obj.SaveAllFields(false);

        }

        [Test()]
        public void Test_2_20_CreateWithUri_InCloud_Patch_NoOverwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // prepare response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            // save object to cloud
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/"+objId));
            obj["key"] = "value";
            obj.SaveAllFields(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            string etag = (string)SDKTestHack.GetField(obj,"mEtag");
            Assert.AreEqual("1", etag);


            client.AddResponse(201, "{\"_created\" : 1, \"_modified\" : 1}");

            obj["key1"] = "value1";
            obj.Save(false);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            
            // request contains x-http-method-override, if-match header
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", objId);
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId", headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey", headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            Assert.AreEqual("PATCH", headerList["X-HTTP-Method-Override"] );
            Assert.AreEqual("1", headerList["If-Match"] );
            string reqBody = "{\"key1\" : \"value1\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_2_21_CreateWithUri_InCloud_Patch_NotOverwrite_EtagNotMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // prepare response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            // save object to cloud
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/"+objId));
            obj["key"] = "value";
            obj.SaveAllFields(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            string etag = (string)SDKTestHack.GetField(obj,"mEtag");
            Assert.AreEqual("1", etag);
            
            
            string mockResponseBody =  "{" +
                "\"errorCode\" : \"OBJECT_VERSION_IN_STALE\"," +
                    "\"message\" : \"object version did not matched\" " +
                    "}";
            client.AddResponse(new CloudException(409, mockResponseBody));
            

            obj["key1"] = "value1";
            CloudException exp = null;
            try {
                obj.Save(false);
                Assert.Fail("Exception not thrown");
            } catch (CloudException e) {
                exp = e;
            }
            Assert.IsNotNull(exp);
            Assert.AreEqual(409, exp.Status);
            Assert.AreEqual(mockResponseBody, exp.Body);
            
            // request contains x-http-method-override, if-match header
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", objId);
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId", headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey", headerList["X-Kii-AppKey"] );
            Assert.AreEqual("PATCH", headerList["X-HTTP-Method-Override"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            Assert.AreEqual("1", headerList["If-Match"] );
            string reqBody = "{\"key1\" : \"value1\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_2_22_CreateWithUri_InCloud_Patch_Overwrite_EtagNone ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // prepare response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            // save object to cloud
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/"+objId));
            obj["key"] = "value";
            obj.SaveAllFields(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            string etag = (string)SDKTestHack.GetField(obj,"mEtag");
            Assert.AreEqual("1", etag);
            
            // set etag to null
            SDKTestHack.SetField(obj,"mEtag", null);

            client.AddResponse(201, "{\"_created\" : 1, \"_modified\" : 1}");
            obj["key1"] = "value1";

            // object save successfully as Overwrite is true.
            obj.Save(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            
            // request contains x-http-method-override
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", objId);
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId", headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey", headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            Assert.AreEqual("PATCH", headerList["X-HTTP-Method-Override"] );
            string reqBody = "{\"key1\" : \"value1\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_2_23_CreateWithUri_InCloud_Patch_Overwrite_EtagMatched ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // prepare response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            // save object to cloud
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/test/objects/"+objId));
            obj["key"] = "value";
            obj.SaveAllFields(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            string etag = (string)SDKTestHack.GetField(obj,"mEtag");
            Assert.AreEqual("1", etag);
            
            client.AddResponse(201, "{\"_created\" : 1, \"_modified\" : 1}");
            obj["key1"] = "value1";
            
            // object save successfully as Overwrite is true.
            obj.Save(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            
            // request contains x-http-method-override
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", objId);
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId", headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey", headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            Assert.AreEqual("PATCH", headerList["X-HTTP-Method-Override"] );
            string reqBody = "{\"key1\" : \"value1\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

    }
}
