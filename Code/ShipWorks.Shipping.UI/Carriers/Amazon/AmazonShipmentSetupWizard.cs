using System;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Setup wizard for Amazon shipment type
    /// </summary>
    public partial class AmazonShipmentSetupWizard : ShipmentTypeSetupWizardForm
    {
        private readonly AmazonShipmentType shipmentType;
        private readonly IShippingSettings shippingSettings;
        private ShippingWizardPageFinish shippingWizardPageFinish;

        /// <summary>
        /// Constructor to be used by Visual Studio designer
        /// </summary>
        protected AmazonShipmentSetupWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentType"></param>
        /// <param name="shippingSettings"></param>
        public AmazonShipmentSetupWizard(AmazonShipmentType shipmentType, IShippingSettings shippingSettings) : this()
        {
            this.shipmentType = shipmentType;
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Pages.Add(new ShippingWizardPageDefaults(shipmentType));
            Pages.Add(new ShippingWizardPagePrinting(shipmentType));
            Pages.Add(new ShippingWizardPageAutomation(shipmentType));
            Pages.Add(CreateFinishPage());
        }

        /// <summary>
        /// Create the finish wizard page
        /// </summary>
        private WizardPage CreateFinishPage()
        {
            shippingWizardPageFinish = new ShippingWizardPageFinish(shipmentType);
            shippingWizardPageFinish.SteppingInto += OnSteppingIntoFinish;
            return shippingWizardPageFinish;
        }

        /// <summary>
        /// Next pressed on contact screen
        /// </summary>
        private void OnStepNextContactInfo(object sender, WizardStepEventArgs e)
        {
            if (!contactInformation.ValidateRequiredFields())
            {
                e.NextPage = CurrentPage;
            }

            PersonAdapter shipper = new PersonAdapter();

            contactInformation.SaveToEntity(shipper);

            ShippingOriginEntity shippingOrigin = new ShippingOriginEntity();

            shippingOrigin.Description = shipper.Street1;
            shippingOrigin.FirstName = shipper.FirstName;
            shippingOrigin.LastName = shipper.LastName;
            shippingOrigin.MiddleName = shipper.MiddleName;
            shippingOrigin.Company = shipper.Company;
            shippingOrigin.Street1 = shipper.Street1;
            shippingOrigin.Street2 = shipper.Street2;
            shippingOrigin.Street3 = shipper.Street3;
            shippingOrigin.City = shipper.City;
            shippingOrigin.StateProvCode = shipper.StateProvCode;
            shippingOrigin.PostalCode = shipper.PostalCode;
            shippingOrigin.CountryCode = shipper.CountryCode;
            shippingOrigin.Phone = shipper.Phone;
            shippingOrigin.Fax = shipper.Fax;
            shippingOrigin.Email = shipper.Email;
            shippingOrigin.Website = shipper.Website;

            try
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(shippingOrigin);
                }
            }
            catch (ORMQueryExecutionException ex)
            {
                if (ex.Message.Contains("IX_ShippingOrigin_Description"))
                {
                    MessageHelper.ShowMessage(this, "A shipper with the selected name or description already exists.");
                }
                else
                {
                    throw;
                }
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this, "Your changes cannot be saved because another use has deleted the shipper.");
                DialogResult = DialogResult.Abort;
            }
        }

        /// <summary>
        /// Finish the wizard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            shippingSettings.MarkAsConfigured(ShipmentTypeCode.Amazon);
        }
    }
}
