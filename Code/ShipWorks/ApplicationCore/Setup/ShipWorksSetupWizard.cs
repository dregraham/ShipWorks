using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Utility;
using ShipWorks.Stores;
using ShipWorks.Templates.Media;
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
        /// <summary>
        /// Constructor
        /// </summary>
        private ShipWorksSetupWizard()
        {
            InitializeComponent();
        }

        #region Opening

        /// <summary>
        /// Run the setup wizard.  Will return false if the user doesn't have permissions, the user canceled, or if the Wizard was not able to run because
        /// it was already running on another computer.
        /// </summary>
        public static bool RunWizard(IWin32Window owner)
        {
            // See if we have permissions
            if (!(UserSession.Security.HasPermission(PermissionType.ManageStores) && 
                  UserSession.Security.HasPermission(PermissionType.ShipmentsManageSettings)))
            {
                MessageHelper.ShowInformation(owner, "An administrator must log on to setup to ShipWorks.");
                return false;
            }

            try
            {
                using (ShipWorksSetupLock wizardLock = new ShipWorksSetupLock())
                {
                    using (ShipWorksSetupWizard wizard = new ShipWorksSetupWizard())
                    {
                        // If it was succesful, make sure our local list of stores is refreshed
                        if (wizard.ShowDialog(owner) == DialogResult.OK)
                        {
                            StoreManager.CheckForChanges();

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (SqlAppResourceLockException)
            {
                MessageHelper.ShowInformation(owner, "Another user is already setting up ShipWorks.  This can only be done on one computer at a time.");
                return false;
            }
        }

        /// <summary>
        /// Designed to be called from the last step of another wizard where a brand new database and user account was just created, this makes it look to the user
        /// like the ShipWorks Setup wizard is a seamless continuation of the previous wizard.  The DialogResult of the ShipWorks Setup is used as the DialogResult
        /// that closes the original wizard.
        /// </summary>
        public static void ContinueAfterCreateDatabase(WizardForm originalWizard, string username, string password)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Initialize the session
            UserSession.InitializeForCurrentDatabase();

            // Logon the user
            UserSession.Logon(username, password, true);

            // Initialize the session
            UserManager.InitializeForCurrentUser();
            UserSession.InitializeForCurrentSession();

            originalWizard.BeginInvoke(new MethodInvoker(originalWizard.Hide));

            // Run the setup wizard
            bool complete = RunWizard(originalWizard);

            // If the wizard didn't complete, then the we can't exit this with the user still looking like they were logged in
            if (!complete)
            {
                UserSession.Logoff(false);
            }

            // Counts as a cancel on the original wizard if they didn't complete the setup.
            originalWizard.DialogResult = complete ? DialogResult.OK : DialogResult.Cancel;
        }

        #endregion

        #region Label Printer

        /// <summary>
        /// Stepping into the label printer page
        /// </summary>
        private void OnSteppingIntoLabelPrinter(object sender, WizardSteppingIntoEventArgs e)
        {
            labelPrinter.LoadPrinters(labelPrinter.PrinterName, -1, PrinterSelectionInvalidPrinterBehavior.AlwaysPreserve);
        }

        /// <summary>
        /// The selected printer has changed
        /// </summary>
        private void OnPrinterChanged(object sender, EventArgs e)
        {
            printerFormatControl.Visible = !string.IsNullOrEmpty(labelPrinter.PrinterName);
            printerFormatControl.PrinterName = labelPrinter.PrinterName;
        }

        #endregion

    }
}
