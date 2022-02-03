using System;
using System.Data.Common;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Data;
using log4net;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// A process to be run by only one process at once. Uses an Applock to make sure
    /// no other computer runs this task.
    /// </summary>
    public abstract class ReoccurringAppLockedTask : IDisposable
    {
        private readonly ISqlSession sqlSession;
        private readonly ISqlAppLock sqlAppLock;
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
        private TaskCompletionSource<Unit> delayTaskCompletionSource;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ReoccurringAppLockedTask( 
            Func<Type, ILog> logFactory, 
            ISqlSession sqlSession,
            ISqlAppLock sqlAppLock)
        {
            this.sqlSession = sqlSession;
            this.sqlAppLock = sqlAppLock;
            Log = logFactory(GetType());
        }
        
        protected ILog Log { get; }
        
        /// <summary>
        /// How often should this run?
        /// </summary>
        protected abstract int RunInterval { get; }
        
        /// <summary>
        /// The name of the lock taken. Must be unique!
        /// </summary>
        protected abstract string AppLockName { get; }
        
        /// <summary>
        /// The thing that runs defined in the subclass 
        /// </summary>
        protected abstract Task ProcessTask(CancellationToken token);
        
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
                    Log.Error("Error while Processing", ex);
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
        private async Task Process()
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
                        await ProcessTask(cancellationToken).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error while Processing", ex);
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