using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Response;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;
using CodeDescriptionType = ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration.CodeDescriptionType;

namespace ShipWorks.Shipping.Carriers.UPS.LinkNewAccount.Response
{
    public class UpsLinkNewAccountToProfileResponse : ICarrierResponse
    {
        private readonly ManageAccountResponse nativeResponse;
        public const string GoodShipperAccountStatus = "010";

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountResponse" /> class.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request.</param>
        public UpsLinkNewAccountToProfileResponse(
            ManageAccountResponse nativeResponse,
            CarrierRequest request)
        {
            this.nativeResponse = nativeResponse;
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
                throw new UpsApiException(UpsApiResponseStatus.Hard, responseStatus.Code, responseStatus.Description);
            }

            RegCodeDescriptionType accountStatus = nativeResponse.ShipperAccountStatus.FirstOrDefault(s => s.Code != GoodShipperAccountStatus);
            if (accountStatus != null)
            {
                throw new UpsApiException(UpsApiResponseStatus.Hard, accountStatus.Code, accountStatus.Description);
            }
        }
    }
}
