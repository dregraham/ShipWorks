using System;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration.Ordering;
using Interapptive.Shared.Data;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;

namespace ShipWorks.Shipping.Tracking
{
    /// <summary>
    /// Sends shipments to be tracked and imports notifications
    /// </summary>
    [Order(typeof(IInitializeForCurrentUISession), Order.Unordered)]
    public class PlatformShipmentTrackerCoordinator : ReoccurringAppLockedTask, IInitializeForCurrentUISession
    {
        private readonly IPlatformShipmentTracker tracker;

        public PlatformShipmentTrackerCoordinator(
            Func<Type, ILog> logFactory, 
            ISqlSession sqlSession,
            ISqlAppLock sqlAppLock,
            IPlatformShipmentTracker tracker) : base(logFactory, sqlSession, sqlAppLock)
        {
            this.tracker = tracker;
        }

        /// <summary>
        /// 2 minutes
        /// </summary>
        protected override int RunInterval => (int) TimeSpan.FromMinutes(2).TotalMilliseconds;
        
        /// <summary>
        /// The name of the lock taken. Must be unique!
        /// </summary>
        protected override string AppLockName => "PlatformShipmentTrackerRunning";

        /// <summary>
        /// The thing that runs defined in the subclass 
        /// </summary>
        protected override async Task ProcessTask(CancellationToken token)
        {
            Log.Info("Sending shipments to track.");
            await tracker.TrackShipments(token).ConfigureAwait(false);
            
            Log.Info("Process tracking notifications");
            await tracker.PopulateLatestTracking(token).ConfigureAwait(false);
        }
    }
}