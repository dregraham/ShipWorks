using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    [XmlRoot(ElementName = "StatusCode")]
    public class LemonStandStatusCode
    {
        [XmlElement(ElementName = "Code")]
        public string Code { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
    }
}
