using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.FedEx;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// Add LTL Freight information to shipment
    /// </summary>
    public class FedExLtlFreightManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExLtlFreightManipulator" /> class.
        /// </summary>
        public FedExLtlFreightManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            IFedExAccountEntity account = settings.GetAccountReadOnly(shipment);

            return Validate(request, shipment)
                .Map(InitializeRequest)
                .Bind(x => CreateFedExLtlFreightDetailManipulations(x, shipment.FedEx, account, sequenceNumber))
                .Map(x => ConfigureShippingCharges(x, shipment.FedEx, account));
        }

        /// <summary>
        /// Does this manipulator apply to the shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) =>
            FedExUtility.IsFreightLtlService(shipment.FedEx.Service);

        /// <summary>
        /// Add options for LTL freight
        /// </summary>
        private GenericResult<ProcessShipmentRequest> CreateFedExLtlFreightDetailManipulations(ProcessShipmentRequest request, IFedExShipmentEntity fedex, IFedExAccountEntity account, int sequenceNumber)
        {
            var requestedShipment = request.RequestedShipment;

            requestedShipment.PackageCount = fedex.Packages.Count().ToString();
            requestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes =
                requestedShipment.ShippingDocumentSpecification
                    .ShippingDocumentTypes
                    .Append(RequestedShippingDocumentType.FREIGHT_ADDRESS_LABEL)
                    .ToArray();

            requestedShipment.ShippingDocumentSpecification
                .Ensure(x => x.FreightAddressLabelDetail)
                .Format = new ShippingDocumentFormat
                {
                    ImageType = ShippingDocumentImageType.PNG,
                    ImageTypeSpecified = true,
                    StockType = ShippingDocumentStockType.PAPER_4X6,
                    StockTypeSpecified = true,
                    ProvideInstructions = true,
                    ProvideInstructionsSpecified = true
                };

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

            return GetFreightSpecialServices(fedex.FreightSpecialServices)
                .Do(x => AddSpecialServices(fedex, x, requestedShipment))
                .Map(x => request);
        }

        /// <summary>
        /// Add special services to the request
        /// </summary>
        private static void AddSpecialServices(IFedExShipmentEntity fedex, IEnumerable<ShipmentSpecialServiceType> serviceTypes, RequestedShipment requestedShipment)
        {
            if (serviceTypes.None())
            {
                return;
            }

            requestedShipment.SpecialServicesRequested.SpecialServiceTypes = serviceTypes.ToArray();

            if (serviceTypes.Contains(ShipmentSpecialServiceType.FREIGHT_GUARANTEE) &&
                fedex.FreightGuaranteeType != FedExFreightGuaranteeType.None)
            {
                requestedShipment.SpecialServicesRequested.FreightGuaranteeDetail = new FreightGuaranteeDetail
                {
                    Date = fedex.FreightGuaranteeDate,
                    DateSpecified = true,
                    Type = fedex.FreightGuaranteeType == FedExFreightGuaranteeType.Date ?
                        FreightGuaranteeType.GUARANTEED_DATE :
                        FreightGuaranteeType.GUARANTEED_MORNING,
                    TypeSpecified = true,
                };
            }
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
                    },
                    Packaging = packagingType.Value,
                    PackagingSpecified = true,
                    Pieces = package.FreightPieces.ToString(),
                    Weight = new Weight
                    {
                        Value = (decimal) package.Weight,
                        Units = WeightUnits.LB,
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
        private GenericResult<IEnumerable<ShipmentSpecialServiceType>> GetFreightSpecialServices(int freightSpecialServices) =>
            EnumHelper.GetEnumList<FedExFreightSpecialServicesType>()
                .Select(x => x.Value)
                .Where(x => (freightSpecialServices & (int) x) != 0)
                .Select(x => EnumHelper.GetApiValue<ShipmentSpecialServiceType>(x))
                .Select(x => x.Match(
                    v => GenericResult.FromSuccess(v),
                    () => new CarrierException($"An invalid FedEx Freight special service was selected.")))
                .Flatten();

        /// <summary>
        /// Configures the shipping charges based on the FedEx shipment/account settings.
        /// </summary>
        private ProcessShipmentRequest ConfigureShippingCharges(ProcessShipmentRequest request, IFedExShipmentEntity fedExShipment, IFedExAccountEntity account)
        {
            var shippingCharges = request.RequestedShipment.ShippingChargesPayment;

            shippingCharges.PaymentType = PaymentType.SENDER;

            var responsibleParty = shippingCharges.Ensure(x => x.Payor).Ensure(x => x.ResponsibleParty);
            responsibleParty.Ensure(x => x.Contact).PersonName = account.FirstName + " " + account.LastName;

            responsibleParty.AccountNumber = account.AccountNumber;
            responsibleParty.Address = new Address
            {
                StreetLines = new string[] { account.Street1, account.Street2 },
                City = account.City,
                StateOrProvinceCode = account.StateProvCode,
                PostalCode = account.PostalCode,
                CountryCode = account.CountryCode
            };

            return request;
        }

        /// <summary>
        /// Validates the specified request.
        /// </summary>
        private static GenericResult<ProcessShipmentRequest> Validate(ProcessShipmentRequest request, IShipmentEntity shipment)
        {
            IFedExShipmentEntity fedex = shipment.FedEx;

            FreightShipmentRoleType? role = EnumHelper.GetApiValue<FreightShipmentRoleType>(fedex.FreightRole);
            if (role == null)
            {
                return new FedExException($"FedEx Freight Role is required.");
            }

            FreightClassType? freightClass = EnumHelper.GetApiValue<FreightClassType>(fedex.FreightClass);
            if (freightClass == null)
            {
                return new FedExException($"FedEx Freight Class is required.");
            }

            if (!fedex.Packages.All(p => EnumHelper.GetApiValue<PhysicalPackagingType>(p.FreightPackaging).HasValue))
            {
                return new FedExException($"FedEx Freight Packaging Type is required.");
            }

            return request;
        }

        /// <summary>
        /// Initialize the request properties needed for freight
        /// </summary>
        private ProcessShipmentRequest InitializeRequest(ProcessShipmentRequest request)
        {
            var requestedShipment = request.Ensure(x => x.RequestedShipment);
            requestedShipment.Ensure(x => x.FreightShipmentDetail);
            requestedShipment.Ensure(x => x.ShippingChargesPayment);
            requestedShipment.Ensure(x => x.ShippingDocumentSpecification).Ensure(x => x.ShippingDocumentTypes);
            requestedShipment.Ensure(x => x.SpecialServicesRequested).Ensure(x => x.SpecialServiceTypes);

            // LTL Freight requires RequestedPackageLineItems and ExpressFreightDetail to be null
            requestedShipment.RequestedPackageLineItems = null;
            requestedShipment.ExpressFreightDetail = null;

            return request;
        }
    }
}
