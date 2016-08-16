using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "ystorewsResponse")]
    public class YahooResponse
    {
        [XmlElement(ElementName = "ResponseResourceList")]
        public YahooResponseResourceList ResponseResourceList { get; set; }

        /// <summary>
        /// List of errors returned by an order query, same as error messages
        /// </summary>
        /// <value>
        /// The error resource list.
        /// </value>
        [XmlElement(ElementName = "ErrorResourceList")]
        public YahooErrorResourceList ErrorResourceList { get; set; }

        /// <summary>
        /// List of errors returned by a catalog query, same as error resource list
        /// </summary>
        /// <value>
        /// The error messages.
        /// </value>
        [XmlElement(ElementName = "ErrorMessages")]
        public YahooErrorResourceList ErrorMessages { get; set; }
    }
}
