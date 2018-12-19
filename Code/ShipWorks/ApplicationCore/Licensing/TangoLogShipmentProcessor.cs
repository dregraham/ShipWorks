﻿using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using Interapptive.Shared.Data;
using log4net;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.ApplicationCore.Licensing.TangoRequests;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Class that logs shipments to tango.  Recovers from failures as well.
    /// </summary>
    [Order(typeof(IInitializeForCurrentUISession), Order.Unordered)]
    [Component(SingleInstance = true)]
    public class TangoLogShipmentProcessor : IInitializeForCurrentUISession, ITangoLogShipmentProcessor, IDisposable
    {
        private const string AppLockName = "TangoLogShipmentProcessorRunning";
        private static readonly ILog log = LogManager.GetLogger(typeof(TangoLogShipmentProcessor));
        private static readonly BlockingCollection<(StoreEntity Store, ShipmentEntity Shipment)> shipmentsToLog;
        private static readonly int runInterval = 1 * 60 * 1000;
        private readonly ISqlSession sqlSession;
        private readonly IShippingManager shippingManager;
        private readonly IStoreManager storeManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly ITangoLogShipmentRequest tangoWebClient;
        private readonly ISqlAppLock sqlAppLock;
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        /// <summary>
        /// Static constructor
        /// </summary>
        static TangoLogShipmentProcessor()
        {
            shipmentsToLog = new BlockingCollection<(StoreEntity Store, ShipmentEntity Shipment)>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TangoLogShipmentProcessor(
            ITangoLogShipmentRequest tangoWebClient,
            ISqlSession sqlSession,
            IShippingManager shippingManager,
            IStoreManager storeManager,
            ISqlAdapterFactory sqlAdapterFactory,
            ISqlAppLock sqlAppLock)
        {
            this.sqlAppLock = sqlAppLock;
            this.sqlSession = sqlSession;
            this.shippingManager = shippingManager;
            this.storeManager = storeManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.tangoWebClient = tangoWebClient;
        }

        /// <summary>
        /// Start the processing queue now that the SQL Session is running
        /// </summary>
        public void InitializeForCurrentSession()
        {
            if (cancellationTokenSource == null || cancellationToken.IsCancellationRequested)
            {
                cancellationTokenSource = new CancellationTokenSource();
                cancellationToken = cancellationTokenSource.Token;
            }

            Task.Run(() => Run().ConfigureAwait(false), cancellationToken);
        }

        /// <summary>
        /// End the session to pause shipment logging
        /// </summary>
        public void EndSession()
        {
            Dispose();
        }

        /// <summary>
        /// Start the thread to process the queue periodically.
        /// </summary>
        private async Task Run()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Process().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.Error("Error while Processing", ex);
                }

                await Task.Delay(runInterval, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Process any shipments that need lo
        /// </summary>
        private async Task Process()
        {
            if (sqlSession == null)
            {
                return;
            }

            using (DbConnection connection = sqlSession.OpenConnection())
            {
                using (IDisposable sqlLock = sqlAppLock.Take(connection, AppLockName, TimeSpan.FromMilliseconds(10)))
                {
                    if (sqlLock == null)
                    {
                        return;
                    }

                    try
                    {
                        do
                        {
                            LogShipmentsToTango(connection);

                            // Get the next page
                            await AddFailedShipments(connection).ConfigureAwait(false);

                        } while (shipmentsToLog.Any() && !cancellationToken.IsCancellationRequested);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error while Processing", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Add a log shipment task to the queue to process
        /// </summary>
        public void Add(StoreEntity store, ShipmentEntity shipment)
        {
            Add((store, shipment));
        }

        /// <summary>
        /// Add a log shipment task to the queue to process
        /// </summary>
        private static void Add((StoreEntity Store, ShipmentEntity Shipment) shipmentToLog)
        {
            // Add the shipment to the list.  If it fails, we'll process it during recovery.
            shipmentsToLog.TryAdd(shipmentToLog);
        }

        /// <summary>
        /// Process the queue
        /// </summary>
        private void LogShipmentsToTango(DbConnection connection)
        {
            // Process each task synchronously
            while (shipmentsToLog.TryTake(out (StoreEntity Store, ShipmentEntity Shipment) shipmentToLog))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                tangoWebClient.LogShipment(connection, shipmentToLog.Store, shipmentToLog.Shipment)
                    .Do(() => log.InfoFormat("Logged shipment {0}", shipmentToLog.Shipment.ShipmentID))
                    .OnFailure(ex => LogException(ex, shipmentToLog.Shipment.ShipmentID));
            }
        }

        /// <summary>
        /// Log an exception raised while processing
        /// </summary>
        private void LogException(Exception ex, long shipmentID)
        {
            // If a cancellation is requested while processing, a sql exception will most likely be thrown
            // so just ignore
            if (!cancellationToken.IsCancellationRequested)
            {
                // We want to continue to the next shipment to log, so just put a message in the log file.
                log.Error($"Logging shipment {shipmentID} FAILED.", ex);
            }
        }

        /// <summary>
        /// Add shipments that need logged to the list
        /// </summary>
        private async Task AddFailedShipments(DbConnection connection)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create(connection))
            {
                var query = new QueryFactory().Shipment
                    .Where(ShipmentFields.OnlineShipmentID.IsNull().Or(ShipmentFields.OnlineShipmentID == string.Empty))
                    .AndWhere(ShipmentFields.Processed == true)
                    .AndWhere(ShipmentFields.Voided == false)
                    .WithPath(ShipmentEntity.PrefetchPathOrder)
                    .OrderBy(ShipmentFields.ProcessedDate.Descending())
                    .Limit(20);

                EntityCollection<ShipmentEntity> shipmentCollection = new EntityCollection<ShipmentEntity>();

                await sqlAdapter.FetchQueryAsync(query, shipmentCollection).ConfigureAwait(false);

                foreach (ShipmentEntity shipment in shipmentCollection)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    shippingManager.EnsureShipmentLoaded(shipment);

                    if (shipment.Order.Store == null)
                    {
                        shipment.Order.Store = storeManager.GetStore(shipment.Order.StoreID);
                    }

                    Add(shipment.Order.Store, shipment);
                }
            }
        }

        /// <summary>
        /// Top the processing
        /// </summary>
        public void Dispose()
        {
            cancellationTokenSource.Cancel(true);
        }
    }
}
