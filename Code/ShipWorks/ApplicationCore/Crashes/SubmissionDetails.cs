using System.Reflection;

namespace ShipWorks.ApplicationCore.Crashes
{
    /// <summary>
    /// Submission details for a crash report
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class SubmissionDetails
    {
        public string Identifier { get; set; }

        public string Version { get; set; }

        public string Email { get; set; }

        public string Background { get; set; }

        public string ExceptionTitle { get; set; }

        public string ExceptionSummary { get; set; }

        public string Exception { get; set; }

        public string Environment { get; set; }

        public string Assemblies { get; set; }

        public string LogName { get; internal set; }

        public string CustomerID { get; set; }

        public string InstanceID { get; set; }

        public string OperatingSystem { get; set; }

        public string SessionID { get; set; }

        public string Screens { get; set; }

        public string CPUs { get; set; }

        public string PhysicalMemory { get; set; }

        public string ScreenDimensionsPrimary { get; set; }

        public string ScreenDpiPrimary { get; set; }
    }
}
