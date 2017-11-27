using System;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.WebServices
{
    /// <summary>
    /// Mark the web service as implementing the interface
    /// </summary>
    public partial class SwsimV67 : ISwsimV67
    {
        /// <summary>
        /// Get account info
        /// </summary>
        public AccountInfoResult GetAccountInfo(Credentials credentials)
        {
            AccountInfoV25 accountInfo;
            Address address;
            string email;

            GetAccountInfo(credentials, out accountInfo, out address, out email);

            return new AccountInfoResult(accountInfo, address, email);
        }

        /// <summary>
        /// Get rates
        /// </summary>
        public RateV24[] GetRates(Credentials account, RateV24 rate)
        {
            RateV24[] rateResults;

            GetRates(account, rate, out rateResults);

            return rateResults;
        }

        /// <summary>
        /// Create an envelope Indicium
        /// </summary>
        public CreateIndiciumResult CreateEnvelopeIndicium(CreateEnvelopeIndiciumParameters parameters)
        {
            var integratorTxID = parameters.IntegratorTxID;
            var rate = parameters.Rate;

            Guid stampsTxID;
            string url;
            PostageBalance postageBalance;
            string mac;
            string postageHash;
            string trackingNumber;

            string result = CreateEnvelopeIndicium(
                parameters.Item,
                ref integratorTxID,
                ref rate,
                parameters.From,
                parameters.To,
                parameters.CustomerID,
                parameters.Mode,
                parameters.ImageType,
                parameters.CostCodeId,
                parameters.HideFIM,
                parameters.RateToken,
                parameters.OrderId,
                "", // memo
                false, // BypassCleanseAddress
                0, // ImageId
                0, // ImageId2
                out trackingNumber,
                out stampsTxID,
                out url,
                out postageBalance,
                out mac,
                out postageHash);

            return new CreateIndiciumResult
            {
                TrackingNumber = trackingNumber,
                IntegratorTxID = integratorTxID,
                Rate = rate,
                Result = result,
                StampsTxID = stampsTxID,
                URL = url,
                PostageBalance = postageBalance,
                Mac = mac,
                PostageHash = postageHash
            };
        }

        /// <summary>
        /// Create an Indicium
        /// </summary>
        public CreateIndiciumResult CreateIndicium(CreateIndiciumParameters parameters)
        {
            var integratorTxID = parameters.IntegratorTxID;
            var trackingNumber = parameters.TrackingNumber;
            var rate = parameters.Rate;

            Guid stampsTxID;
            string url;
            PostageBalance postageBalance;
            string mac;
            string postageHash;
            byte[][] imageData;
            HoldForPickUpFacility holdForPickup;
            string formURL;

            string result = CreateIndicium(
                parameters.Item,
                ref integratorTxID,
                ref trackingNumber,
                ref rate,
                parameters.From,
                parameters.To,
                parameters.CustomerID,
                parameters.Customs,
                parameters.SampleOnly,
                parameters.PostageMode,
                parameters.ImageType,
                parameters.EltronPrinterDPIType,
                parameters.Memo,
                parameters.CostCodeId,
                parameters.DeliveryNotification,
                parameters.ShipmentNotification,
                parameters.RotationDegrees,
                parameters.HorizontalOffset,
                parameters.HorizontalOffsetSpecified,
                parameters.VerticalOffset,
                parameters.VerticalOffsetSpecified,
                parameters.PrintDensity,
                parameters.PrintDensitySpecified,
                parameters.PrintMemo,
                parameters.PrintMemoSpecified,
                parameters.PrintInstructions,
                parameters.PrintInstructionsSpecified,
                parameters.RequestPostageHash,
                parameters.NonDeliveryOption,
                parameters.RedirectTo,
                parameters.OriginalPostageHash,
                parameters.ReturnImageData,
                parameters.ReturnImageDataSpecified,
                parameters.InternalTransactionNumber,
                parameters.PaperSize,
                parameters.EmailLabelTo,
                parameters.PayOnPrint,
                parameters.ReturnLabelExpirationDays,
                parameters.ReturnLabelExpirationDaysSpecified,
                parameters.ImageDpi,
                parameters.RateToken,
                parameters.OrderId,
                false, // BypassCleanseAddress
                0, //image id,
                out trackingNumber,
                out stampsTxID,
                out url,
                out postageBalance,
                out mac,
                out postageHash,
                out imageData,
                out holdForPickup,
                out formURL);

            return new CreateIndiciumResult
            {
                TrackingNumber = trackingNumber,
                IntegratorTxID = integratorTxID,
                Rate = rate,
                Result = result,
                StampsTxID = stampsTxID,
                URL = url,
                PostageBalance = postageBalance,
                Mac = mac,
                PostageHash = postageHash,
                ImageData = imageData,
            };
        }
    }
}