using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rebex.Net;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// DTO to hold Email Smtp Probe Results
    /// </summary>
    public class SmtpProbeResult
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public SmtpSecurity SmtpSecurity { get; set; }
    }
}
