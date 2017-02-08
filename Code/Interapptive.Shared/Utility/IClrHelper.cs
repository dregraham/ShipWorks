using System;
using System.Collections.Generic;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// CLR/.Net version detection from
    /// https://msdn.microsoft.com/en-us/library/hh925568(v=vs.110).aspx#net_a
    /// </summary>
    public interface IClrHelper
    {
        /// <summary>
        /// The installed CLR Versions
        /// </summary>
        IEnumerable<Version> ClrVersions { get; }

        /// <summary>
        /// Reloads the current list so that ClrVersions will be repopulated.
        /// </summary>
        void Reload();
    }
}