using System;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.WebServices
{
	/// <summary>
	/// Interface around the Stamps WebService
	/// </summary>
	public interface ISwsimV135 : IDisposable
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
		string GetAccountInfo(object Item, out AccountInfoV65 AccountInfo, out Address Address, out string CustomerEmail, out string accountStatus, out DateAdvance dateAdvanceConfig, out string verificationPhoneNumber, out string verificationPhoneExtension);

		/// <summary>
		/// Get account info
		/// </summary>
		AccountInfoResult GetAccountInfo(Credentials credentials);

		/// <summary>
		/// Get the URL
		/// </summary>
		string GetURL(object Item, UrlType URLType, string ApplicationContext, out string URL, out string PPLSessionRequestID);

		/// <summary>
		/// Purchase postage
		/// </summary>
		string PurchasePostage(object Item, decimal PurchaseAmount, decimal ControlTotal, MachineInfo MI,
			string IntegratorTxID, bool? SendEmail, bool SendEmailSpecified,
			out PurchaseStatus PurchaseStatus, out int TransactionID, out PostageBalance PostageBalance,
			out string RejectionReason, out bool MIRequired, out string ProcessorTransactionID);

		/// <summary>
		/// Get rates
		/// </summary>
		RateV46[] GetRates(Credentials account, RateV46 rate, Carrier carrier);

		/// <summary>
		/// Cleanse the address
		/// </summary>
		void CleanseAddressAsync(object Item, Address Address, string FromZIPCode);

		/// <summary>
		/// Register an account
		/// </summary>
		RegistrationStatus RegisterAccount(
			System.Guid IntegrationID,
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
					Nullable<bool> SendEmail,
					bool SendEmailSpecified,
					System.Nullable<bool> ResetPasswordAfterRegistration,
					bool ResetPasswordAfterRegistrationSpecified,
					string UserCurrency,
					string IovationBlackBox,
					out string SuggestedUserName,
					out int UserId,
					out string PromoUrl);

		/// <summary>
		/// Create a scan form
		/// </summary>
		string CreateManifest(
			object Item,
			ref string IntegratorTxID,
			Guid[] StampsTxIDs,
			string[] TrackingNumbers,
			DateTime? ShipDate,
			bool ShipDateSpecified,
			string PrintLayout,
			Address FromAddress,
			ImageType ImageType,
			bool PrintInstructions,
			ManifestType ManifestType,
			int NumberOfLabels,
			out EndOfDayManifest[] EndOfDayManifests);

		/// <summary>
		/// Cancel an Indicium
		/// </summary>
		string CancelIndicium(object Item, object[] Item1, bool? SendEmail, bool SendEmailSpecified);

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
		string TrackShipment(object Item, object Item1, Carrier Carrier, out TrackingEvent[] TrackingEvents, out DateTime? GuaranteedDeliveryDate,
			out DateTime? ExpectedDeliveryDate, out string ServiceDescription, out string Carrier1, out DestinationInfo DestinationInfo);

		/// <summary>
		/// Add a carrier
		/// </summary>
		string AddCarrier(object Item, bool UserOwnedAccount, Carrier Carrier, string AccountNumber, string AccountZIPCode, string AccountCountry, Address Address, bool AgreeToEula, Invoice Invoice, bool NegotiatedRates, string DeviceIdentity, string ClientId, string ClientSecret, string PickupNumber, string DistributionCenter);

		/// <summary>
		/// Set automatic funding settings
		/// </summary>
		string SetAutoBuy(object Item, AutoBuySettings AutoBuySettings);
	}
}
