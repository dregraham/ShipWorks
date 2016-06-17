using System;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Factory for getting view models
    /// </summary>
    public class ShippingViewModelFactory : IShippingViewModelFactory
    {
        private readonly Func<ShipmentTypeCode, IShipmentViewModel> shipmentViewModelFactory;
        private readonly Func<AddressViewModel> addressViewModelFactory;
        private readonly Func<IInsuranceViewModel> insuranceViewModelFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingViewModelFactory(Func<ShipmentTypeCode, IShipmentViewModel> shipmentViewModelFactory,
                                        Func<AddressViewModel> addressViewModelFactory,
                                        Func<IInsuranceViewModel> insuranceViewModelFactory)
        {
            this.shipmentViewModelFactory = shipmentViewModelFactory;
            this.addressViewModelFactory = addressViewModelFactory;
            this.insuranceViewModelFactory = insuranceViewModelFactory;
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

        /// <summary>
        /// Return a new InsuranceViewModel
        /// </summary>
        /// <returns></returns>
        public IInsuranceViewModel GetInsuranceViewModel()
        {
            return insuranceViewModelFactory();
        }
    }
}
