using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using Autofac;
using Interapptive.Shared.Messaging;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.UI;
using ShipWorks.Messaging.Messages;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Provide available shipment types to UI controls
    /// </summary>
    public class ShipmentTypeProvider : INotifyPropertyChanged, IDisposable
    {
        private static ShipmentTypeProvider current = DesignModeDetector.IsDesignerHosted() ?
            null :
            IoC.UnsafeGlobalLifetimeScope.Resolve<ShipmentTypeProvider>();

        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly PropertyChangedHandler handler;
        private IEnumerable<ShipmentTypeCode> available;
        private readonly IDisposable subscription;
        private readonly IShippingManager shippingManager;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Get the current instance of the shipment type provider
        /// </summary>
        public static ShipmentTypeProvider Current => current;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeProvider(IObservable<IShipWorksMessage> messenger, IShipmentTypeManager shipmentTypeManager, IShippingManager shippingManager)
        {
            this.shipmentTypeManager = shipmentTypeManager;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            subscription = messenger.OfType<EnabledCarriersChangedMessage>().Subscribe(UpdateAvailableCarriers);
            Available = shipmentTypeManager
                .ShipmentTypeCodes
                .Where(shippingManager.IsShipmentTypeEnabled)
                .ToList();
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

        /// <summary>
        /// Dispose this object
        /// </summary>
        public void Dispose() => subscription.Dispose();
    }
}
