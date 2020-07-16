using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Interapptive.Shared.AutoUpdate
{
    /// <summary>
    /// Container for auto update telemetry
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public struct AutoUpdateTelemetry
    {
        /// <summary>
        /// The version we are updating from
        /// </summary>
        public string StartVersion { get; set; }

        /// <summary>
        /// The version we are updating to
        /// </summary>
        public string TargetVersion { get; set; }

        /// <summary>
        /// The date/time the update started
        /// </summary>
        public DateTime Started { get; set; }
    }
}
