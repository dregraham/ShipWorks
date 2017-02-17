using Autofac.Features.Indexed;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Generic factory
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="TService">Type of service</typeparam>
    /// <remarks>This is just a wrapper for IIndex, but it makes it easier to mock</remarks>
    public class Factory<TKey, TService> : IFactory<TKey, TService>
    {
        readonly IIndex<TKey, TService> lookup;

        /// <summary>
        /// Constructor
        /// </summary>
        public Factory(IIndex<TKey, TService> lookup)
        {
            this.lookup = lookup;
        }

        /// <summary>
        /// Create a service for the given key
        /// </summary>
        public TService Create(TKey key) => lookup[key];
    }
}
