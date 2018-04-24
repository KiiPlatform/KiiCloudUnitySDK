using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Collection of constant values for internal use.
    /// </summary>
    public class ConstantValues
    {
        internal const string DEFAULT_BASE_URL = "https://api.kii.com/api";
        internal const string DEFAULT_BASE_URL_JP = "https://api-jp.kii.com/api";
        internal const string DEFAULT_BASE_URL_CN = "https://api-cn2.kii.com/api";
        internal const string DEFAULT_BASE_URL_SG = "https://api-sg.kii.com/api";
        internal const string DEFAULT_BASE_URL_CN3 = "https://api-cn3.kii.com/api";
        internal const string DEFAULT_BASE_URL_EU = "https://api-eu.kii.com/api";

        internal const string URI_HEADER = "kiicloud://";

        internal const string URI_SCHEME = "kiicloud";

        /// <summary>
        ///   SDK version.
        /// </summary>
        /// <remarks>
        ///   Do not use this from your application.
        /// </remarks>
        public const string SDK_VERSION = "3.2.10.0";

        /// <summary>
        ///   User agent name.
        /// </summary>
        /// <remarks>
        ///   Do not use this from your application.
        /// </remarks>
        public const string UA_PRODUCT = "KiiSDKUnity";

    }
}
