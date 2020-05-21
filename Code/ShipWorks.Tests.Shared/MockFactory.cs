﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;

namespace ShipWorks.Tests.Shared
{
    /// <summary>
    /// Create mocks from factory methods
    /// </summary>
    internal class MockFactory<TFactory> : IMockFactory<TFactory> where TFactory : class
    {
        private readonly AutoMock mock;

        /// <summary>
        /// Constructor
        /// </summary>
        public MockFactory(AutoMock mock)
        {
            this.mock = mock;
        }

        /// <summary>
        /// Create a mock when the given factory method is called
        /// </summary>
        public Mock<TMock> Mock<TMock>(Expression<Func<TFactory, TMock>> factoryMethod) where TMock : class
        {
            var createdMock = mock.CreateMock<TMock>();

            mock.Mock<TFactory>()
                .Setup(factoryMethod)
                .Returns(createdMock);

            return createdMock;
        }

        /// <summary>
        /// Create a mock when the given factory method is called
        /// </summary>
        public Mock<TMock> MockAsync<TMock>(Expression<Func<TFactory, Task<TMock>>> factoryMethod) where TMock : class
        {
            var createdMock = mock.CreateMock<TMock>();

            mock.Mock<TFactory>()
                .Setup(factoryMethod)
                .ReturnsAsync(createdMock.Object);

            return createdMock;
        }
    }
}