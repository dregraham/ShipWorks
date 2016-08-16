using System;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Messaging.Logging;
using Newtonsoft.Json;
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
        /// Panel that was hidden
        /// </summary>
        [JsonConverter(typeof(TypeConverter))]
        public DockControl Panel { get; }
    }
}
