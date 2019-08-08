using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores.Warehouse;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;


namespace ShipWorks.Stores.Tests.Warehouse
{
    public class ShipmentDtoFactoryTest
    {
        private readonly AutoMock mock;

        private readonly Mock<IShipmentTypeManager> shipmentTypeManager;
        private readonly Mock<ICarrierShipmentAdapterFactory> shipmentAdapterFactory;
        private readonly Mock<IUserManager> userManager;

        private readonly ShipmentDtoFactory testObject;

        private readonly ShipmentEntity shipmentEntity;
        private readonly string tangoID;

        public ShipmentDtoFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentAdapterFactory = mock.Mock<ICarrierShipmentAdapterFactory>();
            userManager = mock.Mock<IUserManager>();

            testObject = mock.Create<ShipmentDtoFactory>();


            shipmentEntity = new ShipmentEntity()
            {
                Order = new OrderEntity()
            };

            tangoID = "123";
        }


        [Fact]
        public void CreateHubShipment_VerifyLoadShipmentDataIsCalled()
        {
            testObject.CreateHubShipment(shipmentEntity, tangoID);

            shipmentTypeManager.Verify(m => m.LoadShipmentData(shipmentEntity, false), Times.Once);
        }

        [Fact]
        public void CreateHubShipment_DelegatesToShipmentAdapterFactory()
        {
            testObject.CreateHubShipment(shipmentEntity, tangoID);
            shipmentAdapterFactory.Verify(a => a.Get(shipmentEntity), Times.Once);
        }

        [Theory]
        [InlineData(InsuranceProvider.Carrier, true, 0, 1)]
        [InlineData(InsuranceProvider.Carrier, false, 0, 0)]
        [InlineData(InsuranceProvider.ShipWorks, true, 1, 0)]
        [InlineData(InsuranceProvider.ShipWorks, false, 0, 0)]
        [InlineData(InsuranceProvider.Invalid, true, 0, 0)]
        [InlineData(InsuranceProvider.Invalid, false, 0, 0)]
        public void CreateHubShipment_InsuranceIsSetCorrectly(InsuranceProvider provider, bool isInsured, int shipWorksInsured, int carrierInsured)
        {
            shipmentEntity.InsuranceProvider = (int) provider;
            shipmentEntity.Insurance = isInsured;
            var hubShipment = testObject.CreateHubShipment(shipmentEntity, tangoID);

            Assert.Equal(shipWorksInsured, hubShipment.ShipworksInsured);
            Assert.Equal(carrierInsured, hubShipment.CarrierInsured);
        }

        [Fact]
        public void CreateHubShipment_VerifiedByIsSetCorrectly_WhenVerifiedByIsSpecified()
        {
            var verifiedDate = DateTime.Now;
            shipmentEntity.Order.VerifiedBy = 42;
            shipmentEntity.Order.Verified = true;
            shipmentEntity.Order.VerifiedDate = verifiedDate;

            string username = "bob";
            userManager.Setup(m=>m.GetUser(42)).Returns(new UserEntity { Username = username });

            var hubShipment = testObject.CreateHubShipment(shipmentEntity, tangoID);

            Assert.True(hubShipment.Verified);
            Assert.Equal(username, hubShipment.VerifiedByUser);
            Assert.Equal(verifiedDate, hubShipment.VerifiedDate);
        }

        [Fact]
        public void CreateHubShipment_VerifiedByIsSetCorrectly_WhenVerifiedByIsNotSpecified()
        {
            shipmentEntity.Order.Verified = false;

            var hubShipment = testObject.CreateHubShipment(shipmentEntity, tangoID);

            Assert.False(hubShipment.Verified);
            Assert.Empty(hubShipment.VerifiedByUser);
        }

        [Fact]
        public void CreateHubShipment_VerifyAddressIsSetCorrectly()
        {
            shipmentEntity.ShipFirstName = "Bob";
            shipmentEntity.ShipStreet3 = "Street3";
            shipmentEntity.ShipPostalCode = "90210";

            var hubShipment = testObject.CreateHubShipment(shipmentEntity, tangoID);

            Assert.Equal("Bob", hubShipment.RecipientAddress.FirstName);
            Assert.Equal("Street3", hubShipment.RecipientAddress.Street3);
            Assert.Equal("90210", hubShipment.RecipientAddress.PostalCode);
        }
    }
}
