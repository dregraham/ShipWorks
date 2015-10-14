using Autofac;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public class BestRateShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BestRateShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.BestRate);

            builder.RegisterType<NullAccountRepository>()
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.BestRate)
                .SingleInstance();

            builder.RegisterType<BestRateShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.BestRate)
                .ExternallyOwned();
        }
    }
}
