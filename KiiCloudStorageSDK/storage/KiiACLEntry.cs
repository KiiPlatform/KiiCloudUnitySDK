using System;
using System.Text;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides APIs to add/remove ACL entry.
    /// </summary>
    /// <remarks>
    /// Developers don't need to instantiate by themselves. Please call KiiACL.Subject(KiiSubject) API.
    /// </remarks>
    /// <typeparam name="T">
    /// Must be <see cref="KiiBucket"/>, <see cref="KiiObject"/>
    /// </typeparam>
    /// <typeparam name="U">
    /// Enum of ACL Actions
    /// </typeparam>
    public class KiiACLEntry<T, U> where T : AccessControllable
    {
        private KiiSubject mSubject;
        private KiiACL<T, U> mParent;

        internal KiiACLEntry(KiiACL<T, U> parent, KiiSubject subject)
        {
            if(parent == null) {
                throw new ArgumentException ("parent is null");
            }
            if (subject == null) {
                throw new ArgumentException("subject can not be null");
            }
            mParent = parent;
            mSubject = subject;
        }
        /// <summary>
        /// Determines whether the specified <see cref="KiiACLEntry"/> is equal to the current <see cref="KiiCorp.Cloud.Storage.KiiACLEntry"/>.
        /// </summary>
        /// <param name='other'>
        /// The <see cref="KiiACL"/> to compare with the current <see cref="KiiCorp.Cloud.Storage.KiiACLEntry"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="KiiACLEntry"/> is equal to the current
        /// <see cref="KiiCorp.Cloud.Storage.KiiACLEntry"/>; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object that)
        {
            if (this == that) return true;
            if (that == null) return false;
            if (!this.GetType().Equals(that.GetType())) return false;
            return ToSubjectString(this.mSubject).Equals(ToSubjectString(((KiiACLEntry<T, U>)that).mSubject)) &&
                this.mParent.Equals(((KiiACLEntry<T, U>)that).mParent);
        }
        /// <summary>
        /// Serves as a hash function for a <see cref="KiiCorp.Cloud.Storage.KiiACLEntry"/> object.
        /// </summary>
        /// <returns>
        /// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = ToSubjectString(this.mSubject).GetHashCode();
                result = (result * 397) ^ this.mParent.GetHashCode();
                return result;
            }
        }
        private string ToSubjectString(KiiSubject subject)
        {
            if (subject is KiiGroup)
            {
                return "GROUP:" + ((KiiGroup)subject).ID;
            }
            else if (subject is KiiUser)
            {
                return "USER:" + ((KiiUser)subject).ID;
            }
            else if (subject is KiiAnonymousUser)
            {
                return "ANONYMOUS_USER";
            }
            else if (subject is KiiAnyAuthenticatedUser)
            {
                return "ANY_AUTHENTICATED_USER";
            }
            else
            {
                throw new SystemException ("Unexpected error." + subject.GetType().ToString());
            }
        }
        #region Blocking APIs
        /// <summary>
        /// Save this ACL entry.
        /// </summary>
        /// <remarks>
        /// Saving this ACL entry throws <see cref="CloudException"/> if <see cref="KiiTopicACL"/> has been created with 
        /// <see cref="KiiAnonymousUser"/> and <see cref="TopicAction"/>.
        /// <para>If operation is REVOKE and there is no entry in KiiCloud, KiiCloud will send error response.</para>
        /// </remarks>
        /// <param name='operation'>
        /// ACL operation. See <see cref="ACLOperation"/>
        /// </param>
        public void Save(ACLOperation operation)
        {
            ExecSave(operation, Kii.HttpClientFactory, (KiiACLEntry<T, U> entry, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }
        #endregion

        #region Async APIs
        /// <summary>
        /// Save this ACL entry.
        /// </summary>
        /// <remarks>
        /// Subscribe or send message to topic is not supported for <see cref="KiiAnonymousUser"/>.
        /// Saving this ACL entry throws <see cref="CloudException"/> if <see cref="KiiTopicACL"/> has been created with 
        /// <see cref="KiiAnonymousUser"/> and <see cref="TopicAction"/>.
        /// <para>If operation is REVOKE and there is no entry in KiiCloud, KiiCloud will send error response.</para>
        /// </remarks>
        /// <param name='operation'>
        /// ACL operation. See <see cref="ACLOperation"/>
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void Save(ACLOperation operation, KiiACLCallback<T, U> callback)
        {
            ExecSave(operation, Kii.AsyncHttpClientFactory, callback);
        }
        #endregion

        #region Execution
        private void ExecSave(ACLOperation operation, KiiHttpClientFactory factory, KiiACLCallback<T, U> callback)
        {
            mParent.SaveParentIfNeeds();
            string requestUrl = Utils.Path(mParent.ParentUrl, "acl", mParent.ActionString, mSubject.Subject);

            KiiHttpClient client = GetHttpClient(factory, requestUrl, operation);
            if (client == null)
            {
                if (callback != null) { callback(null, new InvalidOperationException("not grant/revoke request")); }
                return;
            }
            KiiCloudEngine.SetAuthBearer(client);

            // send request
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(null, e); }
                    return;
                }
                if (callback != null) { callback(this, null); }
            });
        }
        #endregion

        private KiiHttpClient GetHttpClient(KiiHttpClientFactory factory, string url, ACLOperation operation)
        {
            switch (operation) {
            case ACLOperation.GRANT:
                return factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.PUT);
            case ACLOperation.REVOKE:
                return factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.DELETE);
            default:
                return null;
            }
        }


        #region properties

        /// <summary>
        /// Sets the parent object.
        /// </summary>
        /// <remarks>
        /// Developers don't need to use this property.
        /// </remarks>
        /// <value>
        /// The parent ACL instance.
        /// </value>
        public KiiACL<T, U> Parent
        {
            set
            {
                mParent = value;
            }
        }

        /// <summary>
        /// Gets the subject.
        /// </summary>
        /// <remarks>
        /// Developers don't need to use this property.
        /// </remarks>
        /// <value>
        /// The subject.
        /// </value>
        public KiiSubject Subject
        {
            get
            {
                return mSubject;
            }
        }

        /// <summary>
        /// Gets the action of this entry.
        /// </summary>
        /// <remarks>
        /// Developers don't need to use this property.
        /// </remarks>
        /// <value>
        /// The action.
        /// </value>
        public U Action
        {
            get
            {
                return mParent.Action;
            }
        }

        #endregion
    }
}

