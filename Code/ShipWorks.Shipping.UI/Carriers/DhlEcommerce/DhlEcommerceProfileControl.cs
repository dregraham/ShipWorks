using System.Collections.Generic;
using System.Data;
using System.Linq;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.DhlEcommerce;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.UI.Carriers.DhlEcommerce
{
    /// <summary>
    /// DHL eCommerce Profile Control
    /// </summary>
    [KeyedComponent(typeof(ShippingProfileControlBase), ShipmentTypeCode.DhlEcommerce)]
    public partial class DhlEcommerceProfileControl : ShippingProfileControlBase
    {
        private readonly IDhlEcommerceAccountRepository accountRepo;

        /// <summary>
        /// Constructor for Visual Studio Designer
        /// </summary>
        public DhlEcommerceProfileControl()
        {
            InitializeComponent();

            dimensions.Initialize();

            ResizeGroupBoxes(tabPageSettings);

            // ShipEngine only support Standard for DHL eCommerce
            requestedLabelFormat.ExcludeFormats(ThermalLanguage.EPL, ThermalLanguage.ZPL);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceProfileControl(IDhlEcommerceAccountRepository accountRepo) : this()
        {
            this.accountRepo = accountRepo;
        }

        /// <summary>
        /// Load the given profile
        /// </summary>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            var dhlEcommerceProfile = profile.DhlEcommerce;
            var packageProfile = profile.Packages.Single();

            LoadDhlEcommerceAccounts();

            EnumHelper.BindComboBox<DhlEcommerceServiceType>(service);
            EnumHelper.BindComboBox<DhlEcommercePackagingType>(packageType);
            EnumHelper.BindComboBox<ShipEngineContentsType>(contents);
            EnumHelper.BindComboBox<ShipEngineNonDeliveryType>(nonDelivery);
            EnumHelper.BindComboBox<TaxIdType>(taxIdType);

            customsTinIssuingAuthority.DisplayMember = "Key";
            customsTinIssuingAuthority.ValueMember = "Value";
            customsTinIssuingAuthority.DataSource = Geography.Countries.Select(n => new KeyValuePair<string, string>(n, Geography.GetCountryCode(n))).ToList();

            if (ShippingSettings.Fetch().DhlEcommerceInsuranceProvider == (int) InsuranceProvider.Carrier)
            {
                insuranceControl.UseInsuranceBoxLabel = "DHL Declared Value";
                insuranceControl.InsuredValueLabel = "Declared value:";
            }

            // From
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.DhlEcommerceAccountID, accountState, dhlEcommerceAccount, labelAccount);

            // Shipment
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.Service, serviceState, service, labelService);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.PackagingType, packageTypeState, packageType, labelPackageType);
            AddValueMapping(packageProfile, PackageProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(packageProfile, PackageProfileFields.DimsProfileID, dimensionsState, dimensions, labelDimensions);

            // Labels
            AddValueMapping(profile, ShippingProfileFields.RequestedLabelFormat, requestedLabelFormatState, requestedLabelFormat, labelThermalNote);

            // Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);

            // Options
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.SaturdayDelivery, saturdayState, saturdayDelivery, labelSaturday);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.DeliveryDutyPaid, dutyDeliveryPaidState, dutyDeliveryPaid, labelDuty);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.NonMachinable, nonMachinableState, nonMachinable, labelNonMachinable);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.ResidentialDelivery, resDeliveryState, resDelivery, labelResDelivery);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.Reference1, reference1State, reference1, labelReference1);

            // Customs
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.Contents, contentsState, contents, labelContents);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.NonDelivery, nonDeliveryState, nonDelivery, labelNonDelivery);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.CustomsRecipientTin, customsRecipientTinState, customsRecipientTin, labelCustomsRecipientTin);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.CustomsTaxIdType, taxIdTypeState, taxIdType, labelTaxIdType);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.CustomsTinIssuingAuthority, customsTinIssuingAuthorityState, customsTinIssuingAuthority, labelCustomsTinIssuingAuthority);
        }

        /// <summary>
        /// Loads the DHL eCommerce accounts.
        /// </summary>
        private void LoadDhlEcommerceAccounts()
        {
            dhlEcommerceAccount.DisplayMember = "Key";
            dhlEcommerceAccount.ValueMember = "Value";

            if (accountRepo.Accounts.Count() > 0)
            {
                dhlEcommerceAccount.DataSource = accountRepo.Accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.DhlEcommerceAccountID)).ToList();
                dhlEcommerceAccount.Enabled = true;
            }
            else
            {
                dhlEcommerceAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                dhlEcommerceAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Save all the package profile stuff to their entities
        /// </summary>
        public override void SaveToEntity()
        {
            base.SaveToEntity();

            if (dimensions.Enabled)
            {
                dimensions.SaveToEntities();
            }
        }
    }
}