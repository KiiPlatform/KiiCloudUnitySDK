using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The exception that is thrown when the Request is not acceptable due to conflict.
    /// Reason would be represented by enum.
    /// </summary>
    public class ConflictException : CloudException
    {
        internal ConflictException (String message, SystemException cause, String httpBody, Reason reason) : base (409, message, cause, httpBody)
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
            /// Requested ACL already exists.
            /// </summary>
            ACL_ALREADY_EXISTS,
            /// <summary>
            /// Requested Bucket already exists.
            /// </summary>
            BUCKET_ALREADY_EXISTS,
            /// <summary>
            /// Object update rejected due to version update in cloud.
            /// </summary>
            OBJECT_VERSION_IS_STALE,
            /// <summary>
            /// Requested Object already exists.
            /// </summary>
            OBJECT_ALREADY_EXISTS,
            /// <summary>
            /// Requested user already exists.
            /// </summary>
            USER_ALREADY_EXISTS,
            /// <summary>
            /// Requested action has been invoked at an illegal or inappropriate time.
            /// </summary>
            INVALID_STATUS,
            /// <summary>
            /// The user to link is already linked with a facebook account.
            /// </summary>
            FACEBOOK_USER_ALREADY_LINKED,
            /// <summary>
            /// The user to link is already linked with a QQ account.
            /// </summary>
            QQ_USER_ALREADY_LINKED,
            /// <summary>
            /// The user to link is already linked with a google account.
            /// </summary>
            GOOGLE_USER_ALREADY_LINKED,
            /// <summary>
            /// The user to link is already linked.
            /// </summary>
            USER_ALREADY_LINKED,
            /// <summary>
            /// The user to unlink is not linked
            /// </summary>
            USER_NOT_LINKED,
            /// <summary>
            /// Requested group already exists.
            /// </summary>
            GROUP_ALREADY_EXISTS,
            /// <summary>
            /// Unknown error happens.
            /// </summary>
            __UNKNOWN__
        }

    }
}

