using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Thrown if a user already has the identity when the operation is expected for targeted to a pseudo user.
    /// </summary>
    /// <remarks></remarks>
    public class AlreadyHasIdentityException : SystemException
    {
        internal AlreadyHasIdentityException() : base()
        {
        }
        internal AlreadyHasIdentityException(string detailMessage) : base(detailMessage)
        {
        }
        internal AlreadyHasIdentityException(string detailMessage, Exception e) : base(detailMessage, e)
        {
        }
    }
}
