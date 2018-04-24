using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiGeoPoint
    {
         [Test(), KiiUTInfo(
            action = "When we call GeoPoint() with valid parameters",
            expected = "We can get GeoPoint struct (see assertion)"
            )]
        public void Test_0001_GeoPoint_Valid_Parameters ()
        {
            KiiGeoPoint location = new KiiGeoPoint(71.00,102);

            Assert.AreEqual("{\"lat\":71,\"lon\":102,\"_type\":\"point\"}", location.ToJson().ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call GeoPoint() ",
            expected = "We can get GeoPoint struct (see assertion)"
            )]
        public void Test_0002_GeoPoint_Default_Constructor ()
        {
            KiiGeoPoint location = new KiiGeoPoint();

            Assert.AreEqual("{\"lat\":-0,\"lon\":-0,\"_type\":\"point\"}", location.ToJson().ToString());
        }
       [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GeoPoint() with invalid lat",
            expected = "Throw Argument exception"
            )]
        public void Test_0003_GeoPoint_lat_invalid_lon_valid ()
        {
            new KiiGeoPoint(-100.00,100.00);

        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GeoPoint() with invalid lon",
            expected = "Throw Argument exception"
            )]
        public void Test_0004_GeoPoint_lat_valid_lon_invalid ()
        {
            new KiiGeoPoint(-10.00,200.00);

        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call GeoPoint() with invalid lat and lon",
            expected = "Throw Argument exception"
            )]
        public void Test_0005_GeoPoint_lat_invalid_lon_invalid ()
        {
            new KiiGeoPoint(-100.00,200.00);

        }
    }
}

