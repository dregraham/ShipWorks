using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.Routing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using Interapptive.Shared.Utility;

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
        /// Constuctor
        /// </summary>
        protected CarrierShipmentAdapterBase(ShipmentEntity shipment, IShipmentTypeFactory shipmentTypeFactory, ICustomsManager customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipmentTypeFactory, nameof(shipmentTypeFactory));

            this.shipment = shipment;
            this.customsManager = customsManager;
            shipmentType = shipmentTypeFactory.Get(shipment);
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
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
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
        /// Are customs allowed?
        /// </summary>
        public virtual bool CustomsAllowed
        {
            get { return !shipmentType.IsDomestic(shipment); }
        }
    }
}
