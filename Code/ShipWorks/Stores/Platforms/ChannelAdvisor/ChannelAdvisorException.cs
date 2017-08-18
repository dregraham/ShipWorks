using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Exception to wrap error messages and codes coming back from failed CA calls
    /// </summary>
    [Serializable]
    public class ChannelAdvisorException : Exception
    {
        // CA message code on the soap response
        int messageCode = -1;

        /// <summary>
        /// SOAP Message Code from CA
        /// </summary>
        public int MessageCode
        {
            get { return messageCode; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelAdvisorException"/> class.
        /// </summary>
        public ChannelAdvisorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor for specifying CA Error Code
        /// </summary>
        public ChannelAdvisorException(int messageCode, string message)
            : base(message)
        {
            this.messageCode = messageCode;
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        protected ChannelAdvisorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            messageCode = info.GetInt32("messageCode");
        }

        /// <summary>
        /// Serialization
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("messageCode", messageCode);
        }
    }
}
