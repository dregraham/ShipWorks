using System;
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
        /// <returns>A list of RateResults composed of the single best rate for each UPS account.</returns>
        public List<RateResult> GetBestRates(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            List<RateResult> allRates = new List<RateResult>();
            
            List<UpsAccountEntity> upsAccounts = new List<UpsAccountEntity>();
            upsAccounts.Add(accountRepository.Accounts.ToList().First());

            Dictionary<RateResult, UpsShipmentEntity> rateShipments = new Dictionary<RateResult, UpsShipmentEntity>();

            // Create a clone so we don't have to worry about modifying the original shipment
            ShipmentEntity testRateShipment = EntityUtility.CloneEntity(shipment);
            testRateShipment.ShipmentType = (int)ShipmentTypeCode.UpsOnLineTools;

            foreach (UpsAccountEntity account in upsAccounts)
            {
                testRateShipment.Ups = new UpsShipmentEntity();

                shipmentType.ConfigureNewShipment(testRateShipment);
                UpdateUpsShipmentSettings(testRateShipment, shipment.ContentWeight, account.UpsAccountID);

                try
                {
                    IEnumerable<RateResult> results = shipmentType.GetRates(testRateShipment).Rates.Where(r => r.Tag != null);

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

            // Return all the rates, filtered by service level, then grouped by UpsServiceType and ServiceLevel
            List<RateResult> filteredRates = allRates
                .Where(r => r.Amount > 0 && MeetsServiceLevelCriteria(r.ServiceLevel, (ServiceLevelType)shipment.BestRate.ServiceLevel))
                .GroupBy(r => (UpsServiceType)r.Tag)
                .SelectMany(RateResultsByServiceLevel)
                .ToList();

            foreach (RateResult rate in filteredRates)
            {
                // Replace the service type with a function that will select the correct shipment type
                rate.Tag = CreateRateSelectionFunction(rateShipments[rate], rate.Tag);
                rate.Description = rate.Description.Contains("UPS") ? rate.Description : "UPS " + rate.Description;
            }

            return filteredRates.Select(x => new NoncompetitiveRateResult(x)).ToList<RateResult>();
        }

        /// <summary>
        /// Creates a function that can be used to select a specific rate
        /// </summary>
        /// <param name="rateShipment">UpsShipment that was used to get the rate</param>
        /// <param name="originalTag">UpsServiceType associated with the specific rate</param>
        /// <returns>A function that, when executed, will convert the passed in shipment to a UPS shipment
        /// used to create the rate.</returns>
        private static Action<ShipmentEntity> CreateRateSelectionFunction(UpsShipmentEntity rateShipment, object originalTag)
        {
            return selectedShipment =>
                {
                    rateShipment.Service = (int)originalTag;
                    selectedShipment.ShipmentType = (int)ShipmentTypeCode.UpsOnLineTools;
                    ShippingManager.EnsureShipmentLoaded(selectedShipment);

                    if (selectedShipment.Ups == null)
                    {
                        selectedShipment.Ups = rateShipment;
                    }
                    else
                    {
                        // Grab the original ups package so we can get it's UpsPackageID, as we'll need to set it on the
                        // cloned package.  There's probably a better way, so need to check with Brian.
                        UpsPackageEntity selectedUpsPackageEntity = selectedShipment.Ups.Packages[0];
                        long originalUpsPackageID = selectedUpsPackageEntity.UpsPackageID;

                        // Set the rated shipment as the UPS shipment
                        selectedShipment.Ups = rateShipment;

                        // Update the first package UpsPackgeID to be that of the original persisted package.  If this isn't 
                        // done, we get an ORM exception.  There's probably a better way, so need to check with Brian.
                        selectedShipment.Ups.Packages[0].UpsPackageID = originalUpsPackageID;

                        // Set the ups shipment and package to be not new so a copy isn't persisted.
                        selectedShipment.Ups.Packages[0].IsNew = false;
                        selectedShipment.Ups.IsNew = false;
                    }
                };
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
