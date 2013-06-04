using System;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation.Response
{
    /// <summary>
    /// A data transport object containing the details about a cancellation of a Newegg order via the API.
    /// </summary>
    [Serializable]
    [XmlRoot("Result")]
    public class CancellationResultDetail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationResultDetail"/> class.
        /// </summary>
        public CancellationResultDetail()
        { }

        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        /// <value>
        /// The order number.
        /// </value>
        [XmlElement("OrderNumber")]
        public long OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the seller id.
        /// </summary>
        /// <value>
        /// The seller id.
        /// </value>
        [XmlElement("SellerID")]
        public string SellerId { get; set; }

        /// <summary>
        /// Gets or sets the order status.
        /// </summary>
        /// <value>
        /// The order status.
        /// </value>
        [XmlElement("OrderStatus")]
        public string OrderStatus { get; set; }
    }
}
