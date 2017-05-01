using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Filtker services based on package dimensions
    /// </summary>
    public class DimensionServiceFilter : IServiceFilter
    {
        /// <summary>
        /// Get eligible services based on the dimensions
        /// </summary>
        public IEnumerable<UpsServiceType> GetEligibleServices(UpsShipmentEntity shipment, IEnumerable<UpsServiceType> services)
        {
            if (shipment.Packages.Any(package => GetPackageLength(package) >= 108 ||
                                                 GetPackageLength(package) + GetPackageGirth(package) >= 165))
            {
                return Enumerable.Empty<UpsServiceType>();
            }

            return services;
        }

        /// <summary>
        /// Get the longest side of a package
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        private static double GetPackageLength(UpsPackageEntity package)
        {
            return Math.Max(Math.Max(package.DimsLength, package.DimsHeight), package.DimsWidth);
        }

        /// <summary>
        /// Get the package girth 
        /// </summary>
        private static double GetPackageGirth(UpsPackageEntity package)
        {
            return new List<double> {package.DimsLength, package.DimsHeight, package.DimsWidth}.OrderBy(d => d)
                .Take(2)
                .Aggregate<double, double>(0, (c, d) => c + d * 2);
        }
    }
}