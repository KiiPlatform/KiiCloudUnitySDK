using System;
using System.Collections.Generic;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides APIs for KiiCloud application features.
    /// </summary>
    /// <remarks>
    /// Please call Kii.Initialize(string, string, Kii.Site) at the top of your application.
    /// </remarks>
    public class Kii
    {
        /// <summary>
        /// Provides enumerated values that indicate Kii sites.
        /// </summary>
        /// <remarks>
        /// This enum is used for Kii.Initialize(string, string, Kii.Site).
        /// </remarks>
        public enum Site {
            /// <summary>
            /// Use cloud in Japan.
            /// </summary>
            JP,

            /// <summary>
            /// Use cloud in United States.
            /// </summary>
            US,

            /// <summary>
            /// Use cloud in China.
            /// </summary>
            CN,

            /// <summary>
            /// Use cloud in Singapore.
            /// </summary>
            SG,

            /// <summary>
            /// Use cloud in cn3 site of China.
            /// </summary>
            CN3,
            /// <summary>
            /// Use cloud in European Union.
            /// </summary>
            EU
        }

        private static Kii INSTANCE = null;

        private string mAppId;

        private string mAppKey;

        private KiiUser mLoginUser;

        private string mBaseURL;

        private string mKiiAppsBaseURL;

        private long mAccessTokenExpiration;

        private KiiHttpClientFactory clientFactory;

        private KiiHttpClientFactory asyncClientFactory;

        private static IKiiLogger logger = null;

        private Kii(string appId, string appKey)
        {
            if (Utils.IsEmpty(appId))
            {
                throw new ArgumentException(ErrorInfo.KII_APP_ID_IS_NULL);
            }
            if (Utils.IsEmpty(appKey))
            {
                throw new ArgumentException(ErrorInfo.KII_APP_KEY_IS_NULL);
            }
            mAppId = appId;
            mAppKey = appKey;
        }

        /// <summary>
        /// Gets or set the access token lifetime in seconds.
        /// </summary>
        /// <remarks>
        /// If you don't set this or set with 0, token won't be expired.
        /// Set this if you like the access token to be expired
        /// after a certain period. Once set, token retrieved
        /// by each future authentication will have the specified lifetime.
        /// Note that, it will not update the lifetime of token received prior
        /// calling this method. Once expired, you have to login again to renew the token.
        /// </remarks>
        /// <value>
        /// The life time of access token in seconds.
        /// </value>
        /// <exception cref="InvalidOperationException">
        /// Thrown if SDK has not initialized yet.
        /// </exception>
        public static long AccessTokenExpiration
        {
            get {
                if(INSTANCE == null)
                    throw new InvalidOperationException("SDK has not initialized");
                return INSTANCE.mAccessTokenExpiration;
            }
            set
            {
                if(INSTANCE == null)
                    throw new InvalidOperationException("SDK has not initialized");
                if(value < 0)
                    throw new ArgumentException("value can not be negative");
                INSTANCE.mAccessTokenExpiration = value;
            }
        }

        #region Initialize

        /// <summary>
        /// Initialize KiiSDK appId, appKey and Site.
        /// </summary>
        /// <remarks>
        /// Initialize KiiCloudStorage SDK and this must be call prior to all APIs call.
        /// </remarks>
        /// <param name='appId'>
        /// Application ID found in your Kii developer console.
        /// </param>
        /// <param name='appKey'>
        /// Application key found in your Kii developer console
        /// </param>
        /// <param name='site'>
        /// Please set any one of Site.US, Site.JP, Site.CN, Site.CN3 or Site.SG you've chosen when you created application on Kii developer console.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid.
        /// </exception>
        public static void Initialize(string appId, string appKey, Site site)
        {
            Initialize(appId, appKey, DeterminServerUrl(site));
        }
        /// <summary>
        /// Initialize KiiSDK appId, appKey and serverUrl.
        /// </summary>
        /// <remarks>
        /// Initialize KiiCloudStorage SDK and this must be call prior to all APIs call.
        /// </remarks>
        /// <param name='appId'>
        /// Application ID found in your Kii developer console
        /// </param>
        /// <param name='appKey'>
        /// Application key found in your Kii developer console
        /// </param>
        /// <param name='serverUrl'>
        /// Server URL.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument passed to a method is invalid.
        /// </exception>
        public static void Initialize(string appId, string appKey, string serverUrl)
        {
            Initialize(appId, appKey, serverUrl, new KiiClientFactoryImpl(), new KiiAsyncClientFactoryImpl());
        }
        /// <summary>
        /// This method is intended for use in internal purposes. Do not use it to initialize your application.
        /// </summary>
        /// <remarks>
        /// Initialize KiiCloudStorage SDK and this must be call prior to all APIs call.
        /// </remarks>
        /// <param name='appId'>
        /// Application ID found in your Kii developer console
        /// </param>
        /// <param name='appKey'>
        /// Application key found in your Kii developer console
        /// </param>
        /// <param name='site'>
        /// Please set any one of Site.US, Site.JP, Site.CN, Site.CN3 or Site.SG you've chosen when you created application on Kii developer console.
        /// </param>
        /// <param name='syncFactory'>
        /// Http client factory for blocking api.
        /// </param>
        /// <param name='asyncFactory'>
        /// Http client factory for non-blocking api.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument passed to a method is invalid.
        /// </exception>
        public static void Initialize(string appId, string appKey, Site site, KiiHttpClientFactory syncFactory, KiiHttpClientFactory asyncFactory)
        {
            Initialize(appId, appKey, DeterminServerUrl(site), syncFactory, asyncFactory);
        }
        /// <summary>
        /// Initialize KiiSDK appID, appKey, deviceID, serverUrl and KiiHttpClientFactory.
        /// </summary>
        /// <remarks>
        /// This method is intended for use in internal purposes. Do not use it to initialize your application.
        /// </remarks>
        /// <param name='appId'>
        /// Application ID found in your Kii developer console
        /// </param>
        /// <param name='appKey'>
        /// Application key found in your Kii developer console
        /// </param>
        /// <param name='serverUrl'>
        /// Server URL.
        /// </param>
        /// <param name='syncFactory'>
        /// Http client factory for blocking api.
        /// </param>
        /// <param name='asyncFactory'>
        /// Http client factory for non-blocking api.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument passed to a method is invalid.
        /// </exception>
        public static void Initialize(string appId, string appKey, string serverUrl, KiiHttpClientFactory syncFactory, KiiHttpClientFactory asyncFactory)
        {
            if (Utils.IsEmpty(serverUrl))
            {
                throw new ArgumentException(ErrorInfo.KII_SERVER_URL_IS_NULL);
            }
            if (!Uri.IsWellFormedUriString(serverUrl, UriKind.Absolute))
            {
                throw new ArgumentException(ErrorInfo.KII_INVALID_SERVER_URL);
            }
            if (INSTANCE != null && Kii.AppId == appId && Kii.AppKey == appKey && Kii.BaseUrl == serverUrl)
            {
                return;
            }

            KiiCloudEngine.ClearAccessToken();
            INSTANCE = new Kii(appId, appKey);
            INSTANCE.mBaseURL = serverUrl;
            INSTANCE.mKiiAppsBaseURL = CreateKiiAppsBaseURL(appId, serverUrl);

            INSTANCE.clientFactory = syncFactory;
            INSTANCE.asyncClientFactory = asyncFactory;
        }

        private static String DeterminServerUrl(Site site)
        {
            switch (site)
            {
            case Site.US:
                return ConstantValues.DEFAULT_BASE_URL;
            case Site.JP:
                return ConstantValues.DEFAULT_BASE_URL_JP;
            case Site.CN:
                return ConstantValues.DEFAULT_BASE_URL_CN;
            case Site.SG:
                return ConstantValues.DEFAULT_BASE_URL_SG;
            case Site.CN3:
                return ConstantValues.DEFAULT_BASE_URL_CN3;
            case Site.EU:
                return ConstantValues.DEFAULT_BASE_URL_EU;
            default:
                throw new ArgumentException(ErrorInfo.KII_SITE_INVALID);
            }
        }

        #endregion

        internal static void LogOut() {
            Utils.CheckInitialize(false);
            CurrentUser = null;
            KiiCloudEngine.ClearAccessToken();
        }

        /// <summary>
        /// Gets new Kiibucket whose scope is app scope.
        /// </summary>
        /// <remarks>
        /// This API doesn't call server API.
        /// </remarks>
        /// <returns>
        /// KiiBucket instance.
        /// </returns>
        /// <param name='bucketName'>
        /// Bucket name
        /// </param>
        /// <exception cref="ArgumentException">
        /// Is thrown when bucket name is invalid.
        /// </exception>
        public static KiiBucket Bucket(string bucketName)
        {
            return new KiiBucket(null, bucketName);
        }

        /// <summary>
        /// Create a group own by current user.
        /// </summary>
        /// <remarks>
        /// This API is equivalent to Kii.Group(groupName, null)
        /// </remarks>
        /// <returns>
        /// New KiiGroup instance.
        /// </returns>
        /// <param name='groupName'>
        /// Group name.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Is thrown when group name is empty.
        /// </exception>
        public static KiiGroup Group(string groupName)
        {
            return Group(groupName, null);
        }

        /// <summary>
        /// Create a group with the specified groupName and groupMembers.
        /// </summary>
        /// <remarks>
        /// To create new group on KiiCloud, please call KiiGroup.Save() API of returned KiiGroup.
        /// </remarks>
        /// <returns>
        /// New KiiGroup instance.
        /// </returns>
        /// <param name='groupName'>
        /// Group name.
        /// </param>
        /// <param name='groupMembers'>
        /// Group members.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Is thrown when group name is empty.
        /// </exception>
        public static KiiGroup Group(string groupName, IList<KiiUser> groupMembers)
        {
            if (Utils.IsEmpty(groupName))
            {
                throw new ArgumentException(ErrorInfo.KIIGROUP_NAME_NULL);
            }
            return new KiiGroup(groupName, groupMembers);
        }

        /// <summary>
        /// Instantiate KiiServerCodeEntry with specified entry name.
        /// </summary>
        /// <remarks>
        /// Name must be valid.
        /// </remarks>
        /// <returns>
        /// KiiServerCodeEntry instance.
        /// </returns>
        /// <param name="entryName">
        /// Entry name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when entryName is null or empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when entryName does not match ^[a-zA-Z][_a-zA-Z0-9]*$
        /// </exception>
        public static KiiServerCodeEntry ServerCodeEntry(string entryName)
        {
            return ServerCodeEntry(entryName, KiiServerCodeEntry.VERSION_CURRENT);
        }

        /// <summary>
        /// Instantiate KiiServerCodeEntry with specified entry name and version.
        /// </summary>
        /// <remarks>
        /// Name must be valid.
        /// </remarks>
        /// <returns>
        /// KiiServerCodeEntry instance.
        /// </returns>
        /// <param name="entryName">
        /// Entry name.
        /// </param>
        /// <param name="version">
        /// Version.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when entryName / version is null or empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when entryName does not match ^[a-zA-Z][_a-zA-Z0-9]*$ or version is larger than 36 characters.
        /// </exception>
        public static KiiServerCodeEntry ServerCodeEntry(string entryName, string version)
        {
            return ServerCodeEntry(entryName, version, null);
        }

        /// <summary>
        /// Instantiate KiiServerCodeEntry with specified entry name and environment version.
        /// </summary>
        /// <remarks>
        /// Name must be valid.
        /// </remarks>
        /// <returns>
        /// KiiServerCodeEntry instance.
        /// </returns>
        /// <param name="entryName">
        /// Entry name.
        /// </param>
        /// <param name="environmentVersion">
        /// Environment Version.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when entryName / version is null or empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when entryName does not match ^[a-zA-Z][_a-zA-Z0-9]*$ or version is larger than 36 characters.
        /// </exception>
        public static KiiServerCodeEntry ServerCodeEntry(string entryName, KiiServerCodeEnvironmentVersion? environmentVersion)
        {
            return ServerCodeEntry(entryName, KiiServerCodeEntry.VERSION_CURRENT, environmentVersion);
        }

        /// <summary>
        /// Instantiate KiiServerCodeEntry with specified entry name and version and environment version.
        /// </summary>
        /// <remarks>
        /// Name must be valid.
        /// </remarks>
        /// <returns>
        /// KiiServerCodeEntry instance.
        /// </returns>
        /// <param name="entryName">
        /// Entry name.
        /// </param>
        /// <param name="version">
        /// Version.
        /// </param>
        /// <param name="environmentVersion">
        /// Environment Version.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when entryName / version is null or empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when entryName does not match ^[a-zA-Z][_a-zA-Z0-9]*$ or version is larger than 36 characters.
        /// </exception>
        public static KiiServerCodeEntry ServerCodeEntry(string entryName, string version, KiiServerCodeEnvironmentVersion? environmentVersion)
        {
            if (Utils.IsEmpty(entryName))
            {
                throw new ArgumentNullException("entryName must not be empty.");
            }
            if (Utils.IsEmpty(version))
            {
                throw new ArgumentNullException("version must not be empty.");
            }
            if (version.Length > 36)
            {
                throw new ArgumentException("version must be between 1 and 36 characters.");
            }
            if (!KiiServerCodeEntry.IsValidEntryName(entryName))
            {
                throw new ArgumentException("entryName must be ^[a-zA-Z][_a-zA-Z0-9]*$.");
            }
            return new KiiServerCodeEntry(entryName, version, environmentVersion);
        }

        /// <summary>
        /// Get instance of app scope topic.
        /// The topic bound to the application.
        /// </summary>
        /// <param name="name">topic name.</param>
        /// <returns>KiiTopic bound to the application.</returns>
        /// <remarks></remarks>
        public static KiiTopic Topic(string name)
        {
            KiiTopic topic = new KiiTopic (null, name);
            return topic;
        }
        /// <summary>
        /// Gets the list of topics in app scope.
        /// </summary>
        /// <returns>A list of the topics in app scope.</returns>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <exception cref='UnauthorizedException'>
        /// Is thrown when this method called by anonymous user.
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public static KiiListResult<KiiTopic> ListTopics()
        {
            return ListTopics((string)null);
        }
        /// <summary>
        /// Gets the list of next page of topics in app scope.
        /// </summary>
        /// <param name="paginationKey">
        /// Specifies the pagination key that is obtained by <see cref="KiiListResult{T}.PaginationKey"/>.
        /// If specified null or empty, it's same as the <see cref="ListTopics()"/>.
        /// </param>
        /// <returns>A list of the topics in app scope.</returns>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <exception cref='UnauthorizedException'>
        /// Is thrown when this method called by anonymous user.
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public static KiiListResult<KiiTopic> ListTopics(string paginationKey)
        {
            KiiListResult<KiiTopic> result = null;
            ExecListTopics(Kii.HttpClientFactory, paginationKey, (KiiListResult<KiiTopic> topics, Exception e) => {
                if (e != null)
                {
                    throw e;
                }
                result = topics;
            });
            return result;
        }
        /// <summary>
        /// Asynchronous call for <see cref="ListTopics()"/>.
        /// </summary>
        /// <param name="callback">Callback.</param>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public static void ListTopics(KiiGenericsCallback<KiiListResult<KiiTopic>> callback)
        {
            ListTopics(null, callback);
        }
        /// <summary>
        /// Asynchronous call for <see cref="ListTopics(string)"/>.
        /// </summary>
        /// <param name="paginationKey">
        /// Specifies the pagination key that is obtained by <see cref="KiiListResult{T}.PaginationKey"/>.
        /// If specified null or empty, it's same as the <see cref="ListTopics(KiiGenericsCallback{KiiListResult{KiiTopic}})"/>.
        /// </param>
        /// <param name="callback">Callback.</param>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public static void ListTopics(string paginationKey, KiiGenericsCallback<KiiListResult<KiiTopic>> callback)
        {
            ExecListTopics(Kii.AsyncHttpClientFactory, paginationKey, callback);
        }

        #region properties
        internal static Kii Instance
        {
            get
            {
                return INSTANCE;
            }
            set
            {
                INSTANCE = value;
            }
        }

        /// <summary>
        /// Gets the application ID.
        /// </summary>
        /// <remarks>
        /// If Kii.Initialize(string, string, Kii.Site) is not called, return null.
        /// </remarks>
        /// <value>
        /// Application ID found in your Kii developer console
        /// </value>
        public static string AppId
        {
            get
            {
                if (INSTANCE == null)
                {
                    return null;
                }
                return INSTANCE.mAppId;
            }
        }

        /// <summary>
        /// Gets the application key.
        /// </summary>
        /// <remarks>
        /// If Kii.Initialize(string,string,Kii.Site) is not called, return null.
        /// </remarks>
        /// <value>
        /// Application key found in your Kii developer console
        /// </value>
        public static string AppKey
        {
            get
            {
                if (INSTANCE == null)
                {
                    return null;
                }
                return INSTANCE.mAppKey;
            }
        }

        /// <summary>
        /// Gets the server URL.
        /// </summary>
        /// <remarks>
        /// If Kii.Initialize(string,string,string) is not called, return null.
        /// </remarks>
        /// <value>
        /// The server URL.
        /// </value>
        public static string BaseUrl
        {
            get
            {
                if (INSTANCE == null)
                {
                    return null;
                }
                return INSTANCE.mBaseURL;
            }
        }

        /// <summary>
        /// URL for KiiApps server.
        /// </summary>
        /// <remarks>
        /// No need to serialize because this value is generated in
        /// initialize by appId and serverUrl.
        /// </remarks>
        /// <value>
        /// The KiiApps server URL.
        /// </value>
        public static string KiiAppsBaseUrl
        {
            get
            {
                if (INSTANCE == null)
                {
                    return null;
                }
                return INSTANCE.mKiiAppsBaseURL;
            }
        }

        private static string CreateKiiAppsBaseURL(
                string appId,
                string serverUrl)
        {
            string siteName = null;
            Uri baseUri = new Uri(serverUrl);
            switch (baseUri.Host)
            {
                case "api-jp.kii.com":
                    siteName = "jp";
                    break;
                case "api.kii.com":
                    siteName = "us";
                    break;
                case "api-cn2.kii.com":
                    siteName = "cn";
                    break;
                case "api-sg.kii.com":
                    siteName = "sg";
                    break;
                case "api-cn3.kii.com":
                    siteName = "cn3";
                    break;
                case "api-eu.kii.com":
                    siteName = "eu";
                    break;
                default:
                    siteName = null;
                    break;
            }

            if (siteName == null) {
                return null;
            }
            return "https://" + appId + "." + siteName + ".kiiapps.com/api";

        }

        internal static KiiUser CurrentUser
        {
            get {
                if(INSTANCE == null)
                {
                    return null;
                }
                return INSTANCE.mLoginUser;
            }
            set
            {
                Utils.CheckInitialize(false);
                INSTANCE.mLoginUser = value;
            }
        }

        internal static KiiHttpClientFactory HttpClientFactory
        {
            get
            {
                if (INSTANCE == null)
                {
                    throw new InvalidOperationException(ErrorInfo.UTILS_KIICLIENT_NULL);
                }
                return Instance.clientFactory;
            }
            set
            {
                Instance.clientFactory = value;
            }
        }

        internal static KiiHttpClientFactory AsyncHttpClientFactory
        {
            get
            {
                if (INSTANCE == null)
                {
                    throw new InvalidOperationException(ErrorInfo.UTILS_KIICLIENT_NULL);
                }
                return Instance.asyncClientFactory;
            }
            set
            {
                Instance.asyncClientFactory = value;
            }
        }

        /// <summary>
        /// Sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public static IKiiLogger Logger
        {
            get
            {
                return Kii.logger;
            }

            set
            {
                Kii.logger = value;
            }
        }
        #endregion
        #region Execution
        private static void ExecListTopics(KiiHttpClientFactory factory, string paginationKey, KiiGenericsCallback<KiiListResult<KiiTopic>> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiGenericsCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }
            String url = Utils.Path(Kii.BaseUrl , "apps", Kii.AppId, "topics");
            if (!String.IsNullOrEmpty(paginationKey))
            {
                url = url + "?paginationKey=" + Uri.EscapeUriString(paginationKey);
            }
            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);
            client.SendRequest((ApiResponse response, Exception e) => {
                if (e != null)
                {
                    callback(null, e);
                    return;
                }
                JsonObject json = new JsonObject(response.Body);
                String newPaginationKey = json.OptString("paginationKey", null);
                JsonArray array = json.GetJsonArray("topics");
                List<KiiTopic> topics = new List<KiiTopic>();
                for (int i = 0; i < array.Length(); i++)
                {
                    topics.Add(Kii.Topic(array.GetJsonObject(i).GetString("topicID")));
                }
                callback(new KiiListResult<KiiTopic>(topics, newPaginationKey), null);
            });
        }
        #endregion
	}
}
