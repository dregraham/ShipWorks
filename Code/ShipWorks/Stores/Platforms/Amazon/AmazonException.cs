using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Exception to wrap errors coming back from Amazon web services
    /// </summary>
    [Serializable]
    public class AmazonException : Exception
    {
        // which Amazon web service was the exception source
        Type webServiceType;

        // The error code
        string code = "";

        /// <summary>
        /// Amazon MWS Error Code
        /// </summary>
        public string Code
        {
            get { return code; }
        }

        /// <summary>
        /// Gets the Amazon Web Service type that caused the exception
        /// </summary>
        public Type WebServiceType
        {
            get { return webServiceType; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Constructor for MWS Api Errors
        /// </summary>
        public AmazonException(string code, string message, Exception inner)
            : base (message, inner)
        {
            this.code = code;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonException(Type webServiceType, string message)
            : base(message)
        {
            this.webServiceType = webServiceType;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonException(Type webServiceType, Exception inner)
            : base(inner == null ? "" : inner.Message, inner)
        {
            this.webServiceType = webServiceType;
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        protected AmazonException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            webServiceType = info.GetValue("webServiceType", typeof(Type)) as Type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonException"/> class.
        /// </summary>
        public AmazonException(string message) : base(message)
        {
        }

        /// <summary>
        /// Serialize
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("webServiceType", webServiceType);
        }
    }
}
