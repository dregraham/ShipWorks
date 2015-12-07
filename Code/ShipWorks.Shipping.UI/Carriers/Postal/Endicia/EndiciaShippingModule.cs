using Autofac;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;

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

            builder.RegisterType<EndiciaAccountRepository>()
                .AsSelf();

            builder.RegisterType<Express1EndiciaAccountRepository>()
                .AsSelf();
            
            builder.RegisterType<EndiciaRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.Endicia)
                .Keyed<IRatingService>(ShipmentTypeCode.Express1Endicia);

            builder.RegisterType<EndiciaRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.Endicia)
                .Keyed<IRateHashingService>(ShipmentTypeCode.Express1Endicia)
                .AsSelf();

            builder.Register<CarrierAccountRepositoryBase<EndiciaAccountEntity>>((container, parameters) =>
            {
                if (parameters.TypedAs<ShipmentTypeCode>() == ShipmentTypeCode.Express1Endicia)
                {
                    return container.Resolve<Express1EndiciaAccountRepository>();
                }
                return container.Resolve<EndiciaAccountRepository>();
            });


        }
    }
}
