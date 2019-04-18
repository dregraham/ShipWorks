using Autofac;
using Autofac.Features.OwnedInstances;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Let customers know that same day shipping was requested but that same day rates are not available
    /// </summary>
    [Component(RegistrationType.Self)]
    public class AmazonSFPSameDayNotAvailableFootnoteFactory : IRateFootnoteFactory
    {
        public const string ControlKey = "AmazonSameDayNotAvailableFootnoteControl";

        /// <summary>
        /// Not allowed for best rates
        /// </summary>
        public bool AllowedForBestRate => false;

        /// <summary>
        /// Shipment type for which this footnote applies
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.AmazonSFP;

        /// <summary>
        /// Create the footnote control
        /// </summary>
        public RateFootnoteControl CreateFootnote(IFootnoteParameters parameters)
        {
            IAmazonSFPSameDayNotAvailableFootnoteViewModel viewModel =
                IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IAmazonSFPSameDayNotAvailableFootnoteViewModel>>().Value;
            return IoC.UnsafeGlobalLifetimeScope
                .ResolveKeyed<Owned<RateFootnoteControl>>(ControlKey, TypedParameter.From<object>(viewModel)).Value;
        }

        /// <summary>
        /// Create the footnote view model
        /// </summary>
        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter) =>
            IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IAmazonSFPSameDayNotAvailableFootnoteViewModel>>().Value;
    }
}
