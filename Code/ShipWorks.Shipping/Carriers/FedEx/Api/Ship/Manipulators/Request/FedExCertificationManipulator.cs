using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExCertificationManipulator : FedExShippingRequestManipulatorBase
    {
        private readonly IFedExShipmentTokenProcessor tokenProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCertificationManipulator" /> class to
        /// be configured with a token processor of FedExShipmentTokenProcessor and the 
        /// FedExSettingsRepository as the settings repository.
        /// </summary>
        public FedExCertificationManipulator()
            : this(new FedExShipmentTokenProcessor(), new FedExSettingsRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCertificationManipulator" /> class.
        /// </summary>
        /// <param name="tokenProcessor">The token processor.</param>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExCertificationManipulator(IFedExShipmentTokenProcessor tokenProcessor, ICarrierSettingsRepository settingsRepository) : base(settingsRepository)
        {
            // Used for processing tokens in the reference PO
            this.tokenProcessor = tokenProcessor;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization 
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            FedExShipmentEntity fedExShipment = request.ShipmentEntity.FedEx;

            if (SettingsRepository.IsInterapptiveUser && !string.IsNullOrEmpty(fedExShipment.ReferencePO))
            {
                // Process any tokens in the reference PO as the transaction ID
                nativeRequest.TransactionDetail.CustomerTransactionId = tokenProcessor.ProcessTokens(fedExShipment.ReferencePO, request.ShipmentEntity);
            }
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a IFedExNativeShipmentRequest
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
            
            if (nativeRequest.TransactionDetail == null)
            {
                // We'll be manipulating properties of the transaction detail, so make sure it's been created
                nativeRequest.TransactionDetail = new TransactionDetail();
            }
        }
    }
}
