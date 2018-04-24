using System;
using System.Collections.Generic;

using JsonOrg;

namespace KiiCorp.Cloud.Analytics
{
    /// <summary>
    /// Contains the result of Analytics.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class GroupedResult
    {
        IList<GroupedSnapShot> mSnapshots;
        
        internal GroupedResult (IList<GroupedSnapShot> snapshots)
        {
            this.mSnapshots = snapshots;
        }
        
        /// <summary>
        /// Gets the list of SnapShots.
        /// </summary>
        /// <remarks></remarks>
        /// <value>
        /// The list of SnapShots.
        /// </value>
        public IList<GroupedSnapShot> SnapShots
        {
            get
            {
                return mSnapshots;
            }
        }
        
        internal static GroupedResult Parse(JsonArray array)
        {
            IList<GroupedSnapShot> snapshots = new List<GroupedSnapShot>();
            for (int i = 0; i < array.Length(); i++) 
            {
                JsonObject json = array.GetJsonObject(i);
                string name        = json.GetString("name");
                long pointInterval = json.GetLong("pointInterval");
                long pointStart    = json.GetLong("pointStart");
                JsonArray data     = json.GetJsonArray("data");
                
                snapshots.Add(new GroupedSnapShot(name, pointStart, pointInterval, data));
            }
            return new GroupedResult(snapshots);
        }
    }
}

