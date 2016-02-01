using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "Item")]
    public class YahooCatalogItem
    {
        [XmlElement(ElementName = "ShipWeight")]
        public string ShipWeightTransport
        {
            // Deserialization error if ShipWeight node is empty. This resolves that.
            set
            {
                double result;
                double.TryParse(value, out result);
                ShipWeight = result;
            }
        }

        [XmlIgnore]
        public double ShipWeight { get; set; }
    }
}