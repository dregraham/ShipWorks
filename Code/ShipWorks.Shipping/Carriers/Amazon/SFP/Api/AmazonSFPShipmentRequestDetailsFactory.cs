﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Enums;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Api
{
    /// <summary>
    /// Factory for creating the ShipmentRequestDetails used for the Amazon Shipping Api
    /// </summary>
    public class AmazonSFPShipmentRequestDetailsFactory : IAmazonSFPShipmentRequestDetailsFactory
    {
        private const int MaxReferenceLength = 25;

        /// <summary>
        /// Creates the ShipmentRequestDetails.
        /// </summary>
        public ShipmentRequestDetails Create(ShipmentEntity shipment, IAmazonOrder order)
        {
            return new ShipmentRequestDetails
            {
                AmazonOrderId = order.AmazonOrderID,
                Insurance = null,
                ItemList = GetItemList(order),
                ShipDate = shipment.ShipDate,
                PackageDimensions = new PackageDimensions
                {
                    Height = shipment.AmazonSFP.DimsHeight,
                    Length = shipment.AmazonSFP.DimsLength,
                    Width = shipment.AmazonSFP.DimsWidth
                },
                ShipFromAddress = new Address
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
                ShippingServiceOptions = new ShippingServiceOptions
                {
                    CarrierWillPickUp = order.IsSameDay(),
                    DeliveryExperience = EnumHelper.GetApiValue((AmazonSFPDeliveryExperienceType) shipment.AmazonSFP.DeliveryExperience),
                    DeclaredValue = new DeclaredValue
                    {
                        Amount = 0,
                        CurrencyCode = "USD"
                    },
                    LabelFormat = shipment.AmazonSFP.RequestedLabelFormat == (int) ThermalLanguage.ZPL ? "ZPL203" : null
                },
                LabelCustomization = new LabelCustomization
                {
                    CustomTextForLabel = ProcessReferenceNumber(shipment.AmazonSFP.Reference1, shipment.ShipmentID),
                },
                Weight = shipment.TotalWeight
            };
        }

        /// <summary>
        /// Process for tokens and trim to allowable length
        /// </summary>
        public static string ProcessReferenceNumber(string reference, long shipmentID)
        {
            return Regex.Replace(TemplateTokenProcessor.ProcessTokens(reference, shipmentID).Truncate(MaxReferenceLength), @"[^0-9a-zA-Z]{0,25}", string.Empty);
        }

        /// <summary>
        /// Gets the item list.
        /// </summary>
        private static List<Item> GetItemList(IAmazonOrder order)
        {
            return order.AmazonOrderItems
                .Select(x => new Item
                {
                    OrderItemId = x.AmazonOrderItemCode,
                    Quantity = (int) x.Quantity
                })
                .ToList();
        }
    }
}