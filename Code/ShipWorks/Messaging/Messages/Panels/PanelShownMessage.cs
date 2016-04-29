using System;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Messaging.Logging;
using Newtonsoft.Json;
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
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Sender
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Panel that was shown
        /// </summary>
        [JsonConverter(typeof(TypeConverter))]
        public DockControl Panel { get; }
    }
}
