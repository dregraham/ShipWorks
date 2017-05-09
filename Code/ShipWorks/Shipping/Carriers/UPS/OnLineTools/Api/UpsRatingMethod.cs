namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Describes how to get a UPS rate
    /// </summary>
    public enum UpsRatingMethod
    {
        /// <summary>
        /// This describes the method of getting rates via the UPS API
        /// </summary>
        Api,

        /// <summary>
        /// This describes the method of calculating rates in Shipworks using the rating tables
        /// </summary>
        Local
    }
}
