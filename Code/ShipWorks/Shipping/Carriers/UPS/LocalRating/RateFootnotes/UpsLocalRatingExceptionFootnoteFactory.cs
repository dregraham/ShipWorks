using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.UPS.LocalRating.RateFootnotes
{
    public class UpsLocalRatingExceptionFootnoteFactory : IRateFootnoteFactory
    {
        private readonly string errorMessage;

        public UpsLocalRatingExceptionFootnoteFactory(string errorMessage, ShipmentTypeCode shipmentType)
        {
            this.errorMessage = errorMessage;
            ShipmentTypeCode = shipmentType;
        }

        public ShipmentTypeCode ShipmentTypeCode { get; }

        public RateFootnoteControl CreateFootnote(IFootnoteParameters parameters)
        {
            return new UpsLocalRatingExceptionFootnote(errorMessage);
        }

        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IUpsLocalRatingExceptionFootnoteViewModel viewModel = lifetimeScope.Resolve<IUpsLocalRatingExceptionFootnoteViewModel>();
                viewModel.ErrorMessage = errorMessage;

                return viewModel;
            }
        }

        public bool AllowedForBestRate => true;
    }
}