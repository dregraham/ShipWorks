using System;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Express1 Exception
    /// </summary>
    [Serializable]
    public class Express1EndiciaException : EndiciaException
    {
        public Express1EndiciaException()
        {
        }

        public Express1EndiciaException(string message)
            : base(message)
        {
        }

        public Express1EndiciaException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
