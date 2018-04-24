using System;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// A "Push to App" notification mechanism will let your application know that there were some changes on object(s) in a bucket.
    /// By leveraging this notification feature, your application can quickly fetch the changes that occur on the server-side.
    /// </summary>
    /// <remarks></remarks>
    public class PushToAppMessage : ReceivedMessage
    {
        private string mBucketId;
        private string mObjectId;
        private string mObjectScopeGroupId;
        private string mObjectScopeUserId;

        internal PushToAppMessage (JsonMapper json) : base(json)
        {
            this.mBucketId = json.BucketID;
            this.mObjectId = json.ObjectID;
            this.mObjectScopeGroupId = json.ObjectScopeGroupID;
            this.mObjectScopeUserId = json.ObjectScopeUserID;
        }

        /// <summary>
        /// Checks whether the push message contains KiiBucket or not.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>true if the push message contains KiiBucket, otherwise false.</returns>
        public bool ContainsKiiBucket()
        {
            Scope? scope = ObjectScope;
            if(scope == null)
                return false;
            if(scope == Scope.APP_AND_GROUP && ObjectScopeGroup == null)
                return false;
            if(scope == Scope.APP_AND_USER && ObjectScopeUser == null)
                return false;
            return !Utils.IsEmpty(this.mBucketId);
        }
        /// <summary>
        /// Checks whether the push message contains KiiObject or not.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>true if the push message contains KiiObject, otherwise false.</returns>
        public bool ContainsKiiObject()
        {
            if (ContainsKiiBucket()) {
                Scope? scope = ObjectScope;
                if (scope == Scope.APP || scope == Scope.APP_AND_GROUP || scope == Scope.APP_AND_USER) {
                    return !Utils.IsEmpty(this.mObjectId);
                }
            }
            return false;
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
                return MessageType.PUSH_TO_APP;
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
        /// <summary>
        /// Generate KiiBucket based on the information parsed from push message.
        /// </summary>
        /// <remarks></remarks>
        /// <value>Instance when the event of KiiBucket or KiiObject inside KiiBucekt happened. In other cases returns null.</value>
        public KiiBucket KiiBucket
        {
            get
            {
                if (!this.ContainsKiiBucket())
                {
                    return null;
                }
                if (ObjectScope == null) {
                    return null;
                }
                switch (ObjectScope)
                {
                case Scope.APP:
                    return Kii.Bucket(this.mBucketId);
                case Scope.APP_AND_GROUP:
                        return ObjectScopeGroup == null ? null : ObjectScopeGroup.Bucket(this.mBucketId);
                case Scope.APP_AND_USER:
                        return ObjectScopeUser == null ? null : ObjectScopeUser.Bucket(this.mBucketId);
                default:
                    throw new Exception("Unexpected Scope.");
                }
            }
        }
        /// <summary>
        /// Generate KiiObject based on the information parsed from push message.
        /// </summary>
        /// <remarks></remarks>
        /// <value>Instance when the event of KiiObject inside KiiBucekt happened. In other cases returns null.</value>
        public KiiObject KiiObject
        {
            get
            {
                if (!this.ContainsKiiObject())
                {
                    return null;
                }
                if (ObjectScope == null) {
                    return null;
                }

                string uri = null;
                switch (ObjectScope)
                {
                case Scope.APP:
                    uri = Utils.Path(ConstantValues.URI_HEADER, "buckets", this.mBucketId, "objects", this.mObjectId);
                    break;
                case Scope.APP_AND_GROUP:
                    uri = Utils.Path(ConstantValues.URI_HEADER, "groups", this.mObjectScopeGroupId, "buckets", this.mBucketId, "objects", this.mObjectId);
                    break;
                case Scope.APP_AND_USER:
                    uri = Utils.Path(ConstantValues.URI_HEADER, "users", this.mObjectScopeUserId, "buckets", this.mBucketId, "objects", this.mObjectId);
                    break;
                default:
                    throw new Exception("Unexpected Scope.");
                }
                return KiiObject.CreateByUri(new Uri(uri));
            }
        }
        #endregion
    }
}

