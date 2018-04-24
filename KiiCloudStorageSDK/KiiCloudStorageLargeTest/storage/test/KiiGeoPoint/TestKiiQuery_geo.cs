using System;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiQuery_geo : LargeTestBase
    {
        bool approximateEqual (double expected, double actual, double err)
        {
            double absDiff = Math.Abs (expected - actual);
            return absDiff < err;
        }
        private KiiUser testUser;

        [SetUp]
        public override void SetUp ()
        {
            base.SetUp ();
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            string uname = "kiiqueryTest-" + milliseconds;

            testUser = KiiUser.BuilderWithName (uname).Build ();
            testUser.Register ("pass1234");
            testUser = KiiUser.LogIn (uname, "pass1234");

        }

        [TearDown]
        public override void TearDown ()
        {
            testUser.Delete ();
            base.TearDown ();
        }

        private double distanceInMeter (KiiGeoPoint start, KiiGeoPoint end)
        {
            double lat1 = start.Latitude;
            double lat2 = end.Latitude;
            double lon1 = start.Longitude;
            double lon2 = end.Longitude;
            //degrees to radians
            double lat1rad = lat1 * Math.PI / 180;
            double lon1rad = lon1 * Math.PI / 180;
            double lat2rad = lat2 * Math.PI / 180;
            double lon2rad = lon2 * Math.PI / 180;
    
            //deltas
            double dLat = lat2rad - lat1rad;
            double dLon = lon2rad - lon1rad;

            double a = Math.Sin (dLat / 2) * Math.Sin (dLat / 2) + Math.Sin (dLon / 2) * Math.Sin (dLon / 2) * Math.Cos (lat1rad) * Math.Cos (lat2rad);
            double c = 2 * Math.Asin (Math.Sqrt (a));
            double R = 6371 * 1000;
            return R * c;
        }

        [Test(), KiiUTInfo(
            action = "When we execute KiiQuery with GeoDistance clause,",
            expected = "result should be same with expected"
            )]
        public void Test_0001_GeoDistanceQuery ()
        {
            KiiBucket bucket = testUser.Bucket ("aBucket");
            KiiObject obj = bucket.NewKiiObject ();
            KiiGeoPoint point = new KiiGeoPoint (35.667983, 139.739356);
            obj.SetGeoPoint ("myLoc", point);
            obj.Save ();

            KiiGeoPoint center = new KiiGeoPoint (35.677379, 139.702148);
            KiiClause clause = KiiClause.GeoDistance ("myloc", center, 4000, "distanceToMyLoc");
            KiiQuery query = new KiiQuery (clause);
            KiiQueryResult<KiiObject> result = bucket.Query (query);
            KiiObject retObj = result [0];
            KiiGeoPoint retPoint;
            retPoint = retObj.GetGeoPoint ("myLoc");
            Assert.AreEqual (point.Latitude, retPoint.Latitude);
            Assert.AreEqual (point.Longitude, retPoint.Longitude);
            JsonObject jObj = retObj.GetJsonObject ("_calculated");
            double retDistance = jObj.GetDouble ("distanceToMyLoc");
            double expectedDistance = distanceInMeter (point, center);
            Assert.IsTrue (approximateEqual (expectedDistance, retDistance, 0.00001));

        }

        [Test(), KiiUTInfo(
            action = "When we execute KiiQuery with GeoDistance clause,",
            expected = "result should be same with expected"
            )]
        public void Test_0002_GeoDistanceQuery_calculatedDistance_nil ()
        {
            KiiBucket bucket = testUser.Bucket ("aBucket");
            KiiObject obj = bucket.NewKiiObject ();
            KiiGeoPoint point = new KiiGeoPoint (35.667983, 139.739356);
            obj.SetGeoPoint ("myLoc", point);
            obj.Save ();

            KiiGeoPoint center = new KiiGeoPoint (35.677379, 139.702148);
            KiiClause clause = KiiClause.GeoDistance ("myloc", center, 4000, "");
            KiiQuery query = new KiiQuery (clause);
            KiiQueryResult<KiiObject> result = bucket.Query (query);
            KiiObject retObj = result [0];
            KiiGeoPoint retPoint;
            retPoint = retObj.GetGeoPoint ("myLoc");
            Assert.AreEqual (point.Latitude, retPoint.Latitude);
            Assert.AreEqual (point.Longitude, retPoint.Longitude);

            Assert.IsFalse (retObj.Has ("_calculated"));
        }

        [Test(), KiiUTInfo(
            action = "When we execute KiiQuery with GeoDistance clause,",
            expected = "result should be same with expected"
            )]
        public void Test_0004_GeoDistanceQuery_not_sorted ()
        {
            KiiBucket bucket = testUser.Bucket ("aBucket");
            KiiObject obj1 = bucket.NewKiiObject ();
            KiiGeoPoint point1 = new KiiGeoPoint (35.672568, 139.723606);
            obj1.SetGeoPoint ("myLoc", point1);
            obj1.Save ();

            KiiObject obj2 = bucket.NewKiiObject ();
            KiiGeoPoint point2 = new KiiGeoPoint (35.667983, 139.739356);
            obj2.SetGeoPoint ("myLoc", point2);
            obj2.Save ();

            // not in radius
            KiiObject obj3 = bucket.NewKiiObject ();
            KiiGeoPoint point3 = new KiiGeoPoint ();
            obj3.SetGeoPoint ("myLoc", point3);
            obj3.Save ();

            KiiGeoPoint center = new KiiGeoPoint (35.677379, 139.702148);
            KiiClause clause = KiiClause.GeoDistance ("myloc", center, 4000, "distanceToMyLoc");
            KiiQuery query = new KiiQuery (clause);


            KiiQueryResult<KiiObject> result = bucket.Query (query);
            Assert.AreEqual (result.Count, 2);
            KiiObject retObj1 = result [0];
            KiiGeoPoint retPoint1 = retObj1.GetGeoPoint ("myLoc");
            Assert.AreEqual (point1.Latitude, retPoint1.Latitude);
            Assert.AreEqual (point1.Longitude, retPoint1.Longitude);
            JsonObject jObj1 = retObj1.GetJsonObject ("_calculated");

            KiiObject retObj2 = result [1];
            KiiGeoPoint retPoint2 = retObj2.GetGeoPoint ("myLoc");
            Assert.AreEqual (point2.Latitude, retPoint2.Latitude);
            Assert.AreEqual (point2.Longitude, retPoint2.Longitude);
            JsonObject jObj2 = retObj2.GetJsonObject ("_calculated");

            double retDistance1 = jObj1.GetDouble ("distanceToMyLoc");
            double retDistance2 = jObj2.GetDouble ("distanceToMyLoc");
            double expectedDistance1 = distanceInMeter (point1, center);
            double expectedDistance2 = distanceInMeter (point2, center);

            Assert.IsTrue (approximateEqual (expectedDistance1, retDistance1, 0.00001));
            Assert.IsTrue (approximateEqual (expectedDistance2, retDistance2, 0.00001));
        }

        [Test(), KiiUTInfo(
            action = "When we execute KiiQuery with GeoDistance clause,",
            expected = "result should be same with expected"
            )]
        public void Test_0004_GeoDistanceQuery_sort_asc ()
        {
            KiiBucket bucket = testUser.Bucket ("aBucket");
            KiiObject obj1 = bucket.NewKiiObject ();
            KiiGeoPoint point1 = new KiiGeoPoint (35.672568, 139.723606);
            obj1.SetGeoPoint ("myLoc", point1);
            obj1.Save ();

            KiiObject obj2 = bucket.NewKiiObject ();
            KiiGeoPoint point2 = new KiiGeoPoint (35.667983, 139.739356);
            obj2.SetGeoPoint ("myLoc", point2);
            obj2.Save ();
            // not in radius
            KiiObject obj3 = bucket.NewKiiObject ();
            KiiGeoPoint point3 = new KiiGeoPoint ();
            obj3.SetGeoPoint ("myLoc", point3);
            obj3.Save ();

            KiiGeoPoint center = new KiiGeoPoint (35.677379, 139.702148);
            KiiClause clause = KiiClause.GeoDistance ("myloc", center, 4000, "distanceToMyLoc");
            KiiQuery query = new KiiQuery (clause);
            query.SortByAsc ("_calculated.distanceToMyLoc");

            KiiQueryResult<KiiObject> result = bucket.Query (query);
            Assert.AreEqual (result.Count, 2);
            KiiObject retObj1 = result [0];
            KiiGeoPoint retPoint1 = retObj1.GetGeoPoint ("myLoc");
            Assert.AreEqual (point1.Latitude, retPoint1.Latitude);
            Assert.AreEqual (point1.Longitude, retPoint1.Longitude);
            JsonObject jObj1 = retObj1.GetJsonObject ("_calculated");

            KiiObject retObj2 = result [1];
            KiiGeoPoint retPoint2 = retObj2.GetGeoPoint ("myLoc");
            Assert.AreEqual (point2.Latitude, retPoint2.Latitude);
            Assert.AreEqual (point2.Longitude, retPoint2.Longitude);
            JsonObject jObj2 = retObj2.GetJsonObject ("_calculated");

            double retDistance1 = jObj1.GetDouble ("distanceToMyLoc");
            double retDistance2 = jObj2.GetDouble ("distanceToMyLoc");
            double expectedDistance1 = distanceInMeter (point1, center);
            double expectedDistance2 = distanceInMeter (point2, center);
            Assert.IsTrue (approximateEqual (expectedDistance1, retDistance1, 0.00001));
            Assert.IsTrue (approximateEqual (expectedDistance2, retDistance2, 0.00001));

        }

        [Test(), KiiUTInfo(
            action = "When we execute KiiQuery with GeoBox clause,",
            expected = "result should be same with expected"
            )]
        public void Test_0005_GeoBoxQuery ()
        {
            KiiBucket bucket = testUser.Bucket ("aBucket");
            KiiObject obj = bucket.NewKiiObject ();
            KiiGeoPoint point = new KiiGeoPoint (35.667983, 139.739356);
            obj.SetGeoPoint ("myloc", point);
            obj.Save ();
            //not in the box
            KiiObject obj2 = bucket.NewKiiObject ();
            KiiGeoPoint point2 = new KiiGeoPoint ();
            obj2.SetGeoPoint ("myloc", point2);
            obj2.Save ();

            KiiGeoPoint sw = new KiiGeoPoint (35.52105, 139.699402);
            KiiGeoPoint ne = new KiiGeoPoint (36.069082, 140.07843);

            KiiClause clause = KiiClause.GeoBox ("myloc", ne, sw);
            KiiQuery query = new KiiQuery (clause);
            KiiQueryResult<KiiObject> result = bucket.Query (query);
            Assert.AreEqual (result.Count, 1);
            KiiObject retObj = result [0];
            KiiGeoPoint retPoint;
            retPoint = retObj.GetGeoPoint ("myloc");
            Assert.AreEqual (point.Latitude, retPoint.Latitude);
            Assert.AreEqual (point.Longitude, retPoint.Longitude);

        }

    }
}

