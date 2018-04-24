using System;
using NUnit.Framework;

using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class CMO4964
    {

#region Test mock data
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
                    "\"age\" : 18" +
                    "}",
                    "1");
        }
#endregion

#region Test cases
        [Test()]
        public void Test_remove_KiiObject ()
        {
            // Do initialize
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // Set response
            MockHttpClient client = factory.Client;
            this.SetStandardSaveResponse(client);

            // Set KiiObject
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "Kii";
            obj["age"] = 18;

            // Do save
            obj.Save();

            // Assert
            Assert.AreEqual("d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.ID);
            Assert.AreEqual(1337039114613, obj.CreatedTime);
            Assert.AreEqual(1337039114613, obj.ModifedTime);

            // Set response
            this.SetStandardRefreshResponse(client);

            // Create and refresh KiiObject from URI
            KiiObject obj2 = KiiObject.CreateByUri(obj.Uri);
            obj2.Refresh();

            // Assert
            Assert.IsTrue(obj2.Has("name"));
            Assert.AreEqual("Kii", obj2.GetString("name"));
            Assert.IsTrue(obj2.Has("age"));
            Assert.AreEqual( 18, obj2.GetInt("age"));

            // Remove age key
            obj2.Remove("age");

            // Assert
            Assert.IsTrue(obj2.Has("name"));
            Assert.AreEqual("Kii", obj2.GetString("name"));
            Assert.IsFalse(obj2.Has("age"));

            // Remove age key once more
            obj2.Remove("age");
            
            // Assert
            Assert.IsTrue(obj2.Has("name"));
            Assert.AreEqual("Kii", obj2.GetString("name"));
            Assert.IsFalse(obj2.Has("age"));
        }
#endregion
    }
}

