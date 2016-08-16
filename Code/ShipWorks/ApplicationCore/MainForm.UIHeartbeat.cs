using System;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Grid;
using ShipWorks.UI;

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

            // The instance of the MainForm that owns us
            private readonly MainForm mainForm;

            // The timer that keeps the pace
            private System.Windows.Forms.Timer pacemaker = new System.Windows.Forms.Timer();

            /// <summary>
            /// Initializes a new instance of the <see cref="UIHeartbeat"/> class.
            /// </summary>
            public UIHeartbeat(MainForm mainForm)
            {
                this.mainForm = mainForm;
                this.pacemaker.Tick += new EventHandler(OnPacemakerTick);
            }

            /// <summary>
            /// Single pulse of the pacemaker
            /// </summary>
            private void OnPacemakerTick(object sender, EventArgs e)
            {
                DoHeartbeat(HeartbeatOptions.None);
            }

            /// <summary>
            /// Determines if the heartbeat is ready to beat
            /// </summary>
            protected override bool CanBeat()
            {
                if (mainForm.IsDisposed)
                {
                    return false;
                }

                if (!pacemaker.Enabled)
                {
                    return false;
                }

                return base.CanBeat();
            }

            /// <summary>
            /// Sets the heartbeat speed.
            /// </summary>
            protected override void UpdatePacemaker(int pace)
            {
                if (pace > 0)
                {
                    pacemaker.Interval = pace;
                    pacemaker.Enabled = true;
                }
                else
                {
                    pacemaker.Enabled = false;
                }
            }

            /// <summary>
            /// Runs after it is determined that ShipWorks is not in a state to do a heartbeat.
            /// </summary>
            protected override void ProcessHeartbeat(bool changesDetected, bool forceReload)
            {
                // Detect if a modal window is open.  The popup test is, for now, to make sure we don't reload the 
                // grid columns on a filter layout change while the right-click grid column editor is open.
                if (!NativeMethods.IsWindowEnabled(mainForm.Handle) || PopupController.IsAnyPopupVisible)
                {
                    if (changesDetected)
                    {
                        mainForm.UpdateFilterCounts();

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
                else
                {
                    // It's OK to do the normal processing
                    base.ProcessHeartbeat(changesDetected, forceReload);
                }
            }

            /// <summary>
            /// Updates the display of data based on what has changed
            /// </summary>
            protected override void RespondToChanges(bool hadChanges, bool forceReload, bool storesChanged)
            {
                if (hadChanges)
                {
                    bool reloadColumns = false;

                    // If the filter layout is dirty, we have to reload it
                    if (FilterLayoutContext.Current.IsLayoutDirty())
                    {
                        log.InfoFormat("[Heartbeat] Filter layout is dirty");

                        // Grid columns are layout dependant.  Save off the current set before we reload
                        mainForm.gridControl.SaveGridColumnState();

                        // Reload the filter trees
                        mainForm.ReloadFilterLayouts();

                        // Update the new active filter tree selection. Don't clear a search though.
                        if (!mainForm.gridControl.IsSearchActive)
                        {
                            mainForm.gridControl.ActiveFilterNode = mainForm.SelectedFilterNode();
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
                            mainForm.UpdateFilter(filter);
                        }

                        // Ensure the filter tree is showing up-to-date counts
                        mainForm.UpdateFilterCounts();

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
                else if (forceReload)
                {
                    mainForm.gridControl.ReloadFiltering();
                    mainForm.UpdateSelectionDependentUI();
                }
            }

            /// <summary>
            /// Final handling after everything is processied
            /// </summary>
            protected override void FinishHeartbeat()
            {
                base.FinishHeartbeat();

                // If the filter tree is showing some spinning, go ahead and have it update as their may be new counts loaded by now
                if (mainForm.FiltersHaveCalculatingNodes())
                {
                    mainForm.UpdateFilterCounts();
                }

                // If the filter tree is showing some spinning, then loop back around so we can get the updated non-spinning counts as soon as possible
                if (mainForm.FiltersHaveCalculatingNodes())
                {
                    log.DebugFormat("[Heartbeat] Forcing reloop due to spinning filters");

                    Pace = HeartbeatPace.SingleFast;
                }
            }
        }
    }
}