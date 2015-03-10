namespace ShipWorks.AddressValidation.Predicates
{
    /// <summary>
    /// Limit a predicate query to a number of rows
    /// </summary>
    public interface ILimitResultRows
    {
        /// <summary>
        /// Maximum rows that this predicate should return; 0 returns all rows
        /// </summary>
        int MaximumRows { get; }
    }
}