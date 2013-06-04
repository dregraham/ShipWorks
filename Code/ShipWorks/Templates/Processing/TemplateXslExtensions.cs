using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml.XPath;
using Interapptive.Shared.Utility;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Class containing functions intended to be called from within XSL
    /// </summary>
    public class TemplateXslExtensions
    {        
        /// <summary>
        /// Takes an unformatted dateTime string and formats it as 'm/d/yyyy'
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ToShortDate(string dateTime)
        {
            // Parse automatically converts to Local
            DateTime date = DateTime.Parse(dateTime);
            return date.ToShortDateString();
        }

        /// <summary>
        /// Takes an unformatted dateTime string and formats it as 'h:mm tt'
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ToShortTime(string dateTime)
        {
            // Parse automatically converts to Local
            DateTime date = DateTime.Parse(dateTime);
            return date.ToShortTimeString();
        }

        /// <summary>
        /// Takes an unformatted dateTime string and formats it as 'm/d/yyyy h:mm tt'
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ToShortDateTime(string dateTime)
        {
            return ToShortDate(dateTime) + " " + ToShortTime(dateTime);
        }

        /// <summary>
        /// Format the dateTime using arbitrary format
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string FormatDateTime(string dateTime, string format)
        {
            return DateTime.Parse(dateTime).ToString(format);
        }

        /// <summary>
        /// Generates a key value for an OrderItem.  The XPathNavigator should be positioned
        /// such that the OrderItem is the current node.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string GetOrderItemKeyValue(XPathNodeIterator itemNode, bool optionSpecific)
        {
            if (!itemNode.MoveNext())
            {
                return "";
            }

            XPathNavigator xpath = itemNode.Current;

            // It starts out with the item code
            string key = XPathUtility.Evaluate(xpath, "Code", "");

            if (optionSpecific)
            {
                // Then add in all the option info
                XPathNodeIterator options = xpath.Select("Option");

                while (options.MoveNext())
                {
                    key += XPathUtility.Evaluate(options.Current, "Name", "") + XPathUtility.Evaluate(options.Current, "Description", "");
                }
            }

            return key;
        }
    }
}
