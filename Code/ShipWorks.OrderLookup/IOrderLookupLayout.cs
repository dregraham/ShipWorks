namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Represents the OrderLookup Layout
    /// </summary>
    public interface IOrderLookupLayout
    {
        /// <summary>
        /// Apply the layout to the view model
        /// </summary>
        void Apply(IOrderLookupViewModel orderLookupViewModel);

        /// <summary>
        /// Save the view model
        /// </summary>
        void Save();
    }
}
