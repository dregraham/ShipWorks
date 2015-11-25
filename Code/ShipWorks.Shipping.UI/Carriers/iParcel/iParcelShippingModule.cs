using System.Diagnostics.CodeAnalysis;
using Autofac;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    [SuppressMessage("SonarLint", "S101:Class names should comply with a naming convention",
        Justification = "Class is names to match iParcel's naming convention")]
    public class iParcelShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<iParcelShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.iParcel);

            builder.RegisterType<iParcelShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.iParcel)
                .SingleInstance();

            builder.RegisterType<iParcelAccountRepository>()
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.iParcel)
                .SingleInstance();

            builder.RegisterType<iParcelShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.iParcel)
                .ExternallyOwned();

            builder.RegisterType<NullShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.Amazon)
                .SingleInstance();
        }
    }
}
