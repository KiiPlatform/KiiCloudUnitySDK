using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The exception that is thrown when the requested resource is gone.
    /// </summary>
    public class GoneException : CloudException
    {
        internal GoneException(String message, SystemException cause, String httpBody) : base (410, message, cause, httpBody)
        {
        }
    }
}

