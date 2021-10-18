using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Volusion.WizardPages
{
    /// <summary>
    /// Wizard page for downloading Shipping Methods
    /// </summary>
    public partial class VolusionImportPage : VolusionAddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionImportPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Setpping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            // If we are doing autoconfig, and the shipping methods were successfully downloaded, skip the page
            if (SetupState.Configuration == VolusionSetupConfiguration.Automatic && 
                SetupState.AutoDownloadedShippingMethods && 
                SetupState.AutoDownloadedPaymentMethods)
            {
                e.Skip = true;
            }
        }

        /// <summary>
        /// User clicked to browse to their export file
        /// </summary>
        private void OnImportShippingMethods(object sender, EventArgs e)
        {
            VolusionStoreEntity store = GetStore<VolusionStoreEntity>();

            if (openShippingFileDlg.ShowDialog(this) == DialogResult.OK)
            {
                string file = openShippingFileDlg.FileName;

                if (!File.Exists(file))
                {
                    MessageHelper.ShowError(this, "The specified export file does not exist.");
                    return;
                }

                try
                {
                    string fileData = string.Empty;
                    using (var stream = File.OpenText(file))
                    {
                        fileData = stream.ReadToEnd();
                    }

                    VolusionShippingMethods methodsProcessor = new VolusionShippingMethods(store);
                    methodsProcessor.ImportCsv(fileData);

                    shippingSuccess.Visible = true;
                    MessageHelper.ShowInformation(this, "The shipping methods were successfully imported.");
                }
                catch (IOException ex)
                {
                    MessageHelper.ShowError(this, ex.Message);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageHelper.ShowError(this, String.Format("Access is denied to '{0}'", file));
                }
                catch (VolusionException ex)
                {
                    MessageHelper.ShowError(this, ex.Message);
                }
            }
        }

        /// <summary>
        /// Import the payment methods
        /// </summary>
        private void OnImportPayments(object sender, EventArgs e)
        {
            VolusionStoreEntity store = GetStore<VolusionStoreEntity>();

            if (openPaymentsFileDlg.ShowDialog(this) == DialogResult.OK)
            {
                string file = openPaymentsFileDlg.FileName;

                if (!File.Exists(file))
                {
                    MessageHelper.ShowError(this, "The specified export file does not exist.");
                    return;
                }

                try
                {
                    string fileData = string.Empty;
                    using (var stream = File.OpenText(file))
                    {
                        fileData = stream.ReadToEnd();
                    }

                    VolusionPaymentMethods methodsProcessor = new VolusionPaymentMethods(store);
                    methodsProcessor.ImportCsv(fileData);

                    paymentSuccess.Visible = true;
                    MessageHelper.ShowInformation(this, "The payment methods were successfully imported.");
                }
                catch (IOException ex)
                {
                    MessageHelper.ShowError(this, ex.Message);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageHelper.ShowError(this, String.Format("Access is denied to '{0}'", file));
                }
                catch (VolusionException ex)
                {
                    MessageHelper.ShowError(this, ex.Message);
                }
            }
        }
    }
}
