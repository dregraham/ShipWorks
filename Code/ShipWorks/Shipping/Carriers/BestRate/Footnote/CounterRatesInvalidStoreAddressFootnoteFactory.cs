using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    /// <summary>
    /// Factory that creates CounterRatesInvalidStoreAddressFootnoteControls
    /// </summary>
    public class CounterRatesInvalidStoreAddressFootnoteFactory : IRateFootnoteFactory
    {
        /// <summary>
        /// Construct a new CounterRatesInvalidStoreAddressFootnoteControl object
        /// </summary>
        public CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentTypeCode shipmentTypeCode)
        {
            ShipmentTypeCode = shipmentTypeCode;
        }

        /// <summary>
        /// Gets and sets the shipment type
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }

        /// <summary>
        /// Notes that this factory should be used in BestRate
        /// </summary>
        public bool AllowedForBestRate
        {
            get { return true; }
        }

        /// <summary>
        /// Create a new CounterRatesInvalidStoreAddressFootnoteControl
        /// </summary>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new CounterRatesInvalidStoreAddressFootnoteControl(parameters);
        }

        /// <summary>
        /// Get a view model that represents this footnote
        /// </summary>
        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ICounterRatesInvalidStoreAddressFootnoteViewModel viewModel =
                    lifetimeScope.Resolve<ICounterRatesInvalidStoreAddressFootnoteViewModel>();
                viewModel.ShipmentAdapter = shipmentAdapter;
                return viewModel;
            }
        }
    }
}
