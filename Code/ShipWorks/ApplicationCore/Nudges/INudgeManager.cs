using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Nudges
{
    /// <summary>
    /// Manager of all the Nudges that are available in ShipWorks. Nudges are retrieved from Tango
    /// and stored in memory.
    /// </summary>
    public interface INudgeManager
    {
        /// <summary>
        /// Initializes this instance by clearing out any previous nudges and refreshing the
        /// nudges stored in memory for the given list of stores.
        /// </summary>
        /// <param name="stores">The stores.</param>
        void Refresh(IEnumerable<StoreEntity> stores);

        /// <summary>
        /// Get the first nudge of the given type, or null if there are none
        /// </summary>
        Nudge GetFirstNudgeOfType(NudgeType type);

        /// <summary>
        /// Shows any nudges that need to be seen.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="nudge">The nudge.</param>
        void ShowNudge(IWin32Window owner, Nudge nudge);
    }
}
