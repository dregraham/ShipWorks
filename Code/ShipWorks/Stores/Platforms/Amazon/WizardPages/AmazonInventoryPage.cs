using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using System.IO;
using ShipWorks.UI;
using Interapptive.Shared.IO.Text.Csv;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Common.Threading;
using System.Threading;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Amazon.WizardPages
{
    /// <summary>
    /// Setup Wizard page for loading an inventory report into ShipWorks
    /// </summary>
    public partial class AmazonInventoryPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonInventoryPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the control
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            importInventoryControl.InitializeForStore(GetStore<AmazonStoreEntity>().StoreID);
        }
    }
}
