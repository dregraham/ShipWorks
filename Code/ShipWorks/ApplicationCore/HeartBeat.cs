using System;
using System.Diagnostics;
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
using ShipWorks.Shipping.Carriers.Postal.Stamps;
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

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Manages the Heartbeat. Heartbeat controls reloading cache.
    /// </summary>
    public class Heartbeat
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Heartbeat));

        // Heartbeat standards
        public static readonly int HeartbeatFastRate = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;

        private readonly TimeSpan heartbeatMinimumWait = TimeSpan.FromSeconds(.5);
        private readonly int heartbeatNormalRate = (int)TimeSpan.FromSeconds(15).TotalMilliseconds;

        private readonly TimestampTracker heartbeatTimestampTracker = new TimestampTracker();
        private bool heartbeatChangeProcessingPending;
        private DateTime lastHeartbeatTime;

        private int storesChangeVersion = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Heartbeat"/> class.
        /// </summary>
        public Heartbeat()
        {
            lastHeartbeatTime = DateTime.MinValue;

            // Reset timestamp tracking for the heartbeat
            heartbeatTimestampTracker.Reset();

            HeartbeatForcedFastRatesStart = 10;
        }

        /// <summary>
        /// Gets or sets the heartbeat forced fast rates left.
        /// </summary>
        public int HeartbeatForcedFastRatesLeft { get; set; }

        /// <summary>
        /// Gets the heartbeat forced fast rates start.
        /// </summary>
        public int HeartbeatForcedFastRatesStart { get; private set; }

        /// <summary>
        /// Do a single heartbeat.  Should not be called directly - call ForceHeartbeat instead
        /// </summary>
        public void DoHeartbeat(HeartbeatOptions options)
        {
            if (!CanStart())
            {
                return;
            }

            // Check for pending background DB reconnects
            ConnectionMonitor.VerifyConnected();

            // Make sure we are not in a failure state
            if (ConnectionMonitor.Status != ConnectionMonitorStatus.Normal || CrashWindow.IsApplicationCrashed)
            {
                return;
            }

            // Can't be within a connection sensitive scope.  If the connection might be changed, we can't be kicking off stuff
            // and using the connection.
            if (ConnectionSensitiveScope.IsActive)
            {
                return;
            }

            // Shouldn't be able to get here while not logged in
            Debug.Assert(SqlSession.IsConfigured);
            Debug.Assert(UserSession.IsLoggedOn || !Program.ExecutionMode.IsUserInteractive);

            // Extract heartbeat options
            bool forceReload = (options & HeartbeatOptions.ForceReload) != 0;

            // If its beating too fast, then skip it, we don't want to update too often.
            TimeSpan timeSinceLastHeartbeat = DateTime.UtcNow - lastHeartbeatTime;
            if (timeSinceLastHeartbeat < heartbeatMinimumWait && !forceReload)
            {
                // Increase the heartrate for one beat (if its not fast already) so we get back in here quickly.
                if (HeartbeatForcedFastRatesLeft == 0)
                {
                    HeartbeatForcedFastRatesLeft = 1;
                    SetHeartbeatSpeed(HeartbeatFastRate);
                }

                return;
            }

            // Don't update while in the middle of a store deletion
            if (DeletionService.IsDeletingStore)
            {
                return;
            }

            // See if we are in a state of forced fast heart rate
            if (HeartbeatForcedFastRatesLeft > 0)
            {
                // Done with the forced fast rate
                if (--HeartbeatForcedFastRatesLeft == 0)
                {
                    log.DebugFormat("[Heartbeat] Canceling fast heart rate (time out)");
                    SetHeartbeatSpeed(heartbeatNormalRate);
                }
            }

            Stopwatch sw = Stopwatch.StartNew();

            // Check for any change in the database. 
            bool heartbeatChangeDetected = heartbeatTimestampTracker.CheckForChange();

            log.InfoFormat("[Heartbeat] Starting (Changes: {0}). Time since last: {1}", heartbeatChangeDetected, timeSinceLastHeartbeat.TotalSeconds);
            long connections = ConnectionMonitor.TotalConnectionCount;

            DownloadManager.StartAutoDownloadIfNeeded();
            EmailCommunicator.StartAutoEmailingIfNeeded();

            // If there was a heartbeat change detected, kick off actions and set the flag to process heartbeat changes
            if (heartbeatChangeDetected)
            {
                // Changes and filters trigger actions to run, so any time there is a change, we need to check for actions.
                ActionProcessor.StartProcessing();

                // This flag stays true until section below sees it and resets to false.  The section in question only 
                // runs when there are no modal windows or popups open.  This flag has to stay true until that section 
                // runs to ensure changes are not missed.
                heartbeatChangeProcessingPending = true;
            }

            // Not dependant on DBTS, we need to make sure any filters that are affected by date changes are updated
            FilterContentManager.CheckRelativeDateFilters();

            // Make sure all our counts are up-to-date
            FilterContentManager.CheckForChanges();

            // Detect if a modal window is open.  The popup test is, for now, to make sure we don't reload the 
            // grid columns on a filter layout change while the right-click grid column editor is open.
            if (IsProgramReadyForHeartbeat())
            {
                RunActualHeartbeat(forceReload);
            }
            else
            {
                RunProgramNotReadyLogic(heartbeatChangeDetected, forceReload);
            }

            AfterReload();

            // Save the last heartbeat time from the end of the heartbeat.  That way if it takes a while to process, we don't overlap.
            lastHeartbeatTime = DateTime.UtcNow;

            FinalLog(sw, connections);
        }

        /// <summary>
        /// Runs the actual heartbeat. 
        /// </summary>
        private void RunActualHeartbeat(bool forceReload)
        {
            // We only do all this checking if there was a dbts change.
            //
            // IMPORTANT: 
            //
            // This means that all the stuff in this section must be dependant at some level on a timestamp column.
            // If some type of change checking is not 1. Then it probably should be 2. If it can't be, then it cant be excluded
            // by the timestamp tracking change detection.
            //
            if (heartbeatChangeProcessingPending)
            {
                log.DebugFormat("[Heartbeat] Processing timestamp change");
                heartbeatChangeProcessingPending = false;

                bool storesChanged = false;

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
                StampsAccountManager.CheckForChangesNeeded();
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

                ReloadExternalDependencies(forceReload, storesChanged);
            }
            else
            {
                NoDataChangeBehavior(forceReload);
            }
        }

        /// <summary>
        /// Determines whether this instance can start.
        /// </summary>
        protected virtual bool CanStart()
        {
            return true;
        }

        /// <summary>
        /// Sets the heartbeat speed.
        /// </summary>
        protected virtual void SetHeartbeatSpeed(int rate)
        {
            // do nothing
        }

        /// <summary>
        /// Determines whether [is program ready for heartbeat].
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsProgramReadyForHeartbeat()
        {
            return true;
        }

        /// <summary>
        /// Runs after it is determined that ShipWorks is not in a state to do a heartbeat.
        /// </summary>
        protected virtual void RunProgramNotReadyLogic(bool heartbeatChangeDetected, bool forceReload)
        {
            // do nothing
        }

        /// <summary>
        /// Reloads the external dependencies.
        /// </summary>
        protected virtual void ReloadExternalDependencies(bool forceReload, bool storesChanged)
        {
            // do nothing
        }

        /// <summary>
        /// Executes after it is determined no data is changed.
        /// </summary>
        protected virtual void NoDataChangeBehavior(bool forceReload)
        {
            // do nothing
        }

        /// <summary>
        /// Executes after data is reloaded.
        /// </summary>
        protected virtual void AfterReload()
        {
            // do nothing
        }

        /// <summary>
        /// Call to log at end of HeartBeat.
        /// </summary>
        protected virtual void FinalLog(Stopwatch stopwatch, long connections)
        {
            log.DebugFormat("[Heartbeat] Finished. ({0}), Connections: {1}.", stopwatch.Elapsed.TotalSeconds, ConnectionMonitor.TotalConnectionCount - connections);
        }
    }
}