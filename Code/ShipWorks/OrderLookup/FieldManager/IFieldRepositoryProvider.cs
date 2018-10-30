namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// Provides a field layout repository
    /// </summary>
    public interface IFieldRepositoryProvider
    {
        /// <summary>
        /// Field layout repository
        /// </summary>
        IOrderLookupFieldLayoutRepository FieldLayoutRepository { get; }
    }
}
