using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Progress callback helper.
    /// </summary>
    /// <remarks></remarks>
    internal class ProgressCallbackHelper
    {
        private KiiHttpClientProgressCallback progressCallback;
        private KiiHttpClientProgressPercentageCallback progressPercentageCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.ProgressCallbackHelper"/> class.
        /// </summary>
        /// <param name="callback">KiiHttpClientProgressCallback.</param>
        public ProgressCallbackHelper (KiiHttpClientProgressCallback callback)
        {
            this.progressCallback = callback;
            this.progressPercentageCallback = null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.ProgressCallbackHelper"/> class.
        /// </summary>
        /// <param name="callback">KiiHttpClientProgressPercentageCallback.</param>
        public ProgressCallbackHelper (KiiHttpClientProgressPercentageCallback callback)
        {
            this.progressCallback = null;
            this.progressPercentageCallback = callback;
        }
        /// <summary>
        /// Notifies the progress.
        /// </summary>
        /// <param name="doneByte">Done byte.</param>
        /// <param name="totalByte">Total byte.</param>
        public void NotifyProgress(long doneByte, long totalByte)
        {
            if (this.progressCallback != null)
            {
                this.progressCallback(doneByte, totalByte);
            }
            if (this.progressPercentageCallback != null)
            {
                this.progressPercentageCallback((float)(doneByte/totalByte));
            }
        }
    }
}

