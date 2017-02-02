using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services
{
    public class ShippingOriginManagerWrapperTest
    {
        [Fact]
        public void GetOriginAddress_ReturnsNull_WhenOriginIdIsOther()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var testObject = mock.Create<ShippingOriginManagerWrapper>();
                var result = testObject.GetOriginAddress((long) ShipmentOriginSource.Other, 0, 0, ShipmentTypeCode.Usps);
                Assert.Null(result);
            }
        }

        [Fact]
        public void GetOriginAddress_GetsStoreFromManager_WhenOriginIdIsStore()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.Mock<IStoreManager>()
                    .Setup(x => x.GetRelatedStore(12))
                    .Returns(new StoreEntity { City = "Bar" })
                    .Verifiable();

                var testObject = mock.Create<ShippingOriginManagerWrapper>();
                testObject.GetOriginAddress((long) ShipmentOriginSource.Store, 12, 0, ShipmentTypeCode.Usps);

                mock.VerifyAll = true;
            }
        }

        [Fact]
        public void GetOriginAddress_ReturnsStoreAddress_WhenOriginIdIsStore()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.Mock<IStoreManager>()
                    .Setup(x => x.GetRelatedStore(It.IsAny<long>()))
                    .Returns(new StoreEntity { City = "Bar" });

                var testObject = mock.Create<ShippingOriginManagerWrapper>();
                var result = testObject.GetOriginAddress((long) ShipmentOriginSource.Store, 12, 0, ShipmentTypeCode.Usps);

                Assert.Equal("Bar", result.City);
            }
        }

        [Fact]
        public void GetOriginAddress_DelegatesToAccountRetrieverFactory_WhenOriginIdIsAccount()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var testObject = mock.Create<ShippingOriginManagerWrapper>();
                testObject.GetOriginAddress((long) ShipmentOriginSource.Account, 12, 0, ShipmentTypeCode.Usps);

                mock.Mock<ICarrierAccountRetrieverFactory>()
                    .Verify(x => x.Create(ShipmentTypeCode.Usps));
            }
        }

        [Fact]
        public void GetOriginAddress_DelegatesToAccountRetriever_WhenOriginIdIsAccount()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var retriever = mock.Mock<ICarrierAccountRetriever>();

                mock.Mock<ICarrierAccountRetrieverFactory>()
                    .Setup(x => x.Create(It.IsAny<ShipmentTypeCode>()))
                    .Returns(retriever.Object);

                var testObject = mock.Create<ShippingOriginManagerWrapper>();
                testObject.GetOriginAddress((long) ShipmentOriginSource.Account, 0, 13, ShipmentTypeCode.Usps);

                retriever.Verify(x => x.GetAccountReadOnly(13));
            }
        }

        [Fact]
        public void GetOriginAddress_ReturnsAccountAddress_WhenOriginIdIsAccount()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var retriever = mock.Mock<ICarrierAccountRetriever>();
                retriever.Setup(x => x.GetAccountReadOnly(It.IsAny<long>()))
                    .Returns(new UspsAccountEntity { City = "Foo" });

                mock.Mock<ICarrierAccountRetrieverFactory>()
                    .Setup(x => x.Create(It.IsAny<ShipmentTypeCode>()))
                    .Returns(retriever.Object);

                var testObject = mock.Create<ShippingOriginManagerWrapper>();
                var result = testObject.GetOriginAddress((long) ShipmentOriginSource.Account, 0, 13, ShipmentTypeCode.Usps);

                Assert.Equal("Foo", result.City);
            }
        }
    }
}
