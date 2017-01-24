namespace ShipWorks.Common
{
    /// <summary>
    /// Apply an ordered set of manipulators
    /// </summary>
    public interface IOrderedCompositeManipulator<T, K> where T : IManipulator<K>
    {
        /// <summary>
        /// Apply all the manipulators of the specified type
        /// </summary>
        K Apply(K input);
    }
}
