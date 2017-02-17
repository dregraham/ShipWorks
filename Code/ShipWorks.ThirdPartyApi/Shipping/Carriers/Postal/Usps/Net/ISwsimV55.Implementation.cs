﻿using System;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.WebServices
{
    /// <summary>
    /// Mark the web service as implementing the interface
    /// </summary>
    public partial class SwsimV55 : ISwsimV55
    {
        /// <summary>
        /// Get account info
        /// </summary>
        public AccountInfoResult GetAccountInfo(Credentials credentials)
        {
            AccountInfo accountInfo;
            Address address;
            string email;

            GetAccountInfo(credentials, out accountInfo, out address, out email);

            return new AccountInfoResult(accountInfo, address, email);
        }

        /// <summary>
        /// Get rates
        /// </summary>
        public RateV20[] GetRates(Credentials account, RateV20 rate)
        {
            RateV20[] rateResults;

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
                out stampsTxID,
                out url,
                out postageBalance,
                out mac,
                out postageHash,
                out imageData);

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