using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the package special services values of the
    /// FedEx API's RateRequest object.
    /// </summary>
    public class FedExRatePackageSpecialServicesManipulator : IFedExRateRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExRatePackageSpecialServicesManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options) => true;

        /// <summary>
        /// Adds SpecialServices to the FedEx request object's packages
        /// </summary>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            InitializeRequest(request);

            request.RequestedShipment.RequestedPackageLineItems = shipment.FedEx
                .Packages
                .Zip(RequestPackageList(request, shipment.FedEx.Packages.Count()), Tuple.Create)
                .Select(x => BuildPackageSpecialServices(shipment, x.Item1, x.Item2 ?? CreateLineItem()))
                .ToArray();

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
        private RequestedPackageLineItem BuildPackageSpecialServices(
            IShipmentEntity shipment,
            IFedExPackageEntity fedExPackage,
            RequestedPackageLineItem packageRequest)
        {
            var specialServicesRequested = packageRequest.Ensure(x => x.SpecialServicesRequested);

            // Signature
            FedExSignatureType fedExSignatureType = (FedExSignatureType) shipment.FedEx.Signature;
            if (fedExSignatureType == FedExSignatureType.NoSignature && shipment.FedEx.Packages.Any(p => p.DeclaredValue > 500m))
            {
                // The FedEx API allows for this to go through, but per page 170 of the FedExDeveloperGuide2012.pdf document,
                // a signature is required for shipments with a declared value > $500
                throw new FedExException("A signature is required for shipments containing packages with a declared value over $500.");
            }

            // Start a new special services list for this package
            List<PackageSpecialServiceType> specialServices = new List<PackageSpecialServiceType>();

            if (fedExSignatureType != FedExSignatureType.ServiceDefault)
            {
                var account = settings.GetAccountReadOnly(shipment);
                specialServicesRequested.SignatureOptionDetail = new SignatureOptionDetail
                {
                    OptionType = GetApiSignatureType(fedExSignatureType),
                    OptionTypeSpecified = true,
                    SignatureReleaseNumber = account.SignatureRelease
                };

                specialServices.Add(PackageSpecialServiceType.SIGNATURE_OPTION);
            }

            ServiceType apiServiceType = GetApiServiceType((FedExServiceType) shipment.FedEx.Service);

            // Non-standard container (only applies to Ground services)
            if (shipment.FedEx.NonStandardContainer && (apiServiceType == ServiceType.GROUND_HOME_DELIVERY || apiServiceType == ServiceType.FEDEX_GROUND))
            {
                specialServices.Add(PackageSpecialServiceType.NON_STANDARD_CONTAINER);
            }

            if (fedExPackage.ContainsAlcohol)
            {
                specialServices.Add(PackageSpecialServiceType.ALCOHOL);
            }

            // Set the special service type flags
            specialServicesRequested.SpecialServiceTypes = specialServices.ToArray();

            return packageRequest;
        }

        /// <summary>
        /// Gets the type of the API service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Invalid FedEx ServiceType  + serviceType</exception>
        [NDependIgnoreComplexMethodAttribute]
        private static ServiceType GetApiServiceType(FedExServiceType serviceType)
        {
            switch (serviceType)
            {
                case FedExServiceType.PriorityOvernight:
                case FedExServiceType.OneRatePriorityOvernight:
                    return ServiceType.PRIORITY_OVERNIGHT;

                case FedExServiceType.StandardOvernight:
                case FedExServiceType.OneRateStandardOvernight:
                    return ServiceType.STANDARD_OVERNIGHT;

                case FedExServiceType.FirstOvernight:
                case FedExServiceType.OneRateFirstOvernight:
                    return ServiceType.FIRST_OVERNIGHT;

                case FedExServiceType.FedEx2Day:
                case FedExServiceType.OneRate2Day:
                    return ServiceType.FEDEX_2_DAY;

                case FedExServiceType.FedEx2DayAM:
                case FedExServiceType.OneRate2DayAM:
                    return ServiceType.FEDEX_2_DAY_AM;

                case FedExServiceType.FedExExpressSaver:
                case FedExServiceType.OneRateExpressSaver:
                case FedExServiceType.FedExEconomyCanada:
                    return ServiceType.FEDEX_EXPRESS_SAVER;

                case FedExServiceType.InternationalPriority: return ServiceType.INTERNATIONAL_PRIORITY;
                case FedExServiceType.InternationalPriorityExpress: return ServiceType.INTERNATIONAL_PRIORITY_EXPRESS;
                case FedExServiceType.InternationalEconomy: return ServiceType.INTERNATIONAL_ECONOMY;
                case FedExServiceType.InternationalFirst: return ServiceType.INTERNATIONAL_FIRST;
                case FedExServiceType.FedEx1DayFreight: return ServiceType.FEDEX_1_DAY_FREIGHT;
                case FedExServiceType.FedEx2DayFreight: return ServiceType.FEDEX_2_DAY_FREIGHT;
                case FedExServiceType.FedEx3DayFreight: return ServiceType.FEDEX_3_DAY_FREIGHT;

                case FedExServiceType.FedExGround:
                case FedExServiceType.FedExInternationalGround:
                    return ServiceType.FEDEX_GROUND;

                case FedExServiceType.GroundHomeDelivery: return ServiceType.GROUND_HOME_DELIVERY;
                case FedExServiceType.InternationalPriorityFreight: return ServiceType.INTERNATIONAL_PRIORITY_FREIGHT;
                case FedExServiceType.InternationalEconomyFreight: return ServiceType.INTERNATIONAL_ECONOMY_FREIGHT;
                case FedExServiceType.SmartPost: return ServiceType.SMART_POST;
                case FedExServiceType.FirstFreight: return ServiceType.FEDEX_FIRST_FREIGHT;

                case FedExServiceType.FedExNextDayAfternoon: return ServiceType.FEDEX_NEXT_DAY_AFTERNOON;
                case FedExServiceType.FedExNextDayEndOfDay: return ServiceType.FEDEX_NEXT_DAY_END_OF_DAY;
                case FedExServiceType.FedExNextDayMidMorning: return ServiceType.FEDEX_NEXT_DAY_MID_MORNING;
                case FedExServiceType.FedExNextDayEarlyMorning: return ServiceType.FEDEX_NEXT_DAY_EARLY_MORNING;
                case FedExServiceType.FedExDistanceDeferred: return ServiceType.FEDEX_DISTANCE_DEFERRED;
                case FedExServiceType.FedExNextDayFreight: return ServiceType.FEDEX_NEXT_DAY_FREIGHT;
            }

            throw new InvalidOperationException("Invalid FedEx ServiceType " + serviceType);
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
                case FedExSignatureType.NoSignature: return SignatureOptionType.NO_SIGNATURE_REQUIRED;
            }

            return SignatureOptionType.SERVICE_DEFAULT;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private static void InitializeRequest(RateRequest request)
        {
            request.Ensure(x => x.RequestedShipment)
                .Ensure(x => x.RequestedPackageLineItems);
        }
    }
}
