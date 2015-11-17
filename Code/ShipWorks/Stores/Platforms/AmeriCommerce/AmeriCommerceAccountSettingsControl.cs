using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.AmeriCommerce.WebServices;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Properties;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.AmeriCommerce
{
    // delegate for asyncronously loading the stores
    delegate List<StoreTrans> LoadStoresDelegate(AmeriCommerceStoreEntity store);

    /// <summary>
    /// Control for editing AmeriCommerce account details
    /// </summary>
    public partial class AmeriCommerceAccountSettingsControl : AccountSettingsControlBase
    {
        // store being configured
        AmeriCommerceStoreEntity store;

        // whether or not to the current store refresh should be ignored
        bool ignoreRefresh = false;

        // collection of discovered stores
        List<StoreTrans> foundStores = new List<StoreTrans>();

        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the UI from the store entity 
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            Cursor.Current = Cursors.WaitCursor;

            AmeriCommerceStoreEntity ameriCommerceStore = store as AmeriCommerceStoreEntity;
            if (ameriCommerceStore == null)
            {
                throw new InvalidOperationException("A non-AmeriCommerce store was passed to the AmeriCommerceAccountSettingsControl");
            }

            this.store = ameriCommerceStore;

            urlTextBox.Text = ameriCommerceStore.StoreUrl;
            usernameTextBox.Text = ameriCommerceStore.Username;
            passwordTextBox.Text = SecureText.Decrypt(ameriCommerceStore.Password, ameriCommerceStore.Username);

            // try getting a list of the stores on the Amc account
            // Asyncronously load the store combobox since it can be a slower call
            StartAsyncStoreLoading(ameriCommerceStore);
        }

        /// <summary>
        /// Asyncronously loads the stores
        /// </summary>
        private void StartAsyncStoreLoading(AmeriCommerceStoreEntity ameriCommerceStore)
        {
            // starting new load
            ignoreRefresh = false;
            linkRefreshStores.Text = "Loading stores...";
            linkRefreshStores.Font = Font;
            statusPicture.Image = Resources.indiciator_green;
            linkRefreshStores.Enabled = false;

            LoadStoresDelegate invoker = new LoadStoresDelegate(GetStores);
            invoker.BeginInvoke(EntityUtility.CloneEntity(ameriCommerceStore), OnGetStoresComplete, new object[] { invoker, ameriCommerceStore});
        }

        /// <summary>
        /// Method to repopulate the store combobox
        /// </summary>
        private List<StoreTrans> GetStores(AmeriCommerceStoreEntity store)
        {
            try
            {
                AmeriCommerceWebClient client = new AmeriCommerceWebClient(store);
                return client.GetStores();
            }
            catch (AmeriCommerceException)
            {
                // fail silently - the account information is quite possibly incorrect
                return null;
            }
        }

        /// <summary>
        /// Asyncronous callback for store loading
        /// </summary>
        private void OnGetStoresComplete(IAsyncResult asyncResult)
        {
            if (InvokeRequired)
            {
                // If this Form went away BeginInvoke would throw
                Program.MainForm.BeginInvoke(new AsyncCallback(OnGetStoresComplete), asyncResult);
                return;
            }

            // just exit if it's already disposed by the time we got a response
            if (IsDisposed || TopLevelControl == null || !TopLevelControl.Visible)
            {
                return;
            }

            LoadStoresDelegate invoker = (LoadStoresDelegate)((object[])asyncResult.AsyncState)[0];
            AmeriCommerceStoreEntity store = (AmeriCommerceStoreEntity)((object[])asyncResult.AsyncState)[1];

            foundStores.Clear();
            foundStores.AddRange(invoker.EndInvoke(asyncResult) ?? new List<StoreTrans>());

            // clear out existing entries
            storeComboBox.Items.Clear();

            // if the user edited the url in the mean-time, disregard results
            if (ignoreRefresh)
            {
                statusPicture.Image = Resources.error16;
            }
            else
            {
                // make sure we were returned stores
                if (foundStores.Count == 0)
                {
                    statusPicture.Image = Resources.error16;
                }
                else
                {
                    // first, sort by name
                    foundStores.Sort((a, b) => a.storeName.CompareTo(b.storeName));

                    // load the UI
                    storeComboBox.Items.AddRange(foundStores.Select(s => s.storeName).ToArray());

                    // select the first store, or reselect
                    storeComboBox.SelectedIndex = Math.Max(0, foundStores.FindIndex(s => s.ID.GetValue(-1) == store.StoreCode));

                    statusPicture.Image = Resources.check16;
                }
            }

            // re-enable the control
            storeComboBox.Enabled = true;
            linkRefreshStores.Enabled = true;
            linkRefreshStores.Text = "Refresh stores";
            linkRefreshStores.Font = new Font(Font, FontStyle.Underline);
        }

        /// <summary>
        /// Save data back to the entity
        /// </summary>
        [NDependIgnoreLongMethod]
        public override bool SaveToEntity(StoreEntity store)
        {
            AmeriCommerceStoreEntity ameriCommerceStore = store as AmeriCommerceStoreEntity;
            if (ameriCommerceStore == null)
            {
                throw new ArgumentException("A non AmeriCommerce store was passed to AmeriCommerce account settings.");
            }

            // url to the module
            string url = urlTextBox.Text.Trim();
            if (url.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Enter the URL of the ShipWorks module.");
                return false;
            }

            // default to https if not specified
            if (url.IndexOf(Uri.SchemeDelimiter) == -1)
            {
                url = "https://" + url;
            }

            // check valid
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                MessageHelper.ShowError(this, "The specified URL is not a valid address.");
                return false;
            }

            if (!url.StartsWith("https"))
            {
                MessageHelper.ShowMessage(this, "The url must be secure (https).");
                return false;
            }

            ameriCommerceStore.Username = usernameTextBox.Text;
            ameriCommerceStore.Password = SecureText.Encrypt(passwordTextBox.Text, usernameTextBox.Text);
            ameriCommerceStore.StoreUrl = url;

            try
            {

                // see if we need to test the settings because they changed in some way
                if (ameriCommerceStore.Fields[(int)AmeriCommerceStoreFieldIndex.Username].IsChanged ||
                    ameriCommerceStore.Fields[(int)AmeriCommerceStoreFieldIndex.Password].IsChanged ||
                    ameriCommerceStore.Fields[(int)AmeriCommerceStoreFieldIndex.StoreUrl].IsChanged)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    // Create the client for connecting to the module
                    AmeriCommerceWebClient webClient = new AmeriCommerceWebClient(ameriCommerceStore);

                    // perform basic connectivity test
                    webClient.TestConnection();

                    // check the selected store, if there is one 
                    if (storeComboBox.SelectedIndex < 0)
                    {
                        MessageHelper.ShowError(this, "Please select the AmeriCommerce store for ShipWorks to connect to.");
                        return false;
                    }
                    else
                    {
                        ameriCommerceStore.StoreCode = foundStores[storeComboBox.SelectedIndex].ID.GetValue(-1);

                        // Execute call to AmeriCommerce to get store details
                        webClient.GetStore(ameriCommerceStore.StoreCode);

                        // connection is ok, selected store is ok
                        return true;
                    }
                }
                else
                {
                    // connection details didn't change
                    if (storeComboBox.SelectedIndex >= 0)
                    {
                        ameriCommerceStore.StoreCode = foundStores[storeComboBox.SelectedIndex].ID.GetValue(-1);
                    }

                    // if the code has changed, we need to test that the store exists and is valid
                    if (ameriCommerceStore.Fields[(int)AmeriCommerceStoreFieldIndex.StoreCode].IsChanged)
                    {
                        // execute call to americommerce to get store details
                        AmeriCommerceWebClient webClient = new AmeriCommerceWebClient(ameriCommerceStore);
                        webClient.GetStore(ameriCommerceStore.StoreCode);

                        // if the GetStore call didn't fail, it's good
                        return true;
                    }

                    return true;
                }
            }
            catch (AmeriCommerceException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                return false;
            }
        }

        /// <summary>
        /// Url has been changed, de-select the store
        /// </summary>
        private void OnUrlChanged(object sender, EventArgs e)
        {
            ignoreRefresh = true;
        }

        /// <summary>
        /// Reload the store combobox
        /// </summary>
        private void OnRefreshClick(object sender, EventArgs e)
        {
            AmeriCommerceStoreEntity copy = EntityUtility.CloneEntity(store);

            copy.StoreUrl = urlTextBox.Text;
            copy.Username = usernameTextBox.Text;
            copy.Password = SecureText.Encrypt(passwordTextBox.Text, copy.Username);

            StartAsyncStoreLoading(copy);
        }
    }
}
