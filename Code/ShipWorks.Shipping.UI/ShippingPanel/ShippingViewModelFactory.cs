using System;
using ShipWorks.Shipping.UI.ShippingPanel.AddressControl;
using ShipWorks.Shipping.UI.ShippingPanel.CustomsControl;
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
        private readonly Func<InsuranceViewModel> insuranceViewModelFactory;
        private readonly Func<CustomsControlViewModel> customsControlViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingViewModelFactory(Func<ShipmentTypeCode, IShipmentViewModel> shipmentViewModelFactory, 
                                        Func<AddressViewModel> addressViewModelFactory, 
                                        Func<InsuranceViewModel> insuranceViewModelFactory,
                                        Func<CustomsControlViewModel> customsControlViewModel)
        {
            this.shipmentViewModelFactory = shipmentViewModelFactory;
            this.addressViewModelFactory = addressViewModelFactory;
            this.insuranceViewModelFactory = insuranceViewModelFactory;
            this.customsControlViewModel = customsControlViewModel;
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
        public InsuranceViewModel GetInsuranceViewModel()
        {
            return insuranceViewModelFactory();
        }

        /// <summary>
        /// Return a new CustomsControlViewModel
        /// </summary>
        /// <returns></returns>
        public CustomsControlViewModel GetCustomsControlViewModel()
        {
            return customsControlViewModel();
        }
    }
}
