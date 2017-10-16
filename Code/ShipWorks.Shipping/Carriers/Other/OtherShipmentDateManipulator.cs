using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Other
{
    /// <summary>
    /// Manipulate the date of the given shipment
    /// </summary>
    [KeyedComponent(typeof(IShipmentDateManipulator), ShipmentTypeCode.Other)]
    public class OtherShipmentDateManipulator : IShipmentDateManipulator
    {
        /// <summary>
        /// Manipulate the date of the given shipment
        /// </summary>
        public void Manipulate(ShipmentEntity shipment)
        {
            // ShipWorks typically auto-updates the ShipDate on unprocessed shipments to be Today at
            // the earliest.  But the use-case for Other shipments is a bit different, where people
            // manually enter shipment details, often occurring in the past.
        }
    }
}
