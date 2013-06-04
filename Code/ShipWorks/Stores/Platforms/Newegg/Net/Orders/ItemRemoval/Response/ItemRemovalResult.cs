using System;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval.Response
{
    /// <summary>
    /// The top level data transport object containing the results of removing an order item via the Newegg API
    /// </summary>
    [Serializable]
    [XmlRoot("NeweggAPIResponse")]
    public class ItemRemovalResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemRemovalResult"/> class.
        /// </summary>
        public ItemRemovalResult()
        {
            Details = new ItemRemovalResultDetails();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is successful.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is successful; otherwise, <c>false</c>.
        /// </value>
        [XmlElement("IsSuccess")]
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the memo.
        /// </summary>
        /// <value>
        /// The memo.
        /// </value>
        [XmlElement("Memo")]
        public string Memo { get; set; }

        /// <summary>
        /// Gets or sets the type of the operation.
        /// </summary>
        /// <value>
        /// The type of the operation.
        /// </value>
        [XmlElement("OperationType")]
        public string OperationType { get; set; }

        /// <summary>
        /// Gets or sets the seller ID.
        /// </summary>
        /// <value>
        /// The seller ID.
        /// </value>
        [XmlElement("SellerID")]
        public string SellerId { get; set; }


        /// <summary>
        /// Gets or sets the response date transport in pacific standard time. This
        /// should not be referenced in code as it is only a shim for transporting
        /// the OrderDateInPacificStandardTime property.
        /// </summary>
        /// <value>
        /// The response date transport in pacific standard time.
        /// </value>
        [XmlElement("ResponseDate")]
        public string ResponseDateTransportInPacificStandardTime
        {
            // This is a shim for the response date property. Newegg is sending the request date
            // in the format of mm/dd/yyyy hh:mm:ss, but this causes a serialization error
            // because the xml format for dates should be yyyy-mm-dd
            get
            {
                return ResponseDateInPacificStandardTime == DateTime.MinValue ? string.Empty : XmlConvert.ToString(ResponseDateInPacificStandardTime, XmlDateTimeSerializationMode.RoundtripKind);
            }
            set
            {
                // Newegg response times are in Pacific Standard Time, but we need to add the timezone information
                // to the date that is provided
                this.ResponseDateInPacificStandardTime = string.IsNullOrEmpty(value) ? DateTime.MinValue : DateTime.Parse(value);
            }
        }


        /// <summary>
        /// Gets or sets the response date in pacific standard time.
        /// </summary>
        /// <value>
        /// The response date in pacific standard time.
        /// </value>
        [XmlIgnore]
        public DateTime ResponseDateInPacificStandardTime { get; set; }

        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        /// <value>
        /// The details.
        /// </value>
        [XmlElement("ResponseBody")] 
        public ItemRemovalResultDetails Details { get; set; }

    }
}
