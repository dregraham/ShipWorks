using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// UserControl for MarketplaceAdvisor store settings
    /// </summary>
    public partial class MarketplaceAdvisorStoreSettingsControl : StoreSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorStoreSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load data from the store into the UI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            flagsControl.LoadFlags((MarketplaceAdvisorStoreEntity) store);
        }

        /// <summary>
        /// Save data from the UI into the store
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            ((MarketplaceAdvisorStoreEntity) store).DownloadFlags = (int) flagsControl.ReadFlags();

            return true;
        }
    }
}
