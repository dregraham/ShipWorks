using System;
using System.Runtime.Serialization;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Base for all exceptions thrown by the Endicia integration
    /// </summary>
    public class EndiciaException : Exception
    {
        public EndiciaException()
        {

        }

        public EndiciaException(string message)
            : base(message)
        {

        }

        public EndiciaException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected EndiciaException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        {

        }

        public override string Message
        {
            get
            {
                if (base.Message.Contains("Unable to pass through rate"))
                {
                    return
                        "Your Express1 account has not been enabled for using the selected mail class.\n\n" +
                        "Please contact Express1 at 1-800-399-3971 for assistance."; 
                }

                return base.Message;
            }
        }
    }
}
