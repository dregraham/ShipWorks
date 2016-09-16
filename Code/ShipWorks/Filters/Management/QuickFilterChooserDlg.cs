using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Controls;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Window for choosing an existing quick filter
    /// </summary>
    public partial class QuickFilterChooserDlg : Form
    {
        FilterTarget[] targets;
        FilterTarget initialTarget;

        FilterNodeEntity chosenNode = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public QuickFilterChooserDlg(FilterTarget[] targets, FilterTarget initialTarget)
        {
            InitializeComponent();

            this.targets = targets;
            this.initialTarget = initialTarget;

            WindowStateSaver.Manage(this);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            sandGrid.Rows.Clear();

            EnumHelper.BindComboBox<FilterTarget>(filtersFor, t => targets.Contains(t));

            if (targets.Contains(initialTarget))
            {
                filtersFor.SelectedValue = initialTarget;
            }

            if (targets.Length == 1)
            {
                filtersFor.Visible = false;

                labelFiltersFor.Text = "Quick Filters for " + EnumHelper.GetDescription(targets[0]).ToLowerInvariant() + ":";
            }

            UpdateButtonState();
        }

        /// <summary>
        /// Update the UI state of the buttons
        /// </summary>
        private void UpdateButtonState()
        {
            ok.Enabled = sandGrid.SelectedElements.Count == 1;
            edit.Enabled = sandGrid.SelectedElements.Count == 1;
        }

        /// <summary>
        /// The filter node chosen by the user. Only valid of the DialogResult is OK.
        /// </summary>
        public FilterNodeEntity ChosenFilterNode
        {
            get { return chosenNode; }
        }

        /// <summary>
        /// Change the filter target selected in the ComboBox
        /// </summary>
        private void OnChangeFilterTarget(object sender, EventArgs e)
        {
            FilterTarget target = (FilterTarget) filtersFor.SelectedValue;

            LoadGridForTarget(target);
        }

        /// <summary>
        /// Load the grid with quick filters of the given target type
        /// </summary>
        private void LoadGridForTarget(FilterTarget target)
        {
            Cursor.Current = Cursors.WaitCursor;

            sandGrid.Rows.Clear();

            // Get all existing quick nodes for the filter
            List<FilterNodeEntity> localNodes = QuickFilterHelper.GetQuickFilters(target);

            // Load into the grid, alphabetized
            foreach (FilterNodeEntity filterNode in localNodes.OrderBy(n => n.Filter.Name))
            {
                List<string> reasons = ObjectReferenceManager.GetReferenceReasons(new List<long> { filterNode.FilterNodeID });

                FilterTreeGridRow row = new FilterTreeGridRow(filterNode);
                row.Cells.Add(new GridCell(string.Join("\n", reasons.ToArray())));

                row.Height = 0;
                row.Tag = filterNode;

                sandGrid.Rows.Add(row);
            }
        }

        /// <summary>
        /// The grid selection has changed
        /// </summary>
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonState();
        }

        /// <summary>
        /// A row has been activated
        /// </summary>
        private void OnRowActivated(object sender, GridRowEventArgs e)
        {
            EditFilter(e.Row);
        }

        /// <summary>
        /// Edit the selected filter node
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            EditFilter((GridRow) sandGrid.SelectedElements[0]);
        }

        /// <summary>
        /// Edit the filter associated with the given row
        /// </summary>
        private void EditFilter(GridRow gridRow)
        {
            FilterNodeEntity filterNode = (FilterNodeEntity) gridRow.Tag;

            FilterEditingService.EditFilter(filterNode, this);

            OnChangeFilterTarget(filtersFor, EventArgs.Empty);

            SelectRow(filterNode);

            CheckForCalculatingCounts();
        }

        /// <summary>
        /// Create a new quick filter
        /// </summary>
        private void OnCreate(object sender, EventArgs e)
        {
            FilterNodeEntity newFilter = QuickFilterHelper.CreateQuickFilter((FilterTarget) filtersFor.SelectedValue);

            if (FilterEditingService.EditFilter(newFilter, this) != FilterEditingResult.OK)
            {
                QuickFilterHelper.DeleteQuickFilter(newFilter);
            }
            else
            {
                OnChangeFilterTarget(filtersFor, EventArgs.Empty);

                SelectRow(newFilter);
            }

            CheckForCalculatingCounts();
        }

        /// <summary>
        /// Select the row associated with the given node
        /// </summary>
        private void SelectRow(FilterNodeEntity filterNode)
        {
            GridRow row = sandGrid.Rows.OfType<GridRow>().SingleOrDefault(r => ((FilterNodeEntity) r.Tag).FilterNodeID == filterNode.FilterNodeID);
            if (row != null)
            {
                row.Selected = true;
            }
        }

        /// <summary>
        /// Begin checking for counts that are calculating so that they may be refreshed.
        /// </summary>
        private void CheckForCalculatingCounts()
        {
            filterCountTimer.Start();
            OnRefreshFilterCountTimer(filterCountTimer, EventArgs.Empty);
        }

        /// <summary>
        /// Timer use to check for the completion of filter counts that are in the process of calculating.
        /// </summary>
        private void OnRefreshFilterCountTimer(object sender, EventArgs e)
        {
            // Make sure our counts are up to date
            bool changesOrCalcuations = FilterContentManager.CheckForChanges();

            UpdateFilterCounts();

            // All we want to do is get latest counts so that we can stop the spinning
            if (!changesOrCalcuations)
            {
                filterCountTimer.Stop();
            }
        }

        /// <summary>
        /// Update the counts of each node in the grid
        /// </summary>
        public void UpdateFilterCounts()
        {
            foreach (FilterTreeGridRow row in sandGrid.Rows)
            {
                row.UpdateFilterCount();
            }
        }

        /// <summary>
        /// OK'ing the current selection
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (sandGrid.SelectedElements.Count == 1)
            {
                chosenNode = (FilterNodeEntity) sandGrid.SelectedElements[0].Tag;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
