using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Utility;
using ShipWorks.Properties;
using ShipWorks.Stores;
using ShipWorks.Templates.Media;
using ShipWorks.UI.Controls;
using ShipWorks.UI.Wizard;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.ApplicationCore.Setup
{
    /// <summary>
    /// Setup wizard for getting started with ShipWorks
    /// </summary>
    public partial class ShipWorksSetupWizard : WizardForm
    {
        PrinterType labelPrinterType;

        /// <summary>
        /// Constructor
        /// </summary>
        private ShipWorksSetupWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            LoadStoreTypes();
        }

        #region Select Printers

        /// <summary>
        /// Stepping into the select printers page
        /// </summary>
        private void OnSteppingIntoSelectPrinter(object sender, WizardSteppingIntoEventArgs e)
        {
            standardPrinter.LoadPrinters(standardPrinter.PrinterName, -1, PrinterSelectionInvalidPrinterBehavior.AlwaysPreserve);
            labelPrinter.LoadPrinters(labelPrinter.PrinterName, -1, PrinterSelectionInvalidPrinterBehavior.AlwaysPreserve);
        }

        /// <summary>
        /// The selected label printer has changed
        /// </summary>
        private void OnLabelPrinterChanged(object sender, EventArgs e)
        {
            printerTypeControl.Visible = !string.IsNullOrEmpty(labelPrinter.PrinterName);
            printerTypeControl.PrinterName = labelPrinter.PrinterName;
        }

        /// <summary>
        /// Stepping next from the printers page
        /// </summary>
        private void OnStepNextSelectPrinters(object sender, WizardStepEventArgs e)
        {
            // If there are printers on the system, make sure they've selected one
            if (PrintUtility.InstalledPrinters.Count > 0)
            {
                if (string.IsNullOrEmpty(labelPrinter.PrinterName) || string.IsNullOrEmpty(standardPrinter.PrinterName))
                {
                    MessageHelper.ShowInformation(this, "Please select your printers before continuing.");
                    e.NextPage = CurrentPage;
                    return;
                }
            }

            labelPrinterType = printerTypeControl.GetPrinterType();
            if (labelPrinterType == null)
            {
                e.NextPage = CurrentPage;
                return;
            }
        }

        #endregion

        #region Online Store

        /// <summary>
        /// Load the list of store types to choose from
        /// </summary>
        private void LoadStoreTypes()
        {
            comboStoreType.Items.Clear();
            comboStoreType.Items.Add("Choose...");

            // Add each store type as a radio
            foreach (StoreType storeType in StoreTypeManager.StoreTypes)
            {
                comboStoreType.Items.Add(new ImageComboBoxItem(storeType.StoreTypeName, storeType, EnumHelper.GetImage(storeType.TypeCode)));
            }

            comboStoreType.SelectedIndex = 0;
        }

        /// <summary>
        /// Changing the option to connect to an online store now or later
        /// </summary>
        private void OnChangeStoreOption(object sender, EventArgs e)
        {
            comboStoreType.Enabled = radioStoreConnect.Checked;
        }

        /// <summary>
        /// Stepping next from the online store page
        /// </summary>
        private void OnStepNextOnlineStore(object sender, WizardStepEventArgs e)
        {

        }

        #endregion
    }
}
