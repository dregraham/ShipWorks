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
        private FootnoteParameters footnoteParameters;

        /// <summary>
        /// Raised when the user clicks "Select" to select a rate
        /// </summary>
        public event RateSelectedEventHandler RateSelected;

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

            gridColumnSelect.ButtonClicked += OnSelectRate;
        }

        
        /// <summary>
        /// Initialize the rate control
        /// </summary>
        public void Initialize(FootnoteParameters parameters)
        {
            this.footnoteParameters = parameters;
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
            Debug.Assert(sandGrid.EmptyText != null, "sandGrid.EmptyText != null");
                
            sandGrid.EmptyText = emptyReason;

            if (sandGrid.Rows.Count > 0)
            {
                sandGrid.Rows.Clear();
            }

            panelOutOfDate.Visible = false;
            UpdateFootnotes(rateGroup);
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
            RateGroup = rateGroup;
            sandGrid.Rows.Clear();

            foreach (RateResult rate in rateGroup.Rates)
            {
                GridRow row = new GridRow(new[]
                {
                    new GridCell(GetProviderLogo(rate)),
                    new GridCell(rate.Description),
                    new GridCell(rate.Days),
                    new GridCell(rate.Selectable ? rate.Amount.ToString("c") : "", rate.AmountFootnote),
                    new GridHyperlinkCell(rate.SelectionText)
                }) { Tag = rate };

                sandGrid.Rows.Add(row);
            }

            // Add a show more rates row to the grid if allowed.
            AddShowMoreRatesRow(rateGroup);

            panelOutOfDate.Visible = rateGroup.Rates.Count > 0 && rateGroup.OutOfDate;

            UpdateFootnotes(rateGroup);
        }

        /// <summary>
        /// If the shipment type is BestRate and the rateGroup.ShowMoreRateResult is not null,
        /// adds a row to the grid to allow showing more rates.
        /// </summary>
        private void AddShowMoreRatesRow(RateGroup rateGroup)
        {
            // If we are best rate and there is a show more rates rate result, add a row to the grid
            if (rateGroup.Carrier == ShipmentTypeCode.BestRate && rateGroup.ShowMoreRateResult != null)
            {
                RateResult rate = rateGroup.ShowMoreRateResult;
                RateGroup originaRateGroup = rate.Tag as RateGroup;

                // If we go from BestRate to another carrier and back to BestRate, the rate.Tag is a best rate result tag, 
                // so we skip creating the best rate result tag.
                if (originaRateGroup != null)
                {
                    rate.Tag = new BestRateResultTag()
                    {
                        RateSelectionDelegate = entity =>
                        {
                            originaRateGroup.ShowMoreRateResult = null;
                            LoadRates(originaRateGroup);
                        }
                    };
                }

                GridRow row = new GridRow(new[]
                {
                    new GridCell(""),
                    new GridHyperlinkCell(rate.Description),
                    new GridCell(""),
                    new GridCell(""),
                    new GridHyperlinkCell("More...")
                }) { Tag = rate };

                sandGrid.Rows.Add(row);
            }
        }

        /// <summary>
        /// Gets the rate group loaded in the control. If a rate group has not been loaded
        /// into the control, a group without any rate results is returned.
        /// </summary>
        public RateGroup RateGroup { get; private set; }

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

        /// <summary>
        /// User has clicked to select a rate
        /// </summary>
        private void OnSelectRate(object sender, GridRowColumnEventArgs e)
        {
            RateResult rate = e.Row.Tag as RateResult;
            if (!rate.Selectable)
            {
                return;
            }

            if (RateSelected != null)
            {
                RateSelected(this, new RateSelectedEventArgs(rate));
            }
        }

        /// <summary>
        /// Event raised indicating that a full reload of the rates is required
        /// </summary>
        protected void OnReloadRatesRequired(object sender, EventArgs e)
        {
            if (this.ReloadRatesRequired != null)
            {
                ReloadRatesRequired(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Return the displayed height of the footnote section
        /// </summary>
        public int FootnoteHeight
        {
            get { return panelFootnote.Visible ? panelFootnote.Height : 0; }
        }
    }
}
