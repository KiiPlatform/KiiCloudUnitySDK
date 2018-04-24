using System;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides Bucket ACL operations.
    /// </summary>
    /// <remarks>
    /// To get this instance, <see cref="KiiBucket.Acl"/>
    /// </remarks>
    public class KiiBucketAcl : KiiACL<KiiBucket, BucketAction>
    {
        private const string ACTION_CREATE_OBJECTS_IN_BUCKET = "CREATE_OBJECTS_IN_BUCKET";
        private const string ACTION_DROP_BUCKET_WITH_ALL_CONTENT = "DROP_BUCKET_WITH_ALL_CONTENT";
        private const string ACTION_QUERY_OBJECTS_IN_BUCKET = "QUERY_OBJECTS_IN_BUCKET";
        private const string ACTION_READ_OBJECTS_IN_BUCKET = "READ_OBJECTS_IN_BUCKET";

        private static string[] ACTION_NAMES = {
            ACTION_CREATE_OBJECTS_IN_BUCKET,
            ACTION_DROP_BUCKET_WITH_ALL_CONTENT,
            ACTION_QUERY_OBJECTS_IN_BUCKET,
            ACTION_READ_OBJECTS_IN_BUCKET
        };

        internal KiiBucketAcl(KiiBucket parent)
        {
            Parent = parent;
        }

        internal KiiBucketAcl(KiiBucket parent, BucketAction action)
        {
            Parent = parent;
            Action = action;
        }

        internal override string ToActionString(BucketAction action)
        {
            switch (action)
            {
                case BucketAction.CREATE_OBJECTS_IN_BUCKET:
                    return ACTION_CREATE_OBJECTS_IN_BUCKET;
                case BucketAction.DROP_BUCKET_WITH_ALL_CONTENT:
                    return ACTION_DROP_BUCKET_WITH_ALL_CONTENT;
                case BucketAction.QUERY_OBJECTS_IN_BUCKET:
                    return ACTION_QUERY_OBJECTS_IN_BUCKET;
                case BucketAction.READ_OBJECTS_IN_BUCKET:
                    return ACTION_READ_OBJECTS_IN_BUCKET;
                default:
                    throw new SystemException("unexpected error." + action.GetType().ToString());
            }
        }

        internal override BucketAction ToAction (string actionName)
        {
            switch (actionName)
            {
            case ACTION_CREATE_OBJECTS_IN_BUCKET:
                return BucketAction.CREATE_OBJECTS_IN_BUCKET;
            case ACTION_DROP_BUCKET_WITH_ALL_CONTENT:
                return BucketAction.DROP_BUCKET_WITH_ALL_CONTENT;
            case ACTION_QUERY_OBJECTS_IN_BUCKET:
                return BucketAction.QUERY_OBJECTS_IN_BUCKET;
            case ACTION_READ_OBJECTS_IN_BUCKET:
                return BucketAction.READ_OBJECTS_IN_BUCKET;
            default:
                throw new ArgumentException("actionName is not BucketAction");
            }
        }

        internal override KiiACL<KiiBucket, BucketAction> CreateFromAction (KiiBucket parent, BucketAction action)
        {
            return new KiiBucketAcl(parent, action);
        }

        internal override string[] ActionNames
        {
            get
            {
                return ACTION_NAMES;
            }
        }

        internal override string ParentID
        {
            get
            {
                return Parent.Name;
            }
        }

        internal override string ParentUrl
        {
            get
            {
                return Parent.Url;
            }
        }
    }
}

