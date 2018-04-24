using System;
using System.Text;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides APIs of push installation.
    /// </summary>
    /// <remarks>
    /// To get this instance, please use KiiUser.PushInstallation property.
    /// </remarks>
    public class KiiPushInstallation
    {
        private bool development;

        internal KiiPushInstallation () : this(false)
        {
        }
        internal KiiPushInstallation (bool development)
        {
            this.development = development;
        }

        /// <summary>
        /// Type of device
        /// </summary>
        /// <remarks>
        /// </remarks>
        public enum DeviceType
        {
            /// <summary>
            /// iOS
            /// </summary>
            IOS,
            /// <summary>
            /// Android
            /// </summary>
            ANDROID
        }

        #region Blocking APIs
        /// <summary>
        /// Install the deviceID of specified platform to KiiCloud.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name='deviceId'>
        /// iOS : Device Token, Android : Registration ID
        /// </param>
        /// <param name='deviceType'>
        /// IOS or Android
        /// </param>
        /// <exception cref="ArgumentException">
        /// Is thrown when deviceId is null or empty.
        /// </exception>
        /// <exception cref="CloudException">
        /// Is thrown when server sends error response.
        /// </exception>
        public void Install(string deviceId, DeviceType deviceType)
        {
            this.ExecInstall (deviceId, deviceType, Kii.HttpClientFactory, (Exception e)=> {
                if (e != null)
                {
                    throw e;
                }
            });
        }
        /// <summary>
        /// Uninstall the deviceID of specified platform to KiiCloud.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name='deviceId'>
        /// iOS : Device Token, Android : Registration ID
        /// </param>
        /// <param name='deviceType'>
        /// IOS or Android
        /// </param>
        /// <exception cref="ArgumentException">
        /// Is thrown when deviceId is null or empty.
        /// </exception>
        /// <exception cref="CloudException">
        /// Is thrown when server sends error response.
        /// </exception>
        public void Uninstall(string deviceId, DeviceType deviceType)
        {
            this.ExecUninstall (deviceId, deviceType, Kii.HttpClientFactory, (Exception e)=> {
                if (e != null)
                {
                    throw e;
                }
            });
        }
        #endregion

        #region Async APIs
        /// <summary>
        /// Install the deviceID of specified platform to KiiCloud.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name='deviceId'>
        /// iOS : Device Token, Android : Registration ID
        /// </param>
        /// <param name='deviceType'>
        /// IOS or Android
        /// </param>
        /// <param name='callback'>
        /// Callback delegate. If exception is null, execution is succeeded.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when callback is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when deviceId is null or empty.
        /// </exception>
        /// <exception cref="CloudException">
        /// Is thrown when server sends error response.
        /// </exception>
        public void Install(string deviceId, DeviceType deviceType, KiiPushInstallationCallback callback)
        {
            this.ExecInstall (deviceId, deviceType, Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Uninstall the deviceID of specified platform to KiiCloud.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name='deviceId'>
        /// iOS : Device Token, Android : Registration ID
        /// </param>
        /// <param name='deviceType'>
        /// IOS or Android
        /// </param>
        /// <param name='callback'>
        /// Callback delegate. If exception is null, execution is succeeded.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when callback is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Is thrown when deviceId is null or empty.
        /// </exception>
        /// <exception cref="CloudException">
        /// Is thrown when server sends error response.
        /// </exception>
        public void Uninstall(string deviceId, DeviceType deviceType, KiiPushInstallationCallback callback)
        {
            this.ExecUninstall (deviceId, deviceType, Kii.AsyncHttpClientFactory, callback);
        }
        #endregion

        #region Execution
        private void ExecInstall(string deviceID, DeviceType deviceType, KiiHttpClientFactory factory, KiiPushInstallationCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiPushInstallationCallback must not be null");
            }
            if (Utils.IsEmpty(deviceID))
            {
                callback(new ArgumentException(ErrorInfo.KIIPUSHINSTALLATION_DEVICE_ID_NULL));
                return;
            }
            Utils.CheckInitialize(true);
            string url = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "installations");
            JsonObject requestBody = new JsonObject();
            requestBody.Put("installationRegistrationID", deviceID);
            requestBody.Put("deviceType", Enum.GetName (typeof(DeviceType), deviceType));
            if (this.development)
            {
                requestBody.Put("development", true);
            }
            
            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer(client);
            client.ContentType = "application/vnd.kii.InstallationCreationRequest+json";
            
            client.SendRequest (requestBody.ToString(), (ApiResponse response, Exception e) => {
                callback(e);
            });
        }
        private void ExecUninstall(string deviceID, DeviceType deviceType, KiiHttpClientFactory factory, KiiPushInstallationCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiPushInstallationCallback must not be null");
            }
            if (Utils.IsEmpty(deviceID))
            {
                callback(new ArgumentException(ErrorInfo.KIIPUSHINSTALLATION_DEVICE_ID_NULL));
                return;
            }
            Utils.CheckInitialize(true);
            string url = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "installations", Enum.GetName (typeof(DeviceType), deviceType) + ":" + deviceID);
            
            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.DELETE);
            KiiCloudEngine.SetAuthBearer(client);

            client.SendRequest ((ApiResponse response, Exception e) => {
                callback(e);
            });
        }
        #endregion
    }
}

