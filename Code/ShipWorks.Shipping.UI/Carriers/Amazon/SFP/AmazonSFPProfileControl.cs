using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Enums;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.UI.Carriers.Amazon.SFP
{
    /// <summary>
    /// Amazon Shipping Profile
    /// </summary>
    public partial class AmazonSFPProfileControl : ShippingProfileControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPProfileControl()
        {
            InitializeComponent();

            ResizeGroupBoxes(tabPageSettings);

            // Amazon does not support ZPL
            requestedLabelFormat.ExcludeFormats(ThermalLanguage.EPL);
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

            EnumHelper.BindComboBox<AmazonSFPDeliveryExperienceType>(deliveryExperience);

            AmazonSFPProfileEntity amazonProfile = profile.AmazonSFP;
            PackageProfileEntity packageProfile = profile.Packages.Single();

            // Origin
            AddValueMapping(profile, ShippingProfileFields.OriginID, originState, originCombo, labelSender);

            // Shipment
            AddValueMapping(amazonProfile, AmazonSFPProfileFields.ShippingServiceID, serviceState, service, labelService);
            AddValueMapping(amazonProfile, AmazonSFPProfileFields.DeliveryExperience, deliveryExperienceState, deliveryExperience, labelDeliveryExperience);
            AddValueMapping(packageProfile, PackageProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(packageProfile, PackageProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);

            AddValueMapping(amazonProfile, AmazonSFPProfileFields.Reference1, reference1State, reference1Token, labelReference1);
            AddValueMapping(amazonProfile.ShippingProfile, ShippingProfileFields.RequestedLabelFormat, requestedLabelFormatState, requestedLabelFormat);

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
                IAmazonSFPServiceTypeRepository amazonServiceTypeRepository = scope.Resolve<IAmazonSFPServiceTypeRepository>();
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
            List<KeyValuePair<string, long>> origins = ShipmentTypeManager.GetType(ShipmentTypeCode.AmazonSFP).GetOrigins();

            originCombo.DisplayMember = "Key";
            originCombo.ValueMember = "Value";
            originCombo.DataSource = origins;
        }
    }
}