using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services.ProcessShipmentsWorkflow;
using ShipWorks.UI;

namespace ShipWorks.Api.Orders.Shipments
{
    /// <summary>
    /// Api ShipmentProcessor
    /// </summary>
    [Component]
    public class ApiShipmentProcessor : IApiShipmentProcessor
    {
        private readonly ILifetimeScope scope;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiShipmentProcessor(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        /// <summary>
        /// Process the given Shipment
        /// </summary>
        public async Task<ProcessShipmentResult> Process(ShipmentEntity shipment)
        {
            if (shipment.ShipmentTypeCode == ShipmentTypeCode.None)
            {
                return new ProcessShipmentResult(shipment, new ShippingException($"Cannot process a shipment with the shipment type of '{EnumHelper.GetDescription(ShipmentTypeCode.None)}'"));
            }

            using (var overriddenScope = scope.BeginLifetimeScope(ConfigureShipmentProcessorDependencies))
            {
                var shipments = new[] { shipment };

                using (ICarrierConfigurationShipmentRefresher refresher = overriddenScope.Resolve<ICarrierConfigurationShipmentRefresher>())
                {
                    var shipmentProcessor = overriddenScope.Resolve<IShipmentProcessor>();

                    refresher.RetrieveShipments = () => shipments;
                    IEnumerable<ProcessShipmentResult> result = await shipmentProcessor.Process(shipments, refresher, null, null)
                        .ConfigureAwait(false);
                    
                    return result.FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Override the IAsyncMessageHelper to use the background implementation
        /// </summary>
        private void ConfigureShipmentProcessorDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<BackgroundAsyncMessageHelper>()
                .AsSelf()
                .As<IAsyncMessageHelper>();

            builder.RegisterType<NullNudgeManager>()
                .AsImplementedInterfaces();

            builder.Register(c => new Control())
                .As<IWin32Window>()
                .As<Control>();

            builder.RegisterType<ApiProcessShipmentsWorkflowFactory>()
                .As<IProcessShipmentsWorkflowFactory>();
        }
    }
}
