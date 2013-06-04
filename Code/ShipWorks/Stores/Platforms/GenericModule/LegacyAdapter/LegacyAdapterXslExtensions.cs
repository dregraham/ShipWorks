using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter
{
    /// <summary>
    /// Extension Object used during the legacy store module transformation process 
    /// </summary>
    public class LegacyAdapterXslExtensions
    {
        /// <summary>
        /// Parses an order number of the form 100-1 for "100"
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string GetOrderNumber(string orderNumberText)
        {
            if (orderNumberText == null)
            {
                return "";
            }

            if (orderNumberText.Contains("-"))
            {
                string[] parts = orderNumberText.Split('-');
                return parts[0];
            }
            else
            {
                return orderNumberText;
            }
        }

        /// <summary>
        /// Parses an order number of the form 100-1 for "-1"
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string GetOrderNumberPostfix(string orderNumberText)
        {
            if (orderNumberText == null)
            {
                return "";
            }

            if (orderNumberText.Contains("-"))
            {
                return orderNumberText.Substring(orderNumberText.IndexOf('-'));
            }
            else
            {
                // no postfix involved
                return "";
            }
        }

        /// <summary>
        /// Converts a dateTime to a date value allowed by Xml Schema
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ToUtcDateTime(string dateTime)
        {
            try
            {
                DateTime parsed = DateTime.Parse(dateTime);
                parsed.ToString();

                return parsed.ToString("s");
            }
            catch (FormatException)
            {
                return dateTime;
            }
        }
    }
}
