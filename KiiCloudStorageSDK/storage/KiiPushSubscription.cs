using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides APIs of subscription.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class KiiPushSubscription
    {
        private KiiUser mUser;

        internal KiiPushSubscription (KiiUser user)
        {
            this.mUser = user;
        }

        #region Blocking APIs
        /// <summary>
        /// Checks whether the target is already subscribed by current user or not.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>true if the target is subscribed, otherwise false.</returns>
        /// <param name="target">to be checked for subscription existence.</param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public bool IsSubscribed (KiiSubscribable target)
        {
            bool ret = false;
            this.ExecIsSubscribed(target, Kii.HttpClientFactory, (KiiSubscribable subscribable, bool isSubscribed, Exception e)=>{
                if (e != null)
                {
                    throw e;
                }
                ret = isSubscribed;
            });
            return ret;
        }
        /// <summary>
        /// Subscribe the specified target.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name='target'>
        /// Target to subscribe.
        /// </param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void Subscribe (KiiSubscribable target)
        {
            this.ExecSubscribe(target, Kii.HttpClientFactory, (KiiSubscribable subscribable, Exception e)=>{
                if (e != null)
                {
                    throw e;
                }
            });
        }
        /// <summary>
        /// Unsubscribe the specified target.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name='target'>
        /// Target to subscribe.
        /// </param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void Unsubscribe (KiiSubscribable target)
        {
            this.ExecUnsubscribe(target, Kii.HttpClientFactory, (KiiSubscribable subscribable, Exception e)=>{
                if (e != null)
                {
                    throw e;
                }
            });
        }
        #endregion

        #region Async APIs
        /// <summary>
        /// Checks whether the target is already subscribed by current user or not.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="target">to be checked for subscription existence.</param>
        /// <param name="callback">
        /// Callback delegate. If exception is null, execution is succeeded.
        /// </param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void IsSubscribed (KiiSubscribable target, KiiCheckSubscriptionCallback callback)
        {
            this.ExecIsSubscribed(target, Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Subscribe the specified target.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name='target'>
        /// Target to subscribe.
        /// </param>
        /// <param name='callback'>
        /// Callback delegate. If exception is null, execution is succeeded.
        /// </param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void Subscribe (KiiSubscribable target, KiiSubscriptionCallback callback)
        {
            this.ExecSubscribe(target, Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Unsubscribe the specified target.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name='target'>
        /// Target to subscribe.
        /// </param>
        /// <param name='callback'>
        /// Callback delegate. If exception is null, execution is succeeded.
        /// </param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void Unsubscribe (KiiSubscribable target, KiiSubscriptionCallback callback)
        {
            this.ExecUnsubscribe(target, Kii.AsyncHttpClientFactory, callback);
        }
        #endregion

        #region Execution
        private void ExecIsSubscribed (KiiSubscribable target, KiiHttpClientFactory factory, KiiCheckSubscriptionCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiCheckSubscriptionCallback must not be null");
            }
            if (target == null)
            {
                callback(target, false, new ArgumentNullException("KiiSubscribable must not be null"));
                return;
            }
            Utils.CheckInitialize(true);
            KiiHttpClient client = factory.Create(ToUrl(target), Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);
            client.SendRequest ((ApiResponse response, Exception e) => {
                if (e != null)
                {
                    if (e is NotFoundException)
                    {
                        callback(target, false, null);
                    }
                    else
                    {
                        callback(target, false, e);
                    }
                }
                else
                {
                    callback(target, response.IsSuccess(), e);
                }
            });
        }
        private void ExecSubscribe (KiiSubscribable target, KiiHttpClientFactory factory, KiiSubscriptionCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiSubscriptionCallback must not be null");
            }
            if (target == null)
            {
                callback(target, new ArgumentNullException("KiiSubscribable must not be null"));
                return;
            }
            Utils.CheckInitialize(true);
            KiiHttpClient client = factory.Create(ToUrl(target), Kii.AppId, Kii.AppKey, KiiHttpMethod.PUT);
            KiiCloudEngine.SetAuthBearer(client);
            client.SendRequest ((ApiResponse response, Exception e) => {
                callback(target, e);
            });
        }
        private void ExecUnsubscribe (KiiSubscribable target, KiiHttpClientFactory factory, KiiSubscriptionCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiSubscriptionCallback must not be null");
            }
            if (target == null)
            {
                callback(target, new ArgumentNullException("KiiSubscribable must not be null"));
                return;
            }
            Utils.CheckInitialize(true);
            KiiHttpClient client = factory.Create(ToUrl(target), Kii.AppId, Kii.AppKey, KiiHttpMethod.DELETE);
            KiiCloudEngine.SetAuthBearer(client);
            client.SendRequest ((ApiResponse response, Exception e) => {
                callback(target, e);
            });
        }
        private String ToUrl(KiiSubscribable target)
        {
            // TODO:We need refactoring.
            // This logic should be implemented by each KiiSubscribable class.
            if (target is KiiBucket)
            {
                return Utils.Path(((KiiBucket)target).Url, "filters", "all", "push", "subscriptions", "users", this.mUser.ID);
            }
            else if (target is KiiTopic)
            {
                return Utils.Path(((KiiTopic)target).Url, "push", "subscriptions", "users", this.mUser.ID);
            }
            else
            {
                throw new InvalidOperationException("subscribing " + target.GetType() + " is not supported");
            }
        }
        #endregion
    }
}

