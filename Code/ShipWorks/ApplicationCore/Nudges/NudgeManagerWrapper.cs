using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Nudges
{
    /// <summary>
    /// Wrapper for the static NudgeManager
    /// </summary>
    public class NudgeManagerWrapper : INudgeManager
    {
        /// <summary>
        /// Initializes this instance, and calls Refresh to clear out any previous nudges and refresh the
        /// nudges stored in memory for the given list of stores.
        /// </summary>
        /// <param name="stores">The stores.</param>
        public void Refresh(IEnumerable<StoreEntity> stores) => NudgeManager.Refresh(stores);

        /// <summary>
        /// Get the first nudge of the given type, or null if there are none
        /// </summary>
        public Nudge GetFirstNudgeOfType(NudgeType type) => NudgeManager.GetFirstNudgeOfType(type);

        /// <summary>
        /// Shows any nudges that need to be seen.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="nudge">The nudge.</param>
        public void ShowNudge(IWin32Window owner, Nudge nudge) => NudgeManager.ShowNudge(owner, nudge);
    }
}
