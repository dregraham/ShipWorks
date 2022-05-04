using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// DHL eCommerce implementation of shipment type
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.ShipmentType" />
    [Component(RegistrationType.Self)]
    [KeyedComponent(typeof(ShipmentType), ShipmentTypeCode.DhlEcommerce, SingleInstance = true)]
    public class DhlEcommerceShipmentType : ShipmentType
    {
        private readonly ICarrierAccountRepository<DhlEcommerceAccountEntity, IDhlEcommerceAccountEntity> accountRepository;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IShipEngineTrackingResultFactory trackingResultFactory;
        private readonly ILicenseService licenseService;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceShipmentType(ICarrierAccountRepository<DhlEcommerceAccountEntity, IDhlEcommerceAccountEntity> accountRepository,
            IShipEngineWebClient shipEngineWebClient,
            IShipEngineTrackingResultFactory trackingResultFactory,
            ILicenseService licenseService)
        {
            this.accountRepository = accountRepository;
            this.shipEngineWebClient = shipEngineWebClient;
            this.trackingResultFactory = trackingResultFactory;
            this.licenseService = licenseService;
        }

        /// <summary>
        /// The ShipmentTypeCode represented by this ShipmentType
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.DhlEcommerce;

        /// <summary>
        /// Gets a value indicating whether multiple packages are supported by this shipment type.
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports multiple packages]; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsMultiplePackages => false;

        /// <summary>
        /// Supports using an origin address from a shipping account
        /// </summary>
        public override bool SupportsAccountAsOrigin => true;

        /// <summary>
        /// Indicates if the shipment service type supports getting rates
        /// </summary>
        public override bool SupportsGetRates => true;

        /// <summary>
        /// Indicates if the shipment service type supports return shipments
        /// </summary>
        public override bool SupportsReturns => false;

        /// <summary>
        /// Create and Initialize a new shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            if (shipment.DhlEcommerce == null)
            {
                shipment.DhlEcommerce = new DhlEcommerceShipmentEntity(shipment.ShipmentID);
            }

            DhlEcommerceShipmentEntity dhlEcommerceShipmentEntity = shipment.DhlEcommerce;

            dhlEcommerceShipmentEntity.Service = (int) DhlEcommerceServiceType.US_DhlSmartMailParcelGround;
            dhlEcommerceShipmentEntity.PackagingType = (int) DhlEcommercePackagingType.ParcelSelectMachinable;
            dhlEcommerceShipmentEntity.DeliveredDutyPaid = false;
            dhlEcommerceShipmentEntity.NonMachinable = false;
            dhlEcommerceShipmentEntity.SaturdayDelivery = false;
            dhlEcommerceShipmentEntity.RequestedLabelFormat = (int) ThermalLanguage.None;
            dhlEcommerceShipmentEntity.Contents = (int) ShipEngineContentsType.Merchandise;
            dhlEcommerceShipmentEntity.NonDelivery = (int) ShipEngineNonDeliveryType.ReturnToSender;
            dhlEcommerceShipmentEntity.DhlEcommerceAccountID = 0;
            dhlEcommerceShipmentEntity.ShipEngineLabelID = string.Empty;
            dhlEcommerceShipmentEntity.ResidentialDelivery = false;
            dhlEcommerceShipmentEntity.CustomsRecipientTin = string.Empty;
            dhlEcommerceShipmentEntity.CustomsTaxIdType = (int) TaxIdType.Ioss;
            dhlEcommerceShipmentEntity.CustomsTinIssuingAuthority = "US";
            dhlEcommerceShipmentEntity.DimsAddWeight = false;
            dhlEcommerceShipmentEntity.DimsHeight = 0;
            dhlEcommerceShipmentEntity.DimsLength = 0;
            dhlEcommerceShipmentEntity.DimsProfileID = 0;
            dhlEcommerceShipmentEntity.DimsWeight = 0;
            dhlEcommerceShipmentEntity.DimsWidth = 0;
            dhlEcommerceShipmentEntity.Reference1 = string.Empty;
            dhlEcommerceShipmentEntity.Insurance = false;
            dhlEcommerceShipmentEntity.InsuranceValue = 0;
            dhlEcommerceShipmentEntity.InsurancePennyOne = false;

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.DhlEcommerce == null)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }

            return new List<IPackageAdapter>()
            {
                new DhlEcommercePackageAdapter(shipment)
            };
        }

        /// <summary>
        /// Update the dynamic shipment data that could have changed "outside" the known editor
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            // TODO: DHLECommerce Check insurance stuff below.
            //shipment.Insurance = shipment.DhlEcommerce
            //shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;

            shipment.RequestedLabelFormat = shipment.DhlEcommerce.RequestedLabelFormat;
        }

        /// <summary>
        /// Get the shipment common detail for tango
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            ShipmentCommonDetail commonDetail = new ShipmentCommonDetail();

            DhlEcommerceShipmentEntity dhlEcommerceShipmentEntity = shipment.DhlEcommerce;
            DhlEcommerceAccountEntity account = accountRepository.GetAccount(dhlEcommerceShipmentEntity.DhlEcommerceAccountID);

            // TODO: DHLECommerce Check account number
            //commonDetail.OriginAccount = (account == null) ? "" : account.xxxxxx.ToString();
            commonDetail.ServiceType = dhlEcommerceShipmentEntity.Service;

            commonDetail.PackagingType = (int) dhlEcommerceShipmentEntity.PackagingType;
            commonDetail.PackageLength = dhlEcommerceShipmentEntity.DimsLength;
            commonDetail.PackageWidth = dhlEcommerceShipmentEntity.DimsWidth;
            commonDetail.PackageHeight = dhlEcommerceShipmentEntity.DimsHeight;

            return commonDetail;
        }

        /// <summary>
        /// Ensures that the carrier specific data for the shipment, such as the DhlEcommerce data, are loaded for the shipment.  If the data
        /// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
        /// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
        /// had been deleted.
        /// </summary>
        protected override void LoadShipmentDataInternal(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "DhlEcommerce", typeof(DhlEcommerceShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment) =>
            GetServiceDescriptionInternal((DhlEcommerceServiceType) shipment.DhlEcommerce.Service);

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(string serviceCode) =>
            Functional.ParseInt(serviceCode)
                .Match(x => GetServiceDescriptionInternal((DhlEcommerceServiceType) x), _ => "Unknown");

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        private string GetServiceDescriptionInternal(DhlEcommerceServiceType service) =>
            EnumHelper.GetDescription(service);

        /// <summary>
        /// Get the total packages contained by the shipment
        /// </summary>
        public override int GetParcelCount(ShipmentEntity shipment)
        {
            return 1;
        }

        /// <summary>
        /// Get the parcel data that describes details about a particular parcel
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            return new ShipmentParcel(shipment, null,
                new InsuranceChoice(shipment, shipment.DhlEcommerce, shipment.DhlEcommerce, shipment.DhlEcommerce),
                new DimensionsAdapter(shipment.DhlEcommerce))
            {
                TotalWeight = shipment.TotalWeight
            };
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        public override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            long shipperID = accountRepository.AccountsReadOnly.Select(x => x.DhlEcommerceAccountID).FirstOrDefault();

            profile.DhlEcommerce.DhlEcommerceAccountID = shipperID;
            profile.OriginID = (int) ShipmentOriginSource.Account;

            profile.DhlEcommerce.Service = (int) DhlEcommerceServiceType.US_DhlGlobalmailPacketStandard;
            profile.DhlEcommerce.DeliveryDutyPaid = false;
            profile.DhlEcommerce.NonMachinable = false;
            profile.DhlEcommerce.SaturdayDelivery = false;
            profile.DhlEcommerce.Contents = (int) ShipEngineContentsType.Merchandise;
            profile.DhlEcommerce.NonDelivery = (int) ShipEngineNonDeliveryType.ReturnToSender;
            profile.DhlEcommerce.CustomsTaxIdType = (int) TaxIdType.Ioss;
            profile.DhlEcommerce.CustomsRecipientTin = string.Empty;
            profile.DhlEcommerce.CustomsTinIssuingAuthority = "US";
            profile.DhlEcommerce.ResidentialDelivery = false;
            profile.DhlEcommerce.Reference1 = string.Empty;
            profile.DhlEcommerce.PackagingType = (int) DhlEcommercePackagingType.IrregularParcel;
        }

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address and any store specific logic that may impact whether customs
        /// is required (i.e. eBay GSP).
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        protected override bool IsCustomsRequiredByShipment(IShipmentEntity shipment) => shipment.ShipCountryCode != shipment.OriginCountryCode;

        /// <summary>
        /// Gets a ShippingBroker
        /// </summary>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment, IBestRateExcludedAccountRepository bestRateExcludedAccountRepository)
        {
            IEnumerable<long> excludedAccounts = bestRateExcludedAccountRepository.GetAll();

            // TODO: DHLECommerce update for best rate
            //IEnumerable<IDhlEcommerceAccountEntity> nonExcludedAccounts = DhlEcommerceAccountManager.AccountsReadOnly.Where(a => !excludedAccounts.Contains(a.AccountId));

            //if (nonExcludedAccounts.Any())
            //{
            //    return new DhlEcommerceBestRateBroker(this, new DhlEcommerceAccountRepository(), BestRateExcludedAccountRepository.Current);
            //}

            return new NullShippingBroker();
        }

        /// <summary>
        /// Get the dims weight from a shipment, if any
        /// </summary>
        protected override double GetDimsWeight(IShipmentEntity shipment) =>
            shipment.DhlEcommerce?.DimsAddWeight == true ? shipment.DhlEcommerce.DimsWeight : 0;

        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public override void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {
            if (shipment.DhlEcommerce != null)
            {
                shipment.DhlEcommerce.RequestedLabelFormat = (int) requestedLabelFormat;
            }
        }

        /// <summary>
        /// Create the XML input to the XSL engine
        /// </summary>
        public override void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            Lazy<List<TemplateLabelData>> labels = new Lazy<List<TemplateLabelData>>(() => LoadLabelData(shipment));
            container.AddElement("TIN", () => loaded().DhlEcommerce.CustomsRecipientTin);
            // Add the labels content
            container.AddElement(
                "Labels",
                new LabelsOutline(container.Context, shipment, labels, () => ImageFormat.Png),
                ElementOutline.If(() => shipment().Processed));
        }

        /// <summary>
        /// Track the shipment
        /// </summary>
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            try
            {
                string labelID = shipment.DhlEcommerce?.ShipEngineLabelID;
                ShipEngine.DTOs.TrackingInformation trackingInfo;
                if (string.IsNullOrWhiteSpace(labelID))
                {
                    trackingInfo = Task.Run(() =>
                        shipEngineWebClient.Track("dhl_Ecommerce", shipment.TrackingNumber, ApiLogSource.DhlEcommerce)).Result;
                }
                else
                {
                    trackingInfo = Task.Run(() =>
                        shipEngineWebClient.Track(labelID, ApiLogSource.DhlEcommerce)).Result;
                }

                return trackingResultFactory.Create(trackingInfo);
            }
            catch (Exception)
            {
                return new TrackingResult { Summary = $"<a href='http://www.dhl.com/en/Ecommerce/tracking.html?AWB={shipment.TrackingNumber}&brand=DHL' style='color:blue; background-color:white'>Click here to view tracking information online</a>" };
            }
        }

        /// <summary>
        /// Get DhlEcommerceShipment Tracking URL
        /// </summary>
        protected override string GetCarrierTrackingUrlInternal(ShipmentEntity shipment) =>
            $"http://www.dhl.com/en/Ecommerce/tracking.html?AWB={shipment.TrackingNumber}&brand=DHL";

        /// <summary>
        /// Gets the service types that are available for this shipment type (i.e have not been excluded).
        /// </summary>
        public override IEnumerable<int> GetAvailableServiceTypes(IExcludedServiceTypeRepository repository)
        {
            return EnumHelper.GetEnumList<DhlEcommerceServiceType>()
                .Select(x => x.Value)
                .Cast<int>()
                .Except(GetExcludedServiceTypes(repository));
        }

        /// <summary>
        /// Gets the service types that have been excluded for this shipment type. The integer
        /// values are intended to correspond to the appropriate enumeration values of the specific
        /// shipment type (i.e. the integer values would correspond to PostalServiceType values
        /// for a UspsShipmentType).
        ///
        /// Also removes DhlEcommerceMax if it is restricted.
        /// </summary>
        /// <param name="repository">The repository from which the service types are fetched.</param>
        public override IEnumerable<int> GetExcludedServiceTypes(IExcludedServiceTypeRepository repository)
        {
            var baseExcludedTypes = base.GetExcludedServiceTypes(repository).ToList();

            // If restricted from DHL eCommerce Max, remove it
            if (licenseService.CheckRestriction(EditionFeature.DhlEcommerceMax, null) != EditionRestrictionLevel.None)
            {
                baseExcludedTypes.Remove((int) DhlEcommerceServiceType.US_DhlSMParcelExpeditedMax);
            }

            return baseExcludedTypes;
        }

        /// <summary>
        /// Gets the AvailablePackageTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public override Dictionary<int, string> BuildPackageTypeDictionary(List<ShipmentEntity> shipments, IExcludedPackageTypeRepository excludedServiceTypeRepository)
        {
            // Get valid packaging types
            List<int> validPackageTypes = Enum.GetValues(typeof(DhlEcommercePackagingType)).Cast<int>().ToList();
            IEnumerable<int> excludedPackageTypes = GetExcludedPackageTypes(excludedServiceTypeRepository);

            // If there's an existing shipment with a package type that has been excluded, we need to re-add it here
            if (shipments != null && shipments.Any())
            {
                IEnumerable<int> neededPackageTypes = shipments.Select(s => s.DhlEcommerce.PackagingType).Distinct().ToList();
                excludedPackageTypes = excludedPackageTypes.Except(neededPackageTypes);
                validPackageTypes.AddRange(neededPackageTypes);
            }

            return validPackageTypes.Except(excludedPackageTypes)
                .ToDictionary(t => t, t => EnumHelper.GetDescription((DhlEcommercePackagingType) t));
        }

        /// <summary>
        /// Load all the label data for the given shipmentID
        /// </summary>
        private static List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipmentFactory)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipmentFactory, nameof(shipmentFactory));

            return DataResourceManager.GetConsumerResourceReferences(shipmentFactory().ShipmentID)
                .Where(x => x.Label.StartsWith("LabelPrimary") || x.Label.StartsWith("LabelPart"))
                .Select(x => new TemplateLabelData(null, "Label", x.Label.StartsWith("LabelPrimary") ?
                    TemplateLabelCategory.Primary : TemplateLabelCategory.Supplemental, x))
                .ToList();
        }
    }
}
