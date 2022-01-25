using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Tracking
{
    /// <summary>
    /// Tracking Repository
    /// </summary>
    [Component]
    public class TrackingRepository : ITrackingRepository
    {
        private readonly ISqlAdapter sqlAdapter;
        
        public TrackingRepository(ISqlAdapterFactory sqlAdapterFactory)
        {
            sqlAdapter = sqlAdapterFactory.Create();
        }
        /// <summary>
        /// Marks the shipment with a status of AwaitingResponse
        /// </summary>
        public async Task MarkAsSent(ShipmentEntity shipment)
        {
            shipment.TrackingStatus = TrackingStatus.AwaitingUpdate;
            await sqlAdapter.SaveEntityAsync(shipment).ConfigureAwait(false);
        }

        /// <summary>
        /// Save the notification to a shipment
        /// </summary>
        public async Task SaveNotification(TrackingNotification notification)
        {
            var shipments = await GetShipments(notification.TrackingNumber).ConfigureAwait(false);
            foreach (var shipment in shipments)
            {
                shipment.TrackingStatus = notification.TrackingStatus;
                shipment.ActualDeliveryDate = notification.ActualDeliveryDate;
                shipment.EstimatedDeliveryDate = notification.EstimatedDeliveryDate;
                shipment.TrackingHubTimestamp = notification.HubTimestamp;
            }

            await sqlAdapter.SaveEntityCollectionAsync(shipments).ConfigureAwait(false);
        }

        /// <summary>
        /// Fetch a batch of Shipments to track
        /// </summary>
        public async Task<IEnumerable<ShipmentEntity>> FetchShipmentsToTrack()
        {
            var query = new QueryFactory().Shipment
                .Where(ShipmentFields.TrackingStatus == TrackingStatus.Pending)
                .Where(ShipmentFields.ShipmentType != ShipmentTypeCode.Other)
                .Where(ShipmentFields.ShipmentType != ShipmentTypeCode.AmazonSFP)
                .Where(ShipmentFields.ShipmentType != ShipmentTypeCode.AmazonSWA)
                .Where(ShipmentFields.Processed == true)
                .Where(ShipmentFields.Voided == false)
                .Limit(100);

            return await QueryShipmentEntities(query).ConfigureAwait(false);
        }

        public async Task<DateTime> GetLatestNotificationDate()
        {
            var queryFactory = new QueryFactory();
            var query = queryFactory.Create().Select(queryFactory.Shipment.Select(ShipmentFields.TrackingHubTimestamp).Max());
            return await sqlAdapter.FetchScalarAsync<DateTime>(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get shipments matching the tracking number
        /// </summary>
        private async Task<EntityCollection<ShipmentEntity>> GetShipments(string trackingNumber)
        {
            var query = new QueryFactory().Shipment
                .Where(ShipmentFields.TrackingNumber == trackingNumber);

            return await QueryShipmentEntities(query).ConfigureAwait(false);
        }

        private async Task<EntityCollection<ShipmentEntity>> QueryShipmentEntities(EntityQuery<ShipmentEntity> query)
        {
            var shipments = new EntityCollection<ShipmentEntity>();
            await sqlAdapter.FetchQueryAsync(query, shipments).ConfigureAwait(false);

            return shipments;
        }
    }
}