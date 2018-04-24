using System;

using JsonOrg;

namespace KiiCorp.Cloud.Analytics
{
    /// <summary>
    /// Contains the snapshot
    /// </summary>
    /// <remarks></remarks>
    public class GroupedSnapShot
    {
        private string name;
        private long pointStart;
        private long pointInterval;
        private JsonArray data;

        internal GroupedSnapShot(string name, long pointStart, long pointInterval, JsonArray data) {
            this.name = name;
            this.pointStart = pointStart;
            this.pointInterval = pointInterval;
            this.data = data;
        }
        
        #region properties
        
        /// <summary>
        /// Gets the name specified with group.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>
        /// The name of key.
        /// </value>
        public string Name
        {
            get
            {
                return name;
            }
        }
        
        /// <summary>
        /// Gets the started time of aggregation procedure.
        /// </summary>
        /// <remarks>
        /// Value is UNIX time in milliseconds.
        /// </remarks>
        /// <value>
        /// The point start.
        /// </value>
        public long PointStart
        {
            get
            {
                return pointStart;
            }
        }
        
        /// <summary>
        /// Gets the interval of aggregation procedure.
        /// </summary>
        /// <remarks>
        /// Value is UNIX time in milliseconds.
        /// </remarks>
        /// <value>
        /// The point interval.
        /// </value>
        public long PointInterval
        {
            get
            {
                return pointInterval;
            }
        }
        
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>
        /// The data.
        /// </value>
        public JsonArray Data
        {
            get
            {
                return data;
            }
        }
        
        #endregion
    }
}

