using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// An eBay exception for when an eBay order does not contain a GSP reference ID when ShipWorks
    /// is expecting there to be one (i.e. an order is downloaded from eBay, the order is marked
    /// as a GSP order, but the GSP reference ID is not provided).
    /// </summary>
    [Serializable]
    public class EbayMissingGspReferenceException : EbayException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EbayMissingGspReferenceException"/> class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected EbayMissingGspReferenceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayMissingGspReferenceException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public EbayMissingGspReferenceException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayMissingGspReferenceException"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        public EbayMissingGspReferenceException(string message, string errorCode)
            : base(message, errorCode)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayMissingGspReferenceException"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public EbayMissingGspReferenceException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
