using System;
using System.Globalization;

namespace KiiCorp.Cloud.Storage.Connector
{
    /// <summary>
    /// Supported social network.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public enum Provider
    {
        /// <summary>
        /// Facebook
        /// </summary>
        /// <remarks>
        /// User Facebook to authenticate.
        /// </remarks>
        FACEBOOK,

        /// <summary>
        /// Twitter
        /// </summary>
        /// <remarks>
        /// Use Twitter to authenticate
        /// </remarks>
        TWITTER,

        /// <summary>
        /// Twitter
        /// </summary>
        /// <remarks>
        /// Use LinkedIn to authenticate
        /// </remarks>
        LINKEDIN,

        /// <summary>
        /// Yahoo
        /// </summary>
        /// <remarks>
        /// Use Yahoo to authenticate
        /// </remarks>
        YAHOO,

        /// <summary>
        /// Google
        /// </summary>
        /// <remarks>
        /// Use Google to authenticate
        /// </remarks>
        [Obsolete("Use Provider.GOOGLEPLUS instead")]
        GOOGLE,

        /// <summary>
        /// Google
        /// </summary>
        /// <remarks>
        /// Use GooglePlus to authenticate
        /// </remarks>
        GOOGLEPLUS,

        /// <summary>
        /// Dropbox
        /// </summary>
        /// <remarks>
        /// Use Dropbox to authenticate
        /// </remarks>
        DROPBOX,

        /// <summary>
        /// Box
        /// </summary>
        /// <remarks>
        /// Use Box to authenticate
        /// </remarks>
        BOX,

        /// <summary>
        /// RenRen
        /// </summary>
        /// <remarks>
        /// Use RenRen to authenticate
        /// </remarks>
        RENREN,

        /// <summary>
        /// Sina
        /// </summary>
        /// <remarks>
        /// Use Sina Weibo to authenticate
        /// </remarks>
        SINA,

        /// <summary>
        /// Live
        /// </summary>
        /// <remarks>
        /// Use Live to authenticate
        /// </remarks>
        LIVE,
        /// <summary>
        /// QQ
        /// </summary>
        /// <remarks>
        /// Use QQ to authenticate
        /// </remarks>
        QQ,
        /// <summary>
        /// Kii
        /// </summary>
        /// <remarks>
        /// Use Kii to authenticate
        /// </remarks>
        KII
    }

    // Provider extension method.
    internal static class ProviderExtensions
    {
        internal static string GetProviderName(this Provider p)
        {
            return p.ToString().ToLower();
        }

        internal static string GetLinkedProviderSocialNetworkName(this Provider p)
        {
            string name = p.GetProviderName();
            return "google".Equals(name) ? "googleplus" : "live".Equals(name) ? "hotmail" : name;
        }

        internal static string GetProviderNameFromLinkedSocialNetworkName(string socialNetworkName) {
            return socialNetworkName.Equals("hotmail") ? 
                Provider.LIVE.GetProviderName() : socialNetworkName;
        }

        internal static string GetTokenRequestContentType(this Provider p)
        {
            if (p == Provider.RENREN)
            {
                return "application/vnd.kii.AuthTokenRenRenRequest+json";
            }
            if (p == Provider.QQ)
            {
                return "application/vnd.kii.AuthTokenQQRequest+json";
            }
            string providerName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(p.GetProviderName());
            return "application/vnd.kii.AuthToken" + providerName + "Request+json";
        }
        internal static string GetLinkRequestContentType(this Provider p)
        {
            if (p == Provider.RENREN)
            {
                return "application/vnd.kii.LinkRenRenRequest+json";
            }
            if (p == Provider.QQ)
            {
                return "application/vnd.kii.LinkQQRequest+json";
            }
            string providerName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(p.GetProviderName());
            return "application/vnd.kii.Link" + providerName + "Request+json";
        }
    }
}

