using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.UPS.LocalRating.RateFootnotes
{
    public class UpsLocalRatingDisabledFootnoteFactory : IRateFootnoteFactory
    {
        private readonly UpsAccountEntity upsAccount;

        public UpsLocalRatingDisabledFootnoteFactory(UpsAccountEntity upsAccount)
        {
            this.upsAccount = upsAccount;
            ShipmentTypeCode = upsAccount.ShipmentType;
        }

        public ShipmentTypeCode ShipmentTypeCode { get; }

        public RateFootnoteControl CreateFootnote(IFootnoteParameters parameters)
        {
            return new UpsLocalRatingDisabledFootnote(parameters, upsAccount);
        }

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

        public bool AllowedForBestRate => true;
    }
}