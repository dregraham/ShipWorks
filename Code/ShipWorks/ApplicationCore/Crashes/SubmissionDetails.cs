using System.Reflection;

namespace ShipWorks.ApplicationCore.Crashes
{
    /// <summary>
    /// Submission details for a crash report
    /// </summary>
    [Obfuscation(Exclude = true)]
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
    }
}
