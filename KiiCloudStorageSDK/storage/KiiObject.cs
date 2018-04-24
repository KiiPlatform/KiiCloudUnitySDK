using System;
using System.Collections.Generic;
using System.IO;

using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides APIs for key-value Object CRUD operation on KiiCloud.
    /// </summary>
    /// <remarks>
    /// KiiObject works as key-value object.
    /// <code>
    /// // example 1. Create new object on KiiCloud.
    /// KiiObject obj = Kii.Bucket("hiscore").NewKiiObject();
    /// obj["name"] = "John";
    /// obj["score"] = 95;
    /// obj.Save();
    /// // example 2. Update an object.
    /// KiiObject obj = KiiObject.CreateByUri(uri);
    /// obj.Refresh();
    /// obj["score"] = 100;
    /// obj.Save();
    /// </code>
    /// </remarks>
    public class KiiObject : KiiBaseObject, AccessControllable
    {
        private KiiScope mScope;

        private string mBucket;

        private string mId;

        private long mModifiedTime = -1;

        private long mCreatedTime = -1;

        private string mEtag = null;

        private string mBodyContentType = null;

        // we will use this on later release
//        private bool mDeleted = false;


        internal KiiObject(KiiScope scope, string bucket) : base(null)
        {
            if (!Utils.ValidateBucketName(bucket))
            {
                throw new ArgumentException(
                        ErrorInfo.KIIBASEOBJECT_INVALID_FORMAT + bucket);
            }
            this.mJSON = new JsonObject();
            this.mBucket = bucket;
            this.mScope = scope;
        }

        internal KiiObject(KiiScope scope, string bucket, string objectID) : base(null)
        {
            if (!Utils.ValidateBucketName(bucket))
            {
                throw new ArgumentException(
                    ErrorInfo.KIIBASEOBJECT_INVALID_FORMAT + bucket);
            }
            if (Utils.IsEmpty(objectID))
            {
                throw new ArgumentException("Specified objectID is null or empty");
            }
            if (!Utils.ValidateObjectID(objectID))
            {
                throw new ArgumentException("Specified objectID is invalid");
            }
            this.mJSON = new JsonObject();
            this.mBucket = bucket;
            this.mScope = scope;
            this.mId = objectID;
        }

        internal KiiObject(KiiScope scope, string bucket, JsonObject obj) : base(obj, null) {
            // TODO: define reserved keys.
            if (!Utils.ValidateBucketName(bucket)) {
                throw new ArgumentException(ErrorInfo.KIIBASEOBJECT_INVALID_FORMAT + bucket);
            }
            mBucket = bucket;
            mScope = scope;
            try {
                mId = obj.GetString("_id");
                mModifiedTime = obj.GetLong("_modified");
                mCreatedTime = obj.GetLong("_created");
                mEtag = obj.GetString("_version");
            } catch (JsonException e) {
                throw new SystemException("Unexpected error!", e);
            }
        }

        internal KiiObject(Uri uri) : base(null)
        {
            if (uri == null)
            {
                throw new ArgumentException(ErrorInfo.KIIBASEOBJECT_URL_IS_NULL);
            }
            
            string scheme = uri.Scheme;
            if (scheme != ConstantValues.URI_SCHEME)
            {
                throw new ArgumentException(ErrorInfo.KIIBASEOBJECT_URI_NO_SUPPORT + uri);
            }

            string authority = uri.Authority;
            string[] segments = uri.Segments;
            if (authority == "buckets")
            {
                // application-scope object
                // kiicloud://buckets/BUCKET_NAME/objects/OBJECT_ID
                if (segments.Length != 4) {
                    throw new ArgumentException(ErrorInfo.KIIBASEOBJECT_URI_NO_SUPPORT + uri + "l=" + segments.Length);
                }
                // segments[2] must be 'objects'
                if (segments[2] != "objects/")
                {
                    throw new ArgumentException(ErrorInfo.KIIBASEOBJECT_URI_NO_SUPPORT + uri);
                }
                mScope = null;
                mBucket = segments[1];
                mId = segments[3];
            }
            else if (authority ==  "users")
            {
                // user scope
                // kiicloud://users/USER_ID/bucket/BUCKET_NAME/objects/OBJECT_ID
                if (segments.Length != 6)
                {
                    throw new ArgumentException(ErrorInfo.KIIBASEOBJECT_URI_NO_SUPPORT + uri);
                }
                // segments[2] must be 'buckets'
                if (segments[2] != "buckets/")
                {
                    throw new ArgumentException(ErrorInfo.KIIBASEOBJECT_URI_NO_SUPPORT + uri);
                }
                // segments[4] must be 'objects'
                if (segments[4] != "objects/")
                {
                    throw new ArgumentException(ErrorInfo.KIIBASEOBJECT_URI_NO_SUPPORT + uri);
                }
                mScope = KiiUser.CreateByUri(new Uri(Utils.Path(ConstantValues.URI_HEADER, "users", segments[1])));
                mBucket = segments[3];
                mId = segments[5];
            }
            else if (authority == "groups")
            {
                // group scope
                // kiicloud://groups/GROUP_ID/bucket/BUCKET_NAME/objects/OBJECT_ID
                if (segments.Length != 6) {
                    throw new ArgumentException(ErrorInfo.KIIBASEOBJECT_URI_NO_SUPPORT + uri);
                }
                // segments[2] must be 'buckets'
                if (segments[2] != "buckets/")
                {
                    throw new ArgumentException(ErrorInfo.KIIBASEOBJECT_URI_NO_SUPPORT + uri);
                }
                // segments[4] must be 'objects'
                if (segments[4] != "objects/")
                {
                    throw new ArgumentException(ErrorInfo.KIIBASEOBJECT_URI_NO_SUPPORT + uri);
                }
                mScope = KiiGroup.CreateByUri(new Uri(Utils.Path(ConstantValues.URI_HEADER, "groups", segments[1])));
                mBucket = segments[3];
                mId = segments[5];
            }
    
            mJSON = new JsonObject();
        }

        /// <summary>
        /// Creates KiiObject by Uri
        /// </summary>
        /// <remarks>
        /// Please use <see cref="Uri"/> property for an argument.
        /// </remarks>
        /// <returns>
        /// KiiObject instance.
        /// </returns>
        /// <param name='uri'>
        /// Uri of KiiObject
        /// </param>
        public static KiiObject CreateByUri(Uri uri)
        {
            return new KiiObject(uri);
        }

        #region Blocking APIs
        /// <summary>
        /// Create or update the KiiObject on KiiCloud.
        /// </summary>
        /// <remarks>
        /// This API uploads only modified fields.
        /// </remarks>
        /// <remarks>
        /// This call is same as KiiObject.Save(true)
        /// </remarks>
        public void Save()
        {
            Save (true);
        }

        /// <summary>
        /// Create or update the KiiObject on KiiCloud.
        /// </summary>
        /// <remarks>
        /// This API uploads only modified fields.
        /// </remarks>
        /// <param name='overWrite'>
        /// Over write.
        /// </param>
        public void Save(bool overWrite)
        {
            CheckScope();
            if (Utils.IsEmpty(ID))
            {
                ExecSaveToCloud(Kii.HttpClientFactory, (KiiObject obj, Exception e) =>
                {
                    if (e != null)
                    {
                        throw e;
                    }
                });
            }
            else
            {
                ExecUpdateToCloud(true, overWrite, Kii.HttpClientFactory, (KiiObject obj, Exception e) =>
                {
                    if (e != null)
                    {
                        throw e;
                    }
                });
            }
        }

        /// <summary>
        /// Create or update the KiiObject on KiiCloud.
        /// </summary>
        /// <remarks>
        /// When this API updates object in KiiCloud,
        /// object's all fields in KiiCloud are replaced by fields in this instance.
        /// </remarks>
        /// <param name='overWrite'>
        /// If <code>false</code> and object in KiiCloud is updated, <see cref="CloudException"/> is thrown.
        /// </param>
        public void SaveAllFields(bool overWrite)
        {
            CheckScope();
            if (Utils.IsEmpty(ID))
            {
                ExecSaveToCloud(Kii.HttpClientFactory, (KiiObject obj, Exception e) =>
                {
                    if (e != null)
                    {
                        throw e;
                    }
                });
            }
            else
            {
                ExecUpdateToCloud(false, overWrite, Kii.HttpClientFactory, (KiiObject obj, Exception e) =>
                {
                    if (e != null)
                    {
                        throw e;
                    }
                });
            }
        }

        /// <summary>
        /// Upload the body of the KiiObject to KiiCloud.
        /// </summary>
        /// <remarks>
        /// <para>The stream will be closed after execution.</para>
        /// <para>NOTE: This api access to server. Should not be executed in UI/Main thread.</para>
        /// <para>NOTE: After this operation, KiiObject version on cloud will be updated. If you want to use <see cref="Save(bool)"/> with overwrite=false argument, please do <see cref="Refresh()"/> before saving.</para>
        /// </remarks>
        /// <param name="contentType">Content type of body.</param>
        /// <param name="stream">The object body to be uploaded. This stream will be closed after execution.</param>
        /// <exception cref="NotFoundException">
        /// Is thrown when KiiObject is not in KiiCloud.
        /// </exception>
        /// <exception cref="UnauthorizedException">
        /// Is thrown when current user cannot access this KiiObject.
        /// </exception>
        public void UploadBody(string contentType, Stream stream)
        {
            ExecUploadBody(contentType, stream, Kii.HttpClientFactory, (KiiObject obj, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            }, null, null);
        }

        /// <summary>
        /// Download the body of the KiiObject from KiiCloud.
        /// </summary>
        /// <remarks>
        /// <para>The stream will <b>NOT</b> be closed after executon.</para>
        /// <para>NOTE: This api access to server. Should not be executed in UI/Main thread.</para>
        /// </remarks>
        /// <param name="outStream">The stream to be written. This stream is NOT closed after execution.</param>
        /// <exception cref="NotFoundException">
        /// Is thrown when KiiObject or body is not in KiiCloud.
        /// </exception>
        /// <exception cref="UnauthorizedException">
        /// Is thrown when current user cannot access this KiiObject.
        /// </exception>
        public void DownloadBody(Stream outStream)
        {
            ExecDownloadBody(outStream, Kii.HttpClientFactory, (KiiObject obj, Stream stream, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            },
            null, null);
        }

        /// <summary>
        /// Get latest KiiObject form KiiCloud and refresh properties.
        /// </summary>
        /// <remarks>
        /// Please call this API to the object created by <see cref="CreateByUri(Uri)"/>
        /// </remarks>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when this instance doesn't have id or bucket.
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends broken Json.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void Refresh()
        {
            ExecRefresh(Kii.HttpClientFactory, (KiiObject obj, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Delete KiiObject from KiiCloud
        /// </summary>
        /// <remarks>
        /// Do not use this instance after this API is called.
        /// </remarks>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when this instance doesn't have id or bucket.
        /// </exception>
        /// <exception cref="CloudException">
        /// Is thrown when server sends error response.
        /// </exception>
        public void Delete()
        {
            ExecDelete(Kii.HttpClientFactory, (KiiObject obj, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Delete the body of the KiiObject from KiiCloud.
        /// </summary>
        /// <remarks>
        /// NOTE: This api access to server. Should not be executed in UI/Main thread.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if this object has not saved to the Kii Cloud.
        /// </exception>
        /// <exception cref="NotFoundException">
        /// Thrown if object body,object,bucket or bucket owner not exists in Kii Cloud.
        /// </exception>
        /// <exception cref="UnauthorizedException">
        /// Thrown if current user has no permission to delete the KiiObject body.
        /// </exception>
        public void DeleteBody()
        {
            ExecDeleteBody(Kii.HttpClientFactory, (KiiObject obj, Exception e) =>
                       {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Lists the acl entries of this object
        /// </summary>
        /// <remarks>
        /// This API sends a request to KiiCloud.
        /// </remarks>
        /// <returns>
        /// The list of acl entries.
        /// </returns>
        public IList<KiiACLEntry<KiiObject, ObjectAction>> ListAclEntries()
        {
            return new KiiObjectAcl(this).ListAclEntries();
        }

        /// <summary>
        /// Publishes the KiiObject attached file and return the file URL. URL will not be expired.
        /// </summary>
        /// <remarks>
        /// NOTE: This api access to server. Should not be executed in UI/Main thread.
        /// </remarks>
        /// <returns>
        /// The file URL.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Is thrown when this object is not uploaded in Kii Cloud.
        /// </exception>
        /// <exception cref="NotFoundException">
        /// Is thrown when Object / Bucket / Bucket owner is not in Kii Cloud.
        /// </exception>
        /// <exception cref="UnauthorizedException">
        /// Is throwns when current user cannot access this KiiObject.
        /// </exception>
        public string PublishBody()
        {
            string result = null;
            ExecPublishBody(Kii.HttpClientFactory, (KiiObject obj, string url, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                result = url;
            });
            return result;
        }

        /// <summary>
        /// Publishes the KiiObject attached file and return the file URL. URL will be
        /// expired on the specified expire time.
        /// </summary>
        /// <remarks>
        /// NOTE: This api access to server. Should not be executed in UI/Main thread.
        /// </remarks>
        /// <param name="expiresAt">
        /// A time of expired date in milliseconds (Since January 1, 1970 00:00:00 UTC).
        /// </param>
        /// <returns>
        /// The file URL.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Is thrown when this object is not uploaded in Kii Cloud.
        /// </exception>
        /// <exception cref="NotFoundException">
        /// Is thrown when Object / Bucket / Bucket owner is not in Kii Cloud.
        /// </exception>
        /// <exception cref="UnauthorizedException">
        /// Is throwns when current user cannot access this KiiObject.
        /// </exception>
        public string PublishBodyExpiresAt(DateTime expiresAt)
        {
            string result = null;
            ExecPublishBodyExpiresAt(expiresAt, Kii.HttpClientFactory, (KiiObject obj, string url, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                result = url;
            });
            return result;
        }

        /// <summary>
        /// Publishes the KiiObject attached file and return the file URL. URL will be
        /// expired on the specified expire time.
        /// </summary>
        /// <remarks>
        /// NOTE: This api access to server. Should not be executed in UI/Main thread.
        /// </remarks>
        /// <param name="expiresIn">
        /// The period time in seconds the publication URL has to be available, after that it will expire.
        /// </param>
        /// <returns>
        /// The file URL.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Is thrown when this object is not uploaded in Kii Cloud.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when expiresIn &lt;= 0
        /// </exception>
        /// <exception cref="NotFoundException">
        /// Is thrown when Object / Bucket / Bucket owner is not in Kii Cloud.
        /// </exception>
        /// <exception cref="UnauthorizedException">
        /// Is throwns when current user cannot access this KiiObject.
        /// </exception>
        public string PublishBodyExpiresIn(long expiresIn)
        {
            string result = null;
            ExecPublishBodyExpiresIn(expiresIn, Kii.HttpClientFactory, (KiiObject obj, string url, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                result = url;
            });
            return result;
        }

        #endregion

        #region Async APIs
        /// <summary>
        /// Create or update the KiiObject on KiiCloud.
        /// </summary>
        /// <remarks>
        /// This API uploads only modified fields.
        /// </remarks>
        /// <remarks>
        /// This call is same as KiiObject.Save(true)
        /// </remarks>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void Save(KiiObjectCallback callback)
        {
            Save (true, callback);
        }
        
        /// <summary>
        /// Create or update the KiiObject on KiiCloud.
        /// </summary>
        /// <remarks>
        /// This API uploads only modified fields.
        /// </remarks>
        /// <param name='overWrite'>
        /// Over write.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void Save(bool overWrite, KiiObjectCallback callback)
        {
            CheckScope();
            if (Utils.IsEmpty(ID))
            {
                ExecSaveToCloud(Kii.AsyncHttpClientFactory, callback);
            }
            else
            {
                ExecUpdateToCloud(true, overWrite, Kii.AsyncHttpClientFactory, callback);
            }
        }

        /// <summary>
        /// Create or update the KiiObject on KiiCloud.
        /// </summary>
        /// <remarks>
        /// When this API updates object in KiiCloud,
        /// object's all fields in KiiCloud are replaced by fields in this instance.
        /// </remarks>
        /// <param name='overWrite'>
        /// If <code>false</code> and object in KiiCloud is updated, <see cref="CloudException"/> is thrown.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void SaveAllFields(bool overWrite, KiiObjectCallback callback)
        {
            CheckScope();
            if (Utils.IsEmpty(ID))
            {
                ExecSaveToCloud(Kii.AsyncHttpClientFactory, callback);
            }
            else
            {
                ExecUpdateToCloud(false, overWrite, Kii.AsyncHttpClientFactory, callback);
            }
        }

        /// <summary>
        /// Upload the body of the KiiObject to KiiCloud. This API will be executed asynchronously. 
        /// </summary>
        /// <remarks>
        /// <para>The stream will be closed after execution.</para>
        /// <para>NOTE: After this operation, KiiObject version on cloud will be updated. If you want to use <see cref="Save(bool)"/> with overwrite=false argument, please do <see cref="Refresh()"/> before saving.</para>
        /// </remarks>
        /// <param name="contentType">Content type of body.</param>
        /// <param name="stream">The object body to be uploaded.</param>
        /// <param name="callback">Callback.</param>
        public void UploadBody(string contentType, Stream stream, KiiObjectCallback callback)
        {
            UploadBody(contentType, stream, callback, (KiiObjectBodyProgressCallback)null);
        }
        
        /// <summary>
        /// Upload the body of the KiiObject to KiiCloud. This API will be executed asynchronously. 
        /// </summary>
        /// <remarks>
        /// KiiObjectBodyProgressCallback doesn't work properly when use KiiInitializeBehaviour in order to initialize the KiiCloudSDK.
        /// Use <see cref="UploadBody(string, Stream, KiiObjectCallback, KiiObjectBodyProgressPercentageCallback)"/> instead.
        /// <para>The stream will be closed after execution.</para>
        /// <para>NOTE: After this operation, KiiObject version on cloud will be updated. If you want to use <see cref="Save(bool)"/> with overwrite=false argument, please do <see cref="Refresh()"/> before saving.</para>
        /// </remarks>
        /// <param name="contentType">Content type of body.</param>
        /// <param name="stream">The object body to be uploaded.</param>
        /// <param name="callback">Callback.</param>
        /// <param name="progressCallback">This callback will be called when API will tell a progress.</param>
        public void UploadBody(string contentType, Stream stream, KiiObjectCallback callback, KiiObjectBodyProgressCallback progressCallback)
        {
            ExecUploadBody(contentType, stream, Kii.AsyncHttpClientFactory, callback, progressCallback, null);
        }

        /// <summary>
        /// Upload the body of the KiiObject to KiiCloud. This API will be executed asynchronously. 
        /// </summary>
        /// <remarks>
        /// <para>The stream will be closed after execution.</para>
        /// <para>NOTE: After this operation, KiiObject version on cloud will be updated. If you want to use <see cref="Save(bool)"/> with overwrite=false argument, please do <see cref="Refresh()"/> before saving.</para>
        /// </remarks>
        /// <param name="contentType">Content type of body.</param>
        /// <param name="stream">The object body to be uploaded.</param>
        /// <param name="callback">Callback.</param>
        /// <param name="progressCallback">This callback will be called when API will tell a progress.</param>
        public void  UploadBody(string contentType, Stream stream, KiiObjectCallback callback, KiiObjectBodyProgressPercentageCallback progressCallback)
        {
            ExecUploadBody(contentType, stream, Kii.AsyncHttpClientFactory, callback, null, progressCallback);
        }

        /// <summary>
        /// Download the body of the KiiObject from KiiCloud. This API will be executed asynchronously.
        /// </summary>
        /// <remarks>
        /// The stream will <b>NOT</b> be closed after executon.
        /// </remarks>
        /// <param name="outStream">The stream to be written. This stream is NOT closed after execution.</param>
        /// <param name="callback">Callback. </param> 
        public void DownloadBody(Stream outStream, KiiObjectBodyDownloadCallback callback)
        {
            DownloadBody(outStream, callback, (KiiObjectBodyProgressCallback)null);
        }

        /// <summary>
        /// Download the body of the KiiObject from KiiCloud. This API will be executed asynchronously.
        /// </summary>
        /// <remarks>
        /// KiiObjectBodyProgressCallback doesn't work properly when use KiiInitializeBehaviour in order to initialize the KiiCloudSDK.
        /// Use <see cref="DownloadBody(Stream, KiiObjectBodyDownloadCallback, KiiObjectBodyProgressPercentageCallback)"/> instead.
        /// The stream will <b>NOT</b> be closed after executon.
        /// </remarks>
        /// <param name="outStream">The stream to be written. This stream is NOT closed after execution.</param>
        /// <param name="callback">Callback. </param> 
        /// <param name="progressCallback">This callback will be called when API will tell a progress.</param>
        public void DownloadBody(Stream outStream, KiiObjectBodyDownloadCallback callback, KiiObjectBodyProgressCallback progressCallback)
        {
            ExecDownloadBody(outStream, Kii.AsyncHttpClientFactory, callback, progressCallback, null);
        }

        /// <summary>
        /// Download the body of the KiiObject from KiiCloud. This API will be executed asynchronously.
        /// </summary>
        /// <remarks>
        /// The stream will <b>NOT</b> be closed after executon.
        /// </remarks>
        /// <param name="outStream">The stream to be written. This stream is NOT closed after execution.</param>
        /// <param name="callback">Callback. </param> 
        /// <param name="progressCallback">This callback will be called when API will tell a progress.</param>
        public void DownloadBody(Stream outStream, KiiObjectBodyDownloadCallback callback, KiiObjectBodyProgressPercentageCallback progressCallback)
        {
            ExecDownloadBody(outStream, Kii.AsyncHttpClientFactory, callback, null, progressCallback);
        }

        /// <summary>
        /// Get latest KiiObject form KiiCloud and refresh properties.
        /// </summary>
        /// <remarks>
        /// Please call this API to the object created by <see cref="CreateByUri(Uri)"/>
        /// </remarks>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void Refresh(KiiObjectCallback callback)
        {
            ExecRefresh(Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Delete KiiObject from KiiCloud
        /// </summary>
        /// <remarks>
        /// Do not use this instance after this API is called.
        /// </remarks>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void Delete(KiiObjectCallback callback)
        {
            ExecDelete(Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Delete the body of the KiiObject from KiiCloud.
        /// </summary>
        /// <remarks>
        /// This API sends a request to KiiCloud.
        /// </remarks>
        /// <param name='callback'>
        /// KiiObjectCallback.
        /// </param>
        public void DeleteBody(KiiObjectCallback callback)
        {
            ExecDeleteBody(Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Lists the acl entries of this object
        /// </summary>
        /// <remarks>
        /// This API sends a request to KiiCloud.
        /// </remarks>
        /// <returns>
        /// The list of acl entries.
        /// </returns>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void ListAclEntries(KiiACLListCallback<KiiObject, ObjectAction> callback)
        {
            new KiiObjectAcl(this).ListAclEntries(callback);
        }

        /// <summary>
        /// Publishes the KiiObject attached file and return the file URL. URL will not be expired.
        /// </summary>
        /// <remarks>
        /// This API sends a request to KiiCloud. 
        /// </remarks>
        /// <param name="callback">
        /// Callback. 
        /// </param>
        public void PublishBody(KiiObjectPublishCallback callback)
        {
            ExecPublishBody(Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Publishes the KiiObject attached file and return the file URL. URL will be expired on the specified expire time.
        /// </summary>
        /// <remarks>
        /// This API sends a request to KiiCloud. 
        /// </remarks>
        /// <param name="expiresAt">
        /// A time of expired date in milliseconds (Since January 1, 1970 00:00:00 UTC).
        /// </param>
        /// <param name="callback">
        /// Callback. 
        /// </param>
        public void PublishBodyExpiresAt(DateTime expiresAt, KiiObjectPublishCallback callback)
        {
            ExecPublishBodyExpiresAt(expiresAt, Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Publishes the KiiObject attached file and return the file URL. URL will be expired on the specified expire time.
        /// </summary>
        /// <remarks>
        /// This API sends a request to KiiCloud. 
        /// </remarks>
        /// <param name="expiresIn">
        /// The period time in seconds the publication URL has to be available, after that it will expire.
        /// </param>
        /// <param name="callback">
        /// Callback. 
        /// </param>
        public void PublishBodyExpiresIn(long expiresIn, KiiObjectPublishCallback callback)
        {
            ExecPublishBodyExpiresIn(expiresIn, Kii.AsyncHttpClientFactory, callback);
        }
        #endregion

        #region Execution
        private void ExecSaveToCloud(KiiHttpClientFactory factory, KiiObjectCallback callback)
        {
            Utils.CheckInitialize(false);
            
            KiiHttpClient client = factory.Create(UrlWithoutID, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer(client);
            // TODO: confirm content-type. CMO-193.
            client.ContentType = "application/json";
            
            string entityToCloud = mJSON.ToString();
            // send request
            client.SendRequest(entityToCloud, (ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(this, e); }
                    return;
                }
                mEtag = response.ETag;
                // parse response
                try 
                {
                    JsonObject respObj = new JsonObject(response.Body);
                    String retUuid = respObj.GetString("objectID");
                    mId = retUuid;
                    mCreatedTime = respObj.GetLong("createdAt");
                    mModifiedTime = respObj.OptLong("updatedAt", -1);
                    if (mModifiedTime == -1)
                    {
                        mModifiedTime = mCreatedTime;
                    }
                    mJSONPatch = new JsonObject();
                }
                catch (JsonException e2) 
                {
                    if (callback != null) { callback(this, new IllegalKiiBaseObjectFormatException(e2.Message)); }
                    return;
                }
                if (callback != null) { callback(this, null); }
            });
        }

        private void ExecUpdateToCloud(bool patch, bool overWrite, KiiHttpClientFactory factory, KiiObjectCallback callback)
        {
            Utils.CheckInitialize(false);
            
            KiiHttpClient client;
            string entityToCloud;
            if (patch) 
            {
                client = factory.Create(Utils.Path(UrlWithoutID, ID), Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
                client.Headers["X-HTTP-Method-Override"] =  "PATCH";
                entityToCloud= mJSONPatch.ToString();
            }
            else 
            {
                client = factory.Create(Utils.Path(UrlWithoutID, ID), Kii.AppId, Kii.AppKey, KiiHttpMethod.PUT);
                entityToCloud= mJSON.ToString();
            }
            KiiCloudEngine.SetAuthBearer(client);
            if (!overWrite) 
            {
                if (IsSaved)
                {
                    if(mEtag == null)
                    {
                        if (callback != null) { callback(this, new InvalidOperationException("IllegalState, refresh the kiiobject before call this method")); }
                        return;
                    }
                    client.Headers["If-Match"] =  mEtag;
                }
                else
                {
                    if (patch && Utils.IsEmpty(mEtag)) {
                        if (callback != null) { callback(this, new InvalidOperationException("Can not create or update KiiObject in this state. If you want to update KiiObject, please call KiiObject#refresh() before call this method.")); }
                        return;
                    }
                    client.Headers["If-None-Match"] = "*";
                }
            }
            
            client.ContentType = "application/json";

            // send request
            client.SendRequest(entityToCloud, (ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(this, e); }
                    return;
                }
                mEtag = response.ETag;
                // parse response
                try
                {
                    JsonObject respObj = new JsonObject(response.Body);
                    string retUuid = respObj.OptString("objectID", null);
                    if (!Utils.IsEmpty(retUuid))
                    {
                        mId = retUuid;
                    }
                    if (patch) 
                    {
                        mCreatedTime = respObj.GetLong("_created");
                        mModifiedTime = respObj.GetLong("_modified");
                    }
                    else 
                    {
                        mModifiedTime = respObj.GetLong("modifiedAt");
                        if (respObj.Has("createdAt"))
                        {
                            mCreatedTime =respObj.GetLong("createdAt");
                        }
                    }
                    mJSONPatch = new JsonObject();
                }
                catch (JsonException e2) 
                {
                    if (callback != null) { callback(this, new IllegalKiiBaseObjectFormatException(e2.Message)); }
                    return;
                }
                if (callback != null) { callback(this, null); }
            });
        }

        private void ExecUploadBody(string contentType, Stream stream, KiiHttpClientFactory factory, 
                                    KiiObjectCallback callback, KiiObjectBodyProgressCallback progressCallback,
                                    KiiObjectBodyProgressPercentageCallback progressPercentageCallback)
        {
            // argument validation
            if (Utils.IsEmpty(contentType))
            {
                if (callback != null) { callback(this, new ArgumentException("contentType must not be empty.")); }
                return;
            }

            if (stream == null)
            {
                if (callback != null) { callback(this, new ArgumentException("stream must not be empty.")); }
                return;
            }

            if (!stream.CanRead)
            {
                if (callback != null) { callback(this, new ArgumentException("Please pass readable stream.")); }
                return;
            }

            Utils.CheckInitialize(false);
            
            string bucket = Bucket;
            if (Utils.IsEmpty(bucket))
            {
                if (callback != null) { callback(this, new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_MISS_ENTITY)); }
                return;
            }
            
            if (Utils.IsEmpty(ID)) 
            {
                if (callback != null) { callback(this, new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_NO_ID)); }
                return;
            }
            
            string url = Utils.Path(UrlWithoutID, ID, "body");

            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.PUT);
            KiiCloudEngine.SetAuthBearer(client);
            client.ContentType = contentType;

            // send request

            if (progressPercentageCallback != null)
            {
                client.SendRequest(stream, (float progress) => {
                    progressPercentageCallback(this, progress);
                },
                (ApiResponse response, Exception e) =>
                {
                    try
                    {
                        stream.Close();
                    }
                    catch (Exception e1)
                    {
                        if (callback != null) { callback(this, e1); }
                        return;
                    }
                    if (e != null)
                    {
                        if (callback != null) { callback(this, e); }
                        return;
                    }
                    // TODO : Review eTag value due to KiiCorp/AndroidStorageSDK#1469
                    //mEtag = response.ETag;
                    
                    // parse response
                    try
                    {
                        JsonObject respObj = new JsonObject(response.Body);
                        mModifiedTime = respObj.GetLong("modifiedAt");
                    }
                    catch (JsonException e2) 
                    {
                        if (callback != null) { callback(this, new IllegalKiiBaseObjectFormatException(e2.Message)); }
                        return;
                    }
                    
                    // set contentType 
                    mBodyContentType = contentType;
                    if (callback != null) { callback(this, null); }
                });
            }
            else
            {
                client.SendRequest(stream, (long doneByte, long totalByte) => 
                                   {
                    if (progressCallback != null) { progressCallback(this, doneByte, totalByte); }
                },
                (ApiResponse response, Exception e) =>
                {
                    try
                    {
                        stream.Close();
                    }
                    catch (Exception e1)
                    {
                        if (callback != null) { callback(this, e1); }
                        return;
                    }
                    if (e != null)
                    {
                        if (callback != null) { callback(this, e); }
                        return;
                    }
                    // TODO : Review eTag value due to KiiCorp/AndroidStorageSDK#1469
                    //mEtag = response.ETag;
                    
                    // parse response
                    try
                    {
                        JsonObject respObj = new JsonObject(response.Body);
                        mModifiedTime = respObj.GetLong("modifiedAt");
                    }
                    catch (JsonException e2) 
                    {
                        if (callback != null) { callback(this, new IllegalKiiBaseObjectFormatException(e2.Message)); }
                        return;
                    }
                    
                    // set contentType 
                    mBodyContentType = contentType;
                    if (callback != null) { callback(this, null); }
                });
            }
        }
        private void ExecDownloadBody(Stream stream, KiiHttpClientFactory factory, 
                                      KiiObjectBodyDownloadCallback callback, KiiObjectBodyProgressCallback progressCallback,
                                      KiiObjectBodyProgressPercentageCallback progressPercentageCallback)
        {
            // argument validation
            if (stream == null)
            {
                if (callback != null) { callback(this, stream, new ArgumentException("stream must not be empty.")); }
                return;
            }
            
            if (!stream.CanWrite)
            {
                if (callback != null) { callback(this, stream, new ArgumentException("Please pass writable stream.")); }
                return;
            }

            Utils.CheckInitialize(false);
            
            string bucket = Bucket;
            if (Utils.IsEmpty(bucket))
            {
                if (callback != null) { callback(this, stream, new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_MISS_ENTITY)); }
                return;
            }
            
            if (Utils.IsEmpty(ID)) 
            {
                if (callback != null) { callback(this, stream, new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_NO_ID)); }
                return;
            }
            
            string url = Utils.Path(UrlWithoutID, ID, "body");

            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);

            // send request
            if (progressPercentageCallback != null)
            {
                client.SendRequestForDownload(stream, (float progress) =>{
                    progressPercentageCallback(this, progress);
                },
                (ApiResponse response, Exception e) => {
                    if (e != null)
                    {
                        if (callback != null) { callback(this, stream, e); }
                        return;
                    }
                    
                    // update field
                    // TODO : Review eTag value due to KiiCorp/AndroidStorageSDK#1469
                    //mEtag = response.ETag;
                    mBodyContentType = response.ContentType;
                    // parse response
                    if (callback != null) { callback(this, stream, null); }
                });
            }
            else
            {
                client.SendRequestForDownload(stream, (long doneByte, long totalByte) => {
                    if (progressCallback != null) { progressCallback(this, doneByte, totalByte); }
                },
                (ApiResponse response, Exception e) => {
                    if (e != null)
                    {
                        if (callback != null) { callback(this, stream, e); }
                        return;
                    }
                    
                    // update field
                    // TODO : Review eTag value due to KiiCorp/AndroidStorageSDK#1469
                    //mEtag = response.ETag;
                    mBodyContentType = response.ContentType;
                    // parse response
                    if (callback != null) { callback(this, stream, null); }
                });
            }
        }

        private void ExecRefresh(KiiHttpClientFactory factory, KiiObjectCallback callback)
        {
            Utils.CheckInitialize(false);
            
            string bucket = Bucket;
            if (Utils.IsEmpty(bucket))
            {
                if (callback != null) { callback(this, new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_MISS_ENTITY)); }
                return;
            }
            
            if (Utils.IsEmpty(ID)) 
            {
                if (callback != null) { callback(this, new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_NO_ID)); }
                return;
            }
            
            string getUrl = Utils.Path(UrlWithoutID, ID);

            KiiHttpClient client = factory.Create(getUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);

            // send request
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(this, e); }
                    return;
                }
                mEtag = response.ETag;
                // parse response
                try
                {
                    // TODO: CMO-198 clarify the necessity of parse.
                    JsonObject respObj = new JsonObject(response.Body);
                    mJSON = respObj;
                    mCreatedTime = respObj.GetLong("_created");
                    mModifiedTime = respObj.OptLong("_modified", -1);
                    if (mModifiedTime == -1)
                    {
                        mModifiedTime = mCreatedTime;
                    }
                }
                catch (JsonException e2)
                {
                    if (callback != null) { callback(this, new IllegalKiiBaseObjectFormatException(e2.Message)); }
                    return;
                }
                if (callback != null) { callback(this, null); }
            });
        }

        private void ExecDelete(KiiHttpClientFactory factory, KiiObjectCallback callback)
        {
            Utils.CheckInitialize(false);
            
            string bucket = Bucket;
            if (Utils.IsEmpty(bucket))
            {
                if (callback != null) { callback(this, new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_MISS_ENTITY)); }
                return;
            }
            
            if (Utils.IsEmpty(ID)) 
            {
                if (callback != null) { callback(this, new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_NO_ID)); }
                return;
            }
            
            string deleteUrl = Utils.Path(UrlWithoutID, ID);

            KiiHttpClient client = factory.Create(deleteUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.DELETE);
            KiiCloudEngine.SetAuthBearer(client);

            // send request
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(this, e); }
                    return;
                }
                ClearAll();
                if (callback != null) { callback(this, null); }
            });
        }

        private void ExecDeleteBody(KiiHttpClientFactory factory, KiiObjectCallback callback)
        {
            Utils.CheckInitialize(false);
            string bucket = Bucket;
            if (Utils.IsEmpty(bucket))
            {
                if (callback != null) { callback(this, new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_MISS_ENTITY)); }
                return;
            }
            
            if (Utils.IsEmpty(ID)) 
            {
                if (callback != null) { callback(this, new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_NO_ID)); }
                return;
            }
            
            string deleteBodyUrl = Utils.Path(UrlWithoutID, ID, "body");
            KiiHttpClient client = factory.Create(deleteBodyUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.DELETE);
            KiiCloudEngine.SetAuthBearer(client);

            // send request
            client.SendRequest((ApiResponse response, Exception e) => {
                if(e == null)
                {
                    this.mBodyContentType = null;
                }
                if (callback != null) { callback(this, e); }
            });
        }

        private void ExecPublishBody(KiiHttpClientFactory factory, KiiObjectPublishCallback callback)
        {
            Utils.CheckInitialize(false);
            if (!IsUploaded())
            {
                if (callback != null) { callback(this, null, new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_NO_ID)); }
                return;
            }

            ExecPublish(null, factory, callback);
        }

        private void ExecPublishBodyExpiresAt(DateTime expiresAt, KiiHttpClientFactory factory, KiiObjectPublishCallback callback)
        {
            Utils.CheckInitialize(false);
            if (!IsUploaded())
            {
                if (callback != null) { callback(this, null, new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_NO_ID)); }
                return;
            }

            DateTime epock = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long time = (long)expiresAt.ToUniversalTime().Subtract(epock).TotalMilliseconds;

            JsonObject args = new JsonObject();
            try
            {
                args.Put("expiresAt", time);
            }
            catch (JsonException)
            {
                // not happens
            }

            ExecPublish(args, factory, callback);
        }

        private void ExecPublishBodyExpiresIn(long expiresIn, KiiHttpClientFactory factory, KiiObjectPublishCallback callback)
        {
            Utils.CheckInitialize(false);
            if (!IsUploaded())
            {
                if (callback != null) { callback(this, null, new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_NO_ID)); }
                return;
            }
            if (expiresIn <= 0)
            {
                if (callback != null) { callback(this, null, new ArgumentException("expiresIn must be >0")); }
                return;
            }
            
            JsonObject args = new JsonObject();
            try
            {
                args.Put("expiresIn", expiresIn);
            }
            catch (JsonException)
            {
                // not happens
            }
            
            ExecPublish(args, factory, callback);
        }

        private void ExecPublish(JsonObject args, KiiHttpClientFactory factory, KiiObjectPublishCallback callback)
        {
            string postUrl = Utils.Path(UrlWithoutID, ID, "body", "publish");

            KiiHttpClient client = factory.Create(postUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer(client);

            KiiHttpClientCallback httpCallback = (ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null) { callback(this, null, e); }
                    return;
                }
                // parse response
                string url = null;
                try 
                {
                    JsonObject respObj = new JsonObject(response.Body);
                    url = respObj.GetString("url");
                    if (callback != null) { callback(this, url, null); }
                }
                catch (JsonException e2) 
                {
                    if (callback != null) { callback(this, null, new IllegalKiiBaseObjectFormatException(e2.Message)); }
                }
            };

            if (args == null)
            {
                client.SendRequest(httpCallback);
            }
            else
            {
                client.ContentType = "application/vnd.kii.ObjectBodyPublicationRequest+json";
                client.SendRequest(args.ToString(), httpCallback);
            }
        }

        #endregion

        private bool IsUploaded ()
        {
            return (mId != null);
        }

        private void CheckScope()
        {
            if (mScope != null) {
                if (mScope is KiiGroup) {
                    if (((KiiGroup) mScope).ID == null)
                        try {
                            ((KiiGroup) mScope).Save();
                        }catch(GroupOperationException e) {
                            Utils.ThrowException(e);
                        }
                } else if (mScope is KiiUser) {
                    string id = ((KiiUser) mScope).ID;
                    if (Utils.IsEmpty(id)) {
                        throw new SystemException("Scope of a user scope object can not be null");
                    }
                        
                } else {
                    throw new SystemException("scope is other than user/group/application");
                }
            }
        }

        private String GetUserId(KiiUser parentUser) {
            string id = parentUser.ID;
            if (id == null) {
                throw new InvalidOperationException(ErrorInfo.KIIBUCKET_NO_LOGIN);
            }
            return id;
        }
        
        private String GetGroupId(KiiGroup parentGroup) {
            string id = parentGroup.ID;
            if (id == null) {
                throw new InvalidOperationException(ErrorInfo.KIIBUCKET_NO_GROUP_ID);
            }
            return id;
        }



        private void ClearAll() {
            mJSON = new JsonObject();
            this.mId = null;
            this.mBucket = null;
            this.mScope = null;
            this.mCreatedTime = -1;
            this.mModifiedTime = -1;
            this.mEtag = null;
            this.mBodyContentType = null;
        }

        /// <summary>
        /// Gets the acl for this object
        /// </summary>
        /// <remarks>
        /// See <see cref="KiiObjectAcl"/>
        /// </remarks>
        /// <returns>
        /// KiiObjectAcl instance.
        /// </returns>
        /// <param name='action'>
        /// Object ACL action.
        /// </param>
        public KiiObjectAcl Acl(ObjectAction action)
        {
            return new KiiObjectAcl(this, action);
        }

        /// <summary>
        /// Sets the geo point to this object with the specified key.
        /// </summary>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid. Please see parameter explanation.
        /// </exception>
        /// <param name='key'>
        /// Key, name of the field. Must not null or empty string.
        /// </param>
        /// <param name='geoObj'>
        /// Geo object to be tied to the specified key. Must not null.
        /// </param>
        public void SetGeoPoint(string key,KiiGeoPoint geoObj)
        {
            if( Utils.IsEmpty(key)){
                throw new ArgumentException("Invalid Parameter, Key should not null or empty string");
            }

            JsonObject obj = geoObj.ToJson();
            this[key] = obj;
        }

        /// <summary>
        /// Gets the GeoPoint associated with the given key.
        /// </summary>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid. Please see parameter explanation.
        /// </exception>
        /// <exception cref="IllegalKiiBaseObjectFormatException">
        /// Throw IllegalKiiBaseObjectFormatException if can not get KiiGeoPoint object with specified key.
        /// </exception>
        /// <returns>
        /// An instance of KiiGeoPoint tied to the key.
        /// </returns>
        /// <param name='key'>
        /// The key to retrieve. Must not null or empty string.
        /// </param>
        /// <remarks></remarks>
        public KiiGeoPoint GetGeoPoint(string key){
            if(Utils.IsEmpty(key)){
                throw new ArgumentException("Invalid Parameter, Key should not null or empty string");
            }

            JsonObject obj = this.GetJsonObject(key);

            if(obj==null)
            {
                return new KiiGeoPoint();
            }

            try
            {
                return KiiGeoPoint.GeoPoint(obj);
            }
            catch (JsonException e)
            {
                throw new IllegalKiiBaseObjectFormatException(e.Message);
            }

        }

        /// <summary>
        /// Get the GeoPoint of this object tied to the specified key. If KiiGeoPoint can not be returned for specified key, fallback will be returned.
        /// </summary>
        /// <returns>
        /// An instance of KiiGeoPoint tied to the key.
        /// </returns>
        /// <param name='key'>
        /// The key to retrieve.
        /// </param>
        /// <param name='fallBack'>
        /// return if geo point can not be returned for the specified key.
        /// </param>
        /// <remarks></remarks>
        public KiiGeoPoint GetGeoPoint(string key, KiiGeoPoint fallBack){
            try
            {
                JsonObject obj = this.GetJsonObject(key);
                return KiiGeoPoint.GeoPoint(obj);
            }
            catch (Exception)
            {
                return fallBack;
            }
        }

        #region properties
        internal string ID
        {
            get
            {
                return mId;
            }
        }
        /// <summary>
        /// Gets the URI of this instance.
        /// </summary>
        /// <remarks>
        /// Format is the following:
        /// <list type="bullet">
        ///   <item><term>Application scope object: kiicloud://buckets/{name of bucket}/objects/{id of the object}</term></item>
        ///   <item><term>User scope object: kiicloud://users/{id of the user}/buckets/{name of bucket}/objects/{id of the object}</term></item>
        ///   <item><term>Group scope object: kiicloud://groups/{id of the group}/buckets/{name of bucket}/objects/{id of the object}</term></item>
        /// </list>
        /// </remarks>
        /// <value>
        /// The URI of this instance.
        /// </value>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when this object doesn't have ID
        /// </exception>
        public Uri Uri
        {
            get
            {
                if (Utils.IsEmpty(ID)) {
                    return null;
                }
                if (Utils.IsEmpty(Bucket)) {
                    throw new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_MISS_ENTITY);
                }
                // build URI according to scope
                string url = Utils.Path(ConstantValues.URI_HEADER, KiiCloudAuthorityAndSegments);
                return new Uri(url);
            }
        }
        internal string UrlWithoutID
        {
            get
            {
                return Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, KiiCloudAuthorityAndSegmentsWithoutID);
            }
        }
        internal string Url
        {
            get
            {
                if (Utils.IsEmpty(ID)) {
                    throw new InvalidOperationException(ErrorInfo.KIIBASEOBJECT_NO_ID);
                }
                return Utils.Path(UrlWithoutID, ID);
            }
        }
        internal string KiiCloudAuthorityAndSegments
        {
            get
            {
                return Utils.Path(KiiCloudAuthorityAndSegmentsWithoutID, ID);
            }
        }
        internal string KiiCloudAuthorityAndSegmentsWithoutID
        {
            get
            {
                if (mScope == null) {
                    // application scope
                    return Utils.Path(
                        "buckets", mBucket,
                        "objects");
                }
                if (mScope is KiiGroup) {
                    // group scope
                    string scopeId = GetGroupId((KiiGroup)mScope);
                    return Utils.Path(
                        "groups", scopeId,
                        "buckets", mBucket,
                        "objects");
                }
                if (mScope is KiiUser) {
                    // user scope
                    string scopeId = GetUserId((KiiUser)mScope);
                    return Utils.Path(
                        "users", scopeId, 
                        "buckets", mBucket,
                        "objects");
                }
                // won't reach
                throw new InvalidOperationException(ErrorInfo.KIIBUCKET_UNKNOWN_SCOPE);
            }
        }
        /// <summary>
        /// Gets the created time on KiiCloud.
        /// </summary>
        /// <remarks>
        /// If this instance is not saved or refreshed, return -1
        /// </remarks>
        /// <value>
        /// The created time.
        /// </value>
        public long CreatedTime
        {
            get
            {
                return mCreatedTime;
            }
        }

        /// <summary>
        /// Gets the modifed time on KiiCloud.
        /// </summary>
        /// <remarks>
        /// If this instance is not saved or refreshed, return -1
        /// </remarks>
        /// <value>
        /// The modifed time.
        /// </value>
        public long ModifedTime
        {
            get
            {
                return mModifiedTime;
            }
        }

        /// <summary>
        /// Gets the ContentType of the body. 
        /// </summary>
        /// <remarks>
        /// This property will be set after <see cref="UploadBody(String, Stream)"/> / <see cref="DownloadBody(Stream)"/> 
        /// and clear after <see cref="DeleteBody()"/>
        /// </remarks>
        /// <value>The type of the body content.</value>
        public string BodyContentType
        {
            get
            {
                return mBodyContentType;
            }
        }

        internal string Bucket
        {
            get
            {
                return mBucket;
            }
        }

        internal bool IsSaved
        {
            get
            {
                return mCreatedTime > -1;
            }
        }
        #endregion

    }
}

