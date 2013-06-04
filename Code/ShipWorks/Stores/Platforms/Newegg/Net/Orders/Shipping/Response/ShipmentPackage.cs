using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response
{
    /// <summary>
    /// A data transport object in the ShippingResult tree containing detailed 
    /// information about each package uploaded to Newegg.
    /// </summary>
    [Serializable]
    [XmlRoot("Package")]
    public class ShipmentPackage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentPackage"/> class.
        /// </summary>
        public ShipmentPackage()
        {
            Items = new List<ShippedItem>();
        }


        /// <summary>
        /// Gets or sets the tracking number.
        /// </summary>
        /// <value>
        /// The tracking number.
        /// </value>
        [XmlElement("TrackingNumber")]
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is successfully processed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is successfully processed; otherwise, <c>false</c>.
        /// </value>
        [XmlElement("ProcessStatus")]
        public bool IsSuccessfullyProcessed { get; set; }

        /// <summary>
        /// Gets or sets the processing description.
        /// </summary>
        /// <value>
        /// The processing description.
        /// </value>
        [XmlElement("ProcessResult")]
        public string ProcessingDescription { get; set; }


        /// <summary>
        /// Gets or sets the ship date transport in pacific standard time. This
        /// should not be referenced in code as it is only a shim for transporting
        /// the ShipDateInPacificStandardTime property.
        /// </summary>
        /// <value>The ship date transport in pacific standard time.</value>
        [XmlElement("ShipDate")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string ShipDateTransportInPacificStandardTime
        {
            // This is a shim for the request date property. Newegg is sending the request date
            // in the format of mm/dd/yyyy hh:mm:ss, but this causes a serialization error
            // because the xml format for dates should be yyyy-mm-dd
            get
            {
                return ShipDateInPacificStandardTime == DateTime.MinValue ? string.Empty : XmlConvert.ToString(ShipDateInPacificStandardTime, XmlDateTimeSerializationMode.RoundtripKind);
            }
            set
            {
                this.ShipDateInPacificStandardTime = string.IsNullOrEmpty(value) ? DateTime.MinValue : DateTime.Parse(value);
            }
        }


        /// <summary>
        /// Gets or sets the ship date in pacific standard time.
        /// </summary>
        /// <value>
        /// The ship date in pacific standard time.
        /// </value>
        [XmlIgnore]
        // This is ignored during xml serialization due to the RequestDateTransport shim property
        public DateTime ShipDateInPacificStandardTime { get; set; }
                
        /// <summary>
        /// Gets or sets the ship from address1.
        /// </summary>
        /// <value>
        /// The ship from address1.
        /// </value>
        [XmlElement("ShipFromAddress")]
        public string ShipFromAddress1 { get; set; }

        /// <summary>
        /// Gets or sets the ship from address2.
        /// </summary>
        /// <value>
        /// The ship from address2.
        /// </value>
        [XmlElement("ShipFromAddress2")]
        public string ShipFromAddress2 { get; set; }

        /// <summary>
        /// Gets or sets the ship from city.
        /// </summary>
        /// <value>
        /// The ship from city.
        /// </value>
        [XmlElement("ShipFromCity")]
        public string ShipFromCity { get; set; }

        /// <summary>
        /// Gets or sets the state of the ship from.
        /// </summary>
        /// <value>
        /// The state of the ship from.
        /// </value>
        [XmlElement("ShipFromState")]
        public string ShipFromState { get; set; }

        /// <summary>
        /// Gets or sets the ship from zip code.
        /// </summary>
        /// <value>
        /// The ship from zip code.
        /// </value>
        [XmlElement("ShipFromZipcode")]
        public string ShipFromZipCode { get; set; }

        /// <summary>
        /// Gets or sets the ship from country.
        /// </summary>
        /// <value>
        /// The ship from country.
        /// </value>
        [XmlElement("ShipFromCountry")]
        public string ShipFromCountry { get; set; }

        /// <summary>
        /// Gets or sets the ship carrier.
        /// </summary>
        /// <value>
        /// The ship carrier.
        /// </value>
        [XmlElement("ShipCarrier")]
        public string ShipCarrier { get; set; }

        /// <summary>
        /// Gets or sets the ship service.
        /// </summary>
        /// <value>
        /// The ship service.
        /// </value>
        [XmlElement("ShipService")]
        public string ShipService { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [XmlArray("ItemList")]
        [XmlArrayItem("ItemDes")]        
        public List<ShippedItem> Items { get; set; }
    }
}
