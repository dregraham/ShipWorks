using Autofac;
using ShipWorks.Shipping.Carriers.None;

namespace ShipWorks.Shipping.Carriers.None
{
    /// <summary>
    /// Shipping module for the None carrier
    /// </summary>
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
