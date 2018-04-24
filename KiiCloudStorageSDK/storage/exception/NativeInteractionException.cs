using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The exception that is thrown when error occurred in native layer.
    /// </summary>
    /// <remarks></remarks>
    public class NativeInteractionException : SocialException
    {
        /// <summary>
        /// Initializes NativeInteractionException.
        /// </summary>
        /// <remarks>
        /// This constructor is called by
        /// KiiSocialNetworkConnector. Your applications does not need to use
        /// this constructor.
        /// </remarks>
        /// <param name='message'>
        /// Error message
        /// </param>
        public NativeInteractionException(String message) : base(message)
        {
        }

        /// <summary>
        /// Initializes NativeInteractionException.
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
        public NativeInteractionException(String message, Exception cause) : base(message, cause)
        {
        }
    }
}

