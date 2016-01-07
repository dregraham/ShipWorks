using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// License describes the capabilities of the customer's license.
    /// </summary>
    public interface ILicense
    {
        /// <summary>
        /// Refresh the License capabilities from Tango
        /// </summary>
        void Refresh();

        /// <summary>
        /// Reason the license is Disabled
        /// </summary>
        string DisabledReason { get; set; }

        /// <summary>
        /// Is the license Disabled
        /// </summary>
        bool IsDisabled { get; }
    }
}
