using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public class FedExShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FedExLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.FedEx);
            
            builder.RegisterType<FedExSettingsRepository>()
                .Keyed<ICarrierSettingsRepository>(ShipmentTypeCode.FedEx)
                .AsSelf();

            builder.Register<IShippingClerk>((container, parameters) =>
            {
                return FedExShippingClerkFactory.CreateShippingClerk(
                    parameters.TypedAs<ShipmentEntity>(),
                    container.ResolveKeyed<ICarrierSettingsRepository>(ShipmentTypeCode.FedEx));
            });
        }
    }
}
