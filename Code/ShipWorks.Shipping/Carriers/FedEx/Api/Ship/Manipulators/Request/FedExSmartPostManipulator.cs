using System;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// Manipulator for adding SmartPost information to the FedEx request
    /// </summary>
    public class FedExSmartPostManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExSmartPostManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment)
        {
            return (FedExServiceType) shipment.FedEx.Service == FedExServiceType.SmartPost;
        }

        /// <summary>
        /// Add the SmartPost to the FedEx carrier request
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            MethodConditions.EnsureArgumentIsNotNull(request, nameof(request));
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            // If we shouldn't apply, return
            if (!ShouldApply(shipment))
            {
                return request;
            }

            // Get the FedEx shipment and account
            IFedExShipmentEntity fedExShipment = shipment.FedEx;
            IFedExAccountEntity account = settings.GetAccountReadOnly(shipment);

            // Get the RequestedShipment object for the request
            RequestedShipment requestedShipment = InitializeRequest(request);

            SmartPostShipmentDetail smartPostShipmentDetail = new SmartPostShipmentDetail();

            string smartPostHubID = FedExUtility.GetSmartPostHub(fedExShipment.SmartPostHubID, account);
            if (string.IsNullOrWhiteSpace(smartPostHubID))
            {
                throw new CarrierException("A SmartPost Hub ID is required for shipping with SmartPost.");
            }
            smartPostShipmentDetail.HubId = fedExShipment.SmartPostHubID;

            smartPostShipmentDetail.Indicia = GetSmartPostIndiciaType((FedExSmartPostIndicia) fedExShipment.SmartPostIndicia);
            smartPostShipmentDetail.IndiciaSpecified = true;

            var endorsement = GetSmartPostEndorsementType((FedExSmartPostEndorsement)fedExShipment.SmartPostEndorsement);
            if (endorsement != null)
            {
                smartPostShipmentDetail.AncillaryEndorsement = endorsement.Value;
                smartPostShipmentDetail.AncillaryEndorsementSpecified = true;
            }
            else
            {
                smartPostShipmentDetail.AncillaryEndorsementSpecified = false; 
            }

            if (!string.IsNullOrWhiteSpace(fedExShipment.SmartPostCustomerManifest))
            {
                string smartPostCustomerManifest = TemplateTokenProcessor.ProcessTokens(fedExShipment.SmartPostCustomerManifest,
                                                         fedExShipment.ShipmentID);

                if (!string.IsNullOrWhiteSpace(smartPostCustomerManifest))
                {
                    smartPostShipmentDetail.CustomerManifestId = smartPostCustomerManifest;
                }
            }

            // For smartpost this is always zero
            requestedShipment.TotalInsuredValue = null; 

            requestedShipment.SmartPostDetail = smartPostShipmentDetail;

            return request;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private static RequestedShipment InitializeRequest(ProcessShipmentRequest request)
        {
            request.Ensure(r => r.RequestedShipment);

            RequestedShipment requestedShipment = request.RequestedShipment;

            // Make sure a package is there
            if (requestedShipment.RequestedPackageLineItems == null)
            {
                requestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];
                requestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
            }

            // Make sure a package is there
            if (requestedShipment.RequestedPackageLineItems[0] == null)
            {
                requestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
            }

            return requestedShipment;
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

            throw new CarrierException("Invalid indicia type: " + indicia);
        }

        /// <summary>
        /// Get the FedEx API value that corresponds to our internal vlaue
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

            throw new CarrierException("Invalid endorsement value: " + endorsement);
        }
    }
}
