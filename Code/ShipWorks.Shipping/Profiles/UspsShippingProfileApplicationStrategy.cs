using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Usps shipping profile application strategy
    /// </summary>
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.Usps)]
    public class UspsShippingProfileApplicationStrategy : IShippingProfileApplicationStrategy
    {
        private readonly IShippingProfileApplicationStrategy baseStrategy;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsShippingProfileApplicationStrategy(IShippingProfileApplicationStrategy baseStrategy)
        {
            this.baseStrategy = baseStrategy;
        }
        
        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            baseStrategy.ApplyProfile(profile, shipment);

            // We can be called during the creation of the base Postal shipment, before the USPS one exists
            if (shipment.Postal.Usps != null)
            {
                UspsShipmentEntity uspsShipment = shipment.Postal.Usps;
                IUspsProfileEntity uspsProfile = profile.Postal.Usps;

                ShippingProfileUtility.ApplyProfileValue(uspsProfile.UspsAccountID, uspsShipment, UspsShipmentFields.UspsAccountID);
                ShippingProfileUtility.ApplyProfileValue(uspsProfile.RequireFullAddressValidation, uspsShipment, UspsShipmentFields.RequireFullAddressValidation);
                ShippingProfileUtility.ApplyProfileValue(uspsProfile.HidePostage, uspsShipment, UspsShipmentFields.HidePostage);
                ShippingProfileUtility.ApplyProfileValue(uspsProfile.RateShop, uspsShipment, UspsShipmentFields.RateShop);
                ShippingProfileUtility.ApplyProfileValue(uspsProfile.PostalProfile.Profile.Insurance, uspsShipment, UspsShipmentFields.Insurance);
            }
        }
    }
}