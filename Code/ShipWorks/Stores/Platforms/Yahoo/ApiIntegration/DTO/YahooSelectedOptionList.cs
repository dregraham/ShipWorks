using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [XmlRoot(ElementName = "SelectedOptionList")]
    public class YahooSelectedOptionList
    {
        [XmlElement(ElementName = "Option")]
        public List<YahooOption> Option { get; set; }
    }
}