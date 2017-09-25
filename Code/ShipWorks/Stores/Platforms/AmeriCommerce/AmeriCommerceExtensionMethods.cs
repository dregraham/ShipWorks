using System;
using ShipWorks.Stores.Platforms.AmeriCommerce.WebServices;


namespace ShipWorks.Stores.Platforms.AmeriCommerce
{
    /// <summary>
    /// Extension methods for dealing with AmeriCommerce's data type wrappers.
    /// </summary>
    public static class AmeriCommerceExtensionMethods
    {
        /// <summary>
        /// Reads the value of an AmeriCommerce DataInt32 data type
        /// </summary>
        public static int GetValue(this DataInt32 amcInt32, int defaultValue)
        {
            if (amcInt32 == null || amcInt32.IsNull)
            {
                return defaultValue;
            }
            else
            {
                return amcInt32.Value;
            }
        }

        /// <summary>
        /// Reads the value of an AmeriCommerce DataMoney data type
        /// </summary>
        public static decimal GetValue(this DataMoney amcMoney, decimal defaultValue)
        {
            if (amcMoney == null || amcMoney.IsNull)
            {
                return defaultValue;
            }
            else
            {
                return amcMoney.Value;
            }
        }

        /// <summary>
        /// Reads the value of an AmeriCommerce DataDateTime data type
        /// </summary>
        public static DateTime GetValue(this DataDateTime amcDate, DateTime defaultValue)
        {
            if (amcDate == null || amcDate.IsNull)
            {
                return defaultValue;
            }
            else
            {
                return amcDate.Value.ToUniversalTime();
            }
        }
    }
}
