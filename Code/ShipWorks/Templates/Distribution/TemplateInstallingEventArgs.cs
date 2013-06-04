using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Distribution
{
    /// <summary>
    /// EventArgs for the TemplateInstalling event
    /// </summary>
    public class TemplateInstallingEventArgs : EventArgs
    {
        string originalFullName;
        string targetFullName;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateInstallingEventArgs(string originalFullName, string targetFullName)
        {
            this.originalFullName = originalFullName;
            this.targetFullName = targetFullName;
        }

        /// <summary>
        /// The full name of the template being installed, as it was\is originally.
        /// </summary>
        public string OriginalFullName
        {
            get { return originalFullName; }
        }

        /// <summary>
        /// The full name of the template as it will be installed
        /// </summary>
        public string TargetFullName
        {
            get { return targetFullName; }
        }
    }
}
