using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration.Ordering;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Shopify;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.UI.Platforms.Shopify
{
    /// <summary>
    /// Wizard page to Get number of days back
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.Shopify, ExternallyOwned = true)]
    [Order(typeof(WizardPage), 1)]
    public partial class ShopifyDaysBackWizardPage : AddStoreWizardPage
    {
        private readonly IShopifyDaysBackViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyDaysBackWizardPage(IShopifyDaysBackViewModel viewModel)
        {
            this.viewModel = viewModel;
            Title = "Shopify Store Setup";
            Description = "Get number of days back";

            InitializeComponent();
        }

        /// <summary>
        /// Save the page when finished
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!viewModel.Save(GetStore<ShopifyStoreEntity>()))
            {
                e.NextPage = this;
            }
        }
    }
}
