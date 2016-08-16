using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using ShipWorks.Stores.Platforms;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Window for guiding the user through changing license activation
    /// </summary>
    public partial class ChangeActivationDlg : Form
    {
        StoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChangeActivationDlg(StoreEntity store)
        {
            InitializeComponent();

            this.store = store;
        }

        /// <summary>
        /// Click the account link to change activation
        /// </summary>
        private void OnClickAccountLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("https://www.interapptive.com/account", this);
        }

        /// <summary>
        /// Change the activation
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                LicenseAccountDetail accountDetail = TangoWebClient.ActivateLicense(store.License, store);

                // We dont want to use the default LicenseActivationHelper message that shows the reset link, because that would be confusing
                // in the context of this window.
                if (accountDetail.ActivationState == LicenseActivationState.ActiveElsewhere)
                {
                    MessageHelper.ShowInformation(this, "The license has not yet been reset, and is still activated to another store.");
                    return;
                }

                if (accountDetail.ActivationState != LicenseActivationState.Active)
                {
                    string activationStateMessage = LicenseActivationHelper.GetActivationStateMessage(accountDetail);
                    if (string.IsNullOrEmpty(activationStateMessage))
                    {
                        MessageHelper.ShowError(this, activationStateMessage);
                    }
                    return;
                }

                DialogResult = DialogResult.OK;
            }
            catch (ShipWorksLicenseException ex)
            {
                MessageHelper.ShowError(this,
                    "There is a problem with the license that was entered.\n\n" +
                    "Details: " + ex.Message);
            }
            catch (TangoException ex)
            {
                MessageHelper.ShowError(this,
                    "The license entered is valid, but ShipWorks was unable to connect\n" +
                    "to the license server to determine the status of the license.\n\n" +
                    "Details: " + ex.Message);
            }
        }
    }
}