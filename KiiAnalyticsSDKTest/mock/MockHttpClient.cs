using System;
using System.Collections.Generic;
using System.IO;
using KiiCorp.Cloud.Storage;

namespace KiiCorp.Cloud.Analytics
{
    internal class MockHttpClient : KiiHttpClient
    {
        #region Request params
        private IList<string> urlList = new List<string>();
        private IList<string> contentTypeList = new List<string>();
        private IList<MockHttpHeaderList> requestHeaderList = new List<MockHttpHeaderList>();
        private IList<string> requestBodyList = new List<string>();
        private IList<KiiHttpMethod> requestMethodList = new List<KiiHttpMethod>();
        #endregion
        
        #region for progress
        private Queue<int> numProgressQueue = new Queue<int>();
        #endregion
        
        
        private Queue<MockResponse> responseQueue = new Queue<MockResponse>();
        
        private MockHttpHeaderList headers = new MockHttpHeaderList();
        
        internal MockHttpClient ()
        {
        }
        /// <summary>
        /// Sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body
        {
            set
            {
                requestBodyList.Add(value);
            }
        }
        
        /// <summary>
        /// Sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public string ContentType
        {
            set
            {
                headers["content-type"] = value;
                contentTypeList.Add(value);
            }
        }
        
        /// <summary>
        /// Sets the Accept value.
        /// </summary>
        /// <value>
        /// The accept.
        /// </value>
        public string Accept
        {
            set
            {
                
            }
        }
        
        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public KiiHttpHeaderList Headers
        {
            get
            {
                return headers;
            }
        }
        
        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <returns>
        /// The response.
        /// </returns>
        public ApiResponse SendRequest()
        {
            ApiResponse response = new ApiResponse();
            if (responseQueue.Count == 0)
            {
                response.Status = 200;
                response.Body = "{}";
                return response;
            }
            MockResponse mockResponse = responseQueue.Dequeue();
            Exception e = mockResponse.Ex;
            if (e != null)
            {
                throw e;
            }
            response.Status = mockResponse.Status;
            response.Body = mockResponse.Body;
            response.ETag = mockResponse.ETag;
            if (mockResponse.StepCount >= 0)
            {
                response.Headers["X-Step-count"] = "" + mockResponse.StepCount;
            }
            return response;
        }
        
        public void SendRequest(KiiHttpClientCallback callback)
        {
            // move request headers to list
            requestHeaderList.Add(headers);
            headers = new MockHttpHeaderList();
            
            ApiResponse response = new ApiResponse();
            if (responseQueue.Count == 0)
            {
                response.Status = 200;
                response.Body = "{}";
                callback(response, null);
                return;
            }
            MockResponse mockResponse = responseQueue.Dequeue();
            Exception e = mockResponse.Ex;
            if (e != null)
            {
                callback(null, e);
                return;
            }
            response.Status = mockResponse.Status;
            response.Body = mockResponse.Body;
            response.ETag = mockResponse.ETag;
            if (mockResponse.StepCount >= 0)
            {
                response.Headers["X-Step-count"] = "" + mockResponse.StepCount;
            }
            callback(response, null);
        }
        
        public void SendRequest(string body, KiiHttpClientCallback callback)
        {
            requestBodyList.Add(body);
            SendRequest(callback);
        }
        
        public void SendRequest(Stream body, KiiHttpClientProgressCallback progressCallback, KiiHttpClientCallback callback)
        {
            // simulate calling progress callback
            long totalByte = body.Length;
            int count = numProgressQueue.Dequeue();
            long blockByte = totalByte / count;
            for (int i = 1 ; i < count ; ++i)
            {
                progressCallback(blockByte * i, totalByte);
            }
            progressCallback(totalByte, totalByte);
            
            SendRequest(callback);
        }
        public void SendRequest(Stream body, KiiHttpClientProgressPercentageCallback progressCallback, KiiHttpClientCallback callback)
        {
            // simulate calling progress callback
            long totalByte = body.Length;
            int count = numProgressQueue.Dequeue();
            long blockByte = totalByte / count;
            for (int i = 1 ; i < count ; ++i)
            {
                progressCallback((float)((blockByte * i) / totalByte));
            }
            progressCallback((float)(totalByte / totalByte));
            
            SendRequest(callback);
        }
        
