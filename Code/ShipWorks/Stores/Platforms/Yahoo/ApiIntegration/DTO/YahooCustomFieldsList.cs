using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "CustomFieldsList")]
    public class YahooCustomFieldsList
    {
        [XmlElement(ElementName = "CustomField")]
        public List<YahooCustomField> CustomField { get; set; }
    }
}
