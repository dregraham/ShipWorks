using System;
using System.Collections.Generic;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator for manipulating COD related
    /// fields of the FedEx RateRequest object.
    /// </summary>
    public class FedExRateCodOptionsManipulator : FedExShippingRequestManipulatorBase
    {
        private int currentPackageIndex;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCodOptionsManipulator" /> class.
        /// </summary>
        public FedExRateCodOptionsManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCodOptionsManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExRateCodOptionsManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCodOptionsManipulator" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExRateCodOptionsManipulator(ICarrierSettingsRepository settingsRepository)
            : base(settingsRepository)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            RateRequest nativeRequest = request.NativeRequest as RateRequest;
            
            ShipmentEntity shipmentEntity = request.ShipmentEntity;

            if (shipmentEntity.FedEx.CodEnabled)
            {
                // Build the COD detail from the shipment
                CodDetail codDetail = GetCodDetail(shipmentEntity);
                
                // Determine which currency to use based on the recipient's country code (for certification purposes)
                string currency = EnumHelper.GetApiValue(ShipmentType.GetCurrencyForCountryCode(shipmentEntity.AdjustedShipCountryCode()));
                
                if (shipmentEntity.FedEx.Service == (int)FedExServiceType.FedExGround || shipmentEntity.FedEx.Service == (int)FedExServiceType.GroundHomeDelivery)
                {
                    // Ground services require configuring the COD settings at the package level
                    ConfigurePackageDetails(nativeRequest, shipmentEntity, codDetail, currency);
                }
                else
                {
                    // Services other than ground require configuring the COD settings at the shipment level
                    ConfigureShipmentDetails(nativeRequest, shipmentEntity, codDetail, currency);
                }
            }
        }

        /// <summary>
        /// Builds the general COD detail that is common to both shipment level COD settings and package level COD settings 
        /// based on the shipment entity.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>A CodDetail object.</returns>
        private CodDetail GetCodDetail(ShipmentEntity shipmentEntity)
        {
            CodDetail codDetail = new CodDetail();
            codDetail.CollectionType = GetApiCodCollectionType((FedExCodPaymentType) shipmentEntity.FedEx.CodPaymentType);
            codDetail.CollectionTypeSpecified = true;

            // Add Freight
            if (shipmentEntity.FedEx.CodAddFreight)
            {
                RateTypeBasisType rateType = SettingsRepository.UseListRates ? RateTypeBasisType.LIST : RateTypeBasisType.ACCOUNT;
                
                codDetail.AddTransportationChargesDetail = new CodAddTransportationChargesDetail()
                { 
                    ChargeBasis = (CodAddTransportationChargeBasisType) shipmentEntity.FedEx.CodChargeBasis,
                    ChargeBasisSpecified = true,
                    RateTypeBasis = rateType,
                    RateTypeBasisSpecified = true,
                    ChargeBasisLevel = ChargeBasisLevelType.CURRENT_PACKAGE,
                    ChargeBasisLevelSpecified = true
                };
            }

            // Recipient
            codDetail.CodRecipient = new Party();
            codDetail.CodRecipient.Address = FedExRequestManipulatorUtilities.CreateAddress<Address>(new PersonAdapter(shipmentEntity.FedEx, "Cod"));
            codDetail.CodRecipient.Contact = FedExRequestManipulatorUtilities.CreateContact<Contact>(new PersonAdapter(shipmentEntity.FedEx, "Cod"));
            
            if (!string.IsNullOrEmpty(shipmentEntity.FedEx.CodAccountNumber))
            {
                // This was added solely for certification tests and not something that can
                // currently be set in the UI
                codDetail.CodRecipient.AccountNumber = shipmentEntity.FedEx.CodAccountNumber;                
            }


            if (!string.IsNullOrEmpty(shipmentEntity.FedEx.CodTIN))
            {
                // Add the tax information if it's provided
                codDetail.CodRecipient.Tins = new TaxpayerIdentification[] {new TaxpayerIdentification() {Number = shipmentEntity.FedEx.CodTIN, TinType = TinType.PERSONAL_STATE}};                
            }

            // If this is the last shipment of an MPS, we need to put the return tracking id, and it exists (in Ground it doesnt)
            if (shipmentEntity.FedEx.Packages.Count > 1 && currentPackageIndex == shipmentEntity.FedEx.Packages.Count - 1 && !string.IsNullOrEmpty(shipmentEntity.FedEx.CodTrackingNumber))
            {
                codDetail.ReturnTrackingId = new TrackingId();
                codDetail.ReturnTrackingId.TrackingNumber = shipmentEntity.FedEx.CodTrackingNumber;
                codDetail.ReturnTrackingId.FormId = shipmentEntity.FedEx.CodTrackingFormID;                
            }

            // This is all that is required in FedEx tests.
            codDetail.ReferenceIndicator = CodReturnReferenceIndicatorType.INVOICE;
            codDetail.ReferenceIndicatorSpecified = true;

            codDetail.CodRecipient.Address.Residential = false;
            codDetail.CodRecipient.Address.ResidentialSpecified = true;

            return codDetail;
        }

        /// <summary>
        /// Configures the COD details at the shipment level.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        /// <param name="codDetail">The cod detail.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        private void ConfigureShipmentDetails(RateRequest nativeRequest, ShipmentEntity shipmentEntity, CodDetail codDetail, string currency)
        {
            if (nativeRequest.RequestedShipment.SpecialServicesRequested == null)
            {
                // The COD Detail hangs off the special services
                nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            if (nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail == null)
            {
                // This is where all the details describing the COD for the overall shipment will get added
                nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail = new CodDetail();
            }

            if (nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes == null)
            {
                nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            }

            // Amount is at the shipment level for all service types other than ground
            codDetail.CodCollectionAmount = new Money {Currency = currency, Amount = shipmentEntity.FedEx.CodAmount};
            nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail = codDetail;

            // Add the COD special service to the shipment's array of special services requested
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
            serviceTypes.Add(ShipmentSpecialServiceType.COD);
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = serviceTypes.ToArray();
        }

        /// <summary>
        /// Configures the package details with the COD information.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="codDetail">The cod detail.</param>
        /// <param name="currency">The currency.</param>
        private void ConfigurePackageDetails(RateRequest nativeRequest, ShipmentEntity shipmentEntity, CodDetail codDetail, string currency)
        {
            // Get a handle to first line item and initialize any values we'll be manipulating
            RequestedPackageLineItem packageLineItem = nativeRequest.RequestedShipment.RequestedPackageLineItems[0];
            if (packageLineItem.SpecialServicesRequested == null)
            {
                // The COD Detail hangs off the special services
                packageLineItem.SpecialServicesRequested = new PackageSpecialServicesRequested();
            }

            if (packageLineItem.SpecialServicesRequested.CodDetail == null)
            {
                // This is where all the details describing the COD package/shipment will get added
                packageLineItem.SpecialServicesRequested.CodDetail = new CodDetail();
            }

            if (packageLineItem.SpecialServicesRequested.SpecialServiceTypes == null)
            {
                packageLineItem.SpecialServicesRequested.SpecialServiceTypes = new PackageSpecialServiceType[0];
            }

            // Amount is calculated at the package level for ground service
            // We're going to distribute the total COD amount evenly across all of the packages
            decimal amount = shipmentEntity.FedEx.CodAmount / shipmentEntity.FedEx.Packages.Count;
            codDetail.CodCollectionAmount = new Money
            {
                Amount = amount,
                AmountSpecified = true,
                Currency = currency
            };

            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail = codDetail;

            // Add the COD special service to the package's array of speical services requested
            List<PackageSpecialServiceType> serviceTypes = new List<PackageSpecialServiceType>(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes);
            serviceTypes.Add(PackageSpecialServiceType.COD);
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes = serviceTypes.ToArray();
        }


        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // We'll potentially be adding COD at the package level, so initialize the package index
            currentPackageIndex = request.SequenceNumber;

            // The native FedEx request type should be a RateRequest
            RateRequest nativeRequest = request.NativeRequest as RateRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            // Make sure the RequestedShipment is there
            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }
            
            // Package initialization - make sure at least one package is on the request
            if (nativeRequest.RequestedShipment.RequestedPackageLineItems == null || nativeRequest.RequestedShipment.RequestedPackageLineItems.Length == 0)
            {
                nativeRequest.RequestedShipment.RequestedPackageLineItems = new[]
                {
                    new RequestedPackageLineItem()
                };
            }
        }

        /// <summary>
        /// Determine the API value to use for our COD payment type
        /// </summary>
        private CodCollectionType GetApiCodCollectionType(FedExCodPaymentType paymentType)
        {
            switch (paymentType)
            {
                case FedExCodPaymentType.Any: return CodCollectionType.ANY;
                case FedExCodPaymentType.Secured: return CodCollectionType.GUARANTEED_FUNDS;
                case FedExCodPaymentType.Unsecured: return CodCollectionType.CASH;
            }

            throw new InvalidOperationException("Invalid FedEx payment type: " + paymentType);
        }
    }
}
