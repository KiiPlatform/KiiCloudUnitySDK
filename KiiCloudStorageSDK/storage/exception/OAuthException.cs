using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The exception that is thrown when oauth is failed.
    /// </summary>
    /// <remarks></remarks>
    public class OAuthException : SocialException
    {
        /// <summary>
        /// Initializes OAuthException.
        /// </summary>
        /// <remarks>
        /// This constructor is called by
        /// KiiSocialNetworkConnector. Your applications does not need to use
        /// this constructor.
        /// </remarks>
        /// <param name='message'>
        /// Error message
        /// </param>
        public OAuthException(String message) : base(message)
        {
        }

        /// <summary>
        /// Initializes OAuthException.
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
        public OAuthException(String message, Exception cause) : base(message, cause)
        {
        }
    }
}

