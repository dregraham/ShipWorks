using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid;
using Divelements.SandGrid.Specialized;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Reusable control for displaying rates for any shipping service
    /// </summary>
    public partial class RateControl : UserControl
    {
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

            sandGrid.Rows.Clear();

            gridColumnSelect.ButtonClicked += new EventHandler<GridRowColumnEventArgs>(OnSelectRate);            
        }

        /// <summary>
        /// Clear the rates in the grid and display the given reason for having no rates displayed.
        /// </summary>
        public void ClearRates(string emptyReason)
        {
            if (sandGrid.EmptyText != emptyReason)
            {
                sandGrid.EmptyText = emptyReason;
            }

            if (sandGrid.Rows.Count > 0)
            {
                sandGrid.Rows.Clear();
            }

            panelOutOfDate.Visible = false;

            CurrentFootnotes.ForEach(f => f.Dispose());
            panelFootnote.Visible = false;
        }

        /// <summary>
        /// Clear the previous content of the footnote control
        /// </summary>
        private List<RateFootnoteControl> CurrentFootnotes
        {
            get { return panelFootnote.Controls.OfType<RateFootnoteControl>().ToList(); }
        }

        /// <summary>
        /// Load the rates into the control
        /// </summary>
        public void LoadRates(RateGroup rateGroup)
        {

            sandGrid.Rows.Clear();

            bool isBestRate = rateGroup.Carrier == ShipmentTypeCode.BestRate;
            if (!isBestRate)
            {
                gridColumnCarrier.Visible = false;
            }

            foreach (RateResult rate in rateGroup.Rates)
            {
                string serviceDescription = rate.Description;
                if (isBestRate && rate.CarrierDescription != null)
                {
                    int indexOfCarrierNameInServiceDescription = rate.Description.IndexOf(rate.CarrierDescription);
                    serviceDescription = (indexOfCarrierNameInServiceDescription < 0) ? rate.Description : rate.Description.Remove(indexOfCarrierNameInServiceDescription, rate.CarrierDescription.Length + 1);
                }
                GridRow row = new GridRow(new GridCell[]
                {
                    new GridCell(rate.CarrierDescription),
                    new GridCell(serviceDescription),
                    new GridCell(rate.Days),
                    new GridCell(rate.Selectable ? rate.Amount.ToString("c") : "", rate.AmountFootnote),
                    new GridHyperlinkCell(rate.Selectable ? "Select" : "")
                });
                
                row.Tag = rate;

                sandGrid.Rows.Add(row);
            }
            
            panelOutOfDate.Visible = rateGroup.Rates.Count > 0 && rateGroup.OutOfDate;

            RemoveFootnotes(CurrentFootnotes);

            AddFootnotes(rateGroup.FootnoteCreators);

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
        }

        /// <summary>
        /// Adds the footnotes.
        /// </summary>
        /// <param name="footNotes">The rate group.</param>
        private void AddFootnotes(IEnumerable<Func<RateFootnoteControl>> footNotes)
        {
            panelFootnote.Height = 0;
            int y = 0;
            foreach (RateFootnoteControl footnote in footNotes.Select(footnoteCreator => footnoteCreator()))
            {
                if (footnote != null)
                {
                    panelFootnote.Controls.Add(footnote);
                    footnote.Location = new Point(0, y);
                    panelFootnote.Visible = true;

                    footnote.RateCriteriaChanged += new EventHandler(OnFootnoteRateCriteriaChanged);
                    panelFootnote.Height += footnote.Height;
                    y += footnote.Height;
                }
                else
                {
                    panelFootnote.Visible = false;
                }
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
                // in the middle of this callstack.
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
