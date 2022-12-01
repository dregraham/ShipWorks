namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Implemented by store types that pull etsy orders
    /// </summary>
    public interface IEtsyCredentials
	{
		/// <summary>
		/// Etsy merchant ID
		/// </summary>
		string MerchantID { get; set; }

		/// <summary>
		/// Etsy auth token
		/// </summary>
		string AuthToken { get; set; }
	}
}
