using System;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation.Response
{
    /// <summary>
    /// The top level data transport object for the results of cancelling an order via the Newegg API.
    /// </summary>
    [Serializable]
    [XmlRoot("UpdateOrderStatusInfo")]
    public class CancellationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationResult"/> class.
        /// </summary>
        public CancellationResult()
        {
            this.Detail = new CancellationResultDetail();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the result is a successful result.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the result was successful; otherwise, <c>false</c>.
        /// </value>
        [XmlElement("IsSuccessful")]
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        /// <value>
        /// The detail.
        /// </value>
        [XmlElement("Result")]
        public CancellationResultDetail Detail { get; set; }
    }
}
