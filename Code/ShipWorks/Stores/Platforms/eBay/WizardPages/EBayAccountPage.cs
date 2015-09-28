using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using System.IO;
using log4net;
using System.Xml;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using System.Net;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Stores.Platforms.Ebay.Tokens;

namespace ShipWorks.Stores.Platforms.Ebay.WizardPages
{
    /// <summary>
    /// Setup wizard page for configuring the eBay account.
    /// </summary>
    public partial class EBayAccountPage : AddStoreWizardPage
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(EBayAccountPage));

        /// <summary>
        /// Constructor
        /// </summary>
        public EBayAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the token page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            EbayStoreEntity store = GetStore<EbayStoreEntity>();

            // No token yet
            if (store.EBayToken.Length == 0)
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
            EbayStoreEntity store = GetStore<EbayStoreEntity>();

            using (TokenImportDialogController importDialog = new TokenImportDialogController())
            {
                try
                {
                    if (importDialog.Show(this) == DialogResult.OK)
                    {
                        // A token file has been selected, so we'll import the token from file 
                        // and use the imported token to configure the eBay store
                        EbayToken token = importDialog.GetToken();
                        
                        store.EBayToken = token.Token;
                        store.EBayTokenExpire = token.ExpirationDate;
                        store.EBayUserID = token.UserId;
                        
                        createTokenControl.CancelWaiting();
                        OnCreateTokenCompleted(null, EventArgs.Empty);

                        MessageHelper.ShowInformation(this, "The token file has been imported.  Click 'Next' to continue.");
                    }
                }
                catch (EbayException ex)
                {
                    MessageHelper.ShowError(this, ex.Message);
                }
            }
        }

        /// <summary>
        /// Trying to move to the next wizard page.
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            EbayStoreEntity ebayStore = (EbayStoreEntity)((AddStoreWizard)Wizard).Store;

            if (ebayStore.EBayToken.Length == 0)
            {
                // cannot continue, no token has been imported
                MessageHelper.ShowError(this, "Please create or import an eBay Login Token.");

                e.NextPage = this;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // retrieve information from ebay to populate the store description
            try
            {
                EbayWebClient webClient = new EbayWebClient(EbayToken.FromStore(ebayStore));

                UserType eBayUser = webClient.GetUser();

                ebayStore.StoreName = eBayUser.UserID;
                ebayStore.Email = eBayUser.Email;

                AddressType address = eBayUser.RegistrationAddress;
                if (address != null)
                {
                    ebayStore.Company = address.CompanyName ?? "";
                    ebayStore.Street1 = address.Street1 ?? "";
                    ebayStore.Street2 = address.Street2 ?? "";
                    ebayStore.City = address.CityName ?? "";
                    ebayStore.StateProvCode = Geography.GetStateProvCode(address.StateOrProvince) ?? "";
                    ebayStore.PostalCode = address.PostalCode ?? "";
                    ebayStore.Phone = address.Phone ?? "";
                }
            }
            catch (EbayException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                e.NextPage = this;
                return;
            }
        }
    }
}
