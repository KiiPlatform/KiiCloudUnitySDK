using System;
using System.Collections.Generic;

using JsonOrg;
using KiiCorp.Cloud.Storage;

namespace KiiCorp.Cloud.Analytics
{
    /// <summary>
    /// Provides API of Kii analytics.
    /// </summary>
    /// <remarks>
    /// Please call Initialize first.
    /// </remarks>
    public class KiiAnalytics
    {
        /// <summary>
        /// Represents Site
        /// </summary>
        /// <remarks></remarks>
        public enum Site
        {
            /// <summary>
            /// Use KiiCloud in Japan.
            /// </summary>
            JP,

            /// <summary>
            /// Use KiiCloud in United States.
            /// </summary>
            US,

            /// <summary>
            /// Use KiiCloud in China.
            /// </summary>
            CN,

            /// <summary>
            /// Use KiiCloud in Singapore.
            /// </summary>
            SG,

            /// <summary>
            /// Use KiiCloud in cn3 site of China.
            /// </summary>
            CN3,

            /// <summary>
            /// Use cloud in European Union.
            /// </summary>
            EU,
        }

        private static KiiAnalytics INSTANCE = null;
        private const string SERVER_URL_JP = "https://api-jp.kii.com/api";
        private const string SERVER_URL_US = "https://api.kii.com/api";
        private const string SERVER_URL_CN = "https://api-cn2.kii.com/api";
        private const string SERVER_URL_SG = "https://api-sg.kii.com/api";
        private const string SERVER_URL_CN3 = "https://api-cn3.kii.com/api";
        private const string SERVER_URL_EU = "https://api-eu.kii.com/api";
        private string mAppID;
        private string mAppKey;
        private string mBaseUrl;
        private string mDeviceID;
        private KiiHttpClientFactory mHttpClientFactory;
        private KiiHttpClientFactory mAsyncHttpClientFactory;

        private KiiAnalytics (string appID, string appKey)
        {
            this.mAppID = appID;
            this.mAppKey = appKey;
        }

        /// <summary>
        /// Initialize the specified appId, appKey, serverUrl and deviceId.
        /// </summary>
        /// <remarks>
        /// Please call this method first.
        /// </remarks>
        /// <param name='appID'>
        /// App ID
        /// </param>
        /// <param name='appKey'>
        /// App key.
        /// </param>
        /// <param name='serverUrl'>
        /// Server URL.
        /// </param>
        /// <param name='deviceID'>
        /// Device identifier.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Is thrown when appId/appKey/serverUrl/deviceID is empty
        /// </exception>
        public static void Initialize (string appID, string appKey, string serverUrl, string deviceID)
        {
            Initialize(appID, appKey, serverUrl, deviceID, new KiiClientFactoryImpl(), new KiiAsyncClientFactoryImpl());
        }

