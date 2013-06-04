using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using ShipWorks.UI;
using Interapptive.Shared.Utility;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Magento.WizardPages
{
    /// <summary>
    /// Add Store Wizard page for configuring Magento credentials
    /// </summary>
    public partial class MagentoAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// User is navigating away from this page, so save it's values
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            MagentoStoreEntity store = GetStore<MagentoStoreEntity>();

            if (!accountSettingsControl.SaveToEntity(store))
            {
                e.NextPage = this;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                GenericModuleStoreType storeType = (GenericModuleStoreType)StoreTypeManager.GetType(store);
                storeType.InitializeFromOnlineModule();
            }
            catch (GenericStoreException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                e.NextPage = this;
                return;
            }
        }
    }
}
