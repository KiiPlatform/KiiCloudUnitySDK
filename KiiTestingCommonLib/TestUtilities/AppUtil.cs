using System;

namespace KiiCorp.Cloud.Storage
{
    public class AppUtil
    {
        private const string BASE_URL_US = "https://api.kii.com/api";
        private const string BASE_URL_JP = "https://api-jp.kii.com/api";
        private const string BASE_URL_CN = "https://api-cn2.kii.com/api";

        public static string getUrlOfSite(Kii.Site site, string appId, params string[] segments)
        {
            string url = getBaseUrl(site);
            url += "/apps/" + appId;
            foreach (string path in segments)
            {
                url += "/" + path;
            }
            return url;
        }

        private static String getBaseUrl(Kii.Site site)
        {
            switch (site)
            {
            case Kii.Site.JP:
                return BASE_URL_JP;
            case Kii.Site.US:
                return BASE_URL_US;
            case Kii.Site.CN:
                return BASE_URL_CN;
            default:
                throw new Exception("Unknown site");
            }
        }
    }
}

