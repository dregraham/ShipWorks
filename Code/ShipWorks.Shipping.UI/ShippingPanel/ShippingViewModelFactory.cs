using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Factory for getting view models
    /// </summary>
    public class ShippingViewModelFactory : IShippingViewModelFactory
    {
        private readonly Func<ShipmentViewModel> shipmentViewModelFactory;
        private readonly Func<AddressViewModel> addressViewModelFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingViewModelFactory(Func<ShipmentViewModel> shipmentViewModelFactory, Func<AddressViewModel> addressViewModelFactory)
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
        public ShipmentViewModel GetShipmentViewModel()
        {
            return shipmentViewModelFactory();
        }
    }
}
