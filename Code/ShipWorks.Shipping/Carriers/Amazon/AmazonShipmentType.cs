using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.ShipSense.Packaging;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Shipping.Editing.Rating;
using Interapptive.Shared.Utility;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.Stores.Content;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Amazon implementation of shipment type
    /// </summary>
    public class AmazonShipmentType : ShipmentType
    {
        private readonly IAmazonAccountManager accountManager;
        private readonly Func<IAmazonRates> amazonRatesFactory;
        private readonly IStoreManager storeManager;
        private readonly IOrderManager orderManager;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly Func<IAmazonLabelService> amazonLabelServiceFactory;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonShipmentType(IAmazonAccountManager accountManager, IDateTimeProvider dateTimeProvider, 
            Func<IAmazonRates> amazonRatesFactory, Func<IAmazonLabelService> amazonLabelServiceFactory, IStoreManager storeManager, IOrderManager orderManager)
        {
            this.accountManager = accountManager;
            this.amazonRatesFactory = amazonRatesFactory;
            this.amazonLabelServiceFactory = amazonLabelServiceFactory;
            this.storeManager = storeManager;
            this.orderManager = orderManager;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Shipment type code
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.Amazon;

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment) =>
            new List<IPackageAdapter> { new NullPackageAdapter() };

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
            $"{shipment.Amazon.CarrierName} {shipment.Amazon.ShippingServiceName}";

        /// <summary>
        /// Get detailed information about the parcel in a generic way that can be used accross shipment types
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            return new ShipmentParcel(shipment, null,
                new InsuranceChoice(shipment, shipment, shipment.Amazon, null),
                new DimensionsAdapter())
            {
                TotalWeight = shipment.TotalWeight
            };
        }

        /// <summary>
        /// Process the shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment) =>
            amazonLabelServiceFactory().Create(shipment);

        /// <summary>
        /// Create the XML input to the XSL engine
        /// </summary>
        public override void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            ElementOutline outline = container.AddElement("Amazon");
            outline.AddElement("Carrier", () => loaded().Amazon.CarrierName);
            outline.AddElement("Service", () => loaded().Amazon.ShippingServiceName);
            outline.AddElement("AmazonUniqueShipmentID", () => loaded().Amazon.AmazonUniqueShipmentID);
            outline.AddElement("ShippingServiceID", () => loaded().Amazon.ShippingServiceID);
            outline.AddElement("ShippingServiceOfferID", () => loaded().Amazon.ShippingServiceOfferID);
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

            AmazonOrderEntity amazonOrder = shipment.Order as AmazonOrderEntity;

            // TODO: Remove or replace this if statement when we decide how to handle non-Amazon orders.
            Debug.Assert(amazonOrder != null);

            amazonShipment.DateMustArriveBy = amazonOrder?.LatestExpectedDeliveryDate ?? dateTimeProvider.Now.AddDays(2);

            // TODO: This should probably be removed when we have amazon profiles...
            long accountID = accountManager.Accounts.Any() ? accountManager.Accounts.First().AmazonAccountID : 0;
            amazonShipment.AmazonAccountID = accountID;

            amazonShipment.DimsWeight = shipment.ContentWeight;
            amazonShipment.CarrierWillPickUp = false;
            amazonShipment.SendDateMustArriveBy = false;

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            RateGroup rateGroup = GetCachedRates<AmazonShipperException>(shipment, GetRatesFromApi);

            Messenger.Current.Send(new AmazonRatesRetrievedMessage(this, rateGroup));

            return rateGroup;
        }

        /// <summary>
        /// Gets rates from the Amazon API
        /// </summary>
        private RateGroup GetRatesFromApi(ShipmentEntity shipment) =>
            amazonRatesFactory().GetRates(shipment);

        /// <summary>
        /// Amazon supports rates
        /// </summary>
        public override bool SupportsGetRates => true;

        /// <summary>
        /// Gets the fields used for rating a shipment.
        /// </summary>
        public override RatingFields RatingFields
        {
            get
            {
                if (ratingField != null)
                {
                    return ratingField;
                }

                ratingField = base.RatingFields;

                ratingField.ShipmentFields.Add(AmazonShipmentFields.AmazonAccountID);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.CarrierName);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.CarrierWillPickUp);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.DateMustArriveBy);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.DeclaredValue);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.DeliveryExperience);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.DimsAddWeight);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.DimsHeight);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.DimsLength);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.DimsWeight);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.ShippingServiceID);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.ShippingServiceName);

                return ratingField;
            }
        }

        /// <summary>
        /// Checks whether this shipment type is allowed for the given shipment
        /// </summary>
        public override bool IsAllowedFor(ShipmentEntity shipment)
        {
            orderManager.PopulateOrderDetails(shipment);
            
            long storeId = shipment.Order.StoreID;

            StoreEntity storeEntity = storeManager.GetStore(storeId);

            if (storeEntity?.TypeCode != (int) StoreTypeCode.Amazon)
            {
                return false;
            }

            return ((AmazonOrderEntity)shipment.Order).IsPrime == (int)AmazonMwsIsPrime.Yes;
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
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            AmazonProfileEntity amazon = profile.Amazon;

            amazon.CarrierWillPickUp = false;
            amazon.DeliveryExperience = (int) AmazonDeliveryExperienceType.DeliveryConfirmationWithoutSignature;
            amazon.Weight = 0;
            amazon.SendDateMustArriveBy = false;

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

            ShippingProfileUtility.ApplyProfileValue(amazonProfile.CarrierWillPickUp, amazonShipment, AmazonShipmentFields.CarrierWillPickUp);
            ShippingProfileUtility.ApplyProfileValue(amazonProfile.DeliveryExperience, amazonShipment, AmazonShipmentFields.DeliveryExperience);
            ShippingProfileUtility.ApplyProfileValue(amazonProfile.SendDateMustArriveBy, amazonShipment, AmazonShipmentFields.SendDateMustArriveBy);

            if (amazonProfile.Weight != null && amazonProfile.Weight.Value != 0)
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
    }
}
