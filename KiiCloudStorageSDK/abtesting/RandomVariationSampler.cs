using System;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;

namespace KiiCorp.Cloud.ABTesting
{
    /// <summary>
    /// Sampler using Time based sampler. 
    /// This does not require user login. Random seed will generated based on execution timestamp.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class RandomVariationSampler : VariationSampler
    {
        private readonly Random mRandomGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.ABTesting.RandomVariationSampler"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public RandomVariationSampler()
        {
            this.mRandomGenerator = new Random(
                    Convert.ToInt32(DateTime.UtcNow.ToFileTimeUtc() %
                        Convert.ToInt64(Int32.MaxValue)));
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
            switch (experiment.Status) 
            {
                case KiiExperimentStatus.RUNNING:
                    int totalPercentage = 0;
                    foreach (Variation v in experiment.Variations)
                    {
                        totalPercentage += v.Percentage;
                    }
                    int random = 0;
                    lock (this)
                    {
                        random = this.mRandomGenerator.Next(totalPercentage);
                    }
                    int accumulated = 0;
                    foreach (Variation v in experiment.Variations)
                    {
                        accumulated += v.Percentage;
                        if (random < accumulated)
                        {
                            return v;
                        }
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

