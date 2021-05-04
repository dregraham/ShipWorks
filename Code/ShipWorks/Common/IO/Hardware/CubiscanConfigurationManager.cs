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
    /// <summary>
    /// Responsible for retrieving CubiscanConfiguration
    /// </summary>
    [Component(SingleInstance = true)]
    public class CubiscanConfigurationManager : ICubiscanConfigurationManager, IInitializeForCurrentUISession
    {
        private readonly IComputerManager computerManager;
        private readonly IDeviceManager deviceManager;
        private bool initialized = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public CubiscanConfigurationManager(IComputerManager computerManager, IDeviceManager deviceManager)
        {
            this.computerManager = computerManager;
            this.deviceManager = deviceManager;
        }

        /// <summary>
        /// Gets the CubiscanConfiguration for this computer
        /// </summary>
        public CubiscanConfiguration GetConfiguration()
        {
            if (!initialized)
            {
                return new CubiscanConfiguration(false, null, 0);
            }
            
            var computer = computerManager.GetComputer().ComputerID;
            var device = deviceManager.DevicesReadOnly.FirstOrDefault(d => d.ComputerID == computer);
            return new CubiscanConfiguration(device != null, device?.IPAddress, device?.PortNumber ?? 0);
        }

        /// <summary>
        /// Set as initialized
        /// </summary>
        public void InitializeForCurrentSession()
        {
            initialized = true;
        }

        /// <summary>
        /// Set as uninitialized
        /// </summary>
        public void EndSession()
        {
            initialized = false;
        }

        /// <summary>
        /// Nothing to dispose
        /// </summary>
        public void Dispose()
        {
            // nothing to dispose
        }
    }
}
