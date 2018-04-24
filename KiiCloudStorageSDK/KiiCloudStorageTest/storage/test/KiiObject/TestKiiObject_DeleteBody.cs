using System;
using System.Collections.Generic;
using System.Reflection;

using NUnit.Framework;
using JsonOrg;

/// <summary>
/// Test kii object_ delete body.
/// Test cases : https://docs.google.com/a/kii.com/spreadsheet/ccc?key=0AsiWA7MkWrQldFdoX080bnVpZWk5LTJzV0ZXZndMUGc&usp=drive_web#gid=7
/// </summary>
namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiObject_DeleteBody
    {
        private void SetStandardResponse(MockHttpClient client)
        {
            client.AddResponse(200, null, null);
        }

        #region deleteBody()

        [Test(), ExpectedException(typeof(InvalidOperationException)), KiiUTInfo(
            action = "When we call deleteBody() and object has not uploaded to cloud,",
            expected = "InvalidOperationException must thrown"
            )]
        public void Test_1_1_DeleteBody_not_uploaded ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);

            KiiObject obj = Kii.Bucket ("MyBucket").NewKiiObject ();
            obj.DeleteBody();
        }

        [Test(), KiiUTInfo(
            action = "When we call deleteBody() and object not exists in the cloud,",
            expected = "NotFoundException must thrown"
            )]
        public void Test_1_2_DeleteBody_object_not_exists ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new NotFoundException("object not found", null, "{}", NotFoundException.Reason.OBJECT_NOT_FOUND));
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            NotFoundException exp = null;
            try {
                obj.DeleteBody();
            }
            catch(NotFoundException e)
            {
                exp = e;
            }
            Assert.IsNotNull (exp);
            Assert.AreEqual (NotFoundException.Reason.OBJECT_NOT_FOUND, exp.reason);
        }

        [Test(), KiiUTInfo(
            action = "When we call deleteBody() and object not exists in the cloud,",
            expected = "NotFoundException must thrown"
            )]
        public void Test_1_3_DeleteBody_body_not_exists ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new NotFoundException("object body not found", null, "{}", NotFoundException.Reason.OBJECT_BODY_NOT_FOUND));
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            NotFoundException exp = null;
            try {
                obj.DeleteBody();
            }
            catch(NotFoundException e)
            {
                exp = e;
            }
            Assert.IsNotNull (exp);
            Assert.AreEqual (NotFoundException.Reason.OBJECT_BODY_NOT_FOUND, exp.reason);
        }

        [Test(), KiiUTInfo(
            action = "When we call deleteBody() and owner not exists in the cloud,",
            expected = "NotFoundException must thrown"
            )]
        public void Test_1_4_DeleteBody_owner_not_exists ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new NotFoundException("user not found", null, "{}", NotFoundException.Reason.USER_NOT_FOUND));
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://users/dummyUser/buckets/images/objects/object1234"));
            NotFoundException exp = null;
            try {
                obj.DeleteBody();
            }
            catch(NotFoundException e)
            {
                exp = e;
            }
            Assert.IsNotNull (exp);
            Assert.AreEqual (NotFoundException.Reason.USER_NOT_FOUND, exp.reason);
        }

        [Test(), ExpectedException(typeof(UnauthorizedException)), KiiUTInfo(
            action = "When we call deleteBody() and owner not exists in the cloud,",
            expected = "UnauthorizedAccess exception must thrown"
            )]
        public void Test_1_5_DeleteBody_unauthorized_user ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new UnauthorizedException("user unauthorized", null, "{}"));
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            obj.DeleteBody();
        }

        [Test(), KiiUTInfo(
            action = "When we call deleteBody() and object body exists in the cloud,",
            expected = "body delete from cloud successfully"
            )]
        public void Test_1_6_DeleteBody ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardResponse(client);
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            // set body content-type
            SDKTestHack.SetField(obj, "mBodyContentType", "text/plain");

            obj.DeleteBody();

            // check body content-type is cleared.
            Assert.IsNull (obj.BodyContentType);
        }

        [Test(), KiiUTInfo(
            action = "When we call deleteBody() and bucket not exists in the cloud,",
            expected = "NotFoundException must thrown"
            )]
        public void Test_1_7_DeleteBody_bucket_not_exists ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set response
            MockHttpClient client = factory.Client;
            client.AddResponse(new NotFoundException("bucket not found", null, "{}", NotFoundException.Reason.BUCKET_NOT_FOUND));
            
            KiiObject obj = KiiObject.CreateByUri(new Uri("kiicloud://buckets/images/objects/object1234"));
            NotFoundException exp = null;
            try {
                obj.DeleteBody();
            }
            catch(NotFoundException e)
            {
                exp = e;
            }
            Assert.IsNotNull (exp);
            Assert.AreEqual (NotFoundException.Reason.BUCKET_NOT_FOUND, exp.reason);
        }
        #endregion
    }
}

