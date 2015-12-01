using Autofac;
using ShipWorks.Shipping.Carriers.Postal.Endicia;

namespace ShipWorks.Shipping.Carriers.Endicia
{
    /// <summary>
    /// Shipping module for endicia
    /// </summary>
    public class EndiciaShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EndiciaShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Endicia);

            builder.RegisterType<EndiciaLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.Endicia);
        }
    }
}
