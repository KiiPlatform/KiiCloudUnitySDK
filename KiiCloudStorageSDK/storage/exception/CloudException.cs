using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The exception that is thrown when KiiCloud sends error response.
    /// </summary>
    /// <remarks>
    /// Developers can get server status code and body by properties.
    /// </remarks>
    public class CloudException : SystemException
    {
        private int statusCode = -1;

        private string httpBody;

        internal CloudException (int statusCode, string body)
        {
            this.statusCode = statusCode;
            this.httpBody = body;
        }

        internal CloudException (int statusCode, string message, SystemException cause) : base(message, cause)
        {
            this.statusCode = statusCode;
            this.httpBody = "";
        }

        internal CloudException (int statusCode, string message, SystemException cause, string httpBody) : base(message, cause)
        {
            this.statusCode = statusCode;
            this.httpBody = httpBody;
        }

        /// <summary>
        /// Dump http status and body. If the body is larger than 400 characters, It will be trimmed.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="KiiCorp.Cloud.Storage.CloudException"/>.</returns>
        public override string ToString ()
        {
            string bodyForPrinting = this.Body;
            if (bodyForPrinting != null && bodyForPrinting.Length > 400) {
                bodyForPrinting = this.Body.Substring(0,400) +"...truncated";
            }
            return string.Format ("[CloudException: Status={0} Body={1}]", this.Status, bodyForPrinting);
        }

        #region properties
        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <remarks>
        /// See the API documents.
        /// </remarks>
        /// <value>
        /// The status code.
        /// </value>
        public int Status
        {
            get
            {
                return statusCode;
            }
        }

        /// <summary>
        /// Gets the body of server response.
        /// </summary>
        /// <remarks>
        /// Format is Json. Developers can handle the details by the following code.
        /// <code>
        /// catch (CloudException e)
        /// {
        ///     JsonObject json = new JsonObject(e.Body);
        ///     // get values
        /// }
        /// </code>
        /// </remarks>
        /// <value>
        /// The body.
        /// </value>
        public string Body
        {
            get
            {
                return httpBody;
            }
        }
        #endregion
    }
}

