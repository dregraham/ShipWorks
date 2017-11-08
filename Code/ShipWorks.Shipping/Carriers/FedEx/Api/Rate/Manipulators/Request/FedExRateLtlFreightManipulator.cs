using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
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

            if (!ShouldApply(shipment, FedExRateRequestOptions.LtlFreight))
            {
                return request;
            }

            Validate(request, shipment);

            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            IFedExAccountEntity account = settings.GetAccountReadOnly(shipment);

            // Add the LTL freight detail
            CreateFedExLtlFreightDetailManipulations(request.RequestedShipment, shipment.FedEx, account);

            // Use the FedEx account and shipment to create the shipping charges payment
            ConfigureShippingCharges(request.RequestedShipment.ShippingChargesPayment, shipment.FedEx, account);

            return request;
        }

        /// <summary>
        /// Does this manipulator apply to the shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options)
        {
            return options.HasFlag(FedExRateRequestOptions.LtlFreight);
        }

        /// <summary>
        /// Add options for LTL freight
        /// </summary>
        private void CreateFedExLtlFreightDetailManipulations(RequestedShipment requestedShipment, IFedExShipmentEntity fedex, IFedExAccountEntity account)
        {
            FreightShipmentRoleType? role = EnumHelper.GetApiValue<FreightShipmentRoleType>(fedex.FreightRole);
            FreightClassType? freightClass = EnumHelper.GetApiValue<FreightClassType>(fedex.FreightClass);
            FreightCollectTermsType? collectTerms = EnumHelper.GetApiValue<FreightCollectTermsType>(fedex.FreightCollectTerms);
            FreightShipmentDetail freightDetail = requestedShipment.FreightShipmentDetail;

            freightDetail.FedExFreightAccountNumber = account.AccountNumber;
            freightDetail.FedExFreightBillingContactAndAddress = new ContactAndAddress
            {
                Address = new Address
                {
                    StreetLines = new string[] { account.Street1, account.Street2 },
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
        /// Configures the shipping charges based on the FedEx shipment/account settings.
        /// </summary>
        private void ConfigureShippingCharges(Payment shippingCharges, IFedExShipmentEntity fedExShipment, IFedExAccountEntity account)
        {
            shippingCharges.PaymentType = PaymentType.SENDER;

            InitializePayor(shippingCharges);

            shippingCharges.Payor.ResponsibleParty.Contact = new Contact();
            shippingCharges.Payor.ResponsibleParty.Contact.PersonName = account.FirstName + " " + account.LastName;

            shippingCharges.Payor.ResponsibleParty.AccountNumber = account.AccountNumber;
            shippingCharges.Payor.ResponsibleParty.Address = new Address
            {
                StreetLines = new string[] { account.Street1, account.Street2 },
                City = account.City,
                StateOrProvinceCode = account.StateProvCode,
                PostalCode = account.PostalCode,
                CountryCode = account.CountryCode
            };
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
                throw new FedExException("request.ShipmentEntity is null");
            }

            if (shipment.FedEx == null)
            {
                throw new FedExException("request.ShipmentEntity.FedEx is null");
            }

            IFedExShipmentEntity fedex = shipment.FedEx;

            FreightShipmentRoleType? role = EnumHelper.GetApiValue<FreightShipmentRoleType>(fedex.FreightRole);
            if (role == null)
            {
                throw new FedExException($"FedEx Freight Role is required.");
            }

            FreightClassType? freightClass = EnumHelper.GetApiValue<FreightClassType>(fedex.FreightClass);
            if (freightClass == null)
            {
                throw new FedExException($"FedEx Freight Class is required.");
            }

            foreach (IFedExPackageEntity package in fedex.Packages)
            {
                PhysicalPackagingType? packagingType = EnumHelper.GetApiValue<PhysicalPackagingType>(package.FreightPackaging);
                if (packagingType == null)
                {
                    throw new FedExException($"FedEx Freight Packaging Type is required.");
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
        /// Initializes the payor.
        /// </summary>
        /// <param name="shippingCharges"></param>
        private void InitializePayor(Payment shippingCharges)
        {
            if (shippingCharges.Payor == null)
            {
                // We'll be manipulating properties of the payor, so make sure it's been created
                shippingCharges.Payor = new Payor();
            }

            if (shippingCharges.Payor.ResponsibleParty == null)
            {
                // We'll be manipulating properties of the responsible party, so make sure it's been created
                shippingCharges.Payor.ResponsibleParty = new Party();
            }

            if (shippingCharges.Payor.ResponsibleParty.Address == null)
            {
                // We'll be manipulating properties of the address, so make sure it's been created
                shippingCharges.Payor.ResponsibleParty.Address = new Address();
            }
        }
    }
}
