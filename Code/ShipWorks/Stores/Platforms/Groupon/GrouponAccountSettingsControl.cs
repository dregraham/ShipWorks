using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Account settings for Groupon Store
    /// </summary>
    public partial class GrouponAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load store settings from the entity to the GUI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            GrouponStoreEntity grouponStore = store as GrouponStoreEntity;
            if (grouponStore == null)
            {
                throw new ArgumentException("A non GenericStore store was passed to GrouponStore account settings.");
            }

            tokenTextBox.Text = grouponStore.Token;
            supplierIDTextbox.Text = grouponStore.SupplierID;
        }
        
        /// <summary>
        /// Saves the user selected settings back to the store entity;
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            GrouponStoreEntity grouponStore = store as GrouponStoreEntity;
            if (grouponStore == null)
            {
                throw new ArgumentException("A non GenericStore store was passed to GrouponStore account settings.");
            }

            grouponStore.Token = tokenTextBox.Text;
            grouponStore.SupplierID = supplierIDTextbox.Text;

            // see if we need to test the settings because they changed in some way
            if (ConnectionVerificationNeeded(grouponStore))
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    GrouponWebClient client = new GrouponWebClient(grouponStore);
                    //Check to see if we have access to Groupon with the new creds
                    //Ask for some orders
                    client.GetOrders(DateTime.UtcNow, 1);

                    return true;
                }
                catch (GrouponException ex)
                {
                    ShowConnectionException(ex);

                    return false;
                }
            }
            else
            {
                // Nothing changed
                return true;
            }
        }

        /// <summary>
        /// Hook to allow derivatives add custom error handling for connectivity testing failures.
        /// Return true to indicate the error has been handled.
        /// </summary>
        protected virtual void ShowConnectionException(GrouponException ex)
        {
            MessageHelper.ShowError(this, ex.Message);
        }

        /// <summary>
        /// For determining if the connection needs to be tested
        /// </summary>
        protected virtual bool ConnectionVerificationNeeded(GrouponStoreEntity store)
        {
            return (store.Fields[(int)GrouponStoreFieldIndex.Token].IsChanged ||
                    store.Fields[(int)GrouponStoreFieldIndex.SupplierID].IsChanged);
        }
    }
}
