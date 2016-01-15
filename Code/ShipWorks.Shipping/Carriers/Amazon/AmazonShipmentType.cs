using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.ShipSense.Packaging;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Stores;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon;
using System.Diagnostics;
using ShipWorks.Editions;
using ShipWorks.Core.Messaging;
using System.Diagnostics.CodeAnalysis;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Amazon implementation of shipment type
    /// </summary>
    public class AmazonShipmentType : ShipmentType
    {
        private readonly IStoreManager storeManager;
        private readonly IOrderManager orderManager;
        private readonly IShippingManager shippingManager;
        private readonly IEditionManager editionManager;
        
        /// <summary>
        /// Constructor for tests
        /// </summary>
        public AmazonShipmentType()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonShipmentType(IStoreManager storeManager, IOrderManager orderManager, IShippingManager shippingManager, IEditionManager editionManager)
        {
            // TODO: Refactor constuctor params down to 5 or less
            this.storeManager = storeManager;
            this.orderManager = orderManager;
            this.shippingManager = shippingManager;
            this.editionManager = editionManager;
        }

        /// <summary>
        /// Shipment type code
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.Amazon;

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.Amazon == null)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }

            return new[] { new AmazonPackageAdapter(shipment) };
        }

        /// <summary>
        /// Ensures that the carrier specific data for the shipment, such as the FedEx data, are loaded for the shipment.  If the data
        /// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
        /// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
        /// had been deleted.
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "Amazon", typeof(AmazonShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment) =>
            shipment.Amazon.ShippingServiceName;

        /// <summary>
        /// Get detailed information about the parcel in a generic way that can be used across shipment types
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            return new ShipmentParcel(shipment, null,
                new AmazonInsuranceChoice(shipment),
                new DimensionsAdapter(shipment.Amazon))
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
                new LabelsOutline(container.Context, shipment, labels, ImageFormat.Png),
                ElementOutline.If(() => shipment().Processed));

            ElementOutline outline = container.AddElement("Amazon");

            outline.AddElement("Carrier", () => loaded().Amazon.CarrierName);
            outline.AddElement("Service", () => loaded().Amazon.ShippingServiceName);
            outline.AddElement("AmazonUniqueShipmentID", () => loaded().Amazon.AmazonUniqueShipmentID);
            outline.AddElement("ShippingServiceID", () => loaded().Amazon.ShippingServiceID);
            outline.AddElement("ShippingServiceOfferID", () => loaded().Amazon.ShippingServiceOfferID);
        }

        /// <summary>
        /// Load all the label data for the given shipmentID
        /// </summary>
        static List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            List<TemplateLabelData> labelData = new List<TemplateLabelData>();

            // Get the resource list for our shipment
            List<DataResourceReference> resources =
                DataResourceManager.GetConsumerResourceReferences(shipment().ShipmentID);

            if (resources.Any())
            {
                // Add our standard label output
                DataResourceReference labelResource = resources.Single(i => i.Label == "LabelPrimary");
                labelData.Add(new TemplateLabelData(null, "Label", TemplateLabelCategory.Primary, labelResource));
            }

            return labelData;
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for a provider based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an IBestRateShippingBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment) =>
            new NullShippingBroker();

        /// <summary>
        /// Create and Initialize a new shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            AmazonShipmentEntity amazonShipment = shipment.Amazon;

            IAmazonOrder amazonCredentials = shipment.Order as IAmazonOrder;

            Debug.Assert(amazonCredentials != null);

            amazonShipment.DimsWeight = shipment.ContentWeight;

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

            orderManager.PopulateOrderDetails(shipment);

            IAmazonOrder order = shipment.Order as IAmazonOrder;

            IAmazonCredentials amazonCredentials = storeManager.GetStore(shipment.Order.StoreID) as IAmazonCredentials;

            return order != null && order.IsPrime && amazonCredentials != null;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is shipment type restricted.
        /// </summary>
        /// 
        /// Overridden to use dependency
        /// </remarks>
        public override bool IsShipmentTypeRestricted
        {
            get
            {
                EditionRestrictionIssue restriction = editionManager.ActiveRestrictions.CheckRestriction(EditionFeature.ShipmentType, ShipmentTypeCode);
                return restriction.Level == EditionRestrictionLevel.Hidden;
            }
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            base.LoadProfileData(profile, refreshIfPresent);
            ShipmentTypeDataService.LoadProfileData(profile, "Amazon", typeof(AmazonProfileEntity), refreshIfPresent);
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        public override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            AmazonProfileEntity amazon = profile.Amazon;

            amazon.DeliveryExperience = (int) AmazonDeliveryExperienceType.DeliveryConfirmationWithoutSignature;
            amazon.Weight = 0;

            amazon.DimsProfileID = 0;
            amazon.DimsLength = 0;
            amazon.DimsWidth = 0;
            amazon.DimsHeight = 0;
            amazon.DimsWeight = 0;
            amazon.DimsAddWeight = true;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            base.ApplyProfile(shipment, profile);

            if (shipment.Amazon == null)
            {
                return;
            }

            AmazonShipmentEntity amazonShipment = shipment.Amazon;
            AmazonProfileEntity amazonProfile = profile.Amazon;

            ShippingProfileUtility.ApplyProfileValue(amazonProfile.DeliveryExperience, amazonShipment, AmazonShipmentFields.DeliveryExperience);

            if (amazonProfile.Weight.GetValueOrDefault() > 0)
            {
                ShippingProfileUtility.ApplyProfileValue(amazonProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }

            ShippingProfileUtility.ApplyProfileValue(amazonProfile.DimsProfileID, amazonShipment, AmazonShipmentFields.DimsProfileID);
            if (amazonProfile.DimsProfileID != null)
            {
                ShippingProfileUtility.ApplyProfileValue(amazonProfile.DimsLength, amazonShipment, AmazonShipmentFields.DimsLength);
                ShippingProfileUtility.ApplyProfileValue(amazonProfile.DimsWidth, amazonShipment, AmazonShipmentFields.DimsWidth);
                ShippingProfileUtility.ApplyProfileValue(amazonProfile.DimsHeight, amazonShipment, AmazonShipmentFields.DimsHeight);
                ShippingProfileUtility.ApplyProfileValue(amazonProfile.DimsWeight, amazonShipment, AmazonShipmentFields.DimsWeight);
                ShippingProfileUtility.ApplyProfileValue(amazonProfile.DimsAddWeight, amazonShipment, AmazonShipmentFields.DimsAddWeight);
            }
        }

        /// <summary>
        /// Updates the total weight of the shipment
        /// </summary>
        public override void UpdateTotalWeight(ShipmentEntity shipment)
        {
            shipment.TotalWeight = shipment.ContentWeight;

            if (shipment.Amazon.DimsAddWeight)
            {
                shipment.TotalWeight += shipment.Amazon.DimsWeight;
            }
        }


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

            string trackingLink = string.Empty;

            string serviceUsed = shippingManager.GetServiceUsed(shipment);
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

            return string.IsNullOrWhiteSpace(trackingLink) ?
                new TrackingResult { Summary = "No tracking information available..." } :
                new TrackingResult { Summary = $"<a style='background-color:#FAFAFA; color:#2266AA;' href ={trackingLink}> Click here for tracking</a>" };
        }

        /// <summary>
        /// Get the Amazon shipment detail
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            ShipmentCommonDetail commonDetail = new ShipmentCommonDetail();

            AmazonShipmentEntity amazonShipment = shipment.Amazon;

            commonDetail.PackageLength = amazonShipment.DimsLength;
            commonDetail.PackageWidth = amazonShipment.DimsWidth;
            commonDetail.PackageHeight = amazonShipment.DimsHeight;

            return commonDetail;
        }
    }
}
