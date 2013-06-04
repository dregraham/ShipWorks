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

namespace ShipWorks.Stores.Platforms.ProStores
{
    /// <summary>
    /// Account settings control for ProStores that login using a token
    /// </summary>
    public partial class ProStoresAccountSettingsTokenControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresAccountSettingsTokenControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the control 
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            mangeTokenControl.InitializeForStore((ProStoresStoreEntity) store);
        }
    }
}
