using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using System.Threading;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Manage synchronization of products on the Hub
    /// </summary>
    [Component(SingleInstance = true)]
    public class WarehouseProductSynchronizationManager : IInitializeForCurrentUISession
    {
        public const string ProductSyncIntervalBasePath = @"Software\Interapptive\ShipWorks\Options";
        private static readonly TimeSpan Never = TimeSpan.FromMilliseconds(-1);
        private readonly object lockObj = new object();
        private readonly Timer timer;
        private readonly RegistryHelper registryHelper;
        private readonly ILog log;
        private readonly IWarehouseProductSynchronizer productSynchronizer;
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseProductSynchronizationManager(
            IWarehouseProductSynchronizer productSynchronizer,
            Func<Type, ILog> createLog)
        {
            log = createLog(GetType());
            this.productSynchronizer = productSynchronizer;
            registryHelper = new RegistryHelper(ProductSyncIntervalBasePath);
            timer = new Timer(HandleTimerTick);
        }

        /// <summary>
        /// Interval for syncing products
        /// </summary>
        private TimeSpan ProductSyncInterval =>
            TimeSpan.FromSeconds(registryHelper.GetValue("ProductSyncIntervalValueInSeconds", 600));

        /// <summary>
        /// Dispose the instance
        /// </summary>
        public void Dispose()
        {
            EndSession();
            timer.Dispose();
        }

        /// <summary>
        /// End the current UI session
        /// </summary>
        public void EndSession()
        {
            lock (lockObj)
            {
                timer.Change(Never, Never);
                cancellationTokenSource?.Cancel();
            }
        }

        /// <summary>
        /// Initialize the synchronizer for this session
        /// </summary>
        public void InitializeForCurrentSession() =>
            timer.Change(ProductSyncInterval, Never);

        /// <summary>
        /// Handle the timer tick event
        /// </summary>
        /// <param name="state"></param>
        [SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"",
            Justification = "This is an event handler for a timer tick")]
        private async void HandleTimerTick(object state)
        {
            using (CreateCancellationToken())
            {
                try
                {
                    await productSynchronizer.Synchronize(cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    log.Error("Error while syncing products", ex);
                }
                finally
                {
                    lock (lockObj)
                    {
                        if (!cancellationTokenSource.IsCancellationRequested)
                        {
                            timer.Change(ProductSyncInterval, Never);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create a cancellation token that will be dereferenced when disposed
        /// </summary>
        private IDisposable CreateCancellationToken()
        {
            cancellationTokenSource = new CancellationTokenSource();

            return Disposable.Create(() =>
            {
                var source = cancellationTokenSource;
                cancellationTokenSource = null;  // Ensure nothing else can use the token source before it's disposed
                source.Dispose();
            });
        }
    }
}
