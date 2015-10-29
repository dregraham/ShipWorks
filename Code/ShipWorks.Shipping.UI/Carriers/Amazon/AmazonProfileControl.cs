using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;

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

            dimensionsControl.Initialize();
            EnumHelper.BindComboBox<AmazonDeliveryExperienceType>(deliveryExperience);


            AmazonProfileEntity amazonProfile = profile.Amazon;

            //if (ShippingSettings.Fetch().AmazonInsuranceProvider == (int)InsuranceProvider.Carrier)
            //{
            //    insuranceControl.UseInsuranceBoxLabel = "Amazon Declared Value";
            //    insuranceControl.InsuredValueLabel = "Declared value:";
            //}

            //Shipment
            AddValueMapping(amazonProfile, AmazonProfileFields.CarrierWillPickUp, carrierWillPickUpState, carrierWillPickUp, labelCarrierWillPickUp);
            AddValueMapping(amazonProfile, AmazonProfileFields.DeliveryExperience, deliveryExperienceState, deliveryExperience, labelDeliveryExperience);
            AddValueMapping(amazonProfile, AmazonProfileFields.SendDateMustArriveBy, sendDeliverByDateState, sendDeliverByDate, labelSendDeliverByDate);
            AddValueMapping(amazonProfile, AmazonProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(amazonProfile, AmazonProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);
            
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