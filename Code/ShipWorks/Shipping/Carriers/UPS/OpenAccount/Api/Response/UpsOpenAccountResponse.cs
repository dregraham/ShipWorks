using System.Collections.Generic;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Response
{
    /// <summary>
    /// This object is used to process the OpenAccountResponse, saving any UPS account info
    /// to the UPS account object. It is populated with the actual WSDL response object.
    /// </summary>
    public class UpsOpenAccountResponse : ICarrierResponse
    {
        private readonly OpenAccountResponse nativeResponse;
        private readonly CarrierRequest request;
        private readonly List<ICarrierResponseManipulator> responseManipulators;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountResponse" /> class.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request.</param>
        /// <param name="responseManipulators">The response manipulators.</param>
        public UpsOpenAccountResponse(OpenAccountResponse nativeResponse, CarrierRequest request, IEnumerable<ICarrierResponseManipulator> responseManipulators)
        {
            this.nativeResponse = nativeResponse;
            this.request = request;
            this.responseManipulators = new List<ICarrierResponseManipulator>(responseManipulators);
        }

        /// <summary>
        /// Gets the request the was used to generate the response.
        /// </summary>
        /// <value>The CarrierRequest object.</value>
        public CarrierRequest Request
        {
            get { return request; }
        }

        /// <summary>
        /// Gets the native response received from the carrier API - virtual for unit tests
        /// </summary>
        /// <value>The native response.</value>
        public virtual object NativeResponse
        {
            get { return nativeResponse; }
        }

        /// <summary>
        /// Performs any processing required based on the response from the carrier.
        /// </summary>
        /// <exception cref="UpsOpenAccountResponseException"></exception>
        public void Process()
        {
            if (nativeResponse.Response.ResponseStatus.Code == EnumHelper.GetApiValue(UpsOpenAccountResponseStatusCode.Failed))
            {
                throw new UpsOpenAccountResponseException(nativeResponse);                
            }

            foreach (ICarrierResponseManipulator manipulator in responseManipulators)
            {
                manipulator.Manipulate(this);
            }
        }
    }
}
