using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore;
using ShipWorks.UI;

namespace ShipWorks
{
    public partial class MainForm
    {
        /// <summary>
        /// Manages the Heartbeat. Heartbeat controls reloading cache.
        /// </summary>
        /// <remarks>
        /// The UIHeartbeat skips reloading caches and other actions when a dialog was open or
        /// when the main window was disabled. In OrderLookup mode, we don't need to update filters
        /// or anything like the UIHeartbeat was doing, but we DO need to skip reloading caches and
        /// performing the other actions. This was discovered by a bug where adding a store caused
        /// a crash because the newly added store was added to the cache before it was completed.
        /// </remarks>
        private class OrderLookupHeartbeat : Heartbeat
        {
            private readonly MainForm mainForm;

            /// <summary>
            /// Initializes a new instance of the <see cref="OrderLookupHeartbeat"/> class.
            /// </summary>
            public OrderLookupHeartbeat(MainForm mainForm)
            {
                this.mainForm = mainForm;
            }

            /// <summary>
            /// Determines if the heartbeat is ready to beat
            /// </summary>
            protected override bool CanBeat() =>
                !mainForm.IsDisposed || base.CanBeat();

            /// <summary>
            /// Runs after it is determined that ShipWorks is not in a state to do a heartbeat.
            /// </summary>
            protected override void ProcessHeartbeat(bool changesDetected, bool forceReload)
            {
                // Detect if a modal window is open.  The popup test is, for now, to make sure we don't reload
                // data when a management or other dialog is open
                if (NativeMethods.IsWindowEnabled(mainForm.Handle) && !PopupController.IsAnyPopupVisible)
                {
                    base.ProcessHeartbeat(changesDetected, forceReload);
                }
            }
        }
    }
}