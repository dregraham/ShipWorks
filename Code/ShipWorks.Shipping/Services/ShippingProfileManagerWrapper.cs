using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Wraps the static ShippingProfileManager with an instance that implements an interface
    /// </summary>
    public class ShippingProfileManagerWrapper : IShippingProfileManager
    {
        private static object syncLock = new object();

        /// <summary>
        /// Get the default profile for the given shipment type
        /// </summary>
        public ShippingProfileEntity GetDefaultProfile(ShipmentTypeCode shipmentTypeCode)
        {
            return ShippingProfileManager.GetDefaultProfile(shipmentTypeCode);
        }

        /// <summary>
        /// Create a profile with the default settings for the shipment type
        /// </summary>
        public ShippingProfileEntity GetOrCreatePrimaryProfile(ShipmentType shipmentType)
        {
            ShippingProfileEntity profile = GetDefaultProfile(shipmentType.ShipmentTypeCode);

            if (profile == null)
            {
                lock (syncLock)
                {
                    profile = GetDefaultProfile(shipmentType.ShipmentTypeCode);

                    if (profile == null)
                    {
                        profile = new ShippingProfileEntity();
                        profile.Name = string.Format("Defaults - {0}", shipmentType.ShipmentTypeName);
                        profile.ShipmentTypeCode = shipmentType.ShipmentTypeCode;
                        profile.ShipmentTypePrimary = true;

                        // Load the shipmentType specific profile data
                        shipmentType.LoadProfileData(profile, true);

                        // Configure it as a primary profile
                        shipmentType.ConfigurePrimaryProfile(profile);

                        // Save the profile
                        ShippingProfileManager.SaveProfile(profile);
                    }
                }
            }

            return profile;
        }
    }
}