using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using System.Xml;

namespace ShipWorks.Stores.Platforms.ProStores
{
    /// <summary>
    /// Wizard for generating a new ProStores token.  This wizard does not Save the entity result to the database.
    /// </summary>
    public partial class ProStoresTokenWizard : WizardForm
    {
        ProStoresStoreEntity store;

        bool tokenCreated = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresTokenWizard(ProStoresStoreEntity store)
        {
            InitializeComponent();

            this.store = store;

            if (store.LoginMethod == (int) ProStoresLoginMethod.ApiToken)
            {
                Pages.Remove(wizardPageWelcome);
            }

            createTokenControl.InitializeForStore(store);
        }

        /// <summary>
        /// Stepping next on the Api Entry Point page
        /// </summary>
        private void OnStepNextApiEntryPoint(object sender, WizardStepEventArgs e)
        {
            string url = apiEntryPoint.Text.Trim();

            if (url.Length == 0)
            {
                MessageHelper.ShowInformation(this, "Enter the API Entry Point of your ProStores store.");
                e.NextPage = CurrentPage;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                XmlDocument apiInfoResponse = ProStoresWebClient.GetStoreApiInfo(url);

                ((ProStoresStoreType) StoreTypeManager.GetType(store)).LoadApiInfo(url, apiInfoResponse);
            }
            catch (ProStoresException ex)
            {
                MessageHelper.ShowError(this, "An error occurred while accessing the API Entry Point:\n\n" + ex.Message);
                e.NextPage = CurrentPage;

                return;
            }
        }

        /// <summary>
        /// The token has been created
        /// </summary>
        private void OnTokenCreated(object sender, EventArgs e)
        {
            tokenCreated = true;

            BackEnabled = false;
            createTokenControl.Enabled = false;
        }

        /// <summary>
        /// Stepping next from the Create token page
        /// </summary>
        private void OnStepNextCreateToken(object sender, WizardStepEventArgs e)
        {
            if (!tokenCreated)
            {
                MessageHelper.ShowInformation(this, "You must create a ProStores login token before continuing.");
                e.NextPage = CurrentPage;
            }
        }
    }
}
