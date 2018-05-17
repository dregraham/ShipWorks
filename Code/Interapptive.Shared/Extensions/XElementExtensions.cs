using System;
using System.Xml.Linq;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Extensions for XElementExtensions
    /// </summary>
    public static class XElementExtensions
    {
        /// <summary>
        /// Get a string value from an XElement
        /// </summary>
        public static string GetValue(this XElement element, string elementName, string defaultValue = "")
        {
            string result = element.Element(elementName)?.Value;
            return result.IsNullOrWhiteSpace() ? defaultValue : result.Trim();
        }

        /// <summary>
        /// Get a decimal value from an XElement
        /// </summary>
        public static decimal GetDecimal(this XElement element, string elementName, decimal defaultValue = 0) =>
            decimal.TryParse(GetValue(element, elementName), out decimal parsedValue) ? parsedValue : defaultValue;

        /// <summary>
        /// Get a double value from an XElement
        /// </summary>
        public static double GetDouble(this XElement element, string elementName, double defaultValue = 0) =>
            double.TryParse(GetValue(element, elementName), out double parsedValue) ? parsedValue : defaultValue;

        /// <summary>
        /// Get a DateTime value from an XElement
        /// </summary>
        public static DateTime GetDate(this XElement element, string elementName, DateTime defaultValue) =>
            DateTime.TryParse(GetValue(element, elementName), out DateTime parsedValue) ? parsedValue : defaultValue;

        /// <summary>
        /// Get a long value from an XElement
        /// </summary>
        public static long GetLong(this XElement element, string elementName, long defaultValue = 0) =>
            long.TryParse(GetValue(element, elementName), out long parsedValue) ? parsedValue : defaultValue;
    }
}
