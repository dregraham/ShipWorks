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

namespace ShipWorks.Stores.Platforms.Yahoo
{
    /// <summary>
    /// Settings control for Yahoo store settings
    /// </summary>
    public partial class YahooStoreSettingsControl : StoreSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public YahooStoreSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the store into the control
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            importProductsControl.InitializeForStore(store.StoreID);
        }
    }
}
