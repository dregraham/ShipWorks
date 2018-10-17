﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Model used by the various order lookup viewmodels
    /// </summary>
    [Component(SingleInstance = true)]
    public class OrderLookupShipmentModel : INotifyPropertyChanged, IOrderLookupShipmentModel
    {
        /// <summary>
        /// Entities for which we want to wire up property changed handlers
        /// </summary>
        private readonly static IEnumerable<(Func<ICarrierShipmentAdapter, INotifyPropertyChanged> getEntity, Func<ShipmentTypeCode, bool> isApplicableFor)> eventEntities =
            new (Func<ICarrierShipmentAdapter, INotifyPropertyChanged>, Func<ShipmentTypeCode, bool>)[]
            {
                (x => x?.Shipment, x => true),
                (x => x?.Shipment?.Postal, PostalUtility.IsPostalShipmentType),
                (x => x?.Shipment?.Postal?.Usps, x => x == ShipmentTypeCode.Usps),
                (x => x?.Shipment?.Postal?.Endicia, x => x == ShipmentTypeCode.Endicia),
                (x => x?.Shipment?.Ups, x => x == ShipmentTypeCode.UpsOnLineTools),
                (x => x?.Shipment?.Amazon, x => x == ShipmentTypeCode.Amazon)
            };

        private readonly IMessenger messenger;
        private readonly IShippingManager shippingManager;
        private readonly IMessageHelper messageHelper;
        private readonly PropertyChangedHandler handler;
        private ICarrierShipmentAdapter shipmentAdapter;
        private OrderEntity selectedOrder;
        private bool shipmentAllowEditing;
        private decimal totalCost;
        private bool isSaving = false;
        private IEnumerable<IPackageAdapter> packageAdapters;

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
        public ICarrierShipmentAdapter ShipmentAdapter
        {
            get => shipmentAdapter;
            private set => handler.Set(nameof(ShipmentAdapter), ref shipmentAdapter, value, true);
        }

        /// <summary>
        /// Keep track of the original ShipmentTypeCode so we can ensure its in the list of providers
        /// </summary>
        public ShipmentTypeCode OriginalShipmentTypeCode { get; private set; }

        /// <summary>
        /// The package adapters for the order in context
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<IPackageAdapter> PackageAdapters
        {
            get => packageAdapters;
            private set => handler.Set(nameof(PackageAdapters), ref packageAdapters, value, true);
        }

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

            AddPropertyChangedEventsToEntities(ShipmentAdapter.ShipmentTypeCode);
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
                OriginalShipmentTypeCode = ShipmentAdapter.ShipmentTypeCode;

                // Update dynamic data here because everything downstream will also attempt to update dynamic data
                // doing it here gives us a head start before we are tracking property changes, this also ensures that the
                // shipment date is not in the past
                ShipmentAdapter.UpdateDynamicData();
            }

            using (handler.SuppressChangeNotifications())
            {
                RefreshProperties();
            }

            AddPropertyChangedEventsToEntities(ShipmentAdapter.ShipmentTypeCode);

            RaisePropertyChanged(nameof(ShipmentTypeCode));

            SelectedOrder = order;

            if (ShipmentAdapter != null)
            {
                messenger.Send(new ShipmentSelectionChangedMessage(this, new[] { ShipmentAdapter.Shipment.ShipmentID }, ShipmentAdapter));
            }
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
        private void AddPropertyChangedEventsToEntities(ShipmentTypeCode shipmentTypeCode) =>
            eventEntities
                .Where(x => x.isApplicableFor(shipmentTypeCode))
                .Select(x => x.getEntity(ShipmentAdapter))
                .Where(x => x != null)
                .ForEach(x => x.PropertyChanged += RaisePropertyChanged);

        /// <summary>
        /// Remove property changed events from shipment entities
        /// </summary>
        /// <remarks>
        /// We're purposely removing the property changed handler from ALL shipment type entities because
        /// it's safe to do and because we might not know what the previous shipment type was</remarks>
        private void RemovePropertyChangedEventsFromEntities() =>
            eventEntities
                .Select(x => x.getEntity(ShipmentAdapter))
                .Where(x => x != null)
                .ForEach(x => x.PropertyChanged -= RaisePropertyChanged);

        /// <summary>
        /// Call the RaisePropertyChanged with PropertyName
        /// </summary>
        private void RaisePropertyChanged(object sender, PropertyChangedEventArgs e) => RaisePropertyChanged(e.PropertyName);

        /// <summary>
        /// Change the provider of the shipment
        /// </summary>
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

                    AddPropertyChangedEventsToEntities(value);
                }

                RaisePropertyChanged(nameof(OrderLookupShipmentModel));

                messenger.Send(new ShipmentSelectionChangedMessage(this, new[] { ShipmentAdapter.Shipment.ShipmentID }, ShipmentAdapter));
            }
        }
    }
}
