using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The exception that is thrown when user cancels the authorization.
    /// </summary>
    /// <remarks></remarks>
    public class UserCancelException : SocialException
    {
        /// <summary>
        /// Initializes UserCancelException.
        /// </summary>
        /// <remarks>
        /// This constructor is called by
        /// KiiSocialNetworkConnector. Your applications does not need to use
        /// this constructor.
        /// </remarks>
        /// <param name='message'>
        /// Error message
        /// </param>
        public UserCancelException(String message) : base(message)
        {
        }

        /// <summary>
        /// Initializes UserCancelException.
        /// </summary>
        /// <remarks>
        /// This constructor is called by
        /// KiiSocialNetworkConnector. Your applications does not need to use
        /// this constructor.
        /// </remarks>
        /// <param name='message'>
        /// Error message
        /// </param>
        /// <param name='cause'>
        /// Cause of this exception
        /// </param>
        public UserCancelException(String message, Exception cause) : base(message, cause)
        {
        }
    }
}

