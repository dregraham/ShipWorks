using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Autofac;
using ShipWorks.Core.Messaging;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.UI;
using ShipWorks.Messages;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Provide available shipment types to UI controls
    /// </summary>
    public class ShipmentTypeProvider : INotifyPropertyChanged
    {
        private static ShipmentTypeProvider current = DesignModeDetector.IsDesignerHosted() ? 
            null : 
            IoC.UnsafeGlobalLifetimeScope.Resolve<ShipmentTypeProvider>();
        
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly PropertyChangedHandler handler;
        private IEnumerable<ShipmentTypeCode> available;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Get the current instance of the shipment type provider
        /// </summary>
        public static ShipmentTypeProvider Current => current;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeProvider(IMessenger messenger, IShipmentTypeManager shipmentTypeManager)
        {
            this.shipmentTypeManager = shipmentTypeManager;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            messenger.Handle<EnabledCarriersChangedMessage>(this, UpdateAvailableCarriers);
            Available = shipmentTypeManager.EnabledShipmentTypeCodes.ToList();
        }

        /// <summary>
        /// Available shipment types
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<ShipmentTypeCode> Available
        {
            get { return available; }
            private set { handler.Set(nameof(Available), ref available, value); }
        }

        /// <summary>
        /// Update the list of available shipment types
        /// </summary>
        private void UpdateAvailableCarriers(EnabledCarriersChangedMessage message)
        {
            Available = shipmentTypeManager.EnabledShipmentTypeCodes
                .Concat(message.Added)
                .Except(message.Removed)
                .Distinct()
                .ToList();
        }
    }
}
