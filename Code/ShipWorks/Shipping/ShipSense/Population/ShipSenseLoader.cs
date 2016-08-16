using System;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Shipping.ShipSense.Population
{
    /// <summary>
    /// Loads the ShipSenseKnowledgebase based on shipment history
    /// </summary>
    public class ShipSenseLoader : IDisposable
    {
        private const string AppLockName = "ShipSenseLoader_Working";
        private static readonly ILog log = LogManager.GetLogger(typeof(ShipSenseLoader));

        private static object runningLock = new object();

        private IShipSenseLoaderGateway shipSenseLoaderGateway;
        private readonly IProgressReporter progressReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseLoader" /> class.
        /// </summary>
        /// <param name="progressReporter">The progress reporter.</param>
        public ShipSenseLoader(IProgressReporter progressReporter)
            : this(progressReporter, new ShipSenseLoaderGateway(new Knowledgebase()))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseLoader" /> class.
        /// </summary>
        /// <param name="progressReporter">The progress reporter.</param>
        /// <param name="shipSenseLoaderGateway">The ship sense loader gateway.</param>
        public ShipSenseLoader(IProgressReporter progressReporter, IShipSenseLoaderGateway shipSenseLoaderGateway)
        {
            this.progressReporter = progressReporter;
            this.shipSenseLoaderGateway = shipSenseLoaderGateway;
            ResetOrderHashKeys = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [reset order hash keys] as part of the load process.
        /// </summary>
        public bool ResetOrderHashKeys { get; set; }

        /// <summary>
        /// Does some precursory work prior to (re)loading the ShipSense knowledge base.
        /// </summary>
        public void PrepareForLoading()
        {
            shipSenseLoaderGateway.ResetShippingSettingsForLoading();
        }

        /// <summary>
        /// Starts loading order and ShipSense data on a new thread.
        /// </summary>
        public static void LoadDataAsync()
        {
            LoadDataAsync(new ProgressItem("Reloading ShipSense"), false);
        }

        /// <summary>
        /// Starts loading order and ShipSense data on a new thread.
        /// </summary>
        /// <param name="progressReporter">The progress reporter.</param>
        /// <param name="resetOrderHashKeys">if set to <c>true</c> this reset all the order hash keys and reload them.</param>
        public static void LoadDataAsync(IProgressReporter progressReporter, bool resetOrderHashKeys)
        {
            try
            {
                ShipSenseLoader shipSenseLoader = new ShipSenseLoader(progressReporter);
                shipSenseLoader.ResetOrderHashKeys = resetOrderHashKeys;

                // We used to have this StartNew in a new ShipSenseLoaderGateway using block, but that doesn't make sense
                // because we were in a using, starting a new thread that could take a long time, but immediately leaving the
                // using block.  This was causing sql app locking issues, so removed the using block and added a Dispose
                // method to ShipSenseLoader that now disposes the gateway.  The ContinueWith now calls this loader dispose method.
                Task.Factory.StartNew(shipSenseLoader.LoadData).ContinueWith(t => shipSenseLoader.Dispose());
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
            }
        }

        /// <summary>
        /// Populates the ShipSenseKnowledgebase based on shipment history
        /// </summary>
        public void LoadData()
        {
            lock (runningLock)
            {
                if (shipSenseLoaderGateway.GetAppLock(AppLockName))
                {
                    try
                    {
                        progressReporter.Starting();
                        UpdateProgress("Updating orders...", 0);

                        if (ResetOrderHashKeys)
                        {
                            shipSenseLoaderGateway.ResetOrderHashKeys();
                        }

                        // Re-calculate the ShipSense hash key for orders that are eligible
                        // and add entries to the ShipSense knowledge base for the orders
                        // that have processed shipment.
                        UpdateOrderHashes();

                        UpdateProgress("Analyzing shipment history...", 0);
                        AddKnowledgebaseEntries();
                    }
                    catch (Exception e)
                    {
                        log.ErrorFormat(e.Message, e);
                    }
                    finally
                    {
                        shipSenseLoaderGateway.ReleaseAppLock(AppLockName);

                        UpdateProgress("Done", 100);
                        progressReporter.Completed();
                    }
                }
                else
                {
                    log.DebugFormat("Unable to get applock ShipSenseLoader_Working");
                }
            }
        }

        /// <summary>
        /// Populates the ShipSenseHashKey of orders that have a blank ShipSenseHashKey
        /// </summary>
        private void UpdateOrderHashes()
        {
            int totalOrdersToAnalyze = shipSenseLoaderGateway.TotalOrdersToAnalyze;
            int ordersAnalyzed = 0;

            using (LoggedStopwatch stopwatch = new LoggedStopwatch(log, "OrderHashes"))
            {
                OrderEntity order = shipSenseLoaderGateway.FetchNextOrderToAnalyze();

                while (order != null && !progressReporter.IsCancelRequested)
                {
                    // Update the hash key and set the recognition status to NotRecognized; the second part of the
                    // loader will update the status to Recognized if an KB entry gets added that matches the hash.
                    // This avoid situations where we have orders with a hash key value but still having a status
                    // of NotApplicable during uploads
                    OrderUtility.UpdateShipSenseHashKey(order);
                    order.ShipSenseRecognitionStatus = (int) ShipSenseOrderRecognitionStatus.NotRecognized;
                    shipSenseLoaderGateway.SaveOrder(order);

                    ordersAnalyzed++;
                    UpdateProgress(progressReporter.Detail, ordersAnalyzed * 100 / totalOrdersToAnalyze);

                    order = shipSenseLoaderGateway.FetchNextOrderToAnalyze();
                }
            }
        }

        /// <summary>
        /// Adds a knowledge base entry for each unique order that has processed shipments
        /// </summary>
        private void AddKnowledgebaseEntries()
        {
            int shipmentsAnalyzed = 0;
            int totalShipmentsToProcess = shipSenseLoaderGateway.TotalShipmentsToAnalyze;

            using (LoggedStopwatch stopwatch = new LoggedStopwatch(log, "KB Entries"))
            {
                ShipmentEntity shipment = shipSenseLoaderGateway.FetchNextShipmentToAnalyze();
                while (shipment != null && !progressReporter.IsCancelRequested)
                {
                    // Save the shipment data to the knowledge base
                    ShippingManager.EnsureShipmentLoaded(shipment);
                    shipSenseLoaderGateway.Save(shipment);

                    // Update the progress
                    shipmentsAnalyzed++;
                    UpdateProgress(progressReporter.Detail, shipmentsAnalyzed * 100 / totalShipmentsToProcess);

                    // Grab the next shipment
                    shipment = shipSenseLoaderGateway.FetchNextShipmentToAnalyze();
                }
            }

            log.InfoFormat("Used {0} shipments to load knowledge base.", shipmentsAnalyzed);
        }

        /// <summary>
        /// A helper method to update the progress reporter.
        /// </summary>
        /// <param name="detail">The detail.</param>
        /// <param name="percentComplete">The percent complete.</param>
        private void UpdateProgress(string detail, int percentComplete)
        {
            if (progressReporter.IsCancelRequested)
            {
                progressReporter.Detail = "Resetting ShipSense has been canceled.";
            }
            else
            {
                progressReporter.PercentComplete = Math.Min(percentComplete, 100);
                progressReporter.Detail = detail;
            }
        }

        /// <summary>
        /// Dispose of any resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of any resources
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                shipSenseLoaderGateway.Dispose();

                shipSenseLoaderGateway = null;
            }
        }
    }
}
