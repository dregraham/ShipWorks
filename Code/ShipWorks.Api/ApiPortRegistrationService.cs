using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Win32;

namespace ShipWorks.Api
{
    /// <summary>
    /// service for registering a port for the api
    /// </summary>
    public class ApiPortRegistrationService : IApiPortRegistrationService
    {
        /// <summary>
        /// Register the given port
        /// </summary>
        public bool Register(long portNumber)
        {
            string command = $"http add urlacl url=http://+:{portNumber}/ShipWorks/ user='Everyone'";

            return NetshUtility.ExecuteNetsh(command) == 0;
        }
    }
}
