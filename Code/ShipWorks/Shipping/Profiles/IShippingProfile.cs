using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Services;
using ShipWorks.Templates.Printing;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Shipping profile
    /// </summary>
    public interface IShippingProfile
    {
        /// <summary>
        /// Shipping Profile
        /// </summary>
        IShippingProfileEntity ShippingProfileEntity { get; }

        /// <summary>
        /// Shortcut
        /// </summary>
        IShortcutEntity Shortcut { get; }

        /// <summary>
        /// Does this profile have a shortcut or barcode
        /// </summary>
        bool HasShortcutOrBarcode { get; }

        /// <summary>
        /// Apply profile to shipments
        /// </summary>
        IEnumerable<ICarrierShipmentAdapter> Apply(IEnumerable<ShipmentEntity> shipment);

        /// <summary>
        /// Is the profile applicable to the given ShipmentTypeCode
        /// </summary>
        bool IsApplicable(ShipmentTypeCode? shipmentTypeCode);

        /// <summary>
        /// Apply profile to shipment
        /// </summary>
        ICarrierShipmentAdapter Apply(ShipmentEntity shipment);

        /// <summary>
        /// Get a printable barcode of this profile
        /// </summary>
        PrintableBarcode ToPrintableBarcode();
    }
}
