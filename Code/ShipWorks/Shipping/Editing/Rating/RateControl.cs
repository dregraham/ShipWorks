using System.Diagnostics;
using Divelements.SandGrid;
using Divelements.SandGrid.Specialized;
using Interapptive.Shared.Utility;
using ShipWorks.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Filters;
using System.ComponentModel;
using ShipWorks.UI.Utility;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Reusable control for displaying rates for any shipping service
    /// </summary>
    public partial class RateControl : UserControl
    {
        private FootnoteParameters footnoteParameters;

        readonly object syncLock = new object();

        // Indicates if the spinner for showing that rates are currently being checked is visible
        private bool showSpinner = false;

        // Indicates if the control will automatically resize it's height to be just enough to fit
        private bool autoHeight = false;

        // If autoHeight is true, controls the maximum height the control will resize itself to be
        private int autoHeightMaximum = 250;

        // The text to display for the action link, if its visible
        private string actionLinkText = "Select";
        private bool hasMoreLinkBeenClicked;

        /// <summary>
        /// Raised when the user selects a rate row
        /// </summary>
        public event RateSelectedEventHandler RateSelected;

        /// <summary>
        /// Raised when the action link has been clicked
        /// </summary>
        public event RateSelectedEventHandler ActionLinkClicked;

        /// <summary>
        /// Raised when its necessary for the rates to be reloaded
        /// </summary>
        public event EventHandler ReloadRatesRequired;
        /// <summary>
        /// Constructor
        /// </summary>
        public RateControl()
        {
            InitializeComponent();

            RateGroup = new RateGroup(new List<RateResult>());
            rateGrid.Rows.Clear();

            gridColumnSelect.ButtonClicked += OnConfigureRateClicked;

            ShowAllRates = true;
            ShowSpinner = false;
            RestrictedRateCount = 5;

            rateGrid.Renderer = AppearanceHelper.CreateWindowsRenderer();
            hasMoreLinkBeenClicked = false;
        }

        /// <summary>
        /// Initialize the rate control
        /// </summary>
        public void Initialize(FootnoteParameters parameters)
        {
            footnoteParameters = parameters;
            ThemedBorderProvider.Apply(this);
        }

        /// <summary>
        /// Gets the rate group loaded in the control. If a rate group has not been loaded
        /// into the control, a group without any rate results is returned.
        /// </summary>
        public RateGroup RateGroup 
        { 
            get; 
            private set; 
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether to [show the configure link].
        /// </summary>
        public bool ActionLinkVisible 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// The text to display for the action link
        /// </summary>
        [DefaultValue("Select")]
        public string ActionLinkText
        {
            get { return actionLinkText; }
            set { actionLinkText = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [show all rates]. If false, then a subset
        /// of rates will be displayed with the last result having a "More" link to view the
        /// full list of results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show all rates]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowAllRates { get; set; }

        /// <summary>
        /// Gets or sets the restricted rate count. This is the maximum rates to display when the 
        /// control is configured with ShowAllRates = false
        /// </summary>
        /// <value>The restricted rate count.</value>
        public int RestrictedRateCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to [show single rate].
        /// </summary>
        /// <value><c>true</c> if [show single rate]; otherwise, <c>false</c>.</value>
        public bool ShowSingleRate { get; set; }

        /// <summary>
        /// Indicates if the control will automatically resize it's height to be just enough to fit
        /// </summary>
        [DefaultValue(false)]
        public bool AutoHeight
        {
            get
            {
                return autoHeight;
            }
            set
            {
                autoHeight = value;
            }
        }

        /// <summary>
        /// If autoHeight is true, controls the maximum height the control will resize itself to be
        /// </summary>
        [DefaultValue(250)]
        public int AutoHeightMaximum
        {
            get
            {
                return autoHeightMaximum;
            }
            set
            {
                autoHeightMaximum = value;
            }
        }

        /// <summary>
        /// Gets the rate that is selected in the grid. A null value is returned if there
        /// is not a rate that has been selected.
        /// </summary>
        public RateResult SelectedRate
        {
            get
            {
                RateResult selectedRate = null;

                // Acquire the lock so the grid isn't reloaded while trying to find the selected rate
                lock (syncLock)
                {
                    IEnumerable<GridRow> selectedRows = rateGrid.SelectedElements.OfType<GridRow>().ToList();
                    if (selectedRows.Any())
                    {
                        selectedRate = selectedRows.Select(s => s.Tag as RateResult).ToList().FirstOrDefault(f => f.Selectable);
                        if(selectedRate != null && selectedRate.Tag is BestRateResultTag && !((BestRateResultTag)selectedRate.Tag).IsRealRate)
                        {
                            selectedRate = null;
                        }
                    }
                }

                return selectedRate;
            }
        }

        /// <summary>
        /// Clear the rates in the grid and display the given reason for having no rates displayed.
        /// </summary>
        public void ClearRates(string emptyReason)
        {
            ClearRates(emptyReason, null);
        }

        /// <summary>
        /// Clear the rates in the grid and display the given reason for having no rates displayed.
        /// </summary>
        /// <remarks>This version will display any footnotes associated with the rate group</remarks>
        public void ClearRates(string emptyReason, RateGroup rateGroup)
        {
            lock (syncLock)
            {
                Debug.Assert(rateGrid.EmptyText != null, "rateGrid.EmptyText != null");

                rateGrid.EmptyText = emptyReason;

                if (rateGrid.Rows.Count > 0)
                {
                    rateGrid.Rows.Clear();
                }

                UpdateFootnotes(rateGroup);

                SetRateDetailsVisibility(rateGroup != null ? rateGroup.Rates : new List<RateResult>());
            }

            ShowSpinner = false;

            UpdateHeight();
        }

        /// <summary>
        /// Clear the previous content of the footnote control
        /// </summary>
        private List<RateFootnoteControl> CurrentFootnotes
        {
            get { return panelFootnote.Controls.OfType<RateFootnoteControl>().ToList(); }
        }

        /// <summary>
        /// Shows the spinner to indicate that rates are being retrieved.
        /// </summary>
        [DefaultValue(false)]
        public bool ShowSpinner
        {
            get
            {
                return showSpinner;
            }
            set
            {
                showSpinner = value;

                if (showSpinner)
                {
                    loadingImage.BringToFront();
                    loadingRatesLabel.BringToFront();
                }
                else
                {
                    loadingImage.SendToBack();
                    loadingRatesLabel.SendToBack();
                }

                UpdateHeight();
            }
        }

        /// <summary>
        /// Load the rates into the control
        /// </summary>
        public void LoadRates(RateGroup rateGroup)
        {
            lock (syncLock)
            {
                RateGroup = rateGroup;
                rateGrid.Rows.Clear();

                gridColumnSelect.Visible = ActionLinkVisible;

                List<RateResult> ratesToShow = ShowAllRates ? rateGroup.Rates : rateGroup.Rates.Take(RestrictedRateCount).ToList();

                foreach (RateResult rate in ratesToShow)
                {
                    GridRow row = new GridRow(new[]
                    {
                        new GridCell(rate.ProviderLogo),
                        new GridCell(rate.Description),
                        new GridCell(rate.Days),
                        new GridCell(rate.Selectable && rate.Shipping.HasValue ? rate.Shipping.Value.ToString("c") : ""),
                        new GridCell(rate.Selectable && rate.Taxes.HasValue ? rate.Taxes.Value.ToString("c") : ""),
                        new GridCell(rate.Selectable && rate.Duties.HasValue ? rate.Duties.Value.ToString("c") : ""),
                        new GridCell(rate.Selectable ? rate.FormattedAmount : "", rate.AmountFootnote)
                    }) { Tag = rate };

                    if (ActionLinkVisible && rate.Selectable)
                    {
                        row.Cells.Add(new GridHyperlinkCell(actionLinkText));
                    }

                    rateGrid.Rows.Add(row);
                }

                SetRateDetailsVisibility(ratesToShow);

                AddShowMoreRatesRow();

                UpdateFootnotes(rateGroup);

                if (ratesToShow.Count == 0)
                {
                    if (CurrentFootnotes.Count == 0)
                    {
                        rateGrid.EmptyText = "ShipWorks could not get rates for this shipment.";
                    }
                    else
                    {
                        rateGrid.EmptyText = "";
                    }
                }
            }

            ShowSpinner = false;

            UpdateHeight();
        }

        /// <summary>
        /// Shows or hides the extra rate columns
        /// </summary>
        private void SetRateDetailsVisibility(List<RateResult> ratesToShow)
        {
            gridColumnDuty.Visible = ratesToShow.Any(x => x.Duties.HasValue);
            gridColumnTax.Visible = ratesToShow.Any(x => x.Taxes.HasValue);
            gridColumnShipping.Visible = ratesToShow.Any(x => x.Shipping.HasValue);
        }

        /// <summary>
        /// The rate control will track whether the More link has been clicked. This will 
        /// collapse the rates only if the link has not been previously clicked (i.e. the
        /// user elected to see more rates at some point, so we want to retain that view).
        /// </summary>
        public void TryCollapseRateResults()
        {
            if (!hasMoreLinkBeenClicked)
            {
                // The more link has not been clicked yet, so we can collapse
                // the results
                ShowAllRates = false;
            }
        }

        /// <summary>
        /// Resets the state of the control to behave as if the more link has not 
        /// been clicked.
        /// </summary>
        public void ResetCollapsibleState()
        {
            hasMoreLinkBeenClicked = false;
        }

        /// <summary>
        /// Adds the show more rates row if the control is configured to not show all rates and 
        /// the list of rates exceeds the restricted rate count.
        /// </summary>
        private void AddShowMoreRatesRow()
        {
            if (!ShowSingleRate)
            {
                // Honor the restricted rate count setting
                if (!ShowAllRates && RateGroup.Rates.Count > RestrictedRateCount)
                {
                    // Make a copy of the original full list, it's needed for showing all
                    RateGroup originalRateGroup = RateGroup.CopyWithRates(RateGroup.Rates);

                    string description = string.Format("{0} more expensive rates available.", originalRateGroup.Rates.Count - RestrictedRateCount);
                    RateResult showMoreRatesRateResult = new RateResult(description, "", 0, originalRateGroup)
                    {
                        Tag = new BestRateResultTag()
                        {
                            IsRealRate = false,
                            RateSelectionDelegate = entity =>
                            {
                                // The user wants to expand the rates, so set the ShowAllRates to true and load the full list of original rates
                                ShowAllRates = true;

                                // Capture that the more link has been clicked, so we don't collapse the rates again. The user 
                                // elected to see more rates, so we want to retain view
                                hasMoreLinkBeenClicked = true;
                                
                                LoadRates(originalRateGroup);
                            }
                        }
                    };

                    // Create the row with the "More" link
                    GridRow row = new GridRow(new[]
                    {
                        new GridCell(""),
                        new GridCell(showMoreRatesRateResult.Description),
                        new GridCell(""),
                        new GridCell(""),
                        new GridCell(""),
                        new GridCell(""),
                        new GridCell(""),
                        new GridHyperlinkCell("More...")
                    }) { Tag = showMoreRatesRateResult };

                    rateGrid.Rows.Add(row);
                }
            }
        }

        /// <summary>
        /// Update the height of the control if necessary
        /// </summary>
        private void UpdateHeight()
        {
            if (autoHeight)
            {
                int height = 5;

                if (rateGrid.Rows.Count == 0)
                {
                    // This was causing a crash when length was 0. 
                    if (rateGrid.Columns.DisplayColumns.Length > 0)
                    {
                        height += rateGrid.Columns.DisplayColumns[0].Bounds.Bottom;
                    }

                    if (!string.IsNullOrEmpty(rateGrid.EmptyText))
                    {
                        height += 18;
                    }
                }
                else
                {
                    // Make sure the grid has updated its row layout, and knows where they all are
                    if (rateGrid.IsHandleCreated)
                    {
                        rateGrid.PerformElementLayout();
                    }

                    height += rateGrid.Rows[rateGrid.Rows.Count - 1].Bounds.Bottom;
                }

                if (panelFootnote.Visible)
                {
                    height += panelFootnote.Height + 5;
                }

                int minimum = ShowSpinner ? 90 : 0;

                this.Height = Math.Max(Math.Min(height, autoHeightMaximum), minimum);
            }
        }

        /// <summary>
        /// Selects the rate in the grid. If the rate is not found, no rows are selected.
        /// </summary>
        /// <param name="rate">The rate.</param>
        public void SelectRate(RateResult rate)
        {
            lock (syncLock)
            {
                ClearSelection();

                int rateIndex = RateGroup.Rates.IndexOf(rate);
                if (rateIndex >= 0 && rateGrid.Rows.Count > rateIndex)
                {
                    GridRow selectedRow = rateGrid.Rows[rateIndex];
                    selectedRow.EnsureVisible();

                    selectedRow.Selected = true;
                }
            }
        }

        /// <summary>
        /// Clears any row selection in the grid.
        /// </summary>
        public void ClearSelection()
        {
            // Remove any highlighting from previously selected rows
            foreach (GridRow row in rateGrid.Rows)
            {
                foreach (GridCell cell in row.Cells)
                {
                    cell.BackColor = Color.Empty;
                }
            }

            rateGrid.SelectedElements.Clear();
        }

        
        /// <summary>
        /// Resets the footnotes with what are contained in the specified rate group
        /// </summary>
        private void UpdateFootnotes(RateGroup rateGroup)
        {
            // Get a copy of the current footnotes, since we don't want to include the newly created footnotes from what we'll remove
            List<RateFootnoteControl> footnotesToRemove = CurrentFootnotes;

            if (rateGroup != null)
            {
                // We're adding new footnotes first to eliminate UI flickering
                AddFootnotes(rateGroup.FootnoteFactories);
            }

            RemoveFootnotes(footnotesToRemove);

            panelFootnote.Visible = panelFootnote.Controls.OfType<RateFootnoteControl>().Any();
        }

        /// <summary>
        /// Removes the footnotes.
        /// </summary>
        private void RemoveFootnotes(IEnumerable<RateFootnoteControl> previousFootnotes)
        {
            foreach (RateFootnoteControl previousFootnote in previousFootnotes)
            {
                previousFootnote.RateCriteriaChanged -= OnFootnoteRateCriteriaChanged;
                panelFootnote.Controls.Remove(previousFootnote);
                previousFootnote.Dispose();
            }
        }

        /// <summary>
        /// Adds the footnotes from the .
        /// </summary>
        /// <param name="footnoteFactories">The footnote factories.</param>
        private void AddFootnotes(IEnumerable<IRateFootnoteFactory> footnoteFactories)
        {
            int y = 0;

            foreach (IRateFootnoteFactory factory in footnoteFactories)
            {
                RateFootnoteControl footnote = factory.CreateFootnote(footnoteParameters);

                panelFootnote.Controls.Add(footnote);
                footnote.Location = new Point(0, y);
                footnote.Width = panelFootnote.Width;
                footnote.RateCriteriaChanged += OnFootnoteRateCriteriaChanged;
                y += footnote.Height;
            }

            panelFootnote.Height = y;
        }

        /// <summary>
        /// The footnote has indicated it has changed something that has changed the rate criteria
        /// </summary>
        private void OnFootnoteRateCriteriaChanged(object sender, EventArgs e)
        {
            if (ReloadRatesRequired != null)
            {
                // We don't need to BeginInvoke here - that happens in the ShippingDlg, so we don't have to worry about being destroyed with the reload
                // in the middle of this call stack.
                ReloadRatesRequired(sender, e);
            }
        }

        /// <summary>
        /// Called when 'Configure' is clicked on a rate result
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="GridRowColumnEventArgs"/> instance containing the event data.</param>
        private void OnConfigureRateClicked(object sender, GridRowColumnEventArgs e)
        {
            RateResult rate = e.Row.Tag as RateResult;
            if (!rate.Selectable)
            {
                return;
            }

            if (ActionLinkClicked != null)
            {
                ActionLinkClicked(this, new RateSelectedEventArgs(rate));
            }
        }

        /// <summary>
        /// Called when the [selected rate row has changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnSelectedRateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedRate != null)
            {
                if (RateSelected != null)
                {
                    RateSelected(this, new RateSelectedEventArgs(SelectedRate));
                }
            }
            else
            {
                rateGrid.SelectionChanged -= OnSelectedRateChanged;
                ClearSelection();
                rateGrid.SelectionChanged += OnSelectedRateChanged;
            }
        }
    }
}
