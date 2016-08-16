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

namespace ShipWorks.Stores.Platforms.OrderDynamics.WizardPages
{
    /// <summary>
    /// Add Store Wizard page for configuring GenericStore credentials
    /// </summary>
    public partial class OrderDynamicsAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDynamicsAccountPage()
        {
            InitializeComponent();

            OrderDynamicsStoreType store = (OrderDynamicsStoreType) StoreTypeManager.GetType(StoreTypeCode.OrderDynamics);

            helpLink.Url = store.AccountSettingsHelpUrl;
        }

        /// <summary>
        /// User is navigating away from this page, so save it's values
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            GenericModuleStoreEntity store = GetStore<GenericModuleStoreEntity>();

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

        /// <summary>
        /// Loads the store settings
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            accountSettingsControl.LoadStore(((AddStoreWizard)Wizard).Store);
        }
    }
}
