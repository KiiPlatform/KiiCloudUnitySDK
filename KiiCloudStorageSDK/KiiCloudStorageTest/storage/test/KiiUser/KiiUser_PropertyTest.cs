using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiUser_PropertyTest
    {
        private const string NUMBER_10 = "1234567890";

        private void setStandardResponse(MockHttpClient client)
        {
            client.AddResponse(200, "{" +
                "\"id\" : \"abcd\"," +
                "\"access_token\" : \"cdef\"," +
                "\"expires_in\" : 9223372036854775}");

        }

        #region Uri
        [Test(), KiiUTInfo(
            action = "When We call Uri to KiiUser object that has UserID,",
            expected = "We can get kiicloud://users/{userID}"
            )]
        public void Test_0000_Uri()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.setStandardResponse(client);

            KiiUser user = KiiUser.LogIn("kii1234", "pass1234");
            Assert.AreEqual("kiicloud://users/abcd", user.Uri.ToString());
        }

        #endregion
    }
}

