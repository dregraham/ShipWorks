using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus.Response
{
    /// <summary>
    /// A data transport object containing the responses to a report status request to Newegg.
    /// </summary>
    [Serializable]
    [XmlRoot("ResponseBody")]
    public class ResponseBody
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseBody"/> class.
        /// </summary>
        public ResponseBody()
        {
            this.Responses = new List<ResponseInfo>();
        }

        /// <summary>
        /// Gets or sets the responses.
        /// </summary>
        /// <value>
        /// The responses.
        /// </value>
        [XmlArray("ResponseList")]
        [XmlArrayItem("ResponseInfo")]
        public List<ResponseInfo> Responses { get; set; }
    }
}
