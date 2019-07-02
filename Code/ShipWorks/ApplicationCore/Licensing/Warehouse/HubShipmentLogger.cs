using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Class for logging shipments to the hub
    /// </summary>
    [Component]
    public class HubShipmentLogger : IHubShipmentLogger
    {
        private readonly IWarehouseOrderClient warehouseOrderClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubShipmentLogger(IWarehouseOrderClient warehouseOrderClient)
        {
            this.warehouseOrderClient = warehouseOrderClient;
        }

        /// <summary>
        /// Log processed shipments to the hub
        /// </summary>
        public async Task LogProcessedShipments(IEnumerable<ShipmentEntity> shipments)
        {
            IEnumerable<ShipmentEntity> shipmentsToLog = shipments.Where(s => s.Order.HubOrderID.HasValue &&
                                                                              s.Processed &&
                                                                              !s.Voided &&
                                                                              s.LoggedShippedToHub == false);

            foreach (var shipmentToLog in shipmentsToLog)
            {
                await warehouseOrderClient.UploadShipment(shipmentToLog, shipmentToLog.Order.HubOrderID.Value, shipmentToLog.OnlineShipmentID)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Log voided shipments to the hub
        /// </summary>
        public async Task LogVoidedShipments(IEnumerable<ShipmentEntity> shipments)
        {
            IEnumerable<ShipmentEntity> voidedShipmentsToLog = shipments.Where(s => s.Order.HubOrderID.HasValue &&
                                                                                    s.Voided &&
                                                                                    s.LoggedShippedToHub == true &&
                                                                                    s.LoggedVoidToHub == false);

            foreach (var shipmentToLog in voidedShipmentsToLog)
            {
                await warehouseOrderClient.UploadVoid(shipmentToLog.ShipmentID, shipmentToLog.Order.HubOrderID.Value, shipmentToLog.OnlineShipmentID)
                    .ConfigureAwait(false);
            }
        }
    }
}
