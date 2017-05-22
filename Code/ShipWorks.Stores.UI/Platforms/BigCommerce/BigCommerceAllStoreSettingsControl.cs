using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Enums;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.UI.Platforms.BigCommerce
{
    /// <summary>
    /// Control for configuring non-credential settings for BigCommerce
    /// </summary>
    [KeyedComponent(typeof(StoreSettingsControlBase), StoreTypeCode.BigCommerce, ExternallyOwned = true)]
    public partial class BigCommerceAllStoreSettingsControl : StoreSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceAllStoreSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the UI from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            bigCommerceStoreSettingsControl.LoadStore(store);
            downloadCriteriaControl.LoadStore(store);
        }

        /// <summary>
        /// Saves the UI to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            bigCommerceStoreSettingsControl.SaveToEntity(store);
            downloadCriteriaControl.SaveToEntity(store);

            return true;
        }
    }
}
