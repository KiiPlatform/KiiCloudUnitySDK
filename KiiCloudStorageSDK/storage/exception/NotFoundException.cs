using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The exception that is thrown when the requested entity is not found.
    /// Reason would be represented by enum.
    /// </summary>
    public class NotFoundException : CloudException
    {
        internal NotFoundException (String message, SystemException cause, String httpBody, Reason reason) : base (404, message, cause, httpBody)
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
            /// Requested ACL not found.
            /// </summary>
            ACL_NOT_FOUND,
            /// <summary>
            /// Requested Bucket not found.
            /// </summary>
            BUCKET_NOT_FOUND,
            /// <summary>
            /// Requested Object not found.
            /// </summary>
            OBJECT_NOT_FOUND,
            /// <summary>
            /// Requested User not found.
            /// </summary>
            USER_NOT_FOUND,
            /// <summary>
            /// Requested Group not found.
            /// </summary>
            GROUP_NOT_FOUND,
            /// <summary>
            /// Requested Object body not found.
            /// </summary>
            OBJECT_BODY_NOT_FOUND,
            /// <summary>
            /// Requested App not found.
            /// Please check app-id, app-key, site params in the initialization.
            /// </summary>
            APP_NOT_FOUND,
            /// <summary>
            /// Requested User address not found.
            /// </summary>
            USER_ADDRESS_NOT_FOUND,
            /// <summary>
            /// Requested Topic not found.
            /// </summary>
            TOPIC_NOT_FOUND,
            /// <summary>
            /// Requested Filter not found.
            /// </summary>
            FILTER_NOT_FOUND,
            /// <summary>
            /// Requested Push Subscription not found.
            /// </summary>
            PUSH_SUBSCRIPTION_NOT_FOUND,
            /// <summary>
            /// Unknown error happens.
            /// </summary>
            __UNKNOWN__
        }
    }
}

