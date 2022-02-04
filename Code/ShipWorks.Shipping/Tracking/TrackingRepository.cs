using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Tracking.DTO;

namespace ShipWorks.Shipping.Tracking
{
    /// <summary>
    /// Tracking Repository
    /// </summary>
    [Component]
    public class TrackingRepository : ITrackingRepository
    {
        private readonly ISqlAdapter sqlAdapter;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public TrackingRepository(ISqlAdapterFactory sqlAdapterFactory)
        {
            sqlAdapter = sqlAdapterFactory.Create();
        }
        
        /// <summary>
        /// Marks the shipment with a status of AwaitingUpdate
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
                shipment.TrackingStatus = EnumHelper.GetEnumByApiValue<TrackingStatus>(notification.TrackingStatus);
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
                .AndWhere(ShipmentFields.ShipmentType != ShipmentTypeCode.Other)
                .AndWhere(ShipmentFields.ShipmentType != ShipmentTypeCode.AmazonSFP)
                .AndWhere(ShipmentFields.ShipmentType != ShipmentTypeCode.AmazonSWA)
                .AndWhere(ShipmentFields.ShipmentType != ShipmentTypeCode.DhlExpress)
                .AndWhere(ShipmentFields.ShipmentType != ShipmentTypeCode.Asendia)
                .AndWhere(ShipmentFields.ShipmentType != ShipmentTypeCode.OnTrac)
                .AndWhere(ShipmentFields.Processed == true)
                .AndWhere(ShipmentFields.Voided == false)
                .Limit(100);

            return await QueryShipmentEntities(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the most recent tracking notification
        /// </summary>
        public async Task<DateTime> GetLatestNotificationDate()
        {
            var queryFactory = new QueryFactory();
            var query = queryFactory.Create().Select(queryFactory.Shipment.Select(ShipmentFields.TrackingHubTimestamp).Max());
            return await sqlAdapter.FetchScalarAsync<DateTime?>(query).ConfigureAwait(false) ??
                   DateTime.MinValue;
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

        /// <summary>
        /// Fetches a collection of shipment entities for the specified query
        /// </summary>
        private async Task<EntityCollection<ShipmentEntity>> QueryShipmentEntities(EntityQuery<ShipmentEntity> query)
        {
            var shipments = new EntityCollection<ShipmentEntity>();
            await sqlAdapter.FetchQueryAsync(query, shipments).ConfigureAwait(false);

            return shipments;
        }
    }
}