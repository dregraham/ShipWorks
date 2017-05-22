using System;
using System.Linq.Expressions;
using Moq;

namespace ShipWorks.Tests.Shared
{
    /// <summary>
    /// Create mocks from factory methods
    /// </summary>
    public interface IMockFactory<TFactory> where TFactory : class
    {
        /// <summary>
        /// Create a mock when the given factory method is called
        /// </summary>
        Mock<TMock> Mock<TMock>(Expression<Func<TFactory, TMock>> factoryMethod) where TMock : class;
    }
}