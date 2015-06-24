using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.Newegg.WizardPages
{
    public partial class NeweggAccountPage : AddStoreWizardPage
    {
        public NeweggAccountPage()
        {
            InitializeComponent();            
        }

        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            IEnumerable<ValidationError> errors = ValidateAccountSettings();
            if (errors.Count() > 0)
            {
                // The account settings provided did not pass validation, so just grab the first error message 
                // and show it to the user
                MessageHelper.ShowError(this, errors.First().ErrorMessage);
                e.NextPage = this;
                return;
            }
          
            Cursor.Current = Cursors.WaitCursor;

            NeweggStoreEntity store = GetStore<NeweggStoreEntity>();
            store.SellerID = storeSettingsControl.SellerId;
            store.SecretKey = storeSettingsControl.SecretKey;
            store.Channel = storeSettingsControl.Marketplace;

            // Let's bounce the store's connection settings off the Newegg API to confirm that they are correct
            try
            {
                NeweggWebClient webClient = new NeweggWebClient(store);
                if (!webClient.AreCredentialsValid())
                {
                    MessageHelper.ShowError(this, "ShipWorks could not communicate with Newegg using the connection settings provided.");
                    e.NextPage = this;
                    return;
                }
            }
            catch (NeweggException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                e.NextPage = this;
            }
        }

        /// <summary>
        /// A helper method that calidates the values of the various account controls 
        /// provided by the user.
        /// </summary>
        /// <returns>A List of any ValidationErrors that were encountered.</returns>
        private IEnumerable<ValidationError> ValidateAccountSettings()
        {
            // We just concerned with making sure the account settings provided by the user 
            // are in an acceptable format at this point, so we're just going to create an 
            // empty entity and assign the the account properties that were provided by the user.
            NeweggStoreEntity storeForValidation = new NeweggStoreEntity();
            storeForValidation.SellerID = storeSettingsControl.SellerId;
            storeForValidation.SecretKey = storeSettingsControl.SecretKey;
            storeForValidation.Channel = storeSettingsControl.Marketplace;

            return NeweggAccountSettingsValidator.Validate(storeForValidation);
        }
    }
}
