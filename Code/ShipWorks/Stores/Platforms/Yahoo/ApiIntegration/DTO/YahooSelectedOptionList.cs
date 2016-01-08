using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "SelectedOptionList")]
    public class YahooSelectedOptionList
    {
        [XmlElement(ElementName = "Option")]
        public List<YahooOption> Option { get; set; }
    }
}