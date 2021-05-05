using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.IO.Hardware.Scales;

namespace Interapptive.Shared.IO.Hardware
{
    /// <summary>
    /// Class to get the computer's Cubiscan configuration
    /// </summary>
    public interface ICubiscanConfigurationManager
    {
        /// <summary>
        /// Gets the computer's Cubiscan configuration
        /// </summary>
        CubiscanConfiguration GetConfiguration();
    }
}
