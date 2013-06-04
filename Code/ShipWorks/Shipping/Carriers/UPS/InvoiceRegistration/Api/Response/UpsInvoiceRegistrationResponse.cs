using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;

namespace ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Response
{
    public class UpsInvoiceRegistrationResponse : ICarrierResponse
    {
        private readonly RegisterResponse nativeResponse;

        private readonly CarrierRequest request;

        private readonly List<ICarrierResponseManipulator> responseManipulators;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsInvoiceRegistrationResponse"/> class.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request.</param>
        /// <param name="responseManipulators">The response manipulators.</param>
        public UpsInvoiceRegistrationResponse(RegisterResponse nativeResponse, CarrierRequest request, List<ICarrierResponseManipulator> responseManipulators)
        {
            this.nativeResponse = nativeResponse;
            this.request = request;
            this.responseManipulators = responseManipulators;
        }

        /// <summary>
        /// Gets the request the was used to generate the response.
        /// </summary>
        /// <value>
        /// The CarrierRequest object.
        /// </value>
        public CarrierRequest Request
        {
            get
            {
                return request;
            }
        }

        /// <summary>
        /// Gets the native response received from the carrier API.
        /// </summary>
        /// <value>
        /// The native response.
        /// </value>
        public object NativeResponse
        {
            get
            {
                return nativeResponse;
            }
        }

        /// <summary>
        /// Performs any processing required based on the response from the carrier.
        /// </summary>
        /// <exception cref="UpsApiException"></exception>
        public void Process()
        {
            CodeDescriptionType responseStatus = nativeResponse.Response.ResponseStatus;
            if (responseStatus.Code != EnumHelper.GetApiValue(UpsInvoiceRegistrationResponseStatusCode.Success))
            {
                throw new UpsApiException(UpsApiResponseStatus.Hard, responseStatus.Code, responseStatus.Description);
            }

            foreach (ICarrierResponseManipulator manipulator in responseManipulators)
            {
                manipulator.Manipulate(this);
            }
        }
    }
}