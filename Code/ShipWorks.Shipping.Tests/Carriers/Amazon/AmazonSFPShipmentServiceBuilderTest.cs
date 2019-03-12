﻿using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using Xunit;
using System.Linq;
using ShipWorks.Shipping.Carriers.Amazon.SFP;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonSFPShipmentServiceBuilderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IAmazonSFPServiceTypeRepository> serviceTypeRepository;
        private readonly Mock<IExcludedServiceTypeRepository> excludedServiceTypeRepository;
        private readonly AmazonSFPShipmentType shipmentType;
        private readonly AmazonSFPShipmentServicesBuilder testObject;
        private readonly List<ExcludedServiceTypeEntity> excludedServices = new List<ExcludedServiceTypeEntity>();
        private readonly List<AmazonServiceTypeEntity> amazonServiceTypes = new List<AmazonServiceTypeEntity>();

        public AmazonSFPShipmentServiceBuilderTest()
        {
            mock = AutoMock.GetLoose();

            serviceTypeRepository = mock.Mock<IAmazonSFPServiceTypeRepository>();
            serviceTypeRepository.Setup(s => s.Get()).Returns(amazonServiceTypes);

            excludedServiceTypeRepository = mock.Mock<IExcludedServiceTypeRepository>();
            excludedServiceTypeRepository.Setup(s => s.GetExcludedServiceTypes(It.IsAny<ShipmentType>()))
                .Returns(excludedServices);

            shipmentType = mock.Create<AmazonSFPShipmentType>();

            testObject = mock.Create<AmazonSFPShipmentServicesBuilder>();
        }

        [Fact]
        public void BuildServiceTypeDictionary_DelegatesToAmazonServiceTypeRepository()
        {
            ShipmentEntity[] shipments = {new ShipmentEntity {Amazon = new AmazonShipmentEntity()}};

            testObject.BuildServiceTypeDictionary(shipments);

            serviceTypeRepository.Verify(s => s.Get());
        }

        [Fact]
        public void BuildServiceTypeDictionary_DelegatesToExcludedServiceTypeRepository()
        {
            ShipmentEntity[] shipments = { new ShipmentEntity { Amazon = new AmazonShipmentEntity() } };

            testObject.BuildServiceTypeDictionary(shipments);

            excludedServiceTypeRepository.Verify(
                s => s.GetExcludedServiceTypes(
                    It.Is<ShipmentType>(c => c.ShipmentTypeCode == shipmentType.ShipmentTypeCode)));
        }

        [Fact]
        public void BuildServiceTypeDictionary_ReturnsShipmentsService_WhenShipmentServiceIsDisabled()
        {
            amazonServiceTypes.Clear();
            excludedServices.Clear();

            amazonServiceTypes.Add(new AmazonServiceTypeEntity()
            {
                AmazonServiceTypeID = 1,
                Description = "This is a random description",
                ApiValue = "some random service"
            });

            excludedServices.Add(new ExcludedServiceTypeEntity { ServiceType = 1, ShipmentType = (int)ShipmentTypeCode.AmazonSFP });

            ShipmentEntity[] shipments = { new ShipmentEntity { Amazon = new AmazonShipmentEntity(){ShippingServiceID = "some random service"} } };

            Dictionary<int, string> result = testObject.BuildServiceTypeDictionary(shipments);
            Assert.Equal("This is a random description", result[1]);
        }

        [Fact]
        public void BuildServiceTypeDictionary_ReturnsServices_WhenServiceIsNotDisabled()
        {
            amazonServiceTypes.Add(new AmazonServiceTypeEntity()
            {
                AmazonServiceTypeID = 1,
                Description = "1",
                ApiValue = "1"
            });

            amazonServiceTypes.Add(new AmazonServiceTypeEntity()
            {
                AmazonServiceTypeID = 2,
                Description = "2",
                ApiValue = "2"
            });

            amazonServiceTypes.Add(new AmazonServiceTypeEntity()
            {
                AmazonServiceTypeID = 3,
                Description = "3",
                ApiValue = "3"
            });

            excludedServices.Add(new ExcludedServiceTypeEntity { ServiceType = 1, ShipmentType = (int)ShipmentTypeCode.AmazonSFP });

            ShipmentEntity[] shipments = { new ShipmentEntity { Amazon = new AmazonShipmentEntity() { ShippingServiceID = "some random service" } } };

            Dictionary<int, string> result = testObject.BuildServiceTypeDictionary(shipments);
            
            Assert.False(result.ContainsKey(1));
            Assert.True(result.ContainsKey(2));
            Assert.True(result.ContainsKey(3));
        }

        [Fact]
        public void BuildServiceTypeDictionary_ReturnsServices_WithUSPSFirst()
        {
            amazonServiceTypes.Add(new AmazonServiceTypeEntity()
            {
                AmazonServiceTypeID = 1,
                Description = "USPS - First",
                ApiValue = "1"
            });

            amazonServiceTypes.Add(new AmazonServiceTypeEntity()
            {
                AmazonServiceTypeID = 2,
                Description = "UPS - Ground",
                ApiValue = "2"
            });

            amazonServiceTypes.Add(new AmazonServiceTypeEntity()
            {
                AmazonServiceTypeID = 4,
                Description = "UPS - Express",
                ApiValue = "4"
            });

            amazonServiceTypes.Add(new AmazonServiceTypeEntity()
            {
                AmazonServiceTypeID = 3,
                Description = "USPS - Priority",
                ApiValue = "3"
            });

            excludedServices.Add(new ExcludedServiceTypeEntity { ServiceType = 1, ShipmentType = (int)ShipmentTypeCode.AmazonSFP });

            ShipmentEntity[] shipments = { new ShipmentEntity { Amazon = new AmazonShipmentEntity() { ShippingServiceID = "some random service" } } };

            Dictionary<int, string> result = testObject.BuildServiceTypeDictionary(shipments);
            
            var expectedResult = new Dictionary<int, string>();
            expectedResult.Add(1, "USPS - First");
            expectedResult.Add(3, "USPS - Priority");
            expectedResult.Add(2, "UPS - Ground");
            expectedResult.Add(4, "UPS - Express");

            Assert.True(result.All(r => expectedResult[r.Key] == r.Value));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}