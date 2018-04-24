using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The exception that is thrown when the Request is not acceptable due to the request is not expected.
    /// Reason would be represented by enum.
    /// </summary>
    public class BadRequestException : CloudException
    {
        internal BadRequestException (String message, SystemException cause, String httpBody, Reason reason) : base (400, message, cause, httpBody)
        {
            this.reason = reason;
        }

        #region properties
        /// <summary>
        /// Represents reason of error.
        /// </summary>
        public Reason reason;
        #endregion

        /// <summary>
        /// Represents reason of error.
        /// </summary>
        public enum Reason {
            /// <summary>
            /// Given JSON is not acceptable.
            /// </summary>
            INVALID_JSON,
            /// <summary>
            /// Given Bucket name is invalid.
            /// </summary>
            INVALID_BUCKET,
            /// <summary>
            /// Given query is not supported
            /// </summary>
            QUERY_NOT_SUPPORTED,
            /// <summary>
            /// Given Input data is invalid.
            /// </summary>
            INVALID_INPUT_DATA,
            /// <summary>
            /// Account status is invalid.
            /// </summary>
            INVALID_ACCOUNT_STATUS,
            /// <summary>
            /// Given password is too short.
            /// </summary>
            PASSWORD_TOO_SHORT,
            /// <summary>
            /// Unknown error happens.
            /// </summary>
            __UNKNOWN__
        }
    }
}

