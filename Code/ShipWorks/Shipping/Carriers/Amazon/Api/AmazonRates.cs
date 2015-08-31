using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.OpenShip;
using ShipWorks.Shipping.Editing.Rating;
using Address = ShipWorks.Shipping.Carriers.Amazon.Api.DTOs.Address;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using System.Drawing;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Gets the rates from Amazon via the IAmazonShippingWebClient
    /// </summary>
    public class AmazonRates : IAmazonRates
    {
        private readonly IAmazonShippingWebClient webClient;
        private readonly IAmazonMwsWebClientSettingsFactory settingsFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonRates"/> class.
        /// </summary>
        /// <param name="webClient">The web client.</param>
        public AmazonRates(IAmazonShippingWebClient webClient,  IAmazonMwsWebClientSettingsFactory settingsFactory)
        {
            this.webClient = webClient;
            this.settingsFactory = settingsFactory;
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            AmazonOrderEntity order = shipment.Order as AmazonOrderEntity;

            if (order == null)
            {
                throw new AmazonShipperException("Not an Amazon Order");
            }

            ShipmentRequestDetails requestDetails = CreateGetRatesRequest(shipment, order);

            GetEligibleShippingServicesResponse response = webClient.GetRates(requestDetails, settingsFactory.Create(shipment.Amazon));

            return GetRateGroupFromResponse(response);
        }

        /// <summary>
        /// Gets the rate group from response.
        /// </summary>
        private static RateGroup GetRateGroupFromResponse(GetEligibleShippingServicesResponse response)
        {
            List<RateResult> rateResults = new List<RateResult>();

            ShippingServiceList serviceList = response.GetEligibleShippingServicesResult.ShippingServiceList;
            foreach (ShippingService shippingService in serviceList.ShippingService)
            {
                AmazonRateTag tag = new AmazonRateTag() { ShippingServiceId = shippingService.ShippingServiceId, ShippingServiceOfferId = shippingService.ShippingServiceOfferId };
                RateResult rateResult = new RateResult(shippingService.ShippingServiceName,"",shippingService.Rate.Amount, tag);
                rateResult.ProviderLogo = GetProviderLogo(shippingService);
                rateResults.Add(rateResult);
            }

            return new RateGroup(rateResults);
        }

        /// <summary>
        /// Determine which carrier the ShippingService belongs to 
        /// Return the logo of that carrier returns Null if we cannot
        /// find a match for the carrier
        /// </summary>
        /// <param name="shippingService"></param>
        /// <returns></returns>
        private static Image GetProviderLogo(ShippingService shippingService)
        {
            switch (shippingService.CarrierName.ToLower())
            {
                case "ups":
                    return EnumHelper.GetImage(ShipmentTypeCode.UpsOnLineTools);
                case "fedex":
                    return EnumHelper.GetImage(ShipmentTypeCode.FedEx);
                case "usps":
                    return EnumHelper.GetImage(ShipmentTypeCode.Usps);
                default:
                    return EnumHelper.GetImage(ShipmentTypeCode.None);
            }
        }

        /// <summary>
        /// Creates the get rates request.
        /// </summary>
        private ShipmentRequestDetails CreateGetRatesRequest(ShipmentEntity shipment, AmazonOrderEntity order)
        {
            ShipmentRequestDetails requestDetails = new ShipmentRequestDetails()
            {
                AmazonOrderId = order.AmazonOrderID,
                Insurance = shipment.Amazon.DeclaredValue.HasValue ?
                    new CurrencyAmount()
                    {
                        Amount = shipment.Amazon.DeclaredValue.Value,
                        CurrencyCode = "USD"
                    } : null,
                ItemList = GetItemList(order),
                PackageDimensions = new PackageDimensions()
                {
                    Height = shipment.Amazon.DimsHeight,
                    Length = shipment.Amazon.DimsLength,
                    Width = shipment.Amazon.DimsWidth
                },
                MustArriveByDate = shipment.Amazon.DateMustArriveBy,
                ShipFromAddress = new Address()
                {
                    AddressLine1 = shipment.OriginStreet1,
                    AddressLine2 = shipment.OriginStreet2,
                    AddressLine3 = shipment.OriginStreet3,
                    City = shipment.OriginCity,
                    CountryCode = shipment.OriginCountryCode,
                    Phone = shipment.OriginPhone,
                    Name = shipment.OriginUnparsedName,
                    PostalCode = shipment.OriginPostalCode,
                    StateOrProvinceCode = shipment.OriginStateProvCode,
                    Email = shipment.OriginEmail
                    
                },
                ShippingServiceOptions = new ShippingServiceOptions()
                {
                    CarrierWillPickUp = shipment.Amazon.CarrierWillPickUp,
                    DeliveryExperience = EnumHelper.GetApiValue((AmazonDeliveryExperienceType) shipment.Amazon.DeliveryExperience)
                },
                Weight = shipment.TotalWeight
            };

            return requestDetails;
        }

        /// <summary>
        /// Gets the item list.
        /// </summary>
        private static List<Item> GetItemList(OrderEntity order)
        {
            List<AmazonOrderItemEntity> amazonOrderItems = null;

            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                amazonOrderItems = DataProvider.GetRelatedEntities(order.OrderID, EntityType.AmazonOrderItemEntity).Cast<AmazonOrderItemEntity>().ToList();
            }
            else
            {
                amazonOrderItems = order.OrderItems.Select(i => (AmazonOrderItemEntity)i).ToList();
            }

            List<Item> items = new List<Item>();
            
            foreach (AmazonOrderItemEntity amazonOrderItem in amazonOrderItems)
            {
                Item item = new Item();
                item.OrderItemId = amazonOrderItem.AmazonOrderItemCode;
                item.Quantity = (int)amazonOrderItem.Quantity;
                items.Add(item);
            }

            return items;
        }
    }
}
