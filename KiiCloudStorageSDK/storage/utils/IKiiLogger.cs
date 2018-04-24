using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides logging APIs
    /// </summary>
    /// <remarks>
    /// Please call String.Format to make an actual message.
    /// </remarks>
    public interface IKiiLogger
    {
        /// <summary>
        /// This method will be called when SDK will output debug log
        /// </summary>
        /// <remarks>
        /// Please call String.Format to make an actual message.
        /// </remarks>
        /// <param name='message'>
        /// Message.
        /// </param>
        /// <param name='args'>
        /// Arguments. Please use in String.Format
        /// </param>
        void Debug(string message, params object[] args);
    }
}

