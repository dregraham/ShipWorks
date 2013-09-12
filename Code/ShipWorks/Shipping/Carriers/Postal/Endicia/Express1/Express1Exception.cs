using System;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Express1 Exception
    /// </summary>
    [Serializable]
    public class Express1Exception : EndiciaException
    {
        public Express1Exception()
        {
        }

        public Express1Exception(string message)
            : base(message)
        {
        }

        public Express1Exception(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
