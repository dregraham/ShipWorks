namespace ShipWorks.Common
{
    /// <summary>
    /// Apply a set of validators
    /// </summary>
    public interface IApplyValidators<T, K> where T : IValidator<K>
    {
        /// <summary>
        /// Apply the validators, collecting the results
        /// </summary>
        IApplyValidatorsResult Apply(K input);
    }
}
