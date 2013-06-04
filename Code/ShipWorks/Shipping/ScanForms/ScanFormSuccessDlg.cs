using System;
using System.Windows.Forms;

namespace ShipWorks.Shipping.ScanForms
{
    /// <summary>
    /// Window that displays after the succesful creation of a SCAN form
    /// </summary>
    public partial class ScanFormSuccessDlg : Form
    {
        private readonly ScanFormBatch scanFormBatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanFormSuccessDlg" /> class.
        /// </summary>
        /// <param name="scanFormBatch">The scan form batch.</param>
        public ScanFormSuccessDlg(ScanFormBatch scanFormBatch)
        {
            InitializeComponent();

            this.scanFormBatch = scanFormBatch;
        }

        /// <summary>
        /// Print the scan form
        /// </summary>
        private void OnPrint(object sender, EventArgs e)
        {
            if (scanFormBatch.Print(this))
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}
