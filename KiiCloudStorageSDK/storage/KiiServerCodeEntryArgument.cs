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
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Argument passed to the KiiServerCodeEntry.
    /// </summary>
    /// <remarks>
    /// To make this instance, please call <see cref = 'NewArgument(JsonOrg.JsonObject)'/>.
    /// </remarks>
    public class KiiServerCodeEntryArgument
    {
        private JsonObject args;

        internal KiiServerCodeEntryArgument (JsonObject args)
        {
            this.args = args;
        }

        internal JsonObject ToJson()
        {
            return this.args;
        }

        /// <summary>
        /// Instantiate new argument passed to the KiiServerCodeEntry.
        /// </summary>
        /// <remarks>
        /// Argument must not be null.
        /// </remarks>
        /// <returns>
        /// The instance of KiiServerCodeEntryArgument
        /// </returns>
        /// <param name="args">
        /// Arguments that will be passed to entry script in Kii Cloud.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when args is null.
        /// </exception>
        public static KiiServerCodeEntryArgument NewArgument(JsonObject args)
        {
            if (args == null) 
            {
                throw new ArgumentNullException("args must not be null.");
            }
            return new KiiServerCodeEntryArgument(args);
        }
    }
}
