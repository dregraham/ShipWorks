﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Asendia;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.UI.Carriers.Asendia
{
    /// <summary>
    /// Asendia Profile Control
    /// </summary>
    [KeyedComponent(typeof(ShippingProfileControlBase), ShipmentTypeCode.Asendia)]
    public partial class AsendiaProfileControl : ShippingProfileControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsendiaProfileControl" /> class.
        /// </summary>
        public AsendiaProfileControl()
        {
            InitializeComponent();

            ResizeGroupBoxes(tabPageSettings);

            // ShipEngine does not support EPL
            requestedLabelFormat.ExcludeFormats(ThermalLanguage.EPL);
        }

        /// <summary>
        /// Load the given profile
        /// </summary>
        /// <param name="profile"></param>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            dimensionsControl.Initialize();

            AsendiaProfileEntity asendiaProfile = profile.Asendia;
            PackageProfileEntity packageProfile = profile.Packages.Single();

            LoadAsendiaAccounts();

            EnumHelper.BindComboBox<AsendiaServiceType>(service);
            EnumHelper.BindComboBox<TaxIdType>(customsRecipientTINType);
            EnumHelper.BindComboBox<ShipEngineContentsType>(contents);
            EnumHelper.BindComboBox<ShipEngineNonDeliveryType>(nonDelivery);

            customsRecipientIssuingAuthority.DisplayMember = "Key";
            customsRecipientIssuingAuthority.ValueMember = "Value";
            customsRecipientIssuingAuthority.DataSource = Geography.Countries.Select(n => new KeyValuePair<string, string>(n, Geography.GetCountryCode(n))).ToList();

            // From
            AddValueMapping(asendiaProfile, AsendiaProfileFields.AsendiaAccountID, accountState, asendiaAccount, labelAccount);

            // Service
            AddValueMapping(asendiaProfile, AsendiaProfileFields.Service, serviceState, service, labelService);

            // Weight and dimensions
            AddValueMapping(packageProfile, PackageProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(packageProfile, PackageProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);

            // Labels
            AddValueMapping(profile, ShippingProfileFields.RequestedLabelFormat, requestedLabelFormatState, requestedLabelFormat, labelThermalNote);

            // Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);

            //Options
            AddValueMapping(asendiaProfile, AsendiaProfileFields.NonMachinable, nonMachinableState, nonMachinable, labelNonMachinable);

            //Customs
            AddValueMapping(asendiaProfile, AsendiaProfileFields.Contents, contentsState, contents, labelContents);
            AddValueMapping(asendiaProfile, AsendiaProfileFields.NonDelivery, nonDeliveryState, nonDelivery, labelNonDelivery);
            //Tax Id
            AddValueMapping(asendiaProfile, AsendiaProfileFields.CustomsRecipientTin, customsRecipientTINState, customsRecipientTIN, labelCustomsRecipientTIN);
            AddValueMapping(asendiaProfile, AsendiaProfileFields.CustomsRecipientTinType, customsRecipientTINTypeState, customsRecipientTINType, labelCustomsRecipientTINType);
            AddValueMapping(asendiaProfile, AsendiaProfileFields.CustomsRecipientIssuingAuthority, customsRecipientIssuingAuthorityState, customsRecipientIssuingAuthority, labelCustomsRecipientIssuingAuthority);
        }

        /// <summary>
        /// Loads the Asendia accounts.
        /// </summary>
        private void LoadAsendiaAccounts()
        {
            asendiaAccount.DisplayMember = "Key";
            asendiaAccount.ValueMember = "Value";

            if (AsendiaAccountManager.Accounts.Count > 0)
            {
                asendiaAccount.DataSource = AsendiaAccountManager.Accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.AsendiaAccountID)).ToList();
                asendiaAccount.Enabled = true;
            }
            else
            {
                asendiaAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                asendiaAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Save all the package profile stuff to their entities
        /// </summary>
        public override void SaveToEntity()
        {
            base.SaveToEntity();

            if (dimensionsControl.Enabled)
            {
                dimensionsControl.SaveToEntities();
            }
        }
    }
}
