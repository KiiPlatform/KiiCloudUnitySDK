using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Generate unity SDK client info.
    /// </summary>
    /// <remarks>
    /// For internal use.
    /// </remarks>
    public class SDKClientInfo
    {
        private const string mSDKVersion = ConstantValues.SDK_VERSION;
        private const string mSDKClientInfo = "sn=cs;sv=" + mSDKVersion;

        /// <summary>
        /// Returns unity cloud storage SDK client info.
        /// </summary>
        public static string GetSDKClientInfo ()
        {
            return mSDKClientInfo;
        }
    }
}
