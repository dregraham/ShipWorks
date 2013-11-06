using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public partial class BestRateProfileControl : ShippingProfileControlBase
    {
        public BestRateProfileControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the given profile
        /// </summary>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            base.LoadProfile(profile);

            dimensionsControl.Initialize();

            BestRateProfileEntity bestRateProfile = profile.BestRate;

            //TODO: Implement insurance wording correctly in story SHIP-156: Specifying insurance/declared value with best rate
            //if (ShippingSettings.Fetch().OnTracInsuranceProvider == (int)InsuranceProvider.Carrier)
            //{
            //    insuranceControl.UseInsuranceBoxLabel = "OnTrac Declared Value";
            //    insuranceControl.InsuredValueLabel = "Declared value:";
            //}

            EnumHelper.BindComboBox<ServiceLevelType>(transitTime);

            //Shipment
            AddValueMapping(bestRateProfile, BestRateProfileFields.TransitDays, transitTimeState, transitTime, labelTransitTime);
            AddValueMapping(bestRateProfile, BestRateProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(bestRateProfile, BestRateProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);

            //Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);
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
