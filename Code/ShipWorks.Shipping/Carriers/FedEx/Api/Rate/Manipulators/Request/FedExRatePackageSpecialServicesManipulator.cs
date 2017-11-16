using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
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
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options)
        {
            return !options.HasFlag(FedExRateRequestOptions.LtlFreight);
        }

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
            IEnumerable<PackageSpecialServiceType> specialServices = specialServicesRequested.Ensure(x => x.SpecialServiceTypes);

            if (fedExSignatureType != FedExSignatureType.ServiceDefault)
            {
                var account = settings.GetAccountReadOnly(shipment);
                specialServicesRequested.SignatureOptionDetail = new SignatureOptionDetail
                {
                    OptionType = GetApiSignatureType(fedExSignatureType),
                    OptionTypeSpecified = true,
                    SignatureReleaseNumber = account.SignatureRelease
                };

                specialServices = specialServices.Append(PackageSpecialServiceType.SIGNATURE_OPTION);
            }

            ServiceType apiServiceType = GetApiServiceType((FedExServiceType) shipment.FedEx.Service);

            // Non-standard container (only applies to Ground services)
            if (shipment.FedEx.NonStandardContainer && (apiServiceType == ServiceType.GROUND_HOME_DELIVERY || apiServiceType == ServiceType.FEDEX_GROUND))
            {
                specialServices = specialServices.Append(PackageSpecialServiceType.NON_STANDARD_CONTAINER);
            }

            if (fedExPackage.ContainsAlcohol)
            {
                specialServices = specialServices.Append(PackageSpecialServiceType.ALCOHOL);
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
        private static ServiceType GetApiServiceType(FedExServiceType serviceType)
        {
            return (ServiceType) EnumHelper.GetApiValue<ServiceType>(serviceType);
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
