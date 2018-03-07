using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// 
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
            
            ShippingProfileUtility.ApplyProfileValue(amazonProfile.ShippingServiceID, amazonShipment, AmazonShipmentFields.ShippingServiceID);
            ShippingProfileUtility.ApplyProfileValue(amazonProfile.DeliveryExperience, amazonShipment, AmazonShipmentFields.DeliveryExperience);
            ShippingProfileUtility.ApplyProfileValue(amazonProfile.ShippingProfile.Insurance, amazonShipment, AmazonShipmentFields.Insurance);
            
            IPackageProfileEntity packageProfile = profile.Packages.First();
            if (packageProfile.Weight.GetValueOrDefault() > 0)
            {
                ShippingProfileUtility.ApplyProfileValue(packageProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }

            ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsProfileID, amazonShipment, AmazonShipmentFields.DimsProfileID);
            if (packageProfile.DimsProfileID != null)
            {
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsLength, amazonShipment, AmazonShipmentFields.DimsLength);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWidth, amazonShipment, AmazonShipmentFields.DimsWidth);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsHeight, amazonShipment, AmazonShipmentFields.DimsHeight);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWeight, amazonShipment, AmazonShipmentFields.DimsWeight);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsAddWeight, amazonShipment, AmazonShipmentFields.DimsAddWeight);
            }
        }
    }
}