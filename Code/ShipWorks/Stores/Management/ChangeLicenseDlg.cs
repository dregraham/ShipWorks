using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Window for entering a license into ShipWorks.  If successful, the license is updated in the StoreEntity, 
    /// but not saved to the database.
    /// </summary>
    public partial class ChangeLicenseDlg : Form
    {
        StoreEntity store;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChangeLicenseDlg(StoreEntity store)
        {
            InitializeComponent();

            this.store = store;
        }

        /// <summary>
        /// User is ready to change the license
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            string storeLicense = licenseKey.Text.Trim();

            // Attempt to activate the license
            LicenseActivationState licenseState = LicenseActivationHelper.ActivateAndSetLicense(store, storeLicense, this);

            if (licenseState == LicenseActivationState.Active)
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}