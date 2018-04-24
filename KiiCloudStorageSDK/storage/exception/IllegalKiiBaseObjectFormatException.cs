using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The exception that is thrown when SDK receives broken json.
    /// </summary>
    /// <remarks>
    /// This exception is not thrown basically.
    /// </remarks>
    public class IllegalKiiBaseObjectFormatException : SystemException
    {
        internal IllegalKiiBaseObjectFormatException (string detailMessage) : base(detailMessage)
        {
        }

        internal IllegalKiiBaseObjectFormatException (string detailMessage, Exception e) : base(detailMessage, e)
        {

        }
    }
}

