using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using System;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// FedEx rate version capture manipulator
    /// </summary>
    public class FedExRateVersionManipulator : IFedExRateRequestManipulator
    {
        /// <summary>
        /// Does this manipulator apply to the shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options)
        {
            return true;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            // We can safely cast this since we've passed validation
            request.Version = new VersionId
            {
                ServiceId = "crs",
                Major = 22,
                Intermediate = 0,
                Minor = 0
            };

            return request;
        }

        /// <summary>
        /// Validates the request making sure it is not null and of the correct type.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void ValidateRequest(RateRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
        }
    }
}
