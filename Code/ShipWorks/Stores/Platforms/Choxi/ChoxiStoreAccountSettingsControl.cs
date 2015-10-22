using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.ApplicationCore;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.UI;
using ShipWorks.Data.Model;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Choxi
{
    /// <summary>
    /// Account settings for GenericStore
    /// </summary>
    public partial class ChoxiStoreAccountSettingsControl : AccountSettingsControlBase
    {
        private const string TestUrl = "http://staging.vendor-api.choxi.com/api/v1/shipworks/";
        private const string LiveUrl = "https://vendor-api.choxi.com/api/v1/shipworks/";

        /// <summary>
        /// Constructor
        /// </summary>
        public ChoxiStoreAccountSettingsControl()
        {
            InitializeComponent();

            useTestServer.Visible = InterapptiveOnly.MagicKeysDown;

            ChoxiStoreType store = (ChoxiStoreType) StoreTypeManager.GetType(StoreTypeCode.Choxi);
            helpLink.Url = store.AccountSettingsHelpUrl;

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
            vendorId.Text = genericStore.ModuleUrl.Substring(genericStore.ModuleUrl.LastIndexOf("/") + 1);

            useTestServer.Visible = useTestServer.Checked || InterapptiveOnly.MagicKeysDown;
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
            genericStore.ModuleUrl = useTestServer.Checked ? TestUrl + vendorId.Text : LiveUrl + vendorId.Text;

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
