using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// FedEx Returns Manipulator
    /// </summary>
    public class FedExReturnsManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExReturnsManipulator" /> class.
        /// </summary>
        public FedExReturnsManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExReturnsManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExReturnsManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Manipulates the specified request adding return information
        /// </summary>
        public override void Manipulate(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!request.ShipmentEntity.ReturnShipment || !request.ShipmentEntity.FedEx.ReturnsClearance)
            {
                return;
            }

            IFedExNativeShipmentRequest nativeRequest = InitializeShipmentRequest(request);

            FedExReturnType returnType = (FedExReturnType) request.ShipmentEntity.FedEx.ReturnType;
            bool returnsClearance = request.ShipmentEntity.FedEx.ReturnsClearance;

            // Get current list of ShipmentSpecialServiceTypes. This will be added back later in the function as there might be more
            // ServiceTypes to conditionally add.
            List<ShipmentSpecialServiceType> shipmentSpecialServiceTypes = nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();
            shipmentSpecialServiceTypes.Add(returnsClearance ? ShipmentSpecialServiceType.RETURNS_CLEARANCE : ShipmentSpecialServiceType.RETURN_SHIPMENT);

            // Add Return Detail
            nativeRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail = new ReturnShipmentDetail();

            // Print or Email
            if (returnType == FedExReturnType.EmailReturnLabel)
            {
                nativeRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnType = ReturnType.PENDING;
                shipmentSpecialServiceTypes.Add(ShipmentSpecialServiceType.PENDING_SHIPMENT);
            }
            else
            {
                nativeRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnType = ReturnType.PRINT_RETURN_LABEL;
            }

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = shipmentSpecialServiceTypes.ToArray();

            // RMA Reason
            if (!string.IsNullOrWhiteSpace(request.ShipmentEntity.FedEx.RmaReason))
            {
                nativeRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.Rma = new Rma
                {
                    Reason = request.ShipmentEntity.FedEx.RmaReason
                };
            }

            // RMA number
            if (!string.IsNullOrWhiteSpace(request.ShipmentEntity.FedEx.RmaNumber))
            {
                List<CustomerReference> customerReferences = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();

                customerReferences.Add(new CustomerReference
                {
                    Value = request.ShipmentEntity.FedEx.RmaNumber,
                    CustomerReferenceType = CustomerReferenceType.RMA_ASSOCIATION
                });

                nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences = customerReferences.ToArray();
            }

            //Email Info
            if (returnType == FedExReturnType.EmailReturnLabel)
            {
                nativeRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail = new ReturnEMailDetail
                {
                    MerchantPhoneNumber = request.ShipmentEntity.OriginPhone
                };
                if (request.ShipmentEntity.FedEx.ReturnSaturdayPickup)
                {
                    nativeRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail.AllowedSpecialServices = new[]
                    {
                        ReturnEMailAllowedSpecialServiceType.SATURDAY_PICKUP
                    };
                }
                EMailRecipient eMailRecipient = new EMailRecipient
                {
                    EmailAddress = request.ShipmentEntity.ShipEmail
                };

                PendingShipmentDetail pendingShipmentDetail = new PendingShipmentDetail
                {
                    Type = PendingShipmentType.EMAIL, 
                    ExpirationDate = DateTime.Today.AddDays(30), 
                    ExpirationDateSpecified = true
                };

                EMailLabelDetail emailLabelDetail = new EMailLabelDetail
                {
                    Recipients = new EMailRecipient[1] {eMailRecipient}
                };

                //NotificationEMailAddress = request.ShipmentEntity.ShipEmail

                pendingShipmentDetail.EmailLabelDetail = emailLabelDetail;
                nativeRequest.RequestedShipment.SpecialServicesRequested.PendingShipmentDetail = pendingShipmentDetail;
            }
        }

        /// <summary>
        /// Initializes the shipment request.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private IFedExNativeShipmentRequest InitializeShipmentRequest(CarrierRequest request)
        {
            // The native FedEx request type should be a IFedExNativeShipmentRequest
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            // Make sure the RequestedShipment is there
            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }

            // Add ShipmentSpecialServicesRequested
            if (nativeRequest.RequestedShipment.SpecialServicesRequested == null)
            {
                nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            // Add ShipemtnSpecialServiceType
            if (nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes == null)
            {
                nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            }

            // Make sure package is there           
            if (nativeRequest.RequestedShipment.RequestedPackageLineItems == null)
            {
                nativeRequest.RequestedShipment.RequestedPackageLineItems = new[]
                {
                    new RequestedPackageLineItem()
                };
            }

            // Make sure references are there
            if (nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences == null)
            {
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences = new CustomerReference[0];
            }

            return nativeRequest;
        }
    }
}
