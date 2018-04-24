using System;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Analytics;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKii
    {
        [SetUp()]
        public void SetUp()
        {
            Kii.Instance = null;
        }

        private void SetStandardSaveResponse(MockHttpClient client)
        {
            client.AddResponse(201, "{" +
                "\"objectID\" : \"d8dc9f29-0fb9-48be-a80c-ec60fddedb54\"," +
                "\"createdAt\" : 1337039114613," +
                "\"dataType\" : \"application/vnd.sandobx.mydata+json\"" +
                "}",
                "1");
        }


        [TearDown()]
        public void TearDown()
        {
            Kii.Instance = null;
        }

        #region Kii.Initialize(string, string, Site)
        [Test(), KiiUTInfo(
            action = "When we set Kii.Site.US,",
            expected = "BaseUrl must be US")]
        public void Test_0000_initialize_id_key_Site_US ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
            Assert.AreEqual("ee573743", Kii.AppId);
            Assert.AreEqual("5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.AppKey);
            Assert.AreEqual("https://api.kii.com/api", Kii.BaseUrl);
        }

        [Test(), KiiUTInfo(
            action = "When we set Kii.Site.JP,",
            expected = "BaseUrl must be JP")]
        public void Test_0000_initialize_id_key_JP ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.JP);
            Assert.AreEqual("ee573743", Kii.AppId);
            Assert.AreEqual("5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.AppKey);
            Assert.AreEqual("https://api-jp.kii.com/api", Kii.BaseUrl);
        }

        [Test(), KiiUTInfo(
            action = "When we set Kii.Site.JP,",
            expected = "BaseUrl must be JP")]
        public void Test_0000_initialize_id_key_CN ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.CN);
            Assert.AreEqual("ee573743", Kii.AppId);
            Assert.AreEqual("5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.AppKey);
            Assert.AreEqual("https://api-cn2.kii.com/api", Kii.BaseUrl);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we set AppID=null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0001_initialize_id_null()
        {
            Kii.Initialize(null, "5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we set AppKey=null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0002_initialize_key_null()
        {
            Kii.Initialize("ee573743", null, Kii.Site.US);
        }

        [Test(), KiiUTInfo(
            action = "When we call Kii.Initialize() 2 times with the same AppId and key,",
            expected = "No exception will be thrown."
            )]
        public void Test_0003_initialize_2times_same ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
            Assert.AreEqual("ee573743", Kii.AppId);
            Assert.AreEqual("5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.AppKey);
            Assert.AreEqual("https://api.kii.com/api", Kii.BaseUrl);

            KiiUser user = KiiUser.UserWithID("dummyID");
            _KiiInternalUtils.SetCurrentUser(user);
            KiiCloudEngine.UpdateAccessToken("accesstoken");

            // same id and key
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
            Assert.AreEqual("ee573743", Kii.AppId);
            Assert.AreEqual("5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.AppKey);
            Assert.AreEqual("https://api.kii.com/api", Kii.BaseUrl);
            Assert.AreEqual(user, KiiUser.CurrentUser);
            Assert.AreEqual("accesstoken", KiiCloudEngine.AccessToken);
        }

        [Test(), KiiUTInfo(
            action = "When we call Kii.Initialize() 2 times with different AppId,",
            expected = "No exception will be thrown."
            )]
        public void Test_0004_initialize_2times_id_change ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
            Assert.AreEqual("ee573743", Kii.AppId);
            Assert.AreEqual("5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.AppKey);
            Assert.AreEqual("https://api.kii.com/api", Kii.BaseUrl);

            KiiCloudEngine.UpdateAccessToken("accesstoken");

            // id is changed
            Kii.Initialize("myId","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);

            Assert.IsNull(KiiCloudEngine.AccessToken);
        }

        [Test(), KiiUTInfo(
            action = "When we call Kii.Initialize() 2 times with different AppKey,",
            expected = "No exception will be thrown."
            )]
        public void Test_0005_initialize_2times_key_change ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
            Assert.AreEqual("ee573743", Kii.AppId);
            Assert.AreEqual("5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.AppKey);
            Assert.AreEqual("https://api.kii.com/api", Kii.BaseUrl);

            KiiCloudEngine.UpdateAccessToken("accesstoken");

            // same id and site
            Kii.Initialize("ee573743","myKey", Kii.Site.US);

            Assert.IsNull(KiiCloudEngine.AccessToken);
        }

        [Test(), KiiUTInfo(
            action = "When we call Kii.Initialize() 2 times with different Site,",
            expected = "No exception will be thrown."
            )]
        public void Test_0006_initialize_2times_Site_change ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
            Assert.AreEqual("ee573743", Kii.AppId);
            Assert.AreEqual("5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.AppKey);
            Assert.AreEqual("https://api.kii.com/api", Kii.BaseUrl);

            KiiCloudEngine.UpdateAccessToken("accesstoken");

            // same id and key
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.JP);

            Assert.IsNull(KiiCloudEngine.AccessToken);
        }
        #endregion

        #region Kii.Initialize(string, string, string)
        [Test(), KiiUTInfo(
            action = "When we give URL to Kii.Initialize(),",
            expected = "BaseUrl must be the specified one")]
        public void Test_0100_initialize_id_key_Site_url ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", "https://api.kii.com/api");
            Assert.AreEqual("ee573743", Kii.AppId);
            Assert.AreEqual("5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.AppKey);
            Assert.AreEqual("https://api.kii.com/api", Kii.BaseUrl);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we set AppID=null and URL,",
            expected = "ArgumentException must be thrown")]
        public void Test_0101_initialize_id_null()
        {
            Kii.Initialize(null, "5eb7b8bc1b4e4c98e659431c69cef8d4", "https://api.kii.com/api");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we set AppKey=null and URL,",
            expected = "ArgumentException must be thrown")]
        public void Test_0102_initialize_key_null()
        {
            Kii.Initialize("ee573743", null, "https://api.kii.com/api");
        }

        [Test(), KiiUTInfo(
            action = "When we set URL=null,",
            expected = "ArgumentException must be thrown")]
        public void Test_0103_initialize_url_null()
        {
            try
            {
                Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", null);
                Assert.Fail("ArgumentException must be thrown");
            }
            catch (ArgumentException e)
            {
                Assert.IsNull(Kii.Instance);
            }
        }
        [Test(), KiiUTInfo(
            action = "When we set URL=invalid URL,",
            expected = "ArgumentException must be thrown")]
        public void Test_0103_initialize_url_invalid()
        {
            try
            {
                Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", "hogehoge");
                Assert.Fail("ArgumentException must be thrown");
            }
            catch (ArgumentException e)
            {
                Assert.IsNull(Kii.Instance);
            }
        }

        [Test(), KiiUTInfo(
            action = "When we call Kii.Initialize() 2 times with the same AppId , key and URL,",
            expected = "No exception will be thrown."
            )]
        public void Test_0104_initialize_id_key_Site_2times_same ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", "https://api.kii.com/api");
            Assert.AreEqual("ee573743", Kii.AppId);
            Assert.AreEqual("5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.AppKey);
            Assert.AreEqual("https://api.kii.com/api", Kii.BaseUrl);

            KiiCloudEngine.UpdateAccessToken("accesstoken");

            // 2 times
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", "https://api.kii.com/api");
            Assert.AreEqual("ee573743", Kii.AppId);
            Assert.AreEqual("5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.AppKey);
            Assert.AreEqual("https://api.kii.com/api", Kii.BaseUrl);
            Assert.AreEqual("accesstoken", KiiCloudEngine.AccessToken);
        }

        [Test(), KiiUTInfo(
            action = "When we call Kii.Initialize() 2 times with the different AppId,",
            expected = "No exception will be thrown."
            )]
        public void Test_0105_initialize_id_key_Url_2times_id_change ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", "https://api.kii.com/api");
            Assert.AreEqual("ee573743", Kii.AppId);
            Assert.AreEqual("5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.AppKey);
            Assert.AreEqual("https://api.kii.com/api", Kii.BaseUrl);

            KiiCloudEngine.UpdateAccessToken("accesstoken");

            // 2 times
            Kii.Initialize("myKey","5eb7b8bc1b4e4c98e659431c69cef8d4", "https://api.kii.com/api");

            Assert.IsNull(KiiCloudEngine.AccessToken);
        }

        [Test(), KiiUTInfo(
            action = "When we call Kii.Initialize() 2 times with the different AppKey,",
            expected = "No exception will be thrown."
            )]
        public void Test_0106_initialize_id_key_Url_2times_key_change ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", "https://api.kii.com/api");
            Assert.AreEqual("ee573743", Kii.AppId);
            Assert.AreEqual("5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.AppKey);
            Assert.AreEqual("https://api.kii.com/api", Kii.BaseUrl);

            KiiCloudEngine.UpdateAccessToken("accesstoken");

            // 2 times
            Kii.Initialize("ee573743","myKey", "https://api.kii.com/api");

            Assert.IsNull(KiiCloudEngine.AccessToken);
        }

        [Test(), KiiUTInfo(
            action = "When we call Kii.Initialize() 2 times with the different URL,",
            expected = "No exception will be thrown."
            )]
        public void Test_0107_initialize_id_key_Url_2times_url_change ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", "https://api.kii.com/api");
            Assert.AreEqual("ee573743", Kii.AppId);
            Assert.AreEqual("5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.AppKey);
            Assert.AreEqual("https://api.kii.com/api", Kii.BaseUrl);

            KiiCloudEngine.UpdateAccessToken("accesstoken");

            // 2 times
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", "https://api-jp.kii.com/api");

            Assert.IsNull(KiiCloudEngine.AccessToken);
        }

        #endregion

        #region Kii.Bucket(string)
        [Test(), KiiUTInfo(
            action = "When we call Kii.Bucket() with valid bucket name,",
            expected = "We can get KiiBucket instance."
            )]
        public void Test_0200_Bucket ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
            KiiBucket bucket = Kii.Bucket("appBucket");

            Assert.AreEqual("kiicloud://buckets/appBucket", bucket.Uri.ToString());

            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set response
            MockHttpClient client = factory.Client;
            this.SetStandardSaveResponse(client);

            KiiObject obj = bucket.NewKiiObject();
            obj.Save();

            Assert.AreEqual("kiicloud://buckets/appBucket/objects/d8dc9f29-0fb9-48be-a80c-ec60fddedb54", obj.Uri.ToString());
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call Kii.Bucket() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0201_Bucket_null ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
            Kii.Bucket(null);
        }

        #endregion

        #region Kii.Group(string)

        [Test(), KiiUTInfo(
            action = "When we call Kii.Group() with valid group name,",
            expected = "We can get KiiGroup with given name"
            )]
        public void Test_0300_Group ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
            KiiGroup group = Kii.Group("group1");

            Assert.AreEqual("group1", group.Name);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call Kii.Group() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0301_Group_null ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
            Kii.Group(null);
        }

        #endregion

        #region Kii.Group(string, IList)

        [Test(), KiiUTInfo(
            action = "When we call Kii.Group() with valid name and member list,",
            expected = "We can get KiiGroup instance with given name"
            )]
        public void Test_0400_Group ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
            List<KiiUser> members = new List<KiiUser>();
            members.Add(KiiUser.CreateByUri(new Uri("kiicloud://users/abcd")));
            members.Add(KiiUser.CreateByUri(new Uri("kiicloud://users/efgh")));
            KiiGroup group = Kii.Group("group1", members);

            Assert.AreEqual("group1", group.Name);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call Kii.Group() with name=null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0401_Group_name_null ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
            List<KiiUser> members = new List<KiiUser>();
            members.Add(KiiUser.CreateByUri(new Uri("kiicloud://users/abcd")));
            members.Add(KiiUser.CreateByUri(new Uri("kiicloud://users/efgh")));
            Kii.Group(null, members);
        }

        [Test(), KiiUTInfo(
            action = "When we call Kii.Group() with memberList=null,",
            expected = "We can get KiiGroup instance with no member."
            )]
        public void Test_0402_Group_member_null ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
            KiiGroup group = Kii.Group("group1", null);

            Assert.AreEqual("group1", group.Name);
        }

        [Test(), KiiUTInfo(
            action = "When we call Kii.Group() with empty member list,",
            expected = "We can get KiiGroup instance with no member."
            )]
        public void Test_0403_Group_member_empty ()
        {
            Kii.Initialize("ee573743","5eb7b8bc1b4e4c98e659431c69cef8d4", Kii.Site.US);
            KiiGroup group = Kii.Group("group1", new List<KiiUser>());

            Assert.AreEqual("group1", group.Name);
        }
        #endregion

        #region Error Case
        [Test(), KiiUTInfo(
            action = "When we call Storage blocking API without initialization.",
            expected = "InvalidOperationException must be thrown."
            )]
        public void Test_Non_Initialize_StorageAPI()
        {
            KiiUser user = null;
            KiiGroup group = null;
            KiiBucket bucket = null;
            KiiObject obj = null;
            try
            {
                // User Management
                user = KiiUser.BuilderWithName("hoge").Build();
                user.Register("password");
                Assert.Fail("InvalidOperationException isn't thrown");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(ErrorInfo.UTILS_KIICLIENT_NULL, e.Message);
            }
            try
            {
                bucket = Kii.Bucket("appbucket");
                bucket.Delete();
                Assert.Fail("InvalidOperationException isn't thrown");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(ErrorInfo.UTILS_KIICLIENT_NULL, e.Message);
            }
            try
            {
                obj = bucket.NewKiiObject();
                obj.Save();
                Assert.Fail("InvalidOperationException isn't thrown");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(ErrorInfo.UTILS_KIICLIENT_NULL, e.Message);
            }
            try
            {
                KiiQuery query = new KiiQuery();
                bucket.Query(query);
                Assert.Fail("InvalidOperationException isn't thrown");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(ErrorInfo.UTILS_KIICLIENT_NULL, e.Message);
            }
            try
            {
                group = Kii.Group("myGroup");
                group.Save();
                Assert.Fail("InvalidOperationException isn't thrown");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(ErrorInfo.UTILS_KIICLIENT_NULL, e.Message);
            }
        }
        [Test(), KiiUTInfo(
            action = "When we call Analytics blocking API without initialization.",
            expected = "InvalidOperationException must be thrown."
            )]
        public void Test_Non_Initialize_AnalyticsAPI()
        {
            try
            {
                KiiEvent ev = KiiAnalytics.NewEvent("MyUser");
                KiiAnalytics.Upload(ev);
                Assert.Fail("InvalidOperationException isn't thrown");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(ErrorInfo.UTILS_KIICLIENT_NULL, e.Message);
            }
            try
            {
                ResultCondition condition = new ResultCondition();
                KiiAnalytics.GetResult("1234", condition);
                Assert.Fail("InvalidOperationException isn't thrown");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(ErrorInfo.UTILS_KIICLIENT_NULL, e.Message);
            }
        }
        [Test(), KiiUTInfo(
            action = "When we call Storage non blocking API without initialization.",
            expected = "InvalidOperationException must be thrown."
            )]
        public void Test_Non_Initialize_StorageAPI_Async()
        {
            KiiUser user = null;
            KiiGroup group = null;
            KiiBucket bucket = null;
            KiiObject obj = null;
            // User Management
            try
            {
                user = KiiUser.BuilderWithName("hoge").Build();
                user.Register("password", (KiiUser u, Exception e)=>{
                });
                Assert.Fail("InvalidOperationException isn't thrown");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(ErrorInfo.UTILS_KIICLIENT_NULL, e.Message);
            }
            try
            {
                bucket = Kii.Bucket("appbucket");
                bucket.Delete((KiiBucket b, Exception e)=>{
                });
                Assert.Fail("InvalidOperationException isn't thrown");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(ErrorInfo.UTILS_KIICLIENT_NULL, e.Message);
            }
            try
            {
                obj = bucket.NewKiiObject();
                obj.Save((KiiObject o, Exception e)=>{
                });
                Assert.Fail("InvalidOperationException isn't thrown");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(ErrorInfo.UTILS_KIICLIENT_NULL, e.Message);
            }
            try
            {
                KiiQuery query = new KiiQuery();
                bucket.Query(query, (KiiQueryResult<KiiObject> result, Exception e)=>{
                });
                Assert.Fail("InvalidOperationException isn't thrown");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(ErrorInfo.UTILS_KIICLIENT_NULL, e.Message);
            }
            try
            {
                group = Kii.Group("myGroup");
                group.Save((KiiGroup g, Exception e)=>{
                });
                Assert.Fail("InvalidOperationException isn't thrown");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(ErrorInfo.UTILS_KIICLIENT_NULL, e.Message);
            }
        }
        [Test(), KiiUTInfo(
            action = "When we call Analytics non blocking API without initialization.",
            expected = "InvalidOperationException must be thrown."
            )]
        public void Test_Non_Initialize_AnalyticsAPI_Async()
        {
            try
            {
                KiiEvent ev = KiiAnalytics.NewEvent("MyUser");
                KiiAnalytics.Upload((Exception e)=>{
                }, ev);
                Assert.Fail("InvalidOperationException isn't thrown");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(ErrorInfo.UTILS_KIICLIENT_NULL, e.Message);
            }
            try
            {
                ResultCondition condition = new ResultCondition();
                KiiAnalytics.GetResult("1234", condition, (string ruleId, ResultCondition c, GroupedResult r, Exception e)=>{
                });
                Assert.Fail("InvalidOperationException isn't thrown");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(ErrorInfo.UTILS_KIICLIENT_NULL, e.Message);
            }
        }
        #endregion
    }
}

