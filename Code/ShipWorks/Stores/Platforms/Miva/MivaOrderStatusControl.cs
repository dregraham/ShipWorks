using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.Miva
{
    /// <summary>
    /// Control for configuring how order statuses are handled
    /// </summary>
    public partial class MivaOrderStatusControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MivaOrderStatusControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the settings from the provided store
        /// </summary>
        public void LoadStore(MivaStoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            // Remove the selected value changed event temporarily so that we can update the drop down and selected value
            // without overwriting the sebenzaOrderStatusEmail.Checked value
            updateStrategy.SelectedValueChanged -= OnUpdateStrategyChanged;

            List<KeyValuePair<string, MivaOnlineUpdateStrategy>> options = new List<KeyValuePair<string, MivaOnlineUpdateStrategy>>();
            options.Add(CreateEntry(MivaOnlineUpdateStrategy.None));

            if (store.ModulePlatform == "Miva Merchant 5")
            {
                options.Add(CreateEntry(MivaOnlineUpdateStrategy.MivaNative));
                options.Add(CreateEntry(MivaOnlineUpdateStrategy.Sebenza));

                sebenzaOrderStatusEmail.Checked = store.OnlineUpdateStatusChangeEmail;
            }

            updateStrategy.DisplayMember = "Key";
            updateStrategy.ValueMember = "Value";
            updateStrategy.DataSource = options;

            updateStrategy.SelectedValue = (MivaOnlineUpdateStrategy)store.OnlineUpdateStrategy;

            // Run the logic that updates the visibility of the sebenzaOrderStatusEmail checkbox.
            // If the store value is the same as the default value, it won't actually get called via += change event.
            OnUpdateStrategyChanged(updateStrategy, null);

            // Re-add the selected change event handler.
            updateStrategy.SelectedValueChanged += OnUpdateStrategyChanged;
        }

        /// <summary>
        /// Creates a combobox binding item
        /// </summary>
        private static KeyValuePair<string, MivaOnlineUpdateStrategy> CreateEntry(MivaOnlineUpdateStrategy mivaOnlineUpdateStrategy)
        {
            string name = "None/Unavailable";
            switch (mivaOnlineUpdateStrategy)
            {
                case MivaOnlineUpdateStrategy.MivaNative:
                    name = "Miva Merchant 5 PR 7 (built-in)";
                    break;
                case MivaOnlineUpdateStrategy.Sebenza:
                    name = "Sebenza Ultimate Order Status module";
                    break;
            }

            return new KeyValuePair<string, MivaOnlineUpdateStrategy>(name, mivaOnlineUpdateStrategy);
        }

        /// <summary>
        /// Saves selected values to the store
        /// </summary>
        public bool SaveToEntity(MivaStoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            store.OnlineUpdateStrategy = (int)updateStrategy.SelectedValue;
            store.OnlineUpdateStatusChangeEmail = sebenzaOrderStatusEmail.Checked;

            try
            {
                // We have to update the core fields that represent our online update capabilities
                switch ((MivaOnlineUpdateStrategy)store.OnlineUpdateStrategy)
                {
                    case MivaOnlineUpdateStrategy.None:
                        SaveNoneToEntity(store);
                        break;
                    case MivaOnlineUpdateStrategy.Sebenza:
                        SaveDetailsToEntity(store, GenericOnlineStatusSupport.StatusWithComment);
                        break;
                    case MivaOnlineUpdateStrategy.MivaNative:
                        SaveDetailsToEntity(store, GenericOnlineStatusSupport.DownloadOnly);
                        break;
                    default:
                        throw new InvalidOperationException("Unhandled MivaOnlineUpdateStrategy: " + store.OnlineUpdateStrategy);
                }
            }
            catch (GenericStoreException ex)
            {
                MessageHelper.ShowError(this, "ShipWorks could not retrieve the list of online status options from Miva Merchant.\n\nDetails: " + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Save Miva details to entity
        /// </summary>
        private void SaveDetailsToEntity(MivaStoreEntity store, GenericOnlineStatusSupport status)
        {
            store.ModuleOnlineStatusSupport = (int) status;
            store.ModuleOnlineStatusDataType = (int)GenericVariantDataType.Text;
            store.ModuleOnlineShipmentDetails = true;

            if (store.Fields[(int)MivaStoreFieldIndex.ModuleOnlineStatusSupport].IsChanged)
            {
                GenericStoreStatusCodeProvider statusCodeProvider = ((MivaStoreType)StoreTypeManager.GetType(store)).CreateStatusCodeProvider();
                statusCodeProvider.UpdateFromOnlineStore();
            }
        }

        /// <summary>
        /// Save a none order
        /// </summary>
        private void SaveNoneToEntity(MivaStoreEntity store)
        {
            store.ModuleOnlineStatusSupport = (int)GenericOnlineStatusSupport.None;
            store.ModuleOnlineShipmentDetails = false;
            store.ModuleStatusCodes = "<Root />";
        }

        /// <summary>
        /// Enable/disable UI based on selected value
        /// </summary>
        private void OnUpdateStrategyChanged(object sender, EventArgs e)
        {
            sebenzaOrderStatusEmail.Enabled = updateStrategy.SelectedValue != null &&
                ((MivaOnlineUpdateStrategy)updateStrategy.SelectedValue == MivaOnlineUpdateStrategy.Sebenza);

            if (!sebenzaOrderStatusEmail.Enabled)
            {
                sebenzaOrderStatusEmail.Checked = false;
            }
        }
    }
}
