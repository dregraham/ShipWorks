using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Exception class for wrapping exceptions during ebay communications
    /// </summary>
    [Serializable]
    public class EbayException : Exception
    {
        // Soap error code, if one exists
        string errorCode = "";

        /// <summary>
        /// The eBay SOAP error code
        /// </summary>
        public string ErrorCode
        {
            get { return errorCode; }
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public EbayException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            errorCode = info.GetString("errorCode");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayException(string message, string errorCode)
            : base(message)
        {
            this.errorCode = errorCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayException(string message)
            : this(message, string.Empty)
        {

        }

        /// <summary>
        /// Serialization
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("errorCode", errorCode);
        }
    }
}
