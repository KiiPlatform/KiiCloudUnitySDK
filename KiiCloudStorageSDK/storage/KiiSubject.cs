using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents an object that can be set as ACL subject.
    /// </summary>
    /// <remarks>
    /// Developers don't need to implement this interface in their apps.
    /// </remarks>
    public interface KiiSubject
    {
        /// <summary>
        /// Gets the subject of this instance.
        /// </summary>
        /// <remarks>
        /// Developers don't need to use this property in their apps.
        /// </remarks>
        /// <value>
        /// The subject string.
        /// </value>
        string Subject
        {
            get;
        }
    }
}

