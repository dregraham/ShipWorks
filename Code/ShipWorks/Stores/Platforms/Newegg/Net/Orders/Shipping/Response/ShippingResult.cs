using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response
{
    /// <summary>
    /// The top level data transport object containing the results of a request to upload
    /// shipping details to Newegg.
    /// </summary>
    [Serializable]
    [XmlRoot("UpdateOrderStatusInfo")]
    public class ShippingResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingResult"/> class.
        /// </summary>
        public ShippingResult()
        {
            PackageSummary = new PackageProcessingSummary();
            Detail = new ShippingResultDetails();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the result wass successful.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if successful; otherwise, <c>false</c>.
        /// </value>
        [XmlElement("IsSuccess")]
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the package summary.
        /// </summary>
        /// <value>
        /// The package summary.
        /// </value>
        [XmlElement("PackageProcessingSummary")]
        public PackageProcessingSummary PackageSummary { get; set; }

        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        /// <value>
        /// The detail.
        /// </value>
        [XmlElement("Result")]
        public ShippingResultDetails Detail { get; set; }
    }
}
