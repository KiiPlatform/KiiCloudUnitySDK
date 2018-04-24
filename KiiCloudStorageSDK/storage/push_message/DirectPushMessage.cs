using System;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// "Direct Push" notifications allows you to push messages directly to a specific user.
    /// The feature is intended to be used by an app developer only.
    /// An app developer can select any application user and directly push a message to this user via the developer portal.
    /// </summary>
    /// <remarks></remarks>
    public class DirectPushMessage : ReceivedMessage
    {
        internal DirectPushMessage (JsonMapper json) : base(json)
        {
        }
        /// <summary>
        /// Return the push message type.
        /// </summary>
        /// <value>The type of the push message.</value>
        /// <remarks></remarks>
        public override MessageType PushMessageType
        {
            get
            {
                return MessageType.DIRECT_PUSH;
            }
        }
        /// <summary>
        /// Return null. Sender is always application admin.
        /// </summary>
        /// <remarks></remarks>
        /// <value>The sender.</value>
        public override KiiUser Sender
        {
            get
            {
                return null;
            }
        }
    }
}

