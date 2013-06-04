using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.Infopia
{
    /// <summary>
    /// Exception to wrap error messages and codes coming back from failed CA calls
    /// </summary>
    [Serializable]
    public class InfopiaException : Exception
    {
        // Infopia message code on the soap response
        int statusCode = 0;

        /// <summary>
        /// SOAP Message Code from CA
        /// </summary>
        public int StatusCode
        {
            get { return statusCode; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor for specifying CA Error Code
        /// </summary>
        public InfopiaException(int statusCode, string message) 
            : base(message)
        {
            this.statusCode = statusCode;
        }

        /// <summary>
        /// Constructor for just message
        /// </summary>
        public InfopiaException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected InfopiaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            statusCode = info.GetInt32("statusCode");
        }

        /// <summary>
        /// Deserilization 
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("statusCode", statusCode);
        }
    }
}
