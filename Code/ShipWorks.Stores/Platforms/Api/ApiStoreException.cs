using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Api
{
    public class ApiStoreException : Exception
    {
        public ApiStoreException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
