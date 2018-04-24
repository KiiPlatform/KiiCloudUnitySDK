using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Kii server code environment version.
    /// </summary>
    public enum KiiServerCodeEnvironmentVersion
    {
        /// <summary>
        /// v0.x
        /// </summary>
        V0,
        /// <summary>
        /// v6.x
        /// </summary>
        V6
    }
    internal static class KiiServerCodeEnvironmentVersionExtensions
    {
        internal static string GetValue(this KiiServerCodeEnvironmentVersion environmentVersion)
        {
            switch (environmentVersion)
            {
                case KiiServerCodeEnvironmentVersion.V0:
                    return "0";
                case KiiServerCodeEnvironmentVersion.V6:
                    return "6";
            }
            throw new Exception("Unexpected EnvironmentVersion " + Enum.GetName(typeof(KiiServerCodeEnvironmentVersion), environmentVersion));
        }
        internal static KiiServerCodeEnvironmentVersion? FromValue(string value)
        {
            if (value == null)
            {
                return null;
            }
            switch (value)
            {
                case "0":
                    return KiiServerCodeEnvironmentVersion.V0;
                case "6":
                    return KiiServerCodeEnvironmentVersion.V6;
            }
            throw null;
        }
    }
}

