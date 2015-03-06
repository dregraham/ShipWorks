using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// DTO to hold Incoming Probe Result
    /// </summary>
    public class IncomingProbeResult
    {
        public string Host { get; set; }
        public EmailIncomingServerType HostType { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public int Port { get; set; }
        public EmailIncomingSecurityType IncomingSecurity { get; set; }
    }
}
