using System;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateWebAuthenticationManipulator: IFedExRateRequestManipulator
    {
        private readonly FedExSettings fedExSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExRateWebAuthenticationManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExRateWebAuthenticationManipulator(FedExSettings fedExSettings)
        {
            this.fedExSettings = fedExSettings;
        }

        /// <summary>
        /// Does this manipulator apply to the shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options)
        {
            return true;
        }

        /// <summary>
        /// Manipulates the specified request by setting the WebAuthenticationDetail property of a RateRequest object.
        /// </summary>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            request.WebAuthenticationDetail = FedExRequestManipulatorUtilities.CreateRateWebAuthenticationDetail(fedExSettings);

            return request;
        }

        /// <summary>
        /// Validates the request making sure it is not null and of the correct type.
        /// </summary>
        private void ValidateRequest(RateRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
        }
    }
}
