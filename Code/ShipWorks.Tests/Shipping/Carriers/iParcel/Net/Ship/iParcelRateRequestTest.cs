﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Carriers.iParcel.Net;
using ShipWorks.Shipping.Carriers.iParcel.Net.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel.Net.Ship
{
    public class iParcelRateRequestTest
    {
        private iParcelRateRequest testObject;

        private iParcelCredentials credentials;
        private ShipmentEntity shipment;
        private Mock<IiParcelServiceGateway> gateway;

        public iParcelRateRequestTest()
        {
            gateway = new Mock<IiParcelServiceGateway>();
            gateway.Setup(g => g.IsValidUser(It.IsAny<iParcelCredentials>())).Returns(true);

            credentials = new iParcelCredentials("username", "password", false, gateway.Object);

            shipment = new ShipmentEntity()
            {
                ShipCity = "St. Louis",
                ShipCompany = "Initech",
                ShipCountryCode = "US",
                ShipEmail = "someone@nowhere.com",
                ShipFirstName = "Peter",
                ShipLastName = "Gibbons",
                ShipPhone = "555-555-5555",
                ShipPostalCode = "63102",
                ShipStateProvCode = "MO",
                ShipStreet1 = "1 Main Street",
                ShipStreet2 = "Suite 500",

                OriginFirstName = "Bill",
                OriginLastName = "Lumbergh",
                OriginStreet1 = "500 First Street",
                OriginStreet2 = "Suite 200",
                OriginCity = "St. Louis",
                OriginStateProvCode = "MO",
                OriginPostalCode = "63102",
                OriginCountryCode = "US",

                Order = new OrderEntity() { OrderTotal = 100.43M },

                IParcel = new IParcelShipmentEntity
                {
                    Reference = "reference-value",
                    Service = (int)iParcelServiceType.Preferred,
                    TrackByEmail = true,
                    TrackBySMS = true
                }
            };

            shipment.Order.OrderItems.Add(new OrderItemEntity { Description = "some description", Quantity = 2, Weight = 1.54, UnitPrice = 3.40M, SKU = "12345678" });
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6 });

            testObject = new iParcelRateRequest(credentials, shipment);
        }

        [Fact]
        public void OperationName()
        {
            Assert.Equal("SubmitPack", testObject.OperationName);
        }

        [Fact]
        public void RootElementName()
        {
            Assert.Equal("iparcelPackageUpload", testObject.RootElementName);
        }

        [Fact]
        public void RequestElements_ContainsThreeItems()
        {
            Assert.Equal(3, testObject.RequestElements.Count);
        }

        [Fact]
        public void RequestElements_ContainsVersionElement()
        {
            Assert.Equal(1, testObject.RequestElements.Count(e => e.GetType() == typeof(iParcelVersionElement)));
        }

        [Fact]
        public void RequestElements_ContainsShipValidationElement()
        {
            Assert.Equal(1, testObject.RequestElements.Count(e => e.GetType() == typeof(iParcelShipValidationElement)));
        }

        [Fact]
        public void RequestElements_ContainsPackageInfoElement()
        {
            Assert.Equal(1, testObject.RequestElements.Count(e => e.GetType() == typeof(iParcelPackageInfoElement)));
        }


        [Fact]
        public void ValidationElement_IsConfiguredForRates()
        {
            // Already have a test that there is exactly one element of this type, so we can just grab the first one
            iParcelShipValidationElement validationElement = (iParcelShipValidationElement)testObject.RequestElements.First(e => e.GetType() == typeof(iParcelShipValidationElement));

            Assert.True(validationElement.UsedForRates);
        }
    }
}
