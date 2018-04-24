using System;
using System.Net;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    internal class KiiCloudEngine
    {
        private static string ACCESS_TOKEN = null;

        public static void UpdateAccessToken(string accessToken) {
            ACCESS_TOKEN = accessToken;
        }

        public static void ClearAccessToken() {
            ACCESS_TOKEN = null;
        }
        internal static void SetAuthBearer(KiiHttpClient client)
        {
            string accessToken = ACCESS_TOKEN;
            if (Utils.IsEmpty(accessToken)) { return; }

            client.Headers["Authorization"] = "Bearer " + accessToken;
        }


        public static void SetAuthBearer(HttpWebRequest request)
        {
            string accessToken = ACCESS_TOKEN;
            if (!Utils.IsEmpty(accessToken))
                request.Headers.Add("Authorization", "Bearer " + accessToken);
        }

        #region properties

        internal static string AccessToken
        {
            get
            {
                return ACCESS_TOKEN;
            }
        }
        #endregion
    }
}

