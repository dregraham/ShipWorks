using Autofac;
using ShipWorks.Shipping.Carriers.Ups;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;

namespace ShipWorks.Shipping.UI.Carriers.Ups
{
    /// <summary>
    /// Service registrations for the Ups shipping carrier
    /// </summary>
    public class UpsShippingModule : Module
    {
        /// <summary>
        /// Load the registrations
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<UpsAccountRepository>()
                .AsImplementedInterfaces();

            builder.RegisterType<UpsOltLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.UpsOnLineTools);

            builder.RegisterType<WorldShipLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.UpsWorldShip);

            builder.RegisterType<UpsOltShipmentValidator>()
                .AsImplementedInterfaces();
        }
    }
}
