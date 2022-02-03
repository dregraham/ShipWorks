using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipEngine.CarrierApi.Client.Model;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Policies;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// AmazonSWA implementation of shipment type
    /// </summary>
    [Component(RegistrationType.Self)]
    [KeyedComponent(typeof(ShipmentType), ShipmentTypeCode.AmazonSWA, SingleInstance = true)]
    public class AmazonSWAShipmentType : ShipmentType
    {
        private readonly ICarrierAccountRepository<AmazonSWAAccountEntity, IAmazonSWAAccountEntity> accountRepository;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IShipEngineTrackingResultFactory trackingResultFactory;
        private readonly IShippingManager shippingManager;
        private readonly IOrderManager orderManager;
        private readonly IStoreManager storeManager;
        private readonly ILicenseService licenseService;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public AmazonSWAShipmentType(ICarrierAccountRepository<AmazonSWAAccountEntity, IAmazonSWAAccountEntity> accountRepository,
            IShipEngineWebClient shipEngineWebClient,
            IShipEngineTrackingResultFactory trackingResultFactory,
            IShippingManager shippingManager,
            IOrderManager orderManager,
            IStoreManager storeManager,
            ILicenseService licenseService)
        {
            this.accountRepository = accountRepository;
            this.shipEngineWebClient = shipEngineWebClient;
            this.trackingResultFactory = trackingResultFactory;
            this.shippingManager = shippingManager;
            this.orderManager = orderManager;
            this.storeManager = storeManager;
            this.licenseService = licenseService;
        }

        /// <summary>
        /// The ShipmentTypeCode represented by this ShipmentType
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.AmazonSWA;

        /// <summary>
        /// Whether multiple packages are supported by this shipment type.
        /// </summary>
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
        /// Checks whether this shipment type is allowed for the given shipment
        /// </summary>
        public override bool IsAllowedFor(ShipmentEntity shipment)
        {
            if (IsShipmentTypeRestricted)
            {
                return false;
            }

            if (shipment.Order == null)
            {
                orderManager.PopulateOrderDetails(shipment);
            }

            IAmazonOrder amazonOrder = shipment.Order as IAmazonOrder;

            AmazonShippingPolicyTarget target = new AmazonShippingPolicyTarget()
            {
                ShipmentType = ShipmentTypeCode.AmazonSWA,
                Shipment = shipment,
                Allowed = false,
                AmazonOrder = amazonOrder,
                AmazonCredentials = storeManager.GetStore(shipment.Order.StoreID) as IAmazonCredentials
            };

            ILicense license = licenseService.GetLicenses().FirstOrDefault();
            license?.ApplyShippingPolicy(ShipmentTypeCode, target);

            return target.Allowed;
        }

        /// <summary>
        /// Create and Initialize a new shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            if (shipment.AmazonSWA == null)
            {
                shipment.AmazonSWA = new AmazonSWAShipmentEntity(shipment.ShipmentID);
            }

            AmazonSWAShipmentEntity swaShipment = shipment.AmazonSWA;
            swaShipment.Service = (int) AmazonSWAServiceType.Ground;
            swaShipment.RequestedLabelFormat = (int) ThermalLanguage.None;
            swaShipment.AmazonSWAAccountID = 0;
            swaShipment.ShipEngineLabelID = string.Empty;
            swaShipment.DimsProfileID = 0;
            swaShipment.DimsLength = 0;
            swaShipment.DimsWidth = 0;
            swaShipment.DimsHeight = 0;
            swaShipment.DimsWeight = 0;
            swaShipment.DimsAddWeight = true;
            swaShipment.InsuranceValue = 0;
            swaShipment.Insurance = false;

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.AmazonSWA == null)
            {
                shippingManager.EnsureShipmentLoaded(shipment);
            }

            return new List<IPackageAdapter>()
            {
                new AmazonSWAPackageAdapter(shipment)
            };
        }

        /// <summary>
        /// Get the parcel data that describes details about a particular parcel
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return new ShipmentParcel(shipment, null,
                new InsuranceChoice(shipment, shipment.AmazonSWA, shipment.AmazonSWA, null),
                new DimensionsAdapter(shipment.AmazonSWA))
            {
                TotalWeight = shipment.TotalWeight
            };
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment) =>
            GetServiceDescriptionInternal((AmazonSWAServiceType) shipment.AmazonSWA.Service);

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(string serviceCode) =>
            Functional.ParseInt(serviceCode)
                .Match(x => GetServiceDescriptionInternal((AmazonSWAServiceType) x), _ => "Unknown");

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        private string GetServiceDescriptionInternal(AmazonSWAServiceType service) =>
            EnumHelper.GetDescription(service);

        /// <summary>
        /// Gets the best rate shipping broker for AmazonSWA
        /// </summary>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment, IBestRateExcludedAccountRepository bestRateExcludedAccountRepository)
        {
            return new NullShippingBroker();
        }

        /// <summary>
        /// Update the dynamic shipment data that could have changed "outside" the known editor
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;

            shipment.RequestedLabelFormat = shipment.AmazonSWA.RequestedLabelFormat;
            shipment.Insurance = shipment.AmazonSWA.Insurance;
        }

        /// <summary>
        /// Get the dims weight from a shipment, if any
        /// </summary>
        protected override double GetDimsWeight(IShipmentEntity shipment) =>
            shipment.AmazonSWA?.DimsAddWeight == true ? shipment.AmazonSWA.DimsWeight : 0;

        /// <summary>
        /// Get the shipment common detail for tango
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            ShipmentCommonDetail commonDetail = new ShipmentCommonDetail();

            AmazonSWAShipmentEntity swaShipment = shipment.AmazonSWA;
            IAmazonSWAAccountEntity account = accountRepository.GetAccountReadOnly(shipment);

            commonDetail.OriginAccount = (account == null) ? "" : account.Description;
            commonDetail.ServiceType = swaShipment.Service;

            // AmazonSWA doesn't have a packaging type concept, so default to 0
            commonDetail.PackagingType = 0;
            commonDetail.PackageLength = swaShipment.DimsLength;
            commonDetail.PackageWidth = swaShipment.DimsWidth;
            commonDetail.PackageHeight = swaShipment.DimsHeight;

            return commonDetail;
        }

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            bucket.Relations.Add(ShipmentEntity.Relations.AmazonSWAShipmentEntityUsingShipmentID);
            adapter.UpdateEntitiesDirectly(new AmazonSWAShipmentEntity { RequestedLabelFormat = newLabelFormat }, bucket);
        }

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address and any store specific logic that may impact whether customs
        /// is required (i.e. eBay GSP).
        /// </summary>
        protected override bool IsCustomsRequiredByShipment(IShipmentEntity shipment) => false;

        /// <summary>
        /// Ensures that the carrier specific data for the shipment, such as the AmazonSWA data, are loaded for the shipment.  If the data
        /// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
        /// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
        /// had been deleted.
        /// </summary>
        protected override void LoadShipmentDataInternal(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(
                this, shipment, shipment, "AmazonSWA", typeof(AmazonSWAShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Gets the service types that are available for this shipment type (i.e have not been excluded).
        /// </summary>
        public override IEnumerable<int> GetAvailableServiceTypes(IExcludedServiceTypeRepository repository) =>
            EnumHelper.GetEnumList<AmazonSWAServiceType>()
                .Select(x => x.Value)
                .Cast<int>()
                .Except(GetExcludedServiceTypes(repository));

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        public override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);
            profile.OriginID = (int) ShipmentOriginSource.Account;

            AmazonSWAProfileEntity swaProfile = profile.AmazonSWA;

            swaProfile.AmazonSWAAccountID = accountRepository.AccountsReadOnly.Any() ?
                accountRepository.AccountsReadOnly.First().AmazonSWAAccountID :
                0;

            swaProfile.Service = (int) AmazonSWAServiceType.Ground;
        }

        /// Create the XML input to the XSL engine
        /// </summary>
        public override void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            Lazy<List<TemplateLabelData>> labels = new Lazy<List<TemplateLabelData>>(() => LoadLabelData(shipment));

            // Add the labels content
            container.AddElement(
                "Labels",
                new LabelsOutline(container.Context, shipment, labels, () => ImageFormat.Png),
                ElementOutline.If(() => shipment().Processed));
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

        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public override void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {
            if (shipment.AmazonSWA != null)
            {
                shipment.AmazonSWA.RequestedLabelFormat = (int) requestedLabelFormat;
            }
        }

        /// <summary>
        /// Track the shipment
        /// </summary>
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            try
            {
                TrackingInformation trackingInfo = Task.Run(() =>
                {
                    return shipEngineWebClient.Track(shipment.AmazonSWA.ShipEngineLabelID, ApiLogSource.AmazonSWA);
                }).Result;

                return trackingResultFactory.Create(trackingInfo);
            }
            catch (Exception)
            {
                return new TrackingResult { Summary = $"<p>Tracking data is currently not available.</p>" };
            }
        }
    }
}
