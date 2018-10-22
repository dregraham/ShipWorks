using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Base class for ICarrierShipmentAdapter that implements code that's the same across all shipment adapters
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public abstract class CarrierShipmentAdapterBase : ICarrierShipmentAdapter
    {
        private ShipmentType shipmentType;
        private ICustomsManager customsManager;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Copy constructor
        /// </summary>
        protected CarrierShipmentAdapterBase(CarrierShipmentAdapterBase adapterToCopy)
        {
            Shipment = EntityUtility.CloneEntity(adapterToCopy.Shipment, true);
            customsManager = adapterToCopy.customsManager;
            shipmentType = adapterToCopy.shipmentType;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected CarrierShipmentAdapterBase(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager,
            ICustomsManager customsManager, IStoreManager storeManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipmentTypeManager, nameof(shipmentTypeManager));

            Shipment = shipment;
            this.customsManager = customsManager;
            this.storeManager = storeManager;
            shipmentType = shipmentTypeManager.Get(shipment);
        }

        /// <summary>
        /// Id of the carrier account
        /// </summary>
        public abstract long? AccountId { get; set; }

        /// <summary>
        /// The shipment associated with this adapter
        /// </summary>
        public ShipmentEntity Shipment { get; }

        /// <summary>
        /// The store associated with the shipment
        /// </summary>
        public StoreEntity Store
        {
            get { return Shipment.Order?.Store ?? storeManager.GetRelatedStore(Shipment); }
        }

        /// <summary>
        /// The shipment type code of this shipment adapter
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode
        {
            get
            {
                return Shipment.ShipmentTypeCode;
            }
        }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        public abstract bool SupportsAccounts { get; }

        /// <summary>
        /// Does this shipment support rate shopping?
        /// </summary>
        public virtual bool SupportsRateShopping { get; } = false;

        /// <summary>
        /// Does this shipment type support multiple packages?
        /// </summary>
        public bool SupportsMultiplePackages
        {
            get
            {
                return shipmentType.SupportsMultiplePackages;
            }
        }

        /// <summary>
        /// Is this shipment a domestic shipment?
        /// </summary>
        public bool IsDomestic
        {
            get
            {
                return shipmentType.IsDomestic(Shipment);
            }
        }

        /// <summary>
        /// Updates shipment dynamic data, total weight, etc
        /// </summary>
        /// <returns>Dictionary of shipments and exceptions.</returns>
        public virtual IDictionary<ShipmentEntity, Exception> UpdateDynamicData()
        {
            if (!Shipment.Processed)
            {
                shipmentType.UpdateTotalWeight(Shipment);
                shipmentType.UpdateDynamicShipmentData(Shipment);
            }

            if (customsManager != null)
            {
                return customsManager.EnsureCustomsLoaded(new[] { Shipment });
            }

            return new Dictionary<ShipmentEntity, Exception>();
        }

        /// <summary>
        /// Does this shipment type support package Types?
        /// </summary>
        public abstract bool SupportsPackageTypes { get; }

        /// <summary>
        /// DateTime of the shipment
        /// </summary>
        public DateTime ShipDate
        {
            get { return Shipment.ShipDate; }
            set { Shipment.ShipDate = value; }
        }

        /// <summary>
        /// Total weight of the shipment
        /// </summary>
        public double TotalWeight
        {
            get { return Shipment.TotalWeight; }
        }

        /// <summary>
        /// Content weight of the shipment
        /// </summary>
        public double ContentWeight
        {
            get { return Shipment.ContentWeight; }
            set { Shipment.ContentWeight = value; }
        }

        /// <summary>
        /// Service type selected
        /// </summary>
        public abstract int ServiceType { get; set; }

        /// <summary>
        /// Service type name
        /// </summary>
        public abstract string ServiceTypeName { get; }

        /// <summary>
        /// Clone the shipment adapter and shipment
        /// </summary>
        public abstract ICarrierShipmentAdapter Clone();

        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        public IEnumerable<IPackageAdapter> GetPackageAdapters() =>
            shipmentType.GetPackageAdapters(Shipment);

        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        /// <remarks>
        /// This version of GetPackageAdapters calls EnsureShipmentLoaded which will most likely trigger a save to the database.
        /// Use the other version of this method if you don't want or need that behavior.</remarks>
        public IEnumerable<IPackageAdapter> GetPackageAdaptersAndEnsureShipmentIsLoaded()
        {
            if (!Shipment.Processed)
            {
                UpdateDynamicData();
            }

            return GetPackageAdapters();
        }

        /// <summary>
        /// Add a new package
        /// </summary>
        public IPackageAdapter AddPackage() => AddPackage(null);

        /// <summary>
        /// Add a new package
        /// </summary>
        /// <param name="manipulateEntity">
        /// Pass in an action to manipulate the package that gets added to the shipment
        /// </param>
        public virtual IPackageAdapter AddPackage(Action<INotifyPropertyChanged> manipulateEntity)
        {
            throw new InvalidOperationException($"Adding a package is not supported");
        }

        /// <summary>
        /// Delete a package
        /// </summary>
        public void DeletePackage(IPackageAdapter package) => DeletePackage(package, null);

        /// <summary>
        /// Delete a package
        /// </summary>
        /// <param name="manipulateEntity">
        /// Pass in an action to manipulate the package that gets added to the shipment
        /// </param>
        public virtual void DeletePackage(IPackageAdapter package, Action<INotifyPropertyChanged> manipulateEntity)
        {
            throw new InvalidOperationException($"Deleting a package is not supported");
        }

        /// <summary>
        /// Are customs allowed?
        /// </summary>
        public virtual bool CustomsAllowed
        {
            get { return !shipmentType.IsDomestic(Shipment); }
        }

        /// <summary>
        /// Update the insurance fields on the shipment and packages
        /// </summary>
        public abstract void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings);

        /// <summary>
        /// Select the service from the given rate result
        /// </summary>
        public void SelectServiceFromRate(RateResult rate)
        {
            if (shipmentType.SupportsGetRates &&
                rate != null &&
                rate.Selectable &&
                IsCompatibleShipmentType(rate.ShipmentType))
            {
                UpdateServiceFromRate(rate);
            }
        }

        /// <summary>
        /// Does the given rate match the service selected for the shipment
        /// </summary>
        public virtual bool DoesRateMatchSelectedService(RateResult rate)
        {
            if (ShipmentTypeCode != rate.ShipmentType)
            {
                return false;
            }

            int? service = GetServiceTypeAsIntFromTag(rate.Tag);
            return service.HasValue && service.Value == ServiceType;
        }

        /// <summary>
        /// For rates that are not selectable, find their first child that is.
        /// </summary>
        public virtual RateResult GetChildRateForRate(RateResult parentRate, IEnumerable<RateResult> rates)
        {
            return parentRate;
        }

        /// <summary>
        /// Is the given shipment type compatible with the current shipment type
        /// </summary>
        protected virtual bool IsCompatibleShipmentType(ShipmentTypeCode shipmentType)
        {
            return shipmentType == ShipmentTypeCode;
        }

        /// <summary>
        /// Get the service type as an integer from the given tag
        /// </summary>
        protected virtual int? GetServiceTypeAsIntFromTag(object tag)
        {
            return tag as int?;
        }

        /// <summary>
        /// Perform the service update
        /// </summary>
        protected virtual void UpdateServiceFromRate(RateResult rate)
        {
            // Setting the service from a rate is carrier specific, but this is not abstract because a few
            // shipment types don't support rating
        }

        /// <summary>
        /// Delete a package from the shipment
        /// </summary>
        /// <param name="manipulateEntity">
        /// Pass in an action to manipulate the package that gets deleted from the shipment
        /// </param>
        protected void DeletePackageFromCollection<TPackage>(EntityCollection<TPackage> packageCollection,
            Func<TPackage, bool> packagePredicate, Action<INotifyPropertyChanged> manipulateEntityBeforeDelete) where TPackage : EntityBase2
        {
            if (packageCollection.Count < 2)
            {
                return;
            }

            TPackage package = packageCollection.FirstOrDefault(packagePredicate);
            
            if (package != null)
            {
                manipulateEntityBeforeDelete?.Invoke(package);

                // If this isn't set, removing packages won't actually remove them from the database
                if (packageCollection.RemovedEntitiesTracker == null)
                {
                    packageCollection.RemovedEntitiesTracker = new EntityCollection<TPackage>();
                }

                packageCollection.Remove(package);
                UpdateDynamicData();
            }
        }

        /// <summary>
        /// Get a list of customs item adapters for this shipment
        /// </summary>
        public virtual IEnumerable<IShipmentCustomsItemAdapter> GetCustomsItemAdapters()
        {
            if (!Shipment.Processed)
            {
                UpdateDynamicData();
            }

            return Shipment.CustomsItems
                .Select(x => new ShipmentCustomsItemAdapter(x))
                .ToReadOnly();
        }

        /// <summary>
        /// Add a new customs item
        /// </summary>
        public IShipmentCustomsItemAdapter AddCustomsItem()
        {
            ShipmentCustomsItemEntity shipmentCustomsItemEntity = customsManager.CreateCustomsItem(Shipment);
            UpdateDynamicData();

            return new ShipmentCustomsItemAdapter(shipmentCustomsItemEntity);
        }

        /// <summary>
        /// Delete a customs item from the shipment
        /// </summary>
        public void DeleteCustomsItem(IShipmentCustomsItemAdapter customsItem)
        {
            ShipmentCustomsItemEntity existingItem = Shipment.CustomsItems
                .FirstOrDefault(x => x.ShipmentCustomsItemID == customsItem.ShipmentCustomsItemID);

            if (existingItem == null)
            {
                return;
            }

            // If this isn't set, removing packages won't actually remove them from the database
            if (Shipment.CustomsItems.RemovedEntitiesTracker == null)
            {
                Shipment.CustomsItems.RemovedEntitiesTracker = new EntityCollection<ShipmentCustomsItemEntity>();
            }

            Shipment.CustomsItems.Remove(existingItem);
            UpdateDynamicData();
        }

        /// <summary>
        /// Send a notification if service related properties change
        /// </summary>
        public virtual IDisposable NotifyIfServiceRelatedPropertiesChange(Action<string> raisePropertyChanged) =>
            Disposable.Empty;

        /// <summary>
        /// Update the total weight of the shipment based on its ContentWeight and any packaging weight.
        /// </summary>
        public void UpdateTotalWeight()
        {
            if (Shipment.Processed)
            {
                shipmentType.UpdateTotalWeight(Shipment);
            }
        }

        /// <summary>
        /// Get a strongly typed ShipmentType
        /// </summary>
        protected T GetShipmentType<T>() where T : ShipmentType => shipmentType as T;
    }
}
