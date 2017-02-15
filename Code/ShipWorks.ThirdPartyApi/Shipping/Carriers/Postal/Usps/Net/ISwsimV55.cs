using System;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net
{
    /// <summary>
    /// Interface around the Stamps WebService
    /// </summary>
    public interface ISwsimV55 : IDisposable
    {
        /// <summary>
        /// Url of the web service
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Cleanse address call has completed
        /// </summary>
        event CleanseAddressCompletedEventHandler CleanseAddressCompleted;

        /// <summary>
        /// Authenticate a user
        /// </summary>
        string AuthenticateUser(Credentials credentials, out DateTime lastLoginTime, out bool clearCredential,
            out string bannerText, out bool passwordExpired, out bool codewordsSet);

        /// <summary>
        /// Get account info
        /// </summary>
        string GetAccountInfo(object Item, out AccountInfo AccountInfo, out Address Address, out string CustomerEmail);

        /// <summary>
        /// Get account info
        /// </summary>
        AccountInfoResult GetAccountInfo(Credentials credentials);

        /// <summary>
        /// Get the URL
        /// </summary>
        string GetURL(object Item, UrlType URLType, string ApplicationContext, out string URL);

        /// <summary>
        /// Purchase postage
        /// </summary>
        string PurchasePostage(object Item, decimal PurchaseAmount, decimal ControlTotal, MachineInfo MI,
            string IntegratorTxID, bool? SendEmail, bool SendEmailSpecified, string Merchant,
            out PurchaseStatus PurchaseStatus, out int TransactionID, out PostageBalance PostageBalance,
            out string RejectionReason, out bool MIRequired, out PurchaseRejectionCode? RejectionCode);

        /// <summary>
        /// Get rates
        /// </summary>
        RateV20[] GetRates(Credentials account, RateV20 rate);

        /// <summary>
        /// Cleanse the address
        /// </summary>
        void CleanseAddressAsync(object Item, Address Address, string FromZIPCode);

        /// <summary>
        /// Register an account
        /// </summary>
        RegistrationStatus RegisterAccount(
            Guid IntegrationID,
            string UserName,
            string Password,
            CodewordType Codeword1Type,
            bool Codeword1TypeSpecified,
            string Codeword1,
            CodewordType Codeword2Type,
            bool Codeword2TypeSpecified,
            string Codeword2,
            Address PhysicalAddress,
            Address MailingAddress,
            MachineInfo MachineInfo,
            string Email,
            AccountType AccountType,
            string PromoCode,
            object Item,
            bool? SendEmail,
            bool SendEmailSpecified,
            out string SuggestedUserName,
            out int UserId,
            out string PromoUrl);

        /// <summary>
        /// Create a scan form
        /// </summary>
        string CreateScanForm(object Item, Guid[] StampsTxIDs, Address FromAddress, ImageType ImageType,
            bool PrintInstructions, Carrier Carrier, DateTime? ShipDate, bool ShipDateSpecified,
            out string ScanFormId, out string Url);

        /// <summary>
        /// Cancel an Indicium
        /// </summary>
        string CancelIndicium(object Item, object Item1, bool? SendEmail, bool SendEmailSpecified);

        /// <summary>
        /// Create an Indicium
        /// </summary>
        CreateIndiciumResult CreateEnvelopeIndicium(CreateEnvelopeIndiciumParameters parameters);

        /// <summary>
        /// Create an Indicium
        /// </summary>
        CreateIndiciumResult CreateIndicium(CreateIndiciumParameters parameters);

        /// <summary>
        /// Change the plan
        /// </summary>
        string ChangePlan(object Item, int PlanId, string PromoCode, bool? SendEmail, bool SendEmailSpecified,
            out PurchaseStatus PurchaseStatus, out int TransactionID, out string RejectionReason);
    }
}
