using System;
using System.ComponentModel;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.SellerVantage
{
    /// <summary>
    /// Account settings for GenericStore
    /// </summary>
    [ToolboxItem(true)]
    public partial class SellerVantageAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SellerVantageAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load store settings from the entity to the GUI
        /// </summary>
        public override void LoadStore(ShipWorks.Data.Model.EntityClasses.StoreEntity store)
        {
            GenericModuleStoreEntity genericStore = store as GenericModuleStoreEntity;
            if (genericStore == null)
            {
                throw new ArgumentException("A non GenericStore store was passed to GenericStore account settings.");
            }

            username.Text = genericStore.ModuleUsername;
            password.Text = SecureText.Decrypt(genericStore.ModulePassword, genericStore.ModuleUsername);
            clientTextBox.Text = genericStore.ModuleOnlineStoreCode;
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

            if (clientTextBox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter your SellerVantage Client Identifier.");
                return false;
            }

            genericStore.ModuleUsername = username.Text;
            genericStore.ModulePassword = SecureText.Encrypt(password.Text, username.Text);
            genericStore.ModuleOnlineStoreCode = clientTextBox.Text;

            // see if we need to test the settings because they changed in some way
            if (genericStore.Fields[(int) GenericModuleStoreFieldIndex.ModuleUsername].IsChanged ||
                genericStore.Fields[(int) GenericModuleStoreFieldIndex.ModulePassword].IsChanged ||
                genericStore.Fields[(int) GenericModuleStoreFieldIndex.ModuleUrl].IsChanged ||
                genericStore.Fields[(int) GenericModuleStoreFieldIndex.ModuleOnlineStoreCode].IsChanged)
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
                    MessageHelper.ShowError(this, ex.Message);

                    return false;
                }
            }
            else
            {
                // nothing changed
                return true;
            }
        }
    }
}
