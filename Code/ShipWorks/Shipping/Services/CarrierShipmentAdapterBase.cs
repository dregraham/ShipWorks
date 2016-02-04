using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Base class for ICarrierShipmentAdapter that implements code that's the same across all shipment adapters
    /// </summary>
    public abstract class CarrierShipmentAdapterBase : ICarrierShipmentAdapter
    {
        private ShipmentEntity shipment;
        private ShipmentType shipmentType;
        private ICustomsManager customsManager;
        private EntityCollection<ShipmentCustomsItemEntity> customsItems;

        /// <summary>
        /// Constructor
        /// </summary>
        protected CarrierShipmentAdapterBase(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager, ICustomsManager customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipmentTypeManager, nameof(shipmentTypeManager));

            this.shipment = shipment;
            this.customsManager = customsManager;
            shipmentType = shipmentTypeManager.Get(shipment);
        }

        /// <summary>
        /// Id of the carrier account
        /// </summary>
        public abstract long? AccountId { get; set; }

        /// <summary>
        /// The shipment associated with this adapter
        /// </summary>
        public ShipmentEntity Shipment
        {
            get
            {
                return shipment;
            }
        }

        /// <summary>
        /// The shipment type code of this shipment adapter
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode
        {
            get
            {
                return shipment.ShipmentTypeCode;
            }
        }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        public abstract bool SupportsAccounts { get; }

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
                return shipmentType.IsDomestic(shipment);
            }
        }

        /// <summary>
        /// Updates shipment dynamic data, total weight, etc
        /// </summary>
        /// <returns>Dictionary of shipments and exceptions.</returns>
        public virtual IDictionary<ShipmentEntity, Exception> UpdateDynamicData()
        {
            if (!shipment.Processed)
            {
                shipmentType.UpdateDynamicShipmentData(shipment);
                shipmentType.UpdateTotalWeight(shipment);
            }

            if (customsManager != null)
            {
                return customsManager.EnsureCustomsLoaded(new[] { shipment });
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
            get { return shipment.ShipDate; }
            set { shipment.ShipDate = value; }
        }

        /// <summary>
        /// Total weight of the shipment
        /// </summary>
        public double TotalWeight
        {
            get { return shipment.TotalWeight; }
        }

        /// <summary>
        /// Content weight of the shipment
        /// </summary>
        public double ContentWeight
        {
            get { return shipment.ContentWeight; }
            set { shipment.ContentWeight = value; }
        }

        /// <summary>
        /// Service type selected
        /// </summary>
        public abstract int ServiceType { get; set; }

        /// <summary>
        /// The shipment's customs items
        /// </summary>
        public virtual EntityCollection<ShipmentCustomsItemEntity> CustomsItems
        {
            get
            {
                if (customsItems == null || !customsItems.Any() || !shipment.CustomsGenerated)
                {
                    UpdateDynamicData();
                    customsItems = shipment.CustomsItems;
                }

                return customsItems;
            }
            set
            {
                customsItems = value;

                if (customsItems == null)
                {
                    return;
                }

                try
                {
                    List<ShipmentCustomsItemEntity> objectIDsToAdd =
                    customsItems.Where(ci => ci.IsNew)
                        .Select(ci => ci.ObjectID)
                        .Except(shipment.CustomsItems.Select(ci => ci.ObjectID))
                        .Select(ci => customsItems.First(sci => sci.ObjectID == ci))
                        .ToList();

                    List<Guid> objectIDsToDelete =
                        shipment.CustomsItems.Select(ci => ci.ObjectID)
                            .Except(customsItems.Select(ci => ci.ObjectID))
                            .ToList();

                    UpdateCustomsItemsForShipment(objectIDsToAdd, objectIDsToDelete);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Add/Remove customs items
        /// </summary>
        private void UpdateCustomsItemsForShipment(List<ShipmentCustomsItemEntity> objectIDsToAdd, List<Guid> objectIDsToDelete)
        {
            foreach (ShipmentCustomsItemEntity sci in objectIDsToAdd)
            {
                sci.Shipment = Shipment;
            }

            foreach (Guid objectID in objectIDsToDelete)
            {
                ShipmentCustomsItemEntity sci = shipment.CustomsItems.First(ci => ci.ObjectID == objectID);
                shipment.CustomsItems.Remove(sci);
            }

            for (int i = 0; i < customsItems.Count; i++)
            {
                ShipmentCustomsItemEntity newShipmentCustomsItem = customsItems[i];
                ShipmentCustomsItemEntity origShipmentCustomsItem = shipment.CustomsItems[i];
                origShipmentCustomsItem.CountryOfOrigin = newShipmentCustomsItem.CountryOfOrigin;
                origShipmentCustomsItem.Description = newShipmentCustomsItem.Description;
                origShipmentCustomsItem.HarmonizedCode = newShipmentCustomsItem.HarmonizedCode;
                origShipmentCustomsItem.NumberOfPieces = newShipmentCustomsItem.NumberOfPieces;
                origShipmentCustomsItem.Quantity = newShipmentCustomsItem.Quantity;
                origShipmentCustomsItem.ShipmentCustomsItemID = newShipmentCustomsItem.ShipmentCustomsItemID;
                origShipmentCustomsItem.UnitPriceAmount = newShipmentCustomsItem.UnitPriceAmount;
                origShipmentCustomsItem.UnitValue = newShipmentCustomsItem.UnitValue;
                origShipmentCustomsItem.Weight = newShipmentCustomsItem.Weight;
            }
        }

        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        public IEnumerable<IPackageAdapter> GetPackageAdapters()
        {
            if (!shipment.Processed)
            {
                UpdateDynamicData();
            }

            return shipmentType.GetPackageAdapters(shipment);
        }

        /// <summary>
        /// Gets specific number of package adapters for the shipment.
        /// </summary>
        public abstract IEnumerable<IPackageAdapter> GetPackageAdapters(int numberOfPackages);

        /// <summary>
        /// Add a new package
        /// </summary>
        public virtual IPackageAdapter AddPackage()
        {
            throw new InvalidOperationException($"Adding a package is not supported");
        }

        /// <summary>
        /// Delete a package
        /// </summary>
        public virtual void DeletePackage(IPackageAdapter package)
        {
            throw new InvalidOperationException($"Deleting a package is not supported");
        }

        /// <summary>
        /// Are customs allowed?
        /// </summary>
        public virtual bool CustomsAllowed
        {
            get { return !shipmentType.IsDomestic(shipment); }
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
                rate.ShipmentType == ShipmentTypeCode)
            {
                UpdateServiceFromRate(rate);
            }
        }

        /// <summary>
        /// Perform the service update
        /// </summary>
        protected virtual void UpdateServiceFromRate(RateResult rate)
        {
            // Setting the service from a rate is carrier specific, but this is not abstract because a few
            // shipment types don't support rating
        }
    }
}
