using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Postal.Endicia;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
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
