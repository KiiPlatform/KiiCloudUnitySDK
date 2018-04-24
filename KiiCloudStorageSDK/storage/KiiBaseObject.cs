using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Base class of key-value object.
    /// </summary>
    /// <remarks>
    /// Developers don't need to extend this class in their apps.
    /// </remarks>
    public abstract class KiiBaseObject
    {
        internal JsonObject mJSON;
        internal JsonObject mJSONPatch;
        private Dictionary<string, object> mReservedKeys;

        internal KiiBaseObject(JsonObject json, Dictionary<string, object> reservedKeys)
        {
            mJSON = json;
            mJSONPatch = new JsonObject();
            mReservedKeys = reservedKeys;
        }

        internal KiiBaseObject(Dictionary<string, object> reservedKeys) {
            mJSON = new JsonObject();
            mJSONPatch = new JsonObject();
            mReservedKeys = reservedKeys;
        }

        internal void SetReserveProperty(string key, int value) {
            SetInner(key, value);
        }

        private void SetInner<T> (string key, T value) {
            try {
                mJSON.Put(key, value);
                mJSONPatch.Put(key, value);
            } catch (JsonException e) {
                throw new IllegalKiiBaseObjectFormatException(e.Message);
            }
    
        }

        internal void SetReserveProperty(string key, string value) {
            SetInner(key, value);
        }

        internal void SetReserveProperty(string key, bool value) {
            SetInner(key, value);
        }

        /// <summary>
        /// Gets the list of keys
        /// </summary>
        /// <remarks>
        /// Reserved keys are not included.
        /// </remarks>
        /// <returns>
        /// List of keys.
        /// </returns>
        public IEnumerable<string> Keys()
        {
            List<string> keys = new List<string>();
            IEnumerator<string> it = mJSON.Keys();
            while(it.MoveNext())
            {
                string key = it.Current;
                if(IsValidKey(key) && !key.StartsWith("_"))
                {
                    keys.Add(key);
                }
            }
            return keys;
        }

        /// <summary>
        /// Remove the specified key and value.
        /// </summary>
        /// <remarks>
        /// Key must be valid and should not be reserved.
        /// </remarks>
        /// <param name='key'>
        /// Key you want to remove
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when key is empty or reserved.
        /// </exception>
        public virtual void Remove(string key)
        {
            AssertKey(key);
            try
            {
                mJSON.Remove(key);
                mJSONPatch.Remove(key);
            }
            catch (KeyNotFoundException)
            {
                // Nothing to do.
            }
        }

        internal void AssertKey(string key)
        {
            if(!IsValidKey(key))
            {
                throw new ArgumentException("Key is invalid :" + key);
            }
        }

        internal bool IsValidKey(string key)
        {
            if (Utils.IsEmpty(key))
            {
                return false;
            }
            if(mReservedKeys != null && mReservedKeys.Count > 0)
            {
                if (mReservedKeys.ContainsKey(key))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Determine if this instance has the specified key.
        /// </summary>
        /// <remarks>
        /// Key must not be null.
        /// </remarks>
        /// <returns>
        /// <code>true</code> if the key is in this instance ; otherwise <code>false</code>
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        public bool Has(string key)
        {
            return mJSON.Has(key);
        }

        /// <summary>
        /// Gets the integer in KiiBaseObject.
        /// </summary>
        /// <remarks>
        /// If key is not found, <see cref='IllegalKiiBaseObjectFormatException'/> is thrown.
        /// </remarks>
        /// <returns>
        /// Value as int associated with the key.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when the type of value is not int
        /// </exception>
        public int GetInt(string key)
        {
            try
            {
                return mJSON.GetInt(key);
            }
            catch (JsonException e)
            {
                throw new IllegalKiiBaseObjectFormatException(e.Message);
            }
        }

        /// <summary>
        /// Gets the integer. If key is not found, returns fallback.
        /// </summary>
        /// <remarks>
        /// If key is not found, returns fallback.
        /// </remarks>
        /// <returns>
        /// Value as int associated with the key or fallback.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='fallback'>
        /// Fallback.
        /// </param>
        public int GetInt(string key, int fallback)
        {
            try
            {
                return mJSON.GetInt(key);
            }
            catch (Exception)
            {
                return fallback;
            }
        }

        /// <summary>
        /// Gets the long in KiiBaseObject.
        /// </summary>
        /// <remarks>
        /// If key is not found, <see cref='IllegalKiiBaseObjectFormatException'/> is thrown.
        /// </remarks>
        /// <returns>
        /// Value as string associated with the key.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when the type of value is not string
        /// </exception>
        public long GetLong(string key)
        {
            try
            {
                return mJSON.GetLong(key);
            }
            catch (JsonException e)
            {
                throw new IllegalKiiBaseObjectFormatException(e.Message);
            }
        }

        /// <summary>
        /// Gets the long. If key is not found, returns fallback.
        /// </summary>
        /// <remarks>
        /// If key is not found, returns fallback.
        /// </remarks>
        /// <returns>
        /// Value as long associated with the key or fallback.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='fallback'>
        /// Fallback.
        /// </param>
        public long GetLong(string key, long fallback)
        {
            try
            {
                return mJSON.GetLong(key);
            }
            catch (Exception)
            {
                return fallback;
            }
        }

        /// <summary>
        /// Gets the double in KiiBaseObject.
        /// </summary>
        /// <remarks>
        /// If key is not found, <see cref='IllegalKiiBaseObjectFormatException'/> is thrown.
        /// </remarks>
        /// <returns>
        /// Value as double associated with the key.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when the type of value is not double
        /// </exception>
        public double GetDouble(string key)
        {
            try
            {
                return mJSON.GetDouble(key);
            }
            catch (JsonException e)
            {
                throw new IllegalKiiBaseObjectFormatException(e.Message);
            }
        }

        /// <summary>
        /// Gets the double. If key is not found, returns fallback.
        /// </summary>
        /// <remarks>
        /// If key is not found, returns fallback.
        /// </remarks>
        /// <returns>
        /// Value as double associated with the key or fallback.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='fallback'>
        /// Fallback.
        /// </param>
        public double GetDouble(string key, double fallback)
        {
            return mJSON.OptDouble(key, fallback);
        }

        /// <summary>
        /// Gets the boolean in KiiBaseObject.
        /// </summary>
        /// <remarks>
        /// If key is not found, <see cref='IllegalKiiBaseObjectFormatException'/> is thrown.
        /// </remarks>
        /// <returns>
        /// Value as string associated with the key.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when the type of value is not bool
        /// </exception>
        public bool GetBoolean(string key)
        {
            try
            {
                return mJSON.GetBoolean(key);
            }
            catch (JsonException e)
            {
                throw new IllegalKiiBaseObjectFormatException(e.Message);
            }
        }

        /// <summary>
        /// Gets the boolean. If key is not found, returns fallback.
        /// </summary>
        /// <remarks>
        /// If key is not found, returns fallback.
        /// </remarks>
        /// <returns>
        /// Value as bool associated with the key or fallback.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='fallback'>
        /// Fallack.
        /// </param>
        public bool GetBoolean(string key, bool fallback)
        {
            return mJSON.OptBoolean(key, fallback);
        }

        /// <summary>
        /// Gets the string in KiiBaseObject.
        /// </summary>
        /// <remarks>
        /// If key is not found, <see cref='IllegalKiiBaseObjectFormatException'/> is thrown.
        /// </remarks>
        /// <returns>
        /// Value as string associated with the key.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when the type of value is not string
        /// </exception>
        public string GetString(string key)
        {
            try
            {
                return mJSON.GetString(key);
            }
            catch (JsonException e)
            {
                throw new IllegalKiiBaseObjectFormatException(e.Message);
            }
        }

        /// <summary>
        /// Gets the string. If key is not found, returns fallback.
        /// </summary>
        /// <remarks>
        /// If key is not found, returns fallback.
        /// </remarks>
        /// <returns>
        /// Value as string associated with the key or fallback.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='fallback'>
        /// Fallback.
        /// </param>
        public string GetString(string key, string fallback)
        {
            return mJSON.OptString(key, fallback);
        }

        /// <summary>
        /// Gets the Uri with the specified key.
        /// </summary>
        /// <remarks>
        /// If key is not found, <see cref='IllegalKiiBaseObjectFormatException'/> is thrown.
        /// </remarks>
        /// <returns>
        /// Value as Uri associated with the key.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when the format of value is not Uri.
        /// </exception>
        public Uri GetUri(string key)
        {
            try
            {
                string uri = mJSON.GetString(key);
                return new Uri(uri);
            }
            catch (JsonException e)
            {
                throw new IllegalKiiBaseObjectFormatException(e.Message);
            }
        }

        /// <summary>
        /// Gets the byte array with the specified key.
        /// </summary>
        /// <remarks>
        /// If value is empty, returns null.
        /// </remarks>
        /// <returns>
        /// The byte array associated with the key.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when value is not string.
        /// </exception>
        public byte[] GetByteArray(string key)
        {
            if (!Has(key))
            {
                return null;
            }
            try
            {
                string temp;
                temp = mJSON.GetString(key);
                // returns null is the value is empty
                if (Utils.IsEmpty(temp))
                {
                    return null;
                }
                return Convert.FromBase64String(temp);
            }
            catch (JsonException e)
            {
                throw new IllegalKiiBaseObjectFormatException(e.Message);
            }
        }

        /// <summary>
        /// Gets the JSON object with the specified key.
        /// </summary>
        /// <remarks>
        /// If key is not found, <see cref='IllegalKiiBaseObjectFormatException'/> is thrown.
        /// </remarks>
        /// <returns>
        /// Value as JsonObject associated with the key.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when the type of value is not JsonObject
        /// </exception>
        public JsonObject GetJsonObject(string key)
        {
            try
            {
                return mJSON.GetJsonObject(key);
            }
            catch (JsonException e)
            {
                throw new IllegalKiiBaseObjectFormatException(e.Message);
            }
        }
        /// <summary>
        /// Gets the Json array with the specified key.
        /// </summary>
        /// <remarks>
        /// If key is not found, <see cref='IllegalKiiBaseObjectFormatException'/> is thrown.
        /// </remarks>
        /// <returns>
        /// Value as JsonArray associated with the key.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when the type of value is not JsonArray.
        /// </exception>
        public JsonArray GetJsonArray(string key)
        {
            try
            {
                return mJSON.GetJsonArray(key);
            }
            catch (JsonException e)
            {
                throw new IllegalKiiBaseObjectFormatException(e.Message);
            }
        }

        /// <summary>
        /// Gets or sets the object with the specified key.
        /// </summary>
        /// <remarks>
        /// As the type of value is <see cref="object"/>, developers need to cast value.
        /// If you want to suppress IllegalKiiBaseObjectFormatException, use GetXxx(key, fallback)
        /// </remarks>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <value>
        /// Value associated with the key.
        /// </value>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when key is null.
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when specified key is not existed
        /// </exception>
        public virtual object this[string key]
        {
            get
            {
                try
                {
                    return mJSON.Get(key);
                }
                catch (JsonException e)
                {
                    throw new IllegalKiiBaseObjectFormatException(e.Message);
                }
            }
            set
            {
                AssertKey(key);
                if (value is byte[])
                {
                    value = Convert.ToBase64String((byte[])value);
                }
                if (value is Uri)
                {
                    value = ((Uri)value).ToString();
                }
                if (value is KiiGeoPoint)
                {
                    value = ((KiiGeoPoint)value).ToJson();
                }
                mJSON.Put(key, value);
                mJSONPatch.Put(key, value);
            }
        }
    }
}

