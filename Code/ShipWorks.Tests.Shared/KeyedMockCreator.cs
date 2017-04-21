using System;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Moq;

namespace ShipWorks.Tests.Shared
{
    /// <summary>
    /// Create keyed mocks using an IIndex
    /// </summary>
    internal class KeyedMockCreator<T> : IKeyedMockCreator<T> where T : class
    {
        object existingIndex;
        private AutoMock mock;

        /// <summary>
        /// Constructor
        /// </summary>
        internal KeyedMockCreator(AutoMock mock)
        {
            this.mock = mock;
        }

        /// <summary>
        /// Get a mock for the given key
        /// </summary>
        public Mock<T> For<K>(K key)
        {
            var mockT = mock.CreateMock<T>();
            GetIndexFor<K>().Setup(x => x[key]).Returns(mockT);

            return mockT;
        }

        /// <summary>
        /// Get an index for the given type
        /// </summary>
        private Mock<IIndex<K, T>> GetIndexFor<K>() =>
            GetExistingIndex<K>() ?? CreateNewIndex<K>();

        /// <summary>
        /// Create a new index
        /// </summary>
        private Mock<IIndex<K, T>> CreateNewIndex<K>()
        {
            var typedIndex = mock.Override<IIndex<K, T>>();
            existingIndex = typedIndex;
            return typedIndex;
        }

        /// <summary>
        /// Attempt to get an existing index
        /// </summary>
        private Mock<IIndex<K, T>> GetExistingIndex<K>()
        {
            if (existingIndex == null)
            {
                return null;
            }

            var typedIndex = existingIndex as Mock<IIndex<K, T>>;
            if (typedIndex == null)
            {
                throw new InvalidOperationException("Mocks created from this creator must all have keys of the same type");
            }

            return typedIndex;
        }
    }
}