using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiGroup
    {
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

        }

        #region KiiGroup.CreateByUri(uri)
        [Test(), KiiUTInfo(
            action = "When we call CreateByUri() with valid URI,",
            expected = "We can get KiiGroup object with no groupName"
            )]
        public void Test_0000_CreateByUri ()
        {
            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/abcd"));

            Assert.IsNotNull(group);
            Assert.AreEqual(null, group.Name);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0001_CreateByUri_null ()
        {
            KiiGroup.CreateByUri(null);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with invalid scheme(http),",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0002_CreateByUri_invalid_scheme ()
        {
            KiiGroup.CreateByUri(new Uri("http://groups/abcd"));
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with invalid authority(kiigroup),",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0003_CreateByUri_invalid_authority ()
        {
            KiiGroup.CreateByUri(new Uri("kiicloud://kiigroup/abcd"));
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with no ID,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0004_CreateByUri_segments_1 ()
        {
            KiiGroup.CreateByUri(new Uri("kiicloud://groups/"));
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with URI that has 3 segments,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0005_CreateByUri_segments_3 ()
        {
            KiiGroup.CreateByUri(new Uri("kiicloud://groups/abcd/efgh"));
        }

        [Test(), KiiUTInfo(
            action = "When we call CreateByUri() with URI that ends with '/',",
            expected = "We can get GroupID"
            )]
        public void Test_0006_CreateByUri_slash ()
        {
            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/abcd/"));

            Assert.AreEqual("abcd", group.ID);
        }

        [Test()]
        public void TestGetIDAfterSave()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            MockHttpClient client = factory.Client;
            // login
            client.AddResponse(200, "{" +
                               "\"id\" : \"user1234\"," +
                               "\"access_token\" : \"cdef\"," +
                               "\"expires_in\" : 9223372036854775}");
            KiiUser.LogIn("kii1234", "pass1234");
            
            client.AddResponse(200, "{" +
                               "\"groupID\" : \"dummyId\"" +
                               "}");
            KiiGroup group = Kii.Group("MyGroup");
            group.Save();
            
            Assert.AreEqual("dummyId", group.ID);
            Assert.AreEqual("MyGroup", group.Name);
            
        }
        
        [Test()]
        public void TestGetIDAfterCreateFromURI()
        {
            KiiGroup group = KiiGroup.CreateByUri(new Uri("kiicloud://groups/dummyID"));
            Assert.IsNotNull(group.ID);
            Assert.AreEqual("dummyID", group.ID);
        }

        #endregion


    }
}

