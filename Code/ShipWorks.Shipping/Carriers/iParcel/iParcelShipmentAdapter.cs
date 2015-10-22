using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Adapter for specific shipment information
    /// </summary>
    public class iParcelShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;
        private readonly iParcelShipmentType shipmentType;
        private readonly ICustomsManager customsManager;

        /// <summary>
        /// Constuctor
        /// </summary>
        public iParcelShipmentAdapter(ShipmentEntity shipment, IShipmentTypeFactory shipmentTypeFactory, ICustomsManager customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.IParcel, nameof(shipment.IParcel));
            MethodConditions.EnsureArgumentIsNotNull(shipmentTypeFactory, nameof(shipmentTypeFactory));
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));

            this.shipment = shipment;
            this.customsManager = customsManager;
            shipmentType = shipmentTypeFactory.Get(shipment) as iParcelShipmentType;
        }

        /// <summary>
        /// Id of the account associated with this shipment
        /// </summary>
        public long? AccountId
        {
            get { return shipment.IParcel.IParcelAccountID; }
            set { shipment.IParcel.IParcelAccountID = value.GetValueOrDefault(); }
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
                return ShipmentTypeCode.iParcel;
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
        public IDictionary<ShipmentEntity, Exception> UpdateDynamicData()
        {
            shipmentType.UpdateDynamicShipmentData(shipment);
            shipmentType.UpdateTotalWeight(shipment);

            return customsManager.EnsureCustomsLoaded(new[] { shipment });
        }
    }
}
