using System;

namespace KiiCorp.Cloud.ABTesting
{
    /// <summary>
    /// Interface of sampler of variation. Used by <see cref="KiiExperiment.GetAppliedVariation(Variation,VariationSampler)"/>
    /// You can implement customized logic of sampling variation by implementing this interface.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public interface VariationSampler
    {
        /// <summary>
        /// Called when sampling needed.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Applied variation for this time.</returns>
        /// <param name="experiment">That requires sampling.</param>
        /// <param name="fallback">The variation to return when failed to get the applied variation.</param>
        Variation ChooseVariation(KiiExperiment experiment, Variation fallback);
    }
}

