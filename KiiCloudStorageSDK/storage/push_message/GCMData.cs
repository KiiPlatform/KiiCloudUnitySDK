using System;
using System.Collections.Generic;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents data to be delivered to each device via Google Cloud Messaging.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class GCMData : KiiPushMessageData
    {
        private static readonly string[] GCM_RESERVE_KEYS = {
            "from",
            "registration_ids",
            "collapse_key",
            "data",
            "delay_while_idle",
            "time_to_live",
            "restricted_package_name",
            "dry_run"
        };
        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.GCMData"/> class.
        /// </summary>
        /// <remarks></remarks>
        public GCMData() : base()
        {
        }
        /// <summary>
        /// Put Int value to the data.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="key">When the same value specified, overwrite existing value.</param>
        /// <param name="value">String value.</param>
        /// <returns>This instance.</returns>
        /// <exception cref='ArgumentException'>
        /// Is thrown when the specified key is null or empty or reserved word.
        /// For details, please refer to <a href="https://developer.android.com/google/gcm/server.html#params">GCM document</a>
        /// </exception>
        public override KiiPushMessageData Put(string key, int value)
        {
            return base.Put(key, value);
        }
        /// <summary>
        /// Put Long value to the data.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="key">When the same value specified, overwrite existing value.</param>
        /// <param name="value">String value.</param>
        /// <returns>This instance.</returns>
        /// <exception cref='ArgumentException'>
        /// Is thrown when the specified key is null or empty or reserved word.
        /// For details, please refer to <a href="https://developer.android.com/google/gcm/server.html#params">GCM document</a>
        /// </exception>
        public override KiiPushMessageData Put(string key, long value)
        {
            return base.Put(key, value);
        }
        /// <summary>
        /// Put Double value to the data.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="key">When the same value specified, overwrite existing value.</param>
        /// <param name="value">String value.</param>
        /// <returns>This instance.</returns>
        /// <exception cref='ArgumentException'>
        /// Is thrown when the specified key is null or empty or reserved word.
        /// For details, please refer to <a href="https://developer.android.com/google/gcm/server.html#params">GCM document</a>
        /// </exception>
        public override KiiPushMessageData Put(string key, double value)
        {
            return base.Put(key, value);
        }
        /// <summary>
        /// Put Bool value to the data.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="key">When the same value specified, overwrite existing value.</param>
        /// <param name="value">String value.</param>
        /// <returns>This instance.</returns>
        /// <exception cref='ArgumentException'>
        /// Is thrown when the specified key is null or empty or reserved word.
        /// For details, please refer to <a href="https://developer.android.com/google/gcm/server.html#params">GCM document</a>
        /// </exception>
        public override KiiPushMessageData Put(string key, bool value)
        {
            return base.Put(key, value);
        }
        /// <summary>
        /// Put String value to the data.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="key">When the same value specified, overwrite existing value.</param>
        /// <param name="value">String value.</param>
        /// <returns>This instance.</returns>
        /// <exception cref='ArgumentException'>
        /// Is thrown when the specified key is null or empty or reserved word.
        /// For details, please refer to <a href="https://developer.android.com/google/gcm/server.html#params">GCM document</a>
        /// </exception>
        public override KiiPushMessageData Put(string key, string value)
        {
            return base.Put(key, value);
        }
        /// <summary>
        /// Determines whether this instance is valid key the specified key.
        /// </summary>
        /// <remarks></remarks>
        /// <returns><c>true</c> if this instance is valid key the specified key; otherwise, <c>false</c>.</returns>
        /// <param name="key">Key.</param>
        protected override bool IsValidKey(string key)
        {
            return ValidateKey (key);
        }
        internal static new bool ValidateKey(string key)
        {
            if (!KiiPushMessageData.ValidateKey (key))
            {
                return false;
            }
            if (key.StartsWith ("google"))
            {
                return false;
            }
            foreach (String reserveKey in GCM_RESERVE_KEYS) {
                if(reserveKey == key.ToLower())
                {
                    return false;
                }
            }
            return true;
        }
        internal static bool IsValidData(KiiPushMessageData data)
        {
            JsonObject json = data.ToJsonObject ();
            IEnumerator<string> keys = json.Keys ();
            while (keys.MoveNext())
            {
                string key = keys.Current;
                if (!ValidateKey(key))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

