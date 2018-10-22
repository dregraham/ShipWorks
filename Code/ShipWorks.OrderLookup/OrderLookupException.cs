using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// OrderLookup Exception
    /// </summary>
    public class OrderLookupException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupException(string message) : base(message)
        {
        }
    }
}
