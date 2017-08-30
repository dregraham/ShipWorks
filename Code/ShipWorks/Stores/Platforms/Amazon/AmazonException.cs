using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Exception to wrap errors coming back from Amazon web services
    /// </summary>
    [Serializable]
    public class AmazonException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonException()
        {

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
            : base(message, inner)
        {
            Code = code;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonException(Type webServiceType, string message)
            : base(message)
        {
            WebServiceType = webServiceType;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonException(Type webServiceType, Exception inner)
            : base(inner == null ? "" : inner.Message, inner)
        {
            WebServiceType = webServiceType;
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        protected AmazonException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            WebServiceType = info.GetValue("webServiceType", typeof(Type)) as Type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonException"/> class.
        /// </summary>
        public AmazonException(string message) : base(message)
        {
        }

        /// <summary>
        /// Amazon MWS Error Code
        /// </summary>
        public string Code { get; } = string.Empty;

        /// <summary>
        /// Gets the Amazon Web Service type that caused the exception
        /// </summary>
        public Type WebServiceType { get; }

        /// <summary>
        /// Serialize
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("webServiceType", WebServiceType);
        }
    }
}
