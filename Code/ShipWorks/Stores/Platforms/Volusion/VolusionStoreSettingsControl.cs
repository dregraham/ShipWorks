using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// Control for configuring non-credential settings for Volusion
    /// </summary>
    public partial class VolusionStoreSettingsControl : StoreSettingsControlBase
    {
        static string statusString = "{0} {1} are loaded.";

        VolusionStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionStoreSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the UI from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            VolusionStoreEntity volusionStore = store as VolusionStoreEntity;

            this.store = volusionStore;

            // timezone
            timeZoneControl.SelectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(volusionStore.ServerTimeZone);

            UpdateStatusLabels();
            
        }

        /// <summary>
        /// Updates the status string labels with information about how many Shipping/Payment methods are loaded
        /// </summary>
        private void UpdateStatusLabels()
        {
            // shipping methods
            VolusionShippingMethods shipMethods = new VolusionShippingMethods(store);
            shippingMethodsStatus.Text = String.Format(statusString, shipMethods.Count, "shipping methods");

            // payment methods
            VolusionPaymentMethods paymentMethods = new VolusionPaymentMethods(store);
            paymentMethodsStatus.Text = String.Format(statusString, paymentMethods.Count, "payment methods");

            //VolusionStoreType.GetOnlineStatusChoices();

            StoreType storeType = StoreTypeManager.GetType(store);

            // order statuses
            //statuses.Items.AddRange();

            foreach(string status in storeType.GetOnlineStatusChoices())
            {
                statuses.Items.Add(status, false);
            }

        }

        /// <summary>
        /// Saves the UI to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            VolusionStoreEntity volusionStore = store as VolusionStoreEntity;

            volusionStore.ServerTimeZone = timeZoneControl.SelectedTimeZone.Id;
            
            volusionStore.ShipmentMethods = this.store.ShipmentMethods;
            volusionStore.PaymentMethods = this.store.PaymentMethods;

            return true;
        }

        /// <summary>
        /// Updates/refreshes shipping methods
        /// </summary>
        private void OnUpdateShippingMethodsClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            bool manualImport = true;
            bool success = false;

            string webPassword = SecureText.Decrypt(store.WebPassword, store.WebUserName);

            // only try to auto-update if they have a web password
            if (webPassword.Length > 0)
            {
                try
                {
                    GetShippingMethods(store.StoreUrl, store.WebUserName, webPassword);
                    manualImport = false;

                    success = true;
                }
                catch (VolusionException ex)
                {
                    // show message
                    MessageHelper.ShowError(this, string.Format("An error occurred while trying to automatically refresh shipping methods:\n\n{0}\n\nYou may proceed by manually importing.", ex.Message));
                }
            }

            // fallback
            if (manualImport)
            {
                // bring up import dialog
                using (VolusionCodeRefreshDialog dlg = new VolusionCodeRefreshDialog(store, VolusionCodeImportMode.ShippingMethods))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        success = true;
                    }
                }
            }

            // explain if it worked
            if (success)
            {
                UpdateStatusLabels();
                MessageHelper.ShowInformation(this, "The shipping methods were successfully updated.");
            }
        }
        
        /// <summary>
        /// Updates/refreshes payment methods
        /// </summary>
        private void OnUpdatePaymentMethodsClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            bool manualImport = true;
            bool success = false;

            string webPassword = SecureText.Decrypt(store.WebPassword, store.WebUserName);

            // only try to auto-update if they have a web password
            if (webPassword.Length > 0)
            {
                try
                {
                    GetPaymentMethods(store.StoreUrl, store.WebUserName, webPassword);
                    manualImport = false;

                    success = true;
                }
                catch (VolusionException ex)
                {
                    // show message
                    MessageHelper.ShowError(this, string.Format("An error occurred while trying to automatically refresh payment methods:\n\n{0}\n\nYou may proceed by manually importing.", ex.Message));
                }
            }

            // fallback
            if (manualImport)
            {
                // bring up import dialog
                using (VolusionCodeRefreshDialog dlg = new VolusionCodeRefreshDialog(store, VolusionCodeImportMode.PaymentMethods))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        success = true;
                    }
                }
            }

            // explain if it worked
            if (success)
            {
                UpdateStatusLabels();
                MessageHelper.ShowInformation(this, "The payment methods were successfully updated.");
            }
        }

        /// <summary>
        /// Tries to load Shipping Methods
        /// </summary>
        private void GetShippingMethods(string url, string userName, string password)
        {
            Cursor.Current = Cursors.WaitCursor;

            VolusionWebSession session = new VolusionWebSession(url);

            if (session.LogOn(userName, password))
            {
                VolusionShippingMethods methods = new VolusionShippingMethods(store);
                methods.ImportCsv(session.RetrieveShippingMethods());
            }
            else
            {
                throw new VolusionException("ShipWorks was unable to login to your Volusion store.  Please check the Web Password in the Account Settings section.");
            }
        }

        /// <summary>
        /// Tries to load Payment Methods
        /// </summary>
        private void GetPaymentMethods(string url, string userName, string password)
        {
            Cursor.Current = Cursors.WaitCursor;

            VolusionWebSession session = new VolusionWebSession(url);

            if (session.LogOn(userName, password))
            {
                VolusionPaymentMethods methods = new VolusionPaymentMethods(store);
                methods.ImportCsv(session.RetrievePaymentMethods());
            }
            else
            {
                throw new VolusionException("ShipWorks was unable to login to your Volusion store.  Please check the Web Password in the Account Settings section.");
            }
        }
    }
}
