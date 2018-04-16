using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response
{
    /// <summary>
    /// A data transport object containing the details about an order downloaded from Newegg.
    /// </summary>
    [Serializable]
    [XmlRoot("OrderInfo")]
    public class Order
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        public Order()
        {
            this.Packages = new List<Package>();
            this.Items = new List<Item>();
        }

        /// <summary>
        /// Gets or sets the seller ID.
        /// </summary>
        /// <value>
        /// The seller ID.
        /// </value>
        [XmlElement("SellerID")]
        public string SellerId { get; set; }

        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        /// <value>
        /// The order number.
        /// </value>
        [XmlElement("OrderNumber")]
        public long OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the invoice number based on the value sent from the API.
        /// </summary>
        /// <value>
        /// The invoice number.
        /// </value>
        [XmlElement(ElementName = "InvoiceNumber")]
        public string InvoiceNumberFromApi
        {
            get { return InvoiceNumber.ToString(CultureInfo.InvariantCulture); } 
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    InvoiceNumber = long.Parse(value);
                }
            }
        }

        [XmlIgnore]
        public long InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has previously been downloaded.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has previously been downloaded; otherwise, <c>false</c>.
        /// </value>
        [XmlElement("OrderDownloaded")]
        public bool HasPreviouslyBeenDownloaded { get; set; }

        /// <summary>
        /// Gets or sets the order date transport in pacific standard time. This
        /// should not be referenced in code as it is only a shim for transporting
        /// the OrderDateInPacificStandardTime property.
        /// </summary>
        /// <value>
        /// The order date transport in pacific standard time.
        /// </value>
        [XmlElement("OrderDate")]
        public string OrderDateTransportInPacificStandardTime
        {
            // This is a shim for the request date property. Newegg is sending the request date
            // in the format of mm/dd/yyyy hh:mm:ss, but this causes a serialization error
            // because the xml format for dates should be yyyy-mm-dd
            get
            {
                return OrderDateInPacificStandardTime == DateTime.MinValue ? string.Empty : XmlConvert.ToString(OrderDateInPacificStandardTime, XmlDateTimeSerializationMode.RoundtripKind);
            }
            set
            {
                // Newegg order times are in Pacific Standard Time, but we need to add the timezone information
                // to the date that is provided
                this.OrderDateInPacificStandardTime = string.IsNullOrEmpty(value) ? DateTime.MinValue : DateTime.Parse(value);
            }
        }

        /// <summary>
        /// Gets or sets the order date order date in pacific standard time.
        /// </summary>
        /// <value>
        /// The order date order date in pacific standard time.
        /// </value>
        [XmlIgnore]
        // This is ignored during xml serialization due to the OrderDateTransportInPacificStandardTime shim property
        public DateTime OrderDateInPacificStandardTime { get; set; }

        public DateTime OrderDateToUtcTime()
        {
            TimeZoneInfo pacificTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

            // Crash was occuring here due to invalid time. Invalid times are during the hour time period that doesn't
            // exist due to daylight savings "spring forward". The invalid time is coming directly from NewEgg.
            // If invalid time, add an hour before converting to UTC, then subtract the hour afterward to eliminate
            // the risk of orders being skipped because the time is in the future.
            return pacificTimeZone.IsInvalidTime(OrderDateInPacificStandardTime) ?
                TimeZoneInfo.ConvertTimeToUtc(OrderDateInPacificStandardTime.AddHours(1), pacificTimeZone).AddHours(-1) :
                TimeZoneInfo.ConvertTimeToUtc(OrderDateInPacificStandardTime, pacificTimeZone);
        }

        /// <summary>
        /// Gets or sets the order status.
        /// </summary>
        /// <value>
        /// The order status.
        /// </value>
        [XmlElement("OrderStatus")]
        [Browsable(false)] [EditorBrowsable(EditorBrowsableState.Never)]
        public int OrderStatusId
        {
            get
            {
                return (int)OrderStatus;
            }
            set
            {
                this.OrderStatus = (Enums.NeweggOrderStatus)value;
            }
        }

        [XmlIgnore]
        public Enums.NeweggOrderStatus OrderStatus { get; set; }

        /// <summary>
        /// Gets or sets the order status description.
        /// </summary>
        /// <value>
        /// The order status description.
        /// </value>
        [XmlElement("OrderStatusDescription")]
        public string OrderStatusDescription { get; set; }

        /// <summary>
        /// Gets or sets the name of the customer.
        /// </summary>
        /// <value>
        /// The name of the customer.
        /// </value>
        [XmlElement("CustomerName")]
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the customer phone number.
        /// </summary>
        /// <value>
        /// The customer phone number.
        /// </value>
        [XmlElement("CustomerPhoneNumber")]
        public string CustomerPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the customer phone number.
        /// </summary>
        /// <value>
        /// The customer phone number.
        /// </value>
        [XmlElement("CustomerEmailAddress")]
        public string CustomerEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the ship to address1.
        /// </summary>
        /// <value>
        /// The ship to address1.
        /// </value>
        [XmlElement("ShipToAddress1")]
        public string ShipToAddress1 { get; set; }

        /// <summary>
        /// Gets or sets the ship to address2.
        /// </summary>
        /// <value>
        /// The ship to address2.
        /// </value>
        [XmlElement("ShipToAddress2")]
        public string ShipToAddress2 { get; set; }

        /// <summary>
        /// Gets or sets the ship to city.
        /// </summary>
        /// <value>
        /// The ship to city.
        /// </value>
        [XmlElement("ShipToCityName")]
        public string ShipToCity { get; set; }

        /// <summary>
        /// Gets or sets the state of the ship to.
        /// </summary>
        /// <value>
        /// The state of the ship to.
        /// </value>
        [XmlElement("ShipToStateCode")]
        public string ShipToState { get; set; }

        /// <summary>
        /// Gets or sets the ship to zip code.
        /// </summary>
        /// <value>
        /// The ship to zip code.
        /// </value>
        [XmlElement("ShipToZipCode")]
        public string ShipToZipCode { get; set; }

        /// <summary>
        /// Gets or sets the ship to country code.
        /// </summary>
        /// <value>
        /// The ship to country code.
        /// </value>
        [XmlElement("ShipToCountryCode")]
        public string ShipToCountryCode { get; set; }

        /// <summary>
        /// Gets or sets the shipping service.
        /// </summary>
        /// <value>
        /// The shipping service.
        /// </value>
        [XmlElement("ShipService")]
        public string ShippingService { get; set; }

        /// <summary>
        /// Gets or sets the first name of the ship to.
        /// </summary>
        /// <value>
        /// The first name of the ship to.
        /// </value>
        [XmlElement("ShipToFirstName")]
        public string ShipToFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the ship to.
        /// </summary>
        /// <value>
        /// The last name of the ship to.
        /// </value>
        [XmlElement("ShipToLastName")]
        public string ShipToLastName { get; set; }

        /// <summary>
        /// Gets or sets the ship to company.
        /// </summary>
        /// <value>
        /// The ship to company.
        /// </value>
        [XmlElement("ShipToCompany")]
        public string ShipToCompany { get; set; }

        /// <summary>
        /// Gets or sets the order item amount.
        /// </summary>
        /// <value>
        /// The order item amount.
        /// </value>
        [XmlElement("OrderItemAmount")]
        public decimal OrderItemAmount { get; set; }

        /// <summary>
        /// Gets or sets the shipping amount.
        /// </summary>
        /// <value>
        /// The shipping amount.
        /// </value>
        [XmlElement("ShippingAmount")]
        public decimal ShippingAmount { get; set; }

        /// <summary>
        /// Gets or sets the discount amount.
        /// </summary>
        /// <value>
        /// The discount amount.
        /// </value>
        [XmlElement("DiscountAmount")]
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets the refund amount.
        /// </summary>
        /// <value>
        /// The refund amount.
        /// </value>
        [XmlElement("RefundAmount")]
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// Gets or sets the order total amount.
        /// </summary>
        /// <value>
        /// The order total amount.
        /// </value>
        [XmlElement("OrderTotalAmount")]
        public decimal OrderTotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the order quantity.
        /// </summary>
        /// <value>
        /// The order quantity.
        /// </value>
        [XmlElement("OrderQty")]
        public int OrderQuantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is auto void.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is auto void; otherwise, <c>false</c>.
        /// </value>
        [XmlElement("IsAutoVoid")]
        public bool IsAutoVoid { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [XmlArray("ItemInfoList")]
        [XmlArrayItem("ItemInfo")]
        public List<Item> Items { get; set; }

        /// <summary>
        /// Gets or sets the packages.
        /// </summary>
        /// <value>
        /// The packages.
        /// </value>
        [XmlArray("PackageInfoList")]
        [XmlArrayItem("PackageInfo")]
        public List<Package> Packages { get; set; }
    }
}
