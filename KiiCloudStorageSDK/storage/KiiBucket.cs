using System;
using System.Collections.Generic;

using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides APIs for bucket features.
    /// </summary>
    /// <remarks>
    /// This instance is created by <see cref='KiiUser.Bucket(string)'/>
    /// </remarks>
    public class KiiBucket : KiiBaseBucket<KiiObject>, AccessControllable, KiiSubscribable
    {
        private KiiScope mParent;

        private string mBucketName;

        internal KiiBucket (KiiScope parent, string bucketName) {
            if (!IsValidBucketName(bucketName)) {
                throw new ArgumentException(ErrorInfo.KIIBUCKET_NAME_INVALID + bucketName);
            }
            this.mParent = parent;
            this.mBucketName = bucketName;
        }

        /// <summary>
        /// Create a new <see cref='KiiObject'/>.
        /// </summary>
        /// <remarks>
        /// The scope of created object is this bucket.
        /// </remarks>
        /// <returns>
        /// New KiiObject.
        /// </returns>
        public KiiObject NewKiiObject()
        {
            return new KiiObject(mParent, mBucketName);
        }
        /// <summary>
        /// Create a new <see cref='KiiObject'/> specifying its ID.
        /// </summary>
        /// <remarks>
        /// If the object has not exist on KiiCloud, <see cref='KiiObject.SaveAllFields(bool)'/> or
        /// <see cref='KiiObject.SaveAllFields(bool, KiiObjectCallback)'/>
        /// will create new Object which has ID specified in the argument.
        /// If the object exist in KiiCloud, references the existing object which has
        /// specified ID. Use <see cref='KiiObject.Refresh'/> to retrieve the contents of
        /// KiiObject.
        /// </remarks>
        /// <returns>New KiiObject.</returns>
        /// <param name="objectID">ID of KiiObject you want to instantiate.</param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when specified objectID is null/empty or not acceptable. Use
        /// <see cref='Utils.ValidateObjectID(string)'/> for details of acceptable string.
        /// </exception>
        public KiiObject NewKiiObject(string objectID)
        {
            return new KiiObject(mParent, mBucketName, objectID);
        }

        #region Blocking APIs
        /// <summary>
        /// Query KiiObjects in this bucket.
        /// </summary>
        /// <remarks>
        /// Query KiiObjects in this bucket with conditions given by argument.
        /// </remarks>
        /// <param name='query'>
        /// Query conditions.
        /// </param>
        /// <returns>
        /// List of KiiObject. If number of result is big, please call GetNextQueryResult()
        /// </returns>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends broken Json.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public KiiQueryResult<KiiObject> Query(KiiQuery query)
        {
            KiiQueryResult<KiiObject> result = null;
            ExecQuery(query, Kii.HttpClientFactory, (KiiQueryResult<KiiObject> r, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                result = r;
            });
            return result;
        }

        /// <summary>
        /// Execute count aggregation of all clause query on current bucket.
        /// </summary>
        /// <remarks>
        /// NOTE: This api access to server. Should not be executed in UI/Main thread.
        /// </remarks>
        /// <returns>
        /// number of objects in the bucket.
        /// </returns>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public int Count()
        {
            return Count ((KiiQuery)null);
        }

        /// <summary>
        /// Execute count aggregation of specified query on current bucket.
        /// </summary>
        /// <remarks>
        /// Query generated from <see cref = 'KiiQueryResult.NextKiiQuery'/> is not supported,
        /// <see cref = 'CloudException'/> will be thrown in this case.
        /// NOTE: This api access to server. Should not be executed in UI/Main thread.
        /// </remarks>
        /// <param name = 'query'>
        /// query to be executed. If null, the operation will be same as <see cref = 'Count'/>.
        /// </param>
        /// <returns>
        /// number of objects in the bucket.
        /// </returns>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public int Count(KiiQuery query)
        {
            int result = -1;
            ExecCount (query, Kii.HttpClientFactory, (KiiBucket b, KiiQuery q, int count, Exception e) =>
            {
                if (e != null) {
                    throw e;
                }
                result = count;
            });
            return result;
        }

        /// <summary>
        /// Delete this bucket.
        /// </summary>
        /// <remarks>
        /// Delete this bucket. All KiiObject in this bucket will be deleted at the same time.
        /// </remarks>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void Delete()
        {
            ExecDelete(Kii.HttpClientFactory, (KiiBucket bucket, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Lists the acl entries of this bucket
        /// </summary>
        /// <returns>
        /// The list of acl entries.
        /// </returns>
        public IList<KiiACLEntry<KiiBucket, BucketAction>> ListAclEntries()
        {
            return new KiiBucketAcl(this).ListAclEntries();
        }

        #endregion

        #region Async APIs
        /// <summary>
        /// Query KiiObjects in this bucket.
        /// </summary>
        /// <remarks>
        /// Query KiiObjects in this bucket with conditions given by argument.
        /// </remarks>
        /// <param name='query'>
        /// Query conditions.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        /// <returns>
        /// List of KiiObject. If number of result is big, please call GetNextQueryResult()
        /// </returns>
        public void Query(KiiQuery query, KiiQueryCallback<KiiObject> callback)
        {
            ExecQuery(query, Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Asynchronous version of <see cref='Count'/>.
        /// </summary>
        /// <remarks>
        /// This API sends a request to KiiCloud.
        /// </remarks>
        /// <param name = 'callback'>
        /// Executes when count execution completed.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when callback is null.
        /// </exception>
        public void Count(CountCallback callback)
        {
            Count (null, callback);
        }

        /// <summary>
        /// Asynchronous version of <see cref = 'Count(KiiQuery)'/>.
        /// </summary>
        /// <remarks>
        /// This API sends a request to KiiCloud.
        /// </remarks>
        /// <param name = 'callback'>
        /// Executes when count execution completed.
        /// </param>
        /// <param name = 'query'>
        /// query to be executed. If null, the operation will be same as <see cref = 'Count(CountCallback)'/>.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when callback is null.
        /// </exception>
        public void Count(KiiQuery query, CountCallback callback)
        {
            if(callback == null)
                throw new ArgumentException("Specified callback is null");
            ExecCount(query, Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Delete this bucket.
        /// </summary>
        /// <remarks>
        /// Delete this bucket. All KiiObject in this bucket will be deleted at the same time.
        /// </remarks>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void Delete(KiiBucketCallback callback)
        {
            ExecDelete(Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Lists the acl entries of this bucket
        /// </summary>
        /// <returns>
        /// The list of acl entries.
        /// </returns>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void ListAclEntries(KiiACLListCallback<KiiBucket, BucketAction> callback)
        {
            new KiiBucketAcl(this).ListAclEntries(callback);
        }
        
        #endregion

        #region Execution
        private void ExecQuery(KiiQuery query, KiiHttpClientFactory factory, KiiQueryCallback<KiiObject> callback)
        {
            Utils.CheckInitialize(false);
            
            KiiHttpClient client = factory.Create(QueryUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer(client);
            client.ContentType = "application/vnd.kii.QueryRequest+json";

            if (query == null) {
                query = new KiiQuery(null);
            }

            // send request
            client.SendRequest(query.ToString(), (ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(null, e); }
                    return;
                }
                // parse response
                KiiQueryResult<KiiObject> queryResult = null;
                try
                {
                    JsonObject obj = new JsonObject(response.Body);
                    JsonArray array = obj.GetJsonArray("results");
                    String nextPaginationKey = obj.OptString("nextPaginationKey");
                    queryResult = new KiiQueryResult<KiiObject>(query, nextPaginationKey, this, false);
                    
                    for (int i = 0; i < array.Length(); i++) {
                        JsonObject entry = array.GetJsonObject(i);
                        KiiObject kiiObject = new KiiObject(mParent, mBucketName, entry);
                        queryResult.Add(kiiObject);
                    }
                }
                catch (JsonException e2)
                {
                    if (callback != null) { callback(null, new IllegalKiiBaseObjectFormatException(e2.Message)); }
                    return;
                }
                if (callback != null) { callback(queryResult, null); }
            });
        }

        private void ExecCount(KiiQuery query, KiiHttpClientFactory factory, CountCallback callback)
        {
            Utils.CheckInitialize (false);
            if (query == null) {
                query = new KiiQuery (null);
            }

            KiiHttpClient client = factory.Create (QueryUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer (client);
            client.ContentType = "application/vnd.kii.QueryRequest+json";
            
            // send request
            string queryString = CountAggregationQuery (query);
            int count = 0;
            client.SendRequest (queryString, (ApiResponse response, Exception e) =>
            {
                if (e != null) {
                    callback (this, query, count, e);
                    return;
                }
                // parse response
                try {
                    JsonObject responseJson = new JsonObject (response.Body);
                    JsonObject aggregations = responseJson.GetJsonObject ("aggregations");
                    count = aggregations.GetInt ("count_field");
                    callback (this, query, count, null);
                } catch (JsonException jse) {
                    callback (this, query, count, new IllegalKiiBaseObjectFormatException (jse.Message));
                }
            });
        }

        private void ExecDelete(KiiHttpClientFactory factory, KiiBucketCallback callback)
        {
            Utils.CheckInitialize(false);
            
            KiiHttpClient client = factory.Create(Url, Kii.AppId, Kii.AppKey, KiiHttpMethod.DELETE);
            KiiCloudEngine.SetAuthBearer(client);

            // send request
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(this, e); }
                    return;
                }
                if (callback != null) { callback(this, null); }
            });
        }

        private string CountAggregationQuery(KiiQuery query) {
            // generate aggregations
            JsonArray aggregations = new JsonArray();
            JsonObject json = new JsonObject();
            json.Put("type", "COUNT");
            json.Put("putAggregationInto", "count_field");
            aggregations.Put(json);
            
            // add aggregations with bucketQuery.
            JsonObject queryJson = new JsonObject(query.ToString());
            JsonObject bucketQueryJson = queryJson.GetJsonObject("bucketQuery");
            bucketQueryJson.Put("aggregations", aggregations);
            
            // clobbering the new value of bucketQuery
            queryJson.Put("bucketQuery", bucketQueryJson);
            return queryJson.ToString();
        }
        #endregion

        /// <summary>
        /// Gets the acl for this bucket.
        /// </summary>
        /// <returns>
        /// KiiBucket ACL.
        /// </returns>
        /// <param name='action'>
        /// Bucket action.
        /// </param>
        public KiiBucketAcl Acl(BucketAction action)
        {
            return new KiiBucketAcl(this, action);
        }
        /// <summary>
        /// Determines whether an argument is valid bucket name.
        /// </summary>
        /// <remarks>Valid bucket name is
        /// <list type="bullet">
        ///   <item><term>Not null</term></item>
        ///   <item><term>Matches ^[a-zA-Z0-9-_]{1,50}$</term></item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// <c>true</c> if an argument is valid bucket name ; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='name'>
        /// Bucket name you want to check.
        /// </param>
        public static bool IsValidBucketName(string name)
        {
            return !Utils.IsEmpty(name) && Utils.ValidateBucketName(name);
        }

        #region properties
        /// <summary>
        /// Gets the bucket name.
        /// </summary>
        /// <value>The bucket name.</value>
        public string Name
        {
            get
            {
                return mBucketName;
            }
        }

        /// <summary>
        /// Gets the URI of this instance.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        public Uri Uri
        {
            get
            {
                return new Uri(Utils.Path(ConstantValues.URI_HEADER, KiiCloudAuthorityAndSegments));
            }
        }
        internal string QueryUrl 
        {
            get
            {
                return Utils.Path(Url, "query");
            }
        }
        internal String Url
        {
            get
            {
                return Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, KiiCloudAuthorityAndSegments);
            }
        }
        internal string KiiCloudAuthorityAndSegments
        {
            get
            {
                if (mParent == null) {
                    // application scope
                    return Utils.Path("buckets", mBucketName);
                }
                if (mParent is KiiUser) {
                    //user scope
                    string parentId = ((KiiUser)mParent).ID;
                    return Utils.Path("users", parentId, "buckets", mBucketName);
                }
                if (mParent is KiiGroup) {
                    string parentId = ((KiiGroup)mParent).ID;
                    return Utils.Path("groups", parentId, "buckets", mBucketName);
                }
                // won't reach
                throw new InvalidOperationException(ErrorInfo.KIIBUCKET_UNKNOWN_SCOPE);
            }
        }
        #endregion

    }
}

