using System;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents Geo Point.
    /// </summary>
    /// <remarks>
    /// This class is used to set <see cref="KiiObject"/> with geo spatial data.
    /// </remarks>
    public struct KiiGeoPoint
    {
        private double latitude;
        private double longitude;



        /// <summary>
        /// Create KiiGeoPoint with given latitude and longitude.
        /// </summary>
        /// <remarks>
        /// If latitude/longitude is out of dange, an exception will be thrown.
        /// </remarks>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid.
        /// </exception>
        /// <param name='latitude'>
        /// Latitude of the point in degrees. Valid if the value is greater than -90 degrees and less than +90 degrees.
        /// </param>
        /// <param name='longitude'>
        /// Longitude of the point in degrees. Valid if the value is greater than -90 degrees and less than +90 degrees.
        /// </param>
        public KiiGeoPoint (double latitude, double longitude)
        {
            if (!IsValueInRange (latitude, -90, +90)) {
                throw new ArgumentException ("Given latitude is out of range");
            }
            if (!IsValueInRange (longitude, -180, +180)) {
                throw new ArgumentException ("Given longitude is out of range");
            }
            this.latitude = latitude;
            this.longitude = longitude;
        }
        /// <summary>
        /// Determines whether the specified <see cref="KiiGeoPoint"/> is equal to the current <see cref="KiiCorp.Cloud.Storage.KiiGeoPoint"/>.
        /// </summary>
        /// <param name='other'>
        /// The <see cref="KiiGeoPoint"/> to compare with the current <see cref="KiiCorp.Cloud.Storage.KiiGeoPoint"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="KiiGeoPoint"/> is equal to the current
        /// <see cref="KiiCorp.Cloud.Storage.KiiGeoPoint"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals (KiiGeoPoint other)
        {
            return (other.Longitude == this.longitude && other.Latitude == this.latitude);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="KiiCorp.Cloud.Storage.KiiGeoPoint"/> object.
        /// </summary>
        /// <returns>
        /// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.
        /// </returns>
        public override int GetHashCode ()
        {
            unchecked
            {
                Double lat = this.latitude;
                Double lon = this.longitude;
                return (37 * lat.GetHashCode ()) + lon.GetHashCode ();
            }
        }
        #region properties
        /// <summary>
        /// Gets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        ///
        public double Latitude {
            get {
                return this.latitude;
            }
        }

        /// <summary>
        /// Gets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public double Longitude {
            get {
                return this.longitude;
            }
        }
        #endregion

        private static bool IsValueInRange (double val, double min, double max)
        {
            return (!Double.IsNaN (val) && val > min && val < max) ;
        }

        internal JsonObject ToJson ()
        {
            JsonObject obj = new JsonObject ();
            obj.Put ("lat", this.latitude);
            obj.Put ("lon", this.longitude);
            obj.Put ("_type", "point");
            return obj;
        }

        internal static KiiGeoPoint GeoPoint (JsonObject obj)
        {
            string type = obj.GetString("_type");
            if(!type.Equals("point")){
                throw new IllegalKiiBaseObjectFormatException("Invalid geo point object.");
            }
            double lat =obj.GetDouble("lat");
            double lon =obj.GetDouble("lon");
            return new KiiGeoPoint (lat, lon);
        }
    }
}

