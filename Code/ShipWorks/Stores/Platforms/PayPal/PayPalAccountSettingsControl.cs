using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.PayPal
{
    /// <summary>
    /// Control for configuring the PayPal store type.
    /// </summary>
    [ToolboxItem(true)]
    public partial class PayPalAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PayPalAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the UI from the store settings
        /// </summary>
        /// <param name="store"></param>
        public override void LoadStore(StoreEntity store)
        {
            credentials.LoadCredentials(new PayPalAccountAdapter(store, ""));
        }

        /// <summary>
        /// Save the user-entered credentials
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            return credentials.SaveCredentials(new PayPalAccountAdapter(store, ""));
        }
    }
}
