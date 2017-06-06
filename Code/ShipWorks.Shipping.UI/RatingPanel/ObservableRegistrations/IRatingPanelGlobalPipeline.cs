using System;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.UI.RatingPanel.ObservableRegistrations
{
    /// <summary>
    /// Register an observable pipeline for the shipping panel view model
    /// </summary>
    [Service]
    public interface IRatingPanelGlobalPipeline
    {
        /// <summary>
        /// Register the pipeline for the view model
        /// </summary>
        IDisposable Register(RatingPanelViewModel viewModel);
    }
}
