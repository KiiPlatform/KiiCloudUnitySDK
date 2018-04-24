using System;
using NUnit.Framework;
using JsonOrg;
namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiClause_geo
    {
        [Test(), KiiUTInfo(
            action = "When we call GeoBox() with valid parameters",
            expected = "We can get geobox clause(see assertion)"
            )]
        public void Test_0001_GeoBox_Valid_Parameters ()
        {
            KiiGeoPoint northEast = new KiiGeoPoint(70.00,100);
            KiiGeoPoint southWest = new KiiGeoPoint(71.00,102);

            KiiClause c = KiiClause.GeoBox("box",northEast,southWest);
            JsonObject clause = c.ToJson();
            JsonObject box = clause.GetJsonObject("box");
            JsonObject ne = box.GetJsonObject("ne");
            JsonObject sw = box.GetJsonObject("sw");
            Assert.AreEqual(clause.GetString("type"),"geobox");
            Assert.AreEqual(clause.GetString("field"),"box");
            Assert.AreEqual(ne.GetDouble("lat"),northEast.Latitude);
            Assert.AreEqual(ne.GetDouble("lon"),northEast.Longitude);
            Assert.AreEqual(sw.GetDouble("lat"),southWest.Latitude);
            Assert.AreEqual(sw.GetDouble("lon"),southWest.Longitude);

        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GeoBox() with null key",
            expected = "Throw Argument exception"
            )]
        public void Test_0002_GeoBox_null_key ()
        {
            KiiGeoPoint northEast = new KiiGeoPoint(70.00,100);
            KiiGeoPoint southWest = new KiiGeoPoint(71.00,102);

            KiiClause.GeoBox(null,northEast,southWest);

        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GeoBox() with empty key",
            expected = "Throw Argument exception"
            )]
        public void Test_0003_GeoBox_empty_key ()
        {
            KiiGeoPoint northEast = new KiiGeoPoint(70.00,100);
            KiiGeoPoint southWest = new KiiGeoPoint(71.00,102);

            KiiClause.GeoBox("",northEast,southWest);

        }

        [Test(), KiiUTInfo(
            action = "When we call GeoBox() with default North East",
            expected = "Can get GeoBox"
            )]
        public void Test_0004_GeoBox_Valid_key_default_NE ()
        {
             KiiGeoPoint northEast = new KiiGeoPoint();
            KiiGeoPoint southWest = new KiiGeoPoint(71.00,102);

            KiiClause c = KiiClause.GeoBox("box",northEast,southWest);

            JsonObject clause = c.ToJson();
            JsonObject box = clause.GetJsonObject("box");
            JsonObject ne = box.GetJsonObject("ne");
            JsonObject sw = box.GetJsonObject("sw");
            Assert.AreEqual(clause.GetString("type"),"geobox");
            Assert.AreEqual(clause.GetString("field"),"box");
            Assert.AreEqual(ne.GetDouble("lat"),northEast.Latitude);
            Assert.AreEqual(ne.GetDouble("lon"),northEast.Longitude);
            Assert.AreEqual(sw.GetDouble("lat"),southWest.Latitude);
            Assert.AreEqual(sw.GetDouble("lon"),southWest.Longitude);
        }

        [Test(), KiiUTInfo(
            action = "When we call GeoBox() with default South West",
            expected = "Can get geoBox"
            )]
        public void Test_0005_GeoBox_Valid_key_default_SW ()
        {
             KiiGeoPoint northEast = new KiiGeoPoint(70.00,100);
            KiiGeoPoint southWest = new KiiGeoPoint();

            KiiClause c = KiiClause.GeoBox("box",northEast,southWest);

            JsonObject clause = c.ToJson();
            JsonObject box = clause.GetJsonObject("box");
            JsonObject ne = box.GetJsonObject("ne");
            JsonObject sw = box.GetJsonObject("sw");
            Assert.AreEqual(clause.GetString("type"),"geobox");
            Assert.AreEqual(clause.GetString("field"),"box");
            Assert.AreEqual(ne.GetDouble("lat"),northEast.Latitude);
            Assert.AreEqual(ne.GetDouble("lon"),northEast.Longitude);
            Assert.AreEqual(sw.GetDouble("lat"),southWest.Latitude);
            Assert.AreEqual(sw.GetDouble("lon"),southWest.Longitude);
        }

        [Test(), KiiUTInfo(
            action = "When we call GeoDistance() with valid parameters",
            expected = "We can get geodistance clause(see assertion)"
            )]
        public void Test_0006_GeoDistance_Valid_Parameters ()
        {
            KiiGeoPoint center = new KiiGeoPoint(70.00,100);
            double radius = 10.0;
            string key = "currentLocation";
            string putDistanceInto = "calculatedDistance";

            KiiClause c = KiiClause.GeoDistance(key,center,radius,putDistanceInto);
            JsonObject clause = c.ToJson();
            Assert.AreEqual(clause.GetString("type"),"geodistance");
            Assert.AreEqual(clause.GetString("field"),key);
            Assert.AreEqual(clause.GetString("putDistanceInto"),putDistanceInto);
            Assert.AreEqual(clause.GetDouble("radius"),radius);

            JsonObject centerJson = clause.GetJsonObject("center");
            Assert.AreEqual(centerJson.GetDouble("lat"),center.Latitude);
            Assert.AreEqual(centerJson.GetDouble("lon"),center.Longitude);

        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GeoDistance() with null key",
            expected = "Throw Argument exception"
            )]
        public void Test_0007_GeoDistance_null_key ()
        {
            KiiGeoPoint center = new KiiGeoPoint(70.00,100);
            double radius = 10.0;
            string key = null;
            string putDistanceInto = "calculatedDistance";

            KiiClause.GeoDistance(key,center,radius,putDistanceInto);


        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GeoDistance() with empty key",
            expected = "Throw Argument exception"
            )]
        public void Test_0008_GeoDistance_empty_key ()
        {
            KiiGeoPoint center = new KiiGeoPoint(70.00,100);
            double radius = 10.0;
            string key = "";
            string putDistanceInto = "calculatedDistance";

            KiiClause.GeoDistance(key,center,radius,putDistanceInto);


        }

        [Test(), KiiUTInfo(
            action = "When we call GeoDistance() with default center",
            expected = "We can get geodistance clause(see assertion)"
            )]
        public void Test_0009_GeoDistance_Valid_key_default_Center ()
        {
            KiiGeoPoint center = new KiiGeoPoint();
            double radius = 10.0;
            string key = "currentLocation";
            string putDistanceInto = "calculatedDistance";

            KiiClause c = KiiClause.GeoDistance(key,center,radius,putDistanceInto);

            JsonObject clause = c.ToJson();
            Assert.AreEqual(clause.GetString("type"),"geodistance");
            Assert.AreEqual(clause.GetString("field"),key);
            Assert.AreEqual(clause.GetString("putDistanceInto"),putDistanceInto);
            Assert.AreEqual(clause.GetDouble("radius"),radius);

            JsonObject centerJson = clause.GetJsonObject("center");
            Assert.AreEqual(centerJson.GetDouble("lat"),center.Latitude);
            Assert.AreEqual(centerJson.GetDouble("lon"),center.Longitude);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GeoDistance() with invalid radius",
            expected = "Throw Argument exception"
            )]
        public void Test_0010_GeoBox_Valid_key_Valid_Center_Invalid_radius ()
        {
            KiiGeoPoint center = new KiiGeoPoint(70.00,100);
            double radius = -1;
            string key = "currentLocation";
            string putDistanceInto = "calculatedDistance";

            KiiClause.GeoDistance(key,center,radius,putDistanceInto);

        }

    }
}

