using System;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Contains the result from Cloud query operation.
    /// </summary>
    /// <remarks>
    /// If there are many items in the result, KiiCloud will return a part of them.
    /// So please call KiiQueryResult.GetNextQueryResult() to get the rest of results.
    /// </remarks>
    /// <typeparam name="T">
    /// Must be <see cref="KiiObject"/>
    /// </typeparam>
    public class KiiQueryResult<T> : List<T>
    {
        private KiiQuery mNextQuery;

        private KiiBaseBucket<T> mBucket;
// we will use this later
//        private bool mIsTrashed;

        internal KiiQueryResult(KiiQuery query, string paginationKey, KiiBaseBucket<T> bucket, bool trashed)
        {
            if (Utils.IsEmpty(paginationKey))
            {
                mNextQuery = null;
            }
            else
            {
                mNextQuery = KiiQuery.copy(query);
                mNextQuery.NextPaginationKey = paginationKey;
            }
            mBucket = bucket;
//            mIsTrashed = trashed;
        }

        #region Blocking APIs

        /// <summary>
        /// Fetches the query result of next page
        /// </summary>
        /// <remarks>
        /// Same as calling KiiBucket.Query(KiiQuery) with NextKiiQuery
        /// </remarks>
        /// <returns>
        /// The next query result.
        /// </returns>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when an operation cannot be performed.
        /// </exception>
        /// <exception cref='SystemException'>
        /// <see cref="T:System.SystemException" /> is the base class for all exceptions defined by the system.
        /// </exception>
        public KiiQueryResult<T> GetNextQueryResult()
        {
            if (!HasNext) {
                throw new InvalidOperationException("End of the page. no more results.");
            }
            KiiQuery query = NextKiiQuery;
            if (mBucket is KiiBucket) {
                return mBucket.Query(query);
//            } else if (mBucket is KiiFileBucket) {
//                KiiFileBucket bucket = (KiiFileBucket) mBucket;
//                return (KiiQueryResult<T>) bucket.query(query, mIsTrashed);
            } else {
                throw new SystemException ("Unexpected error! " + mBucket.GetType().ToString());
            }
        }

        #endregion

        #region Async APIs
        /// <summary>
        /// Fetches the query result of next page
        /// </summary>
        /// <remarks>
        /// Same as calling KiiBucket.Query(KiiQuery) with NextKiiQuery
        /// </remarks>
        /// <returns>
        /// The next query result.
        /// </returns>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void GetNextQueryResult(KiiQueryCallback<T> callback)
        {
            if (!HasNext) {
                if (callback != null) { callback(null, new InvalidOperationException("End of the page. no more results.")); }
                return;
            }
            KiiQuery query = NextKiiQuery;
            if (mBucket is KiiBucket) {
                mBucket.Query(query, callback);
                //            } else if (mBucket is KiiFileBucket) {
                //                KiiFileBucket bucket = (KiiFileBucket) mBucket;
                //                return (KiiQueryResult<T>) bucket.query(query, mIsTrashed);
            } else {
                if (callback != null) { callback(null, new SystemException ("Unexpected error! " + mBucket.GetType().ToString())); }
                return;
            }
        }
        #endregion

        #region properties

        /// <summary>
        /// Gets a value whether there are more results.
        /// </summary>
        /// <remarks>
        /// If developers cannot estimate the number of results, they can use this property.
        /// </remarks>
        /// <value>
        /// <c>true</c> if there are many results of query; otherwise, <c>false</c>.
        /// </value>
        public bool HasNext
        {
            get
            {
                return mNextQuery != null;
            }
        }

        /// <summary>
        /// Gets the next kii query.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>
        /// <see cref="KiiQuery"/> if there are many results ; otherwise null.
        /// </value>
        public KiiQuery NextKiiQuery
        {
            get
            {
                return mNextQuery;
            }
        }

        #endregion
    }
}

