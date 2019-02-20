using Interapptive.Shared.AutoUpdate;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Handles checking for updates during the upgrade window.
    /// </summary>
    public interface IUpgradeTimeWindow
    {
        /// <summary>
        /// Call swc.exe with getupdatewindow parameter
        /// </summary>
        void CallGetUpdateWindow();

        /// <summary>
        /// Update the window
        /// </summary>
        void UpdateWindow(UpdateWindowData updateWindowData);
    }
}