using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.Enums;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Factory for creating the ShipmentRequestDetails used for the Amazon Shipping Api
    /// </summary>
    public class AmazonShipmentRequestDetailsFactory : IAmazonShipmentRequestDetailsFactory
    {
        /// <summary>
        /// Creates the ShipmentRequestDetails.
        /// </summary>
        public ShipmentRequestDetails Create(ShipmentEntity shipment, AmazonOrderEntity order)
        {
            return new ShipmentRequestDetails()
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
                SendDateMustArriveBy = shipment.Amazon.SendDateMustArriveBy,
                ShipFromAddress = new Address()
                {
                    AddressLine1 = shipment.OriginStreet1,
                    AddressLine2 = shipment.OriginStreet2,
                    AddressLine3 = shipment.OriginStreet3,
                    City = shipment.OriginCity,
                    CountryCode = shipment.OriginCountryCode,
                    Phone = shipment.OriginPhone,
                    Name = shipment.OriginPerson.ParsedName.FullName,
                    PostalCode = shipment.OriginPostalCode,
                    StateOrProvinceCode = shipment.OriginStateProvCode,
                    Email = shipment.OriginEmail

                },
                ShippingServiceOptions = new ShippingServiceOptions()
                {
                    CarrierWillPickUp = shipment.Amazon.CarrierWillPickUp,
                    DeliveryExperience = EnumHelper.GetApiValue((AmazonDeliveryExperienceType)shipment.Amazon.DeliveryExperience),
                    DeclaredValue = new DeclaredValue()
                    {
                        Amount = shipment.Amazon.InsuranceValue,
                        CurrencyCode = "USD"
                    }
                },
                Weight = shipment.TotalWeight
            };
        }

        /// <summary>
        /// Gets the item list.
        /// </summary>
        private static List<Item> GetItemList(OrderEntity order)
        {
            return order.OrderItems
                .OfType<AmazonOrderItemEntity>()
                .Select(x => new Item
                {
                    OrderItemId = x.AmazonOrderItemCode,
                    Quantity = (int)x.Quantity
                })
                .ToList();
        }
    }
}