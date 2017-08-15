using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using System.Xml.Xsl;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using ShipWorks.Shipping;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;
using ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.CommerceInterface
{
    /// <summary>
    /// Customized Web Client for CommerceInterface since they use a hybrid status/shipment update message 
    /// </summary>
    public class CommerceInterfaceWebClient : LegacyAdapterStoreWebClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(CommerceInterfaceWebClient));

        /// <summary>
        /// Constructor
        /// </summary>
        public CommerceInterfaceWebClient(GenericModuleStoreEntity store, 
            XslCompiledTransform communicationStylesheet,
            GenericModuleCapabilities legacyCapabilities, 
            Dictionary<string, VariableTransformer> variableTransformers) : base (store, communicationStylesheet, legacyCapabilities, variableTransformers)
        {

        }

        /// <summary>
        /// Uploads order and shipment information to CommerceInterface
        /// </summary>
        public async Task UploadShipmentDetails(OrderEntity order, ShipmentEntity shipment, int statusCode)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }

            if (order.IsManual && order.CombineSplitStatus == CombineSplitStatusType.None)
            {
                log.WarnFormat("Not uploading shipment details since order {0} is manual.", order.OrderID);
                return;
            }

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                ICombineOrderSearchProvider<string> combineOrderSearchProvider = scope.Resolve<ICombineOrderSearchProvider<string>>();
                IEnumerable<string> orderNumbers = await combineOrderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

                foreach (string orderNumber in orderNumbers)
                {
                    UploadShipmentDetailsInternal(orderNumber, shipment, statusCode);
                }
            }
        }

        /// <summary>
        /// Uploads order and shipment information to CommerceInterface
        /// </summary>
        private void UploadShipmentDetailsInternal(string orderNumber, ShipmentEntity shipment, int statusCode)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            request.Variables.Add("order", orderNumber);
            request.Variables.Add("tracking", shipment.TrackingNumber);
            request.Variables.Add("code", statusCode.ToString());
            request.Variables.Add("carrier", GetShipmentCarrier(shipment));

            ProcessRequest(request, "updateshipment");
        }

        /// <summary>
        /// Gets the carrier name for a shipment
        /// </summary>
        private string GetShipmentCarrier(ShipmentEntity shipment)
        {
            return ShippingManager.GetCarrierName((ShipmentTypeCode)shipment.ShipmentType);
        }
    }
}
