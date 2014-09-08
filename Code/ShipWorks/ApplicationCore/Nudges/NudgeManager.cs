using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Nudges
{
    /// <summary>
    /// Manager of all the Nudges that are available in ShipWorks. Nudges are retreived from Tango
    /// and stored in memory.
    /// </summary>
    public static class NudgeManager
    {
        private static List<Nudge> nudges = new List<Nudge>();
        private static object lockObject = new object();

        /// <summary>
        /// Initializes this instance by clearing out any previous nudges and refreshing the
        /// nudges stored in memory for the given list of stores.
        /// </summary>
        /// <param name="stores">The stores.</param>
        public static void Initialize(IEnumerable<StoreEntity> stores)
        {
            lock (lockObject)
            {
                nudges.Clear();

                ITangoWebClient tangoWebClient = new TangoWebClientFactory().CreateWebClient();
                nudges = tangoWebClient.GetNudges(stores).ToList();
            }
        }

        /// <summary>
        /// Gets the nudges.
        /// </summary>
        public static IEnumerable<Nudge> Nudges
        {
            get
            {
                lock (lockObject)
                {
                    // Return a new list, so our internal list isn't altered by any consumers
                    return nudges.ToList();
                }
            }
        }

        /// <summary>
        /// Shows any nudges that need to be seen.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="nudge">The nudge.</param>
        public static void ShowNudge(IWin32Window owner, Nudge nudge)
        {
            // TODO: Move this elsewhere since it doesn't necessarily belong in the NudgeManager
            // as far as SRP goes...
            using (NudgeDlg nudgeDialog = new NudgeDlg(nudge))
            {
                nudgeDialog.ShowDialog(owner);
            }
        }
    }
}
