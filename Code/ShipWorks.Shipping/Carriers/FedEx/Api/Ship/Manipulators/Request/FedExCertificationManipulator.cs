using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// 
    /// </summary>
    public class FedExCertificationManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExShipmentTokenProcessor tokenProcessor;
        private readonly IFedExSettingsRepository settingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCertificationManipulator" /> class.
        /// </summary>
        public FedExCertificationManipulator(IFedExShipmentTokenProcessor tokenProcessor, IFedExSettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
            this.tokenProcessor = tokenProcessor;
        }

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment) =>
            settingsRepository.IsInterapptiveUser && !string.IsNullOrEmpty(shipment.FedEx.ReferencePO);

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            InitializeRequest(request);

            request.TransactionDetail.CustomerTransactionId = tokenProcessor.ProcessTokens(shipment.FedEx.ReferencePO, shipment);

            return request;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private void InitializeRequest(ProcessShipmentRequest request) =>
            request.Ensure(x => x.TransactionDetail);
    }
}
