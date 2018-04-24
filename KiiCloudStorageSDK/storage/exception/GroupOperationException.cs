using System;
using System.Collections.Generic;
using System.IO;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The exception that is thrown when Group operation is failed.
    /// </summary>
    /// <remarks>
    /// Developers can know which users are failed to add/remove.
    /// </remarks>
    public class GroupOperationException : SystemException
    {
        private IList<KiiUser> mAddFailUsers;
        private IList<KiiUser> mRemoveFailUsers;

        internal GroupOperationException(string message,
            Exception exception, IList<KiiUser> addFailUsers,
            IList<KiiUser> removeFailUsers) : base(message, exception)
        {
            this.mAddFailUsers = (addFailUsers != null) ? new List<KiiUser>(addFailUsers) : new List<KiiUser>();
            this.mRemoveFailUsers = (removeFailUsers != null) ? new List<KiiUser>(removeFailUsers) : new List<KiiUser>();
        }

        #region properties
        /// <summary>
        /// Gets the users who are not added.
        /// </summary>
        /// <remarks>
        /// This property doesn't return null
        /// </remarks>
        /// <value>
        /// The list of KiiUser who will be added to the group.
        /// </value>
        public IList<KiiUser> AddFailedUsers
        {
            get
            {
                return mAddFailUsers;
            }
        }
         
        /// <summary>
        /// Gets the users who are not removed.
        /// </summary>
        /// <remarks>
        /// This property doesn't return null
        /// </remarks>
        /// <value>
        /// The list of KiiUser who will be removed from the group.
        /// </value>
        public IList<KiiUser> RemoveFailedUsers
        {
            get
            {
                return mRemoveFailUsers;
            }
        }

        #endregion

    }
}

