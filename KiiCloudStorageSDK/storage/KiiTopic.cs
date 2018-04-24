using System;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Topic on Kii push notification service
    /// </summary>
    /// <remarks>
    /// KiiTopic is entity can be a target sending/subscribe push message. 
    /// </remarks>
    public class KiiTopic : AccessControllable, KiiSubscribable
    {
        private KiiScope mScope;
        private string mName;

        internal KiiTopic (KiiScope scope, string name)
        {
            this.mScope = scope;
            this.mName = name;
        }

        #region Blocking APIs
        /// <summary>
        /// Delete the topic.Only the scope owner or topic creator is allowed to perform delete operation.
        /// </summary>
        /// <remarks></remarks>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void Delete()
        {
            this.ExecDelete (Kii.HttpClientFactory, (KiiTopic target, Exception e)=>{
                if (e != null)
                {
                    throw e;
                }
            });
        }
        /// <summary>
        /// Save this topic on KiiCloud.
        /// </summary>
        /// <remarks></remarks>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void Save()
        {
            this.ExecSave (Kii.HttpClientFactory, (KiiTopic target, Exception e)=>{
                if (e != null)
                {
                    throw e;
                }
            });
        }
        /// <summary>
        /// Send message to this topic.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="message">Message to send.</param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void SendMessage(KiiPushMessage message)
        {
            this.ExecSendMessage (message, Kii.HttpClientFactory, (KiiPushMessage target, Exception e)=>{
                if (e != null)
                {
                    throw e;
                }
            });
        }
        /// <summary>
        /// Checks whether the topic already exists or not.
        /// </summary>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public bool Exists()
        {
            Boolean? result = null;
            ExecExists(Kii.HttpClientFactory, (Boolean? existence, Exception e) => {
                if (e != null)
                {
                    throw e;
                }
                result = existence;
            });
            return (bool)result;
        }
        /// <summary>
        /// Lists the acl entries of this topic
        /// </summary>
        /// <returns>
        /// The list of acl entries.
        /// </returns>
        /// <exception cref='NotFoundException'>
        /// Is thrown when topic or scope is not in KiiCloud.
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public IList<KiiACLEntry<KiiTopic, TopicAction>> ListAclEntries()
        {
            return new KiiTopicACL(this).ListAclEntries();
        }
        #endregion

        #region Async APIs
        /// <summary>
        /// Asynchronous call for <see cref="Delete()"/>.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="callback">Callback.</param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void Delete(KiiTopicCallback callback)
        {
            this.ExecDelete (Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Asynchronous call of <see cref='Save()'/>
        /// </summary>
        /// <remarks></remarks>
        /// <param name="callback">Callback.</param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void Save(KiiTopicCallback callback)
        {
            this.ExecSave (Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Asynchronous call of <see cref='SendMessage(KiiPushMessage)'/>
        /// </summary>
        /// <remarks></remarks>
        /// <param name="message">Message to send.</param>
        /// <param name="callback">Callback.</param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void SendMessage(KiiPushMessage message, KiiPushMessageCallback callback)
        {
            this.ExecSendMessage (message, Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Asynchronous call for <see cref="Exists()"/>.
        /// </summary>
        /// <param name="callback">Callback.</param>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public void Exists(KiiGenericsCallback<Boolean?> callback)
        {
            this.ExecExists(Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Asynchronous call for <see cref="ListAclEntries()"/>.
        /// </summary>
        /// <returns>
        /// The list of acl entries.
        /// </returns>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public void ListAclEntries(KiiACLListCallback<KiiTopic, TopicAction> callback)
        {
            new KiiTopicACL(this).ListAclEntries(callback);
        }
        #endregion

        #region Execution
        private void ExecDelete(KiiHttpClientFactory factory, KiiTopicCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiTopicCallback must not be null");
            }
            Utils.CheckInitialize(true);
            KiiHttpClient client = factory.Create(Url, Kii.AppId, Kii.AppKey, KiiHttpMethod.DELETE);
            KiiCloudEngine.SetAuthBearer(client);
            client.SendRequest ((ApiResponse response, Exception e) => {
                callback(this, e);
            });
        }
        private void ExecSave(KiiHttpClientFactory factory, KiiTopicCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiTopicCallback must not be null");
            }
            Utils.CheckInitialize(false);
            KiiHttpClient client = factory.Create(Url, Kii.AppId, Kii.AppKey, KiiHttpMethod.PUT);
            KiiCloudEngine.SetAuthBearer(client);
            client.SendRequest ((ApiResponse response, Exception e) => {
                callback(this, e);
            });
        }
        private void ExecSendMessage(KiiPushMessage message, KiiHttpClientFactory factory, KiiPushMessageCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiPushMessageCallback must not be null");
            }
            if (message == null)
            {
                callback(message, new ArgumentNullException("KiiPushMessage must not be null"));
                return;
            }
            Utils.CheckInitialize(false);
            KiiHttpClient client = factory.Create(MessageUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer(client);
            client.ContentType = "application/vnd.kii.SendPushMessageRequest+json";
            client.SendRequest (message.ToJson().ToString(), (ApiResponse response, Exception e) => {
                callback(message, e);
            });
        }
        private void ExecExists(KiiHttpClientFactory factory, KiiGenericsCallback<Boolean?> callback)
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
            KiiHttpClient client = factory.Create(Url, Kii.AppId, Kii.AppKey, KiiHttpMethod.HEAD);
            KiiCloudEngine.SetAuthBearer(client);
            client.SendRequest((ApiResponse response, Exception e) => {
                if (e == null)
                {
                    callback(true, null);
                }
                else
                {
                    if (e is NotFoundException)
                    {
                        callback(false, null);
                    }
                    else
                    {
                        callback(null, e);
                    }
                }
            });
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the name of this instance.
        /// </summary>
        /// <remarks></remarks>
        /// <value>The name of this instance.</value>
        public string Name
        {
            get
            {
                return this.mName;
            }
        }
        /// <summary>
        /// Gets the URI of this instance
        /// </summary>
        /// <remarks></remarks>
        /// <value>The URI of this instance.</value>
        public Uri Uri{
            get
            {
                Utils.CheckInitialize(false);
                return new Uri(ConstantValues.URI_HEADER + this.KiiCloudAuthorityAndSegments);
            }
        }
        /// <summary>
        /// Acl the specified action.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="action">Action.</param>
        /// <returns>ACL of this KiiTopic.</returns>
        public KiiTopicACL Acl(TopicAction action)
        {
            return new KiiTopicACL(this, action);
        }
        /// <summary>
        /// Gets the kii cloud authority segment.
        /// </summary>
        /// <value>The kii cloud authority segment.</value>
        internal string KiiCloudAuthorityAndSegments
        {
            get
            {
                if (this.mScope == null) {
                    // application scope
                    return Utils.Path ("topics", this.mName);
                } else if (this.mScope is KiiGroup) {
                    // group scope
                    KiiGroup group = (KiiGroup)this.mScope;
                    return Utils.Path ("groups", group.ID, "topics", this.mName);
                } else if (this.mScope is KiiUser) {
                    // user scope
                    KiiUser user = (KiiUser)this.mScope;
                    return Utils.Path ("users", user.ID, "topics", this.mName);
                }
                // won't reach
                throw new InvalidOperationException (ErrorInfo.KIIBUCKET_UNKNOWN_SCOPE);
            }
        }
        /// <summary>
        /// Gets the ufp URL.
        /// </summary>
        /// <value>The ufp URL.</value>
        internal string Url
        {
            get
            {
                return Utils.Path(Kii.BaseUrl , "apps", Kii.AppId, this.KiiCloudAuthorityAndSegments);
            }
        }
        /// <summary>
        /// Gets the message URL.
        /// </summary>
        /// <value>The message URL.</value>
        internal string MessageUrl
        {
            get
            {
                return Utils.Path (Url, "push", "messages");
            }
        }
        #endregion
    }
}

