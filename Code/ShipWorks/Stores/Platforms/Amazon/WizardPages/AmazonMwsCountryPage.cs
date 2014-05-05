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

namespace ShipWorks.Stores.Platforms.Amazon.WizardPages
{
    public partial class AmazonMwsCountryPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsCountryPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Step Next
        /// </summary>
        private void OnStepNext(object sender, UI.Wizard.WizardStepEventArgs e)
        {
            AmazonStoreEntity amazonStore = GetStore<AmazonStoreEntity>();

            if (!storeSettingsControl.SaveToEntity(amazonStore))
            {
                e.NextPage = this;
            }
        }
    }
}
