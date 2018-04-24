using System;
using System.Collections.Generic;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents push message.
    /// </summary>
    /// <remarks></remarks>
    public abstract class ReceivedMessage
    {
        #region Inner Classes
        /// <summary>
        /// Scope of the object to which the event is occurred.
        /// </summary>
        /// <remarks></remarks>
        public enum Scope
        {
            /// <summary>
            /// App scope
            /// </summary>
            APP,
            /// <summary>
            /// App scope and Group scope
            /// </summary>
            APP_AND_GROUP,
            /// <summary>
            /// App scope and User scope
            /// </summary>
            APP_AND_USER
        }
        /// <summary>
        /// Type of the push message.
        /// Details of the push message is described in <see cref="KiiPushSubscription"/>
        /// </summary>
        /// <remarks></remarks>
        public enum MessageType {
            /// <summary>
            /// 'Push to App' notifications
            /// </summary>
            PUSH_TO_APP,
            /// <summary>
            /// 'Push to User' notifications
            /// </summary>
            PUSH_TO_USER,
            /// <summary>
            /// 'Direct Push' notifications
            /// </summary>
            DIRECT_PUSH
        }
        #endregion

        private KiiUser mSender;
        private Scope? mObjectScope;
        private string mActionIdentifier;
        JsonObject payload;

        internal ReceivedMessage (JsonMapper json)
        {
            this.payload = new JsonObject(json.Json.ToString());
            string sender = json.Sender;
            if (!Utils.IsEmpty(sender))
            {
                this.mSender = KiiUser.CreateByUri (new Uri (Utils.Path (Utils.Path (ConstantValues.URI_HEADER, "users", sender))));
            }
            string objectScope = json.ObjectScopeType;
            try
            {
                this.mObjectScope = (Scope)Enum.Parse (typeof(Scope), objectScope);
            }
            catch
            {
                this.mObjectScope = null;
            }

            try
            {
                this.mActionIdentifier = json.Json.GetString("actionIdentifier");
            }
            catch
            {
                this.mActionIdentifier = null;
            }

        }
        /// <summary>
        /// Parse the json that is received from GCM or APNS server as push message.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="json">message json.</param>
        /// <returns>Instance of <see cref="ReceivedMessage"/></returns>
        public static ReceivedMessage Parse(string json)
        {
            JsonObject jsonObject = new JsonObject (json);

            if (IsAPNS (jsonObject))
            {
                return ParseAPNS(jsonObject);
            }
            else
            {
                return ParseGCM(jsonObject);
            }
        }

        private static ReceivedMessage ParseAPNS(JsonObject json)
        {
            JsonMapper jm = new APNSJsonMapper (json);
            if (jm.IsPushToAppMessage ()) {
                return new PushToAppMessage (jm);
            }
            if (jm.IsPushToUserMessage ())
            {
                return new PushToUserMessage (jm);
            }
            return new DirectPushMessage (jm);
        }
        private static ReceivedMessage ParseGCM(JsonObject json)
        {
            JsonMapper jm = new GCMJsonMapper (json);
            if (jm.IsPushToAppMessage())
            {
                return new PushToAppMessage (jm);
            }
            else if (jm.IsPushToUserMessage ())
            {
                return new PushToUserMessage (jm);
            }
            else
            {
                return new DirectPushMessage (jm);
            }
        }
        private static bool IsAPNS(JsonObject json)
        {
            return json.Has ("aps") && (json.Get("aps") is JsonObject);
        }
        /// <summary>
        /// Get JSONObjcet representation of this message.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>Represents this message.</returns>
        public JsonObject ToJson()
        {
            return this.payload;
        }
        /// <summary>
        /// Returns the value associated with the given key, or null if no mapping of the desired type exists for the given key or a null value is explicitly associated with the key.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>a string value.</returns>
        /// <param name="key">a string key.</param>
        public string GetString(string key)
        {
            return this.payload.OptString (key);
        }
        /// <summary>
        /// Returns the value associated with the given key, or 0 if no mapping of the desired type exists for the given key.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>a int value.</returns>
        /// <param name="key">a string key.</param>
        public int GetInt(string key)
        {
            return this.payload.OptInt (key);
        }
        /// <summary>
        /// Returns the value associated with the given key, or 0 if no mapping of the desired type exists for the given key.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>a long value.</returns>
        /// <param name="key">a string key.</param>
        public long GetLong(string key)
        {
            return this.payload.OptLong (key);
        }
        /// <summary>
        /// Returns the value associated with the given key, or 0 if no mapping of the desired type exists for the given key.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>a double value.</returns>
        /// <param name="key">a string key.</param>
        public double GetDouble(string key)
        {
            return this.payload.OptDouble (key);
        }
        /// <summary>
        /// Returns the value associated with the given key, or false if no mapping of the desired type exists for the given key.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>a boolean value.</returns>
        /// <param name="key">a string key.</param>
        public bool GetBoolean(string key)
        {
            return this.payload.OptBoolean (key);
        }
        /// <summary>
        /// Returns true if this object has a mapping for given key.
        /// </summary>
        /// <remarks></remarks>
        /// <returns><c>true</c> if this instance has key; otherwise, <c>false</c>.</returns>
        /// <param name="key">a string key.</param>
        public bool Has(string key)
        {
            return this.payload.Has (key);
        }
        #region Properties
        /// <summary>
        /// Return the push message type.
        /// </summary>
        /// <remarks></remarks>
        /// <value>The type of the push message.</value>
        public abstract MessageType PushMessageType
        {
            get;
        }
        /// <summary>
        /// Return the user who causes the notification.
        /// </summary>
        /// <remarks></remarks>
        /// <value>The sender.</value>
        public virtual KiiUser Sender
        {
            get
            {
                return this.mSender;
            }
        }
        /// <summary>
        /// Return the scope of the object to which the event is occurred.
        /// </summary>
        /// <remarks></remarks>
        /// <value>The object scope.</value>
        public Scope? ObjectScope
        {
            get
            {
                return this.mObjectScope;
            }
        }

        /// <summary>
        /// Return the action identifier given by APNS plugin callback. Always null for GCM push.
        /// </summary>
        /// <remarks></remarks>
        /// <value>The APNS action identifier.</value>
        public string ActionIdentifier
        {
            get
            {
                return this.mActionIdentifier;
            }
        }

        #endregion

    }
}

