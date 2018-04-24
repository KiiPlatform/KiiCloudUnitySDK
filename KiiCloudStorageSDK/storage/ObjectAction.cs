using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides enumerated values that indicate a type of ACL Action for <see cref="KiiObject"/>.
    /// </summary>
    /// <remarks>
    /// This enum is used for <see cref="KiiObject.Acl(ObjectAction)"/>
    /// </remarks>
    public enum ObjectAction
    {
        /// <summary>
        /// Action of read object in the bucket.
        /// </summary>
        READ_EXISTING_OBJECT,

        /// <summary>
        /// Action of modify, delete object in the bucket.
        /// </summary>
        WRITE_EXISTING_OBJECT
    }
}

