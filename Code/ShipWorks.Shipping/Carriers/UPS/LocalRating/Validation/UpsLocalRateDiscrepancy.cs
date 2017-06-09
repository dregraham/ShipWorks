using System;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// An instance of a local rate not matching the shipment cost of a UPS shipment
    /// </summary>
    public class UpsLocalRateDiscrepancy
    {
        private readonly UpsServiceRate apiRate;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRateDiscrepancy"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="localRate">The local rate.</param>
        public UpsLocalRateDiscrepancy(ShipmentEntity shipment, IUpsLocalServiceRate localRate)
        {
            Shipment = shipment;
            LocalRate = localRate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRateDiscrepancy"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="localRate">The local rate.</param>
        /// <param name="apiRate">The API rate.</param>
        public UpsLocalRateDiscrepancy(ShipmentEntity shipment, IUpsLocalServiceRate localRate, UpsServiceRate apiRate)
        {
            this.apiRate = apiRate;
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
        public IUpsLocalServiceRate LocalRate { get; set; }
        
        /// <summary>
        /// Logs the discrepancies. Includes the shipmentID, the local rate, and the actual shipment cost
        /// </summary>
        public string GetLogMessage()
        {
            StringBuilder discrepancyInfo = new StringBuilder();
            discrepancyInfo.AppendLine($"Shipment ID: {Shipment.ShipmentID}");
            discrepancyInfo.AppendLine($"Local Rate: {LocalRate?.Amount.ToString("C") ?? "Not found"}");
            discrepancyInfo.AppendLine($"API Rate: {apiRate?.Amount.ToString("C") ?? "Not found"}");
            discrepancyInfo.AppendLine($"Label Cost: {Shipment.ShipmentCost:C}");
            
            LocalRate?.Log(discrepancyInfo);

            return discrepancyInfo.ToString();
        }

        /// <summary>
        /// Gets the message to display to the user about this discrepancy
        /// </summary>
        public string GetUserMessage()
        {
            StringBuilder discrepancyInfo = new StringBuilder();
            discrepancyInfo.AppendLine(
                $"There was a discrepancy between the local rate ({LocalRate?.Amount.ToString("C") ?? "not found"}) " +
                $"and the API rate ({apiRate?.Amount.ToString("C") ?? "not found"}) using {EnumHelper.GetDescription((UpsServiceType) Shipment.Ups.Service)}.");

            LocalRate?.Log(discrepancyInfo);

            return discrepancyInfo.ToString();
        }
    }
}