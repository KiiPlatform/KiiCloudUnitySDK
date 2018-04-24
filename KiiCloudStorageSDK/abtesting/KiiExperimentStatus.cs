using System;

namespace KiiCorp.Cloud.ABTesting
{
    /// <summary>
    /// Represents the status of Experiment.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public enum KiiExperimentStatus
    {
        /// <summary>
        /// The experiment is draft. You need to activate Experiment in the
        /// developer portal before starting A/B testing.
        /// </summary>
        DRAFT,
        /// <summary>
        /// The experiment is running.
        /// </summary>
        RUNNING,
        /// <summary>
        /// The experiment is paused. You can restart Experiment in the developer portal.
        /// </summary>
        PAUSED,
        /// <summary>
        /// The experiment is terminated.
        /// </summary>
        TERMINATED
    }
}

