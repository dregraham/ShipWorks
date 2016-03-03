using Divelements.SandRibbon;
using TD.SandDock;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Allow dockable windows to be registered with the main SandDock manager
    /// </summary>
    public interface IRegisterDockableWindow
    {
        /// <summary>
        /// Register a panel with the dock manager
        /// </summary>
        void Register(SandDockManager dockManager, Ribbon ribbon);
    }
}
