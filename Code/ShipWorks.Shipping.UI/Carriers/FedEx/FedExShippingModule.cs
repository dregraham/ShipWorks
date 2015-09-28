using Autofac;
using ShipWorks.Core.ApplicationCode;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers;
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
                .FindConstructorsWith(new NonDefaultConstructorFinder())                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.FedEx);

            builder.RegisterType<FedExShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.FedEx)
                .FindConstructorsWith(new NonDefaultConstructorFinder())
                .SingleInstance();

            builder.RegisterType<FedExShipmentPackageBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.FedEx)
                .SingleInstance();

            builder.RegisterType<FedExUtilityWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<FedExAccountRepository>()
                .Keyed<ICarrierAccountRepository<ICarrierAccount>>(ShipmentTypeCode.FedEx)
                .SingleInstance();
        }
    }
}
