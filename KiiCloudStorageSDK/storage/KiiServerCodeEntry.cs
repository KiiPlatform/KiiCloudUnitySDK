// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Text.RegularExpressions;

using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents a server side code entry in KiiCloud.
    /// </summary>
    /// <remarks>
    /// To make this instance, please call <see cref = 'Kii.ServerCodeEntry(string)'/>.
    /// </remarks>
    public class KiiServerCodeEntry
    {
        internal const string VERSION_CURRENT = "current";
        private const string SERVER_CODE_ENTRY_PATTERN_REX = "^[a-zA-Z][_a-zA-Z0-9]*$";
        private string entryName;
        private string version;
        private KiiServerCodeEnvironmentVersion? environmentVersion;

        internal KiiServerCodeEntry(string entryName, string version, KiiServerCodeEnvironmentVersion? environmentVersion) {
            this.entryName = entryName;
            this.version = version;
            this.environmentVersion = environmentVersion;
        }

        #region Blocking APIs

        /// <summary>
        /// Execute this server code entry and wait for completion.
        /// </summary>
        /// <remarks>
        /// This api access to server. Should not be executed in UI/Main thread.
        /// </remarks>
        /// <returns>
        /// Result of execution includes 
        /// JsonObject returned from specified server code entry in KiiCloud.
        /// </returns>
        /// <param name="argument">The argument that will be passed to the entry of script in the cloud. 
        /// If null is specified, no argument will be passed to the script.
        /// </param>
        /// 
        /// <exception cref="BadRequestException">
        /// Is thrown when an execution is failed.
        /// </exception>
        public KiiServerCodeExecResult Execute(KiiServerCodeEntryArgument argument)
        {
            KiiServerCodeExecResult result = null;
            ExecExecute(argument, Kii.HttpClientFactory, 
                        (KiiServerCodeEntry entry, KiiServerCodeEntryArgument outArgs, KiiServerCodeExecResult outResult, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                result = outResult;
            });
            return result;
        }

        #endregion

        #region Async APIs

        /// <summary>
        /// Asynchronous version of <see cref = 'Execute(KiiServerCodeEntryArgument)'/>.
        /// </summary>
        /// <remarks>
        /// This API will be executed on background thread.
        /// </remarks>
        /// <param name="argument">
        /// The argument that will be passed to the entry of script in the cloud. 
        /// If null is specified, no argument will be passed to the script.
        /// </param>
        /// <param name="callback">
        /// Callback.
        /// </param>
        public void Execute(KiiServerCodeEntryArgument argument, KiiServerCodeEntryCallback callback)
        {
            ExecExecute(argument, Kii.AsyncHttpClientFactory, callback);
        }

        #endregion

        #region Execution

        private void ExecExecute(KiiServerCodeEntryArgument argument, KiiHttpClientFactory factory, KiiServerCodeEntryCallback callback)
        {
            if (argument == null)
            {
                argument = KiiServerCodeEntryArgument.NewArgument(new JsonObject());
            }
            string url = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId,
                                    "server-code", "versions", this.version, this.entryName);

            if (this.environmentVersion.HasValue)
            {
                url += "?environment-version=" + this.environmentVersion.Value.GetValue();
            }

            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer(client);
            client.ContentType = "application/json";

            client.SendRequest(argument.ToJson().ToString(), (ApiResponse response, Exception e) => 
            {
                if (callback == null)
                {
                    return;
                }
                if (e != null)
                {
                    callback(this, argument, null, e);
                    return;
                }
                // parse X-step-count
                int steps; 
                try
                {
                    steps = int.Parse(response.GetHeader("X-Step-count"));
                } 
                catch
                {
                    steps = 0;
                }
                // X-Environment-version
                KiiServerCodeEnvironmentVersion? environmentVersion = KiiServerCodeEnvironmentVersionExtensions.FromValue(response.GetHeader("X-Environment-version"));
                // parse body
                JsonObject resultBody;
                try
                {
                    resultBody = new JsonObject(response.Body);
                } 
                catch (JsonException e2)
                {
                    callback(this, argument, null, new IllegalKiiBaseObjectFormatException(e2.Message)); 
                    return;
                }

                callback(this, argument, new KiiServerCodeExecResult(resultBody, steps, environmentVersion), null);
            });
        }
        #endregion

        #region internal static methods

        internal static bool IsValidEntryName(String name)
        {
            return Regex.IsMatch(name, SERVER_CODE_ENTRY_PATTERN_REX);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the entry.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>The name of the entry.</value>
        public string EntryName
        {
            get
            {
                return entryName;
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>The version.</value>
        internal string Version
        {
            get
            {
                return version;
            }
        }

        internal KiiServerCodeEnvironmentVersion? EnvironmentVersion
        {
            get
            {
                return environmentVersion;
            }
        }
        #endregion

    }
}

