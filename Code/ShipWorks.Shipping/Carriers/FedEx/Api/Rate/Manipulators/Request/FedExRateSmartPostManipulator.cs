using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the attributes of a SmartPost 
    /// attributes of the shipment and each package within the FedEx API's RateRequest object if
    /// the shipment has a FedEx SmartPost HubID.
    /// </summary>
    public class FedExRateSmartPostManipulator : ICarrierRequestManipulator
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);
            
            if (FedExUtility.IsSmartPostEnabled(request.ShipmentEntity))
            {
                // We can safely cast this since we've passed initialization
                RateRequest nativeRequest = request.NativeRequest as RateRequest;
                FedExShipmentEntity fedExShipment = request.ShipmentEntity.FedEx;

                SmartPostShipmentDetail smartPostDetail = new SmartPostShipmentDetail();
                smartPostDetail.HubId = FedExUtility.GetSmartPostHub(fedExShipment.SmartPostHubID, request.CarrierAccountEntity as FedExAccountEntity);

                smartPostDetail.Indicia = GetSmartPostIndiciaType((FedExSmartPostIndicia) fedExShipment.SmartPostIndicia);
                smartPostDetail.IndiciaSpecified = true;

                SmartPostAncillaryEndorsementType? endorsement = GetSmartPostEndorsementType((FedExSmartPostEndorsement) fedExShipment.SmartPostEndorsement);
                if (endorsement != null)
                {
                    smartPostDetail.AncillaryEndorsement = endorsement.Value;
                    smartPostDetail.AncillaryEndorsementSpecified = true;
                }
                
                // Smart Post rates are only retreived by explicitly setting the service type
                nativeRequest.RequestedShipment.ServiceType = ServiceType.SMART_POST;
                nativeRequest.RequestedShipment.ServiceTypeSpecified = true;

                // Smart post won't return rates if insured value is greater than 0
                nativeRequest.RequestedShipment.TotalInsuredValue.Amount = 0;

                // Rate requests differ from ship requests in that all packages are sent in one request, so 
                // iterate over each package in the shipment to set the insured value to 0
                for (int i = 0; i < fedExShipment.Packages.Count; i++)
                {
                    // We can't guarantee that all the line items have been added, so we initialize the line item
                    // to make sure there is a valid object reference at the current index of the line item array
                    // before attempting to access the item in the array
                    InitializeLineItem(nativeRequest, i);
                    if (nativeRequest.RequestedShipment.RequestedPackageLineItems[i].InsuredValue == null)
                    {
                        nativeRequest.RequestedShipment.RequestedPackageLineItems[i].InsuredValue = new Money();
                    }

                    nativeRequest.RequestedShipment.RequestedPackageLineItems[i].InsuredValue.Amount = 0;
                }

                nativeRequest.RequestedShipment.SmartPostDetail = smartPostDetail;
            }
        }

        /// <summary>
        /// Initializes the line item.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        /// <param name="lineItemIndex">Index of the line item.</param>
        private static void InitializeLineItem(RateRequest nativeRequest, int lineItemIndex)
        {
            if (nativeRequest.RequestedShipment.RequestedPackageLineItems.Length <= lineItemIndex)
            {
                // We need to resize the line item array to accommodate the index
                RequestedPackageLineItem[] packageArray = nativeRequest.RequestedShipment.RequestedPackageLineItems;
                Array.Resize(ref packageArray, lineItemIndex + 1);

                nativeRequest.RequestedShipment.RequestedPackageLineItems = packageArray;
            }

            if (nativeRequest.RequestedShipment.RequestedPackageLineItems[lineItemIndex] == null)
            {
                // We need to create a new package line item
                nativeRequest.RequestedShipment.RequestedPackageLineItems[lineItemIndex] = new RequestedPackageLineItem();
            }
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private static void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a ProcessShipmentRequest
            RateRequest nativeRequest = request.NativeRequest as RateRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }

            if (nativeRequest.RequestedShipment.RequestedPackageLineItems == null)
            {
                nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[0];
            }

            if (nativeRequest.RequestedShipment.TotalInsuredValue == null)
            {
                nativeRequest.RequestedShipment.TotalInsuredValue = new Money();
            }
        }

        /// <summary>
        /// Get the FedEx API value that corresponds to our internal value
        /// </summary>
        private static SmartPostIndiciaType GetSmartPostIndiciaType(FedExSmartPostIndicia indicia)
        {
            switch (indicia)
            {
                case FedExSmartPostIndicia.ParcelSelect: return SmartPostIndiciaType.PARCEL_SELECT;
                case FedExSmartPostIndicia.MediaMail: return SmartPostIndiciaType.MEDIA_MAIL;
                case FedExSmartPostIndicia.BoundPrintedMatter: return SmartPostIndiciaType.PRESORTED_BOUND_PRINTED_MATTER;
                case FedExSmartPostIndicia.PresortedStandard: return SmartPostIndiciaType.PRESORTED_STANDARD;
                case FedExSmartPostIndicia.ParcelReturn: return SmartPostIndiciaType.PARCEL_RETURN;
            }

            throw new InvalidOperationException("Invalid indicia type: " + indicia);
        }

        /// <summary>
        /// Get the FedEx API value that corresponds to our internal value
        /// </summary>
        private static SmartPostAncillaryEndorsementType? GetSmartPostEndorsementType(FedExSmartPostEndorsement endorsement)
        {
            switch (endorsement)
            {
                case FedExSmartPostEndorsement.AddressCorrection: return SmartPostAncillaryEndorsementType.ADDRESS_CORRECTION;
                case FedExSmartPostEndorsement.ChangeService: return SmartPostAncillaryEndorsementType.CHANGE_SERVICE;
                case FedExSmartPostEndorsement.ForwardingService: return SmartPostAncillaryEndorsementType.FORWARDING_SERVICE;
                case FedExSmartPostEndorsement.LeaveIfNoResponse: return SmartPostAncillaryEndorsementType.CARRIER_LEAVE_IF_NO_RESPONSE;
                case FedExSmartPostEndorsement.ReturnService: return SmartPostAncillaryEndorsementType.RETURN_SERVICE;
                case FedExSmartPostEndorsement.None: return null;
            }

            throw new InvalidOperationException("Invalid endorsement value: " + endorsement);
        }
    }
}
