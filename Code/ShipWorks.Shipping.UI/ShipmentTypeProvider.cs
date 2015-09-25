using Autofac;
using Interapptive.Shared.Messaging;
using ShipWorks.ApplicationCore;
using ShipWorks.UI.Controls.Design;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Provide available shipment types to UI controls
    /// </summary>
    public class ShipmentTypeProvider
    {
        private static ShipmentTypeProvider current = DesignModeDetector.IsDesignerHosted() ? 
            null : 
            IoC.UnsafeGlobalLifetimeScope.Resolve<ShipmentTypeProvider>();
        
        private readonly IShipmentTypeManager shipmentTypeManager;

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

            messenger.Handle<EnabledCarriersChangedMessage>(this, UpdateAvailableCarriers);
            Available = new ObservableCollection<ShipmentTypeCode>(shipmentTypeManager.EnabledShipmentTypes.Select(x => x.ShipmentTypeCode));
        }

        /// <summary>
        /// Update the list of available shipment types
        /// </summary>
        private void UpdateAvailableCarriers(EnabledCarriersChangedMessage message)
        {
            RemoveShipmentTypes(message.Removed);
            AddShipmentTypes(message.Added);
        }

        /// <summary>
        /// Available shipment types
        /// </summary>
        public ObservableCollection<ShipmentTypeCode> Available { get; private set; }

        /// <summary>
        /// Remove shipment types from the list of available
        /// </summary>
        private void RemoveShipmentTypes(IEnumerable<ShipmentTypeCode> removed)
        {
            List<ShipmentTypeCode> removeItems = Available
                .Where(x => removed.Contains(x))
                .ToList();

            foreach (ShipmentTypeCode shipmentType in removeItems)
            {
                Available.Remove(shipmentType);
            }
        }

        /// <summary>
        /// Add shipment types to the list of available
        /// </summary>
        private void AddShipmentTypes(IEnumerable<ShipmentTypeCode> added)
        {
            List<ShipmentTypeCode> addItems = added.Except(Available).ToList();

            foreach (ShipmentTypeCode shipmentType in addItems)
            {
                int sortValue = shipmentTypeManager.GetSortValue(shipmentType);
                var insertDetails = Available.Select((x, i) => new { ExistingSortValue = shipmentTypeManager.GetSortValue(x), Index = i })
                    .SkipWhile(x => x.ExistingSortValue < sortValue)
                    .FirstOrDefault();

                if (sortValue == 1)
                {
                    Available.Insert(0, shipmentType);
                }
                else if (insertDetails == null)
                {
                    Available.Add(shipmentType);
                }
                else
                {
                    Available.Insert(insertDetails.Index, shipmentType);
                }
            }
        }
    }
}
