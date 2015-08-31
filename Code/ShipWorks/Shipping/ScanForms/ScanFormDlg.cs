using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Divelements.SandGrid;
using Divelements.SandGrid.Specialized;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.ScanForms
{
    /// <summary>
    /// Window for creating an endicia scan form
    /// </summary>
    public partial class ScanFormDlg : Form
    {
        Font fontStrikeout;

        readonly List<ShipmentEntity> shipments;
        readonly IScanFormCarrierAccount carrierAccount;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanFormDlg(List<ShipmentEntity> shipments, IScanFormCarrierAccount carrierAccount)
        {
            InitializeComponent();

            WindowStateSaver.Manage(this);

            this.shipments = shipments;
            this.carrierAccount = carrierAccount;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            fontStrikeout = new Font(sandGrid.Font, FontStyle.Strikeout);

            sandGrid.Rows.Clear();

            foreach (ShipmentEntity shipment in shipments)
            {
                GridRow row = new GridRow(new GridCell[]
                    {
                        new GridCell(shipment.Order.OrderNumberComplete),
                        new GridCell(GetRecipientDisplay(shipment)),
                        new GridDateTimeCell(shipment.ProcessedDate.Value.ToLocalTime()),
                        new GridCell(ShippingManager.GetServiceUsed(shipment)),
                        new GridDecimalCell(shipment.ShipmentCost),
                        new GridCell(shipment.TrackingNumber)
                    });

                row.Checked = true;
                row.Tag = shipment;

                sandGrid.Rows.Add(row);
            }

            UpdateCheckedCount();
        }

        /// <summary>
        /// Get the text to display for the recipient column for the shipment
        /// </summary>
        private string GetRecipientDisplay(ShipmentEntity shipment)
        {
            StringBuilder sb = new StringBuilder(shipment.ShipLastName);

            if (shipment.ShipCountryCode == "US")
            {
                sb.AppendFormat(", {0}", shipment.ShipPostalCode);
            }
            else
            {
                sb.AppendFormat(", {0}", Geography.GetCountryName(shipment.ShipCountryCode));
            }

            return sb.ToString();
        }

        /// <summary>
        /// A checkbox value has been changed
        /// </summary>
        private void OnCheckChanged(object sender, GridRowCheckEventArgs e)
        {
            UpdateCheckedRowDisplay(e.Row);

            UpdateCheckedCount();
        }

        /// <summary>
        /// Update the display of how many are selected
        /// </summary>
        private void UpdateCheckedCount()
        {
            int count = sandGrid.Rows.Cast<GridRow>().Count(r => r.Checked);

            labelSelected.Text = string.Format("{0} shipments selected.", count);

            createScan.Enabled = count > 0;
        }

        /// <summary>
        /// Update the GridRow UI display for the given action based on it's enabled state
        /// </summary>
        private void UpdateCheckedRowDisplay(GridRow gridRow)
        {
            Color color = gridRow.Checked ? sandGrid.ForeColor : Color.DimGray;
            Font font = gridRow.Checked ? sandGrid.Font : fontStrikeout;

            // Apply the font
            gridRow.Font = font;

            // Apply the color
            foreach (GridCell cell in gridRow.Cells)
            {
                cell.ForeColor = color;
            }
        }

        /// <summary>
        /// Create a SCAN form
        /// </summary>
        private void OnCreateScanForm(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                List<ShipmentEntity> selectedShipments = sandGrid.Rows.Cast<GridRow>().Where(r => r.Checked).Select(r => (ShipmentEntity)r.Tag).ToList();
                if (selectedShipments.Count > 0)
                {
                    ScanFormBatch scanFormBatch = new ScanFormBatch(carrierAccount, new DefaultScanFormPrinter(), new DefaultScanFormBatchShipmentRepository());
                    scanFormBatch.Create(selectedShipments);

                    // Allow the user to print all the scan forms at once by passing entire batch to the dialog
                    using (ScanFormSuccessDlg dlg = new ScanFormSuccessDlg(scanFormBatch))
                    {
                        dlg.ShowDialog(this);
                        DialogResult = DialogResult.OK;
                    }                    
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (fontStrikeout != null)
                {
                    fontStrikeout.Dispose();
                }
            }

            base.Dispose(disposing);
        }

    }
}
