using System;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Errors.Response
{
    /// <summary>
    /// A data transport object for containing the details about an error from Newegg.
    /// </summary>
    [Serializable]
    [XmlRoot("Error")]
    public class Error
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        [XmlElement("Code")]
        public string ErrorCode { get; set; }


        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [XmlElement("Message")]
        public string Description { get; set; }
    }
}
