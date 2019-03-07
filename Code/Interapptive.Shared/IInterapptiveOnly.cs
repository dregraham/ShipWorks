using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interapptive.Shared
{
    /// <summary>
    /// Used to control features and functions that should only be available to Interapptive employees.
    /// </summary>
    public interface IInterapptiveOnly
    {
        /// <summary>
        /// Indicates if auto update is disabled.
        /// </summary>
        bool DisableAutoUpdate { get; }
    }
}
