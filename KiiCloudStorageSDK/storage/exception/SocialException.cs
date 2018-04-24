using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The exception that is thrown when social authentication is failed.
    /// </summary>
    /// <remarks></remarks>
    public class SocialException : SystemException
    {
        /// <summary>
        /// Initializes SocialException.
        /// </summary>
        /// <remarks>
        /// This constructor is called by
        /// KiiSocialNetworkConnector. Your applications does not need to use
        /// this constructor.
        /// </remarks>
        /// <param name='message'>
        /// Error message
        /// </param>
        public SocialException(String message) : base(message)
        {
        }
        /// <summary>
        /// Initializes SocialException.
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
        public SocialException(String message, Exception cause) : base(message, cause)
        {
        }
    }
}

