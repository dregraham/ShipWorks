using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Enums;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.UI.Platforms.BigCommerce
{
    /// <summary>
    /// Control for configuring non-credential settings for BigCommerce
    /// </summary>
    public partial class BigCommerceStoreSettingsControl : StoreSettingsControlBase
    {
        BigCommerceStoreEntity bigCommerceStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceStoreSettingsControl()            
        {
            InitializeComponent();

            EnumHelper.BindComboBox<WeightUnitOfMeasure>(weightUnitOfMeasure);
        }

        /// <summary>
        /// Load the UI from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            bigCommerceStore = (BigCommerceStoreEntity)store;

            weightUnitOfMeasure.SelectedValue = (WeightUnitOfMeasure) bigCommerceStore.WeightUnitOfMeasure;
        }

        /// <summary>
        /// Saves the UI to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            bigCommerceStore = (BigCommerceStoreEntity) store;

            // Weight unit of measure
            bigCommerceStore.WeightUnitOfMeasure = (int) weightUnitOfMeasure.SelectedValue;

            return true; 
        }
    }
}
