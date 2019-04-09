using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.Dashboard.Content;

namespace ShipWorks.ApplicationCore.Dashboard
{
    /// <summary>
    /// Display details for a local message
    /// </summary>
    public class DashboardLocalMessageDetails
    {
        /// <summary>
        /// Primary text to display
        /// </summary>
        public string PrimaryText { get; set; } = string.Empty;

        /// <summary>
        /// Secondary text to display
        /// </summary>
        public string SecondaryText { get; set; } = string.Empty;

        /// <summary>
        /// Image type to use
        /// </summary>
        public DashboardMessageImageType ImageType { get; set; }

        /// <summary>
        /// Should friendly time and date be used
        /// </summary>
        public bool UseFriendlyDateTime { get; set; } = true;
    }
}
