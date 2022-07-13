using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
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
        private readonly ILog log;
        private readonly object[] FieldsToReturn = 
        {
            ShipmentFields.ShipmentID,
            ShipmentFields.ShipmentType,
            ShipmentFields.TrackingNumber,
            ShipmentFields.TrackingHubTimestamp
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public TrackingRepository(ISqlAdapterFactory sqlAdapterFactory, Func<Type, ILog> typeFactory)
        {
            sqlAdapter = sqlAdapterFactory.Create();
            log = typeFactory(typeof(TrackingRepository));
        }
        
        /// <summary>
        /// Marks the shipment with a status of AwaitingUpdate
        /// </summary>
        public async Task MarkAsSent(ShipmentEntity shipment)
        {
            var shipmentToSave = new ShipmentEntity(shipment.ShipmentID)
            {
                TrackingStatus = TrackingStatus.AwaitingUpdate
            };

            await sqlAdapter.UpdateEntitiesDirectlyAsync(shipmentToSave,
                new RelationPredicateBucket(ShipmentFields.ShipmentID == shipment.ShipmentID),
                CancellationToken.None);
        }

        /// <summary>
        /// Save the notification to a shipment
        /// </summary>
        public async Task SaveNotification(TrackingNotification notification)
        {
            if (!EnumHelper.TryGetEnumByApiValue(notification.TrackingStatus, out TrackingStatus? status))
            {
                log.Warn($"Unknown tracking api code encountered: {notification.TrackingStatus}");
            }

            status = status ?? TrackingStatus.Unknown;

            var shipments = await GetShipmentIds(notification.TrackingNumber).ConfigureAwait(false);
            foreach (var shipment in shipments)
            {
                var shipmentToSave = new ShipmentEntity(shipment.ShipmentID)
                {
                    TrackingStatus = status.Value,
                    ActualDeliveryDate = notification.ActualDeliveryDate,
                    EstimatedDeliveryDate = notification.EstimatedDeliveryDate,
                    TrackingHubTimestamp = notification.HubTimestamp,
                };

                await sqlAdapter.UpdateEntitiesDirectlyAsync(shipmentToSave,
                    new RelationPredicateBucket(ShipmentFields.ShipmentID == shipmentToSave.ShipmentID));
            }
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
                .AndWhere(ShipmentFields.TrackingNumber != string.Empty)
                .Select(FieldsToReturn)
                .Limit(100);

            var shipments = await QueryShipmentEntities(query).ConfigureAwait(false);

            return shipments;
        }

        /// <summary>
        /// Get the most recent tracking notification
        /// </summary>
        public async Task<DateTime> GetLatestNotificationDate()
        {
            var queryFactory = new QueryFactory();
            var query = queryFactory
                .Create()
                .Select(queryFactory.Shipment.Select(ShipmentFields.TrackingHubTimestamp).Max());

            return await sqlAdapter.FetchScalarAsync<DateTime?>(query).ConfigureAwait(false) ??
                   DateTime.MinValue;
        }

        /// <summary>
        /// Get shipments matching the tracking number
        /// </summary>
        private async Task<List<ShipmentEntity>> GetShipmentIds(string trackingNumber)
        {
            var query = new QueryFactory().Shipment
                .Where(ShipmentFields.TrackingNumber == trackingNumber)
                .Select(FieldsToReturn);

            return await QueryShipmentEntities(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Fetches a collection of IDs for the specified query
        /// </summary>
        private async Task<List<ShipmentEntity>> QueryShipmentEntities(DynamicQuery query)
        {
            var fetchResult = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);

            var shipments = fetchResult.Select(s => 
                new ShipmentEntity((long) s[0])
                {
                    ShipmentType = (int) (s[1] ?? null),
                    TrackingNumber = (string) (s[2] ?? null),
                }
            ).ToList();

            return shipments;
        }
    }
}