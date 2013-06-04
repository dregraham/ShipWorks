using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Errors.Response
{
    /// <summary>
    /// A data transport object for the results of an error from Newegg.
    /// </summary>
    [Serializable]
    [XmlRoot("Errors")]
    public class ErrorResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResult"/> class.
        /// </summary>
        public ErrorResult()
        {
            this.Errors = new List<Error>();
        }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        [XmlElement("Error")]
        public List<Error> Errors { get; set; }
    }
}
