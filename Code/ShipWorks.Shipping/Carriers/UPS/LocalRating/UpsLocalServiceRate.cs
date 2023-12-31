﻿using System.Collections.Generic;
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

        
        private int packageCount;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsLocalServiceRate(UpsServiceType service, string zone, decimal amount, string billableWeight) :
            base(service, amount, false, null)
        {
            Zone = zone;

            PackageRates = new List<decimal> {amount};
            PackageBillableWeights = new List<string> {billableWeight};
            packageCount = 1;
        }

        public List<string> PackageBillableWeights { get; }

        public List<decimal> PackageRates { get; }

        /// <summary>
        /// Zone used to calculate this rate
        /// </summary>
        public string Zone { get; }

        /// <summary>
        /// Adds the package.
        /// </summary>
        public void AddAmount(IUpsLocalServiceRate rateToAdd)
        {
            Amount += rateToAdd.Amount;
            PackageRates.AddRange(rateToAdd.PackageRates);
            PackageBillableWeights.AddRange(rateToAdd.PackageBillableWeights);
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
            logEntry.AppendLine($"Zone {Zone}");
            logEntry.AppendLine($"Number of Packages: {packageCount}");
            for (int pacakgeIndex = 0; pacakgeIndex < packageCount; pacakgeIndex++)
            {
                logEntry.AppendLine($"\tPackage {pacakgeIndex + 1}:");
                logEntry.AppendLine($"\t\tBillable Weight : {PackageBillableWeights[pacakgeIndex]} Lbs");
                logEntry.AppendLine($"\t\tService Rate : {PackageRates[pacakgeIndex]:C}");
            }
            addedSurcharges.ForEach(s => logEntry.AppendLine($"\t{s.Key} : {s.Value:C}"));
            logEntry.AppendLine($"Total : {Amount:C}");
        }
    }
}
