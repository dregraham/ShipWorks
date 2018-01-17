using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Settings;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressShipmentServiceBuilderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IExcludedServiceTypeRepository> excludedServiceTypeRepository;
        private readonly List<ExcludedServiceTypeEntity> excludedServices;
        private readonly DhlExpressShipmentServicesBuilder testObject;

        public DhlExpressShipmentServiceBuilderTest()
        {
            excludedServices = new List<ExcludedServiceTypeEntity>();

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            
            excludedServiceTypeRepository = mock.Mock<IExcludedServiceTypeRepository>();
            excludedServiceTypeRepository.Setup(s => s.GetExcludedServiceTypes(It.IsAny<ShipmentType>()))
                .Returns(excludedServices);

            testObject = mock.Create<DhlExpressShipmentServicesBuilder>();
        }

        [Theory]
        [InlineData(DhlExpressServiceType.ExpressWorldWide)]
        [InlineData(DhlExpressServiceType.ExpressEnvelope)]
        public void BuildServiceTypeDictionary_ReturnsService_WhenShipmentsAreNullAndServiceIsNotExcluded(DhlExpressServiceType serviceType)
        {
            var results = testObject.BuildServiceTypeDictionary(null);

            Assert.Contains((int) serviceType, results.Keys);            
        }

        [Fact]
        public void BuildServiceTypeDictionary_ReturnsAllServices_WhenNoServicesAreExcluded()
        {
            var results = testObject.BuildServiceTypeDictionary(null);

            Assert.Equal(EnumHelper.GetEnumList<DhlExpressServiceType>().Count(), results.Count);
        }

        [Fact]
        public void BuildServiceTypeDictionary_DelegatesToExcludedServiceTypeRepository()
        {            
            testObject.BuildServiceTypeDictionary(null);

            excludedServiceTypeRepository.Verify(r => r.GetExcludedServiceTypes(It.IsAny<DhlExpressShipmentType>()));
        }

        [Theory]
        [InlineData(DhlExpressServiceType.ExpressWorldWide)]
        [InlineData(DhlExpressServiceType.ExpressEnvelope)]
        public void BuildServiceTypeDictionary_ReturnsService_WhenShipmentIncludesServiceButItIsExcluded(DhlExpressServiceType serviceType)
        {
            excludedServices.Add(new ExcludedServiceTypeEntity((int) ShipmentTypeCode.DhlExpress, (int) serviceType));
            var results = testObject.BuildServiceTypeDictionary(new[] { CreateShipmentWithService(serviceType) });

            Assert.Contains((int) serviceType, results.Keys);            
        }

        [Theory]
        [InlineData(DhlExpressServiceType.ExpressWorldWide)]
        [InlineData(DhlExpressServiceType.ExpressEnvelope)]
        public void BuildServiceTypeDictionary_DoesNotContainDuplicates_WhenShipmentIncludesServiceAndServiceIsNotExcluded(DhlExpressServiceType serviceType)
        {
            var results = testObject.BuildServiceTypeDictionary(new[] { CreateShipmentWithService(serviceType) });

            Assert.Equal(EnumHelper.GetEnumList<DhlExpressServiceType>().Count(), results.Keys.Distinct().Count());            
        }

        [Theory]
        [InlineData(DhlExpressServiceType.ExpressWorldWide)]
        [InlineData(DhlExpressServiceType.ExpressEnvelope)]
        public void BuildServiceTypeDictionary_DoesNotContainDuplicates_WhenMultipleShipmentsHaveTheSameService(DhlExpressServiceType serviceType)
        {
            var results = testObject.BuildServiceTypeDictionary(new[] {
                CreateShipmentWithService(serviceType),
                CreateShipmentWithService(serviceType)
            });

            Assert.Equal(2, results.Count);            
        }

        private ShipmentEntity CreateShipmentWithService(DhlExpressServiceType service) =>
            new ShipmentEntity { DhlExpress = new DhlExpressShipmentEntity { Service = (int) service } };

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
