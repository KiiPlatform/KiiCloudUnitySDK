using System;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestNotQuery : LargeTestBase
    {
        [SetUp()]
        public override void SetUp ()
        {
            base.SetUp ();
            string uname = "kiiqueryTest-" + CurrentTimeMillis ();
            AppUtil.CreateNewUser (uname, "123456");
        }
        private long CurrentTimeMillis ()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
        [Test()]
        public void NotEqualsTest()
        {
            KiiBucket bucket = KiiUser.CurrentUser.Bucket("my_bucket");
            KiiObject obj1 = bucket.NewKiiObject();
            obj1["name"] = "foo";
            obj1.Save();
            KiiObject obj2 = bucket.NewKiiObject();
            obj2["name"] = "bar";
            obj2.Save();
            KiiObject obj3 = bucket.NewKiiObject();
            obj3["name"] = "hoge";
            obj3.Save();

            KiiQuery query = new KiiQuery(KiiClause.Not(KiiClause.Equals("name", "foo")));
            query.SortByAsc("name");

            KiiQueryResult<KiiObject> results = bucket.Query(query);
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("bar", results[0].GetString("name"));
            Assert.AreEqual("hoge", results[1].GetString("name"));
        }
        [Test()]
        public void NotNotEqualsTest()
        {
            KiiBucket bucket = KiiUser.CurrentUser.Bucket("my_bucket");
            KiiObject obj1 = bucket.NewKiiObject();
            obj1["name"] = "foo";
            obj1.Save();
            KiiObject obj2 = bucket.NewKiiObject();
            obj2["name"] = "bar";
            obj2.Save();
            KiiObject obj3 = bucket.NewKiiObject();
            obj3["name"] = "hoge";
            obj3.Save();

            KiiQuery query = new KiiQuery(KiiClause.Not(KiiClause.NotEquals("name", "foo")));
            query.SortByAsc("name");

            KiiQueryResult<KiiObject> results = bucket.Query(query);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("foo", results[0].GetString("name"));
        }
        [Test()]
        public void NotPrefixTest()
        {
            KiiBucket bucket = KiiUser.CurrentUser.Bucket("my_bucket");
            KiiObject obj1 = bucket.NewKiiObject();
            obj1["name"] = "foo";
            obj1.Save();
            KiiObject obj2 = bucket.NewKiiObject();
            obj2["name"] = "fool";
            obj2.Save();
            KiiObject obj3 = bucket.NewKiiObject();
            obj3["name"] = "hoge";
            obj3.Save();

            KiiQuery query = new KiiQuery(KiiClause.Not(KiiClause.StartsWith("name", "foo")));
            query.SortByAsc("name");

            KiiQueryResult<KiiObject> results = bucket.Query(query);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("hoge", results[0].GetString("name"));
        }
        [Test()]
        public void NotInTest()
        {
            KiiBucket bucket = KiiUser.CurrentUser.Bucket("my_bucket");
            KiiObject obj1 = bucket.NewKiiObject();
            obj1["name"] = "foo";
            obj1.Save();
            KiiObject obj2 = bucket.NewKiiObject();
            obj2["name"] = "bar";
            obj2.Save();
            KiiObject obj3 = bucket.NewKiiObject();
            obj3["name"] = "hoge";
            obj3.Save();

            KiiQuery query = new KiiQuery(KiiClause.Not(KiiClause.InWithStringValue("name", "foo", "bar")));
            query.SortByAsc("name");

            KiiQueryResult<KiiObject> results = bucket.Query(query);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("hoge", results[0].GetString("name"));
        }
        [Test()]
        public void NotRangeTest()
        {
            KiiBucket bucket = KiiUser.CurrentUser.Bucket("my_bucket");
            KiiObject obj1 = bucket.NewKiiObject();
            obj1["name"] = "foo";
            obj1["age"] = 20;
            obj1.Save();
            KiiObject obj2 = bucket.NewKiiObject();
            obj2["name"] = "bar";
            obj2["age"] = 33;
            obj2.Save();
            KiiObject obj3 = bucket.NewKiiObject();
            obj3["name"] = "hoge";
            obj3["age"] = 38;
            obj3.Save();

            KiiQuery query = new KiiQuery(KiiClause.Not(KiiClause.GreaterThan("age", 30)));
            query.SortByAsc("name");

            KiiQueryResult<KiiObject> results = bucket.Query(query);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("foo", results[0].GetString("name"));
        }
        [Test()]
        public void NotHasFieldTest()
        {
            KiiBucket bucket = KiiUser.CurrentUser.Bucket("my_bucket");
            KiiObject obj1 = bucket.NewKiiObject();
            obj1["name"] = "foo";
            obj1["age"] = 20;
            obj1.Save();
            KiiObject obj2 = bucket.NewKiiObject();
            obj2["name"] = "bar";
            obj2["age"] = 33;
            obj2.Save();
            KiiObject obj3 = bucket.NewKiiObject();
            obj3["name"] = "hoge";
            obj3.Save();

            KiiQuery query = new KiiQuery(KiiClause.Not(KiiClause.HasField("age", FieldType.INTEGER)));
            query.SortByAsc("name");

            KiiQueryResult<KiiObject> results = bucket.Query(query);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("hoge", results[0].GetString("name"));
        }
        [Test()]
        public void NotGeoDistanceTest()
        {
            KiiBucket bucket = KiiUser.CurrentUser.Bucket("my_bucket");
            KiiObject obj1 = bucket.NewKiiObject();
            obj1["name"] = "kii";
            obj1.SetGeoPoint("location", new KiiGeoPoint(35.668387, 139.739495));
            obj1.Save();
            KiiObject obj2 = bucket.NewKiiObject();
            obj2["name"] = "TDL";
            obj2.SetGeoPoint("location", new KiiGeoPoint(35.633114, 139.880405));
            obj2.Save();

            KiiQuery query = new KiiQuery(KiiClause.Not(KiiClause.GeoDistance("location", new KiiGeoPoint(35.667384, 139.739994), 2000, null)));
            query.SortByAsc("name");

            KiiQueryResult<KiiObject> results = bucket.Query(query);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("TDL", results[0].GetString("name"));
        }
        [Test()]
        public void NotGeoBoxTest()
        {
            KiiBucket bucket = KiiUser.CurrentUser.Bucket("my_bucket");
            KiiObject obj1 = bucket.NewKiiObject();
            obj1["name"] = "kii";
            obj1.SetGeoPoint("location", new KiiGeoPoint(35.668387, 139.739495));
            obj1.Save();
            KiiObject obj2 = bucket.NewKiiObject();
            obj2["name"] = "TDL";
            obj2.SetGeoPoint("location", new KiiGeoPoint(35.633114, 139.880405));
            obj2.Save();

            KiiQuery query = new KiiQuery(KiiClause.Not(KiiClause.GeoBox("location", new KiiGeoPoint(35.669494, 139.741727), new KiiGeoPoint(35.660934, 139.734957))));
            query.SortByAsc("name");

            KiiQueryResult<KiiObject> results = bucket.Query(query);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("TDL", results[0].GetString("name"));
        }
        [Test()]
        public void NotOrTest()
        {
            KiiBucket bucket = KiiUser.CurrentUser.Bucket("my_bucket");
            KiiObject obj1 = bucket.NewKiiObject();
            obj1["name"] = "foo";
            obj1.Save();
            KiiObject obj2 = bucket.NewKiiObject();
            obj2["name"] = "bar";
            obj2.Save();
            KiiObject obj3 = bucket.NewKiiObject();
            obj3["name"] = "hoge";
            obj3.Save();

            KiiQuery query = new KiiQuery(KiiClause.Not(KiiClause.Or(KiiClause.Equals("name", "foo"), KiiClause.Equals("name", "bar"))));
            query.SortByAsc("name");

            KiiQueryResult<KiiObject> results = bucket.Query(query);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("hoge", results[0].GetString("name"));
        }
        [Test()]
        public void NotAndTest()
        {
            KiiBucket bucket = KiiUser.CurrentUser.Bucket("my_bucket");
            KiiObject obj1 = bucket.NewKiiObject();
            obj1["name"] = "foo";
            obj1["age"] = 20;
            obj1.Save();
            KiiObject obj2 = bucket.NewKiiObject();
            obj2["name"] = "foo";
            obj2["age"] = 33;
            obj2.Save();

            KiiQuery query = new KiiQuery(KiiClause.Not(KiiClause.And(KiiClause.Equals("name", "foo"), KiiClause.LessThan("age", 30))));
            query.SortByAsc("name");

            KiiQueryResult<KiiObject> results = bucket.Query(query);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(33, results[0].GetInt("age"));
        }
    }
}

