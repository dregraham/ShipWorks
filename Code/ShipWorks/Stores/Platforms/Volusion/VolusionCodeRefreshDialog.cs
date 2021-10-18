using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// Window for importing volusion payment methods or shipping methods
    /// </summary>
    public partial class VolusionCodeRefreshDialog : Form
    {
        // store being imported for
        VolusionStoreEntity store;

        // payment or shipping method import
        VolusionCodeImportMode importMode;

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionCodeRefreshDialog(VolusionStoreEntity store, VolusionCodeImportMode importMode)
        {
            InitializeComponent();

            this.store = store;
            this.importMode = importMode;

            if (importMode == VolusionCodeImportMode.PaymentMethods)
            {
                Text = "Import Payment Methods";

                instructionsLabel.Text = instructionsLabel.Text.Replace("<type>", "payment methods");
                fileHeaderLabel.Text = fileHeaderLabel.Text.Replace("<type>", "Payment methods");
            }
            else
            {
                Text = "Import Shipping Methods";

                instructionsLabel.Text = instructionsLabel.Text.Replace("<type>", "shipping methods");
                fileHeaderLabel.Text = fileHeaderLabel.Text.Replace("<type>", "Shipping methods");
            }
        }

        /// <summary>
        /// Clicked to import the CSV file
        /// </summary>
        private void OnImport(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string file = openFileDialog.FileName;

                if (!File.Exists(file))
                {
                    MessageHelper.ShowError(this, "The specified file does not exist.");
                    return;
                }

                if (DoImport(file))
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }

        /// <summary>
        /// Perform the import from CSV
        /// </summary>
        private bool DoImport(string fileName)
        {
            try
            {
                string fileData = string.Empty;
                using (var stream = File.OpenText(fileName))
                {
                    fileData = stream.ReadToEnd();
                }

                if (importMode == VolusionCodeImportMode.PaymentMethods)
                {
                    VolusionPaymentMethods methodsProcessor = new VolusionPaymentMethods(store);
                    methodsProcessor.ImportCsv(fileData);
                }
                else
                {
                    VolusionShippingMethods methodsProcessor = new VolusionShippingMethods(store);
                    methodsProcessor.ImportCsv(fileData);
                }

                return true;
            }
            catch (IOException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                MessageHelper.ShowError(this, String.Format("Access is denied to '{0}'", fileName));
            }
            catch (VolusionException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }

            return false;
        }
    }
}
