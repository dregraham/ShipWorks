using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using System.Xml.Xsl;
using Interapptive.Shared.Net;
using ShipWorks.Shipping;
using log4net;
using ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter;

namespace ShipWorks.Stores.Platforms.CommerceInterface
{
    /// <summary>
    /// Cutomized Web Client for CommerceInterface since they use a hybrid status/shipment update message 
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
        public void UploadShipmentDetails(OrderEntity order, ShipmentEntity shipment, int statusCode)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }

            if (order.IsManual)
            {
                log.WarnFormat("Not uploading shipment details since order {0} is manual.", order.OrderID);
                return;
            }

            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            GenericModuleStoreType type = (GenericModuleStoreType)StoreTypeManager.GetType(Store);

            request.Variables.Add("order", type.GetOnlineOrderIdentifier(order));
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
