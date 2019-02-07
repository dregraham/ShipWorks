using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// A class for generating a unique identifier for a shipment (based on the order ID and
    /// shipment ID) that gets insured with InsureShip.
    /// </summary>
    public class InsureShipShipmentIdentifier
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipShipmentIdentifier"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public InsureShipShipmentIdentifier(ShipmentEntity shipment)
        {
            this.shipment = shipment;
        }

        /// <summary>
        /// Uses the Order ID and the Shipment ID to create the unique shipment identifier for InsureShip.
        /// </summary>
        public string GetUniqueShipmentId() => GetUniqueShipmentId(shipment);

        /// <summary>
        /// Uses the Order ID and the Shipment ID to create the unique shipment identifier for InsureShip.
        /// </summary>
        public static string GetUniqueShipmentId(IShipmentEntity shipment) =>
            string.Format("{0}-{1}", shipment.OrderID, shipment.ShipmentID);
    }
}
