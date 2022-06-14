﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.Stores;
using ShipWorks.UI.Wizard;

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

            foreach (StoreEntity store in stores.Where(s =>
                s.TypeCode == (int) StoreTypeCode.Amazon || s.TypeCode == (int) StoreTypeCode.ChannelAdvisor))
            {
                storeManager.CreateStoreStatusFilters(this, store);

                if (store.StoreTypeCode == StoreTypeCode.Amazon)
                {
                    var amazonBuyShippingShipmentType = (AmazonSFPShipmentType) ShipmentTypeManager.GetType(ShipmentTypeCode.AmazonSFP);
                    amazonBuyShippingShipmentType.SetupPlatformCarrierIdIfNeeded((AmazonStoreEntity) store);
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

        /// <summary>
        /// Gets the wizard without any wrapping wizards
        /// </summary>
        public IShipmentTypeSetupWizard GetUnwrappedWizard() => this;
    }
}
