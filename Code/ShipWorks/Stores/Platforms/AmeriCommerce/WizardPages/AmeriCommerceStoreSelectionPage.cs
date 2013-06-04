using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.AmeriCommerce.WebServices;
using Interapptive.Shared.Business;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.AmeriCommerce.WizardPages
{
    /// <summary>
    /// Page to allow the user to select which store they are going to communicate with
    /// </summary>
    public partial class AmeriCommerceStoreSelectionPage : AddStoreWizardPage
    {
        // discovered stores tied to the AmeriCommerce account
        List<StoreTrans> foundStores;

        /// <summary>
        /// Shortcut to get to the AmeriCommerce Store Entity
        /// </summary>
        private AmeriCommerceStoreEntity Store
        {
            get
            {
                return GetStore<AmeriCommerceStoreEntity>();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceStoreSelectionPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Moving into the page from the prior
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            string errorMessage = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // try getting a list of the stores on the Amc account
                AmeriCommerceWebClient client = new AmeriCommerceWebClient(Store);
                foundStores = client.GetStores();
            }
            catch(AmeriCommerceException ex)
            {
                errorMessage = "ShipWorks was unable to find any stores associated with this account: " + ex.Message;
            }

            if (foundStores != null)
            {
                if (foundStores.Count == 0)
                {
                    // no stores found, there's a problem
                    errorMessage = "ShipWorks was unable to find any AmeriCommerce stores associated with this account.";
                }
                else
                {
                    List<string> names = foundStores.Select(s => string.IsNullOrWhiteSpace(s.storeName) ? s.domainName : s.storeName).ToList();
                    names.Sort();

                    // load the UI
                    storeComboBox.Items.Clear();
                    storeComboBox.Items.AddRange(names.ToArray());

                    // select the first store, or reselect
                    storeComboBox.SelectedIndex = Math.Max(0, foundStores.FindIndex(s => s.ID.GetValue(-1) == Store.StoreCode));

                    // If there's just 1, move next on the user's behalf
                    if (foundStores.Count == 1)
                    {
                        e.Skip = true;
                        e.RaiseStepEventWhenSkipping = true;
                    }
                }
            }

            // show any errors that happened
            if (errorMessage != null)
            {
                // if an error is to be raised
                MessageHelper.ShowError(this, errorMessage);

                e.Skip = true;
                e.SkipToPage = Wizard.FindPage(typeof(AmeriCommerceAccountPage));
            }
        }

        /// <summary>
        /// Loads the Store information for the selected Store Code.  Leaving exceptions unhandled so the caller
        /// can be aware.
        /// </summary>
        private void LoadStoreDetails()
        {
            AmeriCommerceWebClient webClient = new AmeriCommerceWebClient(Store);

            StoreTrans storeTrans = webClient.GetStore(Store.StoreCode);
            Store.StoreName = storeTrans.storeName;
            Store.Street1 = storeTrans.storeAddressLine1;
            Store.Street2 = storeTrans.storeAddressLine2;
            Store.City = storeTrans.storeCity;
            Store.PostalCode = storeTrans.storeZipCode;
            Store.Phone = storeTrans.storePhone;
            Store.Email = storeTrans.email;
            Store.Website = storeTrans.domainName;

            try
            {
                Store.StateProvCode = webClient.GetStateCode(storeTrans.storeStateID.GetValue(0));
            }
            catch (AmeriCommerceException)
            {
                Store.StateProvCode = "";
            }
        }

        /// <summary>
        /// Moving to the next page in the wizard
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            int selectedIndex = storeComboBox.SelectedIndex;
            if (selectedIndex >= foundStores.Count)
            {
                throw new InvalidOperationException();
            }

            Store.StoreCode = foundStores[selectedIndex].ID.GetValue(-1);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                LoadStoreDetails();

                AmeriCommerceStatusCodeProvider statusProvider = new AmeriCommerceStatusCodeProvider((AmeriCommerceStoreEntity) Store);
                statusProvider.UpdateFromOnlineStore();
            }
            catch (AmeriCommerceException ex)
            {
                MessageHelper.ShowError(this, String.Format("There was an error retrieving store details: {0}", ex.Message));

                // stay on this page
                e.NextPage = this;
            }
        }
    }
}
