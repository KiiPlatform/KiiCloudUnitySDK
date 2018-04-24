using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// This exception will be thrown when .NET framework throws a network-related exception.
    /// </summary>
    /// <remarks>
    /// To know the details, please use <see cref="InnerException"/> property.
    /// </remarks>
    public class NetworkException : CloudException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.NetworkException"/> class.
        /// </summary>
        /// <param name="cause">Cause.</param>
        /// <remarks></remarks>
        public NetworkException (SystemException cause) : base(-1, "network related exception", cause)
        {
        }
    }
}

