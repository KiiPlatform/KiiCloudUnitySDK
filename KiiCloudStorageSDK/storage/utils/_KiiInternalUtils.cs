using System;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Utility class for internal use.
    /// </summary>
    public static class _KiiInternalUtils
    {
        /// <summary>
        /// Sets the current user.
        /// </summary>
        /// <param name="user">User.</param>
        /// <remarks>Do not use it from your application.</remarks>
        public static void SetCurrentUser(KiiUser user)
        {
            Kii.CurrentUser = user;
        }
        /// <summary>
        /// Updates the access token.
        /// </summary>
        /// <param name="accessToken">Access token.</param>
        /// <remarks>Do not use it from your application.</remarks>
        public static void UpdateAccessToken(string accessToken)
        {
            KiiCloudEngine.UpdateAccessToken(accessToken);
        }
        /// <summary>
        /// Sets the social access token dictionary to KiiUser.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="socialAccessTokenDictionary">Social access token dictionary.</param>
        /// <remarks>Do not use it from your application.</remarks>
        public static void SetSocialAccessTokenDictionary(KiiUser user, Dictionary<string, object> socialAccessTokenDictionary)
        {
            user.SetSocialAccessTokenDictionary(socialAccessTokenDictionary);
        }
    }
}

