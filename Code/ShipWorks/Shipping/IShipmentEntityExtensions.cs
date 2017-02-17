using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Extension methods for shipments
    /// </summary>
    public static class IShipmentEntityExtensions
    {
        /// <summary>
        /// Does processing complete externally
        /// </summary>
        /// <remarks>
        /// Created specifically for WorldShip.  A WorldShip shipment is processed in two phases - first it's processed
        /// in ShipWorks, then once its processed in WorldShip its completed.  Opted instead of hard coding WorldShip
        /// if statements to use this instead so its easier to track down all the usages by doing Find References on
        /// this property.
        /// </remarks>
        public static bool ProcessingCompletesExternally(this IShipmentEntity shipment) =>
            shipment?.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip;
    }
}
