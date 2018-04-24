using System;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents a message for MQTT Notification Service.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class MqttMessage
    {
        private KiiPushMessageData data;
        private JsonObject parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.MqttMessage"/> class.
        /// </summary>
        /// <param name="parent">Parent.</param>
        /// <param name="data">Data.</param>
        internal MqttMessage(JsonObject parent, KiiPushMessageData data)
        {
            this.parent = parent;
            this.data = data;
        }
        /// <summary>
        /// Instantiate MqttMessage builder.
        /// MQTT Delivery is enabled by default.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>Builder of the message.</returns>
        public static MqttMessage.Builder CreateBuilder() {
            return new MqttMessage.Builder();
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
            JsonObject ret = new JsonObject(this.parent.ToString());
            if (this.data != null)
            {
                ret.Put ("data", this.data.ToJsonObject ());
            }
            return ret;
        }

        #region Inner Classes
        /// <summary>
        /// Builder of MqttMessage.
        /// </summary>
        /// <remarks></remarks>
        public class Builder
        {
            private KiiPushMessageData data;
            private JsonObject parent;

            internal Builder()
            {
                this.parent = new JsonObject();
                this.parent.Put("enabled", true);
            }
            /// <summary>
            /// Build and return MqttMessage.
            /// </summary>
            /// <remarks></remarks>
            /// <returns>MqttMessage</returns>
            public MqttMessage Build() {
                return new MqttMessage(this.parent, this.data);
            }
            /// <summary>
            /// Set flag of MQTT delivery.
            /// If omit calling this method, MQTT delivery is enabled.
            /// </summary>
            /// <remarks></remarks>
            /// <param name="enabled">if true message would be delivered via MQTT. If false, this message would not delivered thru MQTT.</param>
            /// <returns>Builder of the message.</returns>
            public Builder Enable(bool enabled) {
                this.parent.Put("enabled", enabled);
                return this;
            }
            /// <summary>
            /// Create builder with Data that will be sent only to MQTT devices.
            /// Corresponding to MQTT's custom payload.
            /// The data specified here will be merged with the data specified on <see cref="KiiPushMessage.BuildWith(KiiPushMessage.Data)"/>
            /// </summary>
            /// <remarks></remarks>
            /// <param name="data">MQTT specific data.</param>
            /// <returns>Builder of the message.</returns>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null.
            /// </exception>
            public Builder WithMqttData(MqttData data) {
                if (data == null)
                {
                    throw new ArgumentNullException("data must not be null");
                }
                this.data = data;
                return this;
            }
        }
        #endregion

    }
}

