using System;
using NUnit.Framework;
using JsonOrg;

// test spec : https://docs.google.com/a/kii.com/spreadsheet/ccc?key=0AsJL8lP7ZQXGdFFnak9XeWlwSG55MjdxTktFUm5JZWc&usp=drive_web#gid=6
namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiObjectWithNoId
    {

        [Test(), ExpectedException(typeof(InvalidOperationException))]
        public void Test_3_13_CreateWithNoId_InCloud_NoPatch_NotOverwrite_EtagNone ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"objectID\" : \"abcd-1234\", \"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["key"] = "value";
            obj.SaveAllFields(false);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);

            // set etag to null
            SDKTestHack.SetField(obj, "mEtag", null);
            obj.SaveAllFields(false);
        }

        [Test()]
        public void Test_3_14_CreateWithNoId_InCloud_NoPatch_NotOverwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"objectID\" : \"abcd-1234\", \"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["key"] = "value";
            obj.SaveAllFields(false);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            
            // update object
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            obj["key1"] = "value1";
            obj.SaveAllFields(false);
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
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
        public void Test_3_15_CreateWithNoId_InCloud_NoPatch_NotOverwrite_EtagNotMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"objectID\" : \"abcd-1234\", \"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["key"] = "value";
            obj.SaveAllFields(false);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            
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
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
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
        public void Test_3_16_CreateWithNoId_InCloud_NoPatch_Overwrite_EtagNone ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"objectID\" : \"abcd-1234\", \"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["key"] = "value";
            obj.SaveAllFields(false);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);

            // Set Etag to null
            SDKTestHack.SetField(obj,"mEtag", null);
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            // save successful as overwrite is true.
            obj.SaveAllFields(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
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
        public void Test_3_17_CreateWithNoId_InCloud_NoPatch_Overwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"objectID\" : \"abcd-1234\", \"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["key"] = "value";
            obj.SaveAllFields(false);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
          
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            // save successful as overwrite is true.
            obj.SaveAllFields(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
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
        public void Test_3_19_CreateWithNoId_InCloud_Patch_NoOverwrite_EtagNone ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"objectID\" : \"abcd-1234\", \"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["key"] = "value";
            obj.SaveAllFields(false);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            // set etag to null
            SDKTestHack.SetField(obj, "mEtag", null);
            obj.SaveAllFields(false);
            
        }

        [Test()]
        public void Test_3_20_CreateWithNoId_InCloud_Patch_NoOverwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // prepare response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"objectID\" : \"abcd-1234\", \"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
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
        public void Test_3_21_CreateWithNoId_InCloud_Patch_NotOverwrite_EtagNotMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // prepare response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"objectID\" : \"abcd-1234\", \"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
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
        public void Test_3_22_CreateWithNoId_InCloud_Patch_Overwrite_EtagNone ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // prepare response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"objectID\" : \"abcd-1234\", \"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
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
        public void Test_3_23_CreateWithNoId_InCloud_Patch_Overwrite_EtagMatched ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            string objId = "abcd-1234";
            
            // prepare response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"objectID\" : \"abcd-1234\", \"createdAt\" : 1, \"modifiedAt\" : 1}", "1");
            
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
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
