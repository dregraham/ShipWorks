
namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// An adapter interface intended to be used from the UspsAutomaticDiscountControl to 
    /// handle reading/writing Endicia/Stamps.com settings.
    /// </summary>
    public interface IUspsAutomaticDiscountControlAdapter
    {
        /// <summary>
        /// Gets or sets a value indicating whether USPS automatic expeditedis being used.
        /// </summary>
        bool UsingUspsAutomaticExpedited { get; set; }

        /// <summary>
        /// Gets or sets the USPS automatic expedited account.
        /// </summary>
        long UspsAutomaticExpeditedAccount { get; set; }
    }
}
