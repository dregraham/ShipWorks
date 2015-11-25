using Autofac;
using ShipWorks.Data.Model.EntityClasses;

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
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.OnTrac);

            builder.RegisterType<OnTracLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.OnTrac);
            
            builder.RegisterType<OnTracAccountRepository>()
                .As<ICarrierAccountRepository<OnTracAccountEntity>>();
        }
    }
}
