using System;

namespace ShipWorks.OrderLookup.ShipmentHistory
{
    /// <summary>
    /// Header for the shipment history grid
    /// </summary>
    public class ShipmentHistoryHeader
    {
        private readonly string trackingNumber;
        private readonly string orderIDText;
        private readonly string orderNumberComplete;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentHistoryHeader(long shipmentID, string trackingNumber, long orderID, string orderNumberComplete)
        {
            this.orderNumberComplete = orderNumberComplete;
            this.orderIDText = orderID.ToString();
            this.trackingNumber = trackingNumber;
            ShipmentID = shipmentID;
        }

        /// <summary>
        /// Shipment ID
        /// </summary>
        public long ShipmentID { get; }

        /// <summary>
        /// Does the header match the given filter
        /// </summary>
        public bool MatchesFilter(string searchText) =>
            string.IsNullOrEmpty(searchText) ||
            trackingNumber.StartsWith(searchText, StringComparison.OrdinalIgnoreCase) ||
            orderNumberComplete.StartsWith(searchText, StringComparison.OrdinalIgnoreCase) ||
            orderIDText.StartsWith(searchText, StringComparison.OrdinalIgnoreCase);
    }
}
