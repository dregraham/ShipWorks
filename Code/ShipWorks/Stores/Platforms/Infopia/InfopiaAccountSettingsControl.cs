using System;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Infopia
{
    /// <summary>
    /// Control for editing Infopia Account Settings
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    public partial class InfopiaAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the settings into the UI from the provided store
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            InfopiaStoreEntity infopiaStore = store as InfopiaStoreEntity;
            if (infopiaStore == null)
            {
                throw new ArgumentException("A non Infopia store was passed to Infopia account settings.");
            }

            tokenTextBox.Text = infopiaStore.ApiToken;
        }

        /// <summary>
        /// Save UI settings to the store entity provided
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            InfopiaStoreEntity infopiaStore = store as InfopiaStoreEntity;
            if (infopiaStore == null)
            {
                throw new ArgumentException("A non Infopia store was passed to Infopia account settings.");
            }

            if (tokenTextBox.Text.Trim().Length == 0)
            {
                MessageHelper.ShowError(this, "Please enter the user token for your Infopia store.");
                return false;
            }

            // test connectivity with the provided token
            InfopiaWebClient webClient = new InfopiaWebClient(infopiaStore, tokenTextBox.Text.Trim());
            try
            {
                webClient.TestConnection();

                // connection was successful, save the token
                infopiaStore.ApiToken = tokenTextBox.Text.Trim();

                // success
                return true;
            }
            catch (InfopiaException ex)
            {
                if (ex.StatusCode == (int) InfopiaStatusType.InvalidUserToken)
                {
                    MessageHelper.ShowError(this, "The specified token is not valid.");
                }
                else
                {
                    MessageHelper.ShowError(this, ex.Message);
                }

                return false;
            }
        }
    }
}
