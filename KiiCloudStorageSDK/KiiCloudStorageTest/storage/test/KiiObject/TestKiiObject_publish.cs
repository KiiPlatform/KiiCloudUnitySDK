using System;
using System.Collections.Generic;

using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiObject_publish
    {
        private void SetStandardResponse(MockHttpClient client)
        {
            client.AddResponse(200, "{" +
                "\"publicationID\" : \"d8dc9f29-0fb9-48be-a80c-ec60fddedb54\"," +
                "\"url\" : \"http://api-jp.kii.com/a/1234\"" +
                "}",
                "1");
        }

        #region PublishBody()
        [Test(), KiiUTInfo(
            action = "When we call Publish() and Server returns publicationID/url,",
            expected = "We can get the URL for this body"
            )]
        public void Test_0000_Publish ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            string url = obj.PublishBody();

            Assert.AreEqual("http://api-jp.kii.com/a/1234", url);
            IList<string> body = client.RequestBody;
            Assert.AreEqual(null, body[0]);
        }

        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call Publish() with not uploaded KiiObject,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_0010_Publish_not_uploaded ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = Kii.Bucket("images").NewKiiObject();
            obj.PublishBody();          
        }

        [Test(), ExpectedException(typeof(NotFoundException)), KiiUTInfo(
            action = "When we call Publish() and Server returns HTTP 404,",
            expected = "NotFoundException must be thrown"
            )]
        public void Test_0011_Publish_not_found ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new NotFoundException("object not found", null, "{}", NotFoundException.Reason.OBJECT_NOT_FOUND));

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            obj.PublishBody();
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call Publish() and Server returns broken JSON,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0012_Publish_broken_json ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "broken");
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            obj.PublishBody();
        }
        #endregion

        #region PublishBodyExpiresIn()
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresIn() and Server returns publicationID/url,",
            expected = "We can get the URL for this body"
            )]
        public void Test_0100_PublishExpiresIn ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            string url = obj.PublishBodyExpiresIn(1000);
            
            Assert.AreEqual("http://api-jp.kii.com/a/1234", url);
            IList<string> body = client.RequestBody;
            Assert.AreEqual(1, body.Count);

            JsonObject argsJson = new JsonObject(body[0]);
            Assert.AreEqual(1, argsJson.Length());
            Assert.IsTrue(argsJson.Has("expiresIn"));
            Assert.AreEqual(1000, argsJson.GetLong("expiresIn"));
        }

        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresIn() and Server returns publicationID/url,",
            expected = "We can get the URL for this body"
            )]
        public void Test_0101_PublishExpiresIn_1 ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            string url = obj.PublishBodyExpiresIn(1);
            
            Assert.AreEqual("http://api-jp.kii.com/a/1234", url);
            IList<string> body = client.RequestBody;
            Assert.AreEqual(1, body.Count);
            JsonObject argsJson = new JsonObject(body[0]);
            Assert.AreEqual(1, argsJson.Length());
            Assert.IsTrue(argsJson.Has("expiresIn"));
            Assert.AreEqual(1, argsJson.GetLong("expiresIn"));
        }

        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call PublishExpiresIn() with not uploaded KiiObject,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_0110_PublishExpiresIn_not_uploaded ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = Kii.Bucket("images").NewKiiObject();
            obj.PublishBodyExpiresIn(1000);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call PublishExpiresIn() with 0,", 
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0111_PublishExpiresIn_0 ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);

            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            obj.PublishBodyExpiresIn(0);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call PublishExpiresIn() with -1,", 
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0112_PublishExpiresIn_negative ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            obj.PublishBodyExpiresIn(-1);
        }

        [Test(), ExpectedException(typeof(NotFoundException)), KiiUTInfo(
            action = "When we call PublishExpiresIn() and Server returns HTTP 404,",
            expected = "NotFoundException must be thrown"
            )]
        public void Test_0113_PublishExpiresIn_not_found ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new NotFoundException("object not found", null, "{}", NotFoundException.Reason.OBJECT_NOT_FOUND));
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            obj.PublishBodyExpiresIn(1000);
        }
        
        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call PublishExpiresIn() and Server returns broken JSON,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0114_PublishExpiresIn_broken_json ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "broken");
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            obj.PublishBodyExpiresIn(1000);
        }
        #endregion

        #region PublishBodyExpiresAt()
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresAt() and Server returns publicationID/url,",
            expected = "We can get the URL for this body"
            )]
        public void Test_0200_PublishExpiresAt ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            string url = obj.PublishBodyExpiresAt(new DateTime(2014, 3, 12, 16, 0, 0, DateTimeKind.Utc));
            
            Assert.AreEqual("http://api-jp.kii.com/a/1234", url);
            IList<string> body = client.RequestBody;
            Assert.AreEqual(1, body.Count);
            Console.WriteLine(body[0]);
            JsonObject argsJson = new JsonObject(body[0]);
            Assert.AreEqual(1, argsJson.Length());
            Assert.IsTrue(argsJson.Has("expiresAt"));
            Assert.AreEqual(1394640000000, argsJson.GetLong("expiresAt"));
        }
        
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresAt() and Server returns publicationID/url,",
            expected = "We can get the URL for this body"
            )]
        public void Test_0201_PublishExpiresAt_epock ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            string url = obj.PublishBodyExpiresAt(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            
            Assert.AreEqual("http://api-jp.kii.com/a/1234", url);
            IList<string> body = client.RequestBody;
            Assert.AreEqual(1, body.Count);
            JsonObject argsJson = new JsonObject(body[0]);
            Assert.AreEqual(1, argsJson.Length());
            Assert.IsTrue(argsJson.Has("expiresAt"));
            Assert.AreEqual(0, argsJson.GetLong("expiresAt"));
        }
        
        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call PublishExpiresAt() with not uploaded KiiObject,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_0210_PublishExpiresAt_not_uploaded ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = Kii.Bucket("images").NewKiiObject();
            obj.PublishBodyExpiresAt(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        }
        

        [Test(), ExpectedException(typeof(NotFoundException)), KiiUTInfo(
            action = "When we call PublishExpiresAt() and Server returns HTTP 404,",
            expected = "NotFoundException must be thrown"
            )]
        public void Test_0211_PublishExpiresAt_not_found ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new NotFoundException("object not found", null, "{}", NotFoundException.Reason.OBJECT_NOT_FOUND));
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            obj.PublishBodyExpiresAt(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        }
        
        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call PublishExpiresAt() and Server returns broken JSON,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_0212_PublishExpiresAt_broken_json ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "broken");
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            obj.PublishBodyExpiresAt(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        }
        #endregion

        #region PublishBody(callback)
        [Test(), KiiUTInfo(
            action = "When we call Publish(callback) and Server returns publicationID/url,",
            expected = "We can get the URL for this body"
            )]
        public void Test_1000_Publish ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));

            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBody((KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });

            Assert.IsTrue(done);
            Assert.IsNull(exception);
            
            Assert.AreEqual("http://api-jp.kii.com/a/1234", outUrl);
            IList<string> body = client.RequestBody;
            Assert.AreEqual(null, body[0]);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call Publish() with not uploaded KiiObject,",
            expected = "InvalidOperationException must be returned"
            )]
        public void Test_1010_Publish_not_uploaded ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = Kii.Bucket("images").NewKiiObject();
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBody((KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNull(outUrl);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is InvalidOperationException);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call Publish() and Server returns HTTP 404,",
            expected = "NotFoundException must be returned"
            )]
        public void Test_1011_Publish_not_found ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new NotFoundException("object not found", null, "{}", NotFoundException.Reason.OBJECT_NOT_FOUND));
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBody((KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNull(outUrl);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is NotFoundException);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call Publish() and Server returns broken JSON,",
            expected = "IllegalKiiBaseObjectFormatException must be returned"
            )]
        public void Test_1012_Publish_broken_json ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "broken");
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBody((KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNull(outUrl);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is IllegalKiiBaseObjectFormatException);
        }
        #endregion

        #region PublishBodyExpiresIn(callback)
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresIn() and Server returns publicationID/url,",
            expected = "We can get the URL for this body"
            )]
        public void Test_1100_PublishExpiresIn ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBodyExpiresIn(1000, (KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outUrl);
            Assert.IsNull(exception);
            
            Assert.AreEqual("http://api-jp.kii.com/a/1234", outUrl);
            IList<string> body = client.RequestBody;
            Assert.AreEqual(1, body.Count);
            
            JsonObject argsJson = new JsonObject(body[0]);
            Assert.AreEqual(1, argsJson.Length());
            Assert.IsTrue(argsJson.Has("expiresIn"));
            Assert.AreEqual(1000, argsJson.GetLong("expiresIn"));
        }
        
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresIn() and Server returns publicationID/url,",
            expected = "We can get the URL for this body"
            )]
        public void Test_1101_PublishExpiresIn_1 ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBodyExpiresIn(1, (KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outUrl);
            Assert.IsNull(exception);
            
            Assert.AreEqual("http://api-jp.kii.com/a/1234", outUrl);
            IList<string> body = client.RequestBody;
            Assert.AreEqual(1, body.Count);
            JsonObject argsJson = new JsonObject(body[0]);
            Assert.AreEqual(1, argsJson.Length());
            Assert.IsTrue(argsJson.Has("expiresIn"));
            Assert.AreEqual(1, argsJson.GetLong("expiresIn"));
        }
        
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresIn() with not uploaded KiiObject,",
            expected = "InvalidOperationException must be returned"
            )]
        public void Test_1110_PublishExpiresIn_not_uploaded ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = Kii.Bucket("images").NewKiiObject();
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBodyExpiresIn(1000, (KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNull(outUrl);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is InvalidOperationException);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresIn() with 0,", 
            expected = "ArgumentException must be returned"
            )]
        public void Test_1111_PublishExpiresIn_0 ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBodyExpiresIn(0, (KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNull(outUrl);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is ArgumentException);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresIn() with -1,", 
            expected = "ArgumentException must be returned"
            )]
        public void Test_1112_PublishExpiresIn_negative ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBodyExpiresIn(-1, (KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNull(outUrl);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is ArgumentException);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresIn() and Server returns HTTP 404,",
            expected = "NotFoundException must be returned"
            )]
        public void Test_1113_PublishExpiresIn_not_found ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new NotFoundException("object not found", null, "{}", NotFoundException.Reason.OBJECT_NOT_FOUND));
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBodyExpiresIn(1000, (KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNull(outUrl);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is NotFoundException);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresIn() and Server returns broken JSON,",
            expected = "IllegalKiiBaseObjectFormatException must be returned"
            )]
        public void Test_1114_PublishExpiresIn_broken_json ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "broken");
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBodyExpiresIn(1000, (KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNull(outUrl);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is IllegalKiiBaseObjectFormatException);
        }
        #endregion

        #region PublishBodyExpiresAt(callback)
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresAt() and Server returns publicationID/url,",
            expected = "We can get the URL for this body"
            )]
        public void Test_1200_PublishExpiresAt ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBodyExpiresAt(new DateTime(2014, 3, 12, 16, 0, 0, DateTimeKind.Utc),
                                     (KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outUrl);
            Assert.IsNull(exception);
            
            Assert.AreEqual("http://api-jp.kii.com/a/1234", outUrl);
            IList<string> body = client.RequestBody;
            Assert.AreEqual(1, body.Count);
            JsonObject argsJson = new JsonObject(body[0]);
            Assert.AreEqual(1, argsJson.Length());
            Assert.IsTrue(argsJson.Has("expiresAt"));
            Assert.AreEqual(1394640000000, argsJson.GetLong("expiresAt"));
        }
        
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresAt() and Server returns publicationID/url,",
            expected = "We can get the URL for this body"
            )]
        public void Test_1201_PublishExpiresAt_epock ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBodyExpiresAt(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                     (KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNotNull(outUrl);
            Assert.IsNull(exception);
            
            Assert.AreEqual("http://api-jp.kii.com/a/1234", outUrl);
            IList<string> body = client.RequestBody;
            Assert.AreEqual(1, body.Count);
            JsonObject argsJson = new JsonObject(body[0]);
            Assert.AreEqual(1, argsJson.Length());
            Assert.IsTrue(argsJson.Has("expiresAt"));
            Assert.AreEqual(0, argsJson.GetLong("expiresAt"));
        }
        
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresAt() with not uploaded KiiObject,",
            expected = "InvalidOperationException must be thrown"
            )]
        public void Test_1210_PublishExpiresAt_not_uploaded ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = Kii.Bucket("images").NewKiiObject();
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBodyExpiresAt(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                     (KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNull(outUrl);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is InvalidOperationException);
        }
        
        
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresAt() and Server returns HTTP 404,",
            expected = "NotFoundException must be thrown"
            )]
        public void Test_1211_PublishExpiresAt_not_found ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new NotFoundException("object not found", null, "{}", NotFoundException.Reason.OBJECT_NOT_FOUND));
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBodyExpiresAt(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                     (KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNull(outUrl);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is NotFoundException);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call PublishExpiresAt() and Server returns broken JSON,",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_1212_PublishExpiresAt_broken_json ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "broken");
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            bool done = false;
            string outUrl = null;
            Exception exception = null;
            obj.PublishBodyExpiresAt(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                     (KiiObject obj2, string url, Exception e) =>
            {
                done = true;
                outUrl = url;
                exception = e;
            });
            Assert.IsTrue(done);
            Assert.IsNull(outUrl);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is IllegalKiiBaseObjectFormatException);
        }
        #endregion
    }
}

