using System;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net
{
    /// <summary>
    /// Interface around the Stamps WebService
    /// </summary>
    public interface ISwsimV69 : IDisposable
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
        string GetAccountInfo(object Item, out AccountInfoV27 AccountInfo, out Address Address, out string CustomerEmail);

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
            string IntegratorTxID, bool? SendEmail, bool SendEmailSpecified,
            out PurchaseStatus PurchaseStatus, out int TransactionID, out PostageBalance PostageBalance,
            out string RejectionReason, out bool MIRequired, out PurchaseRejectionCode? RejectionCode);

        /// <summary>
        /// Get rates
        /// </summary>
        RateV25[] GetRates(Credentials account, RateV25 rate);

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

        /// <summary>
        /// Track a shipment
        /// </summary>
        /// <param name="Item">This is the authenticator</param>
        /// <param name="Item1">This is the tracking number</param>
        /// <param name="TrackingEvents">Collection of TrackingEvent objects, each one representing one scan of the package by the USPS.</param>
        /// <param name="GuaranteedDeliveryDate">USPS provided guaranteed date, if available.</param>
        /// <param name="ExpectedDeliveryDate">USPS provided expected or predicted delivery date.</param>
        /// <param name="ServiceDescription">A plain language description of the service returned, i.e. “USPS Priority Mail”</param>
        /// <param name="Carrier">Specifies carrier of the package.</param>
        /// <param name="DestinationInfo">Specifies the destination the package is headed to, if available.</param>
        /// <returns></returns>
        string TrackShipment(object Item, object Item1, out TrackingEvent[] TrackingEvents, out DateTime? GuaranteedDeliveryDate, 
            out DateTime? ExpectedDeliveryDate, out string ServiceDescription, out string Carrier, out DestinationInfo DestinationInfo);
    }
}
