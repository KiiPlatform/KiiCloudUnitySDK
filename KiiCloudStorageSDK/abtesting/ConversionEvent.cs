using System;

namespace KiiCorp.Cloud.ABTesting
{
    /// <summary>
    /// Definition of the conversion event.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class ConversionEvent
    {
        private string mName;

        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.ABTesting.ConversionEvent"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="name">Name.</param>
        public ConversionEvent (string name)
        {
            this.mName = name;
        }
        /// <summary>
        /// Get the conversion event by name from the conversion events array.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Conversion conversion event which have specified name.Null if there is no conversion.</returns>
        /// <param name="name">Name of the conversion event.</param>
        /// <param name="conversionEvents">ConversionEvents conversion events array.</param>
        public static ConversionEvent GetConversionEventByName(string name, ConversionEvent[] conversionEvents)
        {
            foreach (ConversionEvent conversionEvent in conversionEvents)
            {
                if (name == conversionEvent.Name)
                {
                    return conversionEvent;
                }
            }
            return null;
        }
        #region properties
        /// <summary>
        /// Get the name of the conversion event.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>Name of the conversion event.</value>
        public string Name
        {
            get
            {
                return this.mName;
            }
        }
        #endregion

    }
}

