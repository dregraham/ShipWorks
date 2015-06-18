using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// Control for obtaining the Newegg store credentials (seller ID and secret key).
    /// </summary>
    public partial class NeweggStoreMarketplaceControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggStoreCredentialsControl"/> class.
        /// </summary>
        public NeweggStoreMarketplaceControl()            
        {
            InitializeComponent();

            EnumHelper.BindComboBox<NeweggChannelType>(marketplace);
        }

        /// <summary>
        /// Save user-entered values back to the entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            NeweggStoreEntity saveStore = store as NeweggStoreEntity;
            if (saveStore == null)
            {
                throw new ArgumentException("NewEggStoreEntity expected.", "store");
            }

            saveStore.Channel = (int)marketplace.SelectedValue;

            return true;
        }
    }
}
