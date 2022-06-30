﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Platform
{
    /// <summary>
    /// Factory for creating Amazon Buy Shipping ShipmentRequests
    /// </summary>
    [KeyedComponent(typeof(ICarrierShipmentRequestFactory), ShipmentTypeCode.AmazonSFP)]
    public class AmazonSfpShipmentRequestFactory : ShipEngineShipmentRequestFactory, ICarrierShipmentRequestFactory
    {
        private readonly IShipEngineRequestFactory shipmentElementFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IAmazonSFPServiceTypeRepository serviceTypeRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSfpShipmentRequestFactory(IShipEngineRequestFactory shipmentElementFactory,
            IShipmentTypeManager shipmentTypeManager,
            IAmazonSFPServiceTypeRepository serviceTypeRepository)
            : base(shipmentElementFactory, shipmentTypeManager)
        {
            this.shipmentElementFactory = shipmentElementFactory;
            this.shipmentTypeManager = shipmentTypeManager;
            this.serviceTypeRepository = serviceTypeRepository;
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
        /// Creates a ShipEngine purchase label request
        /// </summary>
        public override PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment)
        {
            var amazonOrder = (IAmazonOrder) shipment.Order;

            var purchaseLabelRequest = base.CreatePurchaseLabelRequest(shipment);

            var deliveryExp = (AmazonSFPDeliveryExperienceType) shipment.AmazonSFP.DeliveryExperience;

            purchaseLabelRequest.Shipment.Confirmation = GetConfirmation(deliveryExp);
            purchaseLabelRequest.Shipment.ExternalOrderId = amazonOrder.AmazonOrderID;
            purchaseLabelRequest.Shipment.Items = CreateItems(shipment);

            return purchaseLabelRequest;
        }

        /// <summary>
        /// Get the API ConfirmationEnum
        /// </summary>
        private Shipment.ConfirmationEnum GetConfirmation(AmazonSFPDeliveryExperienceType deliveryExp)
        {
            switch (deliveryExp)
            {
                case AmazonSFPDeliveryExperienceType.DeliveryConfirmationWithSignature:
                    return Shipment.ConfirmationEnum.Signature;

                case AmazonSFPDeliveryExperienceType.DeliveryConfirmationWithAdultSignature:
                    return Shipment.ConfirmationEnum.Adultsignature;

                case AmazonSFPDeliveryExperienceType.DeliveryConfirmationWithoutSignature:
                default:
                    return Shipment.ConfirmationEnum.None;
            }
        }

        /// <summary>
        /// Ensures the Amazon Buy Shipping shipment is not null
        /// </summary>
        protected override void EnsureCarrierShipmentIsNotNull(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.AmazonSFP, nameof(shipment.AmazonSFP));
        }

        /// <summary>
        /// Gets the ShipEngine carrier ID from the Amazon Buy Shipping shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment)
        {
            IAmazonStoreEntity amazonStore = (IAmazonStoreEntity) shipment.Order.Store;
            return amazonStore.PlatformAmazonCarrierID ?? string.Empty;
        }

        /// <summary>
        /// Gets the api value for the Amazon Buy Shipping service
        /// </summary>
        protected override string GetServiceApiValue(ShipmentEntity shipment)
        {
            //EnumHelper.GetApiValue((AmazonSfpServiceType) shipment.AmazonSFP.Service);
            var platformApiCode = serviceTypeRepository.Find(shipment.AmazonSFP.ShippingServiceID)?.PlatformApiCode;
            return platformApiCode;
        }

        /// <summary>
        /// Creates the Amazon Buy Shipping advanced options node
        /// </summary>
        protected override AdvancedOptions CreateAdvancedOptions(ShipmentEntity shipment)
        {
            return new AdvancedOptions();
        }

        /// <summary>
        /// Sets insurance on the package
        /// </summary>
        protected override void SetPackageInsurance(ShipmentPackage shipmentPackage, IPackageAdapter packageAdapter)
        {
            if (packageAdapter.InsuranceChoice.Insured &&
                packageAdapter.InsuranceChoice.InsuranceProvider == InsuranceProvider.Carrier)
            {
                shipmentPackage.InsuredValue = new MoneyDTO(MoneyDTO.CurrencyEnum.USD, decimal.ToDouble(packageAdapter.InsuranceChoice.InsuranceValue));
            }
        }

        /// <summary>
        /// Creates the Amazon Buy Shipping customs node
        /// </summary>
        protected override InternationalOptions CreateCustoms(ShipmentEntity shipment)
        {
            //InternationalOptions customs = new InternationalOptions()
            //{
            //    Contents = (InternationalOptions.ContentsEnum) shipment.AmazonSFP.Contents,
            //    CustomsItems = shipmentElementFactory.CreateCustomsItems(shipment),
            //    NonDelivery = (InternationalOptions.NonDeliveryEnum) shipment.AmazonSFP.NonDelivery
            //};

            //return customs;

            return new InternationalOptions();
        }

        /// <summary>
        /// Creates the TaxIdentifiers node
        /// </summary>
        protected override List<TaxIdentifier> CreateTaxIdentifiers(ShipmentEntity shipment)
        {
            List<TaxIdentifier> TaxIdentifiers = new List<TaxIdentifier>();
            //if (!string.IsNullOrWhiteSpace(shipment.AmazonSFP.CustomsRecipientTin) && shipment.AmazonSFP.CustomsRecipientTin != string.Empty)
            //{
            //    TaxIdentifiers.Add(
            //        new TaxIdentifier()
            //        {
            //            IdentifierType = (TaxIdentifier.IdentifierTypeEnum) shipment.AmazonSFP.CustomsTaxIdType,
            //            IssuingAuthority = shipment.AmazonSFP.CustomsTinIssuingAuthority,
            //            TaxableEntityType = "shipper",
            //            Value = shipment.AmazonSFP.CustomsRecipientTin,
            //        });
            //};

            return TaxIdentifiers;
        }

        /// <summary>
        /// Create shipment items
        /// </summary>
        protected override List<ShipmentItem> CreateItems(ShipmentEntity shipment)
        {
            var amazonOrder = (IAmazonOrder) shipment.Order;

            return amazonOrder.AmazonOrderItems
                .Select(x => new ShipmentItem
                {
                    ExternalOrderId = amazonOrder.AmazonOrderID,
                    ExternalOrderItemId = x.AmazonOrderItemCode,
                    //OrderSourceCode =
                    Quantity = (int) x.Quantity
                })
                .ToList();
        }
    }
}
