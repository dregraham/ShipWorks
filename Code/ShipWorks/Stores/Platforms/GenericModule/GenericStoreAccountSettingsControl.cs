using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Account settings for GenericStore
    /// </summary>
    public partial class GenericStoreAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the control with settings from the given store type. This is to allow
        /// the control to pull any store specific settings from the store type (i.e. the URL
        /// for the help link).
        /// </summary>
        /// <param name="storeType">Type of the store.</param>
        public void Initialize(GenericModuleStoreType storeType)
        {
            helpLink.Url = storeType.AccountSettingsHelpUrl;
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
            moduleUrl.Text = genericStore.ModuleUrl;
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
            string url = moduleUrl.Text.Trim();

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
