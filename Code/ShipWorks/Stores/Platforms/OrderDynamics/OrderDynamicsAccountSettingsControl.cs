using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.UI;
using ShipWorks.Data.Model;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.OrderDynamics
{
    /// <summary>
    /// Account settings for GenericStore
    /// </summary>
    [ToolboxItem(true)]
    public partial class OrderDynamicsAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDynamicsAccountSettingsControl()
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

            urlTextBox.Text = genericStore.ModuleUrl;
            licenseTextBox.Text = genericStore.ModuleOnlineStoreCode;
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

            if (urlTextBox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter the Url to the OrderDyanmics integration.");
                return false;
            }

            if (licenseTextBox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter OrderDynamics LicenseID.");
                return false;
            }

            genericStore.ModuleUrl = urlTextBox.Text;
            genericStore.ModuleOnlineStoreCode = licenseTextBox.Text;

            // see if we need to test the settings because they changed in some way
            if (genericStore.Fields[(int) GenericModuleStoreFieldIndex.ModuleUrl].IsChanged ||
                genericStore.Fields[(int) GenericModuleStoreFieldIndex.ModuleOnlineStoreCode].IsChanged)
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    // Validate the license ID
                    try
                    {
                        if (!OrderDynamicsLicenseHelper.IsValidLicense(licenseTextBox.Text))
                        {
                            MessageHelper.ShowError(this, "Unable to verify the License ID with OrderDynamics.");
                            return false;
                        }
                    }
                    catch (FormatException ex)
                    {
                        MessageHelper.ShowError(this, ex.Message);
                        return false;
                    }

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
                // Nothing changed
                return true;
            }
        }
    }
}
