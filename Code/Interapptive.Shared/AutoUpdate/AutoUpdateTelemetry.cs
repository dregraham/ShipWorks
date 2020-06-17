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
        /// Constructor
        /// </summary>
        public AutoUpdateTelemetry(string startVersion, string targetVersion, DateTime started, Guid instance)
        {
            this.StartVersion = startVersion;
            this.TargetVersion = targetVersion;
            this.Started = started;
            this.Instance = instance;
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

        /// <summary>
        /// The instance of shipworks
        /// </summary>
        public Guid Instance { get; set; }
    }
}
