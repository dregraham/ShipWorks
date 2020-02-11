﻿using Interapptive.Shared.Win32;

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
            string command = $"http add urlacl url=http://+:{portNumber}/ user='Everyone'";

            return NetshCommand.Execute(command) == 0;
        }
    }
}
