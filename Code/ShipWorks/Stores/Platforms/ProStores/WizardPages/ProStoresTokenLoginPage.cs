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
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.ProStores.WizardPages
{
    /// <summary>
    /// Wizard page for importing a ProStores token
    /// </summary>
    public partial class ProStoresTokenLoginPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresTokenLoginPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the token page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            ProStoresStoreEntity store = GetStore<ProStoresStoreEntity>();

            if (store.LoginMethod == (int) ProStoresLoginMethod.LegacyUserPass)
            {
                e.Skip = true;
                return;
            }

            // No token yet
            if (store.ApiToken.Length == 0)
            {
                createTokenControl.InitializeForStore(store);
            }
            // We have a token
            else
            {
                panelCreate.Visible = false;

                manageTokenControl.Top = panelCreate.Top;
                manageTokenControl.Visible = true;

                manageTokenControl.InitializeForStore(store);
            }
        }

        /// <summary>
        /// The user has completed the create token process
        /// </summary>
        private void OnCreateTokenCompleted(object sender, EventArgs e)
        {
            createTokenControl.Enabled = false;
            linkImportTokenFile.Enabled = false;
        }

        /// <summary>
        /// Load a previously saved token from a file
        /// </summary>
        private void OnImportTokenFile(object sender, EventArgs e)
        {
            if (ProStoresTokenManageControl.ImportTokenFile(GetStore<ProStoresStoreEntity>(), this))
            {
                createTokenControl.CancelWaiting();

                OnCreateTokenCompleted(null, EventArgs.Empty);

                MessageHelper.ShowInformation(this, "The token file has been imported.  Click 'Next' to continue.");
            }
        }

        /// <summary>
        /// Trying to move to the next wizard page.
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            ProStoresStoreEntity store = GetStore<ProStoresStoreEntity>();

            if (store.ApiToken.Length == 0)
            {
                // cannot continue, no token has been imported
                MessageHelper.ShowError(this, "Please create or import a ProStores Login Token.");

                e.NextPage = this;
                return;
            }

            store.StoreName = "ProStores: " + store.ShortName;
        }
    }
}
