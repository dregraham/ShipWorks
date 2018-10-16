﻿using System;
using System.Reactive.Concurrency;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using Moq.Language;
using Moq.Language.Flow;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;

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
        public static AutoMock GetLooseThatReturnsMocks() => AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });

        /// <summary>
        /// Build an object for the given type
        /// </summary>
        /// <returns>
        /// This is meant to be used where we want to get an instance of a type or interface, but don't
        /// care to have it be part of the mock system. This is to replace usages of mock.Create for
        /// objects that are NOT the system under test.
        /// </returns>
        public static T Build<T>(this AutoMock mock) where T : class =>
            mock.CreateMock<T>().Object;

        /// <summary>
        /// Override the specified type's registration
        /// </summary>
        public static Mock<T> Override<T>(this AutoMock autoMock) where T : class
        {
            Mock<T> mock = autoMock.CreateMock<T>();

            autoMock.Provide<T>(mock.Object);

            return mock;
        }

        /// <summary>
        /// Gets a shipment type that will be returned by an instance of IShipmentTypeManager
        /// </summary>
        public static Mock<ShipmentType> WithShipmentTypeFromShipmentManager(this AutoMock mock) =>
            WithShipmentTypeFromShipmentManager<ShipmentType>(mock, x => { });

        /// <summary>
        /// Configure a shipment type that will be returned by an instance of IShipmentTypeManager
        /// </summary>
        public static Mock<ShipmentType> WithShipmentTypeFromShipmentManager(this AutoMock mock, Action<Mock<ShipmentType>> shipmentTypeConfiguration) =>
            WithShipmentTypeFromShipmentManager<ShipmentType>(mock, shipmentTypeConfiguration);

        /// <summary>
        /// Configure a shipment type that will be returned by an instance of IShipmentTypeManager
        /// </summary>
        public static Mock<T> WithShipmentTypeFromShipmentManager<T>(this AutoMock mock, Action<Mock<T>> shipmentTypeConfiguration) where T : ShipmentType
        {
            var shipmentTypeMock = mock.MockRepository.Create<T>();
            shipmentTypeConfiguration(shipmentTypeMock);

            mock.Mock<IShipmentTypeManager>()
                .Setup(x => x.Get(It.IsAny<ShipmentTypeCode>()))
                .Returns(shipmentTypeMock.Object);

            mock.Mock<IShipmentTypeManager>()
                .Setup(x => x.Get(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentTypeMock.Object);

            mock.Mock<IFactory<ShipmentTypeCode, ShipmentType>>()
                .Setup(x => x.Create(It.IsAny<ShipmentTypeCode>()))
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
        /// Configure an IFilterHelper
        /// </summary>
        public static Mock<IFilterHelper> WithFilterHelper(this AutoMock mock, bool result)
        {
            Mock<IFilterHelper> mockObject = mock.Mock<IFilterHelper>();

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
        /// Create a mock from the repository
        /// </summary>
        public static Mock<T> CreateMock<T>(this AutoMock mock) where T : class =>
            CreateMock<T>(mock, null);

        /// <summary>
        /// Create a mock from the repository
        /// </summary>
        public static Mock<T> CreateMock<T>(this AutoMock mock, Action<Mock<T>> configure) where T : class
        {
            var mockedObject = mock.MockRepository.Create<T>();
            configure?.Invoke(mockedObject);
            return mockedObject;
        }

        /// <summary>
        /// Add or override an Autofac registration
        /// </summary>
        public static void AddRegistration(this AutoMock mock, Action<ContainerBuilder> buildRegistration)
        {
            ContainerBuilder builder = new ContainerBuilder();

            buildRegistration(builder);

            foreach (var registration in builder.Build().ComponentRegistry.Registrations)
            {
                mock.Container.ComponentRegistry.Register(registration);
            }
        }

        /// <summary>
        /// Mocks a Function with an input and output.
        /// The Func to create the logger is registered and the passed in functionOutput is the output of the function
        /// </summary>
        /// <typeparam name="TInput">The type of input.</typeparam>
        /// <typeparam name="TOutput">The type the Func will return.</typeparam>
        /// <param name="mock">The mock.</param>
        public static Mock<Func<TInput, TOutput>> MockFunc<TInput, TOutput>(this AutoMock mock) where TOutput : class
        {
            Mock<Func<TInput, TOutput>> function = mock.MockRepository.Create<Func<TInput, TOutput>>();
            mock.Provide(function.Object);
            return function;
        }

        /// <summary>
        /// Mocks a Function with an input and output.
        /// The Func to create the logger is registered and the passed in functionOutput is the output of the function
        /// </summary>
        /// <typeparam name="TInput">The type of input.</typeparam>
        /// <typeparam name="TOutput">The type the Func will return.</typeparam>
        /// <param name="mock">The mock.</param>
        /// <param name="functionOutput">The actual output of the function.</param>
        /// <remarks>
        /// To use MockFunc, if there is a class with a parameter of Func&lt;string, IBlah&gt;, you can easily mock up this function
        /// First, create the object you want as the result. (var blah = mock.MockRepository.Create&lt;IBlah&gt;();)
        /// Then call mock.MockFunc&lt;string, IBlah&gt;(blah);
        /// </remarks>
        public static Mock<Func<TInput, TOutput>> MockFunc<TInput, TOutput>(this AutoMock mock, Mock<TOutput> functionOutput) where TOutput : class
        {
            var func = mock.MockFunc<TInput, TOutput>();
            func.Setup(f => f(It.IsAny<TInput>())).Returns(functionOutput.Object);
            return func;
        }

        /// <summary>
        /// Mocks a function with null parameters that returns the output provided.
        /// </summary>
        public static void MockFunc<TOutput>(this AutoMock mock, Mock<TOutput> functionOutput) where TOutput : class
        {
            Mock<Func<TOutput>> function = mock.MockRepository.Create<Func<TOutput>>();
            function.Setup(func => func()).Returns(functionOutput.Object);
            mock.Provide(function.Object);
        }

        /// <summary>
        /// Simplify returning mocked objects
        /// </summary>
        public static IReturnsResult<TIn> Returns<TIn, TOut>(this IReturns<TIn, TOut> returns, Mock<TOut> returnMock)
            where TIn : class
            where TOut : class
        {
            return returns.Returns(returnMock.Object);
        }

        /// <summary>
        /// Simplify returning mocked objects
        /// </summary>
        public static IReturnsResult<TIn> Returns<TIn, TOut>(this IReturnsGetter<TIn, TOut> returns, Mock<TOut> returnMock)
            where TIn : class
            where TOut : class
        {
            return returns.Returns(returnMock.Object);
        }

        /// <summary>
        /// Creates a Mock of ISchedulerProvider and provides it to the mock.
        /// All schedulers are set to use ImmediateScheduler.Instance
        /// </summary>
        public static Mock<ISchedulerProvider> WithMockImmediateScheduler(this AutoMock mock)
        {
            Mock<ISchedulerProvider> schedulerProvider = mock.Mock<ISchedulerProvider>();

            schedulerProvider.Setup(sp => sp.CurrentThread).Returns(ImmediateScheduler.Instance);
            schedulerProvider.Setup(sp => sp.Default).Returns(ImmediateScheduler.Instance);
            schedulerProvider.Setup(sp => sp.TaskPool).Returns(ImmediateScheduler.Instance);
            schedulerProvider.Setup(sp => sp.Dispatcher).Returns(ImmediateScheduler.Instance);
            schedulerProvider.Setup(sp => sp.Immediate).Returns(ImmediateScheduler.Instance);
            schedulerProvider.Setup(sp => sp.NewThread).Returns(ImmediateScheduler.Instance);
            schedulerProvider.Setup(sp => sp.ThreadPool).Returns(ImmediateScheduler.Instance);
            schedulerProvider.Setup(sp => sp.WindowsFormsEventLoop).Returns(ImmediateScheduler.Instance);

            return schedulerProvider;
        }

        /// <summary>
        /// Get a keyed mock creator for the given service
        /// </summary>
        public static IKeyedMockCreator<T> CreateKeyedMockOf<T>(this AutoMock mock) where T : class =>
            new KeyedMockCreator<T>(mock);

        /// <summary>
        /// Get a mock from a given factory method
        /// </summary>
        public static IMockFactory<TFactory> FromFactory<TFactory>(this AutoMock mock) where TFactory : class =>
            new MockFactory<TFactory>(mock);
    }
}
