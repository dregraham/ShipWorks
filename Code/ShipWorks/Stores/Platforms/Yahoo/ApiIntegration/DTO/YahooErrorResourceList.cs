using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    public class YahooError
    {
        [XmlRoot(ElementName = "Error")]
        public class Error
        {
            [XmlElement(ElementName = "Code")]
            public string Code { get; set; }
            [XmlElement(ElementName = "Message")]
            public string Message { get; set; }
        }

        [XmlRoot(ElementName = "ErrorResourceList")]
        public class ErrorResourceList
        {
            [XmlElement(ElementName = "Error")]
            public Error Error { get; set; }
        }
    }
}
