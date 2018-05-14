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
        /// Write start document which will automatically close
        /// </summary>
        public static string GetValue(this XElement element, string elementName, string defaultValue = "")
        {
            string result = element.Element(elementName)?.Value;
            return result.IsNullOrWhiteSpace() ? defaultValue : result.Trim();
        }
    }
}
