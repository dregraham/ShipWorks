using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Adds SpecialServices to the FedEx request object's packages
    /// </summary>
    public class FedExPackageSpecialServicesManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPackageSpecialServicesManipulator" /> class.
        /// </summary>
        public FedExPackageSpecialServicesManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPackageSpecialServicesManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExPackageSpecialServicesManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Adds SpecialServices to the FedEx request object's packages
        /// </summary>
        /// <param name="request"></param>
        public override void Manipulate(CarrierRequest request)
        {
            IFedExNativeShipmentRequest nativeRequest = InitializeShipmentRequest(request);

            FedExShipmentEntity fedex = request.ShipmentEntity.FedEx;
            List<PackageSpecialServiceType> specialServices = new List<PackageSpecialServiceType>();

            // Each package should be in it's own request, so we always use the first item in the line item aray
            PackageSpecialServicesRequested specialServicesRequested = InitializePackageRequest(nativeRequest.RequestedShipment.RequestedPackageLineItems[0]);

            // Signature
            FedExSignatureType fedExSignatureType = (FedExSignatureType) fedex.Signature;
            if (fedExSignatureType != FedExSignatureType.ServiceDefault)
            {
                FedExAccountEntity fedExAccount = request.CarrierAccountEntity as FedExAccountEntity;

                specialServicesRequested.SignatureOptionDetail = new SignatureOptionDetail
                {
                    OptionType = GetApiSignatureType(fedExSignatureType),
                    SignatureReleaseNumber = fedExAccount.SignatureRelease
                };

                specialServices.Add(PackageSpecialServiceType.SIGNATURE_OPTION);
            }

            ServiceType apiServiceType = FedExRequestManipulatorUtilities.GetApiServiceType((FedExServiceType) fedex.Service);

            // Non-standard container (only applies to Ground services)
            if (fedex.NonStandardContainer && (apiServiceType == ServiceType.GROUND_HOME_DELIVERY || apiServiceType == ServiceType.FEDEX_GROUND))
            {
                specialServices.Add(PackageSpecialServiceType.NON_STANDARD_CONTAINER);
            }

            // Add Alcohol if selected
            int currentPackageIndex = request.SequenceNumber;
            FedExPackageEntity package = fedex.Packages[currentPackageIndex];
            if (package.ContainsAlcohol)
            {
                specialServices.Add(PackageSpecialServiceType.ALCOHOL);
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
        }

        /// <summary>
        /// Initializes package returning the SpecialServicesRequested for Package
        /// </summary>
        private PackageSpecialServicesRequested InitializePackageRequest(RequestedPackageLineItem requestedPackageLineItem)
        {
            if (requestedPackageLineItem.SpecialServicesRequested == null)
            {
                requestedPackageLineItem.SpecialServicesRequested = new PackageSpecialServicesRequested();
            }

            return requestedPackageLineItem.SpecialServicesRequested;
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
        /// Initializes nativeRequest ensuring CarrierRequest is a IFedExNativeShipmentRequest and has
        /// required object initialized
        /// </summary>
        private IFedExNativeShipmentRequest InitializeShipmentRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a IFedExNativeShipmentRequest
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            //Make sure the RequestedShipment is there
            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }

            if (nativeRequest.RequestedShipment.RequestedPackageLineItems == null)
            {
                //Make sure the line item object is are there
                nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
            }

            return nativeRequest;
        }
    }
}
