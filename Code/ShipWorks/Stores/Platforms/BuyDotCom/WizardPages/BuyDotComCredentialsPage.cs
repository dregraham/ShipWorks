using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;
using ShipWorks.FileTransfer;
using ShipWorks.Data.Connection;
using System.Net;
using Interapptive.Shared.Net;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.BuyDotCom.WizardPages
{
    /// <summary>
    /// Page with FTP credentials
    /// </summary>
    public partial class BuyDotComCredentialsPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComCredentialsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On Step Next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!settingsControl.SaveToEntity(GetStore<BuyDotComStoreEntity>()))
            {
                e.NextPage = this;
            }
        }
    }
}