using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// Adds SpecialServices to the FedEx request object's packages
    /// </summary>
    public class FedExPackageSpecialServicesManipulator : IFedExShipRequestManipulator
    {
        readonly IFedExSettingsRepository settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExPackageSpecialServicesManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) =>
            !FedExUtility.IsFreightLtlService(shipment.FedEx.Service);

        /// <summary>
        /// Adds SpecialServices to the FedEx request object's packages
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            var fedex = shipment.FedEx;

            // Each package should be in it's own request, so we always use the first item in the line item array
            PackageSpecialServicesRequested specialServicesRequested = request.Ensure(x => x.RequestedShipment)
                .EnsureAtLeastOne(x => x.RequestedPackageLineItems)
                .Ensure(x => x.SpecialServicesRequested);
            IEnumerable<PackageSpecialServiceType> specialServices = specialServicesRequested.Ensure(x => x.SpecialServiceTypes);

            // Signature
            FedExSignatureType fedExSignatureType = (FedExSignatureType) fedex.Signature;
            if (fedExSignatureType != FedExSignatureType.ServiceDefault)
            {
                specialServicesRequested.SignatureOptionDetail = new SignatureOptionDetail
                {
                    OptionType = GetApiSignatureType(fedExSignatureType),
                    SignatureReleaseNumber = settings.GetAccountReadOnly(shipment).SignatureRelease
                };

                specialServices = specialServices.Append(PackageSpecialServiceType.SIGNATURE_OPTION);
            }

            FedExServiceType serviceType = (FedExServiceType) fedex.Service;

            // Non-standard container (only applies to Ground services)
            if (fedex.NonStandardContainer && (serviceType == FedExServiceType.GroundHomeDelivery || serviceType == FedExServiceType.FedExGround))
            {
                specialServices = specialServices.Append(PackageSpecialServiceType.NON_STANDARD_CONTAINER);
            }

            // Add Alcohol if selected
            var package = fedex.Packages.ElementAt(sequenceNumber);
            if (package.ContainsAlcohol)
            {
                specialServices = specialServices.Append(PackageSpecialServiceType.ALCOHOL);
                specialServicesRequested.AlcoholDetail = new AlcoholDetail()
                {
                    // TODO: Will this always be Consumer?  Or do we need a setting?
                    // TODO: If it's a return, would it need to be LICENSEE?
                    RecipientType = (AlcoholRecipientType) package.AlcoholRecipientType,
                    RecipientTypeSpecified = true,
                };
            }

            // Set the special service type flags
            specialServicesRequested.SpecialServiceTypes = specialServices.ToArray();

            return request;
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
    }
}
