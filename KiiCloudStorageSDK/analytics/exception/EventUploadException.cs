using System;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Analytics
{
    /// <summary>
    /// This exception will be thrown when event uploading is failed.
    /// </summary>
    /// <remarks>
    /// Developer can know which events are not uploaded.
    /// </remarks>
    public class EventUploadException : SystemException
    {
        private int mStatus;
        private string mBody;
        private IList<KiiEvent> mErrorEventList;
            
        internal EventUploadException (int status, string body, IList<KiiEvent> errorEventList)
        {
            this.mStatus = status;
            this.mBody = body;
            this.mErrorEventList = errorEventList;
        }
        
        #region properties
        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <remarks></remarks>
        /// <value>
        /// The status code.
        /// </value>
        public int Status
        {
            get
            {
                return mStatus;
            }
        }
        
        /// <summary>
        /// Gets the body string.
        /// </summary>
        /// <remarks></remarks>
        /// <value>
        /// The body string.
        /// </value>
        public string Body
        {
            get
            {
                return mBody;
            }
        }
        
        /// <summary>
        /// Gets the list of events which has some errors.
        /// </summary>
        /// <remarks>This property must not be null.</remarks>
        /// <value>
        /// The list of events.
        /// </value>
        public IList<KiiEvent> ErrorEvents
        {
            get
            {
                return mErrorEventList;
            }
        }
        
        #endregion
    }
}

