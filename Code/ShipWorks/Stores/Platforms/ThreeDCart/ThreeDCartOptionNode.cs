using System.Xml.Linq;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// DTO for 3dcart production option nodes
    /// </summary>
    public class ThreeDCartOptionNode
    {
        /// <summary>
        /// Gets or sets the option type node.
        /// </summary>
        public XElement OptionTypeNode { get; set; }

        /// <summary>
        /// Gets or sets the option value node.
        /// </summary>
        public XElement OptionValueNode { get; set; }
    }
}