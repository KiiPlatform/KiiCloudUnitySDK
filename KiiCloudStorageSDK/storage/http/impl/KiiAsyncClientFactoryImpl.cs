// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
namespace KiiCorp.Cloud.Storage
{
    internal class KiiAsyncClientFactoryImpl : KiiClientFactoryImpl
    {
        public KiiAsyncClientFactoryImpl () : base()
        {

        }

        /// <summary>
        /// Create the new Client with specified url.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="appID">AppID.</param>
        /// <param name="appKey">AppKey.</param>
        /// <param name="method">HTTP method.</param>
        public override KiiHttpClient Create(string url, string appID, string appKey, KiiHttpMethod method)
        {
            return new KiiAsyncHttpClientImpl(url, appID, appKey, method);
        }
    }
}

