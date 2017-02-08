using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Environment Wrapper
    /// </summary>
    public class EnvironmentWrapper : IEnvironmentWrapper
    {
        /// <summary>
        /// Wraps Environment.OSVersion
        /// </summary>
        public OperatingSystem OSVersion => Environment.OSVersion;

        /// <summary>
        /// Wraps Environment.Is64BitOperatingSystem
        /// </summary>
        public bool Is64BitOperatingSystem => Environment.Is64BitOperatingSystem;
    }
}
