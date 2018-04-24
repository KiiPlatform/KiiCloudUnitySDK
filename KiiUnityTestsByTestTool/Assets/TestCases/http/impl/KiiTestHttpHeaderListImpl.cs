using System;
using System.Net;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Implementation of Kii http header list.
    /// </summary>
    internal class KiiTestHttpHeaderListImpl : KiiTestHttpHeaderList
    {
        private HttpWebRequest request;

        internal KiiTestHttpHeaderListImpl (HttpWebRequest request)
        {
            this.request = request;
        }

        #region properties
        public string this[string key]
        {
            set
            {
                request.Headers.Add(key, value);
                if (Kii.Logger != null)
                {
                    Kii.Logger.Debug("Request Header[{0}] = {1}", key, value);
                }
            }
        }
        #endregion
    }
}

