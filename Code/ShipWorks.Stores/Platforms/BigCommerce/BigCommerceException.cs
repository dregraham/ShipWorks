using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// BigCommerce specific exception
    /// </summary>
    [Serializable]
    public class BigCommerceException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceException(string message) 
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceException(string message, int httpStatusCode)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceException(string message, Exception inner, int httpStatusCode)
            : base(message, inner)
        {
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected BigCommerceException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }

        /// <summary>
        /// HttpStatusCode received from an http call
        /// </summary>
        public int HttpStatusCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Override to implement GetObjectData for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// The user displayable exception message
        /// </summary>
        public override string Message
        {
            get
            {
                string errorMessage = base.Message;

                if (InnerException != null)
                {
                    errorMessage = string.Format("{0}{1}{1}{2}", errorMessage, Environment.NewLine, InnerException.Message);
                }

                return errorMessage;
            }
        }
    }
}
