using Autofac;
using ShipWorks.Shipping.Carriers.None;

namespace ShipWorks.Shipping.Carriers.None
{
    public class NoneShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NoneLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.None);
        }
    }
}
