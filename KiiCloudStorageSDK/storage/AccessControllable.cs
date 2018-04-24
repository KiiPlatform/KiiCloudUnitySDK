using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents an object to which user can set ACL.
    /// </summary>
    /// <remarks>
    /// Developers don't need to implement this interface in their apps.
    /// </remarks>
    public interface AccessControllable
    {
        /// <summary>
        /// Gets the URI of this instance
        /// </summary>
        /// <remarks></remarks>
        /// <value>
        /// The URI of this instance.
        /// </value>
        Uri Uri{
            get;
        }
    }
}

