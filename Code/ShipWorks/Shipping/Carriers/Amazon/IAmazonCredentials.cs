using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Amazon credential interface
    /// </summary>
    public interface IAmazonCredentials
    {
        /// <summary>
        /// Amazon account merchant id
        /// </summary>
        string MerchantId { get; set; }

        /// <summary>
        /// Amazon account authentication token
        /// </summary>
        string AuthToken { get; set; }

        /// <summary>
        /// Was the validation successful
        /// </summary>
        bool Success { get; set; }

        /// <summary>
        /// Message from result of validation
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Validate the credentials
        /// </summary>
        void Validate();

        /// <summary>
        /// Populate the given account with the credential data
        /// </summary>
        void PopulateAccount(AmazonAccountEntity account);
    }
}