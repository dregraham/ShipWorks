using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Shipping.UI.Profiles
{
    public partial class GlobalProfileControl : ShippingProfileControlBase
    {
        public GlobalProfileControl()
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

            PackageProfileEntity packageProfile = profile.Packages.Single();

            //From
            AddValueMapping(profile, ShippingProfileFields.OriginID, originState, origin, labelOrigin);

            //Shipment
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
