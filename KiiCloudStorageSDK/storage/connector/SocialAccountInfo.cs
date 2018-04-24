using System;

namespace KiiCorp.Cloud.Storage.Connector
{
    /// <summary>
    /// Social account info.
    /// </summary>
    public class SocialAccountInfo
    {
        private Provider provider;
        private string socialAccountId;
        private DateTime createdAt;

        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.Connector.SocialAccountInfo"/> class.
        /// </summary>
        /// <param name="provider">Provider.</param>
        /// <param name="socialAccountId">Social account identifier.</param>
        /// <param name="createdAt">Created at.</param>
        internal SocialAccountInfo(Provider provider, string socialAccountId, long createdAt) : this(provider, socialAccountId, Utils.UnixTimeToDateTime(createdAt))
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.Connector.SocialAccountInfo"/> class.
        /// </summary>
        /// <param name="provider">Provider.</param>
        /// <param name="socialAccountId">Social account identifier.</param>
        /// <param name="createdAt">Created at.</param>
        internal SocialAccountInfo(Provider provider, string socialAccountId, DateTime createdAt)
        {
            this.provider = provider;
            this.socialAccountId = socialAccountId;
            this.createdAt = createdAt;
        }
        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <value>The provider.</value>
        public Provider Provider
        {
            get
            {
                return this.provider;
            }
        }
        /// <summary>
        /// Gets the social account identifier.
        /// </summary>
        /// <value>The social account identifier.</value>
        public string SocialAccountId
        {
            get
            {
                return this.socialAccountId;
            }
        }
        /// <summary>
        /// Gets the created at.
        /// </summary>
        /// <value>The created at.</value>
        public DateTime CreatedAt
        {
            get
            {
                return this.createdAt;
            }
        }
    }
}

