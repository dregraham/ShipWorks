using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Asendia implementation of shipment type
    /// </summary>
    [Component(RegistrationType.Self)]
    [KeyedComponent(typeof(ShipmentType), ShipmentTypeCode.Asendia, SingleInstance = true)]
    public class AsendiaShipmentType : ShipmentType
    {
        private readonly ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity> accountRepository;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IShipEngineTrackingResultFactory trackingResultFactory;
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public AsendiaShipmentType(ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity> accountRepository, IShipEngineWebClient shipEngineWebClient, IShipEngineTrackingResultFactory trackingResultFactory, IShippingManager shippingManager)
        {
            this.accountRepository = accountRepository;
            this.shipEngineWebClient = shipEngineWebClient;
            this.trackingResultFactory = trackingResultFactory;
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// The ShipmentTypeCode represented by this ShipmentType
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.Asendia;

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
        public override bool SupportsGetRates => false;

        /// <summary>
        /// Create and Initialize a new shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            if (shipment.Asendia == null)
            {
                shipment.Asendia = new AsendiaShipmentEntity(shipment.ShipmentID);
            }

            AsendiaShipmentEntity asendiaShipment = shipment.Asendia;
            asendiaShipment.Service = AsendiaServiceType.AsendiaPriorityTracked;
            asendiaShipment.RequestedLabelFormat = (int) ThermalLanguage.None;
            asendiaShipment.Contents = (int) ShipEngineContentsType.Merchandise;
            asendiaShipment.NonDelivery = (int) ShipEngineNonDeliveryType.ReturnToSender;
            asendiaShipment.NonMachinable = false;
            asendiaShipment.AsendiaAccountID = 0;
            asendiaShipment.ShipEngineLabelID = string.Empty;
            asendiaShipment.DimsProfileID = 0;
            asendiaShipment.DimsLength = 0;
            asendiaShipment.DimsWidth = 0;
            asendiaShipment.DimsHeight = 0;
            asendiaShipment.DimsWeight = 0;
            asendiaShipment.DimsAddWeight = true;
            asendiaShipment.InsuranceValue = 0;

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.Asendia == null)
            {
                shippingManager.EnsureShipmentLoaded(shipment);
            }

            return new List<IPackageAdapter>()
            {
                new AsendiaPackageAdapter(shipment)
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
                new InsuranceChoice(shipment, shipment, shipment.Asendia, null),
                new DimensionsAdapter(shipment.Asendia))
            {
                TotalWeight = shipment.TotalWeight
            };
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            return EnumHelper.GetDescription(shipment.Asendia.Service);
        }

        /// <summary>
        /// Gets the best rate shipping broker for Asendia
        /// </summary>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update the dynamic shipment data that could have changed "outside" the known editor
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;

            shipment.RequestedLabelFormat = shipment.Asendia.RequestedLabelFormat;
            shipment.Insurance = shipment.Asendia.Insurance;
        }

        /// <summary>
        /// Update the total weight of the shipment
        /// </summary>
        public override void UpdateTotalWeight(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            shipment.TotalWeight = shipment.ContentWeight;

            if (shipment.Asendia.DimsAddWeight)
            {
                shipment.TotalWeight += shipment.Asendia.DimsWeight;
            }
        }

        /// <summary>
        /// Get the shipment common detail for tango
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            ShipmentCommonDetail commonDetail = new ShipmentCommonDetail();

            AsendiaShipmentEntity asendiaShipmentEntity = shipment.Asendia;
            AsendiaAccountEntity account = accountRepository.GetAccount(asendiaShipmentEntity.AsendiaAccountID);

            commonDetail.OriginAccount = (account == null) ? "" : account.Description;
            commonDetail.ServiceType = (int) asendiaShipmentEntity.Service;

            // Asendia doesn't have a packaging type concept, so default to 0
            commonDetail.PackagingType = 0;
            commonDetail.PackageLength = asendiaShipmentEntity.DimsLength;
            commonDetail.PackageWidth = asendiaShipmentEntity.DimsWidth;
            commonDetail.PackageHeight = asendiaShipmentEntity.DimsHeight;

            return commonDetail;
        }

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            bucket.Relations.Add(ShipmentEntity.Relations.AsendiaShipmentEntityUsingShipmentID);

            adapter.UpdateEntitiesDirectly(new AsendiaShipmentEntity { RequestedLabelFormat = newLabelFormat }, bucket);
        }

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address and any store specific logic that may impact whether customs
        /// is required (i.e. eBay GSP).
        /// </summary>
        /// <remarks>
        /// Asendia only supports international shipments, so customs is always required
        /// </remarks>
        protected override bool IsCustomsRequiredByShipment(IShipmentEntity shipment) => true;

        /// <summary>
        /// Ensures that the carrier specific data for the shipment, such as the Asendia data, are loaded for the shipment.  If the data
        /// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
        /// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
        /// had been deleted.
        /// </summary>
        protected override void LoadShipmentDataInternal(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(
                            this, shipment, shipment, "Asendia", typeof(AsendiaShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Gets the service types that are available for this shipment type (i.e have not been excluded).
        /// </summary>
        public override IEnumerable<int> GetAvailableServiceTypes(IExcludedServiceTypeRepository repository)
        {
            return EnumHelper.GetEnumList<AsendiaServiceType>()
                .Select(x => x.Value)
                .Cast<int>()
                .Except(GetExcludedServiceTypes(repository));
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadProfileData(profile, "Asendia", typeof(AsendiaProfileEntity), refreshIfPresent);
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        public override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);
            profile.OriginID = (int) ShipmentOriginSource.Account;

            AsendiaProfileEntity asendia = profile.Asendia;

            asendia.AsendiaAccountID = accountRepository.AccountsReadOnly.Any()
                ? accountRepository.AccountsReadOnly.First().AsendiaAccountID
                : 0;

            asendia.Service = (int) AsendiaServiceType.AsendiaPriorityTracked;
            asendia.Contents = (int) ShipEngineContentsType.Merchandise;
            asendia.NonDelivery = (int) ShipEngineNonDeliveryType.ReturnToSender;
            asendia.NonMachinable = false;

            asendia.Weight = 0;
            asendia.DimsProfileID = 0;
            asendia.DimsLength = 0;
            asendia.DimsWidth = 0;
            asendia.DimsHeight = 0;
            asendia.DimsWeight = 0;
            asendia.DimsAddWeight = true;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, IShippingProfileEntity profile)
        {
            base.ApplyProfile(shipment, profile);

            AsendiaShipmentEntity asendiaShipment = shipment.Asendia;
            IAsendiaProfileEntity asendiaProfile = profile.Asendia;

            long? accountID = (asendiaProfile.AsendiaAccountID == 0 && accountRepository.AccountsReadOnly.Any()) ?
                accountRepository.AccountsReadOnly.First().AsendiaAccountID :
                asendiaProfile.AsendiaAccountID;

            ShippingProfileUtility.ApplyProfileValue(accountID, asendiaShipment, AsendiaShipmentFields.AsendiaAccountID);
            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.Service, asendiaShipment, AsendiaShipmentFields.Service);

            if (asendiaProfile.Weight.HasValue && !asendiaProfile.Weight.Value.IsEquivalentTo(0))
            {
                ShippingProfileUtility.ApplyProfileValue(asendiaProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }

            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.NonMachinable, asendiaShipment, AsendiaShipmentFields.NonMachinable);
            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.NonDelivery, asendiaShipment, AsendiaShipmentFields.NonDelivery);
            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.Contents, asendiaShipment, AsendiaShipmentFields.Contents);

            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.DimsProfileID, asendiaShipment, AsendiaShipmentFields.DimsProfileID);
            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.DimsWeight, asendiaShipment, AsendiaShipmentFields.DimsWeight);
            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.DimsLength, asendiaShipment, AsendiaShipmentFields.DimsLength);
            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.DimsHeight, asendiaShipment, AsendiaShipmentFields.DimsHeight);
            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.DimsWidth, asendiaShipment, AsendiaShipmentFields.DimsWidth);
            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.DimsAddWeight, asendiaShipment, AsendiaShipmentFields.DimsAddWeight);

            UpdateTotalWeight(shipment);

            UpdateDynamicShipmentData(shipment);
        }

        /// <summary>
        /// Create the XML input to the XSL engine
        /// </summary>
        public override void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            Lazy<List<TemplateLabelData>> labels = new Lazy<List<TemplateLabelData>>(() => LoadLabelData(shipment));

            // Add the labels content
            container.AddElement(
                "Labels",
                new LabelsOutline(container.Context, shipment, labels, ImageFormat.Png),
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
            if (shipment.Asendia != null)
            {
                shipment.Asendia.RequestedLabelFormat = (int) requestedLabelFormat;
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
                    return shipEngineWebClient.Track(shipment.Asendia.ShipEngineLabelID, ApiLogSource.Asendia);
                }).Result;

                return trackingResultFactory.Create(trackingInfo);
            }
            catch (Exception)
            {
                return new TrackingResult { Summary = $"<a href='http://tracking.asendiausa.com/t.aspx?p={shipment.TrackingNumber}' style='color:blue; background-color:white'>Click here to view tracking information online</a>" };
            }
        }
    }
}
