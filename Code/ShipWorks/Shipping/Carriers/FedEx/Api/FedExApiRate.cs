using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using System.Web.Services.Protocols;
using System.Net;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using Interapptive.Shared.Business;
using log4net;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Used for accessing the FedEx api rate service
    /// </summary>
    public static class FedExApiRate
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FedExApiRate));

        /// <summary>
        /// Create the webserivce instance with the appropriate URL
        /// </summary>
        private static RateService CreateRateService()
        {
            RateService webService = new RateService(new ApiLogEntry(ApiLogSource.FedEx, "Rates"));
            webService.Url = FedExApiCore.ServerUrl;

            return webService;
        }

        /// <summary>
        /// Process the rates for the given shipment
        /// </summary>
        public static List<RateReplyDetail> GetRates(ShipmentEntity shipment)
        {
            FedExShipmentEntity fedex = shipment.FedEx;

            FedExAccountEntity account = FedExAccountManager.GetAccount(fedex.FedExAccountID);
            if (account == null)
            {
                throw new FedExException("No FedEx account is selected for the shipment.");
            }
            else if (account.Is2xMigrationPending)
            {
                throw new FedExException("The FedEx account selected for the shipment was migrated from ShipWorks 2, and has not yet been configured for ShipWorks 3.");
            }

            List<RateReplyDetail> rates = new List<RateReplyDetail>();

            // Do the initial rating on the shipment as-is
            RateRequest primaryRequest = CreateBaseRateRequest(shipment, account);
            
            // FedEx was generating an error when a Rate request had more than 2 lines for the street address
            int numberOfStreetLines = primaryRequest.RequestedShipment.Recipient.Address.StreetLines.Length;
            if (numberOfStreetLines > 2)
            {
                // Remove the address lines 3 through n
                List<string> adjustedStreetLines = primaryRequest.RequestedShipment.Recipient.Address.StreetLines.ToList();
                adjustedStreetLines.RemoveRange(2, numberOfStreetLines - 2);

                primaryRequest.RequestedShipment.Recipient.Address.StreetLines = adjustedStreetLines.ToArray();
            }

            rates.AddRange(ProcessRateRequest(primaryRequest));

            // If they chose non-standard container - we have to redo it for ground and home-delivery to get the non-standard container rates
            if (fedex.NonStandardContainer)
            {
                // Remove the rates we got for ground already (which would be without non-standard container)
                rates.RemoveAll(r => r.ServiceType == ServiceType.GROUND_HOME_DELIVERY || r.ServiceType == ServiceType.FEDEX_GROUND);

                // Update our request to specify non-standard container
                foreach (var package in primaryRequest.RequestedShipment.RequestedPackageLineItems)
                {
                    var existingList = package.SpecialServicesRequested.SpecialServiceTypes.ToList();
                    existingList.Add(PackageSpecialServiceType.NON_STANDARD_CONTAINER);

                    package.SpecialServicesRequested.SpecialServiceTypes = existingList.ToArray();
                }

                // Add in the updated ground and home-delivery rates
                rates.AddRange(
                    ProcessRateRequest(primaryRequest)
                        .Where(r => r.ServiceType == ServiceType.GROUND_HOME_DELIVERY || r.ServiceType == ServiceType.FEDEX_GROUND));
            }

            // Add in smartpost (if configured)
            string smartPostHub = FedExUtility.GetSmartPostHub(fedex.SmartPostHubID, account);
            if (!string.IsNullOrEmpty(smartPostHub))
            {
                RateRequest smartPostRequest = CreateBaseRateRequest(shipment, account);

                ConfigureForSmartPost(smartPostRequest, shipment, account);

                try
                {
                    List<RateReplyDetail> smartPostRates = ProcessRateRequest(smartPostRequest);
                    rates.AddRange(smartPostRates);
                }
                catch (FedExException ex)
                {
                    log.Warn("Error getting SmartPost rates: " + ex.Message);
                }
            }

            return rates;
        }

        /// <summary>
        /// Create the basic \ base reate request based on the given shipment and account
        /// </summary>
        private static RateRequest CreateBaseRateRequest(ShipmentEntity shipment, FedExAccountEntity account)
        {
            FedExShipmentEntity fedex = shipment.FedEx;
            

            // Ensure we have done version capture
            FedExApiRegistration.VersionCapture();

            RateRequest request = new RateRequest();

            // Authentication
            request.WebAuthenticationDetail = FedExApiCore.GetWebAuthenticationDetail<WebAuthenticationDetail>();

            // Client 
            request.ClientDetail = FedExApiCore.GetClientDetail<ClientDetail>(account);

            // Version
            request.Version = new VersionId
            {
                ServiceId = "crs",
                Major = 7,
                Intermediate = 0,
                Minor = 0
            };

            List<ShipmentSpecialServiceType> specialServiceTypes = new List<ShipmentSpecialServiceType>();

            // We want time in transit
            request.ReturnTransitAndCommit = true;
            request.ReturnTransitAndCommitSpecified = true;

            RequestedShipment detail = new RequestedShipment();
            request.RequestedShipment = detail;

            // To hold any specials we may use
            detail.SpecialServicesRequested = new ShipmentSpecialServicesRequested();

            // Where it's coming from
            detail.Shipper = new Party();
            detail.Shipper.Address = FedExApiCore.CreateAddress<Address>(new PersonAdapter(shipment, "Origin"));

            // Where it's going
            detail.Recipient = new Party();
            detail.Recipient.Address = FedExApiCore.CreateAddress<Address>(new PersonAdapter(shipment, "Ship"));

            if (ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx).IsResidentialStatusRequired(shipment))
            {
                detail.Recipient.Address.Residential = shipment.ResidentialResult;
                detail.Recipient.Address.ResidentialSpecified = true;
            }

            // When its giong out
            detail.ShipTimestamp = shipment.ShipDate;
            detail.ShipTimestampSpecified = true;

            // If its future, set that flag
            if (detail.ShipTimestamp.Date != DateTime.Today)
            {
                specialServiceTypes.Add(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT);
            }

            // If its a saturday, set the pickup flag
            if (detail.ShipTimestamp.DayOfWeek == DayOfWeek.Saturday)
            {
                specialServiceTypes.Add(ShipmentSpecialServiceType.SATURDAY_PICKUP);
            }

            // If they want saturday delivery, and it could be delivered on a saturday, set that flag.
            // Note: we don't usually use the selected service for figuring what the rates are - but we do here, since we only want
            // to use the saturday flag if the user can acutally see the saturday checkbox.
            if (shipment.FedEx.SaturdayDelivery && FedExUtility.CanDeliverOnSaturday((FedExServiceType) fedex.Service, detail.ShipTimestamp))
            {
                specialServiceTypes.Add(ShipmentSpecialServiceType.SATURDAY_DELIVERY);
            }

            // How its getting picked up
            detail.DropoffType = DropoffType.REGULAR_PICKUP;
            detail.DropoffTypeSpecified = true;

            // Get the type of packaging used
            detail.PackagingType = GetApiPackagingType((FedExPackagingType) fedex.PackagingType);
            detail.PackagingTypeSpecified = true;

            // We just want the actual account rates
            detail.RateRequestTypes = new RateRequestType[] { FedExApiCore.UseListRates ? RateRequestType.LIST : RateRequestType.ACCOUNT };

            // Weight and declared value
            detail.TotalWeight = new Weight { Units = WeightUnits.LB, Value = (decimal) shipment.TotalWeight };
            detail.TotalInsuredValue = new Money { Currency = "USD", Amount = shipment.FedEx.Packages.Sum(p => p.DeclaredValue) };

            // Package count
            detail.PackageCount = fedex.Packages.Count.ToString();
            detail.PackageDetail = RequestedPackageDetailType.INDIVIDUAL_PACKAGES;
            detail.PackageDetailSpecified = true;

            // Special services
            detail.SpecialServicesRequested.SpecialServiceTypes = specialServiceTypes.ToArray();

            List<RequestedPackageLineItem> packages = new List<RequestedPackageLineItem>();

            // Build the list of packages
            foreach (FedExPackageEntity source in shipment.FedEx.Packages)
            {
                RequestedPackageLineItem package = new RequestedPackageLineItem();
                packages.Add(package);

                package.SequenceNumber = packages.Count.ToString();

                package.Weight = new Weight { Units = WeightUnits.LB, Value = FedExUtility.GetPackageTotalWeight(source) };
                package.InsuredValue = new Money { Amount = source.DeclaredValue, Currency = "USD" };

                if (fedex.PackagingType == (int) FedExPackagingType.Custom)
                {
                    package.Dimensions = new Dimensions();
                    package.Dimensions.Units = LinearUnits.IN;
                    package.Dimensions.Length = ((int) source.DimsLength).ToString();
                    package.Dimensions.Height = ((int) source.DimsHeight).ToString();
                    package.Dimensions.Width = ((int) source.DimsWidth).ToString();
                }

                // Prepare for possible special services
                PackageSpecialServicesRequested services = new PackageSpecialServicesRequested();
                package.SpecialServicesRequested = services;

                List<PackageSpecialServiceType> specialServices = new List<PackageSpecialServiceType>();

                // Signature
                FedExSignatureType signature = (FedExSignatureType) fedex.Signature;
                if (signature != FedExSignatureType.ServiceDefault)
                {
                    services.SignatureOptionDetail = new SignatureOptionDetail();
                    services.SignatureOptionDetail.OptionType = GetApiSignatureType(signature);
                    services.SignatureOptionDetail.SignatureReleaseNumber = account.SignatureRelease;

                    specialServices.Add(PackageSpecialServiceType.SIGNATURE_OPTION);
                }

                services.SpecialServiceTypes = specialServices.ToArray();
            }

            // Set the packages
            detail.RequestedPackageLineItems = packages.ToArray();

            return request;
        }

        /// <summary>
        /// Configure the given request for smartpost
        /// </summary>
        private static void ConfigureForSmartPost(RateRequest request, ShipmentEntity shipment, FedExAccountEntity account)
        {
            FedExShipmentEntity fedex = shipment.FedEx;

            RequestedShipment detail = request.RequestedShipment;

            SmartPostShipmentDetail smartPost = new SmartPostShipmentDetail();
            detail.SmartPostDetail = smartPost;

            // Only get rates for smartpost
            detail.ServiceType = ServiceType.SMART_POST;
            detail.ServiceTypeSpecified = true;

            smartPost.HubId = FedExUtility.GetSmartPostHub(fedex.SmartPostHubID, account);

            smartPost.Indicia = GetSmartPostIndiciaType((FedExSmartPostIndicia) fedex.SmartPostIndicia);
            smartPost.IndiciaSpecified = true;

            var endorsement = GetSmartPostEndorsementType((FedExSmartPostEndorsement) fedex.SmartPostEndorsement);
            if (endorsement != null)
            {
                smartPost.AncillaryEndorsement = endorsement.Value;
                smartPost.AncillaryEndorsementSpecified = true;
            }

            if (smartPost.Indicia == SmartPostIndiciaType.PARCEL_SELECT || fedex.SmartPostConfirmation)
            {
                smartPost.SpecialServices = new SmartPostShipmentSpecialServiceType[] { SmartPostShipmentSpecialServiceType.USPS_DELIVERY_CONFIRMATION };
            }

            // SmartPost won't return rates if insured value is greater than zero
            detail.TotalInsuredValue.Amount = 0;
            foreach (var package in detail.RequestedPackageLineItems)
            {
                package.InsuredValue.Amount = 0;
            }
        }

        /// <summary>
        /// Process the given (already configured) rate request
        /// </summary>
        private static List<RateReplyDetail> ProcessRateRequest(RateRequest request)
        {
            try
            {                
                using (RateService webService = CreateRateService())
                {
                    RateReply reply = webService.getRates(request);

                    if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
                    {
                        throw new FedExApiException(reply.Notifications);
                    }

                    if (reply.RateReplyDetails == null)
                    {
                        if (reply.Notifications.Any(n => n.Code == "556" || n.Code == "557" || n.Code == "558"))
                        {
                            throw new FedExException("There are no FedEx services available for the selected shipment options.");
                        }

                        throw new FedExException("FedEx did not return any rates for the shipment.");
                    }

                    return reply.RateReplyDetails.ToList();
                }
            }
            catch (SoapException ex)
            {
                throw new FedExSoapException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
        }

        /// <summary>
        /// Get the API value for our internal signature type
        /// </summary>
        private static SignatureOptionType GetApiSignatureType(FedExSignatureType signature)
        {
            switch (signature)
            {
                case FedExSignatureType.Indirect: return SignatureOptionType.INDIRECT;
                case FedExSignatureType.Direct: return SignatureOptionType.DIRECT;
                case FedExSignatureType.Adult: return SignatureOptionType.ADULT;
            }

            return SignatureOptionType.SERVICE_DEFAULT;
        }

        /// <summary>
        /// Get the API value for the given packaging type
        /// </summary>
        private static PackagingType GetApiPackagingType(FedExPackagingType packagingType)
        {
            switch (packagingType)
            {
                case FedExPackagingType.Box: return PackagingType.FEDEX_BOX;
                case FedExPackagingType.Box10Kg: return PackagingType.FEDEX_10KG_BOX;
                case FedExPackagingType.Box25Kg: return PackagingType.FEDEX_25KG_BOX;
                case FedExPackagingType.Custom: return PackagingType.YOUR_PACKAGING;
                case FedExPackagingType.Envelope: return PackagingType.FEDEX_ENVELOPE;
                case FedExPackagingType.Pak: return PackagingType.FEDEX_PAK;
                case FedExPackagingType.Tube: return PackagingType.FEDEX_TUBE;
            }

            throw new InvalidOperationException("Invalid FedEx Packaging Type");
        }

        /// <summary>
        /// Get our own FedExServiceType value for the given rate ServiceType
        /// </summary>
        public static FedExServiceType GetFedExServiceType(ServiceType serviceType)
        {
            switch (serviceType)
            {
                case ServiceType.PRIORITY_OVERNIGHT: return FedExServiceType.PriorityOvernight;
                case ServiceType.STANDARD_OVERNIGHT: return FedExServiceType.StandardOvernight;
                case ServiceType.FIRST_OVERNIGHT: return FedExServiceType.FirstOvernight;
                case ServiceType.FEDEX_2_DAY: return FedExServiceType.FedEx2Day;
                case ServiceType.FEDEX_EXPRESS_SAVER: return FedExServiceType.FedExExpressSaver;
                case ServiceType.INTERNATIONAL_PRIORITY: return FedExServiceType.InternationalPriority;
                case ServiceType.INTERNATIONAL_ECONOMY: return FedExServiceType.InternationalEconomy;
                case ServiceType.INTERNATIONAL_FIRST: return FedExServiceType.InternationalFirst;
                case ServiceType.FEDEX_1_DAY_FREIGHT: return FedExServiceType.FedEx1DayFreight;
                case ServiceType.FEDEX_2_DAY_FREIGHT: return FedExServiceType.FedEx2DayFreight;
                case ServiceType.FEDEX_3_DAY_FREIGHT: return FedExServiceType.FedEx3DayFreight;
                case ServiceType.FEDEX_GROUND: return FedExServiceType.FedExGround;
                case ServiceType.GROUND_HOME_DELIVERY: return FedExServiceType.GroundHomeDelivery;
                case ServiceType.INTERNATIONAL_PRIORITY_FREIGHT: return FedExServiceType.InternationalPriorityFreight;
                case ServiceType.INTERNATIONAL_ECONOMY_FREIGHT: return FedExServiceType.InternationalEconomyFreight;
                case ServiceType.SMART_POST: return FedExServiceType.SmartPost;
            }

            throw new InvalidOperationException("Invalid FedEx ServiceType " + serviceType);
        }

        /// <summary>
        /// Get the integer number of days for the given fedex transit time value
        /// </summary>
        public static int GetTransitDays(TransitTimeType transitTime)
        {
            switch (transitTime)
            {
                case TransitTimeType.ONE_DAY: return 1;
                case TransitTimeType.TWO_DAYS: return 2;
                case TransitTimeType.THREE_DAYS: return 3;
                case TransitTimeType.FOUR_DAYS: return 4;
                case TransitTimeType.FIVE_DAYS: return 5;
                case TransitTimeType.SIX_DAYS: return 6;
                case TransitTimeType.SEVEN_DAYS: return 7;
                case TransitTimeType.EIGHT_DAYS: return 8;
                case TransitTimeType.NINE_DAYS: return 9;
                case TransitTimeType.TEN_DAYS: return 10;
                case TransitTimeType.ELEVEN_DAYS: return 11;
                case TransitTimeType.TWELVE_DAYS: return 12;
                case TransitTimeType.THIRTEEN_DAYS: return 13;
                case TransitTimeType.FOURTEEN_DAYS: return 14;
                case TransitTimeType.FIFTEEN_DAYS: return 15;
                case TransitTimeType.SIXTEEN_DAYS: return 16;
                case TransitTimeType.SEVENTEEN_DAYS: return 17;
                case TransitTimeType.EIGHTEEN_DAYS: return 18;
                case TransitTimeType.NINETEEN_DAYS: return 19;
                case TransitTimeType.TWENTY_DAYS: return 20;
            }

            return 0;
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

            throw new InvalidOperationException("Invalid endorsement value: " + endorsement);
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
            }

            throw new InvalidOperationException("Invalid indicia type: " + indicia);
        }
    }
}
