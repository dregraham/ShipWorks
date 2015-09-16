using Autofac;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    public class OnTracShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OnTracShipmentType>()
                .Keyed<ShipmentType>(ShipmentTypeCode.OnTrac);
        }
    }
}
