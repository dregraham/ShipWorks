using Autofac;
using ShipWorks.Core.ApplicationCode;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.UI.Carriers.FedEx
{
    /// <summary>
    /// Shipping module for the FedEx carrier
    /// </summary>
    public class FedExShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FedExShipmentType>()
                .FindConstructorsWith(new NonDefaultConstructorFinder())
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.FedEx);

            builder.RegisterType<FedExShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.FedEx)
                .FindConstructorsWith(new NonDefaultConstructorFinder())
                .SingleInstance();

            builder.RegisterType<FedExShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.FedEx)
                .SingleInstance();

            builder.RegisterType<FedExUtilityWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<FedExAccountRepository>()
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.FedEx)
                .SingleInstance();

            builder.RegisterType<FedExShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.FedEx)
                .ExternallyOwned();

            builder.RegisterType<FedExLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.FedEx);

            builder.RegisterType<FedExRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.FedEx);

            builder.RegisterType<FedExSettingsRepository>()
                .Keyed<ICarrierSettingsRepository>(ShipmentTypeCode.FedEx)
                .AsSelf();

            builder.RegisterType<FedExRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.FedEx);

            builder.RegisterType<FedExAccountRepository>();

            builder.RegisterType<FedExShippingClerkFactory>();

            builder.RegisterType<FedExShipmentProcessingSynchronizer>()
                .Keyed<IShipmentProcessingSynchronizer>(ShipmentTypeCode.FedEx);
        }
    }
}
