using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Amazon implementation of shipment type
    /// </summary>
    public class AmazonShipmentType : ShipmentType
    {
        readonly IAmazonAccountManager accountManager;
        readonly Func<IAmazonRates> amazonRatesFactory;
        readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonShipmentType(IAmazonAccountManager accountManager, IDateTimeProvider dateTimeProvider, Func<IAmazonRates> amazonRatesFactory)
        {
            this.accountManager = accountManager;
            this.amazonRatesFactory = amazonRatesFactory;
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
                new DimensionsAdapter());
        }

        /// <summary>
        /// Process the shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }

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

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        public override RateGroup GetRates(ShipmentEntity shipment) => 
            GetCachedRates<AmazonShipperException>(shipment, GetRatesFromApi);

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
    }
}
