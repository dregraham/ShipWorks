using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using Interapptive.Shared.Net;
using log4net;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model;

namespace ShipWorks.Stores.Platforms.Miva
{
    /// <summary>
    /// UserControl for managing the options for the Sebenza modules integration
    /// </summary>
    public partial class MivaSebenzaOptionsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MivaSebenzaOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the options from the store into the UI
        /// </summary>
        public void LoadStore(MivaStoreEntity store)
        {
            sebenzaAddtionalCheckout.Checked = store.SebenzaCheckoutDataEnabled;
        }

        /// <summary>
        /// Save the options from the UI into the store
        /// </summary>
        public void SaveToEntity(MivaStoreEntity store)
        {
            store.SebenzaCheckoutDataEnabled = sebenzaAddtionalCheckout.Checked;
        }
    }
}
