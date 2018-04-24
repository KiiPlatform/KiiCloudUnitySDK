using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Base interface of KiiBucket
    /// </summary>
    /// <remarks>
    /// Developers don't need to implement this interface
    /// </remarks>
    /// <typeparam name="T">
    /// must be <see cref="KiiObject"/>
    /// </typeparam>
    public interface KiiBaseBucket<T>
    {
        /// <summary>
        /// Query KiiObjects in this bucket.
        /// </summary>
        /// <remarks>
        /// If there are many items in the result, KiiCloud will return a part of them.
        /// So please call KiiQueryResult.GetNextQueryResult() to get the rest of results.
        /// </remarks>
        /// <returns>
        /// The result of query
        /// </returns>
        /// <param name='query'>
        /// Query. If null is given, API returns all items in this bucket.
        /// </param>
        KiiQueryResult<T> Query(KiiQuery query);

        /// <summary>
        /// Query KiiObjects in this bucket.
        /// </summary>
        /// <remarks>
        /// If there are many items in the result, KiiCloud will return a part of them.
        /// So please call KiiQueryResult.GetNextQueryResult() to get the rest of results.
        /// </remarks>
        /// <returns>
        /// The result of query
        /// </returns>
        /// <param name='query'>
        /// Query. If null is given, API returns all items in this bucket.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        void Query(KiiQuery query, KiiQueryCallback<T> callback);
    }
}