        /// <summary>
        /// Initialize the specified appId, appKey, site and deviceID.
        /// </summary>
        /// <remarks>
        /// Please call this method first.
        /// </remarks>
        /// <param name='appID'>
        /// Application ID found in your Kii developer console.
        /// </param>
        /// <param name='appKey'>
        /// Application key found in your Kii developer console.
        /// </param>
        /// <param name='site'>
        /// Please set any one of Site.US, Site.JP, Site.CN, Site.CN3 or Site.SG you've chosen when you created application on Kii developer console.
        /// </param>
        /// <param name='deviceID'>
        /// Device ID.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Is thrown when
        /// <list type="bullet">
        /// <item><term>appId/appKey/deviceID is empty</term></item>
        /// <item><term>site is neither Site.JP nor Site.US</term></item>
        /// </list>
        /// </exception>
        public static void Initialize(string appID, string appKey, Site site, string deviceID)
        {
            Initialize(appID, appKey, GetBaseUrl(site), deviceID);
        }
        /// <summary>
        /// Initialize KiiSDK appID, appKey, deviceID, site and KiiHttpClientFactory.
        /// </summary>
        /// <remarks>
        /// This method is intended for use in internal purposes. Do not use it to initialize your application.
        /// </remarks>
        /// <param name='appID'>
        /// Application ID found in your Kii developer console.
        /// </param>
        /// <param name='appKey'>
        /// Application key found in your Kii developer console.
        /// </param>
        /// <param name='site'>
        /// Please set any one of Site.US, Site.JP, Site.CN, Site.CN3 or Site.SG you've chosen when you created application on Kii developer console.
        /// </param>
        /// <param name='deviceID'>
        /// DeviceID.
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
        public static void Initialize(string appID, string appKey, Site site, string deviceID, KiiHttpClientFactory syncFactory, KiiHttpClientFactory asyncFactory)
        {
            Initialize(appID, appKey, GetBaseUrl(site), deviceID, syncFactory, asyncFactory);
        }
        /// <summary>
        /// Initialize KiiSDK appID, appKey, deviceID, serverUrl and KiiHttpClientFactory.
        /// </summary>
        /// <remarks>
        /// This method is intended for use in internal purposes. Do not use it to initialize your application.
        /// </remarks>
        /// <param name='appID'>
        /// Application ID found in your Kii developer console.
        /// </param>
        /// <param name='appKey'>
        /// Application key found in your Kii developer console.
        /// </param>
        /// <param name='serverUrl'>
        /// Server URL.
        /// </param>
        /// <param name='deviceID'>
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
        public static void Initialize(string appID, string appKey, string serverUrl, string deviceID, KiiHttpClientFactory syncFactory, KiiHttpClientFactory asyncFactory)
        {
            if (Utils.IsEmpty(appID) || Utils.IsEmpty(appKey) || Utils.IsEmpty(serverUrl) || Utils.IsEmpty(deviceID))
            {
                throw new ArgumentException(ErrorInfo.KIIANALYTICS_APPINFO_NULL);
            }
            if (!Uri.IsWellFormedUriString(serverUrl, UriKind.Absolute))
            {
                throw new ArgumentException(ErrorInfo.KIIANALYTICS_INVALID_SERVER_URL);
            }
            if (INSTANCE != null && KiiAnalytics.AppID == appID && KiiAnalytics.AppKey == appKey && KiiAnalytics.BaseUrl == serverUrl)
            {
                return;
            }
            INSTANCE = new KiiAnalytics(appID, appKey);
            INSTANCE.mBaseUrl = serverUrl;
            INSTANCE.mDeviceID = deviceID;
            INSTANCE.mHttpClientFactory = syncFactory;
            INSTANCE.mAsyncHttpClientFactory = asyncFactory;
        }
        /// <summary>
        /// Instantiate new event.
        /// </summary>
        /// <remarks>
        /// After setting key-value pairs, developer must pass this instance to <see cref="Upload(KiiEvent[])"/>
        /// </remarks>
        /// <returns>
        /// KiiEvent instance.
        /// </returns>
        /// <param name='type'>
        /// Event type. Must be matched with &quot;^\\S.{0,127}&quot;
        /// </param>
        /// <exception cref="ArgumentException">
        /// Is thrown when type is invalid.
        /// </exception>
        public static KiiEvent NewEvent(string type)
        {
            return new KiiEvent(type);
        }

        /// <summary>
        /// Upload events to KiiCloud.
        /// </summary>
        /// <remarks>
        /// This API will send a request to KiiCloud.
        /// </remarks>
        /// <param name='eventList'>
        /// Event list.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Is thrown when
        /// <list type="bullet">
        /// <item><term>argument is null.</term></item>
        /// <item><term>argument is alredy sent to KiiCloud.</term></item>
        /// <item><term>argument has more than 50 KiiEvent</term></item>
        /// </list>
        /// </exception>
        /// <exception cref="EventUploadException">
        /// Is thrown when event uploading is failed.
        /// </exception>
        /// <exception cref="NetworkException">
        /// Is thrown when network related exception occurs.
        /// </exception>
        public static void Upload(params KiiEvent[] eventList)
        {
            if (eventList == null || eventList.Length == 0)
            {
                throw new ArgumentException(ErrorInfo.KIIANALYTICS_EVENT_NULL);
            }
            // length of eventlist must be within 50
            if (eventList.Length > 50)
            {
                throw new ArgumentException(ErrorInfo.KIIANALYTICS_EVENT_TOO_LONG);
            }

            ExecUpload(KiiAnalytics.HttpClientFactory, (Exception e) => {
                if (e != null) {
                    throw e;
                }
            }, eventList);
        }

