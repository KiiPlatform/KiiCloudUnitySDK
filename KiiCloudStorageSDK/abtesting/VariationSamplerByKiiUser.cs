using System;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;

namespace KiiCorp.Cloud.ABTesting
{
    /// <summary>
    /// Sampler using KiiUser attributes.
    /// This uses current login KiiUser ID to randomise the variation.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class VariationSamplerByKiiUser : VariationSampler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.ABTesting.VariationSamplerByKiiUser"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public VariationSamplerByKiiUser ()
        {
        }
        /// <summary>
        /// Do sampling. Returns random variation based on the percentage configured in developer portal.
        /// If sampling is failed, returns the fallback.
        /// If the experiment has terminated and fixed variation has chosen, returns chosen variation.
        /// Returned variation is same as <see cref="KiiExperiment.ChosenVariation"/>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Applied variation for this time.</returns>
        /// <param name="experiment">that requires sampling.</param>
        /// <param name="fallback">The variation to return when failed to get the applied variation.</param>
        public Variation ChooseVariation(KiiExperiment experiment, Variation fallback)
        {
            KiiUser user = KiiUser.CurrentUser;
            if (user == null)
            {
                throw new InvalidOperationException(ErrorInfo.UTILS_NO_LOGIN);
            }
            switch (experiment.Status) 
            {
                case KiiExperimentStatus.RUNNING:
                    int totalPercentage = 0;
                    foreach (Variation v in experiment.Variations)
                    {
                        totalPercentage += v.Percentage;
                    }
                    int random = new Random(user.Uri.GetHashCode()).Next(totalPercentage);
                    int i = 0;
                    int accumulated = 0;
                    foreach (Variation v in experiment.Variations)
                    {
                        if (random < accumulated + v.Percentage)
                        {
                            return v;
                        }
                        accumulated += v.Percentage;
                        i++;
                    }
                    throw new InvalidOperationException("Unexpected error.");
                case KiiExperimentStatus.DRAFT:
                    return fallback;
                case KiiExperimentStatus.PAUSED:
                    return fallback;
                case KiiExperimentStatus.TERMINATED:
                    if (experiment.ChosenVariation != null)
                    {
                        return experiment.ChosenVariation;
                    }
                    return fallback;
                default:
                    throw new InvalidOperationException("Unknown status!");
            }
        }
    }
}

