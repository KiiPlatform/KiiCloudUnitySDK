using System;
using System.Collections.Generic;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents a message for Google Cloud Messaging.
    /// </summary>
    /// <remarks></remarks>
    public class GCMMessage
    {
        private KiiPushMessageData mData;
        private JsonObject mParent;

        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.GCMMessage"/> class.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="parent">Parent.</param>
        internal GCMMessage (KiiPushMessageData data, JsonObject parent)
        {
            this.mData = data;
            this.mParent = parent;
        }
        /// <summary>
        /// Returns result of <see cref="ToJson()"/> as string.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>JSON string.</returns>
        public override String ToString()
        {
            return ToJson().ToString();
        }
        /// <summary>
        /// Get JSONObjcet representation of this message.
        /// Operation of returned JSON object won't affect this object.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>Represents this message..</returns>
        public JsonObject ToJson()
        {
            JsonObject ret = new JsonObject(this.mParent.ToString());
            if (this.mData != null)
            {
                ret.Put ("data", this.mData.ToJsonObject ());
            }
            return ret;
        }
        /// <summary>
        /// Instantiate GCMMessage builder.
        /// GCM Delivery is enabled by default.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>Builder of the message.</returns>
        public static GCMMessage.Builder CreateBuilder()
        {
            return new GCMMessage.Builder();
        }

        #region Inner Classes
        /// <summary>
        /// Builder of GCMMessage.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public class Builder
        {
            private KiiPushMessageData mMessageData;
            private JsonObject mParent;

            internal Builder()
            {
                this.mParent = new JsonObject();
                this.mParent.Put("enabled", true);
            }

            /// <summary>
            /// Build and return GCMMessage.
            /// </summary>
            /// <remarks></remarks>
            /// <returns>GCMMessage</returns>
            public GCMMessage Build() {
                if(this.mMessageData != null && !GCMData.IsValidData(this.mMessageData)) {
                    throw new SystemException ("Data contains key that is GCM reserve word");
                }
                return new GCMMessage(this.mMessageData, this.mParent);
            }
            /// <summary>
            /// Set flag of GCM delivery
            /// If omit calling this method, GCM delivery is enabled.
            /// If <see cref="KiiPushMessage.Builder.EnableGCM(bool)"/>, This property would be overwritten. 
            /// </summary>
            /// <remarks></remarks>
            /// <param name="enabled">if true message would be delivered via GCM. If false, this message would not delivered thru GCM.</param>
            /// <returns>Builder of the message.</returns>
            public Builder Enable(bool enabled)
            {
                this.mParent.Put("enabled", enabled);
                return this;
            }
            /// <summary>
            /// Create a builder with the data that will be sent only to android-GCM devices.
            /// Corresponding to GCM's "data"
            /// The data specified here will be merged with the data specified on <see cref="KiiPushMessage.BuildWith(KiiPushMessage.Data)"/>
            /// </summary>
            /// <remarks></remarks>
            /// <param name="data">Data.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithGCMData(GCMData data)
            {
                if (data == null)
                {
                    throw new ArgumentNullException("data must not be null");
                }
                this.mMessageData = data;
                return this;
            }
            /// <summary>
            /// Send restricted package name which is a string containing the package name of the application.
            /// When set, messages will only be sent to registration IDs that match the package name.
            /// Corresponding to GCM's "restricted_package_name"
            /// </summary>
            /// <remarks></remarks>
            /// <param name="restrictedPackageName">Restricted package name.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithRestrictedPackageName(string restrictedPackageName) {
                if (restrictedPackageName == null)
                {
                    throw new ArgumentNullException("restrictedPackageName must not be null");
                }
                this.mParent.Put("restrictedPackageName", restrictedPackageName);
                return this;
            }
            /// <summary>
            /// Indicates how long (in seconds) the message should be kept on GCM storage if the device is offline.
            /// Corresponding to GCM's "time_to_live" property.
            /// </summary>
            /// <remarks></remarks>
            /// <param name="timeToLive">time(in seconds).</param>
            /// <returns>Builder of the message.</returns>
            public Builder WithTimeToLive(int timeToLive) {
                this.mParent.Put("timeToLive", timeToLive);
                return this;
            }
            /// <summary>
            /// Build with collapse key.
            /// Send the message with collapse key which is an arbitrary string that
            /// is used to collapse a group of like messages when the device is offline,
            /// so that only the last message gets sent to the client.
            /// Corresponding to GCM's 'collapse_key'
            /// </summary>
            /// <remarks></remarks>
            /// <param name="collapseKey">Arbitrary string that is used as collapse key.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithCollapseKey(String collapseKey) {
                if (collapseKey == null)
                {
                    throw new ArgumentNullException("collapseKey must not be null");
                }
                this.mParent.Put("collapseKey", collapseKey);
                return this;
            }
            /// <summary>
            /// Build with the flag of delay while device is idle.
            /// Corresponding to GCM's 'delay_while_idle'.
            /// </summary>
            /// <remarks></remarks>
            /// <param name="delayWhileIdle">If true, the message will not be sent immediately if the device is idle.</param>
            /// <returns>Builder of the message.</returns>
            public Builder WithDelayWhileIdle(bool delayWhileIdle) {
                this.mParent.Put("delayWhileIdle", delayWhileIdle);
                return this;
            }
        }
        #endregion
    }
}

