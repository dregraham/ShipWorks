using ShipWorks.Shipping.ShipEngine;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Services;
using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.Amazon;
using System;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// Factory for creating AmazonSWA ShipmentRequests
    /// </summary>
    [KeyedComponent(typeof(ICarrierShipmentRequestFactory), ShipmentTypeCode.AmazonSWA)]
    public class AmazonSWAShipmentRequestFactory : ShipEngineShipmentRequestFactory
    {
        private readonly IAmazonSWAAccountRepository accountRepository;
        private readonly IShipEngineRequestFactory shipmentElementFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSWAShipmentRequestFactory(IAmazonSWAAccountRepository accountRepository,
            IShipEngineRequestFactory shipmentElementFactory,
            IShipmentTypeManager shipmentTypeManager)
            : base(shipmentElementFactory, shipmentTypeManager)
        {
            this.accountRepository = accountRepository;
            this.shipmentElementFactory = shipmentElementFactory;
            this.shipmentTypeManager = shipmentTypeManager;
        }

        /// <summary>
        /// Ensures the AmazonSWA shipment is not null
        /// </summary>
        protected override void EnsureCarrierShipmentIsNotNull(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.AmazonSWA, nameof(shipment.AmazonSWA));
        }

        /// <summary>
        /// Gets the ShipEngine carrier ID from the AmazonSWA shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment)
        {
            AmazonSWAAccountEntity account = accountRepository.GetAccount(shipment);

            if (account == null)
            {
                throw new AmazonSWAException("Invalid account associated with shipment.");
            }

            return account.ShipEngineCarrierId;
        }

        /// <summary>
        /// Create RateShipmentRequest adds items which are required for amazon shipping
        /// </summary>
        public override RateShipmentRequest CreateRateShipmentRequest(ShipmentEntity shipment)
        {
            var request = base.CreateRateShipmentRequest(shipment);

            request.Shipment.Items = GetShipmentItems(shipment);

            if (shipment.Order is AmazonOrderEntity)
            {
                request.Shipment.OrderSourceCode = AddressValidatingShipment.OrderSourceCodeEnum.Amazon;
            }

            return request;
        }

        /// <summary>
        /// Create Purchase Label Request, adding items which are required by Amazon
        /// </summary>
        public override PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment)
        {
            var request = base.CreatePurchaseLabelRequest(shipment);

            if (shipment.Order is AmazonOrderEntity)
            {
                request.Shipment.OrderSourceCode = Shipment.OrderSourceCodeEnum.Amazon;
            }

            request.Shipment.Items = GetShipmentItems(shipment);

            return request;
        }

        /// <summary>
        /// Get shipment items from the shipment
        /// </summary>
        private static List<ShipmentItem> GetShipmentItems(ShipmentEntity shipment)
        {
            var result = new List<ShipmentItem>();

            AmazonOrderEntity amazonOrder = shipment.Order as AmazonOrderEntity;
            foreach (OrderItemEntity item in shipment.Order.OrderItems)
            {
                AmazonOrderItemEntity amazonItem = item as AmazonOrderItemEntity;
                result.Add(
                    new ShipmentItem(
                        externalOrderItemId: amazonItem?.AmazonOrderItemCode ?? string.Empty,
                        externalOrderId: amazonOrder?.AmazonOrderID ?? shipment.Order.OrderNumberComplete,
                        asin: amazonItem?.ASIN ?? string.Empty,
                        name: item.Name,
                        quantity: Convert.ToInt32(item.Quantity)));
            }

            // ShipEngine will throw if there are no items, they recommended we add a fake item
            if (result.None())
            {
                result.Add(new ShipmentItem(
                    name: "NoItem",
                    externalOrderId: shipment.Order.OrderNumberComplete,
                    quantity: 1));
            }

            return result;
        }

        /// <summary>
        /// Gets the api value for the AmazonSWA service
        /// </summary>
        protected override string GetServiceApiValue(ShipmentEntity shipment)
        {
            return EnumHelper.GetApiValue((AmazonSWAServiceType) shipment.AmazonSWA.Service);
        }

        /// <summary>
        /// Creates the AmazonSWA advanced options node
        /// </summary>
        protected override AdvancedOptions CreateAdvancedOptions(ShipmentEntity shipment)
        {
            return null;
        }

        /// <summary>
        /// Creates the AmazonSWA customs node
        /// </summary>
        protected override InternationalOptions CreateCustoms(ShipmentEntity shipment)
        {
            return null;
        }

        /// <summary>
        /// Creates the AmazonSWA tax identifier node
        /// </summary>
        protected override List<TaxIdentifier> CreateTaxIdentifiers(ShipmentEntity shipment)
        {
            return null;
        }

        /// <summary>
        /// Create items
        /// </summary>
        protected override List<ShipmentItem> CreateItems(ShipmentEntity shipment)
        {
            return new List<ShipmentItem>();
        }
    }
}
