using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    public partial class iParcelAccountEditorDlg : Form
    {
        private readonly IParcelAccountEntity iParcelAccount;

        public iParcelAccountEditorDlg(IParcelAccountEntity iParcelAccount)
        {
            InitializeComponent();

            this.iParcelAccount = iParcelAccount;
        }

        /// <summary>
        /// Called when the form is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnLoad(object sender, EventArgs e)
        {
            // Create the iParcelCredentials so we can get the decrypted password
            iParcelCredentials credentials = new iParcelCredentials(this.iParcelAccount.Username, this.iParcelAccount.Password, true, new iParcelServiceGateway());
            username.Text = credentials.Username;
            password.Text = credentials.DecryptedPassword;

            description.Text = iParcelAccount.Description;
            description.PromptText = iParcelAccountManager.GetDefaultDescription(iParcelAccount);

            PersonAdapter personAdapter = new PersonAdapter(iParcelAccount, string.Empty);
            contactInformation.LoadEntity(personAdapter);
        }

        /// <summary>
        /// Called when the person control's content has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnPersonContentChanged(object sender, EventArgs e)
        {
            contactInformation.SaveToEntity();
        }


        /// <summary>
        /// Called when the OK button is clicked and saves the account data.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnOK(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                
                iParcelCredentials credentials = new iParcelCredentials(username.Text, password.Text, false, new iParcelServiceGateway());
                credentials.Validate();
                
                // Credentials are valid, so save the credentials and contact info to the entity
                credentials.SaveToEntity(iParcelAccount);
                contactInformation.SaveToEntity();

                if (description.Text.Trim().Length > 0)
                {
                    iParcelAccount.Description = description.Text.Trim();
                }
                else
                {
                    iParcelAccount.Description = iParcelAccountManager.GetDefaultDescription(iParcelAccount);
                }

                iParcelAccountManager.SaveAccount(iParcelAccount);
                DialogResult = DialogResult.OK;

            }
            catch (iParcelException exception)
            {
                MessageHelper.ShowError(this, exception.Message);
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this, "Your changes cannot be saved because another use has deleted the account.");
                DialogResult = DialogResult.Abort;
            }
        }
    }
}
