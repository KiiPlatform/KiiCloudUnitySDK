using System;
using NUnit.Framework;

// TestSpec: https://docs.google.com/a/kii.com/spreadsheets/d/1LDAUkrJVIc8tLwjX0hfdyMw7x4Y4eu-n8OP0fFurAsM/edit#gid=0
namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiUserWithIDTest
    {

        [SetUp()]
        public void SetUp() {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
        }

        [Test()]
        public void TestUserWithID_1_3_existsInCloud_refresh ()
        {
            // mock refresh response
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            MockHttpClient client = factory.Client;
            client.AddResponse(201, "{" +
                "\"userID\" : \"dummyID\"," +
                "\"loginName\" : \"dummyName\"" +
            "}");

            // create user with id
            KiiUser userWithId = KiiUser.UserWithID("dummyID");
            Assert.IsNull(userWithId.Username);

            // refresh
            userWithId.Refresh();
            Assert.AreEqual("dummyName", userWithId.Username);

            // verify request.
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","users","dummyID");
            Assert.AreEqual(url, client.RequestUrl[0]);
            Assert.AreEqual(KiiHttpMethod.GET, client.RequestMethod[0]);
            MockHttpHeaderList headerList = client.RequestHeader[0];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
        }

        [Test()]
        public void TestUserWithID_1_8_NotExistsInCloud_refresh ()
        {
            // mock refresh response.
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            MockHttpClient client = factory.Client;
            client.AddResponse(new CloudException(404, "USER_NOT_FOUND"));

            // create user with id.
            KiiUser userWithId = KiiUser.UserWithID("dummyID");
            Assert.IsNull(userWithId.Username);
            
            // refresh
            CloudException exp = null;
            try {
                userWithId.Refresh();
                Assert.Fail("Exception not thrown");
            } catch (CloudException e) {
                exp = e;
            }
            Assert.IsNotNull(exp);
            Assert.AreEqual(404, exp.Status);

            // verify request.
            string url = Utils.Path(ConstantValues.DEFAULT_BASE_URL,"apps","appId","users","dummyID");
            Assert.AreEqual(url, client.RequestUrl[0]);
            Assert.AreEqual(KiiHttpMethod.GET, client.RequestMethod[0]);
            MockHttpHeaderList headerList = client.RequestHeader[0];
            Assert.AreEqual("appId",headerList["X-Kii-AppID"] );
            Assert.AreEqual("appKey",headerList["X-Kii-AppKey"] );
            Assert.IsTrue(headerList["X-Kii-SDK"].StartsWith("sn=cs;sv="));
        }
        
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_2_1_UserWithNullID ()
        {
            KiiUser.UserWithID(null);
        }
        
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_2_2_UserWithEmptyID ()
        {
            KiiUser.UserWithID("");
        }
    }
}


