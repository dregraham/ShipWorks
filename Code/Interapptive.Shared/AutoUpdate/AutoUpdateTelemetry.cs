using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interapptive.Shared.AutoUpdate
{
    /// <summary>
    /// Container for auto update telemetry
    /// </summary>
    public struct AutoUpdateTelemetry
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AutoUpdateTelemetry(string startVersion, string targetVersion, DateTime started)
        {
            this.StartVersion = startVersion;
            this.TargetVersion = targetVersion;
            this.Started = started;
        }

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
