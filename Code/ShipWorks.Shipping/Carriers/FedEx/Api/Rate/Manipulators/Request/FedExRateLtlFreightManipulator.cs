using System;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.FedEx;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// Add LTL Freight information to shipment
    /// </summary>
    public class FedExRateLtlFreightManipulator : IFedExRateRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;
        private IShipmentEntity shipment;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRateLtlFreightManipulator" /> class.
        /// </summary>
        public FedExRateLtlFreightManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            this.shipment = shipment;

            if (!ShouldApply(shipment, FedExRateRequestOptions.None))
            {
                return request;
            }

            Validate(request, shipment);

            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // Add the LTL freight detail
            CreateFedExLtlFreightDetailManipulations(request.RequestedShipment, shipment.FedEx);

            return request;
        }

        /// <summary>
        /// Does this manipulator apply to the shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options)
        {
            return FedExUtility.IsFreightLtlService(shipment.FedEx.Service);
        }

        /// <summary>
        /// Add options for LTL freight
        /// </summary>
        private void CreateFedExLtlFreightDetailManipulations(RequestedShipment requestedShipment, IFedExShipmentEntity fedex)
        {
            FreightShipmentRoleType? role = EnumHelper.GetApiValue<FreightShipmentRoleType>(fedex.FreightRole);
            FreightClassType? freightClass = EnumHelper.GetApiValue<FreightClassType>(fedex.FreightClass);
            FreightCollectTermsType? collectTerms = EnumHelper.GetApiValue<FreightCollectTermsType>(fedex.FreightCollectTerms);
            FreightShipmentDetail freightDetail = requestedShipment.FreightShipmentDetail;
            IFedExAccountEntity account = settings.GetAccountReadOnly(shipment);

            freightDetail.FedExFreightAccountNumber = account.AccountNumber;
            freightDetail.FedExFreightBillingContactAndAddress = new ContactAndAddress
            {
                Address = new Address
                {
                    StreetLines = new string[] {account.Street1, account.Street2},
                    City = account.City,
                    StateOrProvinceCode = account.StateProvCode,
                    PostalCode = account.PostalCode,
                    CountryCode = account.CountryCode
                }
            };

            freightDetail.Role = role.Value;
            freightDetail.RoleSpecified = true;

            if (collectTerms.HasValue)
            {
                freightDetail.CollectTermsType = collectTerms.Value;
                freightDetail.CollectTermsTypeSpecified = true;
            }

            freightDetail.TotalHandlingUnits = fedex.FreightTotalHandlinUnits.ToString();

            AddLineItems(fedex, freightClass, freightDetail);

            // Add shipping document types 
            AddShippingDocumentTypes(requestedShipment);

            // Set the special service types on the requested shipment
            List<ShipmentSpecialServiceType> specialServiceTypes = GetFreightSpecialServices(fedex.FreightSpecialServices);
            specialServiceTypes.AddRange(requestedShipment.SpecialServicesRequested.SpecialServiceTypes);
            requestedShipment.SpecialServicesRequested.SpecialServiceTypes = specialServiceTypes.ToArray();
        }

        /// <summary>
        /// Add line items to the request
        /// </summary>
        private static void AddLineItems(IFedExShipmentEntity fedex, FreightClassType? freightClass, FreightShipmentDetail freightDetail)
        {
            List<FreightShipmentLineItem> lineItems = new List<FreightShipmentLineItem>();
            int packageIndex = 1;
            foreach (IFedExPackageEntity package in fedex.Packages)
            {
                PhysicalPackagingType? packagingType = EnumHelper.GetApiValue<PhysicalPackagingType>(package.FreightPackaging);
                if (packagingType == null)
                {
                    throw new CarrierException($"FedEx Freight Packaging Type is required.");
                }

                FreightShipmentLineItem lineItem = new FreightShipmentLineItem()
                {
                    Description = $"Freight Package {packageIndex}",
                    FreightClass = freightClass.Value,
                    FreightClassSpecified = true,
                    Dimensions = new Dimensions
                    {
                        Height = package.DimsHeight.ToString(),
                        Length = package.DimsLength.ToString(),
                        Width = package.DimsWidth.ToString(),
                        Units = LinearUnits.IN,
                        UnitsSpecified = true
                    },
                    Packaging = packagingType.Value,
                    PackagingSpecified = true,
                    Pieces = package.FreightPieces.ToString(),
                    Weight = new Weight
                    {
                        Value = (decimal) package.Weight,
                        ValueSpecified = true,
                        Units = WeightUnits.LB,
                        UnitsSpecified = true
                    }
                };

                lineItems.Add(lineItem);
                packageIndex++;
            }

            freightDetail.LineItems = lineItems.ToArray();
        }

        /// <summary>
        /// Get the list of freight special service types
        /// </summary>
        private List<ShipmentSpecialServiceType> GetFreightSpecialServices(int freightSpecialServices)
        {
            List<ShipmentSpecialServiceType> specialServices = new List<ShipmentSpecialServiceType>();

            foreach (var specialServiceTypeEntry in EnumHelper.GetEnumList<FedExFreightSpecialServicesType>())
            {
                FedExFreightSpecialServicesType specialServiceType = specialServiceTypeEntry.Value;

                if ((freightSpecialServices & (int) specialServiceType) != 0)
                {
                    ShipmentSpecialServiceType? shipmentSpecialServiceType = EnumHelper.GetApiValue<ShipmentSpecialServiceType>(specialServiceType);
                    if (shipmentSpecialServiceType == null)
                    {
                        throw new CarrierException($"An invalid FedEx Freight special service was selected.");
                    }

                    specialServices.Add(shipmentSpecialServiceType.Value);
                }
            }

            return specialServices;
        }

        /// <summary>
        /// Validates the specified request.
        /// </summary>
        private static void Validate(RateRequest request, IShipmentEntity shipment)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (shipment == null)
            {
                throw new CarrierException("request.ShipmentEntity is null");
            }

            if (shipment.FedEx == null)
            {
                throw new CarrierException("request.ShipmentEntity.FedEx is null");
            }

            IFedExShipmentEntity fedex = shipment.FedEx;

            FreightShipmentRoleType? role = EnumHelper.GetApiValue<FreightShipmentRoleType>(fedex.FreightRole);
            if (role == null)
            {
                throw new CarrierException($"FedEx Freight Role is required.");
            }

            FreightClassType? freightClass = EnumHelper.GetApiValue<FreightClassType>(fedex.FreightClass);
            if (freightClass == null)
            {
                throw new CarrierException($"FedEx Freight Class is required.");
            }

            foreach (IFedExPackageEntity package in fedex.Packages)
            {
                PhysicalPackagingType? packagingType = EnumHelper.GetApiValue<PhysicalPackagingType>(package.FreightPackaging);
                if (packagingType == null)
                {
                    throw new CarrierException($"FedEx Freight Packaging Type is required.");
                }
            }
        }

        /// <summary>
        /// Initialize the request properties needed for freight
        /// </summary>
        private void InitializeRequest(RateRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a RateRequest
            if (request == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            // Make sure the RequestedShipment is there
            if (request.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                request.RequestedShipment = new RequestedShipment();
            }

            // LTL Freight requires RequestedPackageLineItems and ExpressFreightDetail to be null
            request.RequestedShipment.RequestedPackageLineItems = null;
            request.RequestedShipment.ExpressFreightDetail = null;

            // Make sure the FreightShipmentDetail is there
            if (request.RequestedShipment.FreightShipmentDetail == null)
            {
                request.RequestedShipment.FreightShipmentDetail = new FreightShipmentDetail();
            }

            // Make sure the ShippingChargesPayment is there
            if (request.RequestedShipment.ShippingChargesPayment == null)
            {
                request.RequestedShipment.ShippingChargesPayment = new Payment();
            }

            // Make sure the SpecialSericesRequested is there
            if (request.RequestedShipment.SpecialServicesRequested == null)
            {
                request.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            // Make sure the SpecialServiceTypes is there
            if (request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes == null)
            {
                request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            }

            // Make sure the ShippingDocumentSpecification is there
            if (request.RequestedShipment.ShippingDocumentSpecification == null)
            {
                request.RequestedShipment.ShippingDocumentSpecification = new ShippingDocumentSpecification();
            }

            // Make sure the ShippingDocumentTypes is there
            if (request.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes == null)
            {
                request.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes = new RequestedShippingDocumentType[0];
            }
        }

        /// <summary>
        /// Adds the label shipping document type if it's not already present.
        /// </summary>
        private static void AddShippingDocumentTypes(RequestedShipment requestedShipment)
        {
            List<RequestedShippingDocumentType> shippingDocumentTypes = new List<RequestedShippingDocumentType>();
            shippingDocumentTypes.AddRange(requestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes);

            if (!shippingDocumentTypes.Contains(RequestedShippingDocumentType.LABEL))
            {
                shippingDocumentTypes.Add(RequestedShippingDocumentType.LABEL);
            }
            
            requestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes = shippingDocumentTypes.ToArray();
        }
    }
}
