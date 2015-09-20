using Autofac.Extras.Moq;
using Moq;
using Xunit;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Settings;
using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx
{
    public static class ShippingAutoMockExtensions
    {
        public static void WithShipmentType<T>(this AutoMock mock, Action<Mock<T>> shipmentTypeConfiguration) where T : ShipmentType
        {
            var shipmentTypeMock = mock.MockRepository.Create<T>();
            shipmentTypeConfiguration(shipmentTypeMock);
            mock.Provide(shipmentTypeMock.Object);
        }
    }

    public class iParcelShipmentServicesBuilderTest
    {
        [Fact]
        public void BuildServiceTypeDictionary_ReturnsImmediate_ShipmentsAreNullAndImmediateIsNotNotExcluded()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.WithShipmentType<iParcelShipmentType>(s => {
                    s.Setup(x => x.GetAvailableServiceTypes(It.IsAny<IExcludedServiceTypeRepository>()))
                        .Returns(new[] { (int)iParcelServiceType.Immediate });
                });

                var testObject = mock.Create<iParcelShipmentServicesBuilder>();
                var results = testObject.BuildServiceTypeDictionary(null);
                Assert.Contains((int)iParcelServiceType.Immediate, results.Keys);
            }
        }

        [Fact]
        public void BuildServiceTypeDictionary_DelegatesToExcludedServiceTypeRepository()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var excludedServiceTypeRepository = mock.Mock<IExcludedServiceTypeRepository>();

                mock.WithShipmentType<iParcelShipmentType>(s => {
                    s.Setup(x => x.GetAvailableServiceTypes(excludedServiceTypeRepository.Object))
                        .Returns(new[] { (int)iParcelServiceType.Immediate })
                        .Verifiable();
                });

                var testObject = mock.Create<iParcelShipmentServicesBuilder>();
                testObject.BuildServiceTypeDictionary(null);

                mock.VerifyAll = true;
            }
        }

        [Fact]
        public void BuildServiceTypeDictionary_ReturnsPrefered_ShipmentIncludesPreferedButItIsNotAvailable()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.WithShipmentType<iParcelShipmentType>(s => {
                    s.Setup(x => x.GetAvailableServiceTypes(It.IsAny<IExcludedServiceTypeRepository>()))
                        .Returns(new[] { (int)iParcelServiceType.Immediate });
                });

                var testObject = mock.Create<iParcelShipmentServicesBuilder>();
                var results = testObject.BuildServiceTypeDictionary(new[] { CreateShipmentWithService(iParcelServiceType.Preferred) });

                Assert.Contains((int)iParcelServiceType.Immediate, results.Keys);
                Assert.Contains((int)iParcelServiceType.Preferred, results.Keys);
            }
        }
        
        [Fact]
        public void BuildServiceTypeDictionary_DoesNotContainDuplicates_ShipmentIncludesPreferedAndItIsAvailable()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.WithShipmentType<iParcelShipmentType>(s => {
                    s.Setup(x => x.GetAvailableServiceTypes(It.IsAny<IExcludedServiceTypeRepository>()))
                        .Returns(new[] { (int)iParcelServiceType.Preferred });
                });

                var testObject = mock.Create<iParcelShipmentServicesBuilder>();
                var results = testObject.BuildServiceTypeDictionary(new[] { CreateShipmentWithService(iParcelServiceType.Preferred) });

                Assert.Equal(1, results.Count);
            }
        }

        private ShipmentEntity CreateShipmentWithService(iParcelServiceType service) =>
            new ShipmentEntity { IParcel = new IParcelShipmentEntity { Service = (int)service } };
    }
}
