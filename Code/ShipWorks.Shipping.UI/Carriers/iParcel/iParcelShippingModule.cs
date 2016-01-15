using System.Diagnostics.CodeAnalysis;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Shipping module the iParcel carrier
    /// </summary>
    [SuppressMessage("SonarLint", "S101:Class names should comply with a naming convention",
        Justification = "Class is names to match iParcel's naming convention")]
    public class iParcelShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<iParcelShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.iParcel);
            
            builder.RegisterType<iParcelDatabaseRepository>()
                .AsImplementedInterfaces();
        
            builder.RegisterType<iParcelServiceGateway>()
                .AsImplementedInterfaces();

            builder.RegisterType<iParcelLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.iParcel);

            builder.RegisterType<iParcelRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.iParcel);

            builder.RegisterType<iParcelRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.iParcel)
                .AsImplementedInterfaces();

            builder.RegisterType<iParcelAccountRepository>()
                .Keyed<CarrierAccountRepositoryBase<IParcelAccountEntity>>(ShipmentTypeCode.iParcel)
                .AsImplementedInterfaces();
        }   
    }
}
