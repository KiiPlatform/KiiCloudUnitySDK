using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents data to be delivered to each device via Apple Push Notification Service.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class APNSData : KiiPushMessageData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.APNSData"/> class.
        /// </summary>
        /// <remarks></remarks>
        public APNSData() : base()
        {
        }
        internal static new bool ValidateKey(string key)
        {
            return KiiPushMessageData.ValidateKey (key);
        }
    }
}

