using System;
using System.Windows.Forms;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.SellerExpress
{
    /// <summary>
    /// Account settings for GenericStore
    /// </summary>
    public partial class SellerExpressAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SellerExpressAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load store settings from the entity to the GUI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            GenericModuleStoreEntity genericStore = store as GenericModuleStoreEntity;
            if (genericStore == null)
            {
                throw new ArgumentException("A non GenericStore store was passed to GenericStore account settings.");
            }

            username.Text = genericStore.ModuleUsername;
            password.Text = SecureText.Decrypt(genericStore.ModulePassword, genericStore.ModuleUsername);
        }

        /// <summary>
        /// Saves the user selected settings back to the store entity;
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            GenericModuleStoreEntity genericStore = store as GenericModuleStoreEntity;
            if (genericStore == null)
            {
                throw new ArgumentException("A non GenericStore store was passed to GenericStore account settings.");
            }

            genericStore.ModuleUsername = username.Text;
            genericStore.ModulePassword = SecureText.Encrypt(password.Text, username.Text);
            genericStore.ModuleUrl = "https://login.sellerexpress.com/shipworks/xml_api.aspx";

            // see if we need to test the settings because they changed in some way
            if (ConnectionVerificationNeeded(genericStore))
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    GenericModuleStoreType storeType = (GenericModuleStoreType)StoreTypeManager.GetType(store);
                    storeType.UpdateOnlineModuleInfo();

                    return true;
                }
                catch (GenericStoreException ex)
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
        protected virtual void ShowConnectionException(GenericStoreException ex)
        {
            MessageHelper.ShowError(this, ex.Message);
        }

        /// <summary>
        /// For determining if the connection needs to be tested
        /// </summary>
        protected virtual bool ConnectionVerificationNeeded(GenericModuleStoreEntity genericStore)
        {
            return (genericStore.Fields[(int)GenericModuleStoreFieldIndex.ModuleUsername].IsChanged ||
                    genericStore.Fields[(int)GenericModuleStoreFieldIndex.ModulePassword].IsChanged ||
                    genericStore.Fields[(int)GenericModuleStoreFieldIndex.ModuleOnlineStoreCode].IsChanged);
        }
    }
}
