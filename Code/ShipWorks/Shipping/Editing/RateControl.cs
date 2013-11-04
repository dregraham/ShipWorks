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

        private ToolTip toolTip;
        private int hoverRow = -1;

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

            PreviousFootnotes.ForEach(f => f.Dispose());
            panelFootnote.Visible = false;
        }

        /// <summary>
        /// Clear the previous content of the footnote control
        /// </summary>
        private List<RateFootnoteControl> PreviousFootnotes
        {
            get { return panelFootnote.Controls.OfType<RateFootnoteControl>().ToList(); }
        }

        /// <summary>
        /// Load the rates into the control
        /// </summary>
        public void LoadRates(RateGroup rateGroup)
        {
            var previousFootnotes = PreviousFootnotes;
            sandGrid.Rows.Clear();

            foreach (RateResult rate in rateGroup.Rates)
            {
                GridRow row = new GridRow(new GridCell[]
                {
                    new GridCell(rate.Description),
                    new GridCell(rate.Days),
                    new GridCell(rate.Selectable ? rate.Amount.ToString("c") : "", rate.AmountFootnote),
                    new GridHyperlinkCell(rate.Selectable ? "Select" : "")
                });

                row.Tag = rate;

                sandGrid.Rows.Add(row);
            }

            panelOutOfDate.Visible = rateGroup.Rates.Count > 0 && rateGroup.OutOfDate;

            RateFootnoteControl footnote = (rateGroup.FootnoteCreator != null) ? rateGroup.FootnoteCreator() : null;

            if (footnote != null)
            {
                footnote.Dock = DockStyle.Fill;
                panelFootnote.Controls.Add(footnote);
                panelFootnote.Visible = true;

                footnote.RateCriteriaChanged += new EventHandler(OnFootnoteRateCriteriaChanged);
            }
            else
            {
                panelFootnote.Visible = false;
            }

            foreach (RateFootnoteControl previousFootnote in previousFootnotes)
            {
                previousFootnote.RateCriteriaChanged -= new EventHandler(OnFootnoteRateCriteriaChanged);
                previousFootnote.Dispose();
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

        /// <summary>
        /// Called when mouse hovers over SandGrid
        /// </summary>
        private void OnSandGridMouseHover(object sender, EventArgs e)
        {
            if (toolTip == null)
            {
                toolTip = new ToolTip();
            }
        }

        /// <summary>
        /// Called when [sand grid mouse move].
        /// </summary>
        private void OnSandGridMouseMove(object sender, MouseEventArgs e)
        {
            GridRow rowMouseIsOn = GetRowMouseIsOn();

            if (rowMouseIsOn != null && rowMouseIsOn.Index != hoverRow && toolTip != null)
            {
                toolTip.Active = false;

                string hoverText = GetHoverText(rowMouseIsOn);
                
                if (!string.IsNullOrEmpty(hoverText))
                {
                    toolTip.SetToolTip(sandGrid, hoverText);
                    toolTip.Active = true;

                    hoverRow = rowMouseIsOn.Index;
                }
            }
        }

        /// <summary>
        /// Gets the hover text for specific row.
        /// </summary>
        private string GetHoverText(GridRow row)
        {
            RateResult rate = row.Tag as RateResult;

            string hoverText = string.Empty;

            if (rate != null)
            {
                hoverText = rate.HoverText;
            }

            return hoverText;
        }

        /// <summary>
        /// Gets the row the mouse is on.
        /// </summary>
        private GridRow GetRowMouseIsOn()
        {
            int mouseYLocation = Cursor.Position.Y - sandGrid.PointToScreen(sandGrid.Location).Y;

            foreach (GridRow row in sandGrid.Rows)
            {
                if (row.Bounds.Top <= mouseYLocation && row.Bounds.Bottom >= mouseYLocation)
                {
                    return row;
                }
            }

            return null;
        }
    }
}
