using Autofac;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public class FedExShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FedExShipmentType>()
                .Keyed<ShipmentType>(ShipmentTypeCode.FedEx);
        }
    }
}
