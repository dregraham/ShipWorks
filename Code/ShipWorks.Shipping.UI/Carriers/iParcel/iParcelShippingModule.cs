using System.Diagnostics.CodeAnalysis;
using Autofac;
using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Shipping.Carriers.iParcel
{
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
        }
    }
}
