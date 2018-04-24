using System;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Used for sending message to topic.
    /// Able to generate common, android/ios specific message.
    /// </summary>
    /// <remarks></remarks>
    public class KiiPushMessage
    {

        private KiiPushMessageData mMessageData;
        private JsonObject mParent;
        private JsonObject mGcm;
        private JsonObject mApns;
        private JsonObject mMqtt;

        private KiiPushMessage (KiiPushMessageData messageData, JsonObject parent, JsonObject gcm, JsonObject apns, JsonObject mqtt)
        {
            this.mMessageData = messageData;
            this.mParent = parent;
            this.mGcm = gcm;
            this.mApns = apns;
            this.mMqtt = mqtt;
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
            JsonObject ret = null;
            ret = new JsonObject(this.mParent.ToString());
            ret.Put("data", this.mMessageData.ToJsonObject());
            ret.Put("gcm", this.mGcm);
            ret.Put("apns", this.mApns);
            ret.Put("mqtt", this.mMqtt);
            return ret;
        }
        /// <summary>
        /// Instantiate message builder with the data.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>Builder of the message.</returns>
        /// <param name="messageData">Message data.</param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null.
        /// </exception>
        public static KiiPushMessage.Builder BuildWith(KiiPushMessageData messageData) {
            if (messageData == null) {
                throw new ArgumentNullException("messageData can not be null");
            }
            return new KiiPushMessage.Builder(messageData);
        }


        #region Inner Classes
        /// <summary>
        /// Builder of the message.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public class Builder
        {
            private KiiPushMessageData mMessageData;
            private JsonObject mParent;
            private JsonObject mGcm;
            private JsonObject mApns;
            private JsonObject mMqtt;

            internal Builder(KiiPushMessageData messageData)
            {
                this.mParent = new JsonObject();
                this.mMessageData = messageData;
                this.mGcm = GCMMessage.CreateBuilder().Build().ToJson();
                this.mApns = APNSMessage.CreateBuilder().Build().ToJson();
                this.mMqtt = MqttMessage.CreateBuilder().Build().ToJson();
            }
            /// <summary>
            /// Build and return KiiPushMessage.
            /// </summary>
            /// <remarks></remarks>
            /// <returns>KiiPushMessage</returns>
            public KiiPushMessage Build() {
                if (this.mGcm.OptBoolean ("enabled"))
                {
                    if (this.mMessageData != null && !GCMData.IsValidData (this.mMessageData))
                    {
                        throw new SystemException ("Data contains key that is GCM reserve word");
                    }
                }
                KiiPushMessage msg = new KiiPushMessage(this.mMessageData, this.mParent, this.mGcm, this.mApns, this.mMqtt);
                return msg;
            }
            /// <summary>
            /// Build with GCM specific message that will be delivered through GCM.
            /// Override the GCM delivery flag if already set by <see cref="GCMMessage"/>
            /// </summary>
            /// <remarks></remarks>
            /// <param name="message">Message.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithGCMMessage(GCMMessage message) {
                if (message == null)
                {
                    throw new ArgumentNullException ("GCMMessage can not be null");
                }
                this.mGcm = message.ToJson();
                return this;
            }
            /// <summary>
            /// Build with APNS specific message that will be delivered through APNS.
            /// Override the APNS delivery flag if already set by <see cref="EnableAPNS(bool)"/>
            /// 
            /// </summary>
            /// <remarks></remarks>
            /// <param name="message">Message.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithAPNSMessage(APNSMessage message) {
                if (message == null)
                {
                    throw new ArgumentNullException ("APNSMessage can not be null");
                }
                this.mApns = message.ToJson();
                return this;
            }
            /// <summary>
            /// Build with MQTT specific message that will be delivered through MQTT.
            /// Override the MQTT delivery flag if already set by <see cref="MqttMessage"/>
            /// </summary>
            /// <remarks></remarks>
            /// <param name="message">Message.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithMqttMessage(MqttMessage message) {
                if (message == null)
                {
                    throw new ArgumentNullException ("MqttMessage can not be null");
                }
                this.mMqtt = message.ToJson();
                return this;
            }
            /// <summary>
            /// Set flag of GCM delivery If omit calling this method, GCM delivery is enabled.
            /// Overwrite the GCM delivery flag in the message JSON if already set by <see cref="WithGCMMessage(GCMMessage)"/> method, GCM delivery flag will be 
            /// </summary>
            /// <remarks></remarks>
            /// <param name="gcmEnabled">if true message would be delivered via GCM. If false, this message would not delivered thru GCM.</param>
            /// <returns>Builder of the message.</returns>
            public Builder EnableGCM(bool gcmEnabled) {
                this.mGcm.Put("enabled", gcmEnabled); 
                return this;
            }
            /// <summary>
            /// Set flag of APNS delivery If omit calling this method, APNS delivery is enabled.
            /// Overwrite APNS delivery flag in the JSON message if already set by <see cref="WithAPNSMessage(APNSMessage)"/>
            /// </summary>
            /// <remarks></remarks>
            /// <param name="apnsEnabled">if true message would be delivered via APNS. If false, this message would not delivered thru APNS.</param>
            /// <returns>Builder of the message.</returns>
            public Builder EnableAPNS(bool apnsEnabled) {
                this.mApns.Put("enabled", apnsEnabled); 
                return this;
            }
            /// <summary>
            /// Set flag of MQTT delivery If omit calling this method, MQTT delivery is enabled.
            /// Overwrite the MQTT delivery flag in the message JSON if already set by <see cref="WithMqttMessage(MqttMessage)"/> method, MQTT delivery flag will be 
            /// </summary>
            /// <remarks></remarks>
            /// <param name="mqttEnabled">if true message would be delivered via MQTT. If false, this message would not delivered thru MQTT.</param>
            /// <returns>Builder of the message.</returns>
            public Builder EnableMqtt(bool mqttEnabled) {
                this.mMqtt.Put("enabled", mqttEnabled); 
                return this;
            }
            /// <summary>
            /// Set the  message type field with the provided value. 
            /// </summary>
            /// <remarks></remarks>
            /// <param name="pushMessageType">Type of the push message.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithPushMessageType(String pushMessageType) {
                if (pushMessageType == null)
                {
                    throw new ArgumentNullException("pushMessageType must not be null");
                }
                this.mParent.Put("pushMessageType", pushMessageType);
                return this;
            }
            /// <summary>
            /// Flags indicates whether "appId" field will be send or not.
            /// </summary>
            /// <remarks></remarks>
            /// <param name="isSendAppId">If true, the appID field will also be sent. Default is false.</param>
            /// <returns>Builder of the message.</returns>
            public Builder SendAppID(bool isSendAppId) {
                this.mParent.Put("sendAppID", isSendAppId);
                return this;
            }
            /// <summary>
            /// Flags indicates whether "sender" field will be send or not.
            /// </summary>
            /// <remarks></remarks>
            /// <param name="sendSender">If true, send the “sender” field which is userID of the user that triggered the notification. Default is true.</param>
            /// <returns>Builder of the message.</returns>
            public Builder SendSender(bool sendSender) {
                this.mParent.Put("sendSender", sendSender);
                return this;
            }
            /// <summary>
            /// Flags indicates whether "when" field will be send or not.
            /// </summary>
            /// <remarks></remarks>
            /// <param name="sendWhen">If true, send the “when” field which denotes the time of push message was sent. Default is false.</param>
            /// <returns>Builder of the message.</returns>
            public Builder SendWhen(bool sendWhen) {
                this.mParent.Put("sendWhen", sendWhen);
                return this;
            }
            /// <summary>
            /// Flags indicates whether "sendOrigin" field will be send or not.
            /// </summary>
            /// <remarks></remarks>
            /// <param name="sendOrigin">If true, send the “origin” field that indicates if the message is the result of an event or sended explicitly by someone. Default is false.</param>
            /// <returns>Builder of the message.</returns>
            public Builder SendOrigin(bool sendOrigin) {
                this.mParent.Put("sendOrigin", sendOrigin);
                return this;
            }
            /// <summary>
            /// Flags indicates whether "objectScope" field will be send or not.
            /// </summary>
            /// <remarks></remarks>
            /// <param name="sendObjectScope">If true, send the “objectScope”-related fields that contain the topic that is the source of this notification. Default is true.</param>
            /// <returns>Builder of the message.</returns>
            public Builder SendObjectScope(bool sendObjectScope) {
                this.mParent.Put("sendObjectScope", sendObjectScope);
                return this;
            }
            /// <summary>
            /// Flags indicates whether "topicID" field will be send or not.
            /// </summary>
            /// <remarks></remarks>
            /// <param name="sendTopicId">If true, send the “topicID” field, which contains the topicID that is the source of this notification. Default is true.</param>
            /// <returns>Builder of the message.</returns>
            public Builder SendTopicId(bool sendTopicId) {
                this.mParent.Put("sendTopicID", sendTopicId);
                return this;
            }
            /// <summary>
            /// Flag indicates whether send to Production installations or not.
            /// Production installations are instantiated by calling <see cref="KiiUser.PushInstallation()"/> or <see cref="KiiUser.PushInstallation(bool)"/> with false.
            /// If omit calling this method, default value is true.
            /// </summary>
            /// <remarks></remarks>
            /// <param name="sendToProduction">If set to <c>true</c> send to production.</param>
            /// <returns>Builder of the message.</returns>
            public Builder SendToProduction(bool sendToProduction) {
                this.mParent.Put("sendToProduction", sendToProduction);
                return this;
            }
            /// <summary>
            /// Flag indicates whether send to Development installations or not.
            /// Development installations are instantiated by calling <see cref="KiiUser.PushInstallation(bool)"/> with true.
            /// If omit calling this method, default value is true.
            /// </summary>
            /// <remarks></remarks>
            /// <param name="sendToDevelopment">If set to <c>true</c> send to development.</param>
            /// <returns>Builder of the message.</returns>
            public Builder SendToDevelopment(bool sendToDevelopment) {
                this.mParent.Put("sendToDevelopment", sendToDevelopment);
                return this;
            }
        }
        #endregion
    }
}

