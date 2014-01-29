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
        /// Constructor
        /// </summary>
        public RateControl()
        {
            InitializeComponent();

            sandGrid.Rows.Clear();

            gridColumnSelect.ButtonClicked += OnSelectRate;

            this.ReloadRatesRequired += OnReloadRatesRequired;
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
        private IEnumerable<RateFootnoteControl> CurrentFootnotes
        {
            get { return panelFootnote.Controls.OfType<RateFootnoteControl>().ToList(); }
        }

        /// <summary>
        /// Load the rates into the control
        /// </summary>
        public void LoadRates(RateGroup rateGroup)
        {
            sandGrid.Rows.Clear();

            if (rateGroup.Carrier == ShipmentTypeCode.BestRate)
            {
                gridColumnProvider.Visible = true;
            }

            foreach (RateResult rate in rateGroup.Rates)
            {
                Image providerLogo = EnumHelper.GetImage(rate.ShipmentType);
                if (rate.ShipmentType == ShipmentTypeCode.Other)
                {
                    providerLogo = ShippingIcons.truck_blue;
                }

                GridRow row = new GridRow(new[]
                {
                    new GridCell(providerLogo ?? ShippingIcons.other),
                    new GridCell(rate.Description),
                    new GridCell(rate.Days),
                    new GridCell(rate.Selectable ? rate.Amount.ToString("c") : "", rate.AmountFootnote),
                    new GridHyperlinkCell(rate.SelectionText)
                }) { Tag = rate };

                sandGrid.Rows.Add(row);
            }
            
            panelOutOfDate.Visible = rateGroup.Rates.Count > 0 && rateGroup.OutOfDate;

            UpdateFootnotes(rateGroup);
        }

        /// <summary>
        /// Resets the footnotes with what are contained in the specified rate group
        /// </summary>
        private void UpdateFootnotes(RateGroup rateGroup)
        {
            RemoveFootnotes(CurrentFootnotes);

            if (rateGroup == null)
            {
                panelFootnote.Visible = false;
            }
            else
            {
                AddFootnotes(rateGroup.FootnoteFactories);
            }
        }

        /// <summary>
        /// Removes the footnotes.
        /// </summary>
        private void RemoveFootnotes(IEnumerable<RateFootnoteControl> previousFootnotes)
        {
            foreach (RateFootnoteControl previousFootnote in previousFootnotes)
            {
                previousFootnote.RateCriteriaChanged -= new EventHandler(OnFootnoteRateCriteriaChanged);
                panelFootnote.Controls.Remove(previousFootnote);
                previousFootnote.Dispose();
            }

            panelFootnote.Visible = false;
        }

        /// <summary>
        /// Adds the footnotes from the .
        /// </summary>
        /// <param name="footnoteFactories">The footnote factories.</param>
        private void AddFootnotes(IEnumerable<IRateFootnoteFactory> footnoteFactories)
        {
            panelFootnote.Height = 0;
            int y = 0;

            foreach (IRateFootnoteFactory factory in footnoteFactories)
            {
                RateFootnoteControl footnote = factory.CreateFootnote(footnoteParameters);

                panelFootnote.Controls.Add(footnote);
                footnote.Location = new Point(0, y);
                panelFootnote.Visible = true;

                footnote.RateCriteriaChanged += new EventHandler(OnFootnoteRateCriteriaChanged);
                panelFootnote.Height += footnote.Height;
                y += footnote.Height;
            }
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
        /// Return the displayed height of the footnote section
        /// </summary>
        public int FootnoteHeight
        {
            get { return panelFootnote.Visible ? panelFootnote.Height : 0; }
        }
    }
}
