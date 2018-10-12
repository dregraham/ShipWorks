using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Model used by the various order lookup viewmodels
    /// </summary>
    [Component(SingleInstance = true)]
    public class OrderLookupShipmentModel : INotifyPropertyChanged, IOrderLookupShipmentModel
    {
        private readonly IMessenger messenger;
        private readonly IShippingManager shippingManager;
        private readonly IMessageHelper messageHelper;
        private readonly PropertyChangedHandler handler;
        private OrderEntity selectedOrder;
        private bool shipmentAllowEditing;
        private decimal totalCost;
        private bool isSaving = false;
        public event EventHandler OnSearchOrder;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupShipmentModel(IMessenger messenger, IShippingManager shippingManager, IMessageHelper messageHelper)
        {
            this.messenger = messenger;
            this.shippingManager = shippingManager;
            this.messageHelper = messageHelper;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The order that is currently in context
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OrderEntity SelectedOrder
        {
            get => selectedOrder;
            private set
            {
                selectedOrder = value;
                RaisePropertyChanged(nameof(SelectedOrder));
            }
        }

        /// <summary>
        /// Can the shipment be edited
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShipmentAllowEditing
        {
            get => shipmentAllowEditing;
            private set => handler.Set(nameof(ShipmentAllowEditing), ref shipmentAllowEditing, value, true);
        }

        /// <summary>
        /// The shipment adapter for the order in context
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICarrierShipmentAdapter ShipmentAdapter { get; private set; }

        /// <summary>
        /// The package adapters for the order in context
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<IPackageAdapter> PackageAdapters { get; private set; }

        /// <summary>
        /// Total cost of the shipment
        /// </summary>
        [Obfuscation(Exclude = true, StripAfterObfuscation = false)]
        public decimal TotalCost
        {
            get => totalCost;
            set => handler.Set(nameof(TotalCost), ref totalCost, value);
        }

        /// <summary>
        /// Invoked when a property on the order object changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise the property changed event
        /// </summary>
        public void RaisePropertyChanged(string propertyName)
        {
            handler.RaisePropertyChanged(propertyName);

            if (ShipmentAdapter?.Shipment != null)
            {
                messenger.Send(new ShipmentChangedMessage(this, ShipmentAdapter, propertyName));
            }
        }

        /// <summary>
        /// Save the current shipment to the database
        /// </summary>
        public void SaveToDatabase()
        {
            if (isSaving)
            {
                return;
            }

            isSaving = true;
            using (Disposable.Create(() => isSaving = false))
            {
                if (!ShipmentAllowEditing || (ShipmentAdapter?.Shipment?.Processed ?? true))
                {
                    return;
                }

                IDictionary<ShipmentEntity, Exception> errors;
                using (handler.SuppressChangeNotifications())
                {
                    ShipmentAdapter.UpdateDynamicData();
                    errors = shippingManager.SaveShipmentToDatabase(ShipmentAdapter?.Shipment, false);
                }

                DisplayError(errors);
            }
        }

        /// <summary>
        /// Refresh the shipment from the database
        /// </summary>
        public void RefreshShipmentFromDatabase()
        {
            RemovePropertyChangedEventsFromEntities();

            shippingManager.RefreshShipment(ShipmentAdapter.Shipment);
            ShipmentAdapter = shippingManager.GetShipmentAdapter(ShipmentAdapter.Shipment);

            RefreshProperties();

            AddPropertyChangedEventsToEntities();
            RaisePropertyChanged(null);
        }

        /// <summary>
        /// Unload the order
        /// </summary>
        public void Unload()
        {
            SaveToDatabase();
            ClearOrder();
        }

        /// <summary>
        /// Show an error if one is associated with the current shipment
        /// </summary>
        private void DisplayError(IDictionary<ShipmentEntity, Exception> errors)
        {
            Exception error = null;

            if (ShipmentAdapter?.Shipment != null && errors.TryGetValue(ShipmentAdapter.Shipment, out error))
            {
                messageHelper.ShowError("The selected shipments were edited or deleted by another ShipWorks user and your changes could not be saved.\n\n" +
                                        "The shipments will be refreshed to reflect the recent changes.");

                RefreshShipmentFromDatabase();
            }
        }

        /// <summary>
        /// Load an order
        /// </summary>
        public void LoadOrder(OrderEntity order)
        {
            OnSearchOrder?.Invoke(this, null);

            if (order == null)
            {
                ClearOrder();
                return;
            }

            RemovePropertyChangedEventsFromEntities();

            if ((order.Shipments?.Count ?? 0) > 0)
            {
                ShipmentAdapter = shippingManager.GetShipmentAdapter(order.Shipments.Last());

                // Update dynamic data here because everything downstream will also attempt to update dynamic data
                // doing it here gives us a head start before we are tracking property changes, this also ensures that the
                // shipment date is not in the past
                ShipmentAdapter.UpdateDynamicData();
            }

            RefreshProperties();

            AddPropertyChangedEventsToEntities();

            if (ShipmentAdapter != null)
            {
                messenger.Send(new ShipmentSelectionChangedMessage(this, new[] { ShipmentAdapter.Shipment.ShipmentID }, ShipmentAdapter));
            }

            RaisePropertyChanged(nameof(ShipmentTypeCode));

            SelectedOrder = order;
        }

        /// <summary>
        /// Clear the order
        /// </summary>
        private void ClearOrder()
        {
            RemovePropertyChangedEventsFromEntities();

            ShipmentAdapter = null;
            ShipmentAllowEditing = false;
            PackageAdapters = null;
            SelectedOrder = null;
            TotalCost = 0;

            messenger.Send(new OrderLookupClearOrderMessage());
        }

        /// <summary>
        /// Refresh properties from the given order
        /// </summary>
        private void RefreshProperties()
        {
            ShipmentAllowEditing = !ShipmentAdapter?.Shipment?.Processed ?? false;
            PackageAdapters = ShipmentAdapter?.GetPackageAdapters();
            TotalCost = ShipmentAdapter?.Shipment?.ShipmentCost ?? 0;
        }

        /// <summary>
        /// Add property change event handlers
        /// </summary>
        private void AddPropertyChangedEventsToEntities()
        {
            if (ShipmentAdapter != null)
            {
                ShipmentAdapter.Shipment.PropertyChanged += RaisePropertyChanged;
            }

            if (ShipmentAdapter?.Shipment?.Postal != null)
            {
                ShipmentAdapter.Shipment.Postal.PropertyChanged += RaisePropertyChanged;
            }
        }

        /// <summary>
        /// Remove property changed events from shipment entities
        /// </summary>
        private void RemovePropertyChangedEventsFromEntities()
        {
            if (ShipmentAdapter != null)
            {
                ShipmentAdapter.Shipment.PropertyChanged -= RaisePropertyChanged;
            }

            if (ShipmentAdapter?.Shipment?.Postal != null)
            {
                ShipmentAdapter.Shipment.Postal.PropertyChanged -= RaisePropertyChanged;
            }
        }

        /// <summary>
        /// Call the RaisePropertyChanged with PropertyName
        /// </summary>
        private void RaisePropertyChanged(object sender, PropertyChangedEventArgs e) => RaisePropertyChanged(e.PropertyName);

        public void ChangeShipmentType(ShipmentTypeCode value)
        {
            if (value != ShipmentAdapter.ShipmentTypeCode)
            {
                // Changing shipment type leads to unloading and loading entities into the current ShipmentEntity.
                // To prepare for this, we remove existing handlers from the existing entities, change the shipment type,
                // then add handlers to the possibly new entities.
                using (handler.SuppressChangeNotifications())
                {
                    RemovePropertyChangedEventsFromEntities();

                    ShipmentAdapter = shippingManager.ChangeShipmentType(value, ShipmentAdapter.Shipment);
                    RefreshProperties();

                    AddPropertyChangedEventsToEntities();

                    messenger.Send(new ShipmentSelectionChangedMessage(this, new[] { ShipmentAdapter.Shipment.ShipmentID }, ShipmentAdapter));
                }

                RaisePropertyChanged(nameof(OrderLookupShipmentModel));
            }
        }
    }
}

