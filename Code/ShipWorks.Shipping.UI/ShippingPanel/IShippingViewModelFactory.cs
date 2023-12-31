﻿using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Factory for getting view models
    /// </summary>
    public interface IShippingViewModelFactory
    {
        /// <summary>
        /// Get the shipping address control view model
        /// </summary>
        AddressViewModel GetAddressViewModel();

        /// <summary>
        /// Get the shipment control view model
        /// </summary>
        IShipmentViewModel GetShipmentViewModel(ShipmentTypeCode shipmentType);

        /// <summary>
        /// Return a new InsuranceViewModel
        /// </summary>
        /// <returns></returns>
        IInsuranceViewModel GetInsuranceViewModel();
    }
}
