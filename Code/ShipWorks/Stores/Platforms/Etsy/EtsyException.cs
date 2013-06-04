using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Etsy specific Exception class
    /// </summary>
    public class EtsyException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}