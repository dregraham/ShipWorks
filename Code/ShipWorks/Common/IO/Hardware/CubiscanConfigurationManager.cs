using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.IO.Hardware;
using Interapptive.Shared.IO.Hardware.Scales;
using ShipWorks.ApplicationCore;
using ShipWorks.Users;

namespace ShipWorks.Common.IO.Hardware
{
    [Component(SingleInstance = true)]
    public class CubiscanConfigurationManager : ICubiscanConfigurationManager
    {
        private readonly IComputerManager computerManager;
        private readonly IDeviceManager deviceManager;

        public CubiscanConfigurationManager(IComputerManager computerManager, IDeviceManager deviceManager)
        {
            this.computerManager = computerManager;
            this.deviceManager = deviceManager;
        }

        public CubiscanConfiguration GetConfiguration()
        {
            var computer = computerManager.GetComputer().ComputerID;
            var device = deviceManager.DevicesReadOnly.FirstOrDefault(d => d.ComputerID == computer);
            return new CubiscanConfiguration(device != null, device?.IPAddress, device?.PortNumber ?? 0);
        }
    }
}
