using System;
using System.Xml;
using System.Xml.Serialization;
using ShipWorks.Stores.Platforms.Newegg.Enums;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response
{
    /// <summary>
    /// A data transport object containing details about a Newegg item.
    /// </summary>
    [Serializable]
    [XmlRoot("ItemInfo")]
    public class Item
    {
        /// <summary>
        /// Gets or sets the seller part number.
        /// </summary>
        /// <value>The seller part number.</value>
        [XmlElement("SellerPartNumber")]
        public string SellerPartNumber { get; set; }

        /// <summary>
        /// Gets or sets the newegg item number.
        /// </summary>
        /// <value>The newegg item number.</value>
        [XmlElement("NeweggItemNumber")]
        public string NeweggItemNumber { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer part number.
        /// </summary>
        /// <value>The manufacturer part number.</value>
        [XmlElement("MfrPartNumber")]
        public string ManufacturerPartNumber { get; set; }

        /// <summary>
        /// Gets or sets the UPC code.
        /// </summary>
        /// <value>The upc code.</value>
        [XmlElement("UPCCode")]
        public string UpcCode { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [XmlElement("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the quantity ordered.
        /// </summary>
        /// <value>The quantity ordered.</value>
        [XmlElement("OrderedQty")]
        public int QuantityOrdered { get; set; }

        /// <summary>
        /// Gets or sets the quantity shipped.
        /// </summary>
        /// <value>The quantity shipped.</value>
        [XmlElement("ShippedQty")]
        public int QuantityShipped { get; set; }

        /// <summary>
        /// Gets or sets the unit price.
        /// </summary>
        /// <value>The unit price.</value>
        [XmlElement("UnitPrice")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the shipping status ID.
        /// </summary>
        /// <value>The shipping status ID.</value>
        [XmlElement("Status")]
        public int ShippingStatusID
        {
            get { return (int)this.ShippingStatus; }
            set { this.ShippingStatus = (Enums.NeweggItemShippingStatus)value; }
        }

        /// <summary>
        /// Gets or sets the shipping status.
        /// </summary>
        /// <value>
        /// The shipping status.
        /// </value>
        [XmlIgnore]
        public NeweggItemShippingStatus ShippingStatus { get; set; }

        /// <summary>
        /// Gets or sets the shipping status description.
        /// </summary>
        /// <value>The shipping status description.</value>
        [XmlElement("StatusDescription")]
        public string ShippingStatusDescription { get; set; }
    }
}
