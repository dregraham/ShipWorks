using System.Collections.Generic;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// An implementation of the IComparer interface that compares ServiceLevelType values
    /// based on the speed/timeliness in which a shipment will be delivered (i.e. One day
    /// will be ordered before second day). This will cause the Anytime value to be ordered
    /// last.
    /// </summary>
    public class ServiceLevelSpeedComparer : IComparer<ServiceLevelType>
    {
        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, 
        /// as shown in the following table. Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare(ServiceLevelType x, ServiceLevelType y)
        {
            int xValue = GetNumericValueForComparison(x);
            int yValue = GetNumericValueForComparison(y);

            if (xValue == yValue)
            {
                // The delivery speed of X and Y are the same
                return 0;
            }
            
            if (xValue > yValue)
            {
                // X is slower than Y in terms of delivery speed
                return 1;
            }

            // X is faster than Y in terms of delivery speed
            return -1;

        }

        /// <summary>
        /// Gets the numeric value of the ServiceLevelType to use for comparison. The Anytime value will
        /// be regarded as the highest available value since it is the slowest.
        /// </summary>
        /// <param name="serviceLevel">The service level.</param>
        /// <returns></returns>
        private int GetNumericValueForComparison(ServiceLevelType serviceLevel)
        {
            // We want to force the Anytime value to appear last since it is the
            // slowest delivery time
            int numericValue = serviceLevel == ServiceLevelType.Anytime ? 999 : (int)serviceLevel;
            return numericValue;
        }
}
}
