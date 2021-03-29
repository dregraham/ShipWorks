using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Api
{
    /// <summary>
    /// API Store exception
    /// </summary>
    public class ApiStoreException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApiStoreException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
