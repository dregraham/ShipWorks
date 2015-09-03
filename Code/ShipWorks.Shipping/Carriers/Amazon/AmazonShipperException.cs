using System;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Base class for AmazonShipperExceptions
    /// </summary>
    [Serializable]
    public class AmazonShipperException : Exception
    {
        public AmazonShipperException()
        {}

        public AmazonShipperException(string message)
            : base(message)
        {}

        public AmazonShipperException(string message, Exception innerException)
            : base(message, innerException)
        {}

        public virtual long Code
        {
            get { return 0; }
        }
    }
}