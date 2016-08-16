using System.Collections.Generic;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Discounted
{
    /// <summary>
    /// Creates Usps Rate Discounted footnote controls
    /// </summary>
    public class UspsRateDiscountedFootnoteFactory : IRateFootnoteFactory
    {
        private readonly List<RateResult> originalRates;
        private readonly List<RateResult> discountedRates;

        /// <summary>
        /// Construct a new UspsRateDiscountedFootnoteFactory object
        /// </summary>
        /// <param name="shipmentType">Type of shipment that instantiated this factory</param>
        /// <param name="originalRates">Original rates from the carrier</param>
        /// <param name="discountedRates">Express1 rates</param>
        public UspsRateDiscountedFootnoteFactory(ShipmentTypeCode shipmentTypeCode, List<RateResult> originalRates, List<RateResult> discountedRates)
        {
            this.originalRates = originalRates;
            this.discountedRates = discountedRates;

            ShipmentTypeCode = shipmentTypeCode;
        }

        /// <summary>
        /// Notes that this factory should not be used in BestRate
        /// </summary>
        public bool AllowedForBestRate
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }

        /// <summary>
        /// Create an Express1 rate discounted control
        /// </summary>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new UspsRateDiscountedFootnote(originalRates, discountedRates);
        }

        /// <summary>
        /// Get a view model that represents this footnote
        /// </summary>
        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IUspsRateDiscountedFootnoteViewModel viewModel = lifetimeScope.Resolve<IUspsRateDiscountedFootnoteViewModel>();
                viewModel.OriginalRates = originalRates;
                viewModel.DiscountedRates = discountedRates;
                return viewModel;
            }
        }
    }
}
