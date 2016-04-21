using System;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// Shipment view model
    /// </summary>
    public interface IShipmentViewModel : IDisposable
    {
        /// <summary>
        /// Load the view model with the given shipment adapter
        /// </summary>
        void Load(ICarrierShipmentAdapter shipmentAdapter);

        /// <summary>
        /// Save the contents of the view model into the loaded adapter
        /// </summary>
        void Save();

        /// <summary>
        /// </summary>
        void RefreshServiceTypes();

        /// <summary>
        /// </summary>
        void RefreshPackageTypes();

        /// <summary>
        /// Are customs allowed for this shipment/shipment type?
        /// </summary>
        bool CustomsAllowed { get; set; }

        /// <summary>
        /// Stream of property changes
        /// </summary>
        IObservable<string> PropertyChangeStream { get; }

        /// <summary>
        /// Load customs into the view model
        /// </summary>
        void LoadCustoms();

        /// <summary>
        /// Select the given rate
        /// </summary>
        void SelectRate(RateResult selectedRate);

        /// <summary>
        /// Refresh the insurance view model for this shipment
        /// </summary>
        void RefreshInsurance();

        /// <summary>
        /// The insurance view model of the shipment
        /// </summary>
        IInsuranceViewModel InsuranceViewModel { get; }

        /// <summary>
        /// Gets the currently selected rate
        /// </summary>
        RateResult SelectedRate { get; }

        /// <summary>
        /// Error message displayed by the weight
        /// </summary>
        /// <remarks>Intended mainly to clear the error message after changing shipments</remarks>
        string WeightErrorMessage { get; set; }
    }
}