using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// Settings control for Newegg accounts
    /// </summary>
    public partial class NeweggAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggAccountSettingsControl"/> class.
        /// </summary>
        public NeweggAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Should the save operation use the async version
        /// </summary>
        public override bool IsSaveAsync => true;

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        /// <param name="store"></param>
        public override void LoadStore(StoreEntity store)
        {
            NeweggStoreEntity neweggStore = GetNeweggStore(store);

            storeCredentialsControl.SellerId = neweggStore.SellerID;
            storeCredentialsControl.SecretKey = neweggStore.SecretKey;
            storeCredentialsControl.Marketplace = neweggStore.Channel;
        }

        /// <summary>
        /// Save the data into the StoreEntity.  Nothing is saved to the database.
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public override async Task<bool> SaveToEntityAsync(StoreEntity store)
        {
            IEnumerable<ValidationError> errors = ValidateAccountSettings();
            if (errors.Count() > 0)
            {
                // The account settings provided did not pass validation, so just grab the first error message
                // and show it to the user
                MessageHelper.ShowError(this, errors.First().ErrorMessage);
                return false;
            }

            // Our settings have passed the initial validation, so now we can assign the store entity with the values
            // provided in the control
            NeweggStoreEntity neweggStore = GetNeweggStore(store);

            neweggStore.SellerID = storeCredentialsControl.SellerId;
            neweggStore.SecretKey = storeCredentialsControl.SecretKey;
            neweggStore.Channel = storeCredentialsControl.Marketplace;

            if (neweggStore.IsDirty)
            {
                try
                {
                    using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                    {
                        // The store settings have been changed, so let's bounce them off the
                        // Newegg API to confirm that they are correct
                        INeweggWebClient webClient = lifetimeScope.Resolve<INeweggWebClient>();
                        var areCredentialsValid = await webClient.AreCredentialsValid(neweggStore).ConfigureAwait(true);
                        if (!areCredentialsValid)
                        {
                            MessageHelper.ShowError(this, "ShipWorks could not communicate with Newegg using the connection settings provided.");
                            return false;
                        }
                    }
                }
                catch (NeweggException ex)
                {
                    MessageHelper.ShowError(this, ex.Message);
                    return false;
                }
            }

            return true;
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
            storeForValidation.SellerID = storeCredentialsControl.SellerId;
            storeForValidation.SecretKey = storeCredentialsControl.SecretKey;
            storeForValidation.Channel = storeCredentialsControl.Marketplace;

            return NeweggAccountSettingsValidator.Validate(storeForValidation);
        }

        /// <summary>
        /// Helper method that does type checking and casts the generic StoreEntity
        /// to a NeweggStoreEntity.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns>A NeweggStoreEntity.</returns>
        private static NeweggStoreEntity GetNeweggStore(StoreEntity store)
        {
            NeweggStoreEntity neweggStore = store as NeweggStoreEntity;
            if (neweggStore == null)
            {
                throw new InvalidOperationException("A non Newegg Store was passed to the Newegg Account Settings control.");
            }

            return neweggStore;
        }
    }
}
