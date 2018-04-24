using System;
using System.Text;
using System.Text.RegularExpressions;

using JsonOrg;
using KiiCorp.Cloud.Storage;

namespace KiiCorp.Cloud.Analytics
{
    /// <summary>
    /// Kii event.
    /// </summary>
    /// <remarks>
    /// Developer can set arbitrary key-value pairs to this instance.
    /// </remarks>
    public class KiiEvent
    {
        private const string KEY_NAME_PATTERN = "^[a-zA-Z][a-zA-Z0-9_]{0,63}$";
        private const string TYPE_PATTERN = "^\\S.{0,127}";

        private string mType;
        private long mTriggeredAt = -1;
        private bool mSent = false;

        private JsonObject mJson = new JsonObject();

        internal KiiEvent (string type)
        {
            if (!IsValidType(type))
            {
                throw new ArgumentException(ErrorInfo.KIIEVENT_TYPE_INVALID);
            }
            mType = type;
            mTriggeredAt = Utils.CurrentTimeMills;
        }
        
        internal virtual JsonObject ConvertToJsonObject (string deviceID)
        {
            long uploadedAt = Utils.CurrentTimeMills;
            mJson.Put("_deviceID", deviceID);
            mJson.Put("_triggeredAt", mTriggeredAt);
            mJson.Put("_uploadedAt", uploadedAt);
            mJson.Put("_type", mType);
            
            return mJson;
        }
        
        #region Validation
        
        internal static bool IsValidType (string value)
        {
            if (Utils.IsEmpty(value))
            {
                return false;    
            }
            if (!Regex.IsMatch(value, TYPE_PATTERN))
            {
                return false;
            }
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            if (enc.GetBytes(value).Length > 128)
            {
                return false;
            }
            return true;
        }
        
        internal static bool IsValidKey (string value)
        {
            if (Utils.IsEmpty(value))
            {
                return false;
            }
            return Regex.IsMatch(value, KEY_NAME_PATTERN);
        }
        
        #endregion
        
        /// <summary>
        /// Gets or sets the attribute with the specified key.
        /// </summary>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <value>
        /// The value for specified key.
        /// </value>
        /// <remarks>
        /// key pattern must be ^[a-zA-Z][a-zA-Z0-9_]{0,63}$
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when value is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when
        /// <list type="bullet">
        /// <item><term>key is invalid</term></item>
        /// <item><term>The type of value is double or double[]</term></item>
        /// <item><term>value is empty string</term></item>
        /// </list>
        /// </exception>
        public object this[string key]
        {
            get
            {
                return mJson.Get(key);
            }
            set
            {
                if (!IsValidKey(key))
                {
                    throw new ArgumentException(ErrorInfo.KIIEVENT_KEY_INVALID);
                }
                if (value == null)
                {
                    throw new ArgumentNullException(ErrorInfo.KIIEVENT_VALUE_NULL);
                }
                if (value is double)
                {
                    throw new ArgumentException(ErrorInfo.KIIEVENT_VALUE_DOUBLE);
                }
                if (value is int[])
                {
                    JsonArray array = ToJsonArray((int[])value);
                    mJson.Put(key, array);
                    return;
                }
                if (value is long[])
                {
                    JsonArray array = ToJsonArray((long[])value);
                    mJson.Put(key, array);
                    return;
                }
                if (value is float[])
                {
                    JsonArray array = ToJsonArray((float[])value);
                    mJson.Put(key, array);
                    return;
                }
                if (value is double[])
                {
                    throw new ArgumentException(ErrorInfo.KIIEVENT_VALUE_DOUBLE);
                }                
                if (value is string[])
                {
                    JsonArray array = ToJsonArray((string[])value);
                    mJson.Put(key, array);
                    return;
                }
                if (value is string)
                {
                    if (((string)value).Length == 0) 
                    {
                        throw new ArgumentException(ErrorInfo.KIIEVENT_VALUE_EMPTY);
                    }
                }
                

                mJson.Put(key, value);
            }
        }
        
        private JsonArray ToJsonArray<T> (T[] values)
        {
            JsonArray array = new JsonArray();
            foreach (T item in values)
            {
                array.Put(item);
            }
            return array;
        }
        
        /// <summary>
        /// Determine whether this event is sent to KiiCloud
        /// </summary>
        /// <remarks></remarks>
        /// <value>
        /// <c>true</c> if this event is sent; otherwise, <c>false</c>.
        /// </value>
        public virtual bool Sent
        {
            get
            {
                return mSent;
            }
            internal set
            {
                mSent = value;
            }
        }
        /// <summary>
        /// Null object of the Kii event.
        /// </summary>
        /// <remarks>
        /// This event is never sent to the KiiCloud.
        /// <see cref="KiiAnalytics.Upload(KiiEvent[])"/> ignores this event.
        /// </remarks>
        public class NullKiiEvent : KiiEvent
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Analytics.KiiEvent+NullKiiEvent"/> class.
            /// </summary>
            public NullKiiEvent() : base("NULL")
            {
            }
            internal override JsonObject ConvertToJsonObject (string deviceID)
            {
                return null;
            }
        }
    }
}

