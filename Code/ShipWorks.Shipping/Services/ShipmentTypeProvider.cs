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
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Provide available shipment types to UI controls
    /// </summary>
    public class ShipmentTypeProvider : IDisposable
    {
        private readonly IShipmentTypeManager shipmentTypeManager;
        private IEnumerable<ShipmentTypeCode> available;
        private static readonly IEnumerable<ShipmentTypeCode> amazonCarriers = new[] { ShipmentTypeCode.Amazon };
        private readonly IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeProvider(IObservable<IShipWorksMessage> messenger, IShipmentTypeManager shipmentTypeManager)
        {
            this.shipmentTypeManager = shipmentTypeManager;

            subscription = messenger.OfType<EnabledCarriersChangedMessage>().Subscribe(UpdateAvailableCarriers);
            available = shipmentTypeManager.EnabledShipmentTypeCodes;
        }
        
        /// <summary>
        /// Get the available shipment types for the given shipment
        /// </summary>
        public IEnumerable<ShipmentTypeCode> GetAvailableShipmentTypes(ICarrierShipmentAdapter shipmentAdapter)
        {
            if (shipmentAdapter.ShipmentTypeCode == ShipmentTypeCode.Amazon)
            {
                return amazonCarriers;
            }

            return available.Except(amazonCarriers);
        }

        /// <summary>
        /// Update the list of available shipment types
        /// </summary>
        private void UpdateAvailableCarriers(EnabledCarriersChangedMessage message)
        {
            available = shipmentTypeManager.EnabledShipmentTypeCodes
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