        private static void ExecUpload (KiiHttpClientFactory httpClientFactory, KiiEventCallback callback, KiiEvent[] eventList)
        {
            JsonArray array = new JsonArray ();
            foreach (KiiEvent ev in eventList) {
                if (ev == null) {
                    throw new ArgumentException (ErrorInfo.KIIANALYTICS_EVENT_NULL);
                }
                if (ev.Sent) {
                    throw new ArgumentException (ErrorInfo.KIIANALYTICS_EVENT_ALREADY_SENT);
                }
                if (ev.GetType () == typeof(KiiEvent.NullKiiEvent)) {
                    continue;
                }
                array.Put (ev.ConvertToJsonObject (INSTANCE.mDeviceID));
            }

            string url = KiiAnalytics.BaseUrl + "/apps/" + KiiAnalytics.AppID + "/events";
            string body = array.ToString ();

            KiiHttpClient client = httpClientFactory.Create (url, KiiAnalytics.AppID, KiiAnalytics.AppKey, KiiHttpMethod.POST);
            client.ContentType = "application/vnd.kii.EventRecordList+json";
            client.Body = body;

            client.SendRequest ((ApiResponse response, Exception e) => {
                if (e != null) {
                    if (! (e is CloudException)) {
                        InvokeUploadCallback (callback, e);
                    } else {
                        CloudException exp = (CloudException)e;
                        InvokeUploadCallback(callback, new EventUploadException (exp.Status, exp.Body, eventList));
                    }
                    return;
                }
                if (response.Status == 200) {
                    try {
                        List<KiiEvent> errorEventList = ParsePartialSuccessResponse(response.Body, eventList);
                        InvokeUploadCallback(callback, new EventUploadException (response.Status, response.Body, errorEventList));
                    } catch (Exception ex) {
                        InvokeUploadCallback(callback, ex);
                    }
                    return;
                }
                foreach (KiiEvent ev in eventList) {
                    ev.Sent = true;
                }
                InvokeUploadCallback (callback, null);
            });
        }

        private static void InvokeUploadCallback (KiiEventCallback callback, Exception e)
        {
            if (callback != null) {
                callback(e);
            }
        }

        /// <summary>
        /// Asynchronously upload events to KiiCloud.
        /// </summary>
        /// <remarks>
        /// This API sends a request to KiiCloud.
        /// </remarks>
        /// <param name='callback'>
        /// KiiEventCallback.
        /// </param>
        /// <param name='eventList'>
        /// Event list to upload.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Is thrown when
        /// <list type="bullet">
        /// <item><term>eventList is null.</term></item>
        /// <item><term>Any event in the list has alredy sent to KiiCloud.</term></item>
        /// <item><term>eventList has more than 50 KiiEvent</term></item>
        /// </list>
        /// </exception>
        public static void Upload (KiiEventCallback callback, params KiiEvent[] eventList)
        {
            if (eventList == null || eventList.Length == 0) {
                throw new ArgumentException (ErrorInfo.KIIANALYTICS_EVENT_NULL);
            }
            // length of eventlist must be within 50
            if (eventList.Length > 50) {
                throw new ArgumentException (ErrorInfo.KIIANALYTICS_EVENT_TOO_LONG);
            }
            ExecUpload (KiiAnalytics.AsyncHttpClientFactory, callback, eventList);
        }

        private static List<KiiEvent> ParsePartialSuccessResponse (string body, KiiEvent[] eventList)
        {
            JsonArray errorEvents;
            try {
                JsonObject json = new JsonObject (body);
                errorEvents = json.OptJsonArray ("invalidEvents");
            } catch (JsonException) {
                throw new IllegalKiiBaseObjectFormatException ("Server response is broken.");
            }

            List<KiiEvent> errorEventList = new List<KiiEvent> ();

            // update sent flags on succeeded events
            foreach (KiiEvent e in eventList) {
                e.Sent = true;
            }

            for (int i = 0; i < errorEvents.Length(); ++i) {
                JsonObject json = errorEvents.OptJSONObject (i);
                if (json == null) {
                    continue;
                }
                int index = json.OptInt ("index", -1);
                if (index < 0 || index >= eventList.Length) {
                    continue;
                }
                errorEventList.Add (eventList [index]);
                eventList [index].Sent = false;
            }
            return errorEventList;

        }

        #region GetResult

        /// <summary>
        /// Gets the result of analytics.
        /// </summary>
        /// <remarks>
        /// This API will send a request to Kii Cloud.
        /// </remarks>
        /// <returns>
        /// The grouped result.
        /// </returns>
        /// <param name='ruleId'>
        /// Rule identifier. Must not be null.
        /// </param>
        /// <param name='condition'>
        /// Condition.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when ruleId is null or empty.
        /// </exception>
        /// <exception cref="CloudException">
        /// Is thrown when KiiCloud sends error response.
        /// </exception>
        public static GroupedResult GetResult(string ruleId, ResultCondition condition)
        {
            if (Utils.IsEmpty(ruleId))
            {
                throw new ArgumentException(ErrorInfo.KIIANALYTICS_RULE_ID_NULL);
            }
            GroupedResult result = null;
            ExecGetResult (ruleId, condition, KiiAnalytics.HttpClientFactory, (string id, ResultCondition c, GroupedResult r, Exception e) => {
                if (e != null) {
                    throw e;
                }
                result = r;
            });
            return result;
        }

