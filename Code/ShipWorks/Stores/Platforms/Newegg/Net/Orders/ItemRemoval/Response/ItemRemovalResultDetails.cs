using System;
using System.Xml.Serialization;
using System.Xml;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval.Response
{
    /// <summary>
    /// A data transport object containing the details of of a ItemRemovalResult.
    /// The API's response starts to get a little odd here: there is an element 
    /// name "Orders" where the naming indicates there will be multiple orders returned, but 
    /// the API only allows you to remove items from one order at a time based on the documentation.
    /// </summary>
    [Serializable]
    [XmlRoot("ResponseBody")]
    public class ItemRemovalResultDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemRemovalResultDetails"/> class.
        /// </summary>
        public ItemRemovalResultDetails()
        {
            Order = new RemovalOrder();
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The orders.
        /// </value>        
        [XmlElement("Orders")] 
        public RemovalOrder Order { get; set; }


        /// <summary>
        /// Gets or sets the response date transport in pacific standard time. This
        /// should not be referenced in code as it is only a shim for transporting
        /// the OrderDateInPacificStandardTime property.
        /// </summary>
        /// <value>
        /// The response date transport in pacific standard time.
        /// </value>
        [XmlElement("RequestDate")]
        public string RequestDateTransportInPacificStandardTime
        {
            // This is a shim for the request date property. Newegg is sending the request date
            // in the format of mm/dd/yyyy hh:mm:ss, but this causes a serialization error
            // because the xml format for dates should be yyyy-mm-dd
            get
            {
                return RequestDateInPacificStandardTime == DateTime.MinValue ? string.Empty : XmlConvert.ToString(RequestDateInPacificStandardTime, XmlDateTimeSerializationMode.RoundtripKind);
            }
            set
            {
                // Newegg request times are in Pacific Standard Time, but we need to add the timezone information
                // to the date that is provided
                this.RequestDateInPacificStandardTime = string.IsNullOrEmpty(value) ? DateTime.MinValue : DateTime.Parse(value);
            }
        }

        /// <summary>
        /// Gets or sets the request date in PST.
        /// </summary>
        /// <value>
        /// The request date in PST.
        /// </value>
        [XmlIgnore]
        public DateTime RequestDateInPacificStandardTime { get; set; }
    }
}
