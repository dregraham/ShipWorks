using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "StatusCodes")]
    public class LemonStandStatusCodes
    {
        [XmlElement(ElementName = "StatusCode")]
        public List<LemonStandStatusCode> StatusCode { get; set; }
    }
}