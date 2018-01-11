using System.Diagnostics.CodeAnalysis;
using Autofac;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Shipping module the iParcel carrier
    /// </summary>
    [SuppressMessage("SonarLint", "S101:Class names should comply with a naming convention",
        Justification = "Class is names to match iParcel's naming convention")]
    public class iParcelShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<iParcelRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.iParcel);

            builder.RegisterType<iParcelRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.iParcel)
                .AsImplementedInterfaces();

            builder.RegisterType<iParcelShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.iParcel)
                .ExternallyOwned();

            builder.RegisterType<iParcelShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.iParcel)
                .SingleInstance();
        }
    }
}
