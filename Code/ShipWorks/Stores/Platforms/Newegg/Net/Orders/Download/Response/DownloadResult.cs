using System;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download.Response
{
    /// <summary>
    /// The top level data transport object for downloading orders from Newegg.
    /// </summary>
    [Serializable]
    [XmlRoot("NeweggAPIResponse")]
    public class DownloadResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadResult"/> class.
        /// </summary>
        public DownloadResult()
        {
            this.Body = new ResponseBody();
        }

        /// <summary>
        /// Gets or sets the seller ID.
        /// </summary>
        /// <value>
        /// The seller IDm.
        /// </value>
        [XmlElement("SellerID")]
        public string SellerId { get; set; }

        /// <summary>
        /// Gets or sets the type of the operation.
        /// </summary>
        /// <value>
        /// The type of the operation.
        /// </value>
        [XmlElement("OperationType")]
        public string OperationType { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        [XmlElement("ResponseBody")]
        public ResponseBody Body { get; set; }

        /// <summary>
        /// Gets or sets the memo.
        /// </summary>
        /// <value>
        /// The memo.
        /// </value>
        [XmlElement("Memo")]
        public string Memo { get; set; }

        /// <summary>
        /// Gets or sets the response date in pacific standard time.
        /// </summary>
        /// <value>
        /// The response date in pacific standard time.
        /// </value>
        [XmlIgnore]
        // This is ignored during xml serialization due to the OrderDateTransportInPacificStandardTime shim property
        public DateTime ResponseDateInPacificStandardTime { get; set; }

        /// <summary>
        /// Converts the responses date to UTC time.
        /// </summary>
        /// <returns></returns>
        public DateTime ResponseDateToUtcTime()
        {
            TimeZoneInfo pacificStandardTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            return System.TimeZoneInfo.ConvertTimeToUtc(this.ResponseDateInPacificStandardTime, pacificStandardTimeZone);
        }

        /// <summary>
        /// Gets or sets the response date transport in pacific standard time. This
        /// should not be referenced in code as it is only a shim for transporting
        /// the ResponseDateInPacificStandardTime property.
        /// </summary>
        /// <value>
        /// The order date transport in pacific standard time.
        /// </value>
        [XmlElement("ResponseDate")]
        public string ResponseDateTransportInPacificStandardTime
        {
            // This is a shim for the request date property. Newegg is sending the request date
            // in the format of mm/dd/yyyy hh:mm:ss, but this causes a serialization error
            // because the xml format for dates should be yyyy-mm-dd
            get
            {
                return ResponseDateInPacificStandardTime == DateTime.MinValue ? string.Empty : XmlConvert.ToString(ResponseDateInPacificStandardTime, XmlDateTimeSerializationMode.RoundtripKind);
            }
            set
            {
                this.ResponseDateInPacificStandardTime = string.IsNullOrEmpty(value) ? DateTime.MinValue : DateTime.Parse(value);
            }
        }
    }
}
