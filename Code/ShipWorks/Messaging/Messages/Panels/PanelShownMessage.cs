using ShipWorks.Core.Messaging;
using TD.SandDock;

namespace ShipWorks.Messaging.Messages.Panels
{
    /// <summary>
    /// A dockable panel was shown
    /// </summary>
    public struct PanelShownMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PanelShownMessage(object sender, DockControl panel)
        {
            Sender = sender;
            Panel = panel;
        }

        /// <summary>
        /// Sender
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Panel that was shown
        /// </summary>
        public DockControl Panel { get; }
    }
}
