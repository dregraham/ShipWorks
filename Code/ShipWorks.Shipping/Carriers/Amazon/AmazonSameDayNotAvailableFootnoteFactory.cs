using Autofac;
using Autofac.Features.OwnedInstances;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Let customers know that same day shipping was requested but that same day rates are not available
    /// </summary>
    [Component(RegistrationType.Self)]
    [ResolveWithAttributes]
    public class AmazonSameDayNotAvailableFootnoteFactory : IRateFootnoteFactory
    {
        public const string ControlKey = "AmazonSameDayNotAvailableFootnoteControl";

        /// <summary>
        /// Not allowed for best rates
        /// </summary>
        public bool AllowedForBestRate => false;

        /// <summary>
        /// Shipment type for which this footnote applies
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.Amazon;

        /// <summary>
        /// Create the footnote control
        /// </summary>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            IAmazonSameDayNotAvailableFootnoteViewModel viewModel =
                IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IAmazonSameDayNotAvailableFootnoteViewModel>>().Value;
            return IoC.UnsafeGlobalLifetimeScope
                .ResolveKeyed<Owned<RateFootnoteControl>>(ControlKey, TypedParameter.From<object>(viewModel)).Value;
        }

        /// <summary>
        /// Create the footnote view model
        /// </summary>
        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter) =>
            IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IAmazonSameDayNotAvailableFootnoteViewModel>>().Value;
    }
}
