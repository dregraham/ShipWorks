﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Imaging;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Enums;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Policies;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Amazon implementation of shipment type
    /// </summary>
    public class AmazonSFPShipmentType : ShipmentType
    {
        private readonly IStoreManager storeManager;
        private readonly IShippingManager shippingManager;
        private readonly ILicenseService licenseService;
        private readonly IAmazonSFPServiceTypeRepository serviceTypeRepository;
        private readonly IOrderManager orderManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPShipmentType(IStoreManager storeManager, IShippingManager shippingManager, ILicenseService licenseService,
            IAmazonSFPServiceTypeRepository serviceTypeRepository, IOrderManager orderManager)
        {
            this.storeManager = storeManager;
            this.shippingManager = shippingManager;
            this.licenseService = licenseService;
            this.serviceTypeRepository = serviceTypeRepository;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Shipment type code
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.AmazonSFP;

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.AmazonSFP == null)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }

            return new[] { new AmazonSFPPackageAdapter(shipment) };
        }

        /// <summary>
        /// Ensures that the carrier specific data for the shipment, such as the FedEx data, are loaded for the shipment.  If the data
        /// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
        /// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
        /// had been deleted.
        /// </summary>
        protected override void LoadShipmentDataInternal(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "AmazonSFP", typeof(AmazonSFPShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment) =>
            shipment.AmazonSFP.ShippingServiceName;

        /// <summary>
        /// Get the carrier specific description of the shipping service used.
        /// </summary>
        public override string GetServiceDescription(string serviceCode) => serviceCode;

        /// <summary>
        /// Get detailed information about the parcel in a generic way that can be used across shipment types
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            return new ShipmentParcel(shipment, null,
                new AmazonSFPInsuranceChoice(shipment),
                new DimensionsAdapter(shipment.AmazonSFP))
            {
                TotalWeight = shipment.TotalWeight
            };
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
                new LabelsOutline(container.Context, shipment, labels, () => ImageFormat.Png),
                ElementOutline.If(() => shipment().Processed));

            ElementOutline outline = container.AddElement("Amazon");

            outline.AddElement("Carrier", () => loaded().AmazonSFP.CarrierName);
            outline.AddElement("Service", () => loaded().AmazonSFP.ShippingServiceName);
            outline.AddElement("AmazonUniqueShipmentID", () => loaded().AmazonSFP.AmazonUniqueShipmentID);
            outline.AddElement("ShippingServiceID", () => loaded().AmazonSFP.ShippingServiceID);
        }

        /// <summary>
        /// Load all the label data for the given shipmentID
        /// </summary>
        private static List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            return DataResourceManager.GetConsumerResourceReferences(shipment().ShipmentID)
                .Where(x => x.Label.StartsWith("LabelPrimary") || x.Label.StartsWith("LabelPart"))
                .Select(x => new TemplateLabelData(null, "Label", x.Label.StartsWith("LabelPrimary") ?
                    TemplateLabelCategory.Primary : TemplateLabelCategory.Supplemental, x))
                .ToList();
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for a provider based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an IBestRateShippingBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment, IBestRateExcludedAccountRepository bestRateExcludedAccountRepository) =>
            new NullShippingBroker();

        /// <summary>
        /// Create and Initialize a new shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            if (shipment.AmazonSFP == null)
            {
                shipment.AmazonSFP = new AmazonSFPShipmentEntity(shipment.ShipmentID);
            }

            AmazonSFPShipmentEntity amazonShipment = shipment.AmazonSFP;

            amazonShipment.DimsWeight = shipment.ContentWeight;
            amazonShipment.Insurance = false;
            amazonShipment.RequestedLabelFormat = (int) ThermalLanguage.None;

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Amazon supports rates
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
                ShipmentType = ShipmentTypeCode.AmazonSFP,
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
        /// Gets a value indicating whether this instance is shipment type restricted.
        /// </summary>
        ///
        /// Overridden to use dependency
        public override bool IsShipmentTypeRestricted =>
            licenseService.CheckRestriction(EditionFeature.ShipmentType, ShipmentTypeCode) == EditionRestrictionLevel.Hidden;

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        public override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            AmazonSFPProfileEntity amazon = profile.AmazonSFP;
            amazon.DeliveryExperience = (int) AmazonSFPDeliveryExperienceType.DeliveryConfirmationWithoutSignature;
            amazon.ShippingServiceID = string.Empty;
            amazon.Reference1 = "Order {//Order/Number}";
            amazon.ShippingProfile.RequestedLabelFormat = (int) ThermalLanguage.None;
        }

        /// <summary>
        /// Get the dims weight from a shipment, if any
        /// </summary>
        protected override double GetDimsWeight(IShipmentEntity shipment) =>
            shipment.AmazonSFP?.DimsAddWeight == true ? shipment.AmazonSFP.DimsWeight : 0;

        /// <summary>
        /// Tracks the shipment.
        /// </summary>
        [SuppressMessage("SonarQube", "S1871:Two branches in the same conditional structure should not have exactly the same implementation",
            Justification = "Easier to understand broken out this way.")]
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            if (string.IsNullOrWhiteSpace(shipment?.TrackingNumber))
            {
                return new TrackingResult { Summary = "No tracking number found..." };
            }

            string trackingLink = GetCarrierTrackingUrl(shipment);

            return string.IsNullOrWhiteSpace(trackingLink) ?
                new TrackingResult { Summary = "No tracking information available..." } :
                new TrackingResult { Summary = $"<a style='background-color:#FAFAFA; color:#2266AA;' href ={trackingLink}> Click here for tracking</a>" };
        }

        /// <summary>
        /// Try to determine carrier, if we can, send back the tracking URL
        /// </summary>
        protected override string GetCarrierTrackingUrlInternal(ShipmentEntity shipment)
        {
            string trackingLink = string.Empty;

            string serviceUsed = shippingManager.GetOverriddenServiceUsed(shipment);
            if (serviceUsed.IndexOf("ups", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                trackingLink = $"http://wwwapps.ups.com/WebTracking/processInputRequest?HTMLVersion=5.0&amp;loc=en_US&" +
                    $"amp;Requester=UPSHome&amp;tracknum={shipment.TrackingNumber}&amp;AgreeToTermsAndConditions=yes&amp;track.x=46&amp;track.y=9";
            }
            else if (serviceUsed.IndexOf("DHL SM", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                trackingLink = $"http://webtrack.dhlglobalmail.com/?mobile=&amp;trackingnumber={shipment.TrackingNumber}";
            }
            else if (serviceUsed.IndexOf("USPS", StringComparison.OrdinalIgnoreCase) >= 0 || serviceUsed.IndexOf("SmartPost", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                trackingLink = $"https://tools.usps.com/go/TrackConfirmAction.action?tLabels={shipment.TrackingNumber}";
            }
            else if (serviceUsed.IndexOf("FedEx", StringComparison.OrdinalIgnoreCase) >= 0 && serviceUsed.IndexOf("FIMS", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                trackingLink = $"http://mailviewrecipient.fedex.com/recip_package_summary.aspx?PostalID={shipment.TrackingNumber}";
            }
            else if (serviceUsed.IndexOf("FedEx", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                trackingLink = $"http://www.fedex.com/Tracking?language=english&amp;cntry_code=us&amp;tracknumbers={shipment.TrackingNumber}";
            }

            return trackingLink;
        }

        /// <summary>
        /// Get the Amazon shipment detail
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            ShipmentCommonDetail commonDetail = new ShipmentCommonDetail();

            AmazonSFPShipmentEntity amazonShipment = shipment.AmazonSFP;

            commonDetail.PackageLength = amazonShipment.DimsLength;
            commonDetail.PackageWidth = amazonShipment.DimsWidth;
            commonDetail.PackageHeight = amazonShipment.DimsHeight;

            return commonDetail;
        }

        /// <summary>
        /// Gets the service types that are available for this shipment type (i.e have not
        /// been excluded).
        /// </summary>
        /// <param name="repository">The repository from which the excluded service types are fetched.</param>
        public override IEnumerable<int> GetAvailableServiceTypes(IExcludedServiceTypeRepository repository)
        {
            IEnumerable<int> allServices = serviceTypeRepository.Get().Select(x => x.AmazonSFPServiceTypeID);
            return allServices.Except(GetExcludedServiceTypes(repository));
        }

        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public override void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {
            if (shipment.AmazonSFP != null)
            {
                shipment.AmazonSFP.RequestedLabelFormat = (int) requestedLabelFormat;
            }
        }

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            bucket.Relations.Add(ShipmentEntity.Relations.AmazonSFPShipmentEntityUsingShipmentID);

            adapter.UpdateEntitiesDirectly(new AmazonSFPShipmentEntity { RequestedLabelFormat = newLabelFormat }, bucket);
        }

        /// <summary>
        /// Update the dynamic shipment data that could have changed "outside" the known editor
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            if (shipment.AmazonSFP != null)
            {
                shipment.RequestedLabelFormat = shipment.AmazonSFP.RequestedLabelFormat;
                shipment.Insurance = shipment.AmazonSFP.Insurance;
            }

            shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;
        }
    }
}
