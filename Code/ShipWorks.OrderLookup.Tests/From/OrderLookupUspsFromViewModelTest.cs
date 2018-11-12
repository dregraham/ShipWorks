using System;
using System.Collections.Generic;
using System.ComponentModel;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.Controls.From;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.OrderLookup.Tests.From
{
    public class OrderLookupUspsFromViewModelTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly UspsFromViewModel testObject;
        private readonly Mock<IOrderLookupShipmentModel> shipmentModel;
        private readonly ShipmentEntity shipment;

        public OrderLookupUspsFromViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            List<KeyValuePair<string, long>> origins = new List<KeyValuePair<string, long>>();
            origins.Add(new KeyValuePair<string, long>("FOOBAR", 1));

            Mock<ShipmentType> shipmentType = mock.Mock<ShipmentType>();
            shipmentType.Setup(s => s.GetOrigins()).Returns(origins);

            Mock<IShipmentTypeManager> shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(s => s.Get(ShipmentTypeCode.Usps)).Returns(shipmentType);

            shipment = Create.Shipment()
                .AsPostal(p => p.AsUsps())
                .Set(x => x.OriginOriginID, 1)
                .Build();

            Mock<ICarrierShipmentAdapter> shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.SetupGet(s => s.ShipmentTypeCode).Returns(ShipmentTypeCode.Usps);
            shipmentAdapter.SetupGet(s => s.Shipment).Returns(shipment);
            shipmentAdapter.SetupGet(s => s.AccountId).Returns(42);

            mock.FromFactory<ICarrierAccountRetrieverFactory>()
                .Mock(x => x.Create(It.IsAny<ShipmentTypeCode>()))
                .Setup(a => a.Accounts)
                .Returns(new[] { new UspsAccountEntity { Description = "blah", UspsAccountID = 42 } });

            shipmentModel = mock.Mock<IOrderLookupShipmentModel>();
            shipmentModel.SetupGet(o => o.ShipmentAdapter).Returns(shipmentAdapter);

            testObject = mock.Create<UspsFromViewModel>();
        }

        [Fact]
        public void Title_IsFrom_WhenNoShipmentIsLoaded()
        {
            shipmentModel.SetupGet(o => o.ShipmentAdapter).Returns((ICarrierShipmentAdapter) null);

            shipmentModel.Raise(x => x.PropertyChanged += null, new PropertyChangedEventArgs("RateShop"));

            Assert.Equal("From", testObject.Title);
        }

        [Fact]
        public void Title_ContainsRateShopping_WhenShipmentSupportsRateShopping()
        {
            shipment.Postal.Usps.RateShop = true;

            shipmentModel.Raise(x => x.PropertyChanged += null, new PropertyChangedEventArgs("RateShop"));

            Assert.Contains("(Rate Shopping)", testObject.Title);
        }

        [Fact]
        public void Title_ContainsAccountDescription_WhenShipmentDoesNotSupportsRateShopping()
        {
            shipment.Postal.Usps.RateShop = false;

            shipmentModel.Raise(x => x.PropertyChanged += null, new PropertyChangedEventArgs("RateShop"));

            Assert.Contains("blah", testObject.Title);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}