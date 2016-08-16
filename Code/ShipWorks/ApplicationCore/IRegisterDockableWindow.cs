using Divelements.SandRibbon;
using TD.SandDock;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Allow controls and other elements to be registered with the main form
    /// </summary>
    public interface IMainFormElementRegistration
    {
        /// <summary>
        /// Register an element in the main form containers
        /// </summary>
        void Register(SandDockManager dockManager, Ribbon ribbon);
    }
}
