using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents Anonymous user
    /// </summary>
    /// <remarks>
    /// This user is used for setting ACL.
    /// <code>
    /// // allow anonymous user to create object in user scope bucket.
    /// KiiUser.CurrentUser.bucket("inbox").Acl(BucketAction.CREATE_OBJECTS_IN_BUCKET)
    /// .Subject(KiiAnonymousUser.Get()).Save(ACLOperation.GRANT);
    /// </code>
    /// </remarks>
    public class KiiAnonymousUser : KiiSubject
    {
        private static KiiAnonymousUser INSTANCE = new KiiAnonymousUser();

        private KiiAnonymousUser()
        {

        }
        /// <summary>
        /// Get Anonymous user instance
        /// </summary>
        /// <remarks>
        /// This instance is used for setting ACL.
        /// </remarks>
        /// <returns>
        /// KiiUser instance.
        /// </returns>
        public static KiiAnonymousUser Get()
        {
            return INSTANCE;
        }

        #region KiiSubject
        /// <summary>
        /// Gets the subject string.
        /// </summary>
        /// <remarks>
        /// Developer don't need to use this property in their apps.
        /// </remarks>
        /// <value>
        /// The subject string.
        /// </value>
        public string Subject
        {
            get
            {
                return "UserID:ANONYMOUS_USER";
            }
        }
        #endregion
    }
}

