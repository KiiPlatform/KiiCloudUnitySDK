using System;
using System.IO;
using System.Net;
using System.Text;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Implementation of Kii http client.
    /// </summary>
    internal class KiiHttpClientImpl : KiiHttpClient
    {
        protected static Encoding enc = Encoding.GetEncoding("UTF-8");
        protected HttpWebRequest request;
        private KiiHttpHeaderList headers;
        private string appID;
        private string appKey;

        public KiiHttpClientImpl (string url, string appID, string appKey, KiiHttpMethod method)
        {
            request = (HttpWebRequest) WebRequest.Create(url);
            this.appID = appID;
            this.appKey = appKey;
            headers = new KiiHttpHeaderListImpl(request);
            this.headers["X-Kii-AppID"] = this.appID;
            this.headers["X-Kii-AppKey"] = this.appKey;
            switch (method)
            {
                case KiiHttpMethod.GET:    request.Method = "GET";    break;
                case KiiHttpMethod.POST:   request.Method = "POST";   break;
                case KiiHttpMethod.PUT:    request.Method = "PUT";    break;
                case KiiHttpMethod.DELETE: request.Method = "DELETE"; break;
                case KiiHttpMethod.HEAD:   request.Method = "HEAD"; break;
            }

            if (Kii.Logger != null)
            {
                Kii.Logger.Debug("Request {0} {1}", request.Method, url);
            }

        }

        #region properties
        public string Body {
            set
            {
                if (Kii.Logger != null)
                {
                    Kii.Logger.Debug("request body : {0}", value);
                }
                byte[] data = enc.GetBytes(value);
                request.ContentLength = data.Length;

                System.IO.Stream reqStream = null;
                try
                {
                    reqStream = request.GetRequestStream();
                    reqStream.Write(data, 0, data.Length);
                }
                catch (SystemException e)
                {
                    throw new NetworkException(e);
                }
                finally
                {
                    if (reqStream != null) { reqStream.Close(); }
                }
            }
        }

        public string ContentType {
            set
            {
                request.ContentType = value;
                if (Kii.Logger != null)
                {
                    Kii.Logger.Debug("request Content-Type: {0}", value);
                }
            }
        }

        public string Accept {
            set
            {
                request.Accept = value;
                if (Kii.Logger != null)
                {
                    Kii.Logger.Debug("request Accept: {0}", value);
                }
            }
        }

        public KiiHttpHeaderList Headers {
            get
            {
                return headers;
            }
        }

        #endregion

        internal void SetSDKClientInfo() {
            request.Headers.Add("X-Kii-SDK", SDKClientInfo.GetSDKClientInfo());
        }

        public ApiResponse SendRequest()
        {
            SetSDKClientInfo();
            try {
                HttpWebResponse httpResponse = (HttpWebResponse) request.GetResponse();

                ApiResponse res = new ApiResponse();
                // status
                res.Status = (int)httpResponse.StatusCode;
                // Etag
                res.ETag = httpResponse.Headers["ETag"];
                // ContentType
                res.ContentType = httpResponse.ContentType;
                // read body
                res.Body = ReadBodyFromResponse(httpResponse);
                CopyHttpHeaders(httpResponse, res);
                return res;
            } catch (System.Net.WebException e) {
                Console.Write("Exception " + e.Message);
                System.Net.HttpWebResponse err = (System.Net.HttpWebResponse) e.Response;
                // read body
                string body = ReadBodyFromResponse(err);

                throw KiiHttpUtils.TypedException((int)err.StatusCode, body);
            }
        }

        public virtual void SendRequest(KiiHttpClientCallback callback)
        {
            SetSDKClientInfo();
            try 
            {
                HttpWebResponse httpResponse = (HttpWebResponse) request.GetResponse();
                
                ApiResponse res = ParseResponseHeader(httpResponse);
                // read body
                res.Body = ReadBodyFromResponse(httpResponse);

                callback(res, null);
            }
            catch (System.Net.WebException e) 
            {
                Console.Write("Exception " + e.Message);
                System.Net.HttpWebResponse err = (System.Net.HttpWebResponse) e.Response;
                // read body
                string body = ReadBodyFromResponse(err);
                
                callback(null, KiiHttpUtils.TypedException((int)err.StatusCode, body));
            }
            catch (Exception e) 
            {
                callback(null, e);
            }
        }

        public virtual void SendRequest(string body, KiiHttpClientCallback callback)
        {
            if (Kii.Logger != null)
            {
                Kii.Logger.Debug("request body : {0}", body);
            }
            byte[] data = enc.GetBytes(body);
            request.ContentLength = data.Length;
            
            System.IO.Stream reqStream = null;
            try
            {
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
            }
            catch (SystemException e)
            {
                callback(null, new NetworkException(e));
                return;
            }
            finally
            {
                if (reqStream != null) { reqStream.Close(); }
            }

            SendRequest(callback);
        }

        public virtual void SendRequest(Stream body, KiiHttpClientProgressCallback progressCallback, KiiHttpClientCallback callback)
        {
            SendRequest (body, new ProgressCallbackHelper (progressCallback), callback);
        }
        public virtual void SendRequest(Stream body, KiiHttpClientProgressPercentageCallback progressCallback, KiiHttpClientCallback callback)
        {
            SendRequest (body, new ProgressCallbackHelper (progressCallback), callback);
        }
        private void SendRequest(Stream body, ProgressCallbackHelper progressCallback, KiiHttpClientCallback callback)
        {
            System.IO.Stream reqStream = null;
            try
            {
                long doneByte = 0;
                long totalByte = body.Length;
                request.ContentLength = totalByte;
                
                reqStream = request.GetRequestStream();
                
                byte[] buffer = new byte[4096];
                int count;
                while(true)
                {
                    count = body.Read(buffer, 0, buffer.Length);
                    if (count <= 0)
                    {
                        break;
                    }
                    reqStream.Write(buffer, 0, count);
                    doneByte += count;
                    if (doneByte > totalByte)
                    {
                        doneByte = totalByte;
                    }
                    progressCallback.NotifyProgress(doneByte, totalByte);
                }
            }
            catch (SystemException e)
            {
                callback(null, new NetworkException(e));
                return;
            }
            finally
            {
                if (reqStream != null) { reqStream.Close(); }
            }
            
            SendRequest(callback);
        }
        public virtual void SendRequestForDownload(Stream outStream, KiiHttpClientProgressCallback progressCallback, KiiHttpClientCallback callback)
        {
            SendRequestForDownload (outStream, new ProgressCallbackHelper (progressCallback), callback);
        }
        public virtual void SendRequestForDownload(Stream outStream, KiiHttpClientProgressPercentageCallback progressCallback, KiiHttpClientCallback callback)
        {
            SendRequestForDownload (outStream, new ProgressCallbackHelper (progressCallback), callback);
        }
        private void SendRequestForDownload(Stream outStream, ProgressCallbackHelper progressCallback, KiiHttpClientCallback callback)
        {
            SetSDKClientInfo();
            try 
            {
                HttpWebResponse httpResponse = (HttpWebResponse) request.GetResponse();
                
                ApiResponse res = ParseResponseHeader(httpResponse);
                // read body
                ReadBodyStream(httpResponse, progressCallback, outStream);
                res.Body = "";
                
                callback(res, null);
            }
            catch (System.Net.WebException e) 
            {
                Console.Write("Exception " + e.Message);
                System.Net.HttpWebResponse err = (System.Net.HttpWebResponse) e.Response;
                // read body
                string body = ReadBodyFromResponse(err);
                
                callback(null, KiiHttpUtils.TypedException((int)err.StatusCode, body));
            }
            catch (Exception e) 
            {
                callback(null, e);
            }
        }
        protected void CopyHttpHeaders(HttpWebResponse source, ApiResponse dest)
        {
            WebHeaderCollection headers = source.Headers;
            foreach (string key in headers.AllKeys)
            {
                dest.Headers.Add(key, headers[key]);
            }
        }
        private ApiResponse ParseResponseHeader(HttpWebResponse httpResponse)
        {
            ApiResponse res = new ApiResponse();
            // status
            res.Status = (int)httpResponse.StatusCode;
            // Etag
            res.ETag = httpResponse.Headers["ETag"];
            // ContentType
            res.ContentType = httpResponse.ContentType;
            // headers
            WebHeaderCollection httpHeaders = httpResponse.Headers;
            foreach (string key in httpHeaders.AllKeys)
            {
                res.Headers[key] = httpHeaders[key];
            }
            return res;
        }

        protected string ReadBodyFromResponse (System.Net.HttpWebResponse response)
        {
            System.IO.Stream s = null;
            System.IO.StreamReader sr = null;
            try
            {
                s = response.GetResponseStream();
                sr = new System.IO.StreamReader(s, enc);
                string body = sr.ReadToEnd();

                if (Kii.Logger != null)
                {
                    Kii.Logger.Debug("Response HTTP {0}\nResponse body : {1}", response.StatusCode, body);
                }

                return body;
            }
            catch (SystemException e)
            {
                throw new NetworkException(e);
            }
            finally
            {
                if (sr != null) { sr.Close(); }
                if (s != null) { s.Close(); }
            }
        }

        protected void ReadBodyStream (System.Net.HttpWebResponse response, ProgressCallbackHelper progressCallback, Stream outStream)
        {
            System.IO.Stream s = null;
            try
            {
                s = response.GetResponseStream();
                byte[] buffer = new byte[4096];
                int count;
                long doneByte = 0;
                long totalByte = response.ContentLength;
                while(true)
                {
                    count = s.Read(buffer, 0, buffer.Length);
                    if (count <= 0)
                    {
                        break;
                    }
                    outStream.Write(buffer, 0, count);
                    doneByte += count;
                    if (doneByte > totalByte)
                    {
                        doneByte = totalByte;
                    }
                    if (progressCallback != null) { progressCallback.NotifyProgress(doneByte, totalByte); }
                }
            }
            catch (SystemException e)
            {
                throw new NetworkException(e);
            }
            finally
            {
                if (s != null) { s.Close(); }
            }
        }
    }
}

