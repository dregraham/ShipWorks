using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus.Response
{
    /// <summary>
    /// A data transport object containing the details of a respnse to a report status request to Newegg.
    /// </summary>
    [Serializable]
    [XmlRoot("ResponseInfo")]
    public class ResponseInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseInfo"/> class.
        /// </summary>
        public ResponseInfo()
        { }


        /// <summary>
        /// Gets or sets the request ID.
        /// </summary>
        /// <value>
        /// The request ID.
        /// </value>
        [XmlElement("RequestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the type of the newegg request.
        /// </summary>
        /// <value>
        /// The type of the newegg request.
        /// </value>
        [XmlElement("RequestType")]
        public string NeweggRequestType { get; set; }

        /// <summary>
        /// Gets or sets the request date transport in pacific standard time. This
        /// should not be referenced in code as it is only a shim for transporting
        /// the RequestDateInPacificStandardTime property.
        /// </summary>
        /// <value>
        /// The request date transport in pacific standard time.
        /// </value>
        [XmlElement("RequestDate")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
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
                this.RequestDateInPacificStandardTime = string.IsNullOrEmpty(value) ? DateTime.MinValue : DateTime.Parse(value);
            }
        }

        
        /// <summary>
        /// Gets or sets the request date in pacific standard time.
        /// </summary>
        /// <value>
        /// The request date.
        /// </value>
        [XmlIgnore]
        // This is ignored during xml serialization due to the RequestDateTransport shim property
        public DateTime RequestDateInPacificStandardTime { get; set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        [XmlElement("Error")]
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [XmlElement("RequestStatus")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the memo.
        /// </summary>
        /// <value>
        /// The memo.
        /// </value>
        [XmlElement("Memo")]
        public string Memo { get; set; }
    }
}
