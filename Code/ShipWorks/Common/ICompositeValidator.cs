namespace ShipWorks.Common
{
    /// <summary>
    /// Composite set of validators
    /// </summary>
    public interface ICompositeValidator<T, K> where T : IValidator<K>
    {
        /// <summary>
        /// Apply the validators, collecting the results
        /// </summary>
        ICompositeValidatorResult Apply(K input);
    }
}
