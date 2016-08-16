using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShipWorks.Shipping.Carriers.Amazon.Api.DTOs
{

    [XmlRoot(ElementName = "ErrorResponse", Namespace = "http://mws.amazonservices.com/MerchantFulfillment/2015-06-01")]
    public class ErrorResponse
    {
        [XmlElement(ElementName = "Error")]
        public Error Error { get; set; }

        [XmlElement(ElementName = "RequestID")]
        public string RequestID { get; set; }
    }

    [XmlRoot(ElementName = "Error")]
    public class Error
    {
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }

        [XmlElement(ElementName = "Code")]
        public string Code { get; set; }

        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }
    }
}
