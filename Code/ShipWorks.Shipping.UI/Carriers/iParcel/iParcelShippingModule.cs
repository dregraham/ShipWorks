using System.Diagnostics.CodeAnalysis;
using Autofac;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
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
            builder.RegisterType<iParcelShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.iParcel);

            builder.RegisterType<iParcelShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.iParcel)
                .SingleInstance();

            builder.RegisterType<iParcelAccountRepository>()
                .Keyed<CarrierAccountRepositoryBase<IParcelAccountEntity>>(ShipmentTypeCode.iParcel)
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.iParcel)
                .AsImplementedInterfaces();

            builder.RegisterType<iParcelShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.iParcel)
                .ExternallyOwned();

            builder.RegisterType<NullShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.iParcel)
                .SingleInstance();

            builder.RegisterType<iParcelDatabaseRepository>()
                .AsImplementedInterfaces();

            builder.RegisterType<iParcelServiceGateway>()
                .AsImplementedInterfaces();

            builder.RegisterType<iParcelLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.iParcel);

            builder.RegisterType<iParcelRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.iParcel);

            builder.RegisterType<iParcelRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.iParcel)
                .AsImplementedInterfaces();
        }
    }
}
