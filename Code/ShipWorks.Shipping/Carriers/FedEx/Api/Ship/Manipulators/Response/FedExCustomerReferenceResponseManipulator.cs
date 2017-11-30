using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Response
{
    /// <summary>
    /// Set reference details
    /// </summary>
    public class FedExCustomerReferenceResponseManipulator : IFedExShipResponseManipulator
    {
        /// <summary>
        /// Manipulate the shipment
        /// </summary>
        public GenericResult<ShipmentEntity> Manipulate(ProcessShipmentReply response, ProcessShipmentRequest request, ShipmentEntity shipment)
        {
            shipment.FedEx.ReferenceCustomer = FindCustomerReferenceFor(CustomerReferenceType.CUSTOMER_REFERENCE, request);
            shipment.FedEx.ReferenceInvoice = FindCustomerReferenceFor(CustomerReferenceType.INVOICE_NUMBER, request);
            shipment.FedEx.ReferencePO = FindCustomerReferenceFor(CustomerReferenceType.P_O_NUMBER, request);
            shipment.FedEx.ReferenceShipmentIntegrity = FindCustomerReferenceFor(CustomerReferenceType.SHIPMENT_INTEGRITY, request);

            return shipment;
        }

        /// <summary>
        /// Find the customer reference for the given type
        /// </summary>
        private static string FindCustomerReferenceFor(CustomerReferenceType referenceType, ProcessShipmentRequest request) =>
            request.RequestedShipment?
                .RequestedPackageLineItems?
                .FirstOrDefault()?
                .CustomerReferences
                .Where(x => x.CustomerReferenceType == referenceType)
                .Select(x => x.Value)
                .FirstOrDefault();
    }
}
