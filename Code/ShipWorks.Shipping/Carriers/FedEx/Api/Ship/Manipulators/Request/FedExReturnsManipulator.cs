using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// FedEx Returns Manipulator
    /// </summary>
    public class FedExReturnsManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExReturnsManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment)
        {
            return shipment.ReturnShipment;
        }

        /// <summary>
        /// Manipulates the specified request adding return information
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            MethodConditions.EnsureArgumentIsNotNull(request, nameof(request));
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            if (!ShouldApply(shipment))
            {
                return request;
            }

            InitializeShipmentRequest(request);

            FedExReturnType returnType = (FedExReturnType) shipment.FedEx.ReturnType;
            bool returnsClearance = shipment.FedEx.ReturnsClearance;

            // Get current list of ShipmentSpecialServiceTypes. This will be added back later in the function as there might be more
            // ServiceTypes to conditionally add.
            List<ShipmentSpecialServiceType> shipmentSpecialServiceTypes = request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();
            shipmentSpecialServiceTypes.Add(returnsClearance ? ShipmentSpecialServiceType.RETURNS_CLEARANCE : ShipmentSpecialServiceType.RETURN_SHIPMENT);

            // Add Return Detail
            request.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail = new ReturnShipmentDetail();

            // Print or Email
            if (returnType == FedExReturnType.EmailReturnLabel)
            {
                request.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnType = ReturnType.PENDING;
                shipmentSpecialServiceTypes.Add(ShipmentSpecialServiceType.PENDING_SHIPMENT);
            }
            else
            {
                request.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnType = ReturnType.PRINT_RETURN_LABEL;
            }

            request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = shipmentSpecialServiceTypes.ToArray();

            // RMA Reason
            if (!string.IsNullOrWhiteSpace(shipment.FedEx.RmaReason))
            {
                request.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.Rma = new Rma
                {
                    Reason = shipment.FedEx.RmaReason
                };
            }

            // RMA number
            if (!string.IsNullOrWhiteSpace(shipment.FedEx.RmaNumber))
            {
                List<CustomerReference> customerReferences = request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();

                customerReferences.Add(new CustomerReference
                {
                    Value = shipment.FedEx.RmaNumber,
                    CustomerReferenceType = CustomerReferenceType.RMA_ASSOCIATION
                });

                request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences = customerReferences.ToArray();
            }

            //Email Info
            if (returnType == FedExReturnType.EmailReturnLabel)
            {
                request.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail = new ReturnEMailDetail
                {
                    MerchantPhoneNumber = shipment.OriginPhone
                };
                if (shipment.FedEx.ReturnSaturdayPickup)
                {
                    request.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail.AllowedSpecialServices = new[]
                    {
                        ReturnEMailAllowedSpecialServiceType.SATURDAY_PICKUP
                    };
                }
                EMailRecipient eMailRecipient = new EMailRecipient
                {
                    EmailAddress = shipment.ShipEmail
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

                //NotificationEMailAddress = shipment.ShipEmail

                pendingShipmentDetail.EmailLabelDetail = emailLabelDetail;
                request.RequestedShipment.SpecialServicesRequested.PendingShipmentDetail = pendingShipmentDetail;
            }

            return request;
        }

        /// <summary>
        /// Initializes the shipment request.
        /// </summary>
        private void InitializeShipmentRequest(ProcessShipmentRequest request)
        {
            request.Ensure(r => r.RequestedShipment)
                .Ensure(rs => rs.SpecialServicesRequested)
                .Ensure(ssr => ssr.SpecialServiceTypes);

            // Make sure package is there           
            if (request.RequestedShipment.RequestedPackageLineItems == null)
            {
                request.RequestedShipment.RequestedPackageLineItems = new[]
                {
                    new RequestedPackageLineItem()
                };
            }

            // Make sure references are there
            if (request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences == null)
            {
                request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences = new CustomerReference[0];
            }
        }
    }
}
