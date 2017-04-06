namespace ShipWorks.Shipping.Carriers.UPS.LocalRating
{
    /// <summary>
    /// Represents a Ups Local Rates Control
    /// </summary>
    public interface IUpsLocalRatingControl
    {
        /// <summary>
        /// The DataContext of the control
        /// </summary>
        object DataContext { get; set; }
    }
}
