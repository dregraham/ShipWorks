﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.Controls.From;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests.From
{
    public class OrderLookupFromViewModelTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly OrderLookupFromViewModel testObject;
        private readonly Mock<IViewModelOrchestrator> orchestrator;

        public OrderLookupFromViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            List<KeyValuePair<string, long>> origins = new List<KeyValuePair<string, long>>();
            origins.Add(new KeyValuePair<string, long>("FOOBAR", 1));

            Mock<ShipmentType> shipmentType = mock.Mock<ShipmentType>();
            shipmentType.Setup(s => s.GetOrigins()).Returns(origins);

            Mock<IShipmentTypeManager> shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(s => s.Get(ShipmentTypeCode.Usps)).Returns(shipmentType);

            Mock<ICarrierShipmentAdapter> shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.SetupGet(s => s.ShipmentTypeCode).Returns(ShipmentTypeCode.Usps);
            shipmentAdapter.SetupGet(s => s.Shipment).Returns(new ShipmentEntity() { OriginOriginID = 1 });

            Mock<ICarrierAccountRetriever> accountRetriever = mock.Mock<ICarrierAccountRetriever>();
            accountRetriever.Setup(a => a.GetAccountReadOnly(It.IsAny<ShipmentEntity>())).Returns(new UspsAccountEntity() { Description = "blah" });

            Mock<ICarrierAccountRetrieverFactory> factory = mock.Mock<ICarrierAccountRetrieverFactory>();
            factory.Setup(f => f.Create(ShipmentTypeCode.Usps)).Returns(accountRetriever);

            orchestrator = mock.Mock<IViewModelOrchestrator>();
            orchestrator.SetupGet(o => o.ShipmentAdapter).Returns(shipmentAdapter);

            testObject = mock.Create<OrderLookupFromViewModel>();
        }

        [Fact]
        public void Title_IsFrom_WhenNoShipmentIsLoaded()
        {
            orchestrator.SetupGet(o => o.ShipmentAdapter).Returns((ICarrierShipmentAdapter)null);

            testObject.RateShop = false;

            Assert.Equal("From", testObject.Title);
        }

        [Fact]
        public void Title_ContainsRateShopping_WhenShipmentSupportsRateShopping()
        {
            testObject.RateShop = true;

            Assert.Contains("(Rate Shopping)", testObject.Title);
        }


        [Fact]
        public void Title_ContainsAccountDescription_WhenShipmentDoesNotSupportsRateShopping()
        {
            testObject.RateShop = false;

            Assert.Contains("blah", testObject.Title);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}