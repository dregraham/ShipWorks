using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Extensions;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the attributes of a SmartPost 
    /// attributes of the shipment and each package within the FedEx API's RateRequest object if
    /// the shipment has a FedEx SmartPost HubID.
    /// </summary>
    public class FedExRateSmartPostManipulator : IFedExRateRequestManipulator
    {
        readonly IFedExSettingsRepository settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExRateSmartPostManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options) =>
            options.HasFlag(FedExRateRequestOptions.SmartPost) && FedExUtility.IsSmartPostEnabled(shipment);

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            InitializeRequest(request);

            var fedExShipment = shipment.FedEx;

            SmartPostShipmentDetail smartPostDetail = new SmartPostShipmentDetail();
            smartPostDetail.HubId = FedExUtility.GetSmartPostHub(fedExShipment.SmartPostHubID, settings.GetAccountReadOnly(shipment));

            smartPostDetail.Indicia = GetSmartPostIndiciaType((FedExSmartPostIndicia) fedExShipment.SmartPostIndicia);
            smartPostDetail.IndiciaSpecified = true;

            SmartPostAncillaryEndorsementType? endorsement = GetSmartPostEndorsementType((FedExSmartPostEndorsement) fedExShipment.SmartPostEndorsement);
            if (endorsement != null)
            {
                smartPostDetail.AncillaryEndorsement = endorsement.Value;
                smartPostDetail.AncillaryEndorsementSpecified = true;
            }

            // Smart Post rates are only retrieved by explicitly setting the service type
            request.RequestedShipment.ServiceType = ServiceType.SMART_POST;
            request.RequestedShipment.ServiceTypeSpecified = true;

            // Smart post won't return rates if insured value is greater than 0
            request.RequestedShipment.TotalInsuredValue.Amount = 0;

            request.RequestedShipment.RequestedPackageLineItems = shipment.FedEx
                .Packages
                .Zip(RequestPackageList(request, shipment.FedEx.Packages.Count()), Tuple.Create)
                .Select(x => BuildInsuredValue(x.Item2 ?? CreateLineItem()))
                .ToArray();

            request.RequestedShipment.SmartPostDetail = smartPostDetail;

            return request;
        }
        /// <summary>
        /// Get a list of request packages, padding any needed packages with nulls
        /// </summary>
        private static IEnumerable<RequestedPackageLineItem> RequestPackageList(RateRequest request, int packageCount) =>
            request.RequestedShipment.RequestedPackageLineItems.Concat(Enumerable.Repeat<RequestedPackageLineItem>(null, packageCount));

        /// <summary>
        /// Create a new line item
        /// </summary>
        private RequestedPackageLineItem CreateLineItem() => new RequestedPackageLineItem();

        /// <summary>
        /// Build package special services element
        /// </summary>
        private RequestedPackageLineItem BuildInsuredValue(RequestedPackageLineItem packageRequest)
        {
            if (packageRequest.InsuredValue == null)
            {
                packageRequest.InsuredValue = new Money();
            }

            packageRequest.InsuredValue.Amount = 0;

            return packageRequest;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private static void InitializeRequest(RateRequest request)
        {
            request.Ensure(x => x.RequestedShipment)
                .Ensure(x => x.RequestedPackageLineItems);
            request.RequestedShipment.Ensure(x => x.TotalInsuredValue);
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
