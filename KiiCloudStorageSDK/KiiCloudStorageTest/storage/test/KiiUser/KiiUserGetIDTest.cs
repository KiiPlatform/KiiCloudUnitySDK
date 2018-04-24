using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiUserGetIDTest
    {
        [Test()]
        public void TestGetIDAfterSave ()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // prepare response
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{\"userID\" : \"dummyID\", \"loginName\" : \"dummyName\"}");
            client.AddResponse(201, "{\"id\" : \"dummyID\", \"expires_in\" : 148478248144076800, \"access_token\" : \"dummyToken\"}");
            client.AddResponse(201, "{\"userID\" : \"dummyID\"}");
            
            KiiUser user = KiiUser.BuilderWithName("dummyName").Build();
            Assert.IsNull(user.ID);
            user.Register("pass");
            Assert.IsNotNull(user.ID);
            Assert.AreEqual("dummyID", user.ID);
        }
        
        [Test()]
        public void TestGetIDAfterCreateFromURI ()
        {
            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/dummyID"));
            Assert.IsNotNull(user.ID);
            Assert.AreEqual("dummyID", user.ID);
        }
    }
}


