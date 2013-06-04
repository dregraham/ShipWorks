using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// Buy.Com specific Exception
    /// </summary>
    [Serializable]
    public class BuyDotComException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComException()
        {

        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}