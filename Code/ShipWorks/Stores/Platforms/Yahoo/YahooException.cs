using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo
{
    /// <summary>
    /// Base for exceptions thrown and known by yahoo
    /// </summary>
    [Serializable]
    public class YahooException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YahooException"/> class.
        /// </summary>
        public YahooException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public YahooException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public YahooException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Deserialize
        /// </summary>
        protected YahooException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
