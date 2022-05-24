using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.DhlEcommerce;
using ShipWorks.Shipping.Settings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.DhlEcommerce
{
    public class DhlEcommerceShipmentServicesBuilderTest
    {
        private readonly AutoMock mock;
        private readonly DhlEcommerceShipmentType shipmentType;
        private readonly Mock<IExcludedServiceTypeRepository> excludedServiceRepo;
        private readonly Mock<IShippingManager> shippingManager;

        public DhlEcommerceShipmentServicesBuilderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipmentType = mock.Create<DhlEcommerceShipmentType>();
            excludedServiceRepo = mock.Mock<IExcludedServiceTypeRepository>();
            shippingManager = mock.Mock<IShippingManager>();
            shippingManager.Setup(s => s.GetOverriddenStoreShipment(It.IsAny<ShipmentEntity>())).Returns<ShipmentEntity>(x => x);
        }

        [Fact]
        public void BuildServiceTypeDictionary_ReturnsEmpty_IfAllShipmentsNotSameInternational()
        {
            var shipments = GetShipments((0, "GB"), (0, "US"));

            var testObj = GetTestObject();
            var result = testObj.BuildServiceTypeDictionary(shipments);
            Assert.Empty(result);
        }

        [Fact]
        public void BuildServiceTypeDictionary_ReturnsCorrectServices_IfAllCanada()
        {
            var shipments = GetShipments((0, "CA"), (0, "CA"));

            var testObj = GetTestObject();
            var result = testObj.BuildServiceTypeDictionary(shipments);
            var internationalServices = GetServiceTypes().Where(s => EnumHelper.GetInternationalServiceAttribute(s).IsInternational);

            var test = result.All(r => internationalServices.Contains((DhlEcommerceServiceType) r.Key));
            Assert.True(test);
        }

        [Fact]
        public void BuildServiceTypeDictionary_ReturnsCorrectServices_IfAllInternational()
        {
            var shipments = GetShipments((0, "GB"), (0, "GB"));

            var testObj = GetTestObject();
            var result = testObj.BuildServiceTypeDictionary(shipments);
            var internationalServices = GetServiceTypes().Where(s =>
            {
                var attribute = EnumHelper.GetInternationalServiceAttribute(s);
                return attribute.IsInternational && string.IsNullOrEmpty(attribute.CountryCodeRestriction);
            });

            var test = result.All(r => internationalServices.Contains((DhlEcommerceServiceType) r.Key));
            Assert.True(test);
        }

        [Fact]
        public void BuildServiceTypeDictionary_ReturnsCorrectServices_IfAllDomestic()
        {
            var shipments = GetShipments((12, "US"), (12, "US"));

            var testObj = GetTestObject();
            var result = testObj.BuildServiceTypeDictionary(shipments);
            var domesticServices = GetServiceTypes().Where(s => !EnumHelper.GetInternationalServiceAttribute(s).IsInternational);

            var test = result.All(r => domesticServices.Contains((DhlEcommerceServiceType) r.Key));
            Assert.True(test);
        }

        [Fact]
        public void BuildServiceTypeDictionary_ReturnsServices_WithAlreadySelectedServiceIncluded()
        {
            var shipments = GetShipments((12, "CA"), (0, "CA"));

            var testObj = GetTestObject();
            var result = testObj.BuildServiceTypeDictionary(shipments);

            Assert.Contains(12, result.Keys);
        }

        private DhlEcommerceShipmentServicesBuilder GetTestObject() => new DhlEcommerceShipmentServicesBuilder(shipmentType, excludedServiceRepo.Object, shippingManager.Object);
        private List<ShipmentEntity> GetShipments(params (int service, string country)[] shipments) => shipments.Select(s => new ShipmentEntity()
        {
            OriginCountryCode = "US",
            ShipCountryCode = s.country,
            DhlEcommerce = new DhlEcommerceShipmentEntity()
            {
                Service = s.service
            }
        }).ToList();

        private List<DhlEcommerceServiceType> GetServiceTypes() => Enum.GetValues(typeof(DhlEcommerceServiceType)).Cast<DhlEcommerceServiceType>().ToList();
    }
}
