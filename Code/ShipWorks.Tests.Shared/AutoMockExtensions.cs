using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.Filters;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;
using ShipWorks.Users.Security;

namespace ShipWorks.Tests.Shared
{
    /// <summary>
    /// Additional methods for AutoMocks
    /// </summary>
    public static class AutoMockExtensions
    {
        /// <summary>
        /// Get an AutoMock that is loose and will try and return meaningful defaults instead of null
        /// </summary>
        /// <returns></returns>
        public static AutoMock GetLooseThatReturnsMocks() => AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) {DefaultValue = DefaultValue.Mock});

        /// <summary>
        /// Configure a shipment type that will be returned by an instance of IShipmentTypeFactory
        /// </summary>
        public static Mock<ShipmentType> WithShipmentTypeFromFactory(this AutoMock mock, Action<Mock<ShipmentType>> shipmentTypeConfiguration) =>
            WithShipmentTypeFromFactory<ShipmentType>(mock, shipmentTypeConfiguration);

        /// <summary>
        /// Configure a shipment type that will be returned by an instance of IShipmentTypeFactory
        /// </summary>
        public static Mock<T> WithShipmentTypeFromFactory<T>(this AutoMock mock, Action<Mock<T>> shipmentTypeConfiguration) where T : ShipmentType
        {
            var shipmentTypeMock = mock.MockRepository.Create<T>();
            shipmentTypeConfiguration(shipmentTypeMock);

            mock.Mock<IShipmentTypeFactory>()
                .Setup(x => x.Get(It.IsAny<ShipmentTypeCode>()))
                .Returns(shipmentTypeMock.Object);

            mock.Mock<IShipmentTypeFactory>()
                .Setup(x => x.Get(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentTypeMock.Object);

            return shipmentTypeMock;
        }

        /// <summary>
        /// Configure a shipment type that will be injected directly; not through a factory
        /// </summary>
        public static Mock<T> WithShipmentType<T>(this AutoMock mock, Action<Mock<T>> shipmentTypeConfiguration) where T : ShipmentType
        {
            var shipmentTypeMock = mock.MockRepository.Create<T>();
            shipmentTypeConfiguration(shipmentTypeMock);
            mock.Provide(shipmentTypeMock.Object);
            return shipmentTypeMock;
        }

        /// <summary>
        /// Configure an IShippingConfiguration
        /// </summary>
        public static Mock<IShippingConfiguration> WithShippingConfiguration(this AutoMock mock, bool autoCreateShipments = true, bool userHasPermission = true, bool getAddressValidation = true)
        {
            Mock<IShippingConfiguration> mockObject = mock.Mock<IShippingConfiguration>();
            mockObject.Setup(s => s.AutoCreateShipments).Returns(autoCreateShipments);
            mockObject.Setup(s => s.UserHasPermission(It.IsAny<PermissionType>(), It.IsAny<long>())).Returns(userHasPermission);
            mockObject.Setup(s => s.GetAddressValidation(It.IsAny<ShipmentEntity>())).Returns(getAddressValidation);

            return mockObject;
        }

        /// <summary>
        /// Configure an IFilterHelper
        /// </summary>
        public static Mock<IFilterHelper> WithFilterHelper(this AutoMock mock, bool result = true)
        {
            Mock<IFilterHelper> mockObject = mock.Mock<IFilterHelper>();
            mockObject.Setup(s => s.EnsureFiltersUpToDate(It.IsAny<TimeSpan>())).Returns(result);

            return mockObject;
        }

        /// <summary>
        /// Configure an IValidator<ShipmentEntity>
        /// </summary>
        public static Mock<IValidator<ShipmentEntity>> WithAddressValidator(this AutoMock mock, bool result = true)
        {
            Mock<IValidator<ShipmentEntity>> mockObject = mock.Mock<IValidator<ShipmentEntity>>();
            mockObject.Setup(av => av.ValidateAsync(It.IsAny<ShipmentEntity>())).Returns(Task.FromResult(result));

            return mockObject;
        }

        /// <summary>
        /// Configure an TestStoreType
        /// </summary>
        public static Mock<TestStoreType> WithTestStoreType(this AutoMock mock)
        {
            Mock<TestStoreType> storeTypeMock = mock.MockRepository.Create<TestStoreType>();
            storeTypeMock.Setup(s => s.ShippingAddressEditableState(It.IsAny<ShipmentEntity>())).Returns(ShippingAddressEditStateType.Editable);

            return storeTypeMock;
        }

        /// <summary>
        /// Configure an IStoreTypeManager
        /// </summary>
        public static Mock<IStoreTypeManager> WithStoreTypeManager(this AutoMock mock, StoreType storeType)
        {
            Mock<IStoreTypeManager> mockObject = mock.Mock<IStoreTypeManager>();

            mockObject.Setup(s => s.GetType(It.IsAny<StoreTypeCode>())).Returns(storeType);
            mockObject.Setup(s => s.GetType(It.IsAny<StoreEntity>())).Returns(storeType);

            return mockObject;
        }

        /// <summary>
        /// Configure an IShippingManager
        /// </summary>
        public static Mock<IShippingManager> WithShippingManager(this AutoMock mock, long orderID,
            IEnumerable<ICarrierShipmentAdapter> createIfNoneShipments, IEnumerable<ICarrierShipmentAdapter> dontCreateIfNoneShipments)
        {
            Mock<IShippingManager> mockObject = mock.Mock<IShippingManager>();

            mockObject.Setup(s => s.GetShipments(orderID, true)).Returns(createIfNoneShipments);
            mockObject.Setup(s => s.GetShipments(orderID, false)).Returns(dontCreateIfNoneShipments);
            mockObject.Setup(s => s.GetShipments(0, It.IsAny<bool>())).Throws<Exception>();

            return mockObject;
        }

        /// <summary>
        /// Configure an ICarrierShipmentAdapterFactory
        /// </summary>
        public static Mock<ICarrierShipmentAdapterFactory> WithCarrierShipmentAdapterFactory(this AutoMock mock, ICarrierShipmentAdapter shipmentAdapter)
        {
            Mock<ICarrierShipmentAdapterFactory> mockObject = mock.Mock<ICarrierShipmentAdapterFactory>();

            mockObject.Setup(s => s.Get(It.IsAny<ShipmentEntity>())).Returns(shipmentAdapter);

            return mockObject;
        }

        /// <summary>
        /// Configure an ICarrierShipmentAdapter
        /// </summary>
        public static Mock<ICarrierShipmentAdapter> WithCarrierShipmentAdapter(this AutoMock mock, ShipmentEntity shipment, bool isDomestic)
        {
            Mock<ICarrierShipmentAdapter> mockObject = mock.Mock<ICarrierShipmentAdapter>();

            mockObject.Setup(s => s.Shipment).Returns(shipment);
            mockObject.Setup(s => s.IsDomestic).Returns(isDomestic);

            return mockObject;
        }

        /// <summary>
        /// Return a shipment adapter when IShippingManager.ChangeShipmentType is called
        /// </summary>
        public static Mock<ICarrierShipmentAdapter> WithCarrierShipmentAdapterFromChangeShipment(this AutoMock mock, Action<Mock<ICarrierShipmentAdapter>> configureAdapter)
        {
            var adapter = mock.Mock<ICarrierShipmentAdapter>();
            configureAdapter(adapter);

            mock.Mock<IShippingManager>()
                .Setup(x => x.ChangeShipmentType(It.IsAny<ShipmentTypeCode>(), It.IsAny<ShipmentEntity>()))
                .Returns(adapter.Object);

            return adapter;
        }

        /// <summary>
        /// Configure an IStoreManager
        /// </summary>
        public static Mock<IStoreManager> WithStoreManager(this AutoMock mock, StoreEntity store)
        {
            Mock<IStoreManager> mockObject = mock.Mock<IStoreManager>();

            mockObject.Setup(s => s.GetStore(It.IsAny<long>())).Returns(store);

            return mockObject;
        }
    }
}
