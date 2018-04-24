using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The exception that is thrown when the request is not authorized.
    /// </summary>
    public class UnauthorizedException : CloudException
    {
        internal UnauthorizedException (String message, SystemException cause, String httpBody) : base (401, message, cause, httpBody)
        {
        }
    }
}

