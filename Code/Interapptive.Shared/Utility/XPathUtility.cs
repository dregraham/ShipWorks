using System;
using System.Xml.XPath;
using log4net;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// XPath utility functions for simplified xpath evaluation
    /// </summary>
    public static class XPathUtility
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(XPathUtility));

        /// <summary>
        /// Evaluates the given XPath expression and returns the result.
        /// </summary>
        public static string Evaluate(XPathNavigator xpath, string node, string defaultValue)
        {
            string value = GetNodeValue(xpath, node);

            // If its empty, it could be that its an empty tag, or it could be the tag does not
            // exist.  For the latter case, we need to use the default value.
            if (value.Length == 0)
            {
                if (xpath.Select(node).Count == 0)
                {
                    return defaultValue;
                }
            }

            return value;
        }

        /// <summary>
        /// Evaluates the given XPath expression and returns the result.
        /// </summary>
        public static int Evaluate(XPathNavigator xpath, string node, int defaultValue)
        {
            string value = GetNodeValue(xpath, node);

            if (value.Length == 0)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt32(value);
            }
            catch (FormatException ex)
            {
                string message = string.Format("Failed to convert '{0}' from path '{1}' to Int32.", value, node);
                log.ErrorFormat(message, ex);

                throw;
            }
        }

        /// <summary>
        /// Evaluates the given XPath expression and returns the result.
        /// </summary>
        public static long Evaluate(XPathNavigator xpath, string node, long defaultValue)
        {
            string value = GetNodeValue(xpath, node);

            if (value.Length == 0)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt64(value);
            }
            catch (FormatException ex)
            {
                string message = string.Format("Failed to convert '{0}' from path '{1}' to Int32.", value, node);
                log.ErrorFormat(message, ex);

                throw;
            }
        }

        /// <summary>
        /// Evaluates the given XPath expression and returns the result.
        /// </summary>
        public static decimal Evaluate(XPathNavigator xpath, string node, decimal defaultValue)
        {
            string value = GetNodeValue(xpath, node);

            if (value.Length == 0)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToDecimal(value);
            }
            catch (FormatException ex)
            {
                string message = string.Format("Failed to convert '{0}' from path '{1}' to Decimal.", value, node);
                log.ErrorFormat(message, ex);

                throw;
            }
        }

        /// <summary>
        /// Evaluates the given XPath expression and returns the result.
        /// </summary>
        public static double Evaluate(XPathNavigator xpath, string node, double defaultValue)
        {
            string value = GetNodeValue(xpath, node);

            if (value.Length == 0)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToDouble(value);
            }
            catch (FormatException ex)
            {
                string message = string.Format("Failed to convert '{0}' from path '{1}' to Double.", value, node);
                log.ErrorFormat(message, ex);

                throw;
            }
        }

        /// <summary>
        /// Evaluates the given XPath expression and returns the result.
        /// </summary>
        public static bool Evaluate(XPathNavigator xpath, string node, bool defaultValue)
        {
            string value = GetNodeValue(xpath, node);

            if (value.Length == 0)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToBoolean(value);
            }
            catch (FormatException ex)
            {
                string message = string.Format("Failed to convert '{0}' from path '{1}' to Boolean.", value, node);
                log.ErrorFormat(message, ex);

                throw;
            }
        }

        /// <summary>
        /// Parses an query result for true/false or 0/1 since Xml Schemas allow either for the xsd:boolean datatype
        /// </summary>
        public static bool EvaluateXsdBoolean(XPathNavigator xpath, string node, bool defaultValue)
        {
            string nodeValue = XPathUtility.Evaluate(xpath, node, "");
            bool realValueBool;

            if (bool.TryParse(nodeValue, out realValueBool))
            {
                return realValueBool;
            }

            // try parsing as an integer
            int realValueInt;
            if (int.TryParse(nodeValue, out realValueInt))
            {
                if (realValueInt == 0)
                {
                    return false;
                }
                else if (realValueInt == 1)
                {
                    return true;
                }
            }

            // value not found, use the default specified
            return defaultValue;
        }

        /// <summary>
        /// Get the value of the given node path for the given XPathNavigator
        /// </summary>
        private static string GetNodeValue(XPathNavigator xpath, string node)
        {
            if (xpath == null)
            {
                throw new ArgumentNullException("xpath");
            }

            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            return (string) xpath.Evaluate("string(" + node + ")");
        }
    }
}
