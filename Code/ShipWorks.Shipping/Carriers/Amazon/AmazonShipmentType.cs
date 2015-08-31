﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.ShipSense.Packaging;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Amazon implementation of shipment type
    /// </summary>
    public class AmazonShipmentType : ShipmentType
    {
        IAmazonAccountManager accountManager;
		Func<IAmazonRates> amazonRatesFactory;

        public AmazonShipmentType(IAmazonAccountManager accountManager, Func<IAmazonRates> amazonRatesFactory)
        {
            this.accountManager = accountManager;
            this.amazonRatesFactory = amazonRatesFactory;
        }

        /// <summary>
        /// Shipment type code
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get
            {
                return ShipmentTypeCode.Amazon;
            }
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            return new List<IPackageAdapter>()
            {
                new NullPackageAdapter()
            };
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
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            return string.Format("{0} {1}", shipment.Amazon.CarrierName, shipment.Amazon.ShippingServiceName);
        }

        /// <summary>
        /// Get detailed information about the parcel in a generic way that can be used accross shipment types
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

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
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            return new NullShippingBroker();
        }

        /// <summary>
        /// Create and Initialize a new shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            AmazonShipmentEntity amazonShipment = shipment.Amazon;

            AmazonOrderEntity amazonOrder = shipment.Order as AmazonOrderEntity;

            // TODO: Remove or replace this if statement when we decide how to handle non-Amazon orders.
            Debug.Assert(amazonOrder != null);
            if (amazonOrder == null)
            {
                amazonShipment.DateMustArriveBy = DateTime.Now.AddDays(2); 
            }
            else
            {
                amazonShipment.DateMustArriveBy = amazonOrder.LatestExpectedDeliveryDate ?? DateTime.Now.AddDays(2);   
            }

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
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            
            return GetCachedRates<AmazonShipperException>(shipment, GetRatesFromApi);
        }

        /// <summary>
        /// Gets rates from the Amazon API
        /// </summary>
        private RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            IAmazonRates amazonRates = amazonRatesFactory();
            return amazonRates.GetRates(shipment);
        }

        /// <summary>
        /// Amazon supports rates
        /// </summary>
        public override bool SupportsGetRates
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the fields used for rating a shipment.
        /// </summary>
        protected override IEnumerable<IEntityField2> GetRatingFields(ShipmentEntity shipment)
        {
            List<IEntityField2> fields = base.GetRatingFields(shipment).ToList();

            fields.AddRange
                (
                    new List<IEntityField2>()
                    {
                        shipment.Amazon.Fields[AmazonShipmentFields.AmazonAccountID.FieldIndex],
                        shipment.Amazon.Fields[AmazonShipmentFields.CarrierName.FieldIndex],
                        shipment.Amazon.Fields[AmazonShipmentFields.CarrierWillPickUp.FieldIndex],
                        shipment.Amazon.Fields[AmazonShipmentFields.DateMustArriveBy.FieldIndex],
                        shipment.Amazon.Fields[AmazonShipmentFields.DeclaredValue.FieldIndex],
                        shipment.Amazon.Fields[AmazonShipmentFields.DeliveryExperience.FieldIndex],
                        shipment.Amazon.Fields[AmazonShipmentFields.DimsAddWeight.FieldIndex],
                        shipment.Amazon.Fields[AmazonShipmentFields.DimsHeight.FieldIndex],
                        shipment.Amazon.Fields[AmazonShipmentFields.DimsLength.FieldIndex],
                        shipment.Amazon.Fields[AmazonShipmentFields.DimsWeight.FieldIndex],
                        shipment.Amazon.Fields[AmazonShipmentFields.ShippingServiceID.FieldIndex],
                        shipment.Amazon.Fields[AmazonShipmentFields.ShippingServiceName.FieldIndex]
                    }
                );

            return fields;
        }
    }
}
