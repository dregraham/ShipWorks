using System;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Pipeline interface for registering observables in the MainGridControl
    /// </summary>
    [Service]
    public interface IMainGridControlPipeline
    {
        /// <summary>
        /// Register the pipeline with the main grid control
        /// </summary>
        IDisposable Register(MainGridControl mainGridControl);
    }
}
