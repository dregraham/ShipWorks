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

            ApplyProfileValue(packageProfileEntity.DimsProfileID, bestRateShipment, BestRateShipmentFields.DimsProfileID);
            ApplyProfileValue(packageProfileEntity.DimsWeight, bestRateShipment, BestRateShipmentFields.DimsWeight);

            if (packageProfileEntity.DimsLength.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfileEntity.DimsLength, bestRateShipment, BestRateShipmentFields.DimsLength);
            }

            if (packageProfileEntity.DimsWidth.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfileEntity.DimsWidth, bestRateShipment, BestRateShipmentFields.DimsWidth);
            }
            
            if (packageProfileEntity.DimsHeight.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfileEntity.DimsHeight, bestRateShipment, BestRateShipmentFields.DimsHeight);
            }

            ApplyProfileValue(packageProfileEntity.DimsAddWeight, bestRateShipment, BestRateShipmentFields.DimsAddWeight);
            ApplyProfileValue(bestRateProfile.ServiceLevel, bestRateShipment, BestRateShipmentFields.ServiceLevel);
            ApplyProfileValue(bestRateProfile.ShippingProfile.Insurance, bestRateShipment, BestRateShipmentFields.Insurance);

            if (packageProfileEntity.Weight.HasValue && packageProfileEntity.Weight.Value != 0)
            {
                ApplyProfileValue(packageProfileEntity.Weight, shipment, ShipmentFields.ContentWeight);
            }

            ApplyProfileValue(bestRateProfile.InternalAllowedCarrierAccounts, bestRateShipment, BestRateShipmentFields.InternalAllowedCarrierAccounts);

            //WORKS-4318 - add DIMS for DhlEcommerce, because after change Shipper dims for DhlEcommerce was empty
            //if DhlEcommerce profile will not be empty, DIMS from BestRate will be overwriten
            ApplyProfilesPackageDimsForDhlEcommerce(profile, shipment);
        }


        /// <summary>
        /// Apply the profiles dims
        /// </summary>
        private static void ApplyProfilesPackageDimsForDhlEcommerce(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            var dhlShipment = shipment.DhlEcommerce != null ? shipment.DhlEcommerce : new DhlEcommerceShipmentEntity();
            
            if(dhlShipment != null){

                var packageProfile = profile.Packages.Single();

                IDhlEcommerceProfileEntity source = profile.DhlEcommerce;

                ApplyProfileValue(packageProfile.DimsWeight, dhlShipment, DhlEcommerceShipmentFields.DimsWeight);

                if (packageProfile.DimsLength.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(packageProfile.DimsLength, dhlShipment, DhlEcommerceShipmentFields.DimsLength);
                }

                if (packageProfile.DimsWidth.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(packageProfile.DimsWidth, dhlShipment, DhlEcommerceShipmentFields.DimsWidth);
                }

                if (packageProfile.DimsHeight.GetValueOrDefault() > 0)
                {
                    ApplyProfileValue(packageProfile.DimsHeight, dhlShipment, DhlEcommerceShipmentFields.DimsHeight);
                }

                ApplyProfileValue(packageProfile.DimsAddWeight, dhlShipment, DhlEcommerceShipmentFields.DimsAddWeight);
            }
        }
    }
}