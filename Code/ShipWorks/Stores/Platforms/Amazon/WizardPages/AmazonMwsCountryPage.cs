using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Amazon.WizardPages
{
    /// <summary>
    /// Select the country that an Amazon store is in
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.Amazon, ExternallyOwned = true)]
    [Order(typeof(WizardPage), 1)]
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
