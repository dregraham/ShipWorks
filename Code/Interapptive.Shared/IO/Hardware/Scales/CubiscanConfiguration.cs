using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interapptive.Shared.IO.Hardware.Scales
{
    public class CubiscanConfiguration
    {
        public CubiscanConfiguration(bool isConfigured, string ipAddress, int port)
        {
            IsConfigured = isConfigured;
            IpAddress = ipAddress;
            Port = port;
        }

        public bool IsConfigured { get; }

        public string IpAddress { get; }

        public int Port { get; }
    }
}
