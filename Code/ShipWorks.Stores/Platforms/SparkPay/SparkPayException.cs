using System;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    /// <summary>
    /// Handleable exception thrown by Generic store and derivatives
    /// </summary>
    public class SparkPayException : Exception
    {
        public SparkPayException()
        {

        }

        public SparkPayException(string message)
            : base(message)
        {

        }

        public SparkPayException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
