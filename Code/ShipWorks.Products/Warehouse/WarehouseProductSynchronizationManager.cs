using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Manage synchronization of products on the Hub
    /// </summary>
    public class WarehouseProductSynchronizationManager : IInitializeForCurrentUISession
    {
        public const string ProductSyncIntervalBasePath = @"Software\Interapptive\ShipWorks\Options";
        private readonly System.Timers.Timer timer;
        private readonly ILog log;
        private readonly IWarehouseProductSynchronizer productSynchronizer;
        private CancellationTokenSource cancellationTokenSource;
        private bool isDisposed;
        private bool isRunning;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseProductSynchronizationManager(
            IWarehouseProductSynchronizer productSynchronizer,
            Func<Type, ILog> createLog)
        {
            log = createLog(GetType());
            this.productSynchronizer = productSynchronizer;
            double productSyncIntervalValue = new RegistryHelper(ProductSyncIntervalBasePath).GetValue("ProductSyncIntervalValue", 10.0);
            
            timer = new System.Timers.Timer(TimeSpan.FromMinutes(productSyncIntervalValue).TotalMilliseconds);
            timer.Elapsed += HandleTimerTick;
        }

        /// <summary>
        /// Timer elapsed handler
        /// </summary>
        [SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"",
            Justification = "This is an event handler for a timer tick")]
        private async void HandleTimerTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (isRunning)
            {
                return;
            }

            isRunning = true;

            using (cancellationTokenSource = new CancellationTokenSource())
            {
                try
                {
                    await productSynchronizer.Synchronize(cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    log.Error("Error while syncing products", ex);
                }

                isRunning = false;
            }
        }

        /// <summary>
        /// Dispose the instance
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            EndSession();
            timer.Dispose();
        }

        /// <summary>
        /// End the current UI session
        /// </summary>
        public void EndSession()
        {
            timer.Elapsed -= HandleTimerTick;
            timer.Stop();
        }

        /// <summary>
        /// Initialize the synchronizer for this session
        /// </summary>
        public void InitializeForCurrentSession() =>
            timer.Start();
    }
}
