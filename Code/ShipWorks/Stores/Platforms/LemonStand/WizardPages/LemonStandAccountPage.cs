using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.LemonStand.WizardPages
{
    public partial class LemonStandAccountPage : AddStoreWizardPage
    {
        public LemonStandAccountPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// User is moving to the next wizard page, perform any autoconfiguration or credentials saving
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            LemonStandStoreEntity store = GetStore<LemonStandStoreEntity>();

            
            store.StoreURL = storeURLTextbox.Text;
            store.APIKey = apiKeyTextbox.Text;
            store.Token = accessTokenTextbox.Text;

            if (storeURLTextbox.Text.Length == 0) 
            {
                MessageHelper.ShowError(this, "Please enter your Store URL");
                e.NextPage = this;
                return;
            }

            if (apiKeyTextbox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "Please enter your API Key");
                e.NextPage = this;
                return;
            }

            if (accessTokenTextbox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "Please enter your Access Token");
                e.NextPage = this;
                return;
            }

            try
            {
                LemonStandWebClient client = new LemonStandWebClient(store);
                //Check to see if we have access to Groupon with the new creds
                //Ask for some orders
                client.GetOrders(DateTime.UtcNow, 1);
            }
            catch (LemonStandException ex)
            {
                ShowConnectionException(ex);
                e.NextPage = this;
                return;
            }

            //GrouponTemplate.InstallGrouponTemplate();
        }

        /// <summary>
        /// Hook to allow derivatives add custom error handling for connectivity testing failures.
        /// Return true to indicate the error has been handled.
        /// </summary>
        protected virtual void ShowConnectionException(LemonStandException ex)
        {
            MessageHelper.ShowError(this, ex.Message);
        }
    }
}
