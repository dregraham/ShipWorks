using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExRateTypeManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExRateTypeManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) => true;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(shipment, request);

            // Use the repository to see what type of rates we should be using
            if (settings.UseListRates)
            {
                // Don't send NONE as the rate type because even though that works for rates and the 
                // documentation suggests that NONE should return account rates, it actually returns
                // no rates while processing.  Not sending a value results in rates being returned.
                request.RequestedShipment.RateRequestTypes = new [] { RateRequestType.LIST };   
            }

            return request;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private void InitializeRequest(IShipmentEntity shipment, ProcessShipmentRequest request)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(request, nameof(request));

            request.Ensure(r => r.RequestedShipment);
        }
    }
}
