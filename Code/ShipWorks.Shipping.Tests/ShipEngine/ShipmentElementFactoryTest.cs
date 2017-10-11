﻿using Autofac.Extras.Moq;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.ShipEngine
{
    public class ShipmentElementFactoryTest : IDisposable
    {
        readonly AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        readonly ShipmentElementFactory testObject;

        public ShipmentElementFactoryTest()
        {
            testObject = mock.Create<ShipmentElementFactory>();
        }

        [Fact]
        public void CreateRateRequest_PopulatesShipToAddress()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                ShipUnparsedName = "John Snow",
                ShipPhone = "123-456-7890",
                ShipCompany = "The Nights Watch",
                ShipStreet1 = "The Wall",
                ShipStreet2 = "street 2",
                ShipStreet3 = "street 3",
                ShipCity = "North of The Gift",
                ShipStateProvCode = "ND",
                ShipPostalCode = "90210",
                ShipCountryCode = "Westeros"
            };

        
            var request = testObject.CreateRateRequest(shipment);

            Assert.Equal(shipment.ShipUnparsedName, request.Shipment.ShipTo.Name);
            Assert.Equal(shipment.ShipPhone, request.Shipment.ShipTo.Phone);
            Assert.Equal(shipment.ShipCompany, request.Shipment.ShipTo.CompanyName);
            Assert.Equal(shipment.ShipStreet1, request.Shipment.ShipTo.AddressLine1);
            Assert.Equal(shipment.ShipStreet2, request.Shipment.ShipTo.AddressLine2);
            Assert.Equal(shipment.ShipStreet3, request.Shipment.ShipTo.AddressLine3);
            Assert.Equal(shipment.ShipCity, request.Shipment.ShipTo.CityLocality);
            Assert.Equal(shipment.ShipStateProvCode, request.Shipment.ShipTo.StateProvince);
            Assert.Equal(shipment.ShipPostalCode, request.Shipment.ShipTo.PostalCode);
            Assert.Equal(shipment.ShipCountryCode, request.Shipment.ShipTo.CountryCode);           
        }

        [Fact]
        public void CreateRateRequest_PopulatesShipFromAddress()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                OriginUnparsedName = "John Snow",
                OriginPhone = "123-456-7890",
                OriginCompany = "The Nights Watch",
                OriginStreet1 = "The Wall",
                OriginStreet2 = "street 2",
                OriginStreet3 = "street 3",
                OriginCity = "North of The Gift",
                OriginStateProvCode = "ND",
                OriginPostalCode = "90210",
                OriginCountryCode = "Westeros"
            };


            var request = testObject.CreateRateRequest(shipment);

            Assert.Equal(shipment.OriginUnparsedName, request.Shipment.ShipFrom.Name);
            Assert.Equal(shipment.OriginPhone, request.Shipment.ShipFrom.Phone);
            Assert.Equal(shipment.OriginCompany, request.Shipment.ShipFrom.CompanyName);
            Assert.Equal(shipment.OriginStreet1, request.Shipment.ShipFrom.AddressLine1);
            Assert.Equal(shipment.OriginStreet2, request.Shipment.ShipFrom.AddressLine2);
            Assert.Equal(shipment.OriginStreet3, request.Shipment.ShipFrom.AddressLine3);
            Assert.Equal(shipment.OriginCity, request.Shipment.ShipFrom.CityLocality);
            Assert.Equal(shipment.OriginStateProvCode, request.Shipment.ShipFrom.StateProvince);
            Assert.Equal(shipment.OriginPostalCode, request.Shipment.ShipFrom.PostalCode);
            Assert.Equal(shipment.OriginCountryCode, request.Shipment.ShipFrom.CountryCode);
        }

        [Fact]
        public void CreateRateRequest_PopulatesTotalWeight()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                TotalWeight = 42.42
            };

            var request = testObject.CreateRateRequest(shipment);

            Assert.Equal(42.42D, request.Shipment.TotalWeight.Value);
            Assert.Equal(Weight.UnitEnum.Pound, request.Shipment.TotalWeight.Unit);
        }

        [Fact]
        public void CreateCustomsItems_CreatesSingleCustomItems_WhenOneCustomItemInShipment()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            ShipmentCustomsItemEntity customsItem = new ShipmentCustomsItemEntity()
            {
                CountryOfOrigin = "China",
                Description = "desc",
                HarmonizedCode = "Harm",
                Quantity = 42.5,
                UnitValue = 12
            };
            shipment.CustomsItems.Add(customsItem);

            var customs = testObject.CreateCustoms(shipment);
            var apiCustoms = customs.CustomsItems.Single();

            Assert.Equal(customsItem.CountryOfOrigin, apiCustoms.CountryOfOrigin);
            Assert.Equal(customsItem.Description, apiCustoms.Description);
            Assert.Equal(customsItem.HarmonizedCode, apiCustoms.HarmonizedTariffCode);
            Assert.Equal(43, apiCustoms.Quantity);
            Assert.Equal((double) customsItem.UnitValue, apiCustoms.Value);
        }

        [Fact]
        public void CreateCustomsItems_CreatesTwoCusomtItems_WhenTwoCustomItemsInShipment()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            var customsItem1 = new ShipmentCustomsItemEntity()
            {
                CountryOfOrigin = "China"
            };
            var customsItem2 = new ShipmentCustomsItemEntity()
            {
                CountryOfOrigin = "US"
            };
            shipment.CustomsItems.Add(customsItem1);
            shipment.CustomsItems.Add(customsItem2);

            var customs = testObject.CreateCustoms(shipment);

            Assert.Equal(2, customs.CustomsItems.Count);
            Assert.NotNull(customs.CustomsItems.Single(c => c.CountryOfOrigin == "China"));
            Assert.NotNull(customs.CustomsItems.Single(c => c.CountryOfOrigin == "US"));

        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
