using System;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Exception thrown by Walmart
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class WalmartException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartException"/> class.
        /// </summary>
        public WalmartException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartException"/> class.
        /// </summary>
        public WalmartException(string message) :
            base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartException"/> class.
        /// </summary>
        public WalmartException(string message, Exception inner) :
            base(message, inner)
        {
        }
    }
}