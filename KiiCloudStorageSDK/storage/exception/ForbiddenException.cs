using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The exception that is thrown when the Request is forbidden.
    /// </summary>
    public class ForbiddenException : CloudException
    {
        internal ForbiddenException (String message, SystemException cause, String httpBody) : base (403, message, cause, httpBody)
        {
        }
    }
}

