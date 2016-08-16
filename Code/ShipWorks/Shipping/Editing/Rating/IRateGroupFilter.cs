namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Interface to allow carriers to implement a filter to apply to a list of RateResults
    /// </summary>
    public interface IRateGroupFilter
    {
        /// <summary>
        /// Method that filters a group of rates and returns a new group of the filtered rate results.
        /// </summary>
        RateGroup Filter(RateGroup rateGroup);
    }
}
