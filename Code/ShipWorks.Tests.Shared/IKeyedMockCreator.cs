using Moq;

namespace ShipWorks.Tests.Shared
{
    /// <summary>
    /// Create keyed mocks using an IIndex
    /// </summary>
    public interface IKeyedMockCreator<T> where T : class
    {
        /// <summary>
        /// Create a mock for the given key
        /// </summary>
        Mock<T> For<TKey>(TKey basic);
    }
}