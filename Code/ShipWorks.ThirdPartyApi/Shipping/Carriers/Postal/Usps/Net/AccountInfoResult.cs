using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net
{
    /// <summary>
    /// Results of the GetAccountInfo call
    /// </summary>
    public struct AccountInfoResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AccountInfoResult(AccountInfoV37 accountInfo, Address address, string email)
        {
            AccountInfo = accountInfo;
            Address = address;
            Email = email;
        }

        /// <summary>
        /// Account information
        /// </summary>
        public AccountInfoV37 AccountInfo { get; }

        /// <summary>
        /// Address
        /// </summary>
        /// <remarks>Express1 does not return this information</remarks>
        public Address Address { get; }

        /// <summary>
        /// Email
        /// </summary>
        /// <remarks>Express1 does not return this information</remarks>
        public string Email { get; }
    }
}