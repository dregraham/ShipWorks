﻿using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    /// <summary>
    /// Amazon Shipping Profile
    /// </summary>
    public partial class AmazonProfileControl : ShippingProfileControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonProfileControl()
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

            LoadOrigins();
            LoadServices();

            dimensionsControl.Initialize();

            EnumHelper.BindComboBox<AmazonDeliveryExperienceType>(deliveryExperience);

            AmazonProfileEntity amazonProfile = profile.Amazon;
            PackageProfileEntity packageProfile = profile.Packages.Single();

            // Origin
            AddValueMapping(profile, ShippingProfileFields.OriginID, originState, originCombo, labelSender);

            // Shipment
            AddValueMapping(amazonProfile, AmazonProfileFields.ShippingServiceID, serviceState, service, labelService);
            AddValueMapping(amazonProfile, AmazonProfileFields.DeliveryExperience, deliveryExperienceState, deliveryExperience, labelDeliveryExperience);
            AddValueMapping(packageProfile, PackageProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(packageProfile, PackageProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);

            // Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);
        }

        /// <summary>
        /// Populate service combo box
        /// </summary>
        private void LoadServices()
        {
            List<KeyValuePair<string, string>> services;

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IAmazonServiceTypeRepository amazonServiceTypeRepository = scope.Resolve<IAmazonServiceTypeRepository>();
                services = amazonServiceTypeRepository.Get()
                    .Select(serviceTypeEntity => new KeyValuePair<string, string>(serviceTypeEntity.Description, serviceTypeEntity.ApiValue))
                    .ToList();
            }
            
            service.DisplayMember = "Key";
            service.ValueMember = "Value";
            service.DataSource = services;
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
            List<KeyValuePair<string, long>> origins = ShipmentTypeManager.GetType(ShipmentTypeCode.Amazon).GetOrigins();

            originCombo.DisplayMember = "Key";
            originCombo.ValueMember = "Value";
            originCombo.DataSource = origins;
        }
    }
}