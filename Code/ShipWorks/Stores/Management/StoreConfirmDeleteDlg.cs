using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Window to make a user confirm that they understand the severity of deleting a store
    /// </summary>
    public partial class StoreConfirmDeleteDlg : Form
    {
        private readonly StoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreConfirmDeleteDlg(StoreEntity store)
        {
            this.store = store;
            InitializeComponent();

            labelStore.Text = string.Format(labelStore.Text, store.StoreName);
        }

        /// <summary>
        /// Confirm the delete
        /// </summary>
        private void OnConfirm(object sender, EventArgs e)
        {
            delete.Enabled = checkBoxConfirm.Checked;
        }

        /// <summary>
        /// Ensure that we can lock the download table prior to letting the user delete
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            try
            {
                // We open a lock that will stay open for the duration of the store delete,
                // which will serve to lock out any other running instance of ShipWorks from downloading
                // for this store.
                using (new SqlEntityLock(store.StoreID, "Deleting"))
                {
                }
                DialogResult = DialogResult.OK;
            }
            catch (SqlAppResourceLockException)
            {
                MessageHelper.ShowError(this,
                    $"Another machine is currently downloading orders for {store.StoreName}. Please wait for the download to complete before trying to delete the store.");
            }
        }
    }
}
