using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Adapter for Endicia specific shipment information
    /// </summary>
    public class EndiciaShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;
        private readonly ShipmentType shipmentType;
        private readonly ICustomsManager customsManager;

        /// <summary>
        /// Constuctor
        /// </summary>
        public EndiciaShipmentAdapter(ShipmentEntity shipment, IShipmentTypeFactory shipmentTypeFactory, ICustomsManager customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Postal, nameof(shipment.Postal));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Postal.Endicia, nameof(shipment.Postal.Endicia));
            MethodConditions.EnsureArgumentIsNotNull(shipmentTypeFactory, nameof(shipmentTypeFactory));
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));

            this.shipment = shipment;
            this.customsManager = customsManager;
            shipmentType = shipmentTypeFactory.Get(shipment);
        }

        /// <summary>
        /// Id of the Endicia account associated with this shipment
        /// </summary>
        public long? AccountId
        {
            get { return shipment.Postal.Endicia.EndiciaAccountID; }
            set { shipment.Postal.Endicia.EndiciaAccountID = value.GetValueOrDefault(); }
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
                return shipmentType.ShipmentTypeCode;
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
        /// <returns>Dictionary of shipments and exceptions.</returns>
        public IDictionary<ShipmentEntity, Exception> UpdateDynamicData()
        {
            shipmentType.UpdateDynamicShipmentData(shipment);
            shipmentType.UpdateTotalWeight(shipment);

            return customsManager.EnsureCustomsLoaded(new[] { shipment });
        }
    }
}
