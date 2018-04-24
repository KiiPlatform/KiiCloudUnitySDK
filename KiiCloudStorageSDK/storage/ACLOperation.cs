using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides enumerated values that indicate a type of ACL operation.
    /// </summary>
    /// <remarks>
    /// This enum is used for KiiACLEntry.Save(ACLOperation).
    /// </remarks>
    public enum ACLOperation
    {
        /// <summary>
        /// Allows subject to do an action.
        /// </summary>
        GRANT,

        /// <summary>
        /// Forbids subject to do an action.
        /// If GRANT entry is not in KiiCloud, this operation will be failed.
        /// </summary>
        REVOKE
    }
}

