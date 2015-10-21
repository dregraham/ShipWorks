using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// Adapter for Ups specific shipment information
    /// </summary>
    public class UpsShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;
        private readonly UpsShipmentType shipmentType;
        private readonly ICustomsManager customsManager;

        /// <summary>
        /// Constuctor
        /// </summary>
        public UpsShipmentAdapter(ShipmentEntity shipment, IShipmentTypeFactory shipmentTypeFactory, ICustomsManager customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Ups, nameof(shipment.Ups));
            MethodConditions.EnsureArgumentIsNotNull(shipmentTypeFactory, nameof(shipmentTypeFactory));
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));

            this.shipment = shipment;
            this.customsManager = customsManager;
            shipmentType = shipmentTypeFactory.Get(shipment) as UpsShipmentType;
        }

        /// <summary>
        /// Id of the Ups account associated with this shipment
        /// </summary>
        public long? AccountId
        {
            get { return shipment.Ups.UpsAccountID; }
            set { shipment.Ups.UpsAccountID = value.GetValueOrDefault(); }
        }

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
                return ShipmentTypeCode.UpsOnLineTools;
            }
        }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        public bool SupportsAccounts
        {
            get
            {
                return true;
            }
        }

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
        /// <param name="validatedAddressScope"></param>
        /// <returns>Dictionary of shipments and exceptions.</returns>
        public IDictionary<ShipmentEntity, Exception> UpdateDynamicData(ValidatedAddressScope validatedAddressScope)
        {
            shipmentType.UpdateDynamicShipmentData(shipment);
            shipmentType.UpdateTotalWeight(shipment);

            IDictionary<ShipmentEntity, Exception> errors = customsManager.EnsureCustomsLoaded(new[] { shipment }, validatedAddressScope);

            return errors;
        }
    }
}
