using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Best Rate shipping profile application strategy
    /// </summary>
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.BestRate)]
    public class BestRateShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager) :
            base(shipmentTypeManager)
        {
        }
        
        /// <summary>
        /// Apply the specified shipment profile to the given shipment.
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            base.ApplyProfile(profile, shipment);
 
            BestRateShipmentEntity bestRateShipment = shipment.BestRate;
            IBestRateProfileEntity bestRateProfile = profile.BestRate;
            IPackageProfileEntity packageProfileEntity = profile.Packages.Single();

            ShippingProfileUtility.ApplyProfileValue(packageProfileEntity.DimsProfileID, bestRateShipment, BestRateShipmentFields.DimsProfileID);
            ShippingProfileUtility.ApplyProfileValue(packageProfileEntity.DimsWeight, bestRateShipment, BestRateShipmentFields.DimsWeight);
            ShippingProfileUtility.ApplyProfileValue(packageProfileEntity.DimsLength, bestRateShipment, BestRateShipmentFields.DimsLength);
            ShippingProfileUtility.ApplyProfileValue(packageProfileEntity.DimsHeight, bestRateShipment, BestRateShipmentFields.DimsHeight);
            ShippingProfileUtility.ApplyProfileValue(packageProfileEntity.DimsWidth, bestRateShipment, BestRateShipmentFields.DimsWidth);
            ShippingProfileUtility.ApplyProfileValue(packageProfileEntity.DimsAddWeight, bestRateShipment, BestRateShipmentFields.DimsAddWeight);

            ShippingProfileUtility.ApplyProfileValue(bestRateProfile.ServiceLevel, bestRateShipment, BestRateShipmentFields.ServiceLevel);
            ShippingProfileUtility.ApplyProfileValue(bestRateProfile.ShippingProfile.Insurance, bestRateShipment, BestRateShipmentFields.Insurance);

            if (packageProfileEntity.Weight.HasValue && packageProfileEntity.Weight.Value != 0)
            {
                ShippingProfileUtility.ApplyProfileValue(packageProfileEntity.Weight, shipment, ShipmentFields.ContentWeight);
            }
        }
    }
}