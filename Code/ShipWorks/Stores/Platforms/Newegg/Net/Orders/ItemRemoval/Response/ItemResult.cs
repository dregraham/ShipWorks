using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval.Response
{
    /// <summary>
    /// Another slight oddity of the API here - there is a "Result" node
    /// in the documentation that only contains a list of items which
    /// gives the appearance that the "Result" node is unneccessary
    /// </summary>
    [Serializable]
    [XmlRoot("Result")]
    public class ItemResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemResult"/> class.
        /// </summary>
        public ItemResult()
        {
            ItemsRemoved = new List<RemovedItem>();
        }

        /// <summary>
        /// Gets or sets the items removed.
        /// </summary>
        /// <value>
        /// The items removed.
        /// </value>
        [XmlArray("ItemList")]
        [XmlArrayItem("Item")]
        public List<RemovedItem> ItemsRemoved { get; set; }
    }
}
