using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Enums;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Tracking
{
    /// <summary>
    /// Tracking Repository
    /// </summary>
    class TrackingRepository : ITrackingRepository
    {
        private readonly ISqlAdapter sqlAdapter;
        private readonly IShippingManager shippingManager;

        public TrackingRepository(ISqlAdapter sqlAdapter, IShippingManager shippingManager)
        {
            this.sqlAdapter = sqlAdapter;
            this.shippingManager = shippingManager;
        }
        /// <summary>
        /// Marks the shipment with a status of AwaitingResponse
        /// </summary>
        public void MarkAsSent(ShipmentEntity shipment)
        {
            shipment.TrackingStatus = TrackingStatus.AwaitingUpdate;
            var result = shippingManager.SaveShipmentToDatabase(shipment, false);
            var ex = result.SingleOrDefault().Value;
            if (ex != null)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save the notification to a shipment
        /// </summary>
        public void SaveNotification(TrackingNotification notification)
        {
            var shipments = GetShipments(notification.TrackingNumber);
            foreach (var shipment in shipments)
            {
                shipment.TrackingStatus = notification.TrackingStatus;
                shipment.ActualDeliveryDate = notification.ActualDeliveryDate;
                shipment.EstimatedDeliveryDate = notification.EstimatedDeliveryDate;
            }

            sqlAdapter.SaveEntityCollection(shipments);
        }

        /// <summary>
        /// Get shipments matching the tracking number
        /// </summary>
        private EntityCollection<ShipmentEntity> GetShipments(string trackingNumber)
        {
            var query = new QueryFactory().Shipment
                .Where(ShipmentFields.TrackingNumber == trackingNumber);

            var shipments = new EntityCollection<ShipmentEntity>();
            sqlAdapter.FetchQuery(query, shipments);

            return shipments;
        }
    }
}