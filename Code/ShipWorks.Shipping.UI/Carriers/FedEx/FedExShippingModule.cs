using Autofac;
using ShipWorks.Shipping.Carriers.FedEx;

namespace ShipWorks.Shipping.UI.Carriers.FedEx
{
    public class FedExShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FedExShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.FedEx);

            builder.RegisterType<FedExShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.FedEx)
                .SingleInstance();

            builder.RegisterType<FedExShipmentPackageBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.FedEx)
                .SingleInstance();

            builder.RegisterType<FedExUtilityWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
