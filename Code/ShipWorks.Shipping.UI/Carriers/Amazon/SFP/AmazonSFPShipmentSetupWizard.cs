using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.UI.Wizard;
using System.Net;
using System.IO;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Setup wizard for Amazon shipment type
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypeSetupWizard), ShipmentTypeCode.AmazonSFP)]
    public partial class AmazonSFPShipmentSetupWizard : WizardForm, IShipmentTypeSetupWizard
    {
        private readonly AmazonSFPShipmentType shipmentType;
        private readonly IShippingSettings shippingSettings;
        private readonly IStoreManager storeManager;
        private readonly IShippingProfileManager shippingProfileManager;
        private ShippingWizardPageFinish shippingWizardPageFinish;
        private static readonly ILog log = LogManager.GetLogger(typeof(AmazonSFPShipmentSetupWizard));
        private string termsAndConditions = string.Empty;

        /// <summary>
        /// Constructor to be used by Visual Studio designer
        /// </summary>
        protected AmazonSFPShipmentSetupWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPShipmentSetupWizard(AmazonSFPShipmentType shipmentType,
            IShippingSettings shippingSettings,
            IStoreManager storeManager,
            IShippingProfileManager shippingProfileManager)
            : this()
        {
            this.shipmentType = shipmentType;
            this.shippingSettings = shippingSettings;
            this.storeManager = storeManager;
            this.shippingProfileManager = shippingProfileManager;
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

            UpdateTermsAndConditionsText();
        }

        /// <summary>
        /// Download T&C and display it
        /// </summary>
        private void UpdateTermsAndConditionsText()
        {
            WebClient webClient = new WebClient();
            termsAndConditions = webClient.DownloadString("https://downloads.hub.shipworks.com/AmazonBuyShippingTermsAndConditions.rtf");

            if (termsAndConditions.IsNullOrWhiteSpace())
            {
                MessageHelper.ShowError(this, "Unable to download Terms and Conditions.");
                log.Error("Unable to download Amazon Terms and Conditions");
                return;
            }

            using (MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(termsAndConditions)))
            {
                txtTermsAndConditions.LoadFile(stream, System.Windows.Forms.RichTextBoxStreamType.RichText);
            }
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
        /// Next pressed on welcome screen
        /// </summary>
        private void OnStepNextWelcome(object sender, WizardStepEventArgs e)
        {
            if (!chkTermsAndConditions.Checked || termsAndConditions.IsNullOrWhiteSpace())
            {
                e.NextPage = CurrentPage;
                MessageHelper.ShowMessage(this, "You must accept the Terms and Conditions to continue.");
            }
        }

        /// <summary>
        /// Finish the wizard
        /// </summary>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            shippingSettings.MarkAsConfigured(ShipmentTypeCode.AmazonSFP);

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
                    ShippingProfileEntity profile = shippingProfileManager.GetOrCreatePrimaryProfile(shipmentType);
                    profile.OriginID = origin.ShippingOriginID;
                    shippingProfileManager.SaveProfile(profile);
                }
                catch (ORMQueryExecutionException ex)
                {
                    // if the exception is because the shipper already exists don't do anything
                    if (!ex.Message.Contains("IX_SWDefault_ShippingOrigin_Description"))
                    {
                        throw;
                    }
                }
            }

            IEnumerable<StoreEntity> stores = storeManager.GetAllStores();

            // For each store that can use Amazon shipping
            foreach (var store in stores.Where(s => s is IAmazonCredentials))
            {
                storeManager.CreateStoreStatusFilters(this, store);

                var amazonBuyShippingShipmentType = (AmazonSFPShipmentType) ShipmentTypeManager.GetType(ShipmentTypeCode.AmazonSFP);
                amazonBuyShippingShipmentType.SetupPlatformCarrierIdIfNeeded(store);
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

        /// <summary>
        /// Gets the wizard without any wrapping wizards
        /// </summary>
        public IShipmentTypeSetupWizard GetUnwrappedWizard() => this;
    }
}
