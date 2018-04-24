using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides Object ACL operations.
    /// </summary>
    /// <remarks>
    /// To get this instance, use <see cref="KiiObject.Acl"/>
    /// </remarks>
    public class KiiObjectAcl : KiiACL<KiiObject, ObjectAction>
    {
        private const string ACTION_READ_EXISTING_OBJECT = "READ_EXISTING_OBJECT";
        private const string ACTION_WRITE_EXISTING_OBJECT = "WRITE_EXISTING_OBJECT";

        private static string[] ACTION_NAMES = {
            ACTION_READ_EXISTING_OBJECT,
            ACTION_WRITE_EXISTING_OBJECT
        };

        internal KiiObjectAcl (KiiObject parent)
        {
            Parent = parent;
        }

        internal KiiObjectAcl (KiiObject parent, ObjectAction action)
        {
            Parent = parent;
            Action = action;
        }

        internal override void SaveParentIfNeeds()
        {
            if (Utils.IsEmpty(Parent.ID))
            {
                Parent.Save();
            }
        }


        internal override string ToActionString(ObjectAction action)
        {
            switch (action)
            {
                case ObjectAction.READ_EXISTING_OBJECT:
                    return ACTION_READ_EXISTING_OBJECT;
                case ObjectAction.WRITE_EXISTING_OBJECT:
                    return ACTION_WRITE_EXISTING_OBJECT;
                default:
                    throw new SystemException("unexpected error." + action.GetType().ToString());
            }
        }

        internal override ObjectAction ToAction (string actionName)
        {
            switch (actionName)
            {
            case ACTION_READ_EXISTING_OBJECT:
                return ObjectAction.READ_EXISTING_OBJECT;
            case ACTION_WRITE_EXISTING_OBJECT:
                return ObjectAction.WRITE_EXISTING_OBJECT;
            default:
                throw new ArgumentException();
            }
        }

        internal override KiiACL<KiiObject, ObjectAction> CreateFromAction (KiiObject parent, ObjectAction action)
        {
            return new KiiObjectAcl(parent, action);
        }

        internal override string ParentID
        {
            get
            {
                return Parent.ID;
            }
        }

        internal override string ParentUrl
        {
            get
            {
                return Parent.Url;
            }
        }

        internal override string[] ActionNames
        {
            get
            {
                return ACTION_NAMES;
            }
        }
    }
}

