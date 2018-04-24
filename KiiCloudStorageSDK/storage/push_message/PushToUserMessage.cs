using System;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// "Push to User" notifications provide a means to push messages using a publisher-subscriber model.
    /// This feature will let you and your app users quickly send messages to other users while providing a way to scope to whom the messages are sent.
    /// </summary>
    /// <remarks></remarks>
    public class PushToUserMessage : ReceivedMessage
    {
        private string mTopicName;
        private string mObjectScopeGroupId;
        private string mObjectScopeUserId;

        internal PushToUserMessage (JsonMapper json) : base(json)
        {
            this.mTopicName = json.Topic;
            this.mObjectScopeGroupId = json.ObjectScopeGroupID;
            this.mObjectScopeUserId = json.ObjectScopeUserID;
        }

        /// <summary>
        /// Checks whether push message contains KiiTopic or not.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>true if the push message contains topic, false otherwise.</returns>
        public bool ContainsKiiTopic() {
            return (!Utils.IsEmpty(this.mTopicName) && ObjectScope != null);
        }

        #region Properties
        /// <summary>
        /// Return the push message type.
        /// </summary>
        /// <remarks></remarks>
        /// <value>The type of the push message.</value>
        public override MessageType PushMessageType
        {
            get
            {
                return MessageType.PUSH_TO_USER;
            }
        }
        /// <summary>
        /// Generate KiiTopic based on the information parsed from push message.
        /// </summary>
        /// <remarks></remarks>
        /// <value>The topic instance in which a message is sent or null if <see cref="ContainsKiiTopic()"/> is false.</value>
        public KiiTopic KiiTopic
        {
            get
            {
                if (!this.ContainsKiiTopic())
                {
                    return null;
                }
                if (ObjectScope == null)
                {
                    return null;
                }
                switch (ObjectScope)
                {
                case Scope.APP:
                    return new KiiTopic(null, this.mTopicName);
                case Scope.APP_AND_GROUP:
                    return new KiiTopic(ObjectScopeGroup, this.mTopicName);
                case Scope.APP_AND_USER:
                    return new KiiTopic(ObjectScopeUser, this.mTopicName);
                default:
                    throw new Exception("Unexpected Scope.");
                }
            }
        }
        /// <summary>
        /// Return the group that the bucket generated notification belongs to.
        /// Push message has this field only if the bucket is group scoped.
        /// </summary>
        /// <remarks></remarks>
        /// <value>KiiGroup.</value>
        public KiiGroup ObjectScopeGroup
        {
            get
            {
                if (Utils.IsEmpty(this.mObjectScopeGroupId))
                {
                    return null;
                }
                return KiiGroup.GroupWithID(this.mObjectScopeGroupId);
            }
        }
        /// <summary>
        /// Return the user that the bucket generated notification belongs to.
        /// Push message has this field only if the subscribed bucket is user scoped.
        /// </summary>
        /// <remarks></remarks>
        /// <value>KiiUser.</value>
        public KiiUser ObjectScopeUser
        {
            get
            {
                if (Utils.IsEmpty(this.mObjectScopeUserId))
                {
                    return null;
                }
                return KiiUser.CreateByUri (new Uri (Utils.Path (Utils.Path (ConstantValues.URI_HEADER, "users", this.mObjectScopeUserId))));
            }
        }
        #endregion
    }
}

