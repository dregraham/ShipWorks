using ShipWorks.Core.Messaging;
using TD.SandDock;

namespace ShipWorks.Messaging.Messages.Panels
{
    /// <summary>
    /// A dockable panel was hidden
    /// </summary>
    public struct PanelHiddenMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PanelHiddenMessage(object sender, DockControl panel)
        {
            Sender = sender;
            Panel = panel;
        }

        /// <summary>
        /// Sender
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Panel that was hidden
        /// </summary>
        public DockControl Panel { get; }
    }
}
