using System;
using System.IO;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// This callback is used when received API response.
    /// </summary>
    /// <param name="response">API response</param>
    /// <param name="e">The exception thrown in execution. If this value is null, the execution is done successfully. </param>
    /// <remarks></remarks>
    public delegate void KiiHttpClientCallback(ApiResponse response, Exception e);
    /// <summary>
    /// A KiiHttpClientProgressCallback is used to get progress of an operation. 
    /// </summary>
    /// <param name="doneByte">Completed size of transfer in bytes.</param>
    /// <param name="totalByte">Total size of transfer in bytes.</param>
    /// <remarks></remarks>
    public delegate void KiiHttpClientProgressCallback(long doneByte, long totalByte);
    /// <summary>
    /// A KiiHttpClientProgressCallback is used to get progress of an operation. 
    /// </summary>
    /// <param name="progress">This is a value between zero and one; 0 means nothing is downloaded, 1 means download complete.</param>
    /// <remarks></remarks>
    public delegate void KiiHttpClientProgressPercentageCallback(float progress);

    /// <summary>
    /// Kii http client. All Http request should be called through this interface
    /// </summary>
    /// <remarks></remarks>
    public interface KiiHttpClient
    {
        /// <summary>
        /// Sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        /// <remarks></remarks>
        string Body { set; }

        /// <summary>
        /// Sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        /// <remarks></remarks>
        string ContentType { set; }

        /// <summary>
        /// Sets the Accept value.
        /// </summary>
        /// <value>
        /// The accept.
        /// </value>
        /// <remarks></remarks>
        string Accept { set; }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        /// <remarks></remarks>
        KiiHttpHeaderList Headers { get; }

        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <returns>
        /// The response.
        /// </returns>
        /// <remarks></remarks>
        ApiResponse SendRequest();

        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <param name="callback">Callback.</param>
        /// <remarks></remarks>
        void SendRequest(KiiHttpClientCallback callback);

        /// <summary>
        /// Sends the request with body string.
        /// </summary>
        /// <param name="body">Body.</param>
        /// <param name="callback">Callback.</param>
        /// <remarks></remarks>
        void SendRequest(string body, KiiHttpClientCallback callback);

        /// <summary>
        /// Sends the request with body stream.
        /// </summary>
        /// <param name="body">Body.</param>
        /// <param name="progressCallback">Callback for progrees.</param>
        /// <param name="callback">Callback.</param>
        /// <remarks></remarks>
        void SendRequest(Stream body, KiiHttpClientProgressCallback progressCallback, KiiHttpClientCallback callback);

        /// <summary>
        /// Sends the request with body stream.
        /// </summary>
        /// <param name="body">Body.</param>
        /// <param name="progressCallback">Callback for progrees.</param>
        /// <param name="callback">Callback.</param>
        /// <remarks></remarks>
        void SendRequest(Stream body, KiiHttpClientProgressPercentageCallback progressCallback, KiiHttpClientCallback callback);

        /// <summary>
        /// Sends the request for downloading.
        /// </summary>
        /// <param name="outStream">Stream to be written.</param>
        /// <param name="progressCallback">Callback for progrees.</param>
        /// <param name="callback">Callback.</param>
        /// <remarks></remarks>
        void SendRequestForDownload(Stream outStream, KiiHttpClientProgressCallback progressCallback, KiiHttpClientCallback callback);

        /// <summary>
        /// Sends the request for downloading.
        /// </summary>
        /// <param name="outStream">Stream to be written.</param>
        /// <param name="progressCallback">Callback for progrees.</param>
        /// <param name="callback">Callback.</param>
        /// <remarks></remarks>
        void SendRequestForDownload(Stream outStream, KiiHttpClientProgressPercentageCallback progressCallback, KiiHttpClientCallback callback);
    }
}

