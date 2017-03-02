using System;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Exception thrown by Walmart
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class WalmartException : Exception
    {
        public WalmartException()
        {
        }

        public WalmartException(string message) :
            base(message)
        {
        }

        public WalmartException(string message, Exception inner) :
            base(message, inner)
        {
        }
    }
}