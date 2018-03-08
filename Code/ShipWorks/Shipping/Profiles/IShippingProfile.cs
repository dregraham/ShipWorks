using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Represents a ShippingProfile
    /// </summary>
    public interface IShippingProfile
    {
        /// <summary>
        /// The associated ShipmentType description. Blank if global
        /// </summary>
        string ShipmentTypeDescription { get; }

        /// <summary>
        /// Shipping Profile
        /// </summary>
        ShippingProfileEntity ShippingProfileEntity { get; }

        /// <summary>
        /// Shortcut
        /// </summary>
        ShortcutEntity Shortcut { get; }

        /// <summary>
        /// The associated Shortcut 
        /// </summary>
        /// <remarks>
        /// This is the description of the ShortcutKey. Blank if no associated keyboard shortcut
        /// </remarks>
        string ShortcutKey { get; }

        /// <summary>
        /// Apply profile to shipment
        /// </summary>
        void Apply(ShipmentEntity shipment);

        /// <summary>
        /// Change profile to be of specified ShipmentType
        /// </summary>
        void ChangeProvider(ShipmentTypeCode? shipmentType);

        /// <summary>
        /// Validate that the shippintProfile can be saved
        /// </summary>
        Result Validate(IShippingProfileManager profileManager, IShortcutManager shortcutManager);

        /// <summary>
        /// Load the ShippingProfileEntities data
        /// </summary>
        void LoadProfileData(bool refreshIfPresent);
    }
}