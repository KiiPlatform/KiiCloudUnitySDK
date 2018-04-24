using System;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represent data to be delivered to each device.
    /// It contains a JSON object whose field represents key-value pairs of message's payload.
    /// Note that, if the data intended to deliver to GCM devices, key can not be a reserved word listed bellow:
    /// 
    /// <list type="bullet">
    ///     <item><term>any key starts with 'google'</term></item>
    ///     <item><term>from</term></item>
    ///     <item><term>registration_ids</term></item>
    ///     <item><term>collapse_key</term></item>
    ///     <item><term>data</term></item>
    ///     <item><term>delay_while_idle</term></item>
    ///     <item><term>time_to_live</term></item>
    ///     <item><term>restricted_package_name</term></item>
    ///     <item><term>dry_run</term></item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class KiiPushMessageData
    {
        /// <summary>
        /// The data.
        /// </summary>
        protected JsonObject mData;
        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.KiiPushMessageData"/> class.
        /// </summary>
        /// <remarks></remarks>
        public KiiPushMessageData()
        {
            this.mData = new JsonObject();
        }
        /// <summary>
        /// Obtain JSONObject representation of the Data.
        /// Operation for returned object won't affect this object.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>JsonObject represents the data.</returns>
        public JsonObject ToJsonObject()
        {
            return new JsonObject (this.mData.ToString());
        }
        /// <summary>
        /// Put Int value to the data.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="key">When the same value specified, overwrite existing value.</param>
        /// <param name="value">Int value.</param>
        /// <returns>This instance.</returns>
        /// <exception cref='ArgumentException'>
        /// Is thrown when the specified key is null or empty.
        /// </exception>
        public virtual KiiPushMessageData Put(string key, int value)
        {
            if (!this.IsValidKey(key))
            {
                throw new ArgumentException("Key is invalid.");
            }
            this.mData.Put(key, value);
            return this;
        }
        /// <summary>
        /// Put Long value to the data.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="key">When the same value specified, overwrite existing value.</param>
        /// <param name="value">Long value.</param>
        /// <returns>This instance.</returns>
        /// <exception cref='ArgumentException'>
        /// Is thrown when the specified key is null or empty.
        /// </exception>
        public virtual KiiPushMessageData Put(string key, long value)
        {
            if (!this.IsValidKey(key))
            {
                throw new ArgumentException("Key is invalid.");
            }
            this.mData.Put(key, value);
            return this;
        }
        /// <summary>
        /// Put Double value to the data.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="key">When the same value specified, overwrite existing value.</param>
        /// <param name="value">Double value.</param>
        /// <returns>This instance.</returns>
        /// <exception cref='ArgumentException'>
        /// Is thrown when the specified key is null or empty.
        /// </exception>
        public virtual KiiPushMessageData Put(string key, double value)
        {
            if (!this.IsValidKey(key))
            {
                throw new ArgumentException("Key is invalid.");
            }
            this.mData.Put(key, value);
            return this;
        }
        /// <summary>
        /// Put Bool value to the data.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="key">When the same value specified, overwrite existing value.</param>
        /// <param name="value">Bool value.</param>
        /// <returns>This instance.</returns>
        /// <exception cref='ArgumentException'>
        /// Is thrown when the specified key is null or empty.
        /// </exception>
        public virtual KiiPushMessageData Put(string key, bool value)
        {
            if (!this.IsValidKey(key))
            {
                throw new ArgumentException("Key is invalid.");
            }
            this.mData.Put(key, value);
            return this;
        }
        /// <summary>
        /// Put String value to the data.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="key">When the same value specified, overwrite existing value.</param>
        /// <param name="value">String value.</param>
        /// <returns>This instance.</returns>
        /// <exception cref='ArgumentException'>
        /// Is thrown when the specified key is null or empty.
        /// </exception>
        public virtual KiiPushMessageData Put(string key, string value)
        {
            if (!this.IsValidKey(key))
            {
                throw new ArgumentException("Key is invalid.");
            }
            this.mData.Put(key, value);
            return this;
        }
        /// <summary>
        /// Determines whether this instance is valid key the specified key.
        /// </summary>
        /// <remarks></remarks>
        /// <returns><c>true</c> if this instance is valid key the specified key; otherwise, <c>false</c>.</returns>
        /// <param name="key">Key.</param>
        protected virtual bool IsValidKey(String key)
        {
            return ValidateKey (key);
        }
        internal static bool ValidateKey(string key)
        {
            return !Utils.IsEmpty (key);
        }
    }
}

