using System;
using System.Linq;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// BestRate profile control
    /// </summary>
    [KeyedComponent(typeof(ShippingProfileControlBase), ShipmentTypeCode.BestRate)]
    public partial class BestRateProfileControl : ShippingProfileControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
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
            LoadOrigins();
            dimensionsControl.Initialize();

            BestRateProfileEntity bestRateProfile = profile.BestRate;
            PackageProfileEntity packageProfile = profile.Packages.Single();

            //TODO: Implement insurance wording correctly in story SHIP-156: Specifying insurance/declared value with best rate
            //if (ShippingSettings.Fetch().OnTracInsuranceProvider == (int)InsuranceProvider.Carrier)
            //{
            //    insuranceControl.UseInsuranceBoxLabel = "OnTrac Declared Value";
            //    insuranceControl.InsuredValueLabel = "Declared value:";
            //}

            EnumHelper.BindComboBox<ServiceLevelType>(transitTime);

            //From
            AddValueMapping(profile, ShippingProfileFields.OriginID, originState, origin, labelOrigin);

            //Shipment
            AddValueMapping(bestRateProfile, BestRateProfileFields.ServiceLevel, transitTimeState, transitTime, labelTransitTime);
            AddValueMapping(packageProfile, PackageProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(packageProfile, PackageProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);

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

        /// <summary>
        /// Load all the origins
        /// </summary>
        private void LoadOrigins()
        {
            List<KeyValuePair<string, long>> origins = ShipmentTypeManager.GetType(ShipmentTypeCode.BestRate).GetOrigins();

            origin.DisplayMember = "Key";
            origin.ValueMember = "Value";
            origin.DataSource = origins;
        }
    }
}
