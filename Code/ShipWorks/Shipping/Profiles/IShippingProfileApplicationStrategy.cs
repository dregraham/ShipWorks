using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Strategy for applying a shipping profile to a shipment
    /// </summary>
    public interface IShippingProfileApplicationStrategy
    {
        /// <summary>
        /// Apply the given profile to the given shipment
        /// </summary>
        void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment);
    }
}