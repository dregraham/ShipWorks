using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Manipulator for adding SmartPost information to the FedEx request
    /// </summary>
    public class FedExSmartPostManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExSmartPostManipulator" /> class.
        /// </summary>
        public FedExSmartPostManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExSmartPostManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExSmartPostManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Add the SmartPost to the FedEx carrier request
        /// </summary>
        /// <param name="request">The FedEx carrier request</param>
        public override void Manipulate(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // Get the FedEx shipment and account
            FedExShipmentEntity fedExShipment = request.ShipmentEntity.FedEx;
            FedExAccountEntity account = request.CarrierAccountEntity as FedExAccountEntity;

            // If we aren't SmartPost, return
            if ((FedExServiceType)fedExShipment.Service != FedExServiceType.SmartPost)
            {
                return;
            }

            // Get the RequestedShipment object for the request
            RequestedShipment requestedShipment = InitializeRequest(request);

            SmartPostShipmentDetail smartPostShipmentDetail = new SmartPostShipmentDetail();

            fedExShipment.SmartPostHubID = FedExUtility.GetSmartPostHub(fedExShipment.SmartPostHubID, account);
            if (string.IsNullOrWhiteSpace(fedExShipment.SmartPostHubID))
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

            if (smartPostShipmentDetail.Indicia == SmartPostIndiciaType.PARCEL_SELECT)
            {
                fedExShipment.SmartPostConfirmation = true;
            }

            if (!string.IsNullOrWhiteSpace(fedExShipment.SmartPostCustomerManifest))
            {
                string smartPostCustomerManifest = TemplateTokenProcessor.ProcessTokens(fedExShipment.SmartPostCustomerManifest,
                                                         fedExShipment.ShipmentID);

                if (!string.IsNullOrWhiteSpace(smartPostCustomerManifest))
                {
                    fedExShipment.SmartPostCustomerManifest = smartPostCustomerManifest;
                    smartPostShipmentDetail.CustomerManifestId = fedExShipment.SmartPostCustomerManifest;
                }
            }

            // For smartpost this is always zero
            requestedShipment.TotalInsuredValue = null; 

            requestedShipment.SmartPostDetail = smartPostShipmentDetail;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private static RequestedShipment InitializeRequest(CarrierRequest request)
        {
            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

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
        /// Get the FedEx API value that correspons to our internal value
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
