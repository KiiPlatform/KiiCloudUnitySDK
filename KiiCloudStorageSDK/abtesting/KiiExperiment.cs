using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using JsonOrg;
using KiiCorp.Cloud.Storage;

namespace KiiCorp.Cloud.ABTesting
{
    /// <summary>
    /// Represents Experiments.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class KiiExperiment
    {
        private static Regex EXP_ID_PATTERN = new Regex("^[a-zA-Z0-9-_\\.]{2,100}$");
        private static Type KIIEXPERIMENTSTATUS = typeof(KiiExperimentStatus);

        private int mVersion;
        private String mId;
        private Variation[] mVariations;
        private ConversionEvent[] mConversionEvents;
        private String mDescription;
        private KiiExperimentStatus mStatus;
        private Variation mChosenVariation;

        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.ABTesting.KiiExperiment"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        internal KiiExperiment ()
        {
        }
        /// <summary>
        /// Get the experiment which has the specified id.
        /// </summary>
        /// <remarks>
        /// NOTE: This api access to server. Should not be executed in UI/Main thread.
        /// </remarks>
        /// <returns>Experiment which has the specified id.</returns>
        /// <param name="experimentID">Experiment id.</param>
        public static KiiExperiment GetByID(String experimentID)
        {
            if (!IsValidExperimentID(experimentID))
            {
                throw new ArgumentException("Experiment id is invalid");
            }
            Utils.CheckInitialize(false);
            string getUrl = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "buckets", "_experiments", "objects", experimentID);
            KiiHttpClient client = Kii.HttpClientFactory.Create(getUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);
            ApiResponse response = client.SendRequest();

            return CreateFromResponse(response);
        }
        /// <summary>
        /// Get the experiment having the specified id in background.
        /// </summary>
        /// <remarks>
        /// NOTE: This api access to server. Should not be executed in UI/Main thread.
        /// </remarks>
        /// <returns>Experiment which has the specified id.</returns>
        /// <param name="experimentID">Experiment id.</param>
        /// <param name="callback">Called on completion of get experiment.</param>
        public static void GetByID(String experimentID, KiiExperimentCallback callback)
        {
            if (!IsValidExperimentID(experimentID))
            {
                callback(null, new ArgumentException("Experiment id is invalid"));
                return;
            }
            Utils.CheckInitialize(false);
            string getUrl = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "buckets", "_experiments", "objects", experimentID);
            KiiHttpClient client = Kii.AsyncHttpClientFactory.Create(getUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(null, e);
                    return;
                }
                else
                {
                    try
                    {
                        KiiExperiment kiiExperiment = CreateFromResponse(response);
                        callback(kiiExperiment, null);
                    }
                    catch (Exception ex)
                    {
                        callback(null, ex);
                    }
                }
            });
        }
        /// <summary>
        /// Get the variation having the specified name.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>variation that match the specified name. Returns null if the experiment has no variation with the specified name.</returns>
        /// <param name="name">Variation name.</param>
        public Variation GetVariationByName(String name)
        {
            foreach (Variation v in this.mVariations)
            {
                if (v.Name == name)
                {
                    return v;
                }
            }
            return null;
        }
        /// <summary>
        /// Get the Conversion Event by its name.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Conversion Event or null if Experiment does not have conversion having specified name.</returns>
        /// <param name="name">Name of the Conversion Event</param>
        public ConversionEvent GetConversionEventByName(String name)
        {
            return ConversionEvent.GetConversionEventByName(name, this.mConversionEvents);
        }
        /// <summary>
        /// Get the variation applied to this trial.
        /// Variation will be determined by specified rate of each variation in this experiment.
        /// Current login user information will be used for sampling.
        /// If the experiment has terminated with specified variant, the specified variant will be returned regardless of login user information.
        /// If the experiment has finished without specified variant, fallback will be returned.
        /// If the status of experiment is draft or paused, fallback will be returned.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Applied variation for this trial.</returns>
        /// <param name="fallback">The variation to return when failed to get the applied variation. If you want to detect error, you need to set null.</param>
        public Variation GetAppliedVariation(Variation fallback)
        {
            if (KiiUser.CurrentUser == null)
            {
                return fallback;
            }
            return this.GetAppliedVariation(fallback, new VariationSamplerByKiiUser());

        }
        /// <summary>
        /// Get the variation applied to this trial.
        /// Sampler should return the variation according to the rate defined in this experiment.
        /// If you use <see cref="VariationSamplerByKiiUser"/> with current login user, It will be same as <see cref="KiiExperiment.GetAppliedVariation(Variation)"/>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Applied variation for this trial.</returns>
        /// <param name="fallback">The variation to return when failed to get the applied variation. If you want to detect error, you need to set null.</param>
        /// <param name="sampler">Variation sampler.</param>
        public Variation GetAppliedVariation(Variation fallback, VariationSampler sampler)
        {
            if (sampler == null)
            {
                return fallback;
            }
            try
            {
                return sampler.ChooseVariation(this, fallback);
            }
            catch
            {
                return fallback;
            }
        }
        private static bool IsValidExperimentID(string experimentID)
        {
            return EXP_ID_PATTERN.IsMatch(experimentID);
        }
        /// <summary>
        /// Creates instance of KiiExperiment from ApiResponse.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>The instance of KiiExperiment.</returns>
        /// <param name="response">API response.</param>
        private static KiiExperiment CreateFromResponse(ApiResponse response)
        {
            KiiExperiment experiment = null;
            try
            {
                JsonObject json = new JsonObject(response.Body);
                experiment = new KiiExperiment();
                experiment.mId = json.GetString("_id");
                experiment.mDescription = json.OptString("description", null);
                experiment.mVersion = json.GetInt("version");
                experiment.mStatus = ParseStatus(json);
                // parse conversionEvents
                JsonArray conversionEventsJson = json.GetJsonArray("conversionEvents");
                List<ConversionEvent> conversionEventList = new List<ConversionEvent>();
                for (int i = 0; i < conversionEventsJson.Length(); i++)
                {
                    JsonObject conversionEventJson = conversionEventsJson.GetJsonObject(i);
                    conversionEventList.Add(new ConversionEvent(conversionEventJson.GetString("name")));
                }
                experiment.mConversionEvents = conversionEventList.ToArray();
                // parse variations
                JsonArray variationsJson = json.GetJsonArray("variations");
                List<Variation> variationList = new List<Variation>();
                for (int i = 0; i < variationsJson.Length(); i++)
                {
                    JsonObject variationJson = variationsJson.GetJsonObject(i);
                    string varName = variationJson.GetString("name");
                    int percentage = variationJson.GetInt("percentage");
                    JsonObject test = variationJson.GetJsonObject("variableSet");
                    variationList.Add(new Variation(experiment, varName, percentage, test));
                }
                experiment.mVariations = variationList.ToArray();
                string chosenVariationName = json.OptString("chosenVariation", null);
                if (!Utils.IsEmpty(chosenVariationName))
                {
                    experiment.mChosenVariation = experiment.GetVariationByName(chosenVariationName);
                }
                return experiment;
            }
            catch (Exception e)
            {
                throw new IllegalKiiBaseObjectFormatException(e.Message);
            }
        }

        /// <summary>
        /// Parse status field in json
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>KiiExperimentStatus enumeration. if fails to
        /// parse, KiiExperimentStatus.PAUSED is returned.
        /// </returns>
        /// <param name="json">JSON object.</param>
        private static KiiExperimentStatus ParseStatus(JsonObject json)
        {
            Debug.Assert(json != null, "json must not be nil.");

            // Default value is KiiExperimentStatus.PAUSED.
            int status = json.OptInt("status", (int)KiiExperimentStatus.PAUSED);

            // If unkown value is received, return default value.
            if (!Enum.IsDefined(KIIEXPERIMENTSTATUS, status))
            {
                return KiiExperimentStatus.PAUSED;
            }
            return (KiiExperimentStatus)Enum.ToObject(KIIEXPERIMENTSTATUS,
                    status);
        }

        #region properties
        /// <summary>
        /// Gets the version of the Experiment.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>Version of the Experiment.</value>
        public int Version
        {
            get
            {
                return this.mVersion;
            }
        }
        /// <summary>
        /// Gets the ID of the Experiment.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>ID of the Experiment.</value>
        public string ID
        {
            get
            {
                return this.mId;
            }
        }
        /// <summary>
        /// Gets conversion events
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>Conversion events.</value>
        public ConversionEvent[] ConversionEvents
        {
            get
            {
                return this.mConversionEvents;
            }
        }
        /// <summary>
        /// Gets the description of the experiment.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>The description of the experiment.</value>
        public string Description
        {
            get
            {
                return this.mDescription;
            }
        }
        /// <summary>
        /// Gets the status of the experiment.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>Current status of experiment.</value>
        public KiiExperimentStatus Status
        {
            get
            {
                return this.mStatus;
            }
        }
        /// <summary>
        /// Gets the variations associated with this experiment
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>An array of the variation.</value>
        public Variation[] Variations
        {
            get
            {
                return this.mVariations;
            }
        }
        /// <summary>
        /// Gets chosen variation if the experiment has finished with specified variation.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>Variation chosen or null if when the no variation has not been chosen.</value>
        public Variation ChosenVariation
        {
            get
            {
                return this.mChosenVariation;
            }
        }
        #endregion

    }
}

