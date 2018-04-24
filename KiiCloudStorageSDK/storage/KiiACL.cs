using System;
using System.Collections.Generic;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides ACL operations.
    /// </summary>
    /// <remarks>
    /// To get this instance, call Acl(string) API of <see cref="KiiBucket"/>, <see cref="KiiObject"/>.
    /// </remarks>
    /// <typeparam name="T">
    /// The type that implements <see cref="AccessControllable"/>.
    /// </typeparam>
    /// <typeparam name="U">
    /// Enum of ACL action.
    /// </typeparam>
    public abstract class KiiACL<T, U> where T : AccessControllable
    {
        private T mParent;
        private U mAction;

        internal KiiACL()
        {

        }
        /// <summary>
        /// Determines whether the specified <see cref="KiiACL"/> is equal to the current <see cref="KiiCorp.Cloud.Storage.KiiACL"/>.
        /// </summary>
        /// <param name='other'>
        /// The <see cref="KiiACL"/> to compare with the current <see cref="KiiCorp.Cloud.Storage.KiiACL"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="KiiACL"/> is equal to the current
        /// <see cref="KiiCorp.Cloud.Storage.KiiACL"/>; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object that)
        {
            if (this == that) return true;
            if (that == null) return false;
            if (!this.GetType().Equals(that.GetType())) return false;
            if (!this.mParent.GetType().Equals(((KiiACL<T, U>)that).mParent.GetType())) return false;
            // assume that objects are different if parent is not saved.
            if (this.ParentID == null || ((KiiACL<T, U>)that).ParentID == null) return false;
            return ToActionString(this.mAction).Equals(ToActionString(((KiiACL<T, U>)that).mAction)) &&
                this.ParentUrl.Equals(((KiiACL<T, U>)that).ParentUrl);
        }
        /// <summary>
        /// Serves as a hash function for a <see cref="KiiCorp.Cloud.Storage.KiiACL"/> object.
        /// </summary>
        /// <returns>
        /// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = ToActionString(this.mAction).GetHashCode();
                if (this.ParentID != null)
                {
                    result = (result * 397) ^ this.ParentUrl.GetHashCode();
                }
                else
                {
                    result = (result * 397) ^ this.Parent.GetHashCode();
                }
                return result;
            }
        }
        /// <summary>
        /// Create a new Entry with specified subject
        /// </summary>
        /// <remarks>
        /// To add an ACL entry to KiiCloud, please call entry.Save(ACLOperation) API.
        /// </remarks>
        /// <returns>
        /// KiiACLEntry instance.
        /// </returns>
        /// <param name='subject'>
        /// Subject.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Is thrown when subject is null.
        /// </exception>
        public KiiACLEntry<T, U> Subject(KiiSubject subject)
        {
            return new KiiACLEntry<T, U>(this, subject);
        }

        #region Blocking APIs
        /// <summary>
        /// Gets the current ACL entries
        /// </summary>
        /// <remarks>
        /// This API access to server and fetch all list of User/Group and granted actions.
        /// </remarks>
        /// <returns>
        /// The acl entries.
        /// </returns>
        /// <exception cref='T:ACLOperationException'>
        /// Is thrown when the ACL operation is failed.
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends broken Json.
        /// </exception>
        internal IList<KiiACLEntry<T, U>> ListAclEntries()
        {
            IList<KiiACLEntry<T, U>> result = null;
            ExecListAclEntries(Kii.HttpClientFactory, (IList<KiiACLEntry<T, U>> list, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                result = list;
            });
            return result;
        }
        #endregion

        #region Async APIs
        /// <summary>
        /// Gets the current ACL entries
        /// </summary>
        /// <remarks>
        /// This API access to server and fetch all list of User/Group and granted actions.
        /// </remarks>
        /// <returns>
        /// The acl entries.
        /// </returns>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        /// <exception cref='T:ACLOperationException'>
        /// Is thrown when the ACL operation is failed.
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends broken Json.
        /// </exception>
        internal void ListAclEntries(KiiACLListCallback<T, U> callback)
        {
            ExecListAclEntries(Kii.AsyncHttpClientFactory, callback);
        }
        #endregion

        #region Execution
        private void ExecListAclEntries(KiiHttpClientFactory factory, KiiACLListCallback<T, U> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiACLListCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }
            string id = ParentID;
            if (Utils.IsEmpty(id)) {
                callback(null, new InvalidOperationException("Topic does not exist in the cloud."));
                return;
            }
            // Fetch ACL
            string aclUrl = Utils.Path(ParentUrl, "acl");

            KiiHttpClient client = factory.Create(aclUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);

            // send request
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(null, e);
                    return;
                }
                // parse response
                IList<KiiACLEntry<T, U>> list = null;
                try
                {
                    JsonObject respObj = new JsonObject(response.Body);
                    list = ParseListResponse(respObj);
                }
                catch (JsonException)
                {
                    callback(null, new IllegalKiiBaseObjectFormatException(response.Body));
                    return;
                }
                callback(list, null);
            });
        }
        #endregion


        private IList<KiiACLEntry<T, U>> ParseListResponse(JsonObject json)
        {
            List<KiiACLEntry<T, U>> entries = new List<KiiACLEntry<T, U>>();
            string[] actionNames = ActionNames;
            foreach (string name in actionNames)
            {
                JsonArray whiteList = json.OptJsonArray(name);
                if(whiteList == null) {
                    continue;
                }
                for (int i = 0 ; i < whiteList.Length() ; ++i)
                {
                    U action;
                    try
                    {
                        action = ToAction(name);
                    }
                    catch(Exception)
                    {
                        // Just ignore and continue if failed to parse action.
                        // Could be the action newly introduced.
                        continue;
                    }

                    KiiACL<T, U> acl = CreateFromAction(mParent, action);

                    JsonObject entry = whiteList.GetJsonObject(i);
                    KiiACLEntry<T, U> kae = null;
                    if (entry.Has("groupID"))
                    {
                        string gid = entry.GetString("groupID");
                        kae = acl.Subject(KiiGroup.GroupWithID(gid));
                        entries.Add(kae);
                    }
                    else if (entry.Has("userID"))
                    {
                        string uid = entry.GetString("userID");
                        KiiSubject sbj = GetSubjetFromUserID(uid);
                        kae = acl.Subject(sbj);
                        entries.Add(kae);
                    }
                }
            }
            return entries;
        }

        private KiiSubject GetSubjetFromUserID(string userID)
        {
            if (string.Compare(userID, "ANONYMOUS_USER", true) == 0)
            {
                return KiiAnonymousUser.Get();
            }
            else if (string.Compare(userID, "ANY_AUTHENTICATED_USER", true) == 0)
            {
                return KiiAnyAuthenticatedUser.Get();
            }
            else
            {
                return KiiUser.UserWithID(userID);
            }
        }

        internal virtual void SaveParentIfNeeds()
        {
        }

        internal abstract string ToActionString(U action);

        internal abstract U ToAction(string actionName);

        internal abstract KiiACL<T, U> CreateFromAction(T parent, U action);

        #region properties

        internal T Parent
        {
            get
            {
                return mParent;
            }
            set
            {
                mParent = value;
            }
        }

        internal U Action
        {
            get
            {
                return mAction;
            }
            set
            {
                mAction = value;
            }
        }

        internal string ActionString
        {
            get
            {
                return ToActionString(mAction);
            }
        }

        internal abstract string ParentID
        {
            get;
        }

        internal abstract string ParentUrl
        {
            get;
        }

        internal abstract string[] ActionNames
        {
            get;
        }

        #endregion

    }
}

