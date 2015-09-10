using Autofac;
using Interapptive.Shared.Messaging;
using ShipWorks.ApplicationCore;
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
        private static ShipmentTypeProvider current = IoC.UnsafeGlobalLifetimeScope.Resolve<ShipmentTypeProvider>();
        
        /// <summary>
        /// Get the current instance of the shipment type provider
        /// </summary>
        public static ShipmentTypeProvider Current => current;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeProvider(IMessenger messenger)
        {
            messenger.Handle<EnabledCarriersChangedMessage>(this, UpdateAvailableCarriers);
            Available = new ObservableCollection<ShipmentType>(ShipmentTypeManager.EnabledShipmentTypes);
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
        public ObservableCollection<ShipmentType> Available { get; private set; }

        /// <summary>
        /// Remove shipment types from the list of available
        /// </summary>
        private void RemoveShipmentTypes(IEnumerable<ShipmentTypeCode> removed)
        {
            List<ShipmentType> removeItems = Available
                .Where(x => removed.Contains(x.ShipmentTypeCode))
                .ToList();

            foreach (ShipmentType shipmentType in removeItems)
            {
                Available.Remove(shipmentType);
            }
        }

        /// <summary>
        /// Add shipment types to the list of available
        /// </summary>
        private void AddShipmentTypes(IEnumerable<ShipmentTypeCode> added)
        {
            List<ShipmentType> addItems = added.Except(Available.Select(x => x.ShipmentTypeCode))
                .Select(x => ShipmentTypeManager.GetType(x))
                .ToList();

            foreach (ShipmentType shipmentType in addItems)
            {
                int sortValue = ShipmentTypeManager.GetSortValue(shipmentType.ShipmentTypeCode);
                var insertDetails = Available.Select((x, i) => new { ExistingSortValue = ShipmentTypeManager.GetSortValue(x.ShipmentTypeCode), Index = i })
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
