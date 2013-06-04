using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using ShipWorks.Data;
using ShipWorks.Data.Connection;

namespace ShipWorks.Stores.Platforms.Etsy.WizardPages
{
    /// <summary>
    /// Page to associate Etsy Account token with ShipWorks Account
    /// </summary>
    public partial class EtsyAssociateAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyAssociateAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On Stepping in
        /// </summary>
        private void OnSteppingIntoAccountPage(object sender, WizardSteppingIntoEventArgs e)
        {
            EtsyStoreEntity store = GetStore<EtsyStoreEntity>();

            etsyManageToken.LoadStore(store);
        }

        /// <summary>
        /// Handles Import Token Click Event. Imports Token...
        /// </summary>
        private void OnImportTokenFileClick(object sender, EventArgs e)
        {
            etsyManageToken.ImportToken();
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNextEtsyAssociateAccountPage(object sender, WizardStepEventArgs e)
        {
            EtsyStoreEntity store = GetStore<EtsyStoreEntity>();

            if (!etsyManageToken.IsTokenValid)
            {
                e.NextPage = this;

                MessageHelper.ShowError(this, "Please create or import an Etsy login token.");
            }
            else
            {
                if (string.IsNullOrEmpty(store.StoreName))
                {
                    EtsyWebClient webClient = new EtsyWebClient(store);

                    webClient.RetrieveShopInformation();
                }
            }
        }
    }
}