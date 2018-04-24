using System;
using System.IO;
using System.Net;

namespace KiiCorp.Cloud.Storage
{
    internal class KiiAsyncHttpClientImpl : KiiHttpClientImpl
    {
        public KiiAsyncHttpClientImpl (string url, string appID, string appKey, KiiHttpMethod method) : base(url, appID, appKey, method)
        {
        }

        public override void SendRequest (KiiHttpClientCallback callback)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((object stateInfo)=> {
                base.SendRequest(callback);
            });
        }

        public override void SendRequest(string body, KiiHttpClientCallback callback)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((object stateInfo)=> {
                base.SendRequest(body, callback);
            });
        }

        public override void SendRequest(Stream body, KiiHttpClientProgressCallback progressCallback, KiiHttpClientCallback callback)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((object stateInfo)=> {
                base.SendRequest(body, progressCallback, callback);
            });
        }

        public override void SendRequest(Stream body, KiiHttpClientProgressPercentageCallback progressCallback, KiiHttpClientCallback callback)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((object stateInfo)=> {
                base.SendRequest(body, progressCallback, callback);
            });
        }

        public override void SendRequestForDownload (Stream outStream, KiiHttpClientProgressCallback progressCallback, KiiHttpClientCallback callback)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((object stateInfo)=> {
                base.SendRequestForDownload(outStream, progressCallback, callback);
            });
        }

        public override void SendRequestForDownload (Stream outStream, KiiHttpClientProgressPercentageCallback progressCallback, KiiHttpClientCallback callback)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((object stateInfo)=> {
                base.SendRequestForDownload(outStream, progressCallback, callback);
            });
        }
    }
}

