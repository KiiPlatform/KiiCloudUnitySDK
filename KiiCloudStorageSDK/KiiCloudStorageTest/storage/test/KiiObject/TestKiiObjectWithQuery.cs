using System;
using NUnit.Framework;
using JsonOrg;

// test spec : https://docs.google.com/a/kii.com/spreadsheet/ccc?key=0AsJL8lP7ZQXGdFFnak9XeWlwSG55MjdxTktFUm5JZWc&usp=drive_web#gid=8
namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiObjectWithQuery
    {
        [Test()]
        public void Test_4_2_CreateWithQuery_NotInCloud_NoPatch_NotOverwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                "\"results\" : [" +
                    "{" +
                        "\"_created\" : 1," +
                        "\"_modified\" : 1," +
                        "\"key\" : \"value\"," +
                        "\"_id\" : \"abcd-1234\"," +
                        "\"_version\" : \"1\" " +
                    "}]" +
                "}");
            
            KiiQueryResult<KiiObject> result = Kii.Bucket("test").Query(null);
            Assert.AreEqual(1, result.Count);
            KiiObject obj = result[0];


            string mockResponseBody =  "{" +
                "\"errorCode\" : \"OBJECT_NOT_FOUND\"," +
                    "\"message\" : \"The object with specified id not found\" " +
                    "}";
            client.AddResponse(new CloudException(404, mockResponseBody));
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
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("1",headerList["If-Match"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            
            string reqBody = "{" +
                    "\"_created\" : 1," +
                    "\"_modified\" : 1," +
                    "\"key\" : \"value\"," +
                    "\"_id\" : \"abcd-1234\"," +
                    "\"_version\" : \"1\" " +
                    "}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_4_5_CreateWithQuery_NotInCloud_NoPatch_Overwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                               "\"results\" : [" +
                               "{" +
                               "\"_created\" : 1," +
                               "\"_modified\" : 1," +
                               "\"key\" : \"value\"," +
                               "\"_id\" : \"abcd-1234\"," +
                               "\"_version\" : \"1\" " +
                               "}]" +
                               "}");
            
            KiiQueryResult<KiiObject> result = Kii.Bucket("test").Query(null);
            Assert.AreEqual(1, result.Count);
            KiiObject obj = result[0];
            
            
            string mockResponseBody =  "{" +
                "\"errorCode\" : \"OBJECT_NOT_FOUND\"," +
                    "\"message\" : \"The object with specified id not found\" " +
                    "}";
            client.AddResponse(new CloudException(404, mockResponseBody));
            CloudException exp = null;
            try {
                obj.SaveAllFields(true);
                Assert.Fail("Exception not thrown");
            } catch (CloudException e) {
                exp = e;
            }
            Assert.IsNotNull(exp);
            Assert.AreEqual(404, exp.Status);
            Assert.AreEqual(mockResponseBody, exp.Body);
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            
            string reqBody = "{" +
                "\"_created\" : 1," +
                    "\"_modified\" : 1," +
                    "\"key\" : \"value\"," +
                    "\"_id\" : \"abcd-1234\"," +
                    "\"_version\" : \"1\" " +
                    "}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_4_8_CreateWithQuery_NotInCloud_Patch_NotOverwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                               "\"results\" : [" +
                               "{" +
                               "\"_created\" : 1," +
                               "\"_modified\" : 1," +
                               "\"key\" : \"value\"," +
                               "\"_id\" : \"abcd-1234\"," +
                               "\"_version\" : \"1\" " +
                               "}]" +
                               "}");
            
            KiiQueryResult<KiiObject> result = Kii.Bucket("test").Query(null);
            Assert.AreEqual(1, result.Count);
            KiiObject obj = result[0];
            
            
            string mockResponseBody =  "{" +
                "\"errorCode\" : \"OBJECT_NOT_FOUND\"," +
                "\"message\" : \"The object with specified id not found\" " +
            "}";
            client.AddResponse(new CloudException(404, mockResponseBody));
            CloudException exp = null;
            try {
                obj["key1"] = "value1";
                obj.Save(false);
                Assert.Fail("Exception not thrown");
            } catch (CloudException e) {
                exp = e;
            }
            Assert.IsNotNull(exp);
            Assert.AreEqual(404, exp.Status);
            Assert.AreEqual(mockResponseBody, exp.Body);
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("1",headerList["If-Match"] );
            Assert.AreEqual("PATCH",headerList["X-HTTP-Method-Override"]);
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            
            string reqBody = "{\"key1\" : \"value1\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_4_11_CreateWithQuery_NotInCloud_Patch_Overwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                               "\"results\" : [" +
                               "{" +
                               "\"_created\" : 1," +
                               "\"_modified\" : 1," +
                               "\"key\" : \"value\"," +
                               "\"_id\" : \"abcd-1234\"," +
                               "\"_version\" : \"1\" " +
                               "}]" +
                               "}");
            
            KiiQueryResult<KiiObject> result = Kii.Bucket("test").Query(null);
            Assert.AreEqual(1, result.Count);
            KiiObject obj = result[0];
            
            
            string mockResponseBody =  "{" +
                "\"errorCode\" : \"OBJECT_NOT_FOUND\"," +
                    "\"message\" : \"The object with specified id not found\" " +
                    "}";
            client.AddResponse(new CloudException(404, mockResponseBody));
            CloudException exp = null;
            try {
                obj["key1"] = "value1";
                obj.Save(true);
                Assert.Fail("Exception not thrown");
            } catch (CloudException e) {
                exp = e;
            }
            Assert.IsNotNull(exp);
            Assert.AreEqual(404, exp.Status);
            Assert.AreEqual(mockResponseBody, exp.Body);
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("PATCH",headerList["X-HTTP-Method-Override"]);
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            
            string reqBody = "{\"key1\" : \"value1\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_4_14_CreateWithQuery_InCloud_NoPatch_NotOverwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                               "\"results\" : [" +
                               "{" +
                               "\"_created\" : 1," +
                               "\"_modified\" : 1," +
                               "\"key\" : \"value\"," +
                               "\"_id\" : \"abcd-1234\"," +
                               "\"_version\" : \"1\" " +
                               "}]" +
                               "}");
            
            KiiQueryResult<KiiObject> result = Kii.Bucket("test").Query(null);
            Assert.AreEqual(1, result.Count);
            KiiObject obj = result[0];

            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1, \"_id\" : \"abcd-1234\"}", "1");
            obj["key1"] = "value1";
            obj.SaveAllFields(false);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            string etag = (string)SDKTestHack.GetField(obj,"mEtag");
            Assert.AreEqual("1", etag);
            

            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("1",headerList["If-Match"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            
            string reqBody = "{" +
                "\"_created\" : 1," +
                    "\"_modified\" : 1," +
                    "\"key\" : \"value\"," +
                    "\"key1\" : \"value1\"," +
                    "\"_id\" : \"abcd-1234\"," +
                    "\"_version\" : \"1\" " +
                    "}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_4_15_CreateWithQuery_InCloud_NoPatch_NotOverwrite_EtagNotMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                               "\"results\" : [" +
                               "{" +
                               "\"_created\" : 1," +
                               "\"_modified\" : 1," +
                               "\"key\" : \"value\"," +
                               "\"_id\" : \"abcd-1234\"," +
                               "\"_version\" : \"1\" " +
                               "}]" +
                               "}");
            
            KiiQueryResult<KiiObject> result = Kii.Bucket("test").Query(null);
            Assert.AreEqual(1, result.Count);
            KiiObject obj = result[0];
            
            string mockResponseBody = "{\"errorCode\" : \"OBJECT_VERSION_IN_STALE\"}";
            client.AddResponse(new CloudException(409, mockResponseBody));
            obj["key1"] = "value1";
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
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("1",headerList["If-Match"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            
            string reqBody = "{" +
                "\"_created\" : 1," +
                    "\"_modified\" : 1," +
                    "\"key\" : \"value\"," +
                    "\"key1\" : \"value1\"," +
                    "\"_id\" : \"abcd-1234\"," +
                    "\"_version\" : \"1\" " +
                    "}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_4_17_CreateWithQuery_InCloud_NoPatch_Overwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                               "\"results\" : [" +
                               "{" +
                               "\"_created\" : 1," +
                               "\"_modified\" : 1," +
                               "\"key\" : \"value\"," +
                               "\"_id\" : \"abcd-1234\"," +
                               "\"_version\" : \"1\" " +
                               "}]" +
                               "}");
            
            KiiQueryResult<KiiObject> result = Kii.Bucket("test").Query(null);
            Assert.AreEqual(1, result.Count);
            KiiObject obj = result[0];
            
            client.AddResponse(201, "{\"createdAt\" : 1, \"modifiedAt\" : 1, \"_id\" : \"abcd-1234\"}", "1");
            obj["key1"] = "value1";
            obj.SaveAllFields(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            string etag = (string)SDKTestHack.GetField(obj,"mEtag");
            Assert.AreEqual("1", etag);
            
            
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            
            string reqBody = "{" +
                "\"_created\" : 1," +
                    "\"_modified\" : 1," +
                    "\"key\" : \"value\"," +
                    "\"key1\" : \"value1\"," +
                    "\"_id\" : \"abcd-1234\"," +
                    "\"_version\" : \"1\" " +
                    "}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_4_18_CreateWithQuery_InCloud_NoPatch_Overwrite_EtagNotMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                               "\"results\" : [" +
                               "{" +
                               "\"_created\" : 1," +
                               "\"_modified\" : 1," +
                               "\"key\" : \"value\"," +
                               "\"_id\" : \"abcd-1234\"," +
                               "\"_version\" : \"1\" " +
                               "}]" +
                               "}");
            
            KiiQueryResult<KiiObject> result = Kii.Bucket("test").Query(null);
            Assert.AreEqual(1, result.Count);
            KiiObject obj = result[0];
            
            string mockResponseBody = "{\"errorCode\" : \"OBJECT_VERSION_IN_STALE\"}";
            client.AddResponse(new CloudException(409, mockResponseBody));
            obj["key1"] = "value1";
            obj["key1"] = "value1";
            CloudException exp = null;
            try {
                obj.SaveAllFields(true);
                Assert.Fail("Exception not thrown");
            } catch (CloudException e) {
                exp = e;
            }
            Assert.IsNotNull(exp);
            Assert.AreEqual(409, exp.Status);
            Assert.AreEqual(mockResponseBody, exp.Body);
            
            
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.PUT, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            
            string reqBody = "{" +
                "\"_created\" : 1," +
                    "\"_modified\" : 1," +
                    "\"key\" : \"value\"," +
                    "\"key1\" : \"value1\"," +
                    "\"_id\" : \"abcd-1234\"," +
                    "\"_version\" : \"1\" " +
                    "}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_4_20_CreateWithQuery_InCloud_Patch_NotOverwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                               "\"results\" : [" +
                               "{" +
                               "\"_created\" : 1," +
                               "\"_modified\" : 1," +
                               "\"key\" : \"value\"," +
                               "\"_id\" : \"abcd-1234\"," +
                               "\"_version\" : \"1\" " +
                               "}]" +
                               "}");
            
            KiiQueryResult<KiiObject> result = Kii.Bucket("test").Query(null);
            Assert.AreEqual(1, result.Count);
            KiiObject obj = result[0];
            
            client.AddResponse(201, "{\"_created\" : 1, \"_modified\" : 1, \"_id\" : \"abcd-1234\"}", "1");
            obj["key1"] = "value1";
            obj.Save(false);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            string etag = (string)SDKTestHack.GetField(obj,"mEtag");
            Assert.AreEqual("1", etag);

            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("1",headerList["If-Match"] );
            Assert.AreEqual("PATCH",headerList["X-HTTP-Method-Override"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));Assert.AreEqual("1",headerList["If-Match"] );
            
            string reqBody = "{\"key1\" : \"value1\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_4_21_CreateWithQuery_InCloud_Patch_NotOverwrite_EtagNotMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                               "\"results\" : [" +
                               "{" +
                               "\"_created\" : 1," +
                               "\"_modified\" : 1," +
                               "\"key\" : \"value\"," +
                               "\"_id\" : \"abcd-1234\"," +
                               "\"_version\" : \"1\" " +
                               "}]" +
                               "}");
            
            KiiQueryResult<KiiObject> result = Kii.Bucket("test").Query(null);
            Assert.AreEqual(1, result.Count);
            KiiObject obj = result[0];

            string mockResponseBody = "{\"errorCode\" : \"OBJECT_VERSION_IN_STALE\"}";
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
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("1",headerList["If-Match"] );
            Assert.AreEqual("PATCH",headerList["X-HTTP-Method-Override"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));Assert.AreEqual("1",headerList["If-Match"] );
            
            string reqBody = "{\"key1\" : \"value1\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_4_23_CreateWithQuery_InCloud_Patch_Overwrite_EtagMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                               "\"results\" : [" +
                               "{" +
                               "\"_created\" : 1," +
                               "\"_modified\" : 1," +
                               "\"key\" : \"value\"," +
                               "\"_id\" : \"abcd-1234\"," +
                               "\"_version\" : \"1\" " +
                               "}]" +
                               "}");
            
            KiiQueryResult<KiiObject> result = Kii.Bucket("test").Query(null);
            Assert.AreEqual(1, result.Count);
            KiiObject obj = result[0];
            
            client.AddResponse(201, "{\"_created\" : 1, \"_modified\" : 1, \"_id\" : \"abcd-1234\"}", "1");
            obj["key1"] = "value1";
            obj.Save(true);
            Assert.AreEqual("abcd-1234", obj.ID);
            Assert.AreEqual(1, obj.CreatedTime);
            Assert.AreEqual(1, obj.ModifedTime);
            string etag = (string)SDKTestHack.GetField(obj,"mEtag");
            Assert.AreEqual("1", etag);
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("PATCH",headerList["X-HTTP-Method-Override"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            
            string reqBody = "{\"key1\" : \"value1\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }

        [Test()]
        public void Test_4_24_CreateWithQuery_InCloud_Patch_Overwrite_EtagNotMatch ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                               "\"results\" : [" +
                               "{" +
                               "\"_created\" : 1," +
                               "\"_modified\" : 1," +
                               "\"key\" : \"value\"," +
                               "\"_id\" : \"abcd-1234\"," +
                               "\"_version\" : \"1\" " +
                               "}]" +
                               "}");
            
            KiiQueryResult<KiiObject> result = Kii.Bucket("test").Query(null);
            Assert.AreEqual(1, result.Count);
            KiiObject obj = result[0];
            
            string mockResponseBody = "{\"errorCode\" : \"OBJECT_VERSION_IN_STALE\"}";
            client.AddResponse(new CloudException(409, mockResponseBody));
            obj["key1"] = "value1";
            CloudException exp = null;
            try {
                obj.Save(true);
                Assert.Fail("Exception not thrown");
            } catch (CloudException e) {
                exp = e;
            }
            Assert.IsNotNull(exp);
            Assert.AreEqual(409, exp.Status);
            Assert.AreEqual(mockResponseBody, exp.Body);
            
            // check request
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","buckets","test", "objects", "abcd-1234");
            Assert.AreEqual(url, client.RequestUrl[1]);
            Assert.AreEqual(KiiHttpMethod.POST, client.RequestMethod[1]);
            MockHttpHeaderList headerList = client.RequestHeader[1];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.AreEqual("PATCH",headerList["X-HTTP-Method-Override"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
            
            string reqBody = "{\"key1\" : \"value1\"}";
            JsonObject expectedBodyJson = new JsonObject(reqBody);
            JsonObject actualBodyJson = new JsonObject(client.RequestBody[1]);
            KiiAssertion.AssertJson(expectedBodyJson, actualBodyJson);
        }
    }
}
