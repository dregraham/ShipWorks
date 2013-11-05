using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public static class BestRateUtility
    {
        /// <summary>
        /// Gets the transit day values for binding to a DropDownList.
        /// </summary>
        public static List<KeyValuePair<int, string>> GetTransitDayValues()
        {
            var expectedDayValues = new Dictionary<int, string>();

            expectedDayValues.Add(0,"Any");

            for (int i = 1; i <= 21; i++)
            {
                expectedDayValues.Add(i, i.ToString());
            }

            return expectedDayValues.ToList();
        }
    }
}
