using System;
using System.Data.Common;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration.Ordering;
using Interapptive.Shared.Data;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;

namespace ShipWorks.Shipping.Tracking
{
    [Order(typeof(IInitializeForCurrentUISession), Order.Unordered)]
    public class PlatformShipmentTrackerCoordinator : IInitializeForCurrentUISession, IDisposable
    {
        private readonly ISqlSession sqlSession;
        private readonly ISqlAppLock sqlAppLock;

        private const string AppLockName = "PlatformShipmentTrackerRunning";
        private const int RunInterval = 60 * 1000;
        
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
        private readonly ILog log;
        
        private TaskCompletionSource<Unit> delayTaskCompletionSource;

        public PlatformShipmentTrackerCoordinator(
            Func<Type, ILog> logFactory, 
            ISqlSession sqlSession,
            ISqlAppLock sqlAppLock)
        {
            this.sqlSession = sqlSession;
            this.sqlAppLock = sqlAppLock;
            log = logFactory(GetType());
        }
        
        /// <summary>
        /// Initialize for the current session
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
        /// End the current session
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

                delayTaskCompletionSource = new TaskCompletionSource<Unit>();
                await Task.WhenAny(Task.Delay(RunInterval, cancellationToken), delayTaskCompletionSource.Task)
                    .ConfigureAwait(false);
                delayTaskCompletionSource = null;
            }
        }
        
        /// <summary>
        /// Process any shipments that need lo
        /// </summary>
        public async Task Process()
        {
            if (sqlSession == null || ConnectionSensitiveScope.IsActive)
            {
                return;
            }

            using (DbConnection connection = sqlSession.OpenConnection())
            {
                using (sqlAppLock.Take(connection, AppLockName, TimeSpan.FromMilliseconds(10)))
                {
                    if (!sqlAppLock.LockAcquired)
                    {
                        return;
                    }

                    try
                    {
                        
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error while Processing", ex);
                    }
                }
            }
        }
        
        /// <summary>
        /// Stop the processing
        /// </summary>
        public void Dispose()
        {
            cancellationTokenSource?.Cancel(true);
        }
    }
}