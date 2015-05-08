namespace Interapptive.Shared.Business
{
    /// <summary>
    /// Interface for accessing basic address information across adapters
    /// </summary>
    public interface IAddressAdapter
    {
        /// <summary>
        /// Street line 1
        /// </summary>
        string Street1 { get; set; }

        /// <summary>
        /// Street line 2
        /// </summary>
        string Street2 { get; set; }

        /// <summary>
        /// Street line 3
        /// </summary>
        string Street3 { get; set; }

        /// <summary>
        /// City
        /// </summary>
        string City { get; set; }

        /// <summary>
        /// The state or province code
        /// </summary>
        string StateProvCode { get; set; }

        /// <summary>
        /// The country code
        /// </summary>
        string CountryCode { get; set; }

        /// <summary>
        /// The postal code
        /// </summary>
        string PostalCode { get; set; }
    }
}