using System;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.WebServices
{
    /// <summary>
    /// Mark the web service as implementing the interface
    /// </summary>
    public partial class SwsimV55 : ISwsimV55
    {
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