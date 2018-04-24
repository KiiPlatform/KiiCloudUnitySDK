using System;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents API response.
    /// </summary>
    /// <remarks></remarks>
    public class ApiResponse
    {
        private int status;
        private string body;
        private string eTag;
        private string contentType;
        private IDictionary<string, string> headers = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

        internal string GetHeader (string key)
        {
            return headers.ContainsKey(key) ? headers[key] : null;
        }

        #region
        /// <summary>
        /// Gets or sets the http status.
        /// </summary>
        /// <value>The http status.</value>
        /// <remarks></remarks>
        public int Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }
        /// <summary>
        /// Gets or sets the response body as string.
        /// </summary>
        /// <value>The response body.</value>
        /// <remarks></remarks>
        public string Body
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
            }
        }
        /// <summary>
        /// Gets or sets the http header of ETag.
        /// </summary>
        /// <value>The http header of ETag.</value>
        /// <remarks></remarks>
        public string ETag
        {
            get
            {
                return eTag;
            }
            set
            {
                eTag = value;
            }
        }
        /// <summary>
        /// Gets or sets the http header of Content-Type.
        /// </summary>
        /// <value>The http header of Content-Type.</value>
        /// <remarks></remarks>
        public string ContentType
        {
            get
            {
                return contentType;
            }
            set
            {
                contentType = value;
            }
        }
        public bool IsSuccess()
        {
            if (this.status >= 200 && this.status < 300)
            {
                return true;
            }
            return false;
        }
        public bool IsRedirection()
        {
            if (this.status >= 300 && this.status < 400)
            {
                return true;
            }
            return false;
        }
        public bool IsError()
        {
            if (this.status >= 400)
            {
                return true;
            }
            return false;
        }
        public IDictionary<string, string> Headers
        {
            get
            {
                return headers;
            }
        }
        #endregion
    }
}

