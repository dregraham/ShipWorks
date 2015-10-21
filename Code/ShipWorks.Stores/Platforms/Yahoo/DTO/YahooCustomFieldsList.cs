using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.DTO
{
    [XmlRoot(ElementName = "CustomFieldsList")]
    public class YahooCustomFieldsList
    {
        [XmlElement(ElementName = "CustomField")]
        public List<YahooCustomField> CustomField { get; set; }
    }
}
