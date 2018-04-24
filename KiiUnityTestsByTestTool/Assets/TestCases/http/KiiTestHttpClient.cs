using System;
using System.IO;

namespace KiiCorp.Cloud.Storage
{

    public interface KiiTestHttpClient
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
        KiiTestHttpHeaderList Headers { get; }

        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <returns>
        /// The response.
        /// </returns>
        /// <remarks></remarks>
        KiiTestHttpResponse SendRequest();
    }
}

