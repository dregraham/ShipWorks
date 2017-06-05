using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// An instance of a local rate not matching the shipment cost of a UPS shipment
    /// </summary>
    public class UpsLocalRateDiscrepancy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRateDiscrepancy"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="localRate">The local rate.</param>
        public UpsLocalRateDiscrepancy(ShipmentEntity shipment, UpsLocalServiceRate localRate)
        {
            Shipment = shipment;
            LocalRate = localRate;
        }

        /// <summary>
        /// The shipment with the local rate discrepancy
        /// </summary>
        public ShipmentEntity Shipment { get; set; }

        /// <summary>
        /// The local rate that caused the discrepancy
        /// </summary>
        public UpsLocalServiceRate LocalRate { get; set; }
        
        /// <summary>
        /// Logs the discrepancies. Includes the shipmentID, the local rate, and the actual shipment cost
        /// </summary>
        public string GetLogMessage()
        {
            return $"Shipment ID: {Shipment.ShipmentID}" + Environment.NewLine + 
                   $"Local Rate: {LocalRate?.Amount.ToString("C") ?? "Not found"}" + Environment.NewLine +
                   $"Label Cost: {Shipment.ShipmentCost:C}" + Environment.NewLine ;
        }
    }
}