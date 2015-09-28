using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the package special services values of the
    /// FedEx API's RateRequest object.
    /// </summary>
    public class FedExRatePackageSpecialServicesManipulator : ICarrierRequestManipulator
    {
        /// <summary>
        /// Adds SpecialServices to the FedEx request object's packages
        /// </summary>
        /// <param name="request"></param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization 
            RateRequest nativeRequest = request.NativeRequest as RateRequest;

            FedExShipmentEntity fedex = request.ShipmentEntity.FedEx;

            // All of the package details are sent to to FedEx in a single call instead of doing smultiple
            // calls like the processing a shipment does
            for (int packageIndex = 0; packageIndex < fedex.Packages.Count; packageIndex++)
            {
                InitializeLineItem(nativeRequest, packageIndex);
                PackageSpecialServicesRequested specialServicesRequested = InitializePackageRequest(nativeRequest.RequestedShipment.RequestedPackageLineItems[packageIndex]);

                // Start a new special services list for this package
                List<PackageSpecialServiceType> specialServices = new List<PackageSpecialServiceType>();

                // Signature
                FedExSignatureType fedExSignatureType = (FedExSignatureType) fedex.Signature;
                if (fedExSignatureType == FedExSignatureType.NoSignature && fedex.Packages.Any(p => p.DeclaredValue > 500m))
                {
                    // The FedEx API allows for this to go through, but per page 170 of the FedExDeveloperGuide2012.pdf document, 
                    // a signature is required for shipments with a declared value > $500
                    throw new FedExException("A signature is required for shipments containing packages with a declared value over $500.");
                }

                if (fedExSignatureType != FedExSignatureType.ServiceDefault)
                {
                    FedExAccountEntity fedExAccount = request.CarrierAccountEntity as FedExAccountEntity;
                    specialServicesRequested.SignatureOptionDetail = new SignatureOptionDetail
                    {
                        OptionType = GetApiSignatureType(fedExSignatureType),
                        OptionTypeSpecified = true,
                        SignatureReleaseNumber = fedExAccount.SignatureRelease
                    };

                    specialServices.Add(PackageSpecialServiceType.SIGNATURE_OPTION);
                }

                ServiceType apiServiceType = GetApiServiceType((FedExServiceType) fedex.Service);

                // Non-standard container (only applies to Ground services)
                if (fedex.NonStandardContainer && (apiServiceType == ServiceType.GROUND_HOME_DELIVERY || apiServiceType == ServiceType.FEDEX_GROUND))
                {
                    specialServices.Add(PackageSpecialServiceType.NON_STANDARD_CONTAINER);
                }


                if (fedex.Packages[packageIndex].ContainsAlcohol)
                {
                    specialServices.Add(PackageSpecialServiceType.ALCOHOL);
                }

                // Set the special service type flags
                specialServicesRequested.SpecialServiceTypes = specialServices.ToArray();
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
        /// Initializes package returning the SpecialServicesRequested for Package
        /// </summary>
        private static PackageSpecialServicesRequested InitializePackageRequest(RequestedPackageLineItem requestedPackageLineItem)
        {
            if (requestedPackageLineItem.SpecialServicesRequested == null)
            {
                requestedPackageLineItem.SpecialServicesRequested = new PackageSpecialServicesRequested();
            }

            return requestedPackageLineItem.SpecialServicesRequested;
        }

        /// <summary>
        /// Gets the type of the API service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Invalid FedEx ServiceType  + serviceType</exception>
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
        }
    }
}
