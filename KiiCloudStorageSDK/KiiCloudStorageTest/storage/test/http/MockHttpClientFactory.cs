using System;

namespace KiiCorp.Cloud.Storage
{
    internal class MockHttpClientFactory : KiiHttpClientFactory
    {
        private MockHttpClient client = null;

        internal MockHttpClientFactory ()
        {
            client = new MockHttpClient();
            client.SetAppIDAndKey(Kii.AppId, Kii.AppKey);
        }

        public KiiHttpClient Create(string url, string appID, string appKey, KiiHttpMethod method)
        {
            client.RequestUrl.Add(url);
            client.RequestMethod.Add (method);
            client.SetAppIDAndKey(appID, appKey);
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

