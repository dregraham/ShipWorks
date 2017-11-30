using System.Linq;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator for handling customer references of a
    /// FedEx shipping request.
    /// </summary>
    public class FedExReferenceManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExShipmentTokenProcessor tokenProcessor;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExReferenceManipulator(IFedExShipmentTokenProcessor tokenProcessor)
        {
            this.tokenProcessor = tokenProcessor;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) => true;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            var lineItem = request.Ensure(x => x.RequestedShipment).EnsureAtLeastOne(x => x.RequestedPackageLineItems);
            lineItem.Ensure(x => x.CustomerReferences);

            return new[]
                {
                    AddCustomerReference(shipment, shipment.FedEx.ReferenceCustomer, CustomerReferenceType.CUSTOMER_REFERENCE),
                    AddCustomerReference(shipment, shipment.FedEx.ReferenceInvoice, CustomerReferenceType.INVOICE_NUMBER),
                    AddCustomerReference(shipment, shipment.FedEx.ReferencePO, CustomerReferenceType.P_O_NUMBER),
                    AddCustomerReference(shipment, shipment.FedEx.ReferenceShipmentIntegrity, CustomerReferenceType.SHIPMENT_INTEGRITY),
                }
                .Flatten()
                .Map(list => list.Where(x => !string.IsNullOrEmpty(x?.Value)).Concat(lineItem.CustomerReferences))
                .Do(list => lineItem.CustomerReferences = list.ToArray())
                .Map(_ => request);
        }

        /// <summary>
        /// Uses the token processor to create a string value based on the token and shipment and adds it 
        /// to the reference list if the reference value is non-blank; the string value of the reference 
        /// being added is returned.
        /// </summary>
        private GenericResult<CustomerReference> AddCustomerReference(IShipmentEntity shipment, string referenceToken, CustomerReferenceType referenceType)
        {
            string referenceValue = tokenProcessor.ProcessTokens(referenceToken, shipment);

            if (string.IsNullOrEmpty(referenceValue))
            {
                return GenericResult.FromSuccess<CustomerReference>(null);
            }

            if (referenceValue.Length > 30)
            {
                // FedEx sends back a confusing error message when this occurs, so be proactive and show the user a friendlier error message
                return new FedExException(string.Format("FedEx does not allow references to exceed 30 characters in length. The reference value of \"{0}\" will exceed the 30 character limit. Please shorten the value and try again.", referenceToken));
            }

            return new CustomerReference { CustomerReferenceType = referenceType, Value = referenceValue };
        }
    }
}
