using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    [XmlRoot(ElementName = "StatusCodes")]
    public class LemonStandStatusCodes
    {
        [XmlElement(ElementName = "StatusCode")]
        public List<LemonStandStatusCode> StatusCode { get; set; }
    }
}
