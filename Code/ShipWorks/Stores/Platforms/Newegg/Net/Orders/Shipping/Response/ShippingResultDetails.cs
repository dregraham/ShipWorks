using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response
{
    /// <summary>
    /// A data transport object within the ShippingResult tree.
    /// </summary>
    [Serializable]
    [XmlRoot("Result")]
    public class ShippingResultDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingResultDetails"/> class.
        /// </summary>
        public ShippingResultDetails()
        {
            this.Shipment = new Shipment();
        }

        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        /// <value>
        /// The order number.
        /// </value>
        [XmlElement("OrderNumber")]
        public long OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the order status.
        /// </summary>
        /// <value>
        /// The order status.
        /// </value>
        [XmlElement("OrderStatus")]
        public string OrderStatus { get; set; }

        /// <summary>
        /// Gets or sets the seller ID.
        /// </summary>
        /// <value>
        /// The seller ID.
        /// </value>
        [XmlElement("SellerID")]
        public string SellerId { get; set; }

        /// <summary>
        /// Gets or sets the shipment.
        /// </summary>
        /// <value>
        /// The shipment.
        /// </value>
        [XmlElement("Shipment")]
        public Shipment Shipment { get; set; }
    }
}
