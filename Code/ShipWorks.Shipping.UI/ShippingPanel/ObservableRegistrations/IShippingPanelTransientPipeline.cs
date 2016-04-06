using System;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Register an observable pipeline for the shipping panel view model
    /// </summary>
    public interface IShippingPanelTransientPipeline : IDisposable
    {
        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        void Register(ShippingPanelViewModel viewModel);
    }
}
