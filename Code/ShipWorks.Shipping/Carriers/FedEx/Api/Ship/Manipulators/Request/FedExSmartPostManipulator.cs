using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
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
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) =>
            (FedExServiceType) shipment.FedEx.Service == FedExServiceType.SmartPost;

        /// <summary>
        /// Add the SmartPost to the FedEx carrier request
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            // Get the FedEx shipment and account
            IFedExShipmentEntity fedExShipment = shipment.FedEx;
            IFedExAccountEntity account = settings.GetAccountReadOnly(shipment);

            // Get the RequestedShipment object for the request
            RequestedShipment requestedShipment = InitializeRequest(request);

            return GenericResult.FromSuccess(new SmartPostShipmentDetail())
                .Bind(detail => SetHubId(fedExShipment, account, detail))
                .Bind(detail => SetIndicia(fedExShipment, detail))
                .Bind(detail => SetEndorsementType(fedExShipment, detail))
                .Do(detail => SetCustomerManifest(fedExShipment, detail))
                .Map(detail =>
                {
                    // For smartpost this is always zero
                    requestedShipment.TotalInsuredValue = null;
                    requestedShipment.SmartPostDetail = detail;
                    return request;
                });
        }

        /// <summary>
        /// Set the Hub ID
        /// </summary>
        private static Result SetHubId(IFedExShipmentEntity fedExShipment, IFedExAccountEntity account, SmartPostShipmentDetail detail)
        {
            string smartPostHubID = FedExUtility.GetSmartPostHub(fedExShipment.SmartPostHubID, account);
            if (string.IsNullOrWhiteSpace(smartPostHubID))
            {
                return new CarrierException("A SmartPost Hub ID is required for shipping with SmartPost.");
            }

            detail.HubId = fedExShipment.SmartPostHubID;

            return Result.FromSuccess();
        }

        /// <summary>
        /// Set the indecia value
        /// </summary>
        private static Result SetIndicia(IFedExShipmentEntity fedExShipment, SmartPostShipmentDetail detail) =>
            GetSmartPostIndiciaType((FedExSmartPostIndicia) fedExShipment.SmartPostIndicia)
                .Do(x =>
                {
                    detail.Indicia = x;
                    detail.IndiciaSpecified = true;
                });

        /// <summary>
        /// Set the endorsement type
        /// </summary>
        private static Result SetEndorsementType(IFedExShipmentEntity fedExShipment, SmartPostShipmentDetail detail) =>
            GetSmartPostEndorsementType((FedExSmartPostEndorsement) fedExShipment.SmartPostEndorsement)
                .Map(endorsementType => endorsementType.Match(v => detail.AncillaryEndorsement = v, () => { }))
                .Do(hasValue => detail.AncillaryEndorsementSpecified = hasValue);

        /// <summary>
        /// Set the customer manifest
        /// </summary>
        private static void SetCustomerManifest(IFedExShipmentEntity fedExShipment, SmartPostShipmentDetail detail)
        {
            if (!string.IsNullOrWhiteSpace(fedExShipment.SmartPostCustomerManifest))
            {
                string smartPostCustomerManifest = TemplateTokenProcessor
                    .ProcessTokens(fedExShipment.SmartPostCustomerManifest, fedExShipment.ShipmentID);

                if (!string.IsNullOrWhiteSpace(smartPostCustomerManifest))
                {
                    detail.CustomerManifestId = smartPostCustomerManifest;
                }
            }
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private static RequestedShipment InitializeRequest(ProcessShipmentRequest request)
        {
            RequestedShipment requestedShipment = request.Ensure(r => r.RequestedShipment);
            requestedShipment.EnsureAtLeastOne(x => x.RequestedPackageLineItems);
            return requestedShipment;
        }

        /// <summary>
        /// Get the FedEx API value that corresponds to our internal value
        /// </summary>
        private static GenericResult<SmartPostIndiciaType> GetSmartPostIndiciaType(FedExSmartPostIndicia indicia)
        {
            switch (indicia)
            {
                case FedExSmartPostIndicia.ParcelSelect: return SmartPostIndiciaType.PARCEL_SELECT;
                case FedExSmartPostIndicia.MediaMail: return SmartPostIndiciaType.MEDIA_MAIL;
                case FedExSmartPostIndicia.BoundPrintedMatter: return SmartPostIndiciaType.PRESORTED_BOUND_PRINTED_MATTER;
                case FedExSmartPostIndicia.PresortedStandard: return SmartPostIndiciaType.PRESORTED_STANDARD;
                case FedExSmartPostIndicia.ParcelReturn: return SmartPostIndiciaType.PARCEL_RETURN;
            }

            return new CarrierException("Invalid indicia type: " + indicia);
        }

        /// <summary>
        /// Get the FedEx API value that corresponds to our internal value
        /// </summary>
        private static GenericResult<SmartPostAncillaryEndorsementType?> GetSmartPostEndorsementType(FedExSmartPostEndorsement endorsement)
        {
            switch (endorsement)
            {
                case FedExSmartPostEndorsement.AddressCorrection: return SmartPostAncillaryEndorsementType.ADDRESS_CORRECTION;
                case FedExSmartPostEndorsement.ChangeService: return SmartPostAncillaryEndorsementType.CHANGE_SERVICE;
                case FedExSmartPostEndorsement.ForwardingService: return SmartPostAncillaryEndorsementType.FORWARDING_SERVICE;
                case FedExSmartPostEndorsement.LeaveIfNoResponse: return SmartPostAncillaryEndorsementType.CARRIER_LEAVE_IF_NO_RESPONSE;
                case FedExSmartPostEndorsement.ReturnService: return SmartPostAncillaryEndorsementType.RETURN_SERVICE;
                case FedExSmartPostEndorsement.None: return GenericResult.FromSuccess<SmartPostAncillaryEndorsementType?>(null);
            }

            return new CarrierException("Invalid endorsement value: " + endorsement);
        }
    }
}
