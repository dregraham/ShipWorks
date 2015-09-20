using Autofac.Extras.Moq;
using Moq;
using Xunit;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Tests.Carriers.OnTrac
{
    public class OnTracShipmentServicesBuilderTest
    {
        [Fact]
        public void BuildServiceTypeDictionary_ReturnsGround_ShipmentsAreNullAndGroundIsNotNotExcluded()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.WithShipmentType<OnTracShipmentType>(s => {
                    s.Setup(x => x.GetAvailableServiceTypes(It.IsAny<IExcludedServiceTypeRepository>()))
                        .Returns(new[] { (int)OnTracServiceType.Ground });
                });

                var testObject = mock.Create<OnTracShipmentServicesBuilder>();
                var results = testObject.BuildServiceTypeDictionary(null);
                Assert.Contains((int)OnTracServiceType.Ground, results.Keys);
            }
        }

        [Fact]
        public void BuildServiceTypeDictionary_DelegatesToExcludedServiceTypeRepository()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var excludedServiceTypeRepository = mock.Mock<IExcludedServiceTypeRepository>();

                mock.WithShipmentType<OnTracShipmentType>(s => {
                    s.Setup(x => x.GetAvailableServiceTypes(excludedServiceTypeRepository.Object))
                        .Returns(new[] { (int)OnTracServiceType.Ground })
                        .Verifiable();
                });

                var testObject = mock.Create<OnTracShipmentServicesBuilder>();
                testObject.BuildServiceTypeDictionary(null);

                mock.VerifyAll = true;
            }
        }

        [Fact]
        public void BuildServiceTypeDictionary_ReturnsPrefered_ShipmentIncludesPreferedButItIsNotAvailable()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.WithShipmentType<OnTracShipmentType>(s => {
                    s.Setup(x => x.GetAvailableServiceTypes(It.IsAny<IExcludedServiceTypeRepository>()))
                        .Returns(new[] { (int)OnTracServiceType.Ground });
                });

                var testObject = mock.Create<OnTracShipmentServicesBuilder>();
                var results = testObject.BuildServiceTypeDictionary(new[] { CreateShipmentWithService(OnTracServiceType.Sunrise) });

                Assert.Contains((int)OnTracServiceType.Ground, results.Keys);
                Assert.Contains((int)OnTracServiceType.Sunrise, results.Keys);
            }
        }
        
        [Fact]
        public void BuildServiceTypeDictionary_DoesNotContainDuplicates_ShipmentIncludesPreferedAndItIsAvailable()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.WithShipmentType<OnTracShipmentType>(s => {
                    s.Setup(x => x.GetAvailableServiceTypes(It.IsAny<IExcludedServiceTypeRepository>()))
                        .Returns(new[] { (int)OnTracServiceType.Sunrise });
                });

                var testObject = mock.Create<OnTracShipmentServicesBuilder>();
                var results = testObject.BuildServiceTypeDictionary(new[] { CreateShipmentWithService(OnTracServiceType.Sunrise) });

                Assert.Equal(1, results.Count);
            }
        }

        [Fact]
        public void BuildServiceTypeDictionary_DoesNotContainDuplicates_WhenMultipleShipmentsHaveTheSameService()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.WithShipmentType<OnTracShipmentType>(s => {
                    s.Setup(x => x.GetAvailableServiceTypes(It.IsAny<IExcludedServiceTypeRepository>()))
                        .Returns(new[] { (int)OnTracServiceType.Ground });
                });

                var testObject = mock.Create<OnTracShipmentServicesBuilder>();
                var results = testObject.BuildServiceTypeDictionary(new[] {
                    CreateShipmentWithService(OnTracServiceType.Sunrise),
                    CreateShipmentWithService(OnTracServiceType.Sunrise)
                });

                Assert.Equal(2, results.Count);
            }
        }

        private ShipmentEntity CreateShipmentWithService(OnTracServiceType service) =>
            new ShipmentEntity { OnTrac = new OnTracShipmentEntity { Service = (int)service } };
    }
}
