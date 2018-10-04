using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Orchestrates the various view models associated with order lookup mode
    /// </summary>
    [Component(SingleInstance = true)]
    public class ViewModelOrchestrator : IInitializeForCurrentUISession, INotifyPropertyChanged, IViewModelOrchestrator
    {
        private readonly IMessenger messenger;
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private readonly IShippingManager shippingManager;
        private readonly IMessageHelper messageHelper;
        private IDisposable subscription;
        private readonly PropertyChangedHandler handler;
        private OrderEntity selectedOrder;
        private bool shipmentAllowEditing;

        /// <summary>
        /// Constructor
        /// </summary>
        public ViewModelOrchestrator(IMessenger messenger, ICarrierShipmentAdapterFactory shipmentAdapterFactory, IShippingManager shippingManager, IMessageHelper messageHelper)
        {
            this.messenger = messenger;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
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
        /// ShipmentType of selected shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentTypeCode ShipmentTypeCode
        {
            get => ShipmentAdapter?.ShipmentTypeCode ?? ShipmentTypeCode.None;
            set
            {
                if (value != ShipmentTypeCode)
                {
                    SaveToDatabase();
                    using (handler.SuppressChangeNotifications())
                    {
                        RemovePropertyChangedEventsFromEntities();

                        shippingManager.ChangeShipmentType(value, ShipmentAdapter.Shipment);

                        RefreshPropertiesFromOrder(selectedOrder);

                        AddPropertyChangedEventsToEntities();

                        if (ShipmentAdapter != null)
                        {
                            messenger.Send(new ShipmentSelectionChangedMessage(this, new[] { ShipmentAdapter.Shipment.ShipmentID }, ShipmentAdapter));
                        }
                    }
                    RaisePropertyChanged(nameof(ViewModelOrchestrator));
                }
            }
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
            if (ShipmentAdapter == null || !ShipmentAllowEditing || (ShipmentAdapter?.Shipment?.Processed ?? true))
            {
                return;
            }

            IDictionary<ShipmentEntity, Exception> errors;
            using (handler.SuppressChangeNotifications())
            {
                errors = shippingManager.SaveShipmentToDatabase(ShipmentAdapter?.Shipment, false);
            }

            DisplayError(errors);
        }

        /// <summary>
        /// Refresh the shipment from the database
        /// </summary>
        public void RefreshShipmentFromDatabase()
        {
            RemovePropertyChangedEventsFromEntities();

            shippingManager.RefreshShipment(ShipmentAdapter.Shipment);

            RefreshPropertiesFromOrder(SelectedOrder);
            AddPropertyChangedEventsToEntities();
            RaisePropertyChanged(null);
        }

        /// <summary>
        /// Unload the order
        /// </summary>
        public void Unload() => LoadOrder(null);

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

                messenger.Send(new OrderSelectionChangingMessage(this, new[] { ShipmentAdapter.Shipment.OrderID }));
            }
        }

        /// <summary>
        /// Start listening for order found messages
        /// </summary>
        public void InitializeForCurrentSession()
        {
            subscription = messenger.OfType<OrderLookupSingleScanMessage>()
                .Subscribe(orderMessage => LoadOrder(orderMessage.Order));
        }

        /// <summary>
        /// Load an order
        /// </summary>
        private void LoadOrder(OrderEntity order)
        {
            SaveToDatabase();
            RemovePropertyChangedEventsFromEntities();

            if (order != null)
            {
                RefreshPropertiesFromOrder(order);

                AddPropertyChangedEventsToEntities();

                if (ShipmentAdapter != null)
                {
                    messenger.Send(new ShipmentSelectionChangedMessage(this, new[] { ShipmentAdapter.Shipment.ShipmentID }, ShipmentAdapter));
                }

                RaisePropertyChanged(nameof(ShipmentTypeCode));
            }
            else
            {
                ShipmentAdapter = null;
                ShipmentAllowEditing = false;
                PackageAdapters = null;
            }

            SelectedOrder = order;
        }

        /// <summary>
        /// Refresh properties from the given order
        /// </summary>
        private void RefreshPropertiesFromOrder(OrderEntity order)
        {
            if (order?.Shipments != null)
            {
                ShipmentAdapter = shipmentAdapterFactory.Get(order.Shipments.First());
                ShipmentAllowEditing = !ShipmentAdapter?.Shipment?.Processed ?? false;
                PackageAdapters = ShipmentAdapter?.GetPackageAdapters();
            }     
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

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            subscription?.Dispose();
        }

        /// <summary>
        /// Stop listening for order found messages
        /// </summary>
        public void EndSession()
        {
            Dispose();
        }
    }
}
