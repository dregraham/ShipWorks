using System.Xml.Linq;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    public class ThreeDCartOptionNode
    {
        public XElement OptionTypeNode { get; set; }

        public XElement OptionValueNode { get; set; }
    }
}