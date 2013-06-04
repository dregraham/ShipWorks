using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using System.Xml;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.ProStores.WizardPages
{
    /// <summary>
    /// Wizard page for entering legacy login credentials for ProStores
    /// </summary>
    public partial class ProStoresLegacyLoginPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresLegacyLoginPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the legacy login page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            e.Skip = GetStore<ProStoresStoreEntity>().LoginMethod == (int) ProStoresLoginMethod.ApiToken;
        }

        /// <summary>
        /// Stepping next from the page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            ProStoresStoreEntity store = GetStore<ProStoresStoreEntity>();

            if (accountSettingsControl.SaveToEntity(store))
            {
                store.StoreName = "ProStores: " + store.ShortName;
            }
            else
            {
                e.NextPage = this;
            }
        }
    }
}
