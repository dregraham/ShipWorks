﻿using System.Diagnostics;
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

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Reusable control for displaying rates for any shipping service
    /// </summary>
    public partial class RateControl : UserControl
    {
        // This is the maximum rates to display when the control is configured 
        // with ShowAllRates = false
        private const short RestrictedRateCount = 5;

        private FootnoteParameters footnoteParameters;

        private readonly object syncLock = new object();

        /// <summary>
        /// Raised when the user clicks "Select" to select a rate
        /// </summary>
        public event RateSelectedEventHandler RateSelected;

        public event RateSelectedEventHandler ConfigureRateClicked;

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
            sandGrid.Rows.Clear();

            gridColumnSelect.ButtonClicked += OnConfigureRateClicked;

            ShowAllRates = true;
        }

        
        /// <summary>
        /// Initialize the rate control
        /// </summary>
        public void Initialize(FootnoteParameters parameters)
        {
            footnoteParameters = parameters;
        }

        /// <summary>
        /// Gets the rate group loaded in the control. If a rate group has not been loaded
        /// into the control, a group without any rate results is returned.
        /// </summary>
        public RateGroup RateGroup { get; private set; }

        /// <summary>
        /// Return the displayed height of the footnote section
        /// </summary>
        public int FootnoteHeight
        {
            get { return panelFootnote.Visible ? panelFootnote.Height : 0; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [show the configure link].
        /// </summary>
        public bool ShowConfigureLink { get; set; }

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
                    IEnumerable<GridRow> selectedRows = sandGrid.SelectedElements.OfType<GridRow>().ToList();
                    if (selectedRows.Any())
                    {
                        selectedRate = selectedRows.Select(s => s.Tag as RateResult).ToList().FirstOrDefault(f => f.Selectable);
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
                Debug.Assert(sandGrid.EmptyText != null, "sandGrid.EmptyText != null");

                sandGrid.EmptyText = emptyReason;

                if (sandGrid.Rows.Count > 0)
                {
                    sandGrid.Rows.Clear();
                }

                panelOutOfDate.Visible = false;
                UpdateFootnotes(rateGroup);
            }
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
        public void ShowSpinner()
        {
            loadingRatesPanel.BringToFront();
        }

        /// <summary>
        /// Hides the spinner.
        /// </summary>
        public void HideSpinner()
        {
            loadingRatesPanel.SendToBack();
        }

        /// <summary>
        /// Load the rates into the control
        /// </summary>
        public void LoadRates(RateGroup rateGroup)
        {
            lock (syncLock)
            {
                RateGroup = rateGroup;
                sandGrid.Rows.Clear();

                gridColumnSelect.Visible = ShowConfigureLink;

                List<RateResult> ratesToShow = ShowAllRates ? rateGroup.Rates : rateGroup.Rates.Take(RestrictedRateCount).ToList();

                foreach (RateResult rate in ratesToShow)
                {
                    GridRow row = new GridRow(new[]
                    {
                        new GridCell(GetProviderLogo(rate)),
                        new GridCell(rate.Description),
                        new GridCell(rate.Days),
                        new GridCell(rate.Selectable ? rate.Amount.ToString("c") : "", rate.AmountFootnote)
                    }) { Tag = rate };

                    if (ShowConfigureLink && rate.Selectable)
                    {
                        row.Cells.Add(new GridHyperlinkCell("Configure"));
                    }

                    sandGrid.Rows.Add(row);
                }
                
                AddShowMoreRatesRow();

                panelOutOfDate.Visible = rateGroup.Rates.Count > 0 && rateGroup.OutOfDate;
                UpdateFootnotes(rateGroup);
            }
        }

        /// <summary>
        /// Adds the show more rates row if the control is configured to not show all rates and 
        /// the list of rates exceeds the restricted rate count.
        /// </summary>
        /// <param name="rateGroup">The rate group.</param>
        private void AddShowMoreRatesRow()
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
                            // The user wants to expand the rates, so set the ShowAllRates
                            // to true and load the full list of original rates
                            ShowAllRates = true;
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
                    new GridHyperlinkCell("More...")
                }) { Tag = showMoreRatesRateResult };

                sandGrid.Rows.Add(row);
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
                if (rateIndex >= 0)
                {
                    if (sandGrid.Rows.Count > rateIndex)
                    {
                        GridRow selectedRow = sandGrid.Rows[rateIndex];
                        selectedRow.EnsureVisible();

                        foreach (GridCell cell in selectedRow.Cells)
                        {
                            // Highlight the selected row otherwise it's just a light shade of gray
                            // that can be hard to tell which row is selected
                            cell.BackColor = Color.DodgerBlue;
                            cell.ForeColor = Color.White;
                        }

                        sandGrid.SelectRow(selectedRow);
                    }
                }
            }
        }

        /// <summary>
        /// Clears any row selection in the grid.
        /// </summary>
        public void ClearSelection()
        {
            // Remove any highlighting from previously selected rows
            foreach (GridRow row in sandGrid.Rows)
            {
                foreach (GridCell cell in row.Cells)
                {
                    cell.BackColor = Color.Empty;
                }
            }

            sandGrid.SelectedElements.Clear();
        }

        /// <summary>
        /// Gets the provider logo.
        /// </summary>
        /// <param name="rate">The rate.</param>
        /// <returns>The provider logo image.</returns>
        private static Image GetProviderLogo(RateResult rate)
        {
            Image providerLogo = EnumHelper.GetImage(rate.ShipmentType);

            // If a postal provider, show USPS logo:
            if (ShipmentTypeManager.IsPostal(rate.ShipmentType) && rate.IsCounterRate)
            {
                providerLogo = ShippingIcons.usps;
            }

            // If non-competitive, show the brown truck.
            NoncompetitiveRateResult noncompetitiveRateResult = rate as NoncompetitiveRateResult;
            if (noncompetitiveRateResult != null)
            {
                providerLogo = ShippingIcons.truck_brown;
            }

            // If null, return the 'other' icon
            return providerLogo ?? ShippingIcons.other;
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

        private void OnConfigureRateClicked(object sender, GridRowColumnEventArgs e)
        {
            RateResult rate = e.Row.Tag as RateResult;
            if (!rate.Selectable)
            {
                return;
            }

            if (ConfigureRateClicked != null)
            {
                ConfigureRateClicked(this, new RateSelectedEventArgs(rate));
            }
        }

        /// <summary>
        /// Event raised indicating that a full reload of the rates is required
        /// </summary>
        protected void OnReloadRatesRequired(object sender, EventArgs e)
        {
            if (ReloadRatesRequired != null)
            {
                ReloadRatesRequired(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the [selected rate row has changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnSelectedRateChanged(object sender, SelectionChangedEventArgs e)
        {
            GridRow selectedRow = e.Grid.SelectedElements.OfType<GridRow>().FirstOrDefault();

            if (SelectedRate != null)
            {
                if (RateSelected != null)
                {
                    RateSelected(this, new RateSelectedEventArgs(SelectedRate));
                }
            }
            else
            {
                this.sandGrid.SelectionChanged -= OnSelectedRateChanged;
                ClearSelection();
                this.sandGrid.SelectionChanged += OnSelectedRateChanged;
            }
        }

    }
}
