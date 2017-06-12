using System;
using System.Linq;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Custom class for common calculation properties.
    /// </summary>
    public partial class UpsPackageEntity
    {
        /// <summary>
        /// Gets the dimensions of the package, rounded to the nearest int
        /// </summary>
        private int[] Dimensions => new[]
        {
            (int) Math.Round(DimsLength, 0, MidpointRounding.AwayFromZero),
            (int) Math.Round(DimsHeight, 0, MidpointRounding.AwayFromZero),
            (int) Math.Round(DimsWidth, 0, MidpointRounding.AwayFromZero)
        };

        /// <summary>
        /// Gets the value of the longest side.
        /// </summary>
        public int LongestSide => Dimensions.Max();

        /// <summary>
        /// Girth of package
        /// </summary>
        /// <remarks>
        /// Side1 * 2 + Side2 * 2 where Side1 and Side2 are the two shortest
        /// sides.
        /// </remarks>
        public int Girth
        {
            get
            {
                // take 2 shortest side, multiply each by 2 and get the sum
                return Dimensions
                    .OrderBy(d => d)
                    .Take(2)
                    .Sum(d => d * 2);
            }
        }

        /// <summary>
        /// Get the second longest side from the dimensions
        /// </summary>
        public int SecondLongestSize
        {
            get { return Dimensions.OrderBy(d => d).ElementAt(1); }
        }

        /// <summary>
        /// Does UPS consider this a large package
        /// </summary>
        /// <remarks>
        /// True if (LongestSide + Girth) > 130
        /// </remarks>
        public bool IsLargePackage => (LongestSide + Girth) > 130;

        /// <summary>
        /// Gets the total weight.
        /// </summary>
        public double TotalWeight => Weight + (DimsAddWeight ? DimsWeight : 0);

        /// <summary>
        /// Gets the higher of TotalWeight and DimensionalWeight rounded up to nearest int
        /// </summary>
        /// <remarks>
        /// Large Packages are subject to a minimum billable weight of 90 pounds 
        /// in addition to the Large Package Surcharge.
        /// </remarks>
        public int BillableWeight
        {
            get
            {
                int area = Dimensions[0] * Dimensions[1] * Dimensions[2];

                double divisor = area <= 1728 ? 166D : 139D;
                int dimensionalWeight = (int) Math.Ceiling(area / divisor);
                
                if (IsLargePackage)
                {
                    dimensionalWeight = Math.Max(dimensionalWeight, 90);
                }

                int actualWeight = (int) Math.Ceiling(TotalWeight);

                return Math.Max(dimensionalWeight, actualWeight);
            }
        }
    }
}