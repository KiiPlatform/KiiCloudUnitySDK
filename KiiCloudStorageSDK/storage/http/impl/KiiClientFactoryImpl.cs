using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace KiiCorp.Cloud.Storage
{
    internal class KiiClientFactoryImpl : KiiHttpClientFactory
    {
        public KiiClientFactoryImpl ()
        {
            ServicePointManager.ServerCertificateValidationCallback += this.OnRemoteCertificateValidationCallback;
        }

        private bool OnRemoteCertificateValidationCallback(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            HttpWebRequest request = sender as HttpWebRequest;

            string hostName = request.RequestUri.Host;
            if( hostName.EndsWith("kii.com") )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual KiiHttpClient Create(string url, string appID, string appKey, KiiHttpMethod method)
        {
            return new KiiHttpClientImpl(url, appID, appKey, method);
        }
    }
}

