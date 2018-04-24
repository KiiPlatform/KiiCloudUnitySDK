using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides APIs for Group features.
    /// </summary>
    /// <remarks>
    /// GroupName doesn't have unique constraints, so developers can create 3 groups whose names are the same one.
    /// </remarks>
    public class KiiGroup : KiiScope, KiiSubject
    {
        private string mId;

        private string groupName;

        private string mOwnerId;

        private HashSet<KiiUser> addUsers;

        private HashSet<KiiUser> removeUsers;

        internal KiiGroup(string groupName, IList<KiiUser> users)
        {
            this.groupName = groupName;
            this.addUsers = new HashSet<KiiUser>();
            this.removeUsers = new HashSet<KiiUser>();

            // Exclude the users whose id does not exists
            if(users == null) {
                return;
            }
            foreach (KiiUser user in users)
            {
                if(Utils.IsEmpty(user.ID))
                {
                    continue;
                }
                this.addUsers.Add(user);
            }
        }

        /// <summary>
        /// Creates KiiGroup by Uri
        /// </summary>
        /// <remarks>
        /// To get the latest information from server, need to call Refresh().
        /// </remarks>
        /// <returns>
        /// KiiGroup instance.
        /// </returns>
        /// <param name='uri'>
        /// Uri of KiiGroup
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when Uri is invalid.
        /// </exception>
        public static KiiGroup CreateByUri(Uri uri)
        {
            if (uri == null) {
                throw new ArgumentException(ErrorInfo.KIIGROUP_URL_IS_NULL);
            }
            // check scheme
            string scheme = uri.Scheme;
            if (scheme != ConstantValues.URI_SCHEME)
            {
                throw new ArgumentException(ErrorInfo.KIIGROUP_URI_NO_SUPPORT + uri);
            }

            string authority = uri.Authority;
            if (authority != "groups")
            {
                throw new ArgumentException(ErrorInfo.KIIGROUP_URI_NO_SUPPORT + uri);
            }
            // check segments
            string[] segments = uri.Segments;
            if (segments.Length != 2) {
                throw new ArgumentException(ErrorInfo.KIIGROUP_URI_NO_SUPPORT + uri);
            }
            KiiGroup group = new KiiGroup(null, null);
            group.mId = Utils.RemoveLastSlash(segments[1]);
            return group;
        }

        /// <summary>
        /// Gets the KiiBucket whose scope is this group
        /// </summary>
        /// <remarks>
        /// Bucket name must be valid.
        /// </remarks>
        /// <returns>
        /// KiiBucket instance whose scope is this group. If bucket is not in KiiCloud, it will be created when object in it is saved.
        /// </returns>
        /// <param name='bucketName'>
        /// Bucket name.
        /// </param>
        public KiiBucket Bucket(string bucketName)
        {
            return new KiiBucket(this, bucketName);
        }

        /// <summary>
        /// Get instance of group scope topic.
        /// The topic bound to this group
        /// </summary>
        /// <param name="name">Name of topic.</param>
        /// <returns>KiiTopic bound to this group.</returns>
        public KiiTopic Topic(string name)
        {
            if (Utils.IsEmpty(ID))
            {
                throw new InvalidOperationException("KiiGroup has deleted or not registered in KiiCloud yet.");
            }
            return new KiiTopic (this, name);
        }

        /// <summary>
        /// Adds the user to this group.
        /// </summary>
        /// <remarks>
        /// If user is already added, this API does nothing.
        /// </remarks>
        /// <param name='user'>
        /// User you want to add
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when user is null or doesn't have ID
        /// </exception>
        public void AddUser(KiiUser user)
        {
            if (user == null)
            {
                throw new ArgumentException("Provided user is null");
            }
            if (Utils.IsEmpty(user.ID))
            {
                throw new ArgumentException("Provided user is not saved to the cloud already");
            }
            addUsers.Add(user);
            removeUsers.Remove (user);
        }

        /// <summary>
        /// Removes the user from this group.
        /// </summary>
        /// <remarks>
        /// User mut not be null.
        /// </remarks>
        /// <param name='user'>
        /// User you want to remove
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when user is null or doesn't have ID.
        /// </exception>
        public void RemoveUser(KiiUser user)
        {
            if (user == null)
            {
                throw new ArgumentException("Provided user is null");
            }
            if (Utils.IsEmpty(user.ID))
            {
                throw new ArgumentException("Provided user is not saved to the cloud already");
            }
            addUsers.Remove(user);

            if (!Utils.IsEmpty(ID))
            {
                removeUsers.Add(user);
            }
        }

        #region Blocking APIs
        /// <summary>
        /// Creates new group own by current user on Kii Cloud with specified ID.
        /// </summary>
        /// <returns>KiiGroup instance.</returns>
        /// <param name="id">ID of the KiiGroup.</param>
        /// <param name="name">Name of the KiiGroup.</param>
        /// <param name="members">Members of the group. Group owner will be added as a group member no matter owner is in the list or not.</param>
        public static KiiGroup RegisterGroupWithID(String id, String name, List<KiiUser> members)
        {
            KiiGroup result = null;
            ExecRegisterWithID(id, name, members, Kii.HttpClientFactory, (KiiGroup group, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                result = group;
            });
            return result;
        }
        /// <summary>
        /// Save this KiiGroup on KiiCloud
        /// </summary>
        /// <remarks>
        /// This API sends some requests to KiiCloud.
        /// </remarks>
        public void Save()
        {
            Utils.CheckInitialize(true);
            if(ID == null)
            {
                SaveToCloud();
            }
            else
            {
                AddMembersToCloud();
                RemoveMembersFromCloud();
            }
        }

        /// <summary>
        /// Gets the latest information of this group.
        /// </summary>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when this group doesn't have ID.
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends broken Json.
        /// </exception>
        /// <exception cref='GroupOperationException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void Refresh()
        {
            ExecRefresh(Kii.HttpClientFactory, (KiiGroup group, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Delete this group from KiiCloud.
        /// </summary>
        /// <remarks>
        /// This API sends a request to KiiCloud.
        /// </remarks>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when this group doesn't have ID.
        /// </exception>
        /// <exception cref='GroupOperationException'>
        /// Is thrown when the group operation is failed.
        /// </exception>
        public void Delete()
        {
            ExecDelete(Kii.HttpClientFactory, (KiiGroup group, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Changes the name of this group.
        /// </summary>
        /// <remarks>
        /// New group name must no be empty.
        /// </remarks>
        /// <param name='name'>
        /// New group name.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is empty.
        /// </exception>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when this group doesn't have ID.
        /// </exception>
        /// <exception cref='GroupOperationException'>
        /// Is thrown when the group operation is failed.
        /// </exception>
        public void ChangeName(string name)
        {
            ExecChangeName(name, Kii.HttpClientFactory, (KiiGroup grup, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Gets the list of members in this group.
        /// </summary>
        /// <remarks>
        /// This api sends a request to KiiCloud.
        /// </remarks>
        /// <returns>
        /// The list of members
        /// </returns>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends broken Json
        /// </exception>
        /// <exception cref='GroupOperationException'>
        /// Is thrown when the group operation is failed.
        /// </exception>
        public IList<KiiUser> ListMembers()
        {
            IList<KiiUser> members = null;
            ExecListMembers(Kii.HttpClientFactory, (IList<KiiUser> list, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                members = list;
            });
            return members;
        }
        /// <summary>
        /// Gets the list of topics in this group scope.
        /// </summary>
        /// <returns>A list of the topics in this group scope.</returns>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <exception cref='UnauthorizedException'>
        /// Is thrown when this method called by anonymous user.
        /// </exception>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when this group doesn't have ID.
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public KiiListResult<KiiTopic> ListTopics()
        {
            return ListTopics((string)null);
        }
        /// <summary>
        /// Gets the list of next page of topics in this group scope.
        /// </summary>
        /// <param name="paginationKey">
        /// Specifies the pagination key that is obtained by <see cref="KiiListResult{T}.PaginationKey"/>.
        /// If specified null or empty, it's same as the <see cref="ListTopics()"/>.
        /// </param>
        /// <returns>A list of the topics in this group scope.</returns>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <exception cref='UnauthorizedException'>
        /// Is thrown when this method called by anonymous user.
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public KiiListResult<KiiTopic> ListTopics(string paginationKey)
        {
            KiiListResult<KiiTopic> result = null;
            ExecListTopics(Kii.HttpClientFactory, paginationKey, (KiiListResult<KiiTopic> topics, Exception e) => {
                if (e != null)
                {
                    throw e;
                }
                result = topics;
            });
            return result;
        }
        #endregion

        #region Async APIs
        /// <summary>
        /// Creates new group own by current user on Kii Cloud with specified ID.
        /// </summary>
        /// <param name="id">ID of the KiiGroup.</param>
        /// <param name="name">Name of the KiiGroup.</param>
        /// <param name="members">Members of the group. Group owner will be added as a group member no matter owner is in the list or not.</param>
        /// <param name="callback">Callback</param>
        public static void RegisterGroupWithID(String id, String name, IList<KiiUser> members,  KiiGenericsCallback<KiiGroup> callback)
        {
            ExecRegisterWithID(id, name, members, Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Save this KiiGroup on KiiCloud
        /// </summary>
        /// <remarks>
        /// This API sends some requests to KiiCloud.
        /// </remarks>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void Save(KiiGroupCallback callback)
        {
            ExecSave(Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Gets the latest information of this group.
        /// </summary>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void Refresh(KiiGroupCallback callback)
        {
            ExecRefresh(Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Delete this group from KiiCloud.
        /// </summary>
        /// <remarks>
        /// This API sends a request to KiiCloud.
        /// </remarks>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void Delete(KiiGroupCallback callback)
        {
            ExecDelete(Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Changes the name of this group.
        /// </summary>
        /// <remarks>
        /// New group name must no be empty.
        /// </remarks>
        /// <param name='name'>
        /// New group name.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void ChangeName(string name, KiiGroupCallback callback)
        {
            ExecChangeName(name, Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Gets the list of members in this group.
        /// </summary>
        /// <remarks>
        /// This api sends a request to KiiCloud.
        /// </remarks>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void ListMembers(KiiUserListCallback callback)
        {
            ExecListMembers(Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Asynchronous call for <see cref="ListTopics()"/>.
        /// </summary>
        /// <param name="callback">Callback.</param>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public void ListTopics(KiiGenericsCallback<KiiListResult<KiiTopic>> callback)
        {
            ListTopics(null, callback);
        }
        /// <summary>
        /// Asynchronous call for <see cref="ListTopics(string)"/>.
        /// </summary>
        /// <param name="paginationKey">
        /// Specifies the pagination key that is obtained by <see cref="KiiListResult{T}.PaginationKey"/>.
        /// If specified null or empty, it's same as the <see cref="ListTopics(KiiGenericsCallback{KiiListResult{KiiTopic}})"/>.
        /// </param>
        /// <param name="callback">Callback.</param>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public void ListTopics(string paginationKey, KiiGenericsCallback<KiiListResult<KiiTopic>> callback)
        {
            ExecListTopics(Kii.AsyncHttpClientFactory, paginationKey, callback);
        }
        #endregion

        #region Execution
        private static void ExecRegisterWithID(string id, string name, IList<KiiUser> members, KiiHttpClientFactory factory, KiiGenericsCallback<KiiGroup> callback)
        {
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }
            if (String.IsNullOrEmpty(id))
            {
                callback(null, new ArgumentException("id is null or empty."));
                return;
            }
            if (!Utils.ValidateGroupID(id))
            {
                callback(null, new ArgumentException("Invalid groupID format. " + id));
                return;
            }
            if (String.IsNullOrEmpty(name))
            {
                callback(null, new ArgumentException("name is null or empty."));
                return;
            }

            String ownerId = Kii.CurrentUser.ID;
            string url = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups", id);
            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.PUT);
            KiiCloudEngine.SetAuthBearer(client);
            client.Accept = "application/vnd.kii.GroupCreationResponse+json";

            JsonObject request = new JsonObject();
            try
            {
                request.Put("name", name);
                request.Put("owner", ownerId);
                if (members != null && members.Count > 0)
                {
                    JsonArray memberIDs = new JsonArray();
                    foreach (KiiUser member in members)
                    {
                        if(!Utils.IsEmpty(member.ID))
                        {
                            memberIDs.Put(member.ID);
                        }
                    }
                    request.Put("members", memberIDs);
                }
            }
            catch (JsonException e)
            {
                if (callback != null) { callback(null, new SystemException("unexpected error!", e)); }
                return;
            }

            client.ContentType = "application/vnd.kii.GroupCreationRequest+json";

            // send Request
            client.SendRequest(request.ToString(), (ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) {
                        callback(null, new GroupOperationException(e.Message, e, members, null));
                    }
                    return;
                }
                // parse response
                try
                {
                    JsonObject respObj = new JsonObject(response.Body);
                    KiiGroup group = new KiiGroup(name, members);
                    group.mId = respObj.GetString("groupID");
                    group.mOwnerId = ownerId;
                    if (callback != null) { callback(group, null); }
                } 
                catch (JsonException) 
                {
                    if (callback != null) { callback(null, new IllegalKiiBaseObjectFormatException(response.Body)); }
                    return;
                }
            });
        }
        private void ExecSave(KiiHttpClientFactory factory, KiiGroupCallback callback)
        {
            Utils.CheckInitialize(true);
            if(ID == null)
            {
                SaveToCloud(factory, callback);
            }
            else
            {
                AddMembersToCloud(factory, (KiiGroup group, Exception e) =>
                {
                    if (e != null)
                    {
                        if (callback != null) { callback(this, e); }
                        return;
                    }
                    RemoveMembersFromCloud(factory, callback);
                });
            }
        }

        private void SaveToCloud(KiiHttpClientFactory factory, KiiGroupCallback callback)
        {
            Utils.CheckInitialize(true);
            mOwnerId = Kii.CurrentUser.ID;
            string url = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId,
                                    "groups");
            
            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer(client);
            client.Accept = "application/vnd.kii.GroupCreationResponse+json";
            
            JsonObject obj = new JsonObject();
            try
            {
                obj.Put("name", groupName);
                obj.Put("owner", mOwnerId);
                obj.Put("members", UserIds());
            }
            catch (JsonException e1)
            {
                if (callback != null) { callback(this, new SystemException("unexpected error!", e1)); }
                return;
            }
            
            client.ContentType = "application/vnd.kii.GroupCreationRequest+json";
            
            // send Request
            client.SendRequest(obj.ToString(), (ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(this, new GroupOperationException(e.Message, e, new List<KiiUser>(addUsers), null)); }
                    return;
                }
                // parse response
                try
                {
                    JsonObject respObj = new JsonObject(response.Body);
                    mId = respObj.GetString("groupID");
                    // TODO: parse response and get list of failed user (CMO-557)
                } 
                catch (JsonException) 
                {
                    if (callback != null) { callback(this, new IllegalKiiBaseObjectFormatException(response.Body)); }
                    return;
                }
                addUsers.Clear();
                if (callback != null) { callback(this, null); }
            });
        }

        private void AddMembersToCloud(KiiHttpClientFactory factory, KiiGroupCallback callback)
        {
            Utils.CheckInitialize(true);
            string id = ID;
            if (Utils.IsEmpty(id))
            {
                if (callback != null) { callback(this, new InvalidOperationException(ErrorInfo.KIIGROUP_NO_ID)); }
                return;
            }

            IList<KiiUser> userList = new List<KiiUser>(addUsers);
            AddMembersToCloud(userList, 0, factory, callback);
        }

        private void AddMembersToCloud(IList<KiiUser> userList, int position, KiiHttpClientFactory factory, KiiGroupCallback callback)
        {
            if (position == userList.Count)
            {
                if (callback != null) { callback(this, null); }
                return;
            }
            KiiUser user = userList[position];
            string url = Utils.Path(MembersUrl, user.ID);

            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.PUT);
            KiiCloudEngine.SetAuthBearer(client);

            // send request
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(this, new GroupOperationException(e.Message, e, new List<KiiUser>(addUsers), new List<KiiUser>(removeUsers))); }
                    return;
                }
                addUsers.Remove(user);
                AddMembersToCloud(userList, position + 1, factory, callback);
            });
        }

        private void RemoveMembersFromCloud(KiiHttpClientFactory factory, KiiGroupCallback callback)
        {
            Utils.CheckInitialize(true);
            string id = ID;
            if (Utils.IsEmpty(id))
            {
                if (callback != null) { callback(this, new InvalidOperationException(ErrorInfo.KIIGROUP_NO_ID)); }
                return;
            }
            List<KiiUser> userList = new List<KiiUser>(removeUsers);
            RemoveMembersFromCloud(userList, 0, factory, callback);
        }

        private void RemoveMembersFromCloud(IList<KiiUser> userList, int position, KiiHttpClientFactory factory, KiiGroupCallback callback)
        {
            if (position == userList.Count)
            {
                if (callback != null) { callback(this, null); }
                return;
            }
            KiiUser user = userList[position];
            string url = Utils.Path(MembersUrl, user.ID);

            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.DELETE);
            KiiCloudEngine.SetAuthBearer(client);

            // send request
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(this, new GroupOperationException(e.Message, e, new List<KiiUser>(addUsers), new List<KiiUser>(removeUsers))); }
                    return;
                }
                removeUsers.Remove(user);
                RemoveMembersFromCloud(userList, position + 1, factory, callback);
            });
        }

        private void ExecRefresh(KiiHttpClientFactory factory, KiiGroupCallback callback)
        {
            Utils.CheckInitialize(true);
            if (Utils.IsEmpty(ID))
            {
                if (callback != null) { callback(this, new InvalidOperationException(ErrorInfo.KIIGROUP_NO_ID)); }
                return;
            }
            
            KiiHttpClient client = factory.Create(Url, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);
            client.Accept = "application/vnd.kii.GroupRetrievalResponse+json";

            // send request
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(this, e); }
                    return;
                }
                // parse response
                try
                {
                    JsonObject respObj = new JsonObject(response.Body);
                    
                    mId = respObj.GetString("groupID");
                    mOwnerId = respObj.OptString("owner", null);
                    groupName = respObj.GetString("name");
                }
                catch(JsonException)
                {
                    if (callback != null) { callback(this, new IllegalKiiBaseObjectFormatException(response.Body)); }
                    return;
                }
                if (callback != null) { callback(this, null); }
            });
        }

        private void ExecDelete(KiiHttpClientFactory factory, KiiGroupCallback callback)
        {
            Utils.CheckInitialize(true);
            if (Utils.IsEmpty(ID))
            {
                if (callback != null) { callback(this, new InvalidOperationException(ErrorInfo.KIIGROUP_NO_ID)); }
                return;
            }
            
            KiiHttpClient client = factory.Create(Url, Kii.AppId, Kii.AppKey, KiiHttpMethod.DELETE);
            KiiCloudEngine.SetAuthBearer(client);
            client.Accept = "application/vnd.kii.GroupRetrievalResponse+json";

            // send request
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(this, e); }
                    return;
                }
                // clear id and members
                mId = null;
                mOwnerId = null;
                addUsers.Clear();
                removeUsers.Clear();

                if (callback != null) { callback(this, null); }
            });
        }

        private void ExecChangeName(string name, KiiHttpClientFactory factory, KiiGroupCallback callback)
        {
            Utils.CheckInitialize(true);
            if (Utils.IsEmpty(name))
            {
                if (callback != null) { callback(this, new ArgumentException("provided name is null")); }
                return;
            }
            string id = ID;
            if (Utils.IsEmpty(id))
            {
                if (callback != null) { callback(this, new InvalidOperationException(ErrorInfo.KIIGROUP_NO_ID)); }
                return;
            }
            
            string url = Utils.Path(Url, "name");
            
            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.PUT);
            KiiCloudEngine.SetAuthBearer(client);
            client.ContentType = "text/plain";
            
            // send request
            client.SendRequest(name, (ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(this, e); }
                    return;
                }
                this.groupName = name;
                if (callback != null) { callback(this, null); }
            });
        }

        private void ExecListMembers(KiiHttpClientFactory factory, KiiUserListCallback callback)
        {
            Utils.CheckInitialize(true);
            string groupId = ID;
            if (Utils.IsEmpty(groupId))
            {
                if (callback != null) { callback(null, new InvalidOperationException(ErrorInfo.KIIGROUP_NO_ID)); }
                return;
            }
            
            string getUrl = Utils.Path(MembersUrl);
            
            KiiHttpClient client = factory.Create(getUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);
            client.Accept = "application/vnd.kii.MembersRetrievalResponse+json";

            // send request
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(null, e); }
                    return;
                }
                // parse response
                List<KiiUser> members = new List<KiiUser>();
                try
                {
                    JsonObject respObj = new JsonObject(response.Body);
                    JsonArray array = respObj.GetJsonArray("members");
                    if (array == null || array.Length() == 0)
                    {
                        if (callback != null) { callback(members, null); }
                        return;
                    }
                    for(int i = 0; i < array.Length(); i++)
                    {
                        JsonObject obj = array.GetJsonObject(i);
                        string id = obj.GetString("userID");
                        if (Utils.IsEmpty(id)) {
                            callback(null, new IllegalKiiBaseObjectFormatException(response.Body));
                            return;
                        }
                        KiiUser user = KiiUser.UserWithID(id);
                        members.Add(user);
                    }
                }
                catch (JsonException) 
                {
                    if (callback != null) { callback(null, new IllegalKiiBaseObjectFormatException(response.Body)); }
                    return;
                }
                if (callback != null) { callback(members, null); }
            });
        }
        private void ExecListTopics(KiiHttpClientFactory factory, string paginationKey, KiiGenericsCallback<KiiListResult<KiiTopic>> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiGenericsCallback must not be null");
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
            if (Utils.IsEmpty(ID))
            {
                callback(null, new InvalidOperationException(ErrorInfo.KIIGROUP_NO_ID));
                return;
            }
            String url = Utils.Path(Url, "topics");
            if (!String.IsNullOrEmpty(paginationKey))
            {
                url = url + "?paginationKey=" + Uri.EscapeUriString(paginationKey);
            }
            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);
            client.SendRequest((ApiResponse response, Exception e) => {
                if (e != null)
                {
                    callback(null, e);
                    return;
                }
                JsonObject json = new JsonObject(response.Body);
                String newPaginationKey = json.OptString("paginationKey", null);
                JsonArray array = json.GetJsonArray("topics");
                List<KiiTopic> topics = new List<KiiTopic>();
                for (int i = 0; i < array.Length(); i++)
                {
                    topics.Add(this.Topic(array.GetJsonObject(i).GetString("topicID")));
                }
                callback(new KiiListResult<KiiTopic>(topics, newPaginationKey), null);
            });
        }
        #endregion

        #region Blocking API
        private void SaveToCloud()
        {
            Utils.CheckInitialize(true);
            mOwnerId = Kii.CurrentUser.ID;
            string url = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId,
                    "groups");

            KiiHttpClient client = Kii.HttpClientFactory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer(client);
            client.Accept = "application/vnd.kii.GroupCreationResponse+json";

            JsonObject obj = new JsonObject();
            try
            {
                obj.Put("name", groupName);
                obj.Put("owner", mOwnerId);
                obj.Put("members", UserIds());
            }
            catch (JsonException e1)
            {
                throw new SystemException("unexpected error!", e1);
            }

            client.ContentType = "application/vnd.kii.GroupCreationRequest+json";
            client.Body = obj.ToString();

            // send Request
            ApiResponse res = null;
            try
            {
                res = client.SendRequest();
            }
            catch (CloudException e)
            {
                throw new GroupOperationException(e.Message, e, new List<KiiUser>(addUsers), null);
            }

            try
            {
                JsonObject respObj = new JsonObject(res.Body);
                mId = respObj.GetString("groupID");
                // TODO: parse response and get list of failed user (CMO-557)
            } catch (JsonException) {
                throw new IllegalKiiBaseObjectFormatException(res.Body);
            }
            addUsers.Clear();
        }

        private void AddMembersToCloud()
        {
            Utils.CheckInitialize(true);
            if (Utils.IsEmpty(ID)) {
                throw new InvalidOperationException(ErrorInfo.KIIGROUP_NO_ID);
            }
            List<KiiUser> userList = new List<KiiUser>(addUsers);
            foreach (KiiUser user in userList)
            {
                string url = Utils.Path(MembersUrl, user.ID);

                KiiHttpClient client = Kii.HttpClientFactory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.PUT);
                KiiCloudEngine.SetAuthBearer(client);
                try
                {
                    client.SendRequest();
                    addUsers.Remove(user);
                }
                catch (CloudException e)
                {
                    throw new GroupOperationException(e.Message, e,
                        new List<KiiUser>(addUsers), new List<KiiUser>(removeUsers));
                }
            }
        }

        private void RemoveMembersFromCloud()
        {
            Utils.CheckInitialize(true);
            if (Utils.IsEmpty(ID)) {
                throw new InvalidOperationException(ErrorInfo.KIIGROUP_NO_ID);
            }
            List<KiiUser> userList = new List<KiiUser>(removeUsers);
            foreach (KiiUser user in userList)
            {
                string url = Utils.Path(MembersUrl, user.ID);

                KiiHttpClient client = Kii.HttpClientFactory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.DELETE);
                KiiCloudEngine.SetAuthBearer(client);

                try
                {
                    client.SendRequest();
                    removeUsers.Remove(user);
                }
                catch (CloudException e)
                {
                    throw new GroupOperationException(e.Message, e, new List<KiiUser>(addUsers), new List<KiiUser>(removeUsers));
                }
            }
        }
        #endregion

        private JsonArray UserIds(){
            JsonArray ids = new JsonArray();
            foreach (KiiUser user in addUsers) {
                string uid = user.ID;
                if(!Utils.IsEmpty(uid))
                {
                    ids.Put(uid);
                }
            }
            return ids;
        }



        #region KiiSubject

        /// <summary>
        /// Gets the subject string.
        /// </summary>
        /// <remarks>
        /// Developers don't need to use this property in their apps.
        /// </remarks>
        /// <value>
        /// The subject string.
        /// </value>
        public string Subject
        {
            get
            {
                return "GroupID:" + ID;
            }
        }
        #endregion

        #region properties
        /// <summary>
        /// Returns the ID of this group.
        /// </summary>
        /// <remarks>
        /// If the group has not saved to the cloud, returns null.
        /// </remarks>
        /// <value>
        /// The group ID.
        /// </value>
        public string ID
        {
            get
            {
                return mId;
            }
        }

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        /// <remarks>
        /// Developers can use this name for UI label etc.
        /// </remarks>
        /// <value>
        /// The group name.
        /// </value>
        public string Name
        {
            get
            {
                return groupName;
            }
            internal set
            {
                groupName = value;
            }
        }

        internal string OwnerID
        {
            set
            {
                mOwnerId = value;
            }
        }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <remarks>
        /// Developers can use this for showing group owner.
        /// If displayName is needed, please call KiiUser.Refresh()/>
        /// </remarks>
        /// <value>
        /// The owner. Null if Group has been created by app admin.
        /// </value>
        public KiiUser Owner
        {
            get
            {
                if (Utils.IsEmpty(mOwnerId)) {
                    return null;
                }
                return KiiUser.UserWithID(mOwnerId);
            }
        }

        /// <summary>
        /// Instantiate KiiGroup that refers to existing group which has specified ID.
        /// </summary>
        /// <remarks>
        /// You have to specify the ID of existing KiiGroup. Unlike KiiObject,
        /// you can not assign ID in the client side.
        /// This API does not access to the server.
        /// After instantiation, call <see cref="Refresh()"/> to fetch the properties.
        /// </remarks>
        /// <returns>
        /// KiiGroup instance.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Is thrown when specified groupID is null or empty.
        /// </exception>
        /// <param name='groupID'>
        /// ID of the group to instantiate.
        /// </param>
        public static KiiGroup GroupWithID(string groupID)
        {
            if (Utils.IsEmpty(groupID)) {
                throw new ArgumentException("Specified groupID is null or invalid.");
            }
            KiiGroup group = new KiiGroup(null, null);
            group.mId = groupID;
            return group;
        }

        /// <summary>
        /// Gets the URI of this group.
        /// </summary>
        /// <remarks>
        /// Developers can use this for getting KiiGroup instance by <see cref="CreateByUri(Uri)"/>
        /// </remarks>
        /// <value>
        /// The URI.
        /// </value>
        public Uri Uri
        {
            get
            {
                Utils.CheckInitialize(false);
                if (Utils.IsEmpty(ID)) {
                    return null;
                }
                String url = Utils.Path(ConstantValues.URI_HEADER, KiiCloudAuthorityAndSegments);
                return new Uri(url);
            }
        }
        internal string KiiCloudAuthorityAndSegments
        {
            get
            {
                return Utils.Path("groups", ID);
            }
        }
        internal string Url
        {
            get
            {
                return Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, KiiCloudAuthorityAndSegments);
            }
        }
        internal string MembersUrl
        {
            get
            {
                return Utils.Path(Url, "members");
                
            }
        }
        #endregion

    }
}

