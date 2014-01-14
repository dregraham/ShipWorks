using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// In implementation of the IComparer interface for comparing BrokerExceptionSeverityLevel values. When used
    /// for sorting, the values will be sorted from highest severity level to lowest severity level.
    /// </summary>
    public class BrokerExceptionSeverityLevelComparer : IComparer<BrokerExceptionSeverityLevel>
    {
        private readonly int errorLevelValue;
        private readonly int warningLevelValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrokerExceptionSeverityLevelComparer"/> class.
        /// </summary>
        public BrokerExceptionSeverityLevelComparer()
        {
            errorLevelValue = 0;
            warningLevelValue = 1;
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare(BrokerExceptionSeverityLevel x, BrokerExceptionSeverityLevel y)
        {
            // Sort values from highest severity level to lowest severity level

            int xValueForComparison = GetNumericValueForComparison(x);
            int yValueForComparison = GetNumericValueForComparison(y);

            if (xValueForComparison == yValueForComparison)
            {
                return 0;
            }

            if (xValueForComparison > yValueForComparison)
            {
                return 1;
            }

            return -1;
        }

        /// <summary>
        /// Gets the numeric value for comparison.
        /// </summary>
        /// <param name="severityLevel">The severity level.</param>
        /// <returns>An integer value used for comparison purposes.</returns>
        private int GetNumericValueForComparison(BrokerExceptionSeverityLevel severityLevel)
        {
            return severityLevel == BrokerExceptionSeverityLevel.High ? errorLevelValue : warningLevelValue;
        }
    }
}
