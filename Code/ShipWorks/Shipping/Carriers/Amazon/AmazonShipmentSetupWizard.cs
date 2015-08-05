using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    public partial class AmazonShipmentSetupWizard : ShipmentTypeSetupWizardForm
    {
        private readonly AmazonCredentials credentialViewModel;

        public AmazonShipmentSetupWizard()
        {
            InitializeComponent();
        }

        public AmazonShipmentSetupWizard(AmazonCredentials credentialViewModel) : this()
        {
            this.credentialViewModel = credentialViewModel;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            merchantId.DataBindings.Add(ObjectUtility.Nameof(() => merchantId.Text), credentialViewModel, ObjectUtility.Nameof(() => credentialViewModel.MerchantId));
            authToken.DataBindings.Add(ObjectUtility.Nameof(() => authToken.Text), credentialViewModel, ObjectUtility.Nameof(() => credentialViewModel.AuthToken)); //, false, DataSourceUpdateMode.OnPropertyChanged);

            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.Amazon);

            var shippingWizardPageFinish = new ShippingWizardPageFinish(shipmentType);

            Pages.Add(new ShippingWizardPageDefaults(shipmentType));
            Pages.Add(new ShippingWizardPagePrinting(shipmentType));
            Pages.Add(new ShippingWizardPageAutomation(shipmentType));
            Pages.Add(shippingWizardPageFinish);

            shippingWizardPageFinish.SteppingInto += OnSteppingIntoFinish;
        }

        /// <summary>
        /// Stepping next from the credentials page
        /// </summary>
        private void OnNextStepCredentials(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            credentialViewModel.Validate();

            Cursor.Current = Cursors.Default;

            if (!credentialViewModel.Success)
            {
                MessageHelper.ShowError(this, credentialViewModel.Message);
                e.NextPage = CurrentPage;
            }
        }

        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
