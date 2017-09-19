using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Newegg.WizardPages
{
    /// <summary>
    /// Settings page for Newegg accounts
    /// </summary>
    public partial class NeweggAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NeweggAccountPage()
        {
            InitializeComponent();
            StepNextAsync = OnStepNext;
        }

        /// <summary>
        /// On stepping next in the wizard
        /// </summary>
        private async Task OnStepNext(object sender, WizardStepEventArgs e)
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
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    var webClient = lifetimeScope.Resolve<INeweggWebClient>();
                    var areCredentailsValid = await webClient.AreCredentialsValid(store).ConfigureAwait(true);
                    if (!areCredentailsValid)
                    {
                        MessageHelper.ShowError(this, "ShipWorks could not communicate with Newegg using the connection settings provided.");
                        e.NextPage = this;
                        return;
                    }
                }
            }
            catch (NeweggException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                e.NextPage = this;
            }
        }

        /// <summary>
        /// A helper method that validates the values of the various account controls
        /// provided by the user.
        /// </summary>
        /// <returns>A List of any ValidationErrors that were encountered.</returns>
        private IEnumerable<ValidationError> ValidateAccountSettings()
        {
            // We just concerned with making sure the account settings provided by the user
            // are in an acceptable format at this point, so we're just going to create an
            // empty entity and assign the account properties that were provided by the user.
            NeweggStoreEntity storeForValidation = new NeweggStoreEntity();
            storeForValidation.SellerID = storeSettingsControl.SellerId;
            storeForValidation.SecretKey = storeSettingsControl.SecretKey;
            storeForValidation.Channel = storeSettingsControl.Marketplace;

            return NeweggAccountSettingsValidator.Validate(storeForValidation);
        }
    }
}
