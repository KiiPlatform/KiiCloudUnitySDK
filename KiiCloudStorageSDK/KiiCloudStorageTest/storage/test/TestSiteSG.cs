// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using NUnit.Framework;
namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestSiteSG
    {

        [SetUp()]
        public void SetUp()
        {
            Kii.Instance = null;
        }

        [TearDown()]
        public void TearDown()
        {
            Kii.Instance = null;
        }

        [Test()]
        public void TestSite()
        {
            Kii.Initialize("dummyId", "dummyKey", Kii.Site.SG);
            Assert.AreEqual("https://api-sg.kii.com/api", Kii.BaseUrl);
        }

        [Test()]
        public void TestRequestUrl()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.SG);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            Kii.AsyncHttpClientFactory = factory;
            MockHttpClient client = (MockHttpClient)factory.Client;

            // send request
            client.AddResponse(200, "{" +
                               "\"id\" : \"user1234\"," +
                               "\"access_token\" : \"cdef\"," +
                               "\"expires_in\" : 9223372036854775}");
            KiiUser.LogIn("kii1234", "pass1234");

            // check request url
            Assert.AreEqual ("https://api-sg.kii.com/api/oauth2/token", client.RequestUrl [0]);
        }
    }
}
