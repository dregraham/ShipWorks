using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.Shipping.Editing.Rating;
using Address = ShipWorks.Shipping.Carriers.Amazon.Api.DTOs.Address;
using ShipWorks.Stores.Platforms.Amazon.Mws;

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

            GetEligibleShippingServices response = webClient.GetRates(requestDetails, settingsFactory.Create(shipment.Amazon));

            return GetRateGroupFromResponse(response);
        }

        /// <summary>
        /// Gets the rate group from response.
        /// </summary>
        private static RateGroup GetRateGroupFromResponse(GetEligibleShippingServices response)
        {
            List<RateResult> rateResults = new List<RateResult>();

            List<ShippingService> serviceList = response.GetEligibleShippingServicesResponse.ShippingServiceList;
            foreach (ShippingService shippingService in serviceList)
            {
                AmazonRateTag tag = new AmazonRateTag() { ShippingServiceId = shippingService.ShippingServiceId, ShippingServiceOfferId = shippingService.ShippingServiceOfferId };
                RateResult rateResult = new RateResult(shippingService.ShippingServiceName,"",shippingService.Rate.Amount, tag);
                rateResults.Add(rateResult);
            }

            return new RateGroup(rateResults);
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
                    StateOrProvinceCode = shipment.OriginStateProvCode
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
            List<Item> items = new List<Item>();
            
            foreach (AmazonOrderItemEntity orderItem in order.OrderItems)
            {
                Item item = new Item();
                item.OrderItemId = orderItem.AmazonOrderItemCode;
                item.Quantity = (int) orderItem.Quantity;
                items.Add(item);
            }

            return items;
        }
    }
}
