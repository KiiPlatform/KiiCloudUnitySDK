using System;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents a message for Apple Push Notification Service.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class APNSMessage
    {
        private KiiPushMessageData mData;
        private JsonObject mParent;
        private JsonObject mAlert;

        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.APNSMessage"/> class.
        /// </summary>
        /// <param name="parent">Parent.</param>
        /// <param name="data">Data.</param>
        /// <param name="alert">Alert.</param>
        internal APNSMessage (JsonObject parent, KiiPushMessageData data, JsonObject alert)
        {
            this.mParent = parent;
            this.mData = data;
            this.mAlert = alert;
        }
        /// <summary>
        /// Instantiate GCMMessage builder.
        /// GCM Delivery is enabled by default.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>Builder of the message.</returns>
        public static APNSMessage.Builder CreateBuilder() {
            return new APNSMessage.Builder();
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
        /// <returns>Represents this message.</returns>
        public JsonObject ToJson()
        {
            JsonObject ret = new JsonObject(this.mParent.ToString());
            if (this.mData != null)
            {
                ret.Put ("data", this.mData.ToJsonObject ());
            }
            if (this.mAlert.Length() > 0)
            {
                ret.Put ("alert", this.mAlert);
            }
            return ret;
        }

        #region Inner Classes
        /// <summary>
        /// Builder of APNSMessage.
        /// </summary>
        /// <remarks></remarks>
        public class Builder
        {
            private KiiPushMessageData mData;
            private JsonObject mParent;
            private JsonObject mAlert;

            internal Builder()
            {
                this.mParent = new JsonObject();
                this.mAlert = new JsonObject();
                this.mParent.Put("enabled", true);
            }
            /// <summary>
            /// Build and return APNSMessage.
            /// </summary>
            /// <remarks></remarks>
            /// <returns>APNSMessage</returns>
            public APNSMessage Build() {
                return new APNSMessage(this.mParent, this.mData, this.mAlert);
            }
            /// <summary>
            /// Set flag of APNS delivery.
            /// If omit calling this method, APNS delivery is enabled.
            /// If <see cref="KiiPushMessage.Builder.EnableAPNS(bool)"/> called after <see cref="KiiPushMessage.Builder.WithAPNSMessage(GCMMessage)"/>,
            /// This property would be overwritten. 
            /// (The converse also overwrite the property.)
            /// </summary>
            /// <remarks></remarks>
            /// <param name="enabled">if true message would be delivered via APNS. If false, this message would not delivered thru APNS.</param>
            /// <returns>Builder of the message.</returns>
            public Builder Enable(bool enabled) {
                this.mParent.Put("enabled", enabled);
                return this;
            }
            /// <summary>
            /// Create builder with Data that will be sent only to iOS-APNS devices.
            /// Corresponding to APNS's custom payload.
            /// The data specified here will be merged with the data specified on <see cref="KiiPushMessage.BuildWith(KiiPushMessage.Data)"/>
            /// </summary>
            /// <remarks></remarks>
            /// <param name="data">APNS specific data.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithAPNSData(APNSData data) {
                if (data == null)
                {
                    throw new ArgumentNullException("data must not be null");
                }
                this.mData = data;
                return this;
            }
            /// <summary>
            /// If provided, it will be used as the "sound" to be sent with the notification.
            /// Corresponding to APNS's "sound" in aps notification payload.
            /// </summary>
            /// <remarks></remarks>
            /// <param name="sound">The name of a sound file in the application bundle.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithSound(String sound) {
                if (sound == null)
                {
                    throw new ArgumentNullException("sound must not be null");
                }
                this.mParent.Put("sound", sound);
                return this;
            }
            /// <summary>
            /// If provided, it will be used as the "badge" to be sent with the notification.
            /// Corresponding to APNS's "badge" in aps notification payload.
            /// </summary>
            /// <remarks></remarks>
            /// <param name="badge">The number to display as the badge of the application icon.</param>
            /// <returns>Builder of the message.</returns>
            public Builder WithBadge(int badge) {
                this.mParent.Put("badge", badge);
                return this;
            }
            /// <summary>
            /// Build with alert action loc key.
            /// Corresponding to APNS's "action-loc-key" in apns.alert
            /// </summary>
            /// <remarks></remarks>
            /// <param name="actionLocKey">String that iOS uses as a key to get localized string in to use for alert "View" button.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithAlertActionLocKey(String actionLocKey) {
                if (actionLocKey == null)
                {
                    throw new ArgumentNullException("actionLocKey must not be null");
                }
                this.mAlert.Put("actionLocKey", actionLocKey);
                return this;
            }
            /// <summary>
            /// Build with alert loc key.
            /// Corresponding to APNS's "action-loc-key" in apns.alert
            /// </summary>
            /// <remarks></remarks>
            /// <param name="locKey">String that used as a key to an alert-message string in a "localizable.strings" file for current localization.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithAlertLocKey(String locKey) {
                if (locKey == null)
                {
                    throw new ArgumentNullException("locKey must not be null");
                }
                this.mAlert.Put("locKey", locKey);
                return this;
            }
            /// <summary>
            /// Build with alert locArgs.
            /// Corresponding to APNS's "loc-args" in apns.alert
            /// </summary>
            /// <remarks></remarks>
            /// <param name="locArgs">Variable string values to appear in place of the format specifiers in locKey.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentException'>
            /// Is thrown when an argument is null or empty.
            /// </exception>
            public Builder WithAlertLocArgs(String[] locArgs) {
                if (locArgs == null || locArgs.Length == 0) {
                    throw new ArgumentException("LocArgs can not be null or empty.");
                }
                JsonArray args = new JsonArray();
                foreach (String s in locArgs) {
                    args.Put(s);
                }
                this.mAlert.Put("locArgs", args);
                return this;
            }
            /// <summary>
            /// Build with alert launch Image.
            /// Corresponding to APNS's "launch-image" in apns.alert
            /// </summary>
            /// <remarks></remarks>
            /// <param name="launchImage">The filename of an image file in the application bundle.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithAlertLaunchImage(String launchImage) {
                if (launchImage == null)
                {
                    throw new ArgumentNullException("launchImage must not be null");
                }
                this.mAlert.Put("launchImage", launchImage);
                return this;
            }
            /// <summary>
            /// Build with alert title.
            /// Corresponding to APNS's "title" in apns.alert
            /// </summary>
            /// <remarks></remarks>
            /// <param name="title">APNS alert title text.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithAlertTitle(String title) {
                if (title == null)
                {
                    throw new ArgumentNullException("title must not be null");
                }
                this.mAlert.Put("title", title);
                return this;
            }
            /// <summary>
            /// Build with alert subtitle.
            /// Corresponding to APNS's "subtitle" in apns.alert
            /// </summary>
            /// <remarks></remarks>
            /// <param name="title">APNS alert subtitle text.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithAlertSubtitle(String subtitle) {
                if (subtitle == null)
                {
                    throw new ArgumentNullException("subtitle must not be null");
                }
                this.mAlert.Put("subtitle", subtitle);
                return this;
            }
            /// <summary>
            /// Build with alert body message.
            /// Corresponding to APNS's "body" in apns.alert
            /// </summary>
            /// <remarks></remarks>
            /// <param name="body">APNS alert message text.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithAlertBody(String body) {
                if (body == null)
                {
                    throw new ArgumentNullException("body must not be null");
                }
                this.mAlert.Put("body", body);
                return this;
            }
            /// <summary>
            /// Build with content-available.
            /// Corresponding to APNs's "content-available" in apns.
            /// For details, please refer to <a href="http://developer.apple.com/library/mac/#documentation/NetworkingInternet/Conceptual/RemoteNotificationsPG/ApplePushService/ApplePushService.html">APNS document</a>
            /// </summary>
            /// <remarks></remarks>
            /// <param name="available">
            /// APNs content-available field number.
            /// If 0, content-available payload is not delivered.
            /// Otherwise, content-available=1 payload is delivered.
            /// </param>
            /// <returns>Builder of the message.</returns>
            public Builder WithContentAvailable(int available)
            {
                this.mParent.Put("contentAvailable", available);
                return this;
            }
            /// <summary>
            /// Build with category.
            /// Corresponding to APNs's "category" in apns.
            /// For details, please refer to <a href="http://developer.apple.com/library/mac/#documentation/NetworkingInternet/Conceptual/RemoteNotificationsPG/ApplePushService/ApplePushService.html">APNS document</a>
            /// </summary>
            /// <remarks></remarks>
            /// <param name="category">
            /// APNs category field string.
            /// </param>
            /// <returns>Builder of the message.</returns>
            public Builder WithCategory(string category)
            {
                this.mParent.Put("category", category);
                return this;
            }
            /// <summary>
            /// Build with mutable-content.
            /// Corresponding to APNs's "mutable-content" in apns.
            /// </summary>
            /// <remarks></remarks>
            /// <param name="mutableContent">
            /// APNs mutable-content field number.
            /// </param>
            /// <returns>Builder of the message.</returns>
            public Builder WithMutableContent(int mutableContent)
            {
                this.mParent.Put("mutableContent", mutableContent);
                return this;
            }
        }
        #endregion
    }
}

