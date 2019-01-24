using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Amazon shipping profile application strategy
    /// </summary>
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.Amazon)]
    public class AmazonShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager) :
            base(shipmentTypeManager)
        {
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            base.ApplyProfile(profile, shipment);

            if (shipment.Amazon == null)
            {
                return;
            }

            AmazonShipmentEntity amazonShipment = shipment.Amazon;
            IAmazonProfileEntity amazonProfile = profile.Amazon;

            ApplyProfileValue(amazonProfile.ShippingServiceID, amazonShipment, AmazonShipmentFields.ShippingServiceID);
            ApplyProfileValue(amazonProfile.DeliveryExperience, amazonShipment, AmazonShipmentFields.DeliveryExperience);
            ApplyProfileValue(amazonProfile.Reference1, amazonShipment, AmazonShipmentFields.Reference1);

            ApplyProfileValue(amazonProfile.ShippingProfile.Insurance, amazonShipment, AmazonShipmentFields.Insurance);
            ApplyProfileValue(amazonProfile.ShippingProfile.RequestedLabelFormat, amazonShipment, AmazonShipmentFields.RequestedLabelFormat);

            IPackageProfileEntity packageProfile = profile.Packages.First();
            if (packageProfile.Weight.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }

            ApplyProfileValue(packageProfile.DimsProfileID, amazonShipment, AmazonShipmentFields.DimsProfileID);
            if (packageProfile.DimsProfileID != null)
            {
                if (packageProfile.DimsLength.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(packageProfile.DimsLength, amazonShipment, AmazonShipmentFields.DimsLength);
                }

                if (packageProfile.DimsWidth.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(packageProfile.DimsWidth, amazonShipment, AmazonShipmentFields.DimsWidth);
                }

                if (packageProfile.DimsHeight.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(packageProfile.DimsHeight, amazonShipment, AmazonShipmentFields.DimsHeight);
                }

                ApplyProfileValue(packageProfile.DimsWeight, amazonShipment, AmazonShipmentFields.DimsWeight);
                ApplyProfileValue(packageProfile.DimsAddWeight, amazonShipment, AmazonShipmentFields.DimsAddWeight);
            }
        }
    }
}