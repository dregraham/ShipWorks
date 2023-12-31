﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Settings;
using ShipWorks.Users;
using static Interapptive.Shared.Utility.Functional;

namespace ShipWorks.OrderLookup.ShipmentHistory
{
    /// <summary>
    /// Entity gateway for shipment history
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ShipmentHistoryEntityGateway : IEntityGateway
    {
        private static readonly Pager pager = new Pager(50);

        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly LruCache<long, ProcessedShipmentEntity> cache;
        private ImmutableArray<ShipmentHistoryHeader> shipmentHeaders;
        private ImmutableArray<ShipmentHistoryHeader> filteredList;
        private readonly string filterTerm = string.Empty;
        private bool isDataFetched = false;
        private CancellationTokenSource cancellationToken;
        private Task fetchDataTask;

        /// <summary>
        /// Copy constructor
        /// </summary>
        private ShipmentHistoryEntityGateway(ShipmentHistoryEntityGateway copyFrom, string filterTerm)
        {
            sqlAdapterFactory = copyFrom.sqlAdapterFactory;
            dateTimeProvider = copyFrom.dateTimeProvider;
            shipmentHeaders = copyFrom.shipmentHeaders;
            cache = copyFrom.cache;
            fetchDataTask = copyFrom.fetchDataTask;
            cancellationToken = copyFrom.cancellationToken;

            this.isDataFetched = true;
            this.filterTerm = filterTerm;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentHistoryEntityGateway(ISqlAdapterFactory sqlAdapterFactory, IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.sqlAdapterFactory = sqlAdapterFactory;
            cache = new LruCache<long, ProcessedShipmentEntity>(1000);
        }

        /// <summary>
        /// Filter this gateway with the given term
        /// </summary>
        public ShipmentHistoryEntityGateway Filter(string filter) =>
            new ShipmentHistoryEntityGateway(this, filter);

        /// <summary>
        /// Open the gateway
        /// </summary>
        public void Open(SortDefinition sortDefinition)
        {
            if (isDataFetched)
            {
                fetchDataTask = (fetchDataTask ?? Task.CompletedTask)
                    .ContinueWith(_ => UpdateFilteredList());

                return;
            }

            cancellationToken = new CancellationTokenSource();

            var factory = new QueryFactory();
            var queryStarter = factory.ProcessedShipment
                    .Where(ProcessedShipmentFields.ProcessedDate >= dateTimeProvider.GetUtcNow().Date);

            var sortedQuery = sortDefinition.SortExpression
                .OfType<ISortClause>()
                    .Aggregate(queryStarter, (query, clause) =>
                    {
                        if (sortDefinition.Relations?.Count > 0)
                        {
                            queryStarter = queryStarter.From(sortDefinition.Relations);
                        }

                        return query.OrderBy(clause);
                    });

            var shipmentQuery = sortedQuery
                .Select(() => new ShipmentHistoryHeader(
                    ProcessedShipmentFields.ShipmentID.ToValue<long>(),
                    ProcessedShipmentFields.TrackingNumber.ToValue<string>(),
                    ProcessedShipmentFields.OrderID.ToValue<long>(),
                    ProcessedShipmentFields.OrderNumberComplete.ToValue<string>()));

            fetchDataTask = UsingAsync(
                    sqlAdapterFactory.Create(),
                    x => x.FetchQueryAsync(shipmentQuery, cancellationToken.Token))
                .ContinueWith(task =>
                {
                    isDataFetched = task.Status == TaskStatus.RanToCompletion;
                    if (isDataFetched)
                    {
                        shipmentHeaders = task.Result.ToImmutableArray();
                    }
                })
                .ContinueWith(task => UpdateFilteredList());
        }

        /// <summary>
        /// Refresh the given entity
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="p"></param>
        public GenericResult<ProcessedShipmentEntity> RefreshEntity(ProcessedShipmentEntity shipment)
        {
            if (Using(
                sqlAdapterFactory.Create(),
                x => x.FetchEntity(shipment)))
            {
                return shipment;
            };

            return GenericResult.FromError("Could not refresh shipment", shipment);
        }

        /// <summary>
        /// Update the filtered list
        /// </summary>
        private void UpdateFilteredList()
        {
            filteredList = shipmentHeaders.Where(x => x.MatchesFilter(filterTerm)).ToImmutableArray();
        }

        /// <summary>
        /// Close the gateway
        /// </summary>
        public void Close()
        {
            cancellationToken?.Cancel();
            fetchDataTask = null;

            isDataFetched = false;
        }

        /// <summary>
        /// Clone the gateway
        /// </summary>
        /// <remarks>We can handle multiple open and closes, so don't bother cloning</remarks>
        public IEntityGateway Clone() => this;

        /// <summary>
        /// Get the entity with the given key
        /// </summary>
        public EntityBase2 GetEntityFromKey(long entityID)
        {
            if (cache.Contains(entityID))
            {
                return cache[entityID];
            }

            var factory = new QueryFactory();

            var shipmentQuery = factory.ProcessedShipment
                .Where(ProcessedShipmentFields.ShipmentID == entityID);

            var entity = Using(
                sqlAdapterFactory.Create(),
                x => x.FetchFirst(shipmentQuery));

            cache[entityID] = entity;

            return entity;
        }

        /// <summary>
        /// Get entity from row
        /// </summary>
        public EntityBase2 GetEntityFromRow(int row, TimeSpan? timeout)
        {
            Stopwatch timer = Stopwatch.StartNew();

            if (filteredList.Length < row)
            {
                return null;
            }

            var shipmentID = filteredList[row].ShipmentID;
            if (cache.Contains(shipmentID))
            {
                return cache[shipmentID];
            }

            var page = pager.PageForRow(row);

            var entityPage = EntityGatewayUtility.ExecuteWithTimeout(timeout, timer, () =>
            {
                var shipmentIDs = page
                    .EnumerateRows()
                    .TakeWhile(x => x < filteredList.Length)
                    .Select(x => filteredList[x].ShipmentID)
                    .Where(x => !cache.Contains(x));

                var factory = new QueryFactory();

                var shipmentQuery = factory.ProcessedShipment
                    .Where(ProcessedShipmentFields.ShipmentID.In(shipmentIDs.ToArray()));

                return Using(sqlAdapterFactory.Create(), x => x.FetchQuery(shipmentQuery))
                    .OfType<ProcessedShipmentEntity>()
                    .ToList();
            });

            return entityPage?
                .Do(x => cache[x.ShipmentID] = x)
                .FirstOrDefault(e => e.ShipmentID == shipmentID);
        }

        /// <summary>
        /// Get the entity key for the given row
        /// </summary>
        public long? GetKeyFromRow(int row) => shipmentHeaders[row].ShipmentID;

        /// <summary>
        /// Get the keys in the sorted order
        /// </summary>
        public IEnumerable<long> GetOrderedKeys() => shipmentHeaders.Select(x => x.ShipmentID);

        /// <summary>
        /// Get the number of rows to display
        /// </summary>
        public PagedRowCount GetRowCount() =>
            fetchDataTask?.IsCompleted == true ?
                new PagedRowCount(filteredList.Length, true) :
                new PagedRowCount(0, false);
    }
}
