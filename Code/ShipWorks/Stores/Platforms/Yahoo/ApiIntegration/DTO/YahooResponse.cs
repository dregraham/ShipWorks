using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [XmlRoot(ElementName = "ystorewsResponse")]
    public class YahooResponse
    {
        [XmlElement(ElementName = "Version")]
        public string Version { get; set; }
        [XmlElement(ElementName = "RequestID")]
        public string RequestID { get; set; }
        [XmlElement(ElementName = "ResponseResourceList")]
        public YahooResponseResourceList ResponseResourceList { get; set; }
        [XmlAttribute(AttributeName = "ystorews", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Ystorews { get; set; }

        [XmlElement(ElementName = "ErrorResourceList")]
        public YahooErrorResourceList ErrorResourceList { get; set; }

    }
}
