using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;

namespace Interapptive.Shared
{
    /// <summary>
    /// Used to control features and functions that should only be available to Interapptive employees.
    /// </summary>
    [Component]
    public class InterapptiveOnlyWrapper : IInterapptiveOnly
    {
        /// <summary>
        /// Indicates if auto update is disabled.
        /// </summary>
        public bool DisableAutoUpdate => InterapptiveOnly.DisableAutoUpdate;
    }
}
