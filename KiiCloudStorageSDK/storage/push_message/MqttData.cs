using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents data to be delivered to each device via MQTT Notification Service.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class MqttData : KiiPushMessageData
    {
        public MqttData() : base()
        {
        }
        internal static new bool ValidateKey(string key)
        {
            return KiiPushMessageData.ValidateKey (key);
        }
    }
}

