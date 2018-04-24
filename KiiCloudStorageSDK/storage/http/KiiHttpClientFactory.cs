using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Kii http client factory.
    /// </summary>
    /// <remarks></remarks>
    public interface KiiHttpClientFactory
    {
        /// <summary>
        /// Create the new Client with specified url.
        /// </summary>
        /// <param name='url'>
        /// URL.
        /// </param>
        /// <param name='appID'>
        /// AppID.
        /// </param>
        /// <param name='appKey'>
        /// AppKey.
        /// </param>
        /// <param name='method'>
        /// HTTP method.
        /// </param>
        /// <returns>Instance of the KiiHttpClient</returns>
        /// <remarks></remarks>
        KiiHttpClient Create(string url, string appID, string appKey, KiiHttpMethod method);
    }
}

