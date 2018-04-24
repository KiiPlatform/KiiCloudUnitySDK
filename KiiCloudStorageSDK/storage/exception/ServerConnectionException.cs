using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The exception that is thrown when server connection is failed. The
    /// process will be succeeded a little later. Please retry later.
    /// </summary>
    /// <remarks></remarks>
    public class ServerConnectionException : SocialException
    {
        /// <summary>
        /// Initializes ServerConnectionException.
        /// </summary>
        /// <remarks>
        /// This constructor is called by
        /// KiiSocialNetworkConnector. Your applications does not need to use
        /// this constructor.
        /// </remarks>
        /// <param name='message'>
        /// Error message
        /// </param>
        public ServerConnectionException(String message) : base(message)
        {
        }

        /// <summary>
        /// Initializes ServerConnectionException.
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
        public ServerConnectionException(String message, Exception cause) : base(message, cause)
        {
        }
    }
}

