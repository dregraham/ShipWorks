using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using log4net;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Manage synchronization of products on the Hub
    /// </summary>
    public class WarehouseProductSynchronizationManager : IInitializeForCurrentUISession
    {
        private static readonly TimeSpan Never = TimeSpan.FromMilliseconds(-1);
        private readonly Timer timer;
        private readonly ILog log;
        private readonly IWarehouseProductSynchronizer productSynchronizer;
        private CancellationTokenSource cancellationTokenSource;
        private bool isDisposed;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseProductSynchronizationManager(
            IWarehouseProductSynchronizer productSynchronizer,
            Func<Type, ILog> createLog)
        {
            log = createLog(GetType());
            this.productSynchronizer = productSynchronizer;
            timer = new Timer(HandleTimerTick);
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
            timer.Change(Never, Never);
        }

        /// <summary>
        /// Initialize the synchronizer for this session
        /// </summary>
        public void InitializeForCurrentSession() =>
            timer.Change(TimeSpan.FromMinutes(10), Never);

        /// <summary>
        /// Handle the timer tick event
        /// </summary>
        /// <param name="state"></param>
        [SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"", 
            Justification = "This is an event handler for a timer tick")]
        private async void HandleTimerTick(object state)
        {
            using (cancellationTokenSource = new CancellationTokenSource())
            {
                try
                {
                    await productSynchronizer.Synchronize(cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    log.Error("Error while syncing products", ex);

                    if (!cancellationTokenSource.IsCancellationRequested || !isDisposed)
                    {
                        timer.Change(TimeSpan.FromMinutes(10), TimeSpan.Zero);
                    }
                }
            }
        }
    }
}
