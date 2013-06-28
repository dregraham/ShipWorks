using Interapptive.Shared.Utility;
using System.ComponentModel;
using System.Reflection;


namespace ShipWorks.ApplicationCore.WindowsServices
{
    /// <summary>
    /// Describes the status of a ShipWorks Windows service as inferred from check-in information.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ServiceStatus
    {
        /// <summary>
        /// The service has never been started on the associated computer.
        /// </summary>
        [Description("Never Started")]
        [ImageResource("gear_run_disabled_16")]
        NeverStarted,

        /// <summary>
        /// The service is stopped, as a result of a "normal" shutdown.
        /// </summary>
        [Description("Stopped")]
        [ImageResource("gear_stop_16")]
        Stopped,

        /// <summary>
        /// The service has stopped checking in.  It may have crashed.
        /// </summary>
        [Description("Not Responding")]
        [ImageResource("gear_error16")]
        NotResponding,

        /// <summary>
        /// The service is running.
        /// </summary>
        [Description("Running")]
        [ImageResource("gear_run16")]
        Running
    }
}
