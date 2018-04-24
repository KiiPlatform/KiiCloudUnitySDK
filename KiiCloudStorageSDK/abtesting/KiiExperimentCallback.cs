using System;

namespace KiiCorp.Cloud.ABTesting
{
    /// <summary>
    /// Called after the completion of <see cref="KiiExperiment.GetByID(String,KiiExperimentCallback)"/>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="experiment">Retrieved experiment</param>
    /// <param name="e">Null if succeeded</param>
    public delegate void KiiExperimentCallback(KiiExperiment experiment, Exception e);
}

