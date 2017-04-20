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
using ShipWorks.ApplicationCore.ComponentRegistration;

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
            //bigCommerceStore = (BigCommerceStoreEntity)store;

            bigCommerceStoreSettingsControl.LoadStore(store);
            downloadCriteriaControl.LoadStore(store);

            //weightUnitOfMeasure.SelectedValue = (WeightUnitOfMeasure) bigCommerceStore.WeightUnitOfMeasure;

            //downloadCriteria.LoadStore(bigCommerceStore);
        }

        /// <summary>
        /// Saves the UI to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            bigCommerceStoreSettingsControl.SaveToEntity(store);
            downloadCriteriaControl.SaveToEntity(store);

            //bigCommerceStore = (BigCommerceStoreEntity) store;

            //// Weight unit of measure
            //bigCommerceStore.WeightUnitOfMeasure = (int) weightUnitOfMeasure.SelectedValue;

            //return downloadCriteria.SaveToEntity(bigCommerceStore);
            return true;
        }
    }
}
