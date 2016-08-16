using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// OnTrac Shipping Profile
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class OnTracProfileControl : ShippingProfileControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracProfileControl()
        {
            InitializeComponent();

            ResizeGroupBoxes(tabPageSettings);
        }

        /// <summary>
        /// Load the given profile
        /// </summary>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            dimensionsControl.Initialize();

            OnTracProfileEntity onTracProfile = profile.OnTrac;

            if (ShippingSettings.Fetch().OnTracInsuranceProvider == (int)InsuranceProvider.Carrier)
            {
                insuranceControl.UseInsuranceBoxLabel = "OnTrac Declared Value";
                insuranceControl.InsuredValueLabel = "Declared value:";
            }

            LoadOnTracAccounts();

            EnumHelper.BindComboBox<ResidentialDeterminationType>(residentialDetermination, t => t != ResidentialDeterminationType.FedExAddressLookup);
            EnumHelper.BindComboBox<OnTracServiceType>(service, type => type != OnTracServiceType.None);
            EnumHelper.BindComboBox<OnTracPackagingType>(packaging);

            //From
            AddValueMapping(onTracProfile, OnTracProfileFields.OnTracAccountID, accountState, onTracAccount, labelAccount);

            //To
            AddValueMapping(onTracProfile, OnTracProfileFields.ResidentialDetermination, residentialState, residentialDetermination, labelResidential);

            //Shipment
            AddValueMapping(onTracProfile, OnTracProfileFields.Service, serviceState, service, labelService);
            AddValueMapping(onTracProfile, OnTracProfileFields.SaturdayDelivery, saturdayState, saturdayDelivery, labelSaturday);
            AddValueMapping(onTracProfile, OnTracProfileFields.SignatureRequired, signatureState, signatureRequired, labelSignature);
            AddValueMapping(onTracProfile, OnTracProfileFields.PackagingType, packagingState, packaging, labelPackaging);
            AddValueMapping(onTracProfile, OnTracProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(onTracProfile, OnTracProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);

            //Options
            AddValueMapping(onTracProfile, OnTracProfileFields.Reference1, referenceState, referenceNumber, labelReference);
            AddValueMapping(onTracProfile, OnTracProfileFields.Reference2, reference2State, referenceNumber2, labelReference2);
            AddValueMapping(onTracProfile, OnTracProfileFields.Instructions, instructionsState, instructions, labelInstructions);

            // Labels
            AddValueMapping(profile, ShippingProfileFields.RequestedLabelFormat, requestedLabelFormatState, requestedLabelFormat);

            //Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);
        }

        /// <summary>
        /// Load the list of OnTrac accounts
        /// </summary>
        void LoadOnTracAccounts()
        {
            onTracAccount.DisplayMember = "Key";
            onTracAccount.ValueMember = "Value";

            if (OnTracAccountManager.Accounts.Count > 0)
            {
                onTracAccount.DataSource = OnTracAccountManager.Accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.OnTracAccountID)).ToList();
                onTracAccount.Enabled = true;
            }
            else
            {
                onTracAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                onTracAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Save to entity.
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