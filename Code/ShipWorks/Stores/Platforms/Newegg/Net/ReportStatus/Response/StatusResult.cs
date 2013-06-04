using System;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus.Response
{
    /// <summary>
    /// The top level data transport object containing the result of a report status request.
    /// </summary>
    [Serializable]
    [XmlRoot("NeweggAPIResponse")]
    public class StatusResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusResult"/> class.
        /// </summary>
        public StatusResult()
        {
            this.Body = new ResponseBody();
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
        /// Gets or sets the type of the operation.
        /// </summary>
        /// <value>
        /// The type of the operation.
        /// </value>
        public string OperationType { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        [XmlElement("ResponseBody")]
        public ResponseBody Body { get; set; }
    }
}
