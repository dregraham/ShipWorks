using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.UI;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.EquaShip
{
    /// <summary>
    /// Edit info for a EquaShip shipper
    /// </summary>
    public partial class EquaShipAccountEditorDlg : Form
    {
        EquaShipAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public EquaShipAccountEditorDlg(EquaShipAccountEntity account)
        {
            InitializeComponent();

            this.account = account;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            username.Text = account.Username;
            password.Text = SecureText.Decrypt(account.Password, account.Username);

            personControl.LoadEntity(new PersonAdapter(account, ""));

            if (account.Description != EquaShipAccountManager.GetDefaultDescription(account))
            {
                description.Text = account.Description;
            }

            description.PromptText = EquaShipAccountManager.GetDefaultDescription(account);

        }

        /// <summary>
        /// The address content of the shipper has been edited
        /// </summary>
        private void OnPersonContentChanged(object sender, EventArgs e)
        {
            personControl.SaveToEntity();
            description.PromptText = EquaShipAccountManager.GetDefaultDescription(account);
        }

        /// <summary>
        /// User is ready to save the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            personControl.SaveToEntity();

            if (username.Text.Trim().Length == 0)
            {
                MessageHelper.ShowError(this, "Enter an account username for the EquaShip account.");
                return;
            }

            if (password.Text.Trim().Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a password for the EquaShip account.");
                return;
            }

            if (account.FirstName.Length == 0 || account.LastName.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a first and last name for the shipper.");
                return;
            }

            if (account.Street1.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a street address for the shipper.");
                return;
            }

            if (description.Text.Trim().Length > 0)
            {
                account.Description = description.Text.Trim();
            }
            else
            {
                account.Description = EquaShipAccountManager.GetDefaultDescription(account);
            }

            account.Username = username.Text.Trim();
            account.Password = SecureText.Encrypt(password.Text.Trim(), account.Username);

            try
            {
                EquaShipClient.TestConnection(account);

                EquaShipClient.GetDropOffLocations(account);
            }
            catch (EquaShipException)
            {
                MessageHelper.ShowError(this, "Unable to verify the username and password provided.");
                return;
            }

            try
            {
                EquaShipAccountManager.SaveAccount(account);

                DialogResult = DialogResult.OK;
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this, "Your changes cannot be saved because another use has deleted the shipper.");

                DialogResult = DialogResult.Abort;
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // Rollback changes if not saved
            if (DialogResult != DialogResult.OK)
            {
                account.RollbackChanges();
            }
        }
    }
}
