namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Generic factory
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="TService">Type of service</typeparam>
    /// <remarks>This is just a wrapper for IIndex, but it makes it easier to mock</remarks>
    public interface IFactory<TKey, TService>
    {
        /// <summary>
        /// Create a service for the given key
        /// </summary>
        TService Create(TKey key);
    }
}
