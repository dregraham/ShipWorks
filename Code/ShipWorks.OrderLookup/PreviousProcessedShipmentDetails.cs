namespace ShipWorks.OrderLookup.ShipmentHistory
{
    /// <summary>
    /// Details about a previously processed shipment
    /// </summary>
    public class PreviousProcessedShipmentDetails
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PreviousProcessedShipmentDetails(long shipmentID, bool voided)
        {
            Voided = voided;
            ShipmentID = shipmentID;
        }

        /// <summary>
        /// Shipment ID
        /// </summary>
        public long ShipmentID { get; }

        /// <summary>
        /// Is the shipment voided
        /// </summary>
        public bool Voided { get; }
    }
}
