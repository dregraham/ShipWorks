using System;
using Autofac.Extras.Moq;
using ShipWorks.ApplicationCore.Licensing.TangoRequests;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Insurance.InsureShip.Net;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip.Net
{
    public class InsureShipCredentialRetrieverTest : IDisposable
    {
        private readonly AutoMock mock;

        public InsureShipCredentialRetrieverTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Get_ReturnsCredentialsFromStore_WhenCredentialsExist()
        {
            var testObject = mock.Create<InsureShipCredentialRetriever>();
            var store = mock.Mock<IStoreEntity>();
            store.SetupGet(x => x.InsureShipApiKey).Returns("Foo");
            store.SetupGet(x => x.InsureShipClientID).Returns(1234L);

            var result = testObject.Get(store.Object);

            Assert.Equal("Foo", result.ApiKey);
            Assert.Equal("1234", result.ClientID);
        }

        [Fact]
        public void Get_GetsEditableStore_WhenCredentialsDoNotExist()
        {
            var testObject = mock.Create<InsureShipCredentialRetriever>();
            var store = mock.Mock<IStoreEntity>();
            store.SetupGet(x => x.StoreID).Returns(7);

            var result = testObject.Get(store.Object);

            mock.Mock<IStoreManager>().Verify(x => x.GetStore(7));
        }

        [Fact]
        public void Get_ReturnsCredentialFromRetrievedStore_WhenInitialStoreDoesNotHaveCredentials()
        {
            var testObject = mock.Create<InsureShipCredentialRetriever>();
            var store = mock.Mock<IStoreEntity>();
            store.SetupGet(x => x.StoreID).Returns(7);

            var concreteStore = Create.Store<GenericModuleStoreEntity>()
                .Set(x => x.InsureShipApiKey, "Foo")
                .Set(x => x.InsureShipClientID, 1234L)
                .Build();
            mock.Mock<IStoreManager>()
                .Setup(x => x.GetStore(AnyLong))
                .Returns(concreteStore);

            var result = testObject.Get(store.Object);

            Assert.Equal("Foo", result.ApiKey);
            Assert.Equal("1234", result.ClientID);
        }

        [Fact]
        public void Get_GetsCredentialsFromTango_WhenCredentialsDoNotExistOnConcreteStore()
        {
            var testObject = mock.Create<InsureShipCredentialRetriever>();
            var store = Create.Store<StoreEntity>().Build();
            mock.Mock<IStoreManager>().Setup(x => x.GetStore(AnyLong)).Returns(store);

            var result = testObject.Get(mock.Build<IStoreEntity>());

            mock.Mock<ITangoGetInsureShipCredentialsRequest>().Verify(x => x.PopulateCredentials(store));
        }

        [Fact]
        public void Get_SavesStore_AfterCredentialsAreRetrievedFromTango()
        {
            var testObject = mock.Create<InsureShipCredentialRetriever>();
            var store = Create.Store<StoreEntity>().Build();
            mock.Mock<IStoreManager>().Setup(x => x.GetStore(AnyLong)).Returns(store);

            var result = testObject.Get(mock.Build<IStoreEntity>());

            mock.Mock<IStoreManager>().Verify(x => x.SaveStore(store));
        }

        [Fact]
        public void Get_ReturnsTangoCredentials_WhenCredentialsDoNotExistOnConcreteStore()
        {
            var testObject = mock.Create<InsureShipCredentialRetriever>();
            var store = Create.Store<StoreEntity>().Build();
            mock.Mock<IStoreManager>().Setup(x => x.GetStore(AnyLong)).Returns(store);
            mock.Mock<ITangoGetInsureShipCredentialsRequest>()
                .Setup(x => x.PopulateCredentials(AnyStore))
                .Callback((StoreEntity innerStore) =>
                {
                    innerStore.InsureShipApiKey = "Foo";
                    innerStore.InsureShipClientID = 1234L;
                });

            var result = testObject.Get(mock.Build<IStoreEntity>());

            Assert.Equal("Foo", result.ApiKey);
            Assert.Equal("1234", result.ClientID);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
