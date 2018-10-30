using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.OrderLookup.ShipmentModelPipelines;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
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
        /// <remarks>
        /// I've included all known shipment types (as of October, 2018) so that we don't run into issues when
        /// adding carriers to the order lookup view.
        /// </remarks>
        private readonly static IEnumerable<(Func<ICarrierShipmentAdapter, IEnumerable<INotifyPropertyChanged>> getEntity, Func<ShipmentTypeCode, bool> shouldAttach)> eventEntities =
            new (Func<ICarrierShipmentAdapter, IEnumerable<INotifyPropertyChanged>>, Func<ShipmentTypeCode, bool>)[]
            {
                RegisterEventEntity(x => x?.Shipment, x => true),
                RegisterEventEntity(x => x?.Shipment?.Amazon, x => x == ShipmentTypeCode.Amazon),
                RegisterEventEntity(x => x?.Shipment?.Asendia, x => x == ShipmentTypeCode.Asendia),
                RegisterEventEntity(x => x?.Shipment?.BestRate, x => x == ShipmentTypeCode.BestRate),
                RegisterEventEntity(x => x?.Shipment?.DhlExpress, x => x == ShipmentTypeCode.DhlExpress),
                RegisterEventEntity(x => x?.Shipment?.FedEx, x => x == ShipmentTypeCode.FedEx),
                RegisterEventEntities(x => x?.Shipment?.FedEx?.Packages, x => x == ShipmentTypeCode.FedEx),
                RegisterEventEntity(x => x?.Shipment?.IParcel, x => x == ShipmentTypeCode.iParcel),
                RegisterEventEntities(x => x?.Shipment?.IParcel?.Packages, x => x == ShipmentTypeCode.iParcel),
                RegisterEventEntity(x => x?.Shipment?.OnTrac, x => x == ShipmentTypeCode.OnTrac),
                RegisterEventEntity(x => x?.Shipment?.Other, x => x == ShipmentTypeCode.Other),
                RegisterEventEntity(x => x?.Shipment?.Postal, PostalUtility.IsPostalShipmentType),
                RegisterEventEntity(x => x?.Shipment?.Postal?.Usps, x => x == ShipmentTypeCode.Usps || x == ShipmentTypeCode.Express1Usps),
                RegisterEventEntity(x => x?.Shipment?.Postal?.Endicia, x => x == ShipmentTypeCode.Endicia || x == ShipmentTypeCode.Express1Endicia),
                RegisterEventEntity(x => x?.Shipment?.Ups, x => x == ShipmentTypeCode.UpsOnLineTools || x == ShipmentTypeCode.UpsWorldShip),
                RegisterEventEntities(x => x?.Shipment?.Ups?.Packages, x => x == ShipmentTypeCode.UpsOnLineTools || x == ShipmentTypeCode.UpsWorldShip)
            };

        /// <summary>
        /// Register an entity that can have property events wired
        /// </summary>
        private static (Func<ICarrierShipmentAdapter, IEnumerable<T>>, Func<ShipmentTypeCode, bool>) RegisterEventEntity<T>(Func<ICarrierShipmentAdapter, T> getEntity, Func<ShipmentTypeCode, bool> shouldAttach) where T : INotifyPropertyChanged =>
            RegisterEventEntities(x => new[] { getEntity(x) }, shouldAttach);

        /// <summary>
        /// Register entities that can have property events wired
        /// </summary>
        private static (Func<ICarrierShipmentAdapter, IEnumerable<T>>, Func<ShipmentTypeCode, bool>) RegisterEventEntities<T>(Func<ICarrierShipmentAdapter, IEnumerable<T>> getEntity, Func<ShipmentTypeCode, bool> shouldAttach) where T : INotifyPropertyChanged
        {
            registeredEventEntityTypes = (registeredEventEntityTypes ?? ImmutableHashSet<Type>.Empty).Add(typeof(T));

            return (getEntity, shouldAttach);
        }

        private static ImmutableHashSet<Type> registeredEventEntityTypes;

        private readonly IMessenger messenger;
        private readonly IShippingManager shippingManager;
        private readonly IMessageHelper messageHelper;
        private readonly Func<IInsuranceBehaviorChangeViewModel> createInsuranceBehaviorChange;
        private readonly IShippingProfileService profileService;
        private readonly PropertyChangedHandler handler;
        private readonly IDisposable subscription;
        private ICarrierShipmentAdapter shipmentAdapter;
        private OrderEntity selectedOrder;
        private bool shipmentAllowEditing;
        private decimal totalCost;
        private bool isSaving = false;
        private IEnumerable<IPackageAdapter> packageAdapters;
        private IDisposable profileDisposable;

        public event EventHandler OnSearchOrder;

        /// <summary>
        /// A shipment is starting to unload
        /// </summary>
        public event EventHandler ShipmentUnloading;

        /// <summary>
        /// A shipment needs binding
        /// </summary>
        public event EventHandler ShipmentNeedsBinding;

        /// <summary>
        /// A shipment is starting to load
        /// </summary>
        public event EventHandler ShipmentLoading;

        /// <summary>
        /// A shipment has fully loaded
        /// </summary>
        public event EventHandler ShipmentLoaded;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupShipmentModel(
            IMessenger messenger,
            IShippingManager shippingManager,
            IMessageHelper messageHelper,
            Func<IInsuranceBehaviorChangeViewModel> createInsuranceBehaviorChange,
            IShippingProfileService profileService,
            IEnumerable<IOrderLookupShipmentModelPipeline> pipelines)
        {
            this.profileService = profileService;
            this.messenger = messenger;
            this.shippingManager = shippingManager;
            this.messageHelper = messageHelper;
            this.createInsuranceBehaviorChange = createInsuranceBehaviorChange;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            subscription = new CompositeDisposable(pipelines.Select(x => x.Register(this)).ToArray());
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
        /// Selected rate that should be used if processing requires it
        /// </summary>
        public RateResult SelectedRate { get; set; }

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
            using (ShipmentAdapter?.Shipment.BatchPropertyChangeNotificationsOnGraph())
            {
                handler.RaisePropertyChanged(propertyName);

                if (ShipmentAdapter?.Shipment != null)
                {
                    messenger.Send(new ShipmentChangedMessage(this, ShipmentAdapter, propertyName, RemovePropertyChangedEventsFromEntities));
                }
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
            RemovePropertyChangedEventsFromEntities(ShipmentAdapter);

            shippingManager.RefreshShipment(ShipmentAdapter.Shipment);
            ShipmentAdapter = shippingManager.GetShipmentAdapter(ShipmentAdapter.Shipment);

            RefreshProperties();

            AddPropertyChangedEventsToEntities(ShipmentAdapter, ShipmentAdapter.ShipmentTypeCode);
            RaisePropertyChanged(null);
        }

        /// <summary>
        /// Unload the order
        /// </summary>
        public void Unload() => Unload(OrderClearReason.Reset);

        /// <summary>
        /// Unload the order
        /// </summary>
        public void Unload(OrderClearReason clearReason)
        {
            SaveToDatabase();
            ClearOrder(clearReason);
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
            ShipmentLoading?.Invoke(this, EventArgs.Empty);

            OnSearchOrder?.Invoke(this, null);

            if (order == null)
            {
                ClearOrder(OrderClearReason.OrderNotFound);
                return;
            }

            RemovePropertyChangedEventsFromEntities(ShipmentAdapter);

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

            AddPropertyChangedEventsToEntities(ShipmentAdapter, ShipmentAdapter.ShipmentTypeCode);

            RaisePropertyChanged(nameof(ShipmentTypeCode));

            SelectedOrder = order;

            if (ShipmentAdapter != null)
            {
                ShipmentLoaded?.Invoke(this, EventArgs.Empty);
                messenger.Send(new ShipmentSelectionChangedMessage(this, new[] { ShipmentAdapter.Shipment.ShipmentID }, ShipmentAdapter));
            }
        }

        /// <summary>
        /// Wire a property changed event on an INotifyPropertyChanged object
        /// </summary>
        public void WirePropertyChangedEvent(INotifyPropertyChanged eventObject)
        {
            if (eventObject == null)
            {
                return;
            }

            if (!registeredEventEntityTypes.Contains(eventObject.GetType()))
            {
                throw new InvalidOperationException($"Cannot wire events for {eventObject.GetType()} because it is not an entity type that will be unwired");
            }

            eventObject.PropertyChanged += RaisePropertyChanged;
        }

        /// <summary>
        /// Unwire property changed event on an INotifyPropertyChanged object
        /// </summary>
        public void UnwirePropertyChangedEvent(INotifyPropertyChanged eventObject)
        {
            if (eventObject == null)
            {
                return;
            }

            eventObject.PropertyChanged -= RaisePropertyChanged;
        }

        /// <summary>
        /// Clear the order
        /// </summary>
        private void ClearOrder(OrderClearReason reason)
        {
            ShipmentUnloading?.Invoke(this, EventArgs.Empty);
            RemovePropertyChangedEventsFromEntities(ShipmentAdapter);

            ShipmentAdapter = null;
            ShipmentAllowEditing = false;
            PackageAdapters = null;
            SelectedOrder = null;
            SelectedRate = null;
            TotalCost = 0;

            messenger.Send(new OrderLookupClearOrderMessage(this, reason));
        }

        /// <summary>
        /// Refresh properties from the given order
        /// </summary>
        private void RefreshProperties()
        {
            ShipmentAllowEditing = !ShipmentAdapter?.Shipment?.Processed ?? false;
            PackageAdapters = ShipmentAdapter?.GetPackageAdaptersAndEnsureShipmentIsLoaded();

            TotalCost = ShipmentAdapter?.Shipment?.ShipmentCost ?? 0;
        }

        /// <summary>
        /// Add property change event handlers
        /// </summary>
        private void AddPropertyChangedEventsToEntities(ICarrierShipmentAdapter adapter, ShipmentTypeCode shipmentTypeCode) =>
            eventEntities
                .Where(x => x.shouldAttach(shipmentTypeCode))
                .Select(x => x.getEntity(adapter))
                .SelectMany(x => x ?? Enumerable.Empty<INotifyPropertyChanged>())
                .ForEach(WirePropertyChangedEvent);

        /// <summary>
        /// Remove property changed events from shipment entities
        /// </summary>
        /// <remarks>
        /// We're purposely removing the property changed handler from ALL shipment type entities because
        /// it's safe to do and because we might not know what the previous shipment type was</remarks>
        private void RemovePropertyChangedEventsFromEntities(ICarrierShipmentAdapter adapter) =>
            eventEntities
                .Select(x => x.getEntity(adapter))
                .SelectMany(x => x ?? Enumerable.Empty<INotifyPropertyChanged>())
                .ForEach(UnwirePropertyChangedEvent);

        /// <summary>
        /// Call the RaisePropertyChanged with PropertyName
        /// </summary>
        private void RaisePropertyChanged(object sender, PropertyChangedEventArgs e) => RaisePropertyChanged(e.PropertyName);

        /// <summary>
        /// Change the provider of the shipment
        /// </summary>
        public void ChangeShipmentType(ShipmentTypeCode value)
        {
            SelectedRate = null;

            if (value != ShipmentAdapter.ShipmentTypeCode)
            {
                ModifyShipmentWithReload(() =>
                {
                    var adapter = shippingManager.ChangeShipmentType(value, ShipmentAdapter.Shipment);
                    adapter.UpdateDynamicData();
                    return adapter;
                });
            }
        }

        /// <summary>
        /// Create the label for a shipment
        /// </summary>
        public void CreateLabel()
        {
            if (!ShipmentAllowEditing || (shipmentAdapter?.Shipment?.Processed ?? true))
            {
                return;
            }

            SaveToDatabase();

            messenger.Send(new ProcessShipmentsMessage(this, new[] { shipmentAdapter.Shipment },
                new[] { shipmentAdapter.Shipment }, SelectedRate));
        }

        /// <summary>
        /// Register the profile handler
        /// </summary>
        public void RegisterProfileHandler(Func<Func<ShipmentTypeCode?>, Action<IShippingProfileEntity>, IDisposable> profileRegistration) =>
            profileDisposable = profileRegistration(() => ShipmentAdapter?.ShipmentTypeCode, x => ApplyProfile(x.ShippingProfileID));

        /// <summary>
        /// Apply the profile to the current shipment
        /// </summary>
        public bool ApplyProfile(long profileID)
        {
            var profile = profileService.Get(profileID);
            if (ShipmentAdapter?.Shipment != null && profile.IsApplicable(ShipmentAdapter?.Shipment?.ShipmentTypeCode))
            {
                ShipmentNeedsBinding?.Invoke(this, EventArgs.Empty);

                var originalShipment = EntityUtility.CloneEntity(ShipmentAdapter.Shipment, true);

                ModifyShipmentWithReload(() => profile.Apply(ShipmentAdapter.Shipment));

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Modify the shipment going through the reload process to ensure that major changes (like provider) can be reflected
        /// </summary>
        private void ModifyShipmentWithReload(Func<ICarrierShipmentAdapter> performModification)
        {
            ShipmentLoading?.Invoke(this, EventArgs.Empty);

            // Changing shipment type leads to unloading and loading entities into the current ShipmentEntity.
            // To prepare for this, we remove existing handlers from the existing entities, change the shipment type,
            // then add handlers to the possibly new entities.
            using (handler.SuppressChangeNotifications())
            {
                RemovePropertyChangedEventsFromEntities(ShipmentAdapter);

                bool originalInsuranceSelection = shipmentAdapter.Shipment.Insurance;
                ShipmentAdapter = performModification();

                createInsuranceBehaviorChange().Notify(originalInsuranceSelection, shipmentAdapter.Shipment.Insurance);

                RefreshProperties();

                AddPropertyChangedEventsToEntities(ShipmentAdapter, ShipmentAdapter.ShipmentTypeCode);
            }

            RaisePropertyChanged(nameof(OrderLookupShipmentModel));

            ShipmentLoaded?.Invoke(this, EventArgs.Empty);
            messenger.Send(new ShipmentSelectionChangedMessage(this, new[] { ShipmentAdapter.Shipment.ShipmentID }, ShipmentAdapter));
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            subscription.Dispose();
            profileDisposable?.Dispose();
        }
    }
}
