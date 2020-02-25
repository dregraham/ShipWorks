using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Nudges
{
    /// <summary>
    /// Null Nudge Manager (follows null object design pattern)
    /// </summary>
    public class NullNudgeManager : INudgeManager
    {
        /// <summary>
        /// Returns default (null) Nudge
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Nudge GetFirstNudgeOfType(NudgeType type)
        {
            return default(Nudge);                
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        public void Refresh(IEnumerable<StoreEntity> stores)
        {
            // Do nothing
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        public void ShowNudge(IWin32Window owner, Nudge nudge)
        {
            // Do nothing
        }
    }
}
