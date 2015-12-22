using System;
using System.Diagnostics;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.ApplicationCore.Enums;
using ShipWorks.ApplicationCore.Services;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Utility;
using ShipWorks.Email;
using ShipWorks.Email.Accounts;
using ShipWorks.FileTransfer;
using ShipWorks.Filters;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Settings.Printing;
using ShipWorks.Stores;
using ShipWorks.Stores.Communication;
using ShipWorks.Templates;
using ShipWorks.Templates.Media;
using ShipWorks.Users;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Shipping;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Manages the Heartbeat. Heartbeat controls reloading cache.
    /// </summary>
    public class Heartbeat
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Heartbeat));

        // Heartbeat standards
        static readonly TimeSpan minimumWait = TimeSpan.FromSeconds(.5);
        static readonly int normalRate = (int) TimeSpan.FromSeconds(15).TotalMilliseconds;
        static readonly int fastRate = (int) TimeSpan.FromSeconds(1).TotalMilliseconds;

        // So we don't have to do all of the heartbeat if nothing has changed
        readonly TimestampTracker timestampTracker = new TimestampTracker();
        bool changeProcessingPending;
        DateTime lastHeartbeatTime;
        private int storesChangeVersion = -1;

        // Current heartbeat rate
        int currentRate = normalRate;

        // If we are in a forced heartbeat that caused the heart rate to change, this is how many fast beats are left
        int forcedFastRatesStart = 10;
        int forcedFastRatesLeft = 0;

        // Tracks if we are in a heartbeat, so we don't beat twice at the same time
        object doingHeartbeatLock = new object();
        bool doingHeartbeat = false;

        // Indicates that we are already waiting for a heartbeat to be invoked to the UI thread
        HeartbeatOptions? pendingOptions = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Heartbeat"/> class.
        /// </summary>
        public Heartbeat()
        {
            Reset();
        }

        /// <summary>
        /// Stop the heartbeat
        /// </summary>
        public void Stop()
        {
            Pace = HeartbeatPace.Stopped;
        }

        /// <summary>
        /// Start the heartbeat
        /// </summary>
        public void Start()
        {
            Pace = HeartbeatPace.Normal;

            ForceHeartbeat(HeartbeatOptions.None);
        }

        /// <summary>
        /// Reset the heartbeat back to its initial state
        /// </summary>
        public void Reset()
        {
            lastHeartbeatTime = DateTime.MinValue;

            // Reset timestamp tracking for the heartbeat
            timestampTracker.Reset();
        }

        /// <summary>
        /// Forces a heartbeat.  Not necessary to call, as the pacemaker will keep it pumping without this.  This just makes it happen instantly, 
        /// and also speeds it up if its expecting changes.
        /// </summary>
        public void ForceHeartbeat(HeartbeatOptions options)
        {
            if (Pace != HeartbeatPace.Stopped)
            {
                // If changes are expected, we increase the heartrate.  This is for things like picking up filter counts more quickly.
                if ((options & HeartbeatOptions.ChangesExpected) != 0)
                {
                    log.InfoFormat("Increasing heart rate");

                    Pace = HeartbeatPace.Fast;
                }

                DoHeartbeat(options);
            }
        }

        /// <summary>
        /// Do a single heartbeat.
        /// </summary>
        protected void DoHeartbeat(HeartbeatOptions options)
        {
            // Prevent two heartbeats happening at once
            lock (doingHeartbeatLock)
            {
                // If we are already doing a hearbeat, we need to save the options the user wanted, so we can use them when we get around to the next hearbeat
                if (doingHeartbeat)
                {
                    if (pendingOptions == null)
                    {
                        pendingOptions = HeartbeatOptions.None;
                    }

                    pendingOptions |= options;

                    // Just get out for now
                    return;
                }

                doingHeartbeat = true;
            }

            // If there were any pending options, be sure to consider them too
            options = options | (pendingOptions ?? HeartbeatOptions.None);
            pendingOptions = null;

            try
            {
                InternalDoHeartbeat(options);
            }
            finally
            {
                lock (doingHeartbeatLock)
                {
                    doingHeartbeat = false;
                }
            }
        }

        /// <summary>
        /// Private version of the heartbeat that's wrapped in a check to make sure it can't be called at the same time from two different threads
        /// </summary>
        private void InternalDoHeartbeat(HeartbeatOptions options)
        {
            if (!CanBeat())
            {
                return;
            }

            // Extract heartbeat options
            bool forceReload = (options & HeartbeatOptions.ForceReload) != 0;

            // Check our pace - we may be going to fast and need to bail
            if (!CheckPace(forceReload))
            {
                return;
            }

            ApplicationBusyToken operationToken = null;

            // We have to make sure that the connection doesn't get swapped out from underneath use while we are beating.  Not an issue on the UI thread, where this is all on the UI thread, but
            // it is an issue in the background service.
            if (!ApplicationBusyManager.TryOperationStarting("refreshing", out operationToken))
            {
                return;
            }

            try
            {
                // If somethign changed that makes it so we can't run (like the user is now logged out), then get out
                if (!CanBeat())
                {
                    return;
                }

                // Debugging \ Logging
                Stopwatch sw = Stopwatch.StartNew();
                long connections = ConnectionMonitor.TotalConnectionCount;

                // Check for any change in the database. 
                bool changesDetected = timestampTracker.CheckForChange();
                log.InfoFormat("[Heartbeat] Starting (Changes: {0})", changesDetected);

                DownloadManager.StartAutoDownloadIfNeeded();
                EmailCommunicator.StartAutoEmailingIfNeeded();

                // If there was a heartbeat change detected, kick off actions and set the flag to process heartbeat changes
                if (changesDetected)
                {
                    // Changes and filters trigger actions to run, so any time there is a change, we need to check for actions.
                    ActionProcessor.StartProcessing();

                    // This flag stays true until section below sees it and resets to false.  The section in question only 
                    // runs when there are no modal windows or popups open.  This flag has to stay true until that section 
                    // runs to ensure changes are not missed.
                    changeProcessingPending = true;
                }

                // Not dependant on DBTS, we need to make sure any filters that are affected by date changes are updated
                FilterContentManager.CheckRelativeDateFilters();

                // Make sure all our counts are up-to-date
                FilterContentManager.CheckForChanges();

                // Time to process the heartbeat
                ProcessHeartbeat(changesDetected, forceReload);

                // Finalize this heartbeat
                FinishHeartbeat();

                // Logging
                log.DebugFormat("[Heartbeat] Finished. ({0}), Connections: {1}, Interval: {2}s", sw.Elapsed.TotalSeconds, ConnectionMonitor.TotalConnectionCount - connections, currentRate / 1000);
            }
            finally
            {
                ApplicationBusyManager.OperationComplete(operationToken);
            }
        }

        /// <summary>
        /// Determines if the heartbeat is ready to beat
        /// </summary>
        protected virtual bool CanBeat()
        {
            if (Pace == HeartbeatPace.Stopped)
            {
                return false;
            }

            // Can't be within a connection sensitive scope.  If the connection might be changed, we can't be kicking off stuff
            // and using the connection.
            if (ConnectionSensitiveScope.IsActive)
            {
                return false;
            }

            // If we don't have a user, then we have to get out
            if (!UserSession.IsLoggedOn)
            {
                return false;
            }

            // Check for pending background DB reconnects
            ConnectionMonitor.VerifyConnected();

            // Make sure we are not in a failure state
            if (ConnectionMonitor.Status != ConnectionMonitorStatus.Normal || CrashWindow.IsApplicationCrashed)
            {
                return false;
            }

            // Don't update while in the middle of a store deletion
            if (DeletionService.IsDeletingStore)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check the pace of the hearbeat.  It may need adjusted, or we may need to not beat at all due to going to fast.
        /// </summary>
        private bool CheckPace(bool forceReload)
        {
            // If its beating too fast, then skip it, we don't want to update too often.
            TimeSpan timeSinceLastHeartbeat = DateTime.UtcNow - lastHeartbeatTime;
            if (timeSinceLastHeartbeat < minimumWait && !forceReload)
            {
                // Increase the heartrate for one beat (if its not fast already) so we get back in here quickly.
                Pace = HeartbeatPace.SingleFast;

                return false;
            }

            // See if we are in a state of forced fast heart rate
            if (forcedFastRatesLeft > 0)
            {
                // Done with the forced fast rate
                if (--forcedFastRatesLeft == 0)
                {
                    log.DebugFormat("[Heartbeat] Canceling fast heart rate (time out)");
                    Pace = HeartbeatPace.Normal;
                }
            }

            log.DebugFormat("[Heartbeat] Time since last: {0}s", timeSinceLastHeartbeat.TotalSeconds);

            return true;
        }

        /// <summary>
        /// Sets the pace of the heartbeat
        /// </summary>
        public HeartbeatPace Pace
        {
            get
            {
                if (currentRate == 0)
                {
                    return HeartbeatPace.Stopped;
                }

                return (currentRate == fastRate) ?
                    HeartbeatPace.Fast :
                    HeartbeatPace.Normal;
            }
            set
            {
                switch (value)
                {
                    case HeartbeatPace.Stopped:
                        {
                            currentRate = 0;
                        }
                        break;

                    case HeartbeatPace.Fast:
                        {
                            // When we go to a fast beat, we keep it there for a wihle
                            forcedFastRatesLeft = forcedFastRatesStart;

                            currentRate = fastRate;
                        }
                        break;

                    case HeartbeatPace.SingleFast:
                        {
                            // If the pace is already fast, and has beats left, we don't change anything.
                            if (Pace != HeartbeatPace.Fast || forcedFastRatesLeft == 0)
                            {
                                forcedFastRatesLeft = 1;
                            }

                            currentRate = fastRate;
                        }
                        break;

                    case HeartbeatPace.Normal:
                        {
                            currentRate = normalRate;
                        }
                        break;
                }

                UpdatePacemaker(currentRate);
            }
        }

        /// <summary>
        /// Update the pacemaker to the given pace
        /// </summary>
        protected virtual void UpdatePacemaker(int pace)
        {

        }

        /// <summary>
        /// Runs the actual heartbeat. 
        /// </summary>
        [NDependIgnoreLongMethod]
        protected virtual void ProcessHeartbeat(bool changesDetected, bool forceReload)
        {
            bool storesChanged = false;

            // We only do all this checking if there was a dbts change.
            //
            // IMPORTANT: 
            //
            // This means that all the stuff in this section must be dependant at some level on a timestamp column.
            // If some type of change checking is not 1. Then it probably should be 2. If it can't be, then it cant be excluded
            // by the timestamp tracking change detection.
            //
            if (changeProcessingPending)
            {
                log.DebugFormat("[Heartbeat] Processing timestamp change");

                // Depending on what changes, we may need to force the grid to redraw
                StoreManager.CheckForChanges();
                if (storesChangeVersion != StoreManager.ChangeVersion)
                {
                    storesChangeVersion = StoreManager.ChangeVersion;
                    storesChanged = true;
                }

                // These just mark that changes need to be checked next time data is requested
                TemplateManager.CheckForChangesNeeded();
                SystemData.CheckForChangesNeeded();
                ConfigurationData.CheckForChangesNeeded();
                ShippingSettings.CheckForChangesNeeded();
                LabelSheetManager.CheckForChangesNeeded();
                EmailAccountManager.CheckForChangesNeeded();
                FtpAccountManager.CheckForChangesNeeded();
                ComputerManager.CheckForChangesNeeded();
                UserManager.CheckForChangesNeeded();
                ActionManager.CheckForChangesNeeded();
                ShippingOriginManager.CheckForChangesNeeded();
                UspsAccountManager.CheckForChangesNeeded();
                EndiciaAccountManager.CheckForChangesNeeded();
                DimensionsManager.CheckForChangesNeeded();
                ShippingProfileManager.CheckForChangesNeeded();
                FedExAccountManager.CheckForChangesNeeded();
                UpsAccountManager.CheckForChangesNeeded();
                OnTracAccountManager.CheckForChangesNeeded();
                iParcelAccountManager.CheckForChangesNeeded();
                ShippingDefaultsRuleManager.CheckForChangesNeeded();
                ShippingPrintOutputManager.CheckForChangesNeeded();
                ShippingProviderRuleManager.CheckForChangesNeeded();
                ServiceStatusManager.CheckForChangesNeeded();

                // Check for any WorldShip shipments that need imported
                WorldShipImportMonitor.CheckForShipments();

                // Check for any server messages that need to be put up in the dashboard.  This does not check the server - just
                // what's already been stored in the database, so it's done after the various CheckForChangesNeeded calls above
                // to ensure the latest data is being used.
                DashboardManager.CheckForChanges();
            }

            // Give any derived versions a chance to update the display of any changed data
            RespondToChanges(changeProcessingPending, forceReload, storesChanged);

            // We've now processed the pending change
            changeProcessingPending = false;
        }

        /// <summary>
        /// Give any derived versions respond to changes that have happened in this heartbeat
        /// </summary>
        protected virtual void RespondToChanges(bool hadChanges, bool forceReload, bool storesChanged)
        {

        }

        /// <summary>
        /// Finish the heartbeat.  Derived class should call this at the end of their overriden function
        /// </summary>
        protected virtual void FinishHeartbeat()
        {
            // Save the last heartbeat time from the end of the heartbeat.  That way if it takes a while to process, we don't overlap.
            lastHeartbeatTime = DateTime.UtcNow;
        }
    }
}