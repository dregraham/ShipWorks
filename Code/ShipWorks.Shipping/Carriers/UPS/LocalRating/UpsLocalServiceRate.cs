using System.Collections.Generic;
using System.Text;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using log4net;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Data structure for storing rating information for a UPS service with methods used when rating locally
    /// </summary>
    public class UpsLocalServiceRate : UpsServiceRate, IUpsLocalServiceRate
    {
        private readonly List<KeyValuePair<string, decimal>> addedSurcharges = new List<KeyValuePair<string, decimal>>();
        private readonly decimal initialAmount;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsLocalServiceRate(UpsServiceType service,
            decimal amount,
            bool negotiated,
            int? guaranteedDaysToDelivery) :
            base(service, amount, negotiated, guaranteedDaysToDelivery)
        {
            initialAmount = amount;
        }

        /// <summary>
        /// Adds the amount.
        /// </summary>
        public void AddAmount(decimal amount, string surchargeName)
        {
            addedSurcharges.Add(new KeyValuePair<string, decimal>(surchargeName, amount));
            Amount += amount;
        }

        /// <summary>
        /// Logs the specified log.
        /// </summary>
        public void Log(StringBuilder logEntry)
        {
            logEntry.AppendLine($"Rate Calculation for {Service}");
            logEntry.AppendLine($"Initial Value : {initialAmount:C}");
            addedSurcharges.ForEach(s=>logEntry.AppendLine($"\tAdded for '{s.Key}' : {s.Value:C}"));
            logEntry.AppendLine($"Total : {Amount:C}");
        }
    }
}
