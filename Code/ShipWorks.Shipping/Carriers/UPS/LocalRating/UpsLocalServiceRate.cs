using System.Collections.Generic;
using System.Text;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Data structure for storing rating information for a UPS service with methods used when rating locally
    /// </summary>
    /// <remarks>
    /// The constructor is the rate of a single package. Add package adds additional packages.
    /// </remarks>
    public class UpsLocalServiceRate : UpsServiceRate, IUpsLocalServiceRate
    {
        private readonly List<KeyValuePair<string, decimal>> addedSurcharges = new List<KeyValuePair<string, decimal>>();
        private readonly string zone;
        private readonly List<decimal> packageRates; 
        private readonly List<string> packageBillableWeights;
        private int packageCount;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsLocalServiceRate(UpsServiceType service, string zone, decimal amount, string billableWeight) :
            base(service, amount, false, null)
        {
            this.zone = zone;

            packageRates = new List<decimal> {amount};
            packageBillableWeights = new List<string> {billableWeight};
            packageCount = 1;
        }

        /// <summary>
        /// Adds the package.
        /// </summary>
        public void AddAmount(UpsLocalServiceRate rateToAdd)
        {
            Amount += rateToAdd.Amount;
            packageRates.AddRange(rateToAdd.packageRates);
            packageBillableWeights.AddRange(rateToAdd.packageBillableWeights);
            packageCount++;
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
            logEntry.AppendLine($"Zone {zone}");
            logEntry.AppendLine($"Number of Packages: {packageCount}");
            for (int pacakgeIndex = 0; pacakgeIndex < packageCount; pacakgeIndex++)
            {
                logEntry.AppendLine($"\tPackage {pacakgeIndex + 1}:");
                logEntry.AppendLine($"\t\tBillable Weight : {packageBillableWeights[pacakgeIndex]} Lbs");
                logEntry.AppendLine($"\t\tService Rate : {packageRates[pacakgeIndex]:C}");
            }
            addedSurcharges.ForEach(s => logEntry.AppendLine($"\t{s.Key} : {s.Value:C}"));
            logEntry.AppendLine($"Total : {Amount:C}");
        }
    }
}
