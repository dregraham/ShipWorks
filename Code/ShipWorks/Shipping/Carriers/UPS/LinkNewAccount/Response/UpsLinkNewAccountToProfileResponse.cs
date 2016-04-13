using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using System;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.UPS.LinkNewAccount.Response
{
    public class UpsLinkNewAccountToProfileResponse : ICarrierResponse
    {
        private readonly LinkNewAccountResponse nativeResponse;
        private readonly ILog log;
        public const string GoodShipperAccountStatus = "010";
        
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsLinkNewAccountToProfileResponse(LinkNewAccountResponse nativeResponse, CarrierRequest request) :
            this(nativeResponse, request, LogManager.GetLogger(typeof(UpsLinkNewAccountToProfileResponse)))
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsLinkNewAccountToProfileResponse(LinkNewAccountResponse nativeResponse, CarrierRequest request, ILog log)
        {
            this.nativeResponse = nativeResponse;
            this.log = log;
            Request = request;
        }

        /// <summary>
        /// Gets the request the was used to generate the response.
        /// </summary>
        public CarrierRequest Request { get; }

        /// <summary>
        /// Gets the native response received from the carrier API.
        /// </summary>
        public object NativeResponse => nativeResponse;

        /// <summary>
        /// Throws exception if account is not created.
        /// </summary>
        public void Process()
        {
            CodeDescriptionType responseStatus = nativeResponse.Response.ResponseStatus;
            if (responseStatus.Code != EnumHelper.GetApiValue(UpsInvoiceRegistrationResponseStatusCode.Success))
            {
                log.Error($"In linking new account to profile, UPS returned a status code of {responseStatus.Code}." +
                          $"{Environment.NewLine}Status Description = {responseStatus.Description}");
                
                throw new UpsApiException(UpsApiResponseStatus.Hard, responseStatus.Code, responseStatus.Description);
            }

            RegCodeDescriptionType accountStatus = nativeResponse.ShipperAccountStatus.FirstOrDefault(s => s.Code != GoodShipperAccountStatus);
            if (accountStatus != null)
            {
                log.Error($"In linking new account to profile, UPS returned an account status code of {accountStatus.Code}." +
                          $"{Environment.NewLine}Status Description = {accountStatus.Description}");

                throw new UpsApiException(UpsApiResponseStatus.Hard, accountStatus.Code, accountStatus.Description);
            }
        }
    }
}
