﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Settings.Origin;
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
        public UpsBestRateBroker()
            : this(new UpsOltShipmentType(), new UpsAccountRepository())
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
        /// Gets a value indicating whether there any accounts available to a broker.
        /// </summary>
        /// <value>
        /// <c>true</c> if the broker [has accounts]; otherwise, <c>false</c>.
        /// </value>
        public bool HasAccounts
        {
            get { return accountRepository.Accounts.Any(); }
        }

        /// <summary>
        /// Gets the single best rate for each UPS account based 
        /// on the configuration of the best rate shipment data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="exceptionHandler"></param>
        /// <returns>A list of RateResults composed of the single best rate for each UPS account.</returns>
        public List<RateResult> GetBestRates(ShipmentEntity shipment, Action<ShippingException> exceptionHandler)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            List<RateResult> allRates = new List<RateResult>();

            List<UpsAccountEntity> upsAccounts = accountRepository.Accounts.ToList();

            Dictionary<RateResult, UpsAccountEntity> rateShipments = new Dictionary<RateResult, UpsAccountEntity>();

            // Create a clone so we don't have to worry about modifying the original shipment
            ShipmentEntity testRateShipment = EntityUtility.CloneEntity(shipment);
            testRateShipment.ShipmentType = (int) shipmentType.ShipmentTypeCode;

            foreach (UpsAccountEntity account in upsAccounts)
            {
                testRateShipment.Ups = new UpsShipmentEntity();

                shipmentType.ConfigureNewShipment(testRateShipment);
                UpdateShipmentSettings(testRateShipment, shipment, account);

                try
                {
                    IEnumerable<RateResult> results = shipmentType.GetRates(testRateShipment).Rates
                                                                  .Where(r => r.Tag != null)
                                                                  .Where(r => r.Amount > 0)
                                                                  .Where(r => (UpsServiceType)r.Tag != UpsServiceType.UpsSurePostBoundPrintedMatter && (UpsServiceType)r.Tag != UpsServiceType.UpsSurePostMedia);

                    // Save a mapping between the rate and the UPS shipment used to get the rate
                    foreach (RateResult result in results)
                    {
                        rateShipments.Add(result, account);
                    }

                    allRates.AddRange(results);
                }
                catch (ShippingException ex)
                {
                    // Offload exception handling to the passed in exception handler
                    exceptionHandler(ex);
                }
            }

            // Return all the rates, then group by UpsServiceType and ServiceLevel
            List<RateResult> filteredRates = allRates
                .GroupBy(r => (UpsServiceType)r.Tag)
                .SelectMany(RateResultsByServiceLevel)
                .ToList();

            foreach (RateResult rate in filteredRates)
            {
                // Replace the service type with a function that will select the correct shipment type
                rate.Tag = CreateRateSelectionFunction(rateShipments[rate], rate.Tag, shipmentType.ShipmentTypeCode);
                rate.Description = rate.Description.Contains("UPS") ? rate.Description : "UPS " + rate.Description;
            }

            return filteredRates.Select(x => new NoncompetitiveRateResult(x)).ToList<RateResult>();
        }

        /// <summary>
        /// Creates a function that can be used to select a specific rate
        /// </summary>
        /// <param name="account">Account that was used to get the rate</param>
        /// <param name="originalTag">UpsServiceType associated with the specific rate</param>
        /// <param name="shipmentTypeCode">Whether we should use online tools or Worldship</param>
        /// <returns>A function that, when executed, will convert the passed in shipment to a UPS shipment
        /// used to create the rate.</returns>
        private Action<ShipmentEntity> CreateRateSelectionFunction(UpsAccountEntity account, object originalTag, ShipmentTypeCode shipmentTypeCode)
        {
            return selectedShipment =>
                {
                    // Create a clone so we don't have to worry about modifying the original shipment
                    ShipmentEntity originalShipment = EntityUtility.CloneEntity(selectedShipment);
                    selectedShipment.ShipmentType = (int) shipmentTypeCode;

                    ShippingManager.EnsureShipmentLoaded(selectedShipment);

                    if (selectedShipment.Ups.Fields.State != EntityState.New)
                    {
                        // Grab the original ups package so we can get it's UpsPackageID, as we'll need to set it on the
                        // cloned package.  There's probably a better way, so need to check with Brian.
                        UpsPackageEntity selectedUpsPackageEntity = selectedShipment.Ups.Packages[0];
                        long originalUpsPackageID = selectedUpsPackageEntity.UpsPackageID;

                        selectedShipment.Ups.Packages.ToList().ForEach(x =>
                        {
                            // Remove it from the list
                            selectedShipment.Ups.Packages.Remove(x);

                            // If its saved in the database, we have to delete it
                            if (!x.IsNew)
                            {
                                using (SqlAdapter adapter = new SqlAdapter())
                                {
                                    adapter.DeleteEntity(x);
                                }
                            }
                        });

                        shipmentType.ConfigureNewShipment(selectedShipment);

                        // Update the first package UpsPackgeID to be that of the original persisted package.  If this isn't 
                        // done, we get an ORM exception.  There's probably a better way, so need to check with Brian.
                        selectedShipment.Ups.Packages[0].UpsPackageID = originalUpsPackageID;
                    }

                    UpdateShipmentSettings(selectedShipment, originalShipment, account);
                    selectedShipment.Ups.Service = (int)originalTag;
                };
        }

        /// <summary>
        /// Gets a list of rates by UpsServiceType
        /// </summary>
        /// <param name="upsTypeGroup">Group </param>
        /// <returns></returns>
        private static IEnumerable<RateResult> RateResultsByServiceLevel(IGrouping<UpsServiceType, RateResult> upsTypeGroup)
        {
            return upsTypeGroup
                .GroupBy(r => r.ServiceLevel)
                .Select(serviceLevelRate => serviceLevelRate.OrderBy(rateToOrder => rateToOrder.Amount)
                    .FirstOrDefault());
        }


        /// <summary>
        /// Updates data on the Ups shipment that is required for checking best rate
        /// </summary>
        /// <param name="testRateShipment">Shipment that we'll be working with</param>
        /// <param name="originalShipment">The original shipment from which data can be copied.</param>
        /// <param name="upsAccount">The UPS Account Entity for this shipment.</param>
        private static void UpdateShipmentSettings(ShipmentEntity testRateShipment, ShipmentEntity originalShipment, UpsAccountEntity upsAccount)
        {
            testRateShipment.OriginOriginID = originalShipment.OriginOriginID;

            // Set the address of the shipment to either the UPS account, or the address of the original shipment
            if (testRateShipment.OriginOriginID == (int)ShipmentOriginSource.Account)
            {
                PersonAdapter.Copy(upsAccount, "", testRateShipment, "Origin");
            }
            else
            {
                PersonAdapter.Copy(originalShipment, testRateShipment, "Origin");
            }

            testRateShipment.Ups.Packages[0].DimsHeight = testRateShipment.BestRate.DimsHeight;
            testRateShipment.Ups.Packages[0].DimsWidth = testRateShipment.BestRate.DimsWidth;
            testRateShipment.Ups.Packages[0].DimsLength = testRateShipment.BestRate.DimsLength;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            testRateShipment.Ups.Packages[0].Weight = originalShipment.ContentWeight;
            testRateShipment.Ups.Packages[0].DimsAddWeight = false;
            testRateShipment.Ups.Packages[0].PackagingType = (int)UpsPackagingType.Custom;
            testRateShipment.Ups.Service = (int)UpsServiceType.UpsGround;
            testRateShipment.Ups.UpsAccountID = upsAccount.UpsAccountID;
        }
    }
}
