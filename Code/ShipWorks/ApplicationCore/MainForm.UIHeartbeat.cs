using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Interapptive.Shared.Win32;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.ApplicationCore.Enums;
using ShipWorks.ApplicationCore.Services;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email;
using ShipWorks.Email.Accounts;
using ShipWorks.FileTransfer;
using ShipWorks.Filters;
using ShipWorks.Filters.Grid;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
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
using ShipWorks.UI;
using ShipWorks.Users;

namespace ShipWorks
{
    public partial class MainForm
    {
        private class UIHeartbeat:HeartBeat
        {
            private readonly MainForm mainForm;

            public UIHeartbeat(MainForm mainForm)
            {
                this.mainForm = mainForm;
            }

            /// <summary>
            /// Do a single heartbeat.  Should not be called directly - call ForceHeartbeat instead
            /// </summary>
            public void DoHeartbeat(HeartbeatOptions options)
            {

                if (mainForm.IsDisposed)
                {
                    return;
                }

                if (!mainForm.heartbeatTimer.Enabled)
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
                Debug.Assert(UserSession.IsLoggedOn);

                // Extract heartbeat options
                bool forceGridReload = (options & HeartbeatOptions.ForceGridReload) != 0;

                // If its beating too fast, then skip it, we don't want to update too often.
                TimeSpan timeSinceLastHeartbeat = DateTime.UtcNow - lastHeartbeatTime;
                if (timeSinceLastHeartbeat < heartbeatMinimumWait && !forceGridReload)
                {
                    // Increase the heartrate for one beat (if its not fast already) so we get back in here quickly.
                    if (HeartbeatForcedFastRatesLeft == 0)
                    {
                        HeartbeatForcedFastRatesLeft = 1;
                        mainForm.heartbeatTimer.Interval = HeartbeatFastRate;
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
                        mainForm.heartbeatTimer.Interval = heartbeatNormalRate;
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
                if (!NativeMethods.IsWindowEnabled(mainForm.Handle) || PopupController.IsAnyPopupVisible)
                {
                    if (heartbeatChangeDetected)
                    {
                        mainForm.filterTree.UpdateFilterCounts();

                        if (forceGridReload)
                        {
                            mainForm.gridControl.ReloadFiltering();
                        }
                        else
                        {
                            mainForm.gridControl.UpdateFiltering();
                        }

                        mainForm.UpdateSelectionDependentUI();
                    }
                    else if (forceGridReload)
                    {
                        mainForm.gridControl.ReloadFiltering();
                        mainForm.UpdateSelectionDependentUI();
                    }
                }
                else
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

                        bool reloadColumns = false;

                        // If the filter layout is dirty, we have to reload it
                        if (FilterLayoutContext.Current.IsLayoutDirty())
                        {
                            log.InfoFormat("[Heartbeat] Filter layout is dirty");

                            // Grid columns are layout dependant.  Save off the current set before we reload
                            mainForm.gridControl.SaveGridColumnState();

                            // Reload the filter tree
                            mainForm.filterTree.SelectedFilterNodeChanged -= new EventHandler(mainForm.OnSelectedFilterNodeChanged);
                            mainForm.filterTree.ReloadLayouts();
                            mainForm.filterTree.SelectedFilterNodeChanged += new EventHandler(mainForm.OnSelectedFilterNodeChanged);

                            // Update the new active filter tree selection. Don't clear a search though.
                            if (!mainForm.gridControl.IsSearchActive)
                            {
                                mainForm.gridControl.ActiveFilterNode = mainForm.filterTree.SelectedFilterNode;
                            }

                            // If a node moved, it could now be inheriting different settings, so we have to force the columns to reload.
                            reloadColumns = true;

                            // # of filters present can effect edition issues
                            mainForm.editionGuiHelper.UpdateUI();
                        }

                        else
                        {
                            // Refresh any filters that have changed in the database.  This is basically just for filter name changes... mostly any
                            // changes to definitions and such are reported as IsLayoutDirty
                            foreach (FilterEntity filter in FilterLayoutContext.Current.RefreshFilters())
                            {
                                mainForm.filterTree.UpdateFilterName(filter);
                            }

                            // Ensure the filter tree is showing up-to-date counts
                            mainForm.filterTree.UpdateFilterCounts();

                            // Ensure the grids are up to date
                            if (forceGridReload)
                            {
                                mainForm.gridControl.ReloadFiltering();
                            }
                            else
                            {
                                mainForm.gridControl.UpdateFiltering();
                            }
                        }

                        if (storesChanged)
                        {
                            mainForm.UpdateStoreDependentUI();

                            // Needed in case a store is renamed, to get the new name to show up
                            mainForm.gridControl.Refresh();

                            // No longer need to reload the columns later, since the UpdateStoreDependentUI does it.
                            reloadColumns = false;
                        }

                        if (reloadColumns)
                        {
                            FilterNodeColumnManager.InitializeForCurrentUser();
                            mainForm.gridControl.ReloadGridColumns();
                        }

                        mainForm.UpdateSelectionDependentUI();
                    }
                    else if (forceGridReload)
                    {
                        mainForm.gridControl.ReloadFiltering();
                        mainForm.UpdateSelectionDependentUI();
                    }
                }

                // If the filter tree is showing some spinning, then loop back around so we can get the updated non-spinning counts as soon as possible
                if (mainForm.filterTree.HasCalculatingNodes() && HeartbeatForcedFastRatesLeft == 0)
                {
                    log.DebugFormat("[Heartbeat] Forcing reloop due to spinning filters");

                    HeartbeatForcedFastRatesLeft = 1;
                    mainForm.heartbeatTimer.Interval = HeartbeatFastRate;
                }

                // Save the last heartbeat time from the end of the heartbeat.  That way if it takes a while to process, we don't overlap.
                lastHeartbeatTime = DateTime.UtcNow;

                // Logging
                log.DebugFormat("[Heartbeat] Finished. ({0}), Connections: {1}, Interval: {2}s", sw.Elapsed.TotalSeconds, ConnectionMonitor.TotalConnectionCount - connections, mainForm.heartbeatTimer.Interval / 1000);
            }


        }
    }
}
