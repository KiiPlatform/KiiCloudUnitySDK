using System;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Analytics;
using JsonOrg;

namespace KiiCorp.Cloud.ABTesting
{
    /// <summary>
    /// Represents values used for testing (value of A and B)
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class Variation
    {

        private string mName;
        private int mPercentage;
        private JsonObject mVariableSet;
        private string mParentExperimentID;
        private int mParentExperimentVersion;
        private ConversionEvent[] mParentExperimentConversionEvents;
        private KiiExperimentStatus mParentExperimentStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.ABTesting.Variation"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        internal Variation(KiiExperiment parentExperiment, string name, 
                            int percentage, JsonObject variableSet)
        {
            this.mParentExperimentID = parentExperiment.ID;
            this.mParentExperimentVersion = parentExperiment.Version;
            this.mParentExperimentConversionEvents = parentExperiment.ConversionEvents;
            mParentExperimentStatus = parentExperiment.Status;
            this.mName = name;
            this.mPercentage = percentage;
            this.mVariableSet = variableSet;
        }
        /// <summary>
        /// You will generate event when the specified condition in the <see cref="KiiCorp.Cloud.ABTesting.KiiExperiment"/> has achieved.
        /// ex.) User has signed up, view the message, purchase item, etc.
        /// You need to call <see cref="KiiAnalytics.Upload(KiiEvent)"/> method of KiiAnalyticsSDK to send the event to Kii Analytics Cloud.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>KiiEvent represents event.</returns>
        /// <param name="conversionEvent">ConversionEvent to specify which conversionEvent has achieved.</param>
        public KiiEvent EventForConversion(ConversionEvent conversionEvent)
        {
            if (conversionEvent == null)
            {
                throw new ArgumentException("conversionEvent is null");
            }
            if (mParentExperimentStatus != KiiExperimentStatus.RUNNING)
            {
                return new KiiEvent.NullKiiEvent();
            }
            KiiEvent e = KiiAnalytics.NewEvent (this.mParentExperimentID);
            e ["variationName"] = this.mName;
            e ["conversionEvent"] = conversionEvent.Name;
            e ["version"] = this.mParentExperimentVersion;
            return e;
        }
        /// <summary>
        /// You will generate event when the specified condition in the <see cref="KiiCorp.Cloud.ABTesting.KiiExperiment"/> has achieved.
        /// ex.) User has signed up, view the message, purchase item, etc.
        /// You need to call <see cref="KiiAnalytics.Upload(KiiEvent)"/> method of KiiAnalyticsSDK to send the event to Kii Analytics Cloud.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>KiiEvent represents event.</returns>
        /// <param name="conversionEventName">Name of the conversion event.</param>
        public KiiEvent EventForConversion(string conversionEventName)
        {
            ConversionEvent conversionEvent = ConversionEvent.GetConversionEventByName(conversionEventName, this.mParentExperimentConversionEvents);
            return this.EventForConversion(conversionEvent);
        }
        /// <summary>
        /// You will generate event when the specified condition in the <see cref="KiiCorp.Cloud.ABTesting.KiiExperiment"/> has achieved.
        /// ex.) User has signed up, view the message, purchase item, etc.
        /// You need to call <see cref="KiiAnalytics.Upload(KiiEvent)"/> method of KiiAnalyticsSDK to send the event to Kii Analytics Cloud.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>KiiEvent represents event.</returns>
        /// <param name="conversionEventIndex">Index of conversionEvents array retrieved by <see cref="KiiExperiment.ConversionEvents"/></param>
        public KiiEvent EventForConversion(int conversionEventIndex)
        {
            ConversionEvent conversionEvent = this.mParentExperimentConversionEvents[conversionEventIndex];
            return this.EventForConversion(conversionEvent);
        }
        #region properties
        /// <summary>
        /// Gets the name of variation.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>Name of variation.</value>
        public string Name
        {
            get
            {
                return this.mName;
            }
        }
        /// <summary>
        /// Gets the percentage of variation to be applied.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>Percentage of variation.</value>
        public int Percentage
        {
            get
            {
                return this.mPercentage;
            }
        }
        /// <summary>
        /// Gets the variable set of variation defined by you on the developer portal.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>Variable set of variation.</value>
        public JsonObject VariableSet
        {
            get
            {
                return this.mVariableSet;
            }
        }
        #endregion
    }
}

