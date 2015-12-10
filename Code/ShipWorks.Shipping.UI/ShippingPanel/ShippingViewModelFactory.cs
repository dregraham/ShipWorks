using System;
using ShipWorks.Shipping.UI.ShippingPanel.AddressControl;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Factory for getting view models
    /// </summary>
    public class ShippingViewModelFactory : IShippingViewModelFactory
    {
        private readonly Func<ShipmentTypeCode, IShipmentViewModel> shipmentViewModelFactory;
        private readonly Func<AddressViewModel> addressViewModelFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingViewModelFactory(Func<ShipmentTypeCode, IShipmentViewModel> shipmentViewModelFactory, Func<AddressViewModel> addressViewModelFactory)
        {
            this.shipmentViewModelFactory = shipmentViewModelFactory;
            this.addressViewModelFactory = addressViewModelFactory;
        }

        /// <summary>
        /// Return a new AddressViewModel
        /// </summary>
        /// <returns></returns>
        public AddressViewModel GetAddressViewModel()
        {
            return addressViewModelFactory();
        }

        /// <summary>
        /// Return a new ShipmentViewModel
        /// </summary>
        /// <returns></returns>
        public IShipmentViewModel GetShipmentViewModel(ShipmentTypeCode shipmentType)
        {
            return shipmentViewModelFactory(shipmentType);
        }
    }
}
