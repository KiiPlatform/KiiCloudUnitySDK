using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides enumerated values that indicate a type of ACL Action for <see cref="KiiBucket"/>.
    /// </summary>
    /// <remarks>
    /// This enum is used for <see cref="KiiBucket.Acl(BucketAction)"/>
    /// </remarks>
    public enum BucketAction
    {
        /// <summary>
        /// Action of query object in the bucket.
        /// </summary>
        QUERY_OBJECTS_IN_BUCKET,

        /// <summary>
        /// Action of create object in the bucket.
        /// </summary>/
        CREATE_OBJECTS_IN_BUCKET,

        /// <summary>
        /// Action of drop bucket.
        /// It will also remove all the contents in the bucket.
        /// </summary>
        DROP_BUCKET_WITH_ALL_CONTENT,
        /// <summary>
        /// If this Action is granted, subject can READ the objects stored in the bucket.
        /// When it's dropped, subject can only READ the objects that has ACL entry allows subject to READ
        /// (ObjectAction.READ_EXISTING_OBJECT).
        /// </summary>
        READ_OBJECTS_IN_BUCKET
    }
}

