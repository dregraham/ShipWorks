using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Profiles
{
    public interface IShippingProfile
    {
        string ShipmentTypeDescription { get; }
        ShippingProfileEntity ShippingProfileEntity { get; }
        ShortcutEntity Shortcut { get; }
        string ShortcutKey { get; }

        void Apply(ShipmentEntity shipment);
        void ChangeProvider(ShipmentTypeCode? shipmentType);
        Result Validate();
    }
}