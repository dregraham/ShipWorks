using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator for manipulating COD related
    /// fields of the FedEx IFedExNativeShipmentRequest object.
    /// </summary>
    public class FedExCodOptionsManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCodOptionsManipulator" /> class.
        /// </summary>
        public FedExCodOptionsManipulator(IFedExSettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment) =>
            shipment.FedEx.CodEnabled;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            return GetCodDetail(shipment, sequenceNumber)
                .Map(codDetail =>
                {
                    // Determine which currency to use based on the recipient's country code (for certification purposes)
                    string currency = EnumHelper.GetApiValue(ShipmentType.GetCurrencyForCountryCode(shipment.AdjustedShipCountryCode()));

                    if (FedExUtility.IsGroundService((FedExServiceType) shipment.FedEx.Service))
                    {
                        // Ground services require configuring the COD settings at the package level
                        ConfigurePackageDetails(request, shipment, codDetail, currency);
                    }
                    else
                    {
                        // Services other than ground require configuring the COD settings at the shipment level
                        ConfigureShipmentDetails(request, shipment, codDetail, currency);
                    }

                    return request;
                });
        }

        /// <summary>
        /// Builds the general COD detail that is common to both shipment level COD settings and package level COD settings 
        /// based on the shipment entity.
        /// </summary>
        /// <param name="shipment">The shipment entity.</param>
        /// <returns>A CodDetail object.</returns>
        private GenericResult<CodDetail> GetCodDetail(IShipmentEntity shipment, int currentPackageIndex)
        {
            return GetApiCodCollectionType((FedExCodPaymentType) shipment.FedEx.CodPaymentType)
                .Map(x => new CodDetail
                {
                    CollectionType = x
                })
                .Map(x => PopulateCodDetail(shipment, currentPackageIndex, x));
        }

        private CodDetail PopulateCodDetail(IShipmentEntity shipment, int currentPackageIndex, CodDetail codDetail)
        {
            // Add Freight
            if (shipment.FedEx.CodAddFreight)
            {
                RateTypeBasisType rateType = settingsRepository.UseListRates ? RateTypeBasisType.LIST : RateTypeBasisType.ACCOUNT;

                codDetail.AddTransportationChargesDetail = new CodAddTransportationChargesDetail()
                {
                    ChargeBasis = (CodAddTransportationChargeBasisType) shipment.FedEx.CodChargeBasis,
                    ChargeBasisSpecified = true,
                    RateTypeBasis = rateType,
                    RateTypeBasisSpecified = true,
                    ChargeBasisLevel = ChargeBasisLevelType.CURRENT_PACKAGE,
                    ChargeBasisLevelSpecified = true
                };
            }

            // Recipient
            codDetail.CodRecipient = new Party();
            codDetail.CodRecipient.Address = FedExRequestManipulatorUtilities.CreateAddress<Address>(shipment.FedEx.CodPerson);
            codDetail.CodRecipient.Contact = FedExRequestManipulatorUtilities.CreateContact<Contact>(shipment.FedEx.CodPerson);

            if (!string.IsNullOrEmpty(shipment.FedEx.CodAccountNumber))
            {
                // This was added solely for certification tests and not something that can
                // currently be set in the UI
                codDetail.CodRecipient.AccountNumber = shipment.FedEx.CodAccountNumber;
            }

            if (!string.IsNullOrEmpty(shipment.FedEx.CodTIN))
            {
                // Add the tax information if it's provided
                codDetail.CodRecipient.Tins = new TaxpayerIdentification[] { new TaxpayerIdentification() { Number = shipment.FedEx.CodTIN, TinType = TinType.PERSONAL_STATE } };
            }

            // If this is the last shipment of an MPS, we need to put the return tracking id, and it exists (in Ground it doesn't)
            if (shipment.FedEx.Packages.Count() > 1 &&
                currentPackageIndex == shipment.FedEx.Packages.Count() - 1 &&
                !string.IsNullOrEmpty(shipment.FedEx.CodTrackingNumber))
            {
                codDetail.ReturnTrackingId = new TrackingId();
                codDetail.ReturnTrackingId.TrackingNumber = shipment.FedEx.CodTrackingNumber;
                codDetail.ReturnTrackingId.FormId = shipment.FedEx.CodTrackingFormID;
            }

            // This is all that is required in FedEx tests.
            codDetail.ReferenceIndicator = CodReturnReferenceIndicatorType.INVOICE;
            codDetail.CodRecipient.Address.Residential = false;

            return codDetail;
        }

        /// <summary>
        /// Configures the COD details at the shipment level.
        /// </summary>
        /// <param name="request">The native request.</param>
        /// <param name="codDetail">The cod detail.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="shipment">The shipment entity.</param>
        private void ConfigureShipmentDetails(ProcessShipmentRequest request, IShipmentEntity shipment, CodDetail codDetail, string currency)
        {
            request.RequestedShipment
                .Ensure(x => x.SpecialServicesRequested)
                .Ensure(x => x.SpecialServiceTypes);

            // Amount is at the shipment level for all service types other than ground
            codDetail.CodCollectionAmount = new Money { Currency = currency, Amount = shipment.FedEx.CodAmount };
            request.RequestedShipment.SpecialServicesRequested.CodDetail = codDetail;

            request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes =
                request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes
                    .Append(ShipmentSpecialServiceType.COD)
                    .ToArray();
        }

        /// <summary>
        /// Configures the package details with the COD information.
        /// </summary>
        /// <param name="request">The native request.</param>
        /// <param name="shipment">The shipment entity.</param>
        /// <param name="codDetail">The cod detail.</param>
        /// <param name="currency">The currency.</param>
        private void ConfigurePackageDetails(IFedExNativeShipmentRequest request, IShipmentEntity shipment, CodDetail codDetail, string currency)
        {
            // Get a handle to first line item and initialize any values we'll be manipulating
            RequestedPackageLineItem packageLineItem = request.RequestedShipment.RequestedPackageLineItems[0];
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
            decimal amount = shipment.FedEx.CodAmount / shipment.FedEx.Packages.Count();
            codDetail.CodCollectionAmount = new Money
            {
                Amount = amount,
                Currency = currency
            };

            request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail = codDetail;

            // Add the COD special service to the package's array of special services requested
            List<PackageSpecialServiceType> serviceTypes = new List<PackageSpecialServiceType>(request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes);
            serviceTypes.Add(PackageSpecialServiceType.COD);
            request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes = serviceTypes.ToArray();
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(ProcessShipmentRequest request) =>
            request.Ensure(x => x.RequestedShipment).EnsureAtLeastOne(x => x.RequestedPackageLineItems);

        /// <summary>
        /// Determine the API value to use for our COD payment type
        /// </summary>
        private GenericResult<CodCollectionType> GetApiCodCollectionType(FedExCodPaymentType paymentType)
        {
            switch (paymentType)
            {
                case FedExCodPaymentType.Any: return CodCollectionType.ANY;
                case FedExCodPaymentType.Secured: return CodCollectionType.GUARANTEED_FUNDS;
                case FedExCodPaymentType.Unsecured: return CodCollectionType.CASH;
            }

            return new InvalidOperationException("Invalid FedEx payment type: " + paymentType);
        }
    }
}
