using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Environment Wrapper
    /// </summary>
    public interface IEnvironmentWrapper
    {
        /// <summary>
        /// Wraps Environment.OSVersion
        /// </summary>
        OperatingSystem OSVersion { get; }

        /// <summary>
        /// Wraps Environment.Is64BitOperatingSystem
        /// </summary>
        bool Is64BitOperatingSystem { get; }
    }
}
