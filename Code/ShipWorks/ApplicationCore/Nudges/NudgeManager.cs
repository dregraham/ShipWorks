﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using log4net;
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
        private readonly static ILog log = LogManager.GetLogger(typeof (NudgeManager));
        private readonly static object lockObject = new object();

        private static List<Nudge> nudges = new List<Nudge>();

        /// <summary>
        /// Initializes this instance by clearing out any previous nudges and refreshing the
        /// nudges stored in memory for the given list of stores.
        /// </summary>
        /// <param name="stores">The stores.</param>
        public static void Initialize(IEnumerable<StoreEntity> stores)
        {
            log.Info("Initializing nudges");
            lock (lockObject)
            {
                nudges.Clear();
             
                try
                {
                    ITangoWebClient tangoWebClient = new TangoWebClientFactory().CreateWebClient();
                    nudges = tangoWebClient.GetNudges(stores).ToList();

                    log.InfoFormat("Found {0} nudges", nudges.Count);
                }
                catch (TangoException exception)
                {
                    // Don't crash if SSL could not be verified
                    log.Error("Could not intialize nudges.", exception);
                }
            }
        }

        /// <summary>
        /// Get the first nudge of the given type, or null if there are none
        /// </summary>
        public static Nudge GetFirstNudgeOfType(NudgeType type)
        {
            return nudges.FirstOrDefault(x => x.NudgeType == type);
        }

        /// <summary>
        /// Shows any nudges that need to be seen.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="nudge">The nudge.</param>
        public static void ShowNudge(IWin32Window owner, Nudge nudge)
        {
            if (nudge != null && nudge.NudgeOptions.Any())
            {
                // TODO: Move this elsewhere since it doesn't necessarily belong in the NudgeManager as far as SRP goes...
                log.InfoFormat("Showing nudge {0}", nudge.NudgeID);
                using (NudgeDlg nudgeDialog = new NudgeDlg(nudge))
                {
                    nudgeDialog.ShowDialog(owner);
                }
            }
        }
    }
}
