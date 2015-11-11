using System;
using System.Text;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.Net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;

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
            HelpLink.LinkClicked += (x, y) => WebHelper.OpenUrl("http://support.shipworks.com/support/solutions/articles/4000066194", this);

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
        }

        /// <summary>
        /// Finish the wizard
        /// </summary>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            shippingSettings.MarkAsConfigured(ShipmentTypeCode.Amazon);

            ShippingOriginEntity origin = new ShippingOriginEntity();

            // Create a person adapter from the new ShippingOriginEntity
            PersonAdapter person = new PersonAdapter(origin, string.Empty);
            contactInformation.SaveToEntity(person);

            // Get the origins description
            origin.Description = GetDefaultDescription(origin);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                try
                {
                    // Save the new ShippingOriginEntity
                    adapter.SaveAndRefetch(origin);

                    // Get the default amazon profile and set its origin to the new origin address
                    ShippingProfileEntity profile = ShippingProfileManager.GetDefaultProfile(ShipmentTypeCode.Amazon);
                    profile.OriginID = origin.ShippingOriginID;
                    adapter.SaveAndRefetch(profile);
                }
                catch (ORMQueryExecutionException ex)
                {
                    // if the exception is because the shipper already exists dont do anything
                    if (!ex.Message.Contains("IX_ShippingOrigin_Description"))
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Get the default description to use for the given shipper
        /// </summary>
        private string GetDefaultDescription(ShippingOriginEntity shipper)
        {
            StringBuilder description = new StringBuilder(new PersonName(new PersonAdapter(shipper, "")).FullName);

            if (shipper.Street1.Length > 0)
            {
                if (description.Length > 0)
                {
                    description.Append(", ");
                }

                description.Append(shipper.Street1);
            }

            if (shipper.PostalCode.Length > 0)
            {
                if (description.Length > 0)
                {
                    description.Append(", ");
                }

                description.Append(shipper.PostalCode);
            }

            return $"Amazon Origin: {description}";
        }
    }
}
