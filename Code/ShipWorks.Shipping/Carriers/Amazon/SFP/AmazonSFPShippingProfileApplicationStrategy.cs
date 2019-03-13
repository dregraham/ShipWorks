using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Amazon shipping profile application strategy
    /// </summary>
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.AmazonSFP)]
    public class AmazonSFPShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager) :
            base(shipmentTypeManager)
        {
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            base.ApplyProfile(profile, shipment);

            if (shipment.AmazonSFP == null)
            {
                return;
            }

            AmazonSFPShipmentEntity amazonShipment = shipment.AmazonSFP;
            IAmazonSFPProfileEntity amazonProfile = profile.AmazonSFP;

            ApplyProfileValue(amazonProfile.ShippingServiceID, amazonShipment, AmazonSFPShipmentFields.ShippingServiceID);
            ApplyProfileValue(amazonProfile.DeliveryExperience, amazonShipment, AmazonSFPShipmentFields.DeliveryExperience);
            ApplyProfileValue(amazonProfile.Reference1, amazonShipment, AmazonSFPShipmentFields.Reference1);

            ApplyProfileValue(amazonProfile.ShippingProfile.Insurance, amazonShipment, AmazonSFPShipmentFields.Insurance);
            ApplyProfileValue(amazonProfile.ShippingProfile.RequestedLabelFormat, amazonShipment, AmazonSFPShipmentFields.RequestedLabelFormat);

            IPackageProfileEntity packageProfile = profile.Packages.First();
            if (packageProfile.Weight.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }

            ApplyProfileValue(packageProfile.DimsProfileID, amazonShipment, AmazonSFPShipmentFields.DimsProfileID);
            if (packageProfile.DimsProfileID != null)
            {
                if (packageProfile.DimsLength.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(packageProfile.DimsLength, amazonShipment, AmazonSFPShipmentFields.DimsLength);
                }

                if (packageProfile.DimsWidth.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(packageProfile.DimsWidth, amazonShipment, AmazonSFPShipmentFields.DimsWidth);
                }

                if (packageProfile.DimsHeight.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(packageProfile.DimsHeight, amazonShipment, AmazonSFPShipmentFields.DimsHeight);
                }

                ApplyProfileValue(packageProfile.DimsWeight, amazonShipment, AmazonSFPShipmentFields.DimsWeight);
                ApplyProfileValue(packageProfile.DimsAddWeight, amazonShipment, AmazonSFPShipmentFields.DimsAddWeight);
            }
        }
    }
}