using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "Item")]
    public class YahooItem
    {
        [XmlElement(ElementName = "LineNumber")]
        public string LineNumber { get; set; }

        [XmlElement(ElementName = "ItemID")]
        public string ItemID { get; set; }

        [XmlElement(ElementName = "ItemCode")]
        public string ItemCode { get; set; }

        [XmlElement(ElementName = "Quantity")]
        public double Quantity { get; set; }

        [XmlElement(ElementName = "UnitPrice")]
        public decimal UnitPrice { get; set; }

        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "URL")]
        public string URL { get; set; }

        [XmlElement(ElementName = "Taxable")]
        public string Taxable { get; set; }

        [XmlElement(ElementName = "ThumbnailURL")]
        public string ThumbnailUrl { get; set; }

        [XmlElement(ElementName = "SelectedOptionList")]
        public YahooSelectedOptionList SelectedOptionList { get; set; }
    }
}
