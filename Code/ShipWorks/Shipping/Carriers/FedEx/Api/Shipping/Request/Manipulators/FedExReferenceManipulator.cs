using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator for handling customer references of a
    /// FedEx shipping request.
    /// </summary>
    public class FedExReferenceManipulator : FedExShippingRequestManipulatorBase
    {
        private readonly IFedExShipmentTokenProcessor tokenProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExReferenceManipulator" /> class and 
        /// uses the FedExShipmentTokenProcessor and the FedExSettingsRepository.
        /// </summary>
        public FedExReferenceManipulator()
            : this(new FedExShipmentTokenProcessor(), new FedExSettingsRepository())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRecipientManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExReferenceManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExReferenceManipulator" /> class.
        /// </summary>
        /// <param name="tokenProcessor">The token processor.</param>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExReferenceManipulator(IFedExShipmentTokenProcessor tokenProcessor, ICarrierSettingsRepository settingsRepository) : base(settingsRepository)
        {
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

            List<CustomerReference> customerReferences = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();

            // A little unique in that we're updating the shipment entity as well as manipulating the request that gets sent to FedEx
            request.ShipmentEntity.FedEx.ReferenceCustomer = AddCustomerReference(request.ShipmentEntity, customerReferences, request.ShipmentEntity.FedEx.ReferenceCustomer, CustomerReferenceType.CUSTOMER_REFERENCE);
            request.ShipmentEntity.FedEx.ReferenceInvoice = AddCustomerReference(request.ShipmentEntity, customerReferences, request.ShipmentEntity.FedEx.ReferenceInvoice, CustomerReferenceType.INVOICE_NUMBER);
            request.ShipmentEntity.FedEx.ReferencePO = AddCustomerReference(request.ShipmentEntity, customerReferences, request.ShipmentEntity.FedEx.ReferencePO, CustomerReferenceType.P_O_NUMBER);
            request.ShipmentEntity.FedEx.ReferenceShipmentIntegrity = AddCustomerReference(request.ShipmentEntity, customerReferences, request.ShipmentEntity.FedEx.ReferenceShipmentIntegrity, CustomerReferenceType.SHIPMENT_INTEGRITY);

            if (customerReferences.Count > 0)
            {
                // Each package should be in it's own request, so we always use the first item in the line item aray
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences = customerReferences.ToArray();
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

            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }

            if (nativeRequest.RequestedShipment.RequestedPackageLineItems == null || nativeRequest.RequestedShipment.RequestedPackageLineItems.Length == 0)
            {
                // We'll be inspecting/manipulating properties of the package line items, so make sure it's been created
                nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
            }

            if (nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences == null)
            {
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences = new CustomerReference[0];
            }
        }

        /// <summary>
        /// Uses the token processor to create a string value based on the token and shipment and adds it 
        /// to the reference list if the reference value is non-blank; the string value of the reference 
        /// being added is returned.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="references">The references.</param>
        /// <param name="referenceToken">The reference token.</param>
        /// <param name="referenceType">Type of the reference.</param>
        /// <returns>The reference number/value.</returns>
        private string AddCustomerReference(ShipmentEntity shipment, List<CustomerReference> references, string referenceToken, CustomerReferenceType referenceType)
        {
            string referenceValue = tokenProcessor.ProcessTokens(referenceToken, shipment);

            if (!string.IsNullOrEmpty(referenceValue))
            {
                if (referenceValue.Length > 30)
                {
                    // FedEx sends back a confusing error message when this occurs, so be proactive and show the user a friendlier error message
                    throw new FedExException(string.Format("FedEx does not allow references to exceed 30 characters in length. The reference value of \"{0}\" will exceed the 30 character limit. Please shorten the value and try again.", referenceToken));
                }

                references.Add(new CustomerReference { CustomerReferenceType = referenceType, Value = referenceValue });
            }

            return referenceValue;
        }
    }
}
