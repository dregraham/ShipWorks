using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator for handling customer references of a
    /// FedEx shipping request.
    /// </summary>
    public class FedExReferenceManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;
        private readonly IFedExShipmentTokenProcessor tokenProcessor;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExReferenceManipulator(IFedExSettingsRepository settings, IFedExShipmentTokenProcessor tokenProcessor)
        {
            this.settings = settings;
            this.tokenProcessor = tokenProcessor;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber)
        {
            return true;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(shipment, request);

            List<CustomerReference> customerReferences = request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();

            ShipmentEntity writableShipment = shipment as ShipmentEntity;

            // A little unique in that we're updating the shipment entity as well as manipulating the request that gets sent to FedEx
            writableShipment.FedEx.ReferenceCustomer = AddCustomerReference(shipment, customerReferences, shipment.FedEx.ReferenceCustomer, CustomerReferenceType.CUSTOMER_REFERENCE);
            writableShipment.FedEx.ReferenceInvoice = AddCustomerReference(shipment, customerReferences, shipment.FedEx.ReferenceInvoice, CustomerReferenceType.INVOICE_NUMBER);
            writableShipment.FedEx.ReferencePO = AddCustomerReference(shipment, customerReferences, shipment.FedEx.ReferencePO, CustomerReferenceType.P_O_NUMBER);
            writableShipment.FedEx.ReferenceShipmentIntegrity = AddCustomerReference(shipment, customerReferences, shipment.FedEx.ReferenceShipmentIntegrity, CustomerReferenceType.SHIPMENT_INTEGRITY);

            if (customerReferences.Count > 0)
            {
                // Each package should be in it's own request, so we always use the first item in the line item aray
                request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences = customerReferences.ToArray();
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

            if (request.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                request.RequestedShipment = new RequestedShipment();
            }

            if (request.RequestedShipment.RequestedPackageLineItems == null || request.RequestedShipment.RequestedPackageLineItems.Length == 0)
            {
                // We'll be inspecting/manipulating properties of the package line items, so make sure it's been created
                request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];
                request.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
            }

            if (request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences == null)
            {
                request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences = new CustomerReference[0];
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
        private string AddCustomerReference(IShipmentEntity shipment, List<CustomerReference> references, string referenceToken, CustomerReferenceType referenceType)
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
