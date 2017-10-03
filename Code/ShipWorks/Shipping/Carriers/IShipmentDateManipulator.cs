using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Manipulate the date of a shipment
    /// </summary>
    public interface IShipmentDateManipulator
    {
        /// <summary>
        /// Manipulate the date of the given shipment
        /// </summary>
        bool Manipulate(ShipmentEntity shipment);
    }
}
