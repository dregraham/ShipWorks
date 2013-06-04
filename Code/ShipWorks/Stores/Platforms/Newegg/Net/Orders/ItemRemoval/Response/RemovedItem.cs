using System;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval.Response
{
    /// <summary>
    /// A data transport object within the ItemRemovalResult tree.
    /// </summary>
    [Serializable]
    [XmlRoot("Item")]
    public class RemovedItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemovedItem"/> class.
        /// </summary>
        public RemovedItem()
        { }

        /// <summary>
        /// Gets or sets the seller part number.
        /// </summary>
        /// <value>
        /// The seller part number.
        /// </value>
        [XmlElement("SellerPartNumber")]
        public string SellerPartNumber { get; set; }
    }
}
