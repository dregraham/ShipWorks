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
        string GetRates(object Item, RateV20 Rate, out RateV20[] Rates);

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
        /// Create an envelope Indicium
        /// </summary>
        string CreateEnvelopeIndicium(
            object Item,
            ref string IntegratorTxID,
            ref RateV20 Rate,
            Address From,
            Address To,
            string CustomerID,
            CreateIndiciumModeV1 Mode,
            ImageType ImageType,
            int CostCodeId,
            bool HideFIM,
            string RateToken,
            string OrderId,
            out string TrackingNumber,
            out Guid StampsTxID,
            out string URL,
            out PostageBalance PostageBalance,
            out string Mac,
            out string PostageHash);

        /// <summary>
        /// Create an Indicium
        /// </summary>
        CreateIndiciumResult CreateIndicium(CreateIndiciumParameters parameters);

        /// <summary>
        /// Create an Indicium
        /// </summary>
        string CreateIndicium(
            object Item,
            ref string IntegratorTxID,
            ref string TrackingNumber,
            ref RateV20 Rate,
            Address From,
            Address To,
            string CustomerID,
            CustomsV4 Customs,
            bool SampleOnly,
            PostageMode PostageMode,
            ImageType ImageType,
            EltronPrinterDPIType EltronPrinterDPIType,
            string memo,
            int cost_code_id,
            bool deliveryNotification,
            ShipmentNotification ShipmentNotification,
            int rotationDegrees,
            int? horizontalOffset,
            bool horizontalOffsetSpecified,
            int? verticalOffset,
            bool verticalOffsetSpecified,
            int? printDensity,
            bool printDensitySpecified,
            bool? printMemo,
            bool printMemoSpecified,
            bool? printInstructions,
            bool printInstructionsSpecified,
            bool requestPostageHash,
            NonDeliveryOption nonDeliveryOption,
            Address RedirectTo,
            string OriginalPostageHash,
            bool? ReturnImageData,
            bool ReturnImageDataSpecified,
            string InternalTransactionNumber,
            PaperSizeV1 PaperSize,
            LabelRecipientInfo EmailLabelTo,
            bool PayOnPrint,
            int? ReturnLabelExpirationDays,
            bool ReturnLabelExpirationDaysSpecified,
            ImageDpi ImageDpi,
            string RateToken,
            string OrderId,
            out System.Guid StampsTxID,
            out string URL,
            out PostageBalance PostageBalance,
            out string Mac,
            out string PostageHash,
            out byte[][] ImageData);

        /// <summary>
        /// Change the plan
        /// </summary>
        string ChangePlan(object Item, int PlanId, string PromoCode, bool? SendEmail, bool SendEmailSpecified,
            out PurchaseStatus PurchaseStatus, out int TransactionID, out string RejectionReason);
    }
}
