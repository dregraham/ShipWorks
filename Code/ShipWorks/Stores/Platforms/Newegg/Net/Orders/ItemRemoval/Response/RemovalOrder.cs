using System;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval.Response
{
    /// <summary>
    /// A data transport object in the ItemRemovalResult tree.
    /// </summary>
    [Serializable]
    [XmlRoot("Orders")]
    public class RemovalOrder
    {
        public RemovalOrder()
        {
            ItemResult = new ItemResult();
        }

        [XmlElement("OrderNumber")]
        public long OrderNumber { get; set; }

        [XmlElement("Result")]
        public ItemResult ItemResult { get; set; }
    }
}
