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
using ShipWorks.Stores.Platforms.Etsy;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.UI.Platforms.Etsy
{
    /// <summary>
    /// Wizard page to Get number of days back
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.Etsy, ExternallyOwned = true)]
    [Order(typeof(WizardPage), 1)]
    public partial class EtsyDaysBackWizardPage : AddStoreWizardPage
    {
        private readonly IEtsyDaysBackViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyDaysBackWizardPage(IEtsyDaysBackViewModel viewModel)
        {
            this.viewModel = viewModel;
            Title = "Etsy Store Setup";
            Description = "Get number of days back";

            InitializeComponent();
        }

        /// <summary>
        /// Save the page when finished
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!viewModel.Save(GetStore<EtsyStoreEntity>()))
            {
                MessageHelper.ShowInformation(this, viewModel.ErrorMessage);
                e.NextPage = this;
            }
        }
    }
}
