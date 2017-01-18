﻿using System;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Services
{
    public class RatesRetriever : IRatesRetriever
    {
        private const string errorMessage = "ShipWorks could not get rates for this shipment";
        private readonly ICachedRatesService cachedRateService;
        private readonly IIndex<ShipmentTypeCode, IRatingService> ratingServiceLookup;
        private readonly IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeLookup;
        private readonly IStoreTypeManager storeTypeManager;

        public RatesRetriever(IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeLookup,
            IIndex<ShipmentTypeCode, IRatingService> ratingServiceLookup,
            ICachedRatesService cachedRateService,
            IStoreTypeManager storeTypeManager)
        {
            this.shipmentTypeLookup = shipmentTypeLookup;
            this.ratingServiceLookup = ratingServiceLookup;
            this.cachedRateService = cachedRateService;
            this.storeTypeManager = storeTypeManager;
        }

        /// <summary>
        /// Get rates for the given shipment using the appropriate ShipmentType
        /// </summary>
        public GenericResult<RateGroup> GetRates(ShipmentEntity shipment)
        {
            ShipmentType shipmentType = shipmentTypeLookup[shipment.ShipmentTypeCode];
            return GetRates(shipment, shipmentType);
        }

        /// <summary>
        /// Get rates for the given shipment using the appropriate ShipmentType
        /// </summary>
        public GenericResult<RateGroup> GetRates(ShipmentEntity shipment, ShipmentType shipmentType)
        {
            if (shipment.Processed)
            {
                string message = "The shipment has already been processed.";
                return GenericResult.FromError(message, new RateGroup(Enumerable.Empty<RateResult>()));
            }

            if (!shipmentType.SupportsGetRates)
            {
                string message = $"The carrier \"{shipmentType.ShipmentTypeName}\" does not support retrieving rates.";

                RateGroup rateGroup = new RateGroup(Enumerable.Empty<RateResult>());
                rateGroup.AddFootnoteFactory(new InformationFootnoteFactory("Select another carrier to get rates."));
                return GenericResult.FromError(message, rateGroup);
            }

            // Ensure data is valid and up-to-date
            shipmentType.UpdateDynamicShipmentData(shipment);

            // We're going to confirm the shipping address with the store as some stores may change
            // the shipping address depending on the shipping program being used (such as eBay's
            // Global Shipping Program), so we want to get rates for the location the package will be shipped

            // We want to retain the buyer's address on the original shipment object, so we're going to use
            // a cloned shipment to confirm the shipping address with the store. This way the original
            // shipment is not altered and persisted to the database if the store alters the address
            ShipmentEntity clonedShipment = EntityUtility.CloneEntity(shipment);

            // Confirm the address of the cloned shipment with the store giving it a chance to inspect/alter the shipping address
            StoreType storeType = storeTypeManager.GetType(clonedShipment);
            storeType.OverrideShipmentDetails(clonedShipment);

            IRatingService ratingService = ratingServiceLookup[shipment.ShipmentTypeCode];

            try
            {
                // Determine residential status
                if (shipmentType.IsResidentialStatusRequired(clonedShipment))
                {
                    clonedShipment.ResidentialResult = ResidentialDeterminationService.DetermineResidentialAddress(clonedShipment);
                }

                // Check to see if the rate is cached, if not call the rating service
                RateGroup rateResults = cachedRateService.GetCachedRates<ShippingException>(clonedShipment, ratingService.GetRates);

                // Copy back any best rate events that were set on the clone
                shipment.BestRateEvents |= clonedShipment.BestRateEvents;

                return rateResults is InvalidRateGroup ?
                    GenericResult.FromError(errorMessage, rateResults) :
                    GenericResult.FromSuccess(rateResults);
            }
            catch (ORMEntityOutOfSyncException ex)
            {
                return BuildResultsFromException(shipment, ex);
            }
            catch (ShippingException ex)
            {
                return BuildResultsFromException(shipment, ex);
            }
        }

        /// <summary>
        /// Build a rate result from the given shipment and exception
        /// </summary>
        private static GenericResult<RateGroup> BuildResultsFromException(ShipmentEntity shipment, Exception ex)
        {
            RateGroup rateGroup = new RateGroup(Enumerable.Empty<RateResult>());
            rateGroup.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(shipment.ShipmentTypeCode, ex));

            return GenericResult.FromError(errorMessage, rateGroup);
        }
    }
}