        public void SendRequestForDownload(Stream outStream, KiiHttpClientProgressCallback progressCallback, KiiHttpClientCallback callback)
        {
            ApiResponse response = new ApiResponse();
            if (responseQueue.Count == 0)
            {
                response.Status = 200;
                response.Body = "{}";
                callback(response, null);
                return;
            }
            MockResponse mockResponse = responseQueue.Dequeue();
            
            Exception e = mockResponse.Ex;
            if (e != null)
            {
                callback(null, e);
                return;
            }
            byte[] bytes = mockResponse.BytesBody;
            
            // simulate calling progress callback
            long totalByte = bytes.Length;
            int count = numProgressQueue.Dequeue();
            long blockByte = totalByte / count;
            for (int i = 1 ; i < count ; ++i)
            {
                progressCallback(blockByte * i, totalByte);
            }
            progressCallback(totalByte, totalByte);
            outStream.Write(bytes, 0, (int) totalByte);
            
            response.Status = mockResponse.Status;
            response.ContentType = mockResponse.BodyContentType;
            response.ETag = mockResponse.ETag;
            
            callback(response, null);
        }
        public void SendRequestForDownload(Stream outStream, KiiHttpClientProgressPercentageCallback progressCallback, KiiHttpClientCallback callback)
        {
            ApiResponse response = new ApiResponse();
            if (responseQueue.Count == 0)
            {
                response.Status = 200;
                response.Body = "{}";
                callback(response, null);
                return;
            }
            MockResponse mockResponse = responseQueue.Dequeue();
            
            Exception e = mockResponse.Ex;
            if (e != null)
            {
                callback(null, e);
                return;
            }
            byte[] bytes = mockResponse.BytesBody;
            
            // simulate calling progress callback
            long totalByte = bytes.Length;
            int count = numProgressQueue.Dequeue();
            long blockByte = totalByte / count;
            for (int i = 1 ; i < count ; ++i)
            {
                progressCallback((float)((blockByte * i) / totalByte));
            }
            progressCallback((float)(totalByte / totalByte));
            outStream.Write(bytes, 0, (int) totalByte);
            
            response.Status = mockResponse.Status;
            response.ContentType = mockResponse.BodyContentType;
            response.ETag = mockResponse.ETag;
            
            callback(response, null);
        }
        
        public void AddResponse(int status, string body)
        {
            AddResponse(status, body, null);
        }
        
        public void AddResponse(int status, string body, string etag)
        {
            responseQueue.Enqueue(new MockResponse(status, body, etag));
        }
        
        public void AddResponse(string contentType, byte[] body, string etag)
        {
            responseQueue.Enqueue(new MockResponse(contentType, body, etag));
        }
        
        public void AddResponse(int status, string body, int stepCount)
        {
            responseQueue.Enqueue(new MockResponse(status, body, "", "", stepCount));
        }
        
        public void AddResponse(Exception e)
        {
            responseQueue.Enqueue(new MockResponse(e));
        }
        
        public IList<string> RequestUrl
        {
            get
            {
                return urlList;
            }
        }
        public IList<string> RequestContentType
        {
            get
            {
                return contentTypeList;
            }
        }
        public IList<string> RequestBody
        {
            get
            {
                return requestBodyList;
            }
        }
        public IList<MockHttpHeaderList> RequestHeader
        {
            get
            {
                return requestHeaderList;
            }
        }
        
        public IList<KiiHttpMethod> RequestMethod
        {
            get
            {
                return requestMethodList;
            }
        }
        
        public void AddNumProgress(int num)
        {
            this.numProgressQueue.Enqueue(num);
        }
    }
    
    internal class MockResponse
    {
        private int status;
        private string body;
        private string etag;
        private byte[] byteBody;
        private string contentType;
        private int stepCount;
        private Exception e;
        
        internal MockResponse(int status, string body) : this(status, body, null)
        {
            
        }
        
        internal MockResponse(int status, string body, string etag) : this(status, body, etag, null, -1)
        {
            
        }
        
        internal MockResponse(int status, string body, string etag, string contentType, int stepCount)
        {
            this.status = status;
            this.body = body;
            this.etag = etag;
            this.contentType = contentType;
            this.stepCount = stepCount;
        }
        
        internal MockResponse(string contentType, byte[] body, string etag)
        {
            this.contentType = contentType;
            this.byteBody = body;
            this.etag = etag;
        }
        
        internal MockResponse(Exception e)
        {
            this.e = e;
        }
        
        internal int Status
        {
            get
            {
                return status;
            }
        }
        
        internal string Body
        {
            get
            {
                return body;
            }
        }
        
        internal byte[] BytesBody
        {
            get
            {
                return byteBody;
            }
        }
        
        internal string ETag
        {
            get
            {
                return etag;
            }
        }
        
        internal string BodyContentType
        {
            get
            {
                return contentType;
            }
        }
        
        internal int StepCount
        {
            get
            {
                return stepCount;
            }
        }
        
        internal Exception Ex
        {
            get
            {
                return e;
            }
        }
    }
}

