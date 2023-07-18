using System;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.WebServices
{
	/// <summary>
	/// Mark the web service as implementing the interface
	/// </summary>
	public partial class SwsimV135 : ISwsimV135
	{
		/// <summary>
		/// Get account info
		/// </summary>
		public AccountInfoResult GetAccountInfo(Credentials credentials)
		{
			AccountInfoV65 accountInfo;
			Address address;

			GetAccountInfo(credentials, out accountInfo, out address, out string customerEmail, out string accountStatus, out DateAdvance dateAdvanceConfig, out string verificationPhoneNumber, out string verificationPhoneExtension);

			return new AccountInfoResult(accountInfo, address, customerEmail);
		}

		/// <summary>
		/// Get rates
		/// </summary>
		public RateV46[] GetRates(Credentials account, RateV46 rate, Carrier carrier)
		{
			RateV46[] rateResults;

			GetRates(account, rate, carrier, out rateResults);

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

			string reference1 = string.Empty;
			string reference2 = string.Empty;
			string reference3 = string.Empty;
			string reference4 = string.Empty;

			string result = CreateEnvelopeIndicium(
				parameters.Item,
				ref integratorTxID,
				ref rate,
				parameters.From,
				null,
				false,
				null,
				false,
				parameters.CustomerID,
				parameters.Mode,
				parameters.ImageType,
				parameters.CostCodeId,
				parameters.HideFIM,
				parameters.RateToken,
				parameters.OrderId,
				"", // memo
				false, // BypassCleanseAddress,
				0, // ImageId
				0, // ImageId2
				ref reference1,
				ref reference2,
				ref reference3,
				ref reference4,
				false, // ReturnIndiciumData
				null, // ExtendedPostageInfoV1
				false, //ReturnImageData,
				ImageDpi.ImageDpiDefault, //ImageDpi
				out trackingNumber,
				out stampsTxID,
				out url,
				out postageBalance,
				out mac,
				out postageHash,
				out byte[] IndicumData,
				out byte[][] ImageData);

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
			string outTrackingNumber = null;

			Guid stampsTxID;
			string url;
			PostageBalance postageBalance;
			string mac;
			string postageHash;
			byte[][] imageData;
			HoldForPickUpFacility holdForPickup;
			string formURL;

			string reference1 = string.Empty;
			string reference2 = string.Empty;
			string reference3 = string.Empty;
			string reference4 = string.Empty;

			string result = CreateIndicium(
				parameters.Item,
				ref integratorTxID,
				ref trackingNumber,
				ref rate,
				parameters.From,
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
				parameters.OutboundTransactionID,
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
				0, // image id,
				ref reference1, // Reference1
				ref reference2, // Reference2
				ref reference3, // Reference3
				ref reference4, // Reference4
				false, // ReturnIndiciumData
				null, // ExtendedPostageInfo
				EnclosedServiceType.Unknown,
				EnclosedPackageType.Unknown,
				null, // OrderDetails
				null, // BrandingId
				false, // BrandingIdSpecified
				null, // NotificationSettingId
				false, // NotificationSettingIdSpecified
				null, // GroupCode
				null, // Description
				out string encodedTrackingNumber,
				out string bannerText,
				out string trailingSuperScript,
				out outTrackingNumber,
				out stampsTxID,
				out url,
				out postageBalance,
				out mac,
				out postageHash,
				out imageData,
				out holdForPickup,
				out formURL,
				out string labelCategory,
				out byte[] indiciumData,
				out string labelId);

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