        private static void ExecGetResult (string ruleId, ResultCondition condition, KiiHttpClientFactory factory, KiiResultCallback callback)
        {
            string url = KiiAnalytics.BaseUrl + "/apps/" + KiiAnalytics.AppID + "/analytics/" + ruleId + "/data";
            if (condition != null) {
                url += "?" + condition.ToQueryString ();
            }

            KiiHttpClient client = factory.Create (url, KiiAnalytics.AppID, KiiAnalytics.AppKey, KiiHttpMethod.GET);
            client.Accept = "application/vnd.kii.GroupedAnalyticResult+json";
            client.SendRequest ((ApiResponse response, Exception e) => {
                if (e != null) {
                    invokeResultCallback (callback, ruleId, condition, null, e);
                    return;
                }
                try {
                    JsonObject obj = new JsonObject (response.Body);
                    JsonArray snapshots = obj.GetJsonArray ("snapshots");
                    GroupedResult result = GroupedResult.Parse (snapshots);
                    invokeResultCallback (callback, ruleId, condition, result, null);
                } catch (JsonException) {
                    Exception ex = new IllegalKiiBaseObjectFormatException ("Server response is broken.");
                    invokeResultCallback (callback, ruleId, condition, null, ex);
                }
            });
        }

        private static void invokeResultCallback (KiiResultCallback callback, string ruleId, ResultCondition condition, GroupedResult result, Exception e)
        {
            if (callback != null) {
                callback (ruleId, condition, result, e);
            }
        }

        /// <summary>
        /// Asynchronously retrieve analytics result.
        /// </summary>
        /// <remarks>
        /// This API will send a request to Kii Cloud.
        /// </remarks>
        /// <param name='ruleId'>
        /// Rule identifier. Must not be null.
        /// </param>
        /// <param name='condition'>
        /// Condition.
        /// </param>
        /// <param name='callback'>
        /// KiiResultCallback.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when ruleId is null or empty.
        /// </exception>
        public static void GetResult (string ruleId, ResultCondition condition, KiiResultCallback callback)
        {
            if (Utils.IsEmpty(ruleId))
            {
                throw new ArgumentException(ErrorInfo.KIIANALYTICS_RULE_ID_NULL);
            }
            ExecGetResult(ruleId, condition, KiiAnalytics.AsyncHttpClientFactory, callback);
        }
        private static string GetBaseUrl(Site site)
        {
            switch (site)
            {
            case Site.JP:
                return SERVER_URL_JP;
            case Site.US:
                return SERVER_URL_US;
            case Site.CN:
                return SERVER_URL_CN;
            case Site.SG:
                return SERVER_URL_SG;
            case Site.CN3:
                return SERVER_URL_CN3;
            case Site.EU:
                return SERVER_URL_EU;
            default:
                throw new ArgumentException(ErrorInfo.KIIANALYTICS_UNKNOWN_SITE);
            }
        }
        #endregion

        #region properties
        internal static KiiAnalytics Instance
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
        internal static string AppID
        {
            get
            {
                if(INSTANCE == null)
                {
                    return null;
                }
                return INSTANCE.mAppID;
            }
        }

        internal static string AppKey
        {
            get
            {
                if(INSTANCE == null)
                {
                    return null;
                }
                return INSTANCE.mAppKey;
            }
        }
        internal static string BaseUrl
        {
            get
            {
                if(INSTANCE == null)
                {
                    return null;
                }
                return INSTANCE.mBaseUrl;
            }
        }
        internal static KiiHttpClientFactory HttpClientFactory
        {
            get
            {
                if (INSTANCE == null)
                {
                    throw new InvalidOperationException(KiiCorp.Cloud.Storage.ErrorInfo.UTILS_KIICLIENT_NULL);
                }
                return INSTANCE.mHttpClientFactory;
            }
            set
            {
                INSTANCE.mHttpClientFactory = value;
            }
        }

        internal static KiiHttpClientFactory AsyncHttpClientFactory
        {
            get
            {
                if (INSTANCE == null)
                {
                    throw new InvalidOperationException(KiiCorp.Cloud.Storage.ErrorInfo.UTILS_KIICLIENT_NULL);
                }
                return INSTANCE.mAsyncHttpClientFactory;
            }
            set
            {
                INSTANCE.mAsyncHttpClientFactory = value;
            }
        }
        #endregion

    }
}
