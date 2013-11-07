﻿using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Stores.Platforms.AmeriCommerce.WebServices;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    /// <summary>
    /// Rate broker that finds the best rates for UPS accounts
    /// </summary>
    public class UpsBestRateBroker : IBestRateShippingBroker
    {
        private readonly UpsShipmentType shipmentType;
        private readonly ICarrierAccountRepository<UpsAccountEntity> accountRepository;

        /// <summary>
        /// Creates a broker with the default shipment type and account repository
        /// </summary>
        /// <remarks>This is designed to be used within ShipWorks</remarks>
        public UpsBestRateBroker() : this(new UpsOltShipmentType(), new UpsAccountRepository())
        {
        }

        /// <summary>
        /// Creates a broker with the specified shipment type and account repository
        /// </summary>
        /// <param name="shipmentType">Instance of a UPS shipment type that will be used to get rates</param>
        /// <param name="accountRepository">Instance of an account repository that will get UPS accounts</param>
        /// <remarks>This is designed to be used by tests</remarks>
        public UpsBestRateBroker(UpsShipmentType shipmentType, ICarrierAccountRepository<UpsAccountEntity> accountRepository)
        {
            this.shipmentType = shipmentType;
            this.accountRepository = accountRepository;
        }
        /// <summary>
        /// Gets the single best rate for each UPS account based 
        /// on the configuration of the best rate shipment data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A list of RateResults composed of the single best rate for each UPS account.</returns>
        public List<RateResult> GetBestRates(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            List<RateResult> allRates = new List<RateResult>();
            List<UpsAccountEntity> upsAccounts = accountRepository.Accounts.ToList();
            Dictionary<RateResult, UpsShipmentEntity> rateShipments = new Dictionary<RateResult, UpsShipmentEntity>();

            // Create a clone so we don't have to worry about modifying the original shipment
            ShipmentEntity testRateShipment = EntityUtility.CloneEntity(shipment);
            testRateShipment.ShipmentType = (int)ShipmentTypeCode.UpsOnLineTools;

            foreach (UpsAccountEntity account in upsAccounts)
            {
                // Create the UpsShipment that will be used to get rates
                testRateShipment.Ups = new UpsShipmentEntity();
                shipmentType.ConfigureNewShipment(testRateShipment);
                UpdateUpsShipmentSettings(testRateShipment, shipment.ContentWeight, account.UpsAccountID);

                try
                {
                    IEnumerable<RateResult> results = shipmentType.GetRates(testRateShipment).Rates;

                    // Save a mapping between the rate and the UPS shipment used to get the rate
                    foreach (RateResult result in results)
                    {
                        rateShipments.Add(result, testRateShipment.Ups);
                    }

                    allRates.AddRange(results);
                }
                catch (ShippingException)
                {
                    // We will handle exceptions in a future story.  For now, just eat them.
                }
            }

            // Update the results to show the correct hover text
            foreach (RateResult result in allRates)
            {
                result.HoverText = "UPS - " + result.Description;
            }

            // Return all the rates, filtered by service level, then grouped by UpsServiceType and ServiceLevel
            List<RateResult> filteredRates = allRates
                .Where(r => r.Amount > 0 && MeetsServiceLevelCriteria(r.ServiceLevel, (ServiceLevelType)shipment.BestRate.ServiceLevel))
                .GroupBy(r => (UpsServiceType)r.Tag)
                .SelectMany(RateResultsByServiceLevel)
                .ToList();

            foreach (RateResult rate in filteredRates)
            {
                // Save the shipment and rate service type for use in the anonymous method
                object originalTag = rate.Tag;
                UpsShipmentEntity rateShipment = rateShipments[rate];

                // Replace the service type with a function that will select the correct shipment type
                rate.Tag = (Action<ShipmentEntity>)(selectedShipment =>
                    {
                        rateShipment.Service = (int)originalTag;
                        selectedShipment.Ups = rateShipment;
                        selectedShipment.ShipmentType = (int)ShipmentTypeCode.UpsOnLineTools;
                    });
            }

            return filteredRates;
        }

        /// <summary>
        /// Gets a list of rates by UpsServiceType
        /// </summary>
        /// <param name="upsTypeGroup">Group </param>
        /// <returns></returns>
        private static IEnumerable<RateResult> RateResultsByServiceLevel(IGrouping<UpsServiceType, RateResult> upsTypeGroup)
        {
            return upsTypeGroup.GroupBy(r => r.ServiceLevel).Select(CheapestRateInGroup);
        }

        /// <summary>
        /// Gets the cheapest rate in group of rates.
        /// </summary>
        /// <param name="serviceLevelGroup">Group of rates from which to return the cheapest</param>
        /// <returns></returns>
        private static RateResult CheapestRateInGroup(IGrouping<ServiceLevelType, RateResult> serviceLevelGroup)
        {
            return serviceLevelGroup.OrderBy(r => r.Amount).FirstOrDefault();
        }

        /// <summary>
        /// Determines whether the rate's service level meets the requested level criteria
        /// </summary>
        /// <param name="rateServiceLevel">Service level of the rate that should be tested</param>
        /// <param name="requestedType">Requested level against which to test</param>
        /// <returns></returns>
        private static bool MeetsServiceLevelCriteria(ServiceLevelType rateServiceLevel, ServiceLevelType requestedType)
        {
            return ServiceLevelType.Anytime == requestedType ||
                   (rateServiceLevel != ServiceLevelType.Anytime && rateServiceLevel <= requestedType);
        }

        /// <summary>
        /// Updates data on the Ups shipment that is required for checking best rate
        /// </summary>
        /// <param name="testRateShipment">Shipment that we'll be working with</param>
        /// <param name="contentWeight">The content weight of the shipment.</param>
        /// <param name="upsAccountID">The UPS Account Entity ID for this shipment.</param>
        private static void UpdateUpsShipmentSettings(ShipmentEntity testRateShipment, double contentWeight, long upsAccountID)
        {
            testRateShipment.Ups.Packages[0].DimsHeight = testRateShipment.BestRate.DimsHeight;
            testRateShipment.Ups.Packages[0].DimsWidth = testRateShipment.BestRate.DimsWidth;
            testRateShipment.Ups.Packages[0].DimsLength = testRateShipment.BestRate.DimsLength;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            testRateShipment.Ups.Packages[0].Weight = contentWeight;
            testRateShipment.Ups.Packages[0].DimsAddWeight = false;
            testRateShipment.Ups.Packages[0].PackagingType = (int) UpsPackagingType.Custom;
            testRateShipment.Ups.Service = (int) UpsServiceType.UpsGround;
            testRateShipment.Ups.UpsAccountID = upsAccountID;
        }
    }
}
