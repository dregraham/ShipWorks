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
        /// The service has never been run on the associated computer.
        /// </summary>
        [Description("Never Run")]
        NeverRun,

        /// <summary>
        /// The service is stopped, as a result of a "normal" shutdown.
        /// </summary>
        [Description("Stopped")]
        Stopped,

        /// <summary>
        /// The service has stopped checking in.  It may have crashed.
        /// </summary>
        [Description("Not Responding")]
        NotResponding,

        /// <summary>
        /// The service is running.
        /// </summary>
        [Description("Running")]
        Running
    }
}
