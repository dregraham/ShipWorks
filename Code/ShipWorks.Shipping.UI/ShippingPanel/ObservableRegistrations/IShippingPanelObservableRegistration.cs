using System;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Register an observable pipeline for the shipping panel view model
    /// </summary>
    public interface IShippingPanelObservableRegistration
    {
        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        IDisposable Register(ShippingPanelViewModel viewModel);
    }
}
