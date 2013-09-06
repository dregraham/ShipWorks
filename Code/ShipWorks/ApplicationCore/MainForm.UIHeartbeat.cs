using System;
using System.Diagnostics;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Grid;
using ShipWorks.UI;
using log4net;

namespace ShipWorks
{
    public partial class MainForm
    {
        /// <summary>
        /// Manages the Heartbeat. Heartbeat controls reloading cache.
        /// </summary>
        private class UIHeartbeat : Heartbeat
        {
            private static ILog log = LogManager.GetLogger(typeof(UIHeartbeat));

            private readonly MainForm mainForm;

            /// <summary>
            /// Initializes a new instance of the <see cref="UIHeartbeat"/> class.
            /// </summary>
            public UIHeartbeat(MainForm mainForm)
            {
                this.mainForm = mainForm;
            }

            /// <summary>
            /// Determines whether this instance can start.
            /// </summary>
            /// <returns></returns>
            protected override bool CanStart()
            {
                return !mainForm.IsDisposed && mainForm.heartbeatTimer.Enabled;
            }

            /// <summary>
            /// Sets the heartbeat speed.
            /// </summary>
            protected override void SetHeartbeatSpeed(int rate)
            {
                mainForm.heartbeatTimer.Interval = rate;
            }

            /// <summary>
            /// Determines whether [is program ready for heartbeat].
            /// </summary>
            protected override bool IsProgramReadyForHeartbeat()
            {
                return NativeMethods.IsWindowEnabled(mainForm.Handle) && !PopupController.IsAnyPopupVisible;
            }

            /// <summary>
            /// Runs after it is determined that ShipWorks is not in a state to do a heartbeat.
            /// </summary>
            protected override void RunProgramNotReadyLogic(bool heartbeatChangeDetected, bool forceReload)
            {
                if (heartbeatChangeDetected)
                {
                    mainForm.filterTree.UpdateFilterCounts();

                    if (forceReload)
                    {
                        mainForm.gridControl.ReloadFiltering();
                    }
                    else
                    {
                        mainForm.gridControl.UpdateFiltering();
                    }

                    mainForm.UpdateSelectionDependentUI();
                }
                else if (forceReload)
                {
                    mainForm.gridControl.ReloadFiltering();
                    mainForm.UpdateSelectionDependentUI();
                }
            }

            /// <summary>
            /// Reloads the external dependencies.
            /// </summary>
            protected override void ReloadExternalDependencies(bool forceReload, bool storesChanged)
            {
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
                    if (forceReload)
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
                    FilterNodeColumnManager.InitializeForCurrentSession();
                    mainForm.gridControl.ReloadGridColumns();
                }

                mainForm.UpdateSelectionDependentUI();
            }

            /// <summary>
            /// Executes after it is determined no data is changed.
            /// </summary>
            protected override void NoDataChangeBehavior(bool forceReload)
            {
                if (forceReload)
                {
                    mainForm.gridControl.ReloadFiltering();
                    mainForm.UpdateSelectionDependentUI();
                }
            }

            /// <summary>
            /// Executes after data is reloaded.
            /// </summary>
            protected override void AfterReload()
            {
                // If the filter tree is showing some spinning, then loop back around so we can get the updated non-spinning counts as soon as possible
                if (mainForm.filterTree.HasCalculatingNodes() && HeartbeatForcedFastRatesLeft == 0)
                {
                    log.DebugFormat("[Heartbeat] Forcing reloop due to spinning filters");

                    HeartbeatForcedFastRatesLeft = 1;
                    mainForm.heartbeatTimer.Interval = HeartbeatFastRate;
                }
            }

            /// <summary>
            /// Call to log at end of HeartBeat.
            /// </summary>
            protected override void FinalLog(Stopwatch stopwatch, long connections)
            {
                // Logging
                log.DebugFormat("[Heartbeat] Finished. ({0}), Connections: {1}, Interval: {2}s", stopwatch.Elapsed.TotalSeconds, ConnectionMonitor.TotalConnectionCount - connections, mainForm.heartbeatTimer.Interval/1000);
            }
        }
    }
}