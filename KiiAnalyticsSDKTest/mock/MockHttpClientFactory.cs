using System;
using KiiCorp.Cloud.Storage;

namespace KiiCorp.Cloud.Analytics
{
    internal class MockHttpClientFactory : KiiHttpClientFactory
    {
        private MockHttpClient client = new MockHttpClient();

        internal MockHttpClientFactory ()
        {
        }

        public KiiHttpClient Create(string url, string appID, string appKey, KiiHttpMethod method)
        {
            client.RequestUrl.Add(url);
            return client;
        }

        public MockHttpClient Client
        {
            get
            {
                return client;
            }
        }
    }
}

