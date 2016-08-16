using System;
using System.Windows.Forms;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Brightpearl
{
    /// <summary>
    /// Account settings for GenericStore
    /// </summary>
    public partial class BrightpearlStoreAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BrightpearlStoreAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load store settings from the entity to the GUI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            LoadControl();
            GenericModuleStoreEntity genericStore = store as GenericModuleStoreEntity;
            if (genericStore == null)
            {
                throw new ArgumentException("A non GenericStore store was passed to GenericStore account settings.");
            }

            username.Text = genericStore.ModuleUsername;
            password.Text = SecureText.Decrypt(genericStore.ModulePassword, genericStore.ModuleUsername);

            timeZone.SelectedValue = BrightpearlUtility.GetTimeZone(genericStore.ModuleUrl);
            accountID.Text = BrightpearlUtility.GetAccountId(genericStore.ModuleUrl);
        }

        /// <summary>
        /// Loads the control.
        /// </summary>
        public void LoadControl()
        {
            EnumHelper.BindComboBox<BrightpearlServerTimeZoneType>(timeZone);            
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

            // url to the module
            string url = BrightpearlUtility.GetModuleUrl(accountID.Text, (BrightpearlServerTimeZoneType)timeZone.SelectedValue);

            // check valid
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                MessageHelper.ShowError(this, "The specified URL is not a valid address.");
                return false;
            }

            genericStore.ModuleUsername = username.Text;
            genericStore.ModulePassword = SecureText.Encrypt(password.Text, username.Text);
            genericStore.ModuleUrl = url;

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
                    genericStore.Fields[(int)GenericModuleStoreFieldIndex.ModuleUrl].IsChanged ||
                    genericStore.Fields[(int)GenericModuleStoreFieldIndex.ModuleOnlineStoreCode].IsChanged);
        }
    }
}
