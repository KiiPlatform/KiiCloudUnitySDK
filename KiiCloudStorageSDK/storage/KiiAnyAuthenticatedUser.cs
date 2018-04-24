using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents any user who is authenticated
    /// </summary>
    /// <remarks>
    /// This user is used for setting ACL.
    /// <code>
    /// // allow authenticated user to create object in user scope bucket.
    /// KiiUser.CurrentUser.bucket("inbox").Acl(BucketAction.CREATE_OBJECTS_IN_BUCKET)
    /// .Subject(KiiAnyAuthenticatedUser.Get()).Save(ACLOperation.GRANT);
    /// </code>
    /// </remarks>
    public class KiiAnyAuthenticatedUser : KiiSubject
    {
        private static KiiAnyAuthenticatedUser INSTANCE = new KiiAnyAuthenticatedUser();

        private KiiAnyAuthenticatedUser()
        {

        }
        
        /// <summary>
        /// Get any authenticated user instance
        /// </summary>
        /// <remarks>
        /// This instance is used for setting ACL.
        /// </remarks>
        /// <returns>
        /// KiiUser instance.
        /// </returns>
        public static KiiAnyAuthenticatedUser Get()
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
                return "UserID:ANY_AUTHENTICATED_USER";
            }
        }

        #endregion
    }
}

