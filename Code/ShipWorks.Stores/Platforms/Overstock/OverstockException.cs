using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Overstock specific exception
    /// </summary>
    [Serializable]
    public class OverstockException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockException(string message, int httpStatusCode)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockException(string message, Exception inner, int httpStatusCode)
            : base(message, inner)
        {
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected OverstockException(SerializationInfo info, StreamingContext context) : base(info, context)
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
