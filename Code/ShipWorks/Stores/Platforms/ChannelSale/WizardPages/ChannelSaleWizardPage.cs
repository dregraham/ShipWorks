using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using ShipWorks.UI;
using Interapptive.Shared.Utility;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.ChannelSale.WizardPages
{
    /// <summary>
    /// Add Store Wizard page for configuring GenericStore credentials
    /// </summary>
    public partial class ChannelSaleWizardPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelSaleWizardPage()
        {
            InitializeComponent();

            ChannelSaleStoreType store = (ChannelSaleStoreType) StoreTypeManager.GetType(StoreTypeCode.ChannelSale);

            helpLink.Url = store.AccountSettingsHelpUrl;
        }

        /// <summary>
        /// User is navigating away from this page, so save it's values
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            GenericModuleStoreEntity store = GetStore<GenericModuleStoreEntity>();

            if (!accountSettingsControl.SaveToEntity(store))
            {
                e.NextPage = this;
                return;
            }

            try
            {
                GenericModuleStoreType storeType = (GenericModuleStoreType)StoreTypeManager.GetType(store);
                storeType.InitializeFromOnlineModule();
            }
            catch (GenericModuleConfigurationException ex)
            {
                string message = String.Format("The ShipWorks module returned invalid configuration information.  Please contact the module developer with the following information.\n\n{0}", ex.Message);
                MessageHelper.ShowError(this, message);

                e.NextPage = this;
                return;
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
