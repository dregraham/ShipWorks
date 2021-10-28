using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Usps shipping profile application strategy
    /// </summary>
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.Usps)]
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.Express1Usps)]
    public class UspsShippingProfileApplicationStrategy : PostalShippingProfileApplicationStrategy
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager) :
            base(shipmentTypeManager)
        {
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            base.ApplyProfile(profile, shipment);

            // We can be called during the creation of the base Postal shipment, before the USPS one exists
            if (shipment.Postal.Usps != null)
            {
                UspsShipmentEntity uspsShipment = shipment.Postal.Usps;
                IUspsProfileEntity uspsProfile = profile.Postal.Usps;

                ApplyProfileValue(uspsProfile.UspsAccountID, uspsShipment, UspsShipmentFields.UspsAccountID);
                ApplyProfileValue(uspsProfile.RequireFullAddressValidation, uspsShipment, UspsShipmentFields.RequireFullAddressValidation);
                ApplyProfileValue(uspsProfile.HidePostage, uspsShipment, UspsShipmentFields.HidePostage);
                ApplyProfileValue(uspsProfile.RateShop, uspsShipment, UspsShipmentFields.RateShop);
                ApplyProfileValue(uspsProfile.PostalProfile.Profile.Insurance, uspsShipment, UspsShipmentFields.Insurance);
            }
        }
    }
}