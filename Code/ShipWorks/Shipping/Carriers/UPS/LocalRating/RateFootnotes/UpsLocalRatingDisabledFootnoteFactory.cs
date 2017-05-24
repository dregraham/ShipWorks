using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.UPS.LocalRating.RateFootnotes
{
    /// <summary>
    /// Footnote factory for creating footnotes when no UPS rates are returned because local is not enabled
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Editing.Rating.IRateFootnoteFactory" />
    public class UpsLocalRatingDisabledFootnoteFactory : IRateFootnoteFactory
    {
        private readonly UpsAccountEntity upsAccount;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRatingDisabledFootnoteFactory"/> class.
        /// </summary>
        public UpsLocalRatingDisabledFootnoteFactory(UpsAccountEntity upsAccount)
        {
            this.upsAccount = upsAccount;
            ShipmentTypeCode = upsAccount.ShipmentType;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; }

        /// <summary>
        /// Creates a footnote control that represents this footnote
        /// </summary>
        public RateFootnoteControl CreateFootnote(IFootnoteParameters parameters)
        {
            return new UpsLocalRatingDisabledFootnote(parameters, upsAccount);
        }

        /// <summary>
        /// Get a view model that represents this footnote
        /// </summary>
        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IUpsLocalRatingDisabledFootnoteViewModel viewModel = lifetimeScope.Resolve<IUpsLocalRatingDisabledFootnoteViewModel>();
                viewModel.UpsAccount = upsAccount;
                viewModel.ShipmentAdapter = shipmentAdapter;

                return viewModel;
            }
        }

        /// <summary>
        /// Notes that this factory should or should not be used in BestRate
        /// </summary>
        public bool AllowedForBestRate => true;
    }
}