using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Autofac;
using ComponentFactory.Krypton.Toolkit;
using Divelements.SandGrid;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Caching;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.Definitions;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.UI.Utility;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Window for displaying and retrying errors from action tasks
    /// </summary>
    public partial class ActionErrorDlg : Form
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ActionErrorDlg));

        static Guid gridSettingsKey = new Guid("{81E3C43C-F779-45d1-B924-00BC93FA1446}");

        // Used to be the entity cache, monitor for changes, and notify us when the grid needs refreshed
        EntityCacheEntityProvider entityProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionErrorDlg()
        {
            InitializeComponent();

            WindowStateSaver.Manage(this, WindowStateSaverOptions.Size);

            ThemedBorderProvider themeBorder = new ThemedBorderProvider(panelGridArea);
            panelTools.StateNormal.Draw = ThemeInformation.VisualStylesEnabled ? InheritBool.True : InheritBool.False;

        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // Can only retry - not delete, if not allowed to manage actions
            delete.Visible = UserSession.Security.HasPermission(PermissionType.ManageActions);

            // Set our special grid row for drawing the child items
            entityGrid.PrimaryGrid.NewRowType = typeof(ActionErrorGridRow);

            // Each time we get an action we want it's steps too
            PrefetchPath2 prefetch = new PrefetchPath2(EntityType.ActionQueueEntity);
            prefetch.Add(ActionQueueEntity.PrefetchPathSteps);

            // Create the entity provider for caching and refreshing
            entityProvider = new EntityCacheEntityProvider(EntityType.ActionQueueEntity, prefetch);
            entityProvider.EntityChangesDetected += new EntityCacheChangeMonitoredChangedEventHandler(OnEntityProviderChangeDetected);

            // Prepare for paging
            entityGrid.InitializeGrid();

            // Prepare configurable columns
            entityGrid.InitializeColumns(new StandardGridColumnStrategy(gridSettingsKey, GridColumnDefinitionSet.ActionErrors, InitializeDefaultGridLayout));
            entityGrid.SaveColumnsOnClose(this);

            // Create the windows renderer
            entityGrid.Renderer = AppearanceHelper.CreateWindowsRenderer();

            // Always start out showing error messages
            showErrorMessages.Checked = true;

            // Open the data
            entityGrid.OpenGateway(new QueryableEntityGateway(entityProvider, new RelationPredicateBucket(ActionQueueFields.Status == (int) ActionQueueStatus.Error)));
        }

        /// <summary>
        /// The entity provider detected changes to the underlying data.  The grid needs updated.
        /// </summary>
        private void OnEntityProviderChangeDetected(object sender, EntityCacheChangeMonitoredChangedEventArgs e)
        {
            if (e.Inserted.Count + e.Deleted.Count > 0)
            {
                entityGrid.ReloadGridRows();
            }
            else
            {
                entityGrid.UpdateGridRows();
            }

            UpdateButtonState();
        }

        /// <summary>
        /// Reload the rows to ensure they are showing the latest state
        /// </summary>
        protected void ReloadGridRows()
        {
            entityGrid.ReloadGridRows();

            UpdateButtonState();
        }

        /// <summary>
        /// Create the default column layout to use for the grid
        /// </summary>
        private void InitializeDefaultGridLayout(GridColumnLayout layout)
        {
            layout.DefaultSortColumnGuid = ActionErrorColumnDefinitionFactory.CreateDefinitions()[ActionQueueFields.TriggerDate].ColumnGuid;
            layout.DefaultSortOrder = ListSortDirection.Descending;
        }

        /// <summary>
        /// Open the grid settings window
        /// </summary>
        private void OnGridSettings(object sender, EventArgs e)
        {
            entityGrid.ShowColumnEditorDialog();
        }

        /// <summary>
        /// Selected rows in the grid have changed
        /// </summary>
        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonState();
        }

        /// <summary>
        /// Update the state of the buttons based on selection
        /// </summary>
        private void UpdateButtonState()
        {
            delete.Enabled = entityGrid.Selection.Count > 0;
            retry.Enabled = entityGrid.Selection.Count > 0;
        }

        /// <summary>
        /// Toggle the display of error messages
        /// </summary>
        private void OnChangeShowErrorMessages(object sender, EventArgs e)
        {
            // This is a static property as a quick shortcut hack that works fine b\c this window and this row type is only
            // ever used once at any given time, while this window is displayed.
            ActionErrorGridRow.ShowDetails = showErrorMessages.Checked;

            ReloadGridRows();
        }

        /// <summary>
        /// Delete the selected tasks
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            DialogResult result = MessageHelper.ShowQuestion(this, "Delete all selected tasks?");

            if (result != DialogResult.OK)
            {
                return;
            }

            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(this,
                "Delete Actions",
                "ShipWorks is deleting action tasks.",
                "Deleting {0} of {1}");

            executor.ExecuteCompleted += (_sender, _e) =>
            {
                // Update the dashboard to display the updated error info
                DashboardManager.CheckForActionChanges();

                // Reload our displayed rows
                ReloadGridRows();
            };

            executor.ExecuteAsync((entityID, state, issueAdder) =>
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    adapter.DeleteEntity(new ActionQueueEntity(entityID));
                    adapter.Commit();
                }

            }, entityGrid.Selection.OrderedKeys);
        }

        /// <summary>
        /// Retry selected actions
        /// </summary>
        private void OnRetry(object sender, EventArgs e)
        {
            ProgressProvider progressProvider = new ProgressProvider();

            // Progress window for long running previews
            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Retry Actions";
            progressDlg.Description = "ShipWorks is retrying selected actions.";

            ProgressDisplayDelayer delayer = new ProgressDisplayDelayer(progressDlg);

            Thread thread = new Thread(ExceptionMonitor.WrapThread(BackgroundRetryActions, "retrying actions"));
            thread.Name = "Preview Processor";
            thread.Start(new object[] { delayer, progressProvider, entityGrid.Selection.OrderedKeys });

            // Show the progress window if enough time goes by
            delayer.ShowAfter(this, TimeSpan.FromSeconds(.4));
        }

        /// <summary>
        /// Background thread for retrying actions
        /// </summary>
        private void BackgroundRetryActions(object state)
        {
            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) ((object[]) state)[0];
            ProgressProvider progressProvider = (ProgressProvider) ((object[]) state)[1];
            List<long> queueList = ((IEnumerable<long>) ((object[]) state)[2]).ToList();

            int attemptCount = queueList.Count;
            int wrongComputerCount = 0;
            int successCount = 0;
            int notRanCount = 0;

            // This just counts the number of times processing has occurred.  This can happen more than once
            // per queue though depending on postponing, so its just a close guide as to progress
            int timesProcessed = 0;

            // Create the progress item
            IProgressReporter progressItem = progressProvider.AddItem("Processing");
            progressItem.Detail = "Retrying...";
            progressItem.Starting();
            progressItem.CanCancel = false;

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ActionProcessor processor = lifetimeScope.Resolve<IActionProcessorFactory>().CreateError(queueList);
                processor.ActionProcessed += (sender, e) =>
                    {
                        if (e.Result == ActionRunnerResult.WrongComputer)
                        {
                            wrongComputerCount++;
                        }

                        else if (e.Result == ActionRunnerResult.Ran)
                        {
                            if (e.QueueAtFinish.Status == (int) ActionQueueStatus.Success)
                            {
                                successCount++;
                            }
                        }

                        else if (e.Result != ActionRunnerResult.Postponed)
                        {
                            notRanCount++;
                        }

                        // If it completed, or couldn't be run, then remove it from the list to be processed.
                        if ((e.Result == ActionRunnerResult.Ran && (e.QueueAtFinish.Status == (int) ActionQueueStatus.Success || e.QueueAtFinish.Status == (int) ActionQueueStatus.Error) ||
                             (e.Result == ActionRunnerResult.WrongComputer || e.Result == ActionRunnerResult.Missing || e.Result == ActionRunnerResult.NoTasks || e.Result == ActionRunnerResult.Locked)))
                        {
                            queueList.Remove(e.QueueID);
                        }

                        timesProcessed++;

                        // We use queueList.count for the actual remaining count when retrying.  This will not increase when one is postponed
                        progressItem.Detail = string.Format("Retrying {0} of {1}...", Math.Min(timesProcessed, attemptCount), attemptCount);
                        progressItem.PercentComplete = Math.Min((timesProcessed * 100) / attemptCount, 100);
                    };

                // Start processing
                processor.ProcessQueues();
            }

            // Progress complete
            progressItem.Completed();

            // Post the result
            BeginInvoke(new MethodInvoker<ProgressDisplayDelayer, int, int, int, int>(OnRetryComplete), delayer, attemptCount, successCount, wrongComputerCount, notRanCount);
        }

        /// <summary>
        /// The retry execution is complete.  This happens back on the UI thread
        /// </summary>
        private void OnRetryComplete(ProgressDisplayDelayer delayer, int attemptCount, int successCount, int wrongComputerCount, int notRanCount)
        {
            delayer.NotifyComplete();

            ReloadGridRows();

            ShowResultMessage(attemptCount, successCount, wrongComputerCount, notRanCount);
        }

        /// <summary>
        /// Show the appropriate message to the user based on what happened
        /// </summary>
        private void ShowResultMessage(int attemptCount, int successCount, int wrongComputerCount, int notRanCount)
        {
            if (successCount == attemptCount)
            {
                MessageHelper.ShowInformation(this, "The selected actions succeeded and have been removed from the grid.");
            }
            else
            {
                int errorCount = attemptCount - successCount - wrongComputerCount - notRanCount;

                string message = "";

                if (successCount > 0)
                {
                    message = string.Format("{0} of the selected actions succeeded and {1} been removed from the grid.", successCount, successCount == 1 ? "has" : "have");
                }

                if (wrongComputerCount > 0)
                {
                    if (message.Length > 0)
                    {
                        message += "\n\n";
                    }

                    message += string.Format("{0} of the selected actions cannot be ran from this computer.", wrongComputerCount);
                }

                if (notRanCount > 0)
                {
                    if (message.Length > 0)
                    {
                        message += "\n\n";
                    }

                    message += string.Format("{0} of the selected actions were not ran.", notRanCount);
                }

                if (errorCount > 0)
                {
                    if (message.Length > 0)
                    {
                        message += "\n\n";
                    }

                    if (errorCount == attemptCount)
                    {
                        message += "The selected actions failed. ";
                    }
                    else
                    {
                        message += string.Format("{0} of the selected actions failed. ", errorCount);
                    }

                    message += "The grid has been updated with the most recent error detail.";
                }

                MessageHelper.ShowError(this, message);
            }
        }

        /// <summary>
        /// Form has been closed
        /// </summary>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            entityProvider.Dispose();
            entityProvider = null;
        }
    }
}